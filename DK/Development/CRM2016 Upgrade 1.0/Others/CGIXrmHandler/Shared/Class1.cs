﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;

namespace CGIXrmHandler
{
    public static class QuotedPrintableDecoder
    {
        public static string Parse(string input)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder currentWord = new StringBuilder();
            StringBuilder currentSurroundingText = new StringBuilder();
            bool readingWord = false;
            bool hasSeenAtLeastOneWord = false;

            int wordQuestionMarkCount = 0;
            int i = 0;
            while (i < input.Length)
            {
                char currentChar = input[i];
                char peekAhead;
                switch (currentChar)
                {
                    case '=':
                        peekAhead = (i == input.Length - 1) ? ' ' : input[i + 1];

                        if (!readingWord && peekAhead == '?')
                        {
                            if (!hasSeenAtLeastOneWord
                                || (hasSeenAtLeastOneWord && currentSurroundingText.ToString().Trim().Length > 0))
                            {
                                sb.Append(currentSurroundingText.ToString());
                            }

                            currentSurroundingText = new StringBuilder();
                            hasSeenAtLeastOneWord = true;
                            readingWord = true;
                            wordQuestionMarkCount = 0;
                        }
                        break;

                    case '?':
                        if (readingWord)
                        {
                            wordQuestionMarkCount++;

                            peekAhead = (i == input.Length - 1) ? ' ' : input[i + 1];

                            if (wordQuestionMarkCount > 3 && peekAhead == '=')
                            {
                                readingWord = false;

                                currentWord.Append(currentChar);
                                currentWord.Append(peekAhead);

                                sb.Append(ParseEncodedWord(currentWord.ToString()));
                                currentWord = new StringBuilder();

                                i += 2;
                                continue;
                            }
                        }
                        break;
                }

                if (readingWord)
                {
                    currentWord.Append(('_' == currentChar) ? ' ' : currentChar);
                    i++;
                }
                else
                {
                    currentSurroundingText.Append(currentChar);
                    i++;
                }
            }

            sb.Append(currentSurroundingText.ToString());

            return sb.ToString();
        }

        private static string ParseEncodedWord(string input)
        {
            StringBuilder sb = new StringBuilder();

            if (!input.StartsWith("=?"))
                return input;

            if (!input.EndsWith("?="))
                return input;

            // Get the name of the encoding but skip the leading =?
            string encodingName = input.Substring(2, input.IndexOf("?", 2) - 2);
            Encoding enc = ASCIIEncoding.ASCII;
            if (!string.IsNullOrEmpty(encodingName))
            {
                enc = Encoding.GetEncoding(encodingName);
            }

            // Get the type of the encoding
            char type = input[encodingName.Length + 3];

            // Start after the name of the encoding and the other required parts
            int startPosition = encodingName.Length + 5;

            switch (char.ToLowerInvariant(type))
            {
                case 'q':
                    sb.Append(ParseQuotedPrintable(enc, input, startPosition, true));
                    break;
                case 'b':
                    string baseString = input.Substring(startPosition, input.Length - startPosition - 2);
                    byte[] baseDecoded = Convert.FromBase64String(baseString);
                    var intermediate = enc.GetString(baseDecoded);
                    sb.Append(intermediate);
                    break;
            }
            return sb.ToString();
        }

        public static string ParseQuotedPrintable(Encoding enc, string input)
        {
            return ParseQuotedPrintable(enc, input, 0, false);
        }
        public static string ParseQuotedPrintable(Encoding enc, string input, int startPos, bool skipQuestionEquals)
        {
            byte[] workingBytes = ASCIIEncoding.ASCII.GetBytes(input);

            int i = startPos;
            int outputPos = i;

            while (i < workingBytes.Length)
            {
                byte currentByte = workingBytes[i];
                char[] peekAhead = new char[2];
                switch (currentByte)
                {
                    case (byte)'=':
                        bool canPeekAhead = (i < workingBytes.Length - 2);

                        if (!canPeekAhead)
                        {
                            workingBytes[outputPos] = workingBytes[i];
                            ++outputPos;
                            ++i;
                            break;
                        }

                        int skipNewLineCount = 0;
                        for (int j = 0; j < 2; ++j)
                        {
                            char c = (char)workingBytes[i + j + 1];
                            if ('\r' == c || '\n' == c)
                            {
                                ++skipNewLineCount;
                            }
                        }

                        if (skipNewLineCount > 0)
                        {
                            // If we have a lone equals followed by newline chars, then this is an artificial
                            // line break that should be skipped past.
                            i += 1 + skipNewLineCount;
                        }
                        else
                        {
                            try
                            {
                                peekAhead[0] = (char)workingBytes[i + 1];
                                peekAhead[1] = (char)workingBytes[i + 2];

                                byte decodedByte = Convert.ToByte(new string(peekAhead, 0, 2), 16);
                                workingBytes[outputPos] = decodedByte;

                                ++outputPos;
                                i += 3;
                            }
                            catch (Exception)
                            {
                                // could not parse the peek-ahead chars as a hex number... so gobble the un-encoded '='
                                i += 1;
                            }
                        }
                        break;

                    case (byte)'?':
                        if (skipQuestionEquals && workingBytes[i + 1] == (byte)'=')
                        {
                            i += 2;
                        }
                        else
                        {
                            workingBytes[outputPos] = workingBytes[i];
                            ++outputPos;
                            ++i;
                        }
                        break;

                    default:
                        workingBytes[outputPos] = workingBytes[i];
                        ++outputPos;
                        ++i;
                        break;
                }
            }

            string output = string.Empty;

            int numBytes = outputPos - startPos;
            if (numBytes > 0)
            {
                output = enc.GetString(workingBytes, startPos, numBytes);
            }

            return output;
        }
        
    }
}