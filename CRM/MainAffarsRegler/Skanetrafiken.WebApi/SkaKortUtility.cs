using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skanetrafiken.Crm;
using Microsoft.Xrm.Sdk.Query;
using Endeavor.Crm;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.Serialization.Json;
using Skanetrafiken.Crm.Entities;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Microsoft.Xrm.Sdk;
using Skanetrafiken.Crm.Properties;

namespace Skanetrafiken.Crm
{
    public class SkaKortUtility
    {

        public enum StatusBlockCode
        {
            GenericError = 0,
            InvalidInput = 1,
            OtherLeadFound = 2,
            OtherAccountFound = 3,
            NoConflictingEntity = 4,
            DataValid = 5,
            NoAccountFound = 6
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="customerInfo"></param>
        /// <returns></returns>
        public static StatusBlock ValidateSkaKortInfo(Plugin.LocalPluginContext localContext, SkaKortInfo skaKortInfo, bool isPostMessage)
        {
            if (skaKortInfo == null)
            {
                return new StatusBlock
                {
                    TransactionOk = false,
                    StatusBlockCode = (int)StatusBlockCode.InvalidInput,
                    ErrorMessage = Properties.Resources.IncomingDataCannotBeNull
                };
            }

            List<string> messages = new List<string>();
            bool error = false;

            if (isPostMessage == true) //This validation is done for Create logic. Do we need Portal Id/Contact Id
            {
                switch (skaKortInfo.InformationSource)
                {
                    case (int)Schema.Generated.ed_informationsource.ForetagsPortal:
                        if (string.IsNullOrWhiteSpace(skaKortInfo.PortalId) ||
                            string.IsNullOrWhiteSpace(skaKortInfo.CardNumber) ||
                            string.IsNullOrWhiteSpace(skaKortInfo.CardName)
                            )
                        {
                            error = true;
                            messages.Add(ReturnMissingFields(localContext, skaKortInfo).ErrorMessage);
                        }
                        break;
                    case (int)Schema.Generated.ed_informationsource.KopOchSkicka:
                        if (string.IsNullOrWhiteSpace(skaKortInfo.ContactId) ||
                            string.IsNullOrWhiteSpace(skaKortInfo.CardNumber) ||
                            string.IsNullOrWhiteSpace(skaKortInfo.CardName)
                            )
                        {
                            error = true;
                            messages.Add(ReturnMissingFields(localContext, skaKortInfo).ErrorMessage);
                        }
                        break;
                    default:
                        return new StatusBlock()
                        {
                            TransactionOk = false,
                            StatusBlockCode = (int)StatusBlockCode.InvalidInput,
                            ErrorMessage = string.Format(Resources.InvalidSource, skaKortInfo.InformationSource)
                        };
                }
            }
            else //This validation is done for none Create logic
            {
                if (string.IsNullOrWhiteSpace(skaKortInfo.CardNumber))
                {
                    error = true;
                    messages.Add(ReturnMissingFields(localContext, skaKortInfo).ErrorMessage);
                }
            }

            if (error)
            {
                return QuickFaultyStatus(StatusBlockCode.InvalidInput, FormatErrorMessages(messages));
            }

            return new StatusBlock()
            {
                TransactionOk = true,
                StatusBlockCode = (int)StatusBlockCode.DataValid
            };
        }

        private static StatusBlock ReturnMissingFields(Plugin.LocalPluginContext localContext, SkaKortInfo skaKortInfo)
        {
            if (skaKortInfo == null)
            {
                return new StatusBlock
                {
                    TransactionOk = false,
                    StatusBlockCode = (int)CustomerUtility.StatusBlockCode.InvalidInput,
                    ErrorMessage = Resources.IncomingDataCannotBeNull
                };
            }
            List<string> missingFields = new List<string>();

            switch (skaKortInfo.InformationSource)
            {
                case (int)Crm.Schema.Generated.ed_informationsource.ForetagsPortal:
                    if (string.IsNullOrWhiteSpace(skaKortInfo.PortalId))
                        missingFields.Add("PortalId");
                    if (string.IsNullOrWhiteSpace(skaKortInfo.CardNumber))
                        missingFields.Add("CardNumber");
                    if (string.IsNullOrWhiteSpace(skaKortInfo.CardName))
                        missingFields.Add("CardName");
                    break;
                case (int)Crm.Schema.Generated.ed_informationsource.KopOchSkicka:
                    if (string.IsNullOrWhiteSpace(skaKortInfo.ContactId))
                        missingFields.Add("ContactId");
                    if (string.IsNullOrWhiteSpace(skaKortInfo.CardNumber))
                        missingFields.Add("CardNumber");
                    if (string.IsNullOrWhiteSpace(skaKortInfo.CardName))
                        missingFields.Add("CardName");
                    break;
                default:
                    return new StatusBlock
                    {
                        TransactionOk = false,
                        StatusBlockCode = (int)CustomerUtility.StatusBlockCode.InvalidInput,
                        ErrorMessage = string.Format(Resources.InvalidSource, skaKortInfo.InformationSource)
                    };
            }
            if (missingFields.Count == 0)
            {
                return new StatusBlock
                {
                    TransactionOk = true,
                    StatusBlockCode = (int)CustomerUtility.StatusBlockCode.DataValid,
                    ErrorMessage = Resources.NoMissingFields
                };
            }

            StringBuilder sb = new StringBuilder(Resources.MissingFields);
            foreach (string s in missingFields)
            {
                sb.Append(string.Format("<BR>{0}, ", s));
            }
            sb.Remove(sb.Length - 2, 2);

            return new StatusBlock
            {
                TransactionOk = false,
                StatusBlockCode = (int)CustomerUtility.StatusBlockCode.InvalidInput,
                ErrorMessage = sb.ToString()
            };
        }
        
        public static StatusBlock QuickFaultyStatus(StatusBlockCode code, string message)
        {
            return new StatusBlock()
            {
                TransactionOk = false,
                StatusBlockCode = (int)code,
                ErrorMessage = message
            };
        }

        private static string FormatErrorMessages(List<string> messages)
        {
            if (messages == null || messages.Count == 0)
                return null;
            StringBuilder sb = new StringBuilder();
            foreach (string s in messages)
            {
                sb.AppendLine(s);
                sb.Append("<BR>");
            }
            return sb.ToString();
        }
    }
}
