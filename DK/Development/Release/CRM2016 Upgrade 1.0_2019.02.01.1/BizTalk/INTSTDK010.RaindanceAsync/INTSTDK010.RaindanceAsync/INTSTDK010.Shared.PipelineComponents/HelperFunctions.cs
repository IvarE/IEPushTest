using Microsoft.BizTalk.Streaming;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace INTSTDK010.Shared.PipelineComponents
{
    public static class HelperFunctions
    {
        public static Stream GetMessageStream(Microsoft.BizTalk.Message.Interop.IBaseMessage msg, Microsoft.BizTalk.Component.Interop.IPipelineContext context)
        {
            Stream stream = msg.BodyPart.GetOriginalDataStream();

            if (!stream.CanSeek)
            {
                ReadOnlySeekableStream readStream = new ReadOnlySeekableStream(stream);

                if (context != null)
                {
                    context.ResourceTracker.AddResource(readStream);
                }

                msg.BodyPart.Data = readStream;
                stream = readStream;
            }
            return stream;
        }

        public static string ExtractDataValueXPath(Stream MsgStream, string MsgXPath)
        {
            XmlReaderSettings settings = new XmlReaderSettings()
            {
                ConformanceLevel = ConformanceLevel.Document,
                IgnoreWhitespace = true,
                ValidationType = ValidationType.None,
                IgnoreProcessingInstructions = true,
                IgnoreComments = true,
                CloseInput = false
            };

            MsgStream.Seek(0, SeekOrigin.Begin);
            XmlReader reader = XmlReader.Create(MsgStream, settings);

            string strValue = null;

            if (!string.IsNullOrEmpty(MsgXPath))
            {
                if (reader.Read())
                {
                    XPathDocument xPathDoc = new XPathDocument(reader);
                    XPathNavigator xNavigator = xPathDoc.CreateNavigator();
                    XPathNodeIterator xNodes = xNavigator.Select(MsgXPath);

                    if (xNodes.Count != 0 && xNodes.MoveNext())
                    {
                        strValue = xNodes.Current.Value;
                    }
                    MsgStream.Seek(0, SeekOrigin.Begin);
                }
            }
            return strValue;
        }
    }
}
