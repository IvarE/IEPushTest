﻿using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Endeavor.Crm.Extensions;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.Sdk.Messages;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Endeavor.Crm;
using System.Diagnostics;
using System.Net;
//using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.IO;
using Skanetrafiken.Crm.TextMessageSender;
using System.Text.RegularExpressions;
using System.Workflow.ComponentModel.Compiler;
using System.Web;
using System.Runtime.Serialization.Json;
using System.Drawing;

namespace Skanetrafiken.Crm.Entities
{
    public class ValueCodeResponseInfo
    {
        public long eanCode;
        public DateTime lastRedemptionDate;
        public bool lastRedemptionDateSpecified;
        public string uniqueCode;
        public int typeOfValueCode;
    }

    public class ValueCodeInputInfo
    {
        public decimal amount;
        public int campaignNumber;
        public DateTime lastRedemptionDate;
        public int typeOfValueCode;
    }

    abstract class ValueCodeSender
    {
        protected ActivityPartyEntity[] receiver, sender;

        public abstract void SendValueCode(Plugin.LocalPluginContext localContext, EntityReference regarding);

        public ValueCodeSender(Plugin.LocalPluginContext localContext, ValueCodeEntity valueCode, ValueCodeTemplateEntity template)
        {
            this.receiver = GetReceiver(localContext, valueCode);
            this.sender = GetSender(localContext);
        }

        private ActivityPartyEntity[] GetSender(Plugin.LocalPluginContext localContext)
        {
            WhoAmIRequest systemUserRequest = new WhoAmIRequest();
            WhoAmIResponse systemUserResponse = (WhoAmIResponse)localContext.OrganizationService.Execute(systemUserRequest);
            Guid userId = systemUserResponse.UserId;

            if (userId == null || userId == Guid.Empty)
                throw new InvalidPluginExecutionException("UserId is null");

            EntityReference defaultSender = CgiSettingEntity.GetSettingEntRef(localContext, CgiSettingEntity.Fields.ed_DefaultSenderValueCodes);

            if (defaultSender != null && defaultSender.Id != null)
            {
                return new ActivityPartyEntity[]
                {
                    new ActivityPartyEntity()
                    {
                        PartyId = defaultSender
                    }
                };
            }
            else
            {
                return new ActivityPartyEntity[]
                {
                    new ActivityPartyEntity()
                    {
                        PartyId = new EntityReference("queue", userId)
                    }
                };
            }
        }

        private ActivityPartyEntity[] GetReceiver(Plugin.LocalPluginContext localContext, ValueCodeEntity valueCode)
        {
            EntityReference ReceiverId;

            if (valueCode.ed_Contact != null && valueCode.ed_Contact.Id != null && !string.IsNullOrEmpty(valueCode.ed_Email))
            {
                ContactEntity receiver = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, valueCode.ed_Contact.Id, new ColumnSet(
                    ContactEntity.Fields.EMailAddress1,
                    ContactEntity.Fields.EMailAddress2));

                if (!string.IsNullOrEmpty(receiver.EMailAddress1) && receiver.EMailAddress1 == valueCode.ed_Email)
                {
                    ReceiverId = new EntityReference(ContactEntity.EntityLogicalName, valueCode.ed_Contact.Id);
                    return new ActivityPartyEntity[]
                    {
                        new ActivityPartyEntity()
                        {
                            PartyId = ReceiverId
                        }
                    };
                }
                else if (!string.IsNullOrEmpty(receiver.EMailAddress2) && receiver.EMailAddress2 == valueCode.ed_Email)
                {
                    ReceiverId = new EntityReference(ContactEntity.EntityLogicalName, valueCode.ed_Contact.Id);
                    return new ActivityPartyEntity[]
                    {
                        new ActivityPartyEntity()
                        {
                            PartyId = ReceiverId
                        }
                    };
                }
                else
                {
                    return new ActivityPartyEntity[]
                    {
                        new ActivityPartyEntity()
                        {
                            AddressUsed = valueCode.ed_Email
                        }
                    };
                }
            }


            if (valueCode.ed_Contact != null && string.IsNullOrEmpty(valueCode.ed_Email))
            {
                ReceiverId = new EntityReference(ContactEntity.EntityLogicalName, valueCode.ed_Contact.Id);
                return new ActivityPartyEntity[]
                {
                    new ActivityPartyEntity()
                    {
                        PartyId = ReceiverId
                    }
                };
            }
            else if (valueCode.ed_Lead != null)
            {
                ReceiverId = new EntityReference(LeadEntity.EntityLogicalName, valueCode.ed_Lead.Id);
                return new ActivityPartyEntity[]
                {
                    new ActivityPartyEntity()
                    {
                        PartyId = ReceiverId
                    }
                };
            }
            else
            {
                throw new InvalidPluginExecutionException("Värdekod saknar Kontakt eller Lead mottagare");
            }

        }
    }

    class EmailValueCodeSender : ValueCodeSender
    {
        TemplateEntity emailTemplate;
        //string exisitingEmailAddress;

        public EmailValueCodeSender(Plugin.LocalPluginContext localContext, ValueCodeEntity valueCode, ValueCodeTemplateEntity template) : base(localContext, valueCode, template)
        {
            emailTemplate = template.GetEmailTemplate(localContext);

            if (valueCode.ed_Contact != null)
            {
                ContactEntity contact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, valueCode.ed_Contact, new ColumnSet(ContactEntity.Fields.EMailAddress1));
                // JOHAN, 190711. Smäller om e-post saknas på värdekod!!!
                //if (!valueCode.ed_Email.Equals(contact.EMailAddress1))
                //{
                //    exisitingEmailAddress = contact.EMailAddress1;
                //    contact.EMailAddress1 = valueCode.ed_Email;
                //    //XrmHelper.Update(localContext, contact);
                //}
            }

            if (valueCode.ed_Lead != null)
            {
                LeadEntity lead = XrmRetrieveHelper.Retrieve<LeadEntity>(localContext, valueCode.ed_Lead, new ColumnSet(LeadEntity.Fields.EMailAddress1));
                if (!valueCode.ed_Email.Equals(lead.EMailAddress1))
                {
                    //exisitingEmailAddress = lead.EMailAddress1;
                    //lead.EMailAddress1 = valueCode.ed_Email;
                    //XrmHelper.Update(localContext, lead);
                }
            }
        }

        public override void SendValueCode(Plugin.LocalPluginContext localContext, EntityReference regarding)
        {
            ValueCodeEntity valueCode = XrmRetrieveHelper.Retrieve<ValueCodeEntity>(localContext, regarding, new ColumnSet(true));

            EmailEntity email = emailTemplate.InstantiateTemplate(localContext, regarding);
            email.From = sender;
            email.RegardingObjectId = regarding;
            email.To = receiver;


            XrmHelper.CreateAndSendEmail(localContext, email, IssueSend: true);

            // Only update the fields we are changing!
            ValueCodeEntity updValueCode = new ValueCodeEntity()
            {
                statuscode = Generated.ed_valuecode_statuscode.Skickad,
                Id = valueCode.Id
            };

            XrmHelper.Update(localContext, updValueCode);
        }
    }

    class SMSValueCodeSender : ValueCodeSender
    {
        string phoneNumber, subject, description;

        public SMSValueCodeSender(Plugin.LocalPluginContext localContext, ValueCodeEntity valueCode, ValueCodeTemplateEntity template) : base(localContext, valueCode, template)
        {
            this.description = GenerateDescription(localContext, template, valueCode);
            this.phoneNumber = valueCode.ed_MobileNumber;
        }

        public override void SendValueCode(Plugin.LocalPluginContext localContext, EntityReference regarding)
        {
            TextMessageEntity textMessage = new TextMessageEntity
            {
                RegardingObjectId = regarding,
                Description = this.description,
                To = this.receiver,
                From = this.sender,
                Subject = this.subject,
                ed_PhoneNumber = this.phoneNumber,
                ed_SenderName = "Skanetrafik"
            };

            Guid smsId = XrmHelper.Create(localContext.OrganizationService, textMessage);
            textMessage = XrmRetrieveHelper.Retrieve<TextMessageEntity>(localContext, smsId, new ColumnSet(true));

            textMessage.SendTextMessage(localContext);
        }

        public string GenerateDescription(Plugin.LocalPluginContext localContext, ValueCodeTemplateEntity template, ValueCodeEntity valueCode)
        {
            if (template == null)
            {
                return String.Empty;
            }

            string text;
            if (localContext.PluginExecutionContext == null)
            {
                text = ReplaceWithEntityData(localContext, template.ed_ValueCodeText, valueCode, valueCode.LogicalName, null);
            }
            else
            {
                text = ReplaceWithEntityData(localContext, template.ed_ValueCodeText, valueCode, valueCode.LogicalName, localContext.PluginExecutionContext.UserId);
            }

            return text;
        }

        private static string ReplaceWithEntityData(Plugin.LocalPluginContext localContext, string text, Entity entity, string prefix, Guid? systemUserId)
        {

            foreach (KeyValuePair<string, object> attribute in entity.Attributes)
            {
                if (text.Contains("{!" + prefix + ":" + attribute.Key + ";}"))
                {

                    string value = "";
                    switch (attribute.Value.GetType().ToString())
                    {
                        case "Microsoft.Xrm.Sdk.Money":
                            Money money = (Money)attribute.Value;
                            int integer_amount = (int)money.Value;
                            value = integer_amount.ToString();
                            break;
                        case "Microsoft.Xrm.Sdk.OptionSetValue":
                            value = ((OptionSetValue)attribute.Value).Value.ToString();
                            break;
                        case "Microsoft.Xrm.Sdk.EntityReference":
                            value = ((EntityReference)attribute.Value).Name.ToString();
                            break;
                        case "System.DateTime":
                            // TODO: Add functionality of datetime
                            value = ((DateTime)attribute.Value).ToLocalTime().Date.ToShortDateString();
                            //if (systemUserId.HasValue)
                            //{
                            //    value = GetFormattedDate(localContext, (DateTime)attribute.Value, systemUserId.Value);
                            //}
                            break;
                        case "Microsoft.Xrm.Sdk.EntityCollection":
                            EntityCollection entityCollection = (EntityCollection)attribute.Value;
                            if (entityCollection.Entities.Count > 0)
                            {
                                EntityReference entityReference = (EntityReference)entityCollection.Entities[0]["partyid"];
                                value = entityReference.Name;
                            }
                            break;
                        case "System.Decimal":
                            var d_money = (decimal)attribute.Value;
                            value = d_money.ToString("F");
                            break;
                        case "System.Float":
                            var f_money = (float)attribute.Value;
                            value = f_money.ToString("F");
                            break;
                        default:
                            value = attribute.Value.ToString();
                            break;
                    }

                    //text = text + " " + attribute.Value.GetType().ToString();

                    text = text.Replace("{!" + prefix + ":" + attribute.Key + ";}", value);
                }

            }

            text = Regex.Replace(text, @"\{!" + prefix + @":[^}]+\}", "");

            return text;
        }

        private static string GetFormattedDate(Plugin.LocalPluginContext localContext, DateTime date, Guid systemUserId)
        {
            string formattedDate = date.ToString();

            // Get user settings
            FilterExpression filter = new FilterExpression(LogicalOperator.And);
            filter.AddCondition(UserSettingsEntity.Fields.SystemUserId, ConditionOperator.Equal, systemUserId);
            ColumnSet columnSet = new ColumnSet(UserSettingsEntity.Fields.DateFormatString, UserSettingsEntity.Fields.DateSeparator, UserSettingsEntity.Fields.TimeFormatString, UserSettingsEntity.Fields.TimeZoneCode);
            Entity userSettings = XrmHelper.RetrieveFirst(localContext, UserSettingsEntity.EntityLogicalName, columnSet, filter);

            // Set user timezone
            if (userSettings != null && userSettings.Contains(UserSettingsEntity.Fields.TimeZoneCode))
            {
                LocalTimeFromUtcTimeRequest request = new LocalTimeFromUtcTimeRequest
                {
                    TimeZoneCode = (Int32)userSettings[UserSettingsEntity.Fields.TimeZoneCode],
                    UtcTime = date.ToUniversalTime()
                };
                LocalTimeFromUtcTimeResponse response = (LocalTimeFromUtcTimeResponse)localContext.OrganizationService.Execute(request);
                date = response.LocalTime;
            }

            // String format string
            if (userSettings != null && userSettings.Contains("dateformatstring") && userSettings.Contains("dateseparator") && userSettings.Contains("timeformatstring"))
            {
                string dateFormat = userSettings["dateformatstring"].ToString().Replace("/", userSettings["dateseparator"].ToString()) + " " + userSettings["timeformatstring"].ToString();
                formattedDate = date.ToString(dateFormat);
            }

            return formattedDate;
        }


    }

    public class ValueCodeEntity : Generated.ed_ValueCode
    {
        public enum Status
        {
            Skapad = 899310002,
            Skickad = 899310003,
            Forfallen = 899310000,
            Inlost = 899310001,
            Makulerad = 899310004
        }

        public void SendValueCode(Plugin.LocalPluginContext localContext)
        {
            if (this.Attributes.Contains(ValueCodeEntity.Fields.ed_ValueCodeTemplate))
            {
                ValueCodeSender valueCodeSender;

                ValueCodeTemplateEntity template = XrmRetrieveHelper.Retrieve<ValueCodeTemplateEntity>(localContext, this.ed_ValueCodeTemplate.Id, new ColumnSet(true));
                //ed_ValueCodeTypeGlobal -> ed_ValueCodeDeliveryTypeGlobal

                if ((int)ed_ValueCodeDeliveryTypeGlobal.Value == (int)Generated.ed_valuecodedeliverytypeglobal.Email)
                {
                    valueCodeSender = new EmailValueCodeSender(localContext, this, template);
                }
                else if ((int)ed_ValueCodeDeliveryTypeGlobal.Value == (int)Generated.ed_valuecodedeliverytypeglobal.SMS)
                {
                    valueCodeSender = new SMSValueCodeSender(localContext, this, template);
                }
                else
                {
                    throw new InvalidPluginExecutionException("Värdekod saknar information om hur värdekod ska skickas till mottagaren.");
                }

                valueCodeSender.SendValueCode(localContext, this.ToEntityReference());

            }
            else
            {
                throw new InvalidPluginExecutionException("Mall för värdekod saknas");
            }
        }

        public static string HandleCancelValueCode(Plugin.LocalPluginContext localContext, string valueCodeId, Guid? userGuid)
        {
            if (!string.IsNullOrWhiteSpace(valueCodeId))
            {
                string[] valueCodeArray = { };

                if (valueCodeId.Contains(";") == true)
                    valueCodeArray = valueCodeId.Split(';').Select(sValue => sValue.Trim()).ToArray();

                //Get settings entity
                FilterExpression settingFilter = new FilterExpression(LogicalOperator.And);
                settingFilter.AddCondition(CgiSettingEntity.Fields.ed_VoucherService, ConditionOperator.NotNull);
                settingFilter.AddCondition(CgiSettingEntity.Fields.ed_CRMPlusService, ConditionOperator.NotNull);
                CgiSettingEntity cgiSetting = XrmRetrieveHelper.RetrieveFirst<CgiSettingEntity>(localContext, 
                    new ColumnSet(CgiSettingEntity.Fields.ed_VoucherService, CgiSettingEntity.Fields.ed_CRMPlusService), settingFilter);

                string cancelValueCodeEndpoint = cgiSetting?.ed_VoucherService;
                string fasadEndpoint = cgiSetting.ed_CRMPlusService;

                if (cgiSetting != null && !string.IsNullOrWhiteSpace(cancelValueCodeEndpoint) && !string.IsNullOrWhiteSpace(fasadEndpoint))
                {
                    string successValueCodes = string.Empty;
                    string errorValueCodes = string.Empty;

                    int nrCanceled = 0;
                    int nrFailed = 0;

                    string responseCodes = string.Empty;
                    string returnString = string.Empty;

                    string cancelledBy = string.Empty;
                    var requestToken = string.Empty;

                    //Function that calls the new AccessToken method from Fasad [CK]
                    #region AccessToken From Fasad

                    //Create a mthod for reuse
                    var tokenHttpWebReq = (HttpWebRequest)WebRequest.Create(string.Format("{0}/api/ValueCode/GetAccessToken/voucher", fasadEndpoint));
                    tokenHttpWebReq.ContentType = "text/plain; charset=utf-8";
                    tokenHttpWebReq.Method = "GET";

                    try
                    {
                        var tokenHttpResponse = (HttpWebResponse)tokenHttpWebReq.GetResponse();
                        if (tokenHttpResponse.StatusCode != HttpStatusCode.OK)
                        {
                            throw new Exception(string.Format("400 - Could not access required token for VoucherService. Response: " + tokenHttpResponse.StatusCode.ToString()));
                        }
                        else
                        {
                            using (var streamReader = new StreamReader(tokenHttpResponse.GetResponseStream()))
                            {
                                //Read response token
                                requestToken = streamReader.ReadToEnd();
                            }

                            if (string.IsNullOrWhiteSpace(requestToken))
                            {
                                throw new Exception(string.Format("400 - Could not access required token for VoucherService. Returned token was null."));
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        return e.Message;
                    }

                    #endregion

                    if (valueCodeArray != null && valueCodeArray.Length > 0)
                    {
                        for (var i = 0; i < valueCodeArray.Length; i++)
                        {
                            if (!string.IsNullOrWhiteSpace(valueCodeArray[i]))
                            {
                                //Get Value Code
                                ValueCodeEntity valueCode = XrmRetrieveHelper.Retrieve<ValueCodeEntity>(localContext, new Guid(valueCodeArray[i]), new ColumnSet(
                                            ValueCodeEntity.Fields.Id,
                                            ValueCodeEntity.Fields.ed_CodeId,
                                            ValueCodeEntity.Fields.ed_ValueCodeVoucherId,
                                            ValueCodeEntity.Fields.ed_CanceledBy,
                                            ValueCodeEntity.Fields.ed_CanceledOn,
                                            ValueCodeEntity.Fields.statecode,
                                            ValueCodeEntity.Fields.statuscode));

                                if (valueCode != null && !string.IsNullOrWhiteSpace(valueCode.ed_ValueCodeVoucherId))
                                {
                                    //Update canceled on / Canceled By
                                    ValueCodeEntity updateValueCode = new ValueCodeEntity();
                                    updateValueCode.Id = valueCode.Id;
                                    updateValueCode.ed_CanceledOn = (DateTime?)DateTime.Now;

                                    if (userGuid != null)
                                    {
                                        updateValueCode.ed_CanceledBy = new EntityReference(SystemUserEntity.EntityLogicalName, (Guid)userGuid);
                                        SystemUserEntity systemUser = XrmRetrieveHelper.Retrieve<SystemUserEntity>(localContext, (Guid)userGuid, new ColumnSet(SystemUserEntity.Fields.FullName));
                                        if (systemUser != null && !string.IsNullOrWhiteSpace(systemUser.FullName)) 
                                        {
                                            cancelledBy = systemUser.FullName;
                                        }
                                    }

                                    XrmHelper.Update(localContext, updateValueCode);

                                    //Trigger call to VoucherService
                                    //Using the admin endpoint that uses a JSON Object in the body
                                    //var httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/{1}", cancelValueCodeEndpoint, valueCode.ed_ValueCodeVoucherId));
                                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}", cancelValueCodeEndpoint));
                                    httpWebRequest.ContentType = "application/json";
                                    httpWebRequest.Method = "DELETE";
                                    httpWebRequest.Headers.Add("channel", "crm");

                                    //Changed with VoucherService 2.0
                                    //ApiHelper.CreateTokenForVoucherService(localContext, httpWebRequest);
                                    httpWebRequest.Headers.Add("Authorization", "Bearer " + requestToken);

                                    //Create stream so that you can send a JSON object as body of content so that you include CancelledBy and VoucherId
                                    Skanetrafiken.Crm.ValueCodes.ValueCodeHandler.ValueCodeCancelRequest cancelValueCodeRequestObj = new ValueCodes.ValueCodeHandler.ValueCodeCancelRequest();
                                    cancelValueCodeRequestObj.voucherId = valueCode.ed_ValueCodeVoucherId; //valueCode.Id.ToString();
                                    cancelValueCodeRequestObj.cancelledBy = cancelledBy;
                                    DataContractJsonSerializer js = null;
                                    MemoryStream msObj = new MemoryStream();
                                    js = new DataContractJsonSerializer(typeof(Skanetrafiken.Crm.ValueCodes.ValueCodeHandler.ValueCodeCancelRequest));
                                    js.WriteObject(msObj, cancelValueCodeRequestObj);

                                    msObj.Position = 0;
                                    StreamReader sr = new StreamReader(msObj);
                                    var InputJSON = sr.ReadToEnd(); //Create a JSON for CancelledBy and VoucherId
                                    sr.Close();
                                    msObj.Close();
                                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                                    {
                                        streamWriter.Write(InputJSON);
                                        streamWriter.Flush();
                                        streamWriter.Close();
                                    }

                                    try
                                    {

                                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                                        responseCodes += httpResponse.StatusCode.ToString() + ";";

                                        //if (httpResponse != null && httpResponse.StatusCode == HttpStatusCode.OK)
                                        if (httpResponse != null && httpResponse.StatusCode == HttpStatusCode.NoContent)
                                        {
                                            nrCanceled++;
                                            successValueCodes += valueCodeArray[i] + ";";
                                            //return "200";
                                            //Update record
                                        }
                                        else
                                        {
                                            //Return to previous status
                                            ValueCodeEntity updateValueCodeBack = new ValueCodeEntity();
                                            updateValueCodeBack.Id = valueCode.Id;
                                            updateValueCodeBack.ed_CanceledOn = null;
                                            updateValueCodeBack.ed_CanceledBy = null;

                                            XrmHelper.Update(localContext, updateValueCodeBack);

                                            nrFailed++;
                                            errorValueCodes += "RE-" + valueCodeArray[i] + "-RE= " + responseCodes + " - Expected 204;";
                                            //return "400";
                                        }
                                    }
                                    catch (WebException we)
                                    {
                                        ValueCodeEntity updateValueCodeBack = new ValueCodeEntity();
                                        updateValueCodeBack.Id = valueCode.Id;
                                        updateValueCodeBack.ed_CanceledOn = null;
                                        updateValueCodeBack.ed_CanceledBy = null;

                                        XrmHelper.Update(localContext, updateValueCodeBack);

                                        nrFailed++;
                                        errorValueCodes += "WE-" + valueCodeArray[i] + "-WE;";
                                    }
                                    catch (Exception e)
                                    {
                                        ValueCodeEntity updateValueCodeBack = new ValueCodeEntity();
                                        updateValueCodeBack.Id = valueCode.Id;
                                        updateValueCodeBack.ed_CanceledOn = null;
                                        updateValueCodeBack.ed_CanceledBy = null;

                                        XrmHelper.Update(localContext, updateValueCodeBack);

                                        nrFailed++;
                                        errorValueCodes += "EX-" + valueCodeArray[i] + "(Exception: " + e.Message + ")-EX;";
                                    }
                                }
                                else
                                {
                                    nrFailed++;
                                    errorValueCodes += "UP-" + valueCodeArray[i] + "-UP;";
                                }
                            }
                        }

                        if (successValueCodes != "" && nrCanceled > 0)
                            returnString = $"200 = Canceled {nrCanceled} Ids. Failed: {nrFailed} ->{errorValueCodes}";
                        else
                            returnString = $"Failed to cancel Ids. ResponseCodes: {responseCodes} Failed: {nrFailed} ->{errorValueCodes}";

                        return returnString;
                    }
                    else
                    {
                        if (userGuid != null)
                        {
                            SystemUserEntity systemUser = XrmRetrieveHelper.Retrieve<SystemUserEntity>(localContext, (Guid)userGuid, new ColumnSet(SystemUserEntity.Fields.FullName));
                            if (systemUser != null && !string.IsNullOrWhiteSpace(systemUser.FullName))
                            {
                                cancelledBy = systemUser.FullName;
                            }
                        }

                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                        //Trigger call to VoucherService
                        //Using the admin endpoint that uses a JSON Object in the body
                        //var httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/{1}", cancelValueCodeEndpoint, valueCodeId));
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/", cancelValueCodeEndpoint)); 
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "DELETE";
                        httpWebRequest.Headers.Add("channel", "crm");
                        //Changed with VoucherService 2.0
                        //ApiHelper.CreateTokenForVoucherService(localContext, httpWebRequest);
                        httpWebRequest.Headers.Add("Authorization", "Bearer " + requestToken);

                        //Create stream so that you can send a JSON object as body of content so that you include CancelledBy and VoucherId
                        Skanetrafiken.Crm.ValueCodes.ValueCodeHandler.ValueCodeCancelRequest cancelValueCodeRequestObj = new ValueCodes.ValueCodeHandler.ValueCodeCancelRequest();
                        cancelValueCodeRequestObj.voucherId = valueCodeId;
                        cancelValueCodeRequestObj.cancelledBy = cancelledBy;
                        DataContractJsonSerializer js = null;
                        MemoryStream msObj = new MemoryStream();
                        js = new DataContractJsonSerializer(typeof(Skanetrafiken.Crm.ValueCodes.ValueCodeHandler.ValueCodeCancelRequest));
                        js.WriteObject(msObj, cancelValueCodeRequestObj);

                        msObj.Position = 0;
                        StreamReader sr = new StreamReader(msObj);
                        var InputJSON = sr.ReadToEnd(); //Create a JSON for CancelledBy and VoucherId
                        sr.Close();
                        msObj.Close();

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        try
                        {
                            var result = string.Empty;

                            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                            {
                                /* var */
                                result = streamReader.ReadToEnd();
                                //returnMessage += " streamReader-OK,";
                                //return result;
                            }
                            var responseCode = httpResponse.StatusCode.ToString() + ";";

                            //if (httpResponse != null && httpResponse.StatusCode == HttpStatusCode.OK)
                            if (httpResponse != null && httpResponse.StatusCode == HttpStatusCode.NoContent)
                                return "200 - ValueCode Canceled!";
                            else
                                return $"400 - ResponseError: {responseCode} - Method {httpWebRequest.Method}";
                        }
                        catch (WebException we)
                        {
                            string resultFromService = string.Empty;
                            HttpWebResponse response = (HttpWebResponse)we.Response;

                            using (var streamReader = new StreamReader(response.GetResponseStream()))
                            {
                                resultFromService = streamReader.ReadToEnd();
                                localContext.TracingService.Trace($"got http error: {response.StatusCode} Content: {resultFromService}");
                            }

                            throw new WebException($"400 - Error when trying to cancel Value Code. Ex:{we.Message}, message:{resultFromService}");
                        }
                        catch (Exception e)
                        {
                            throw new Exception("400 - Failed to Execute: " + e.Message);
                        }
                    }
                }
                else
                {
                    //return returnMessage;
                    var missingSettingsError = "400 - Did not find URI for Valuecode Cancel!";
                    return missingSettingsError;
                }
            }
            else
            {
                //Throw error handling
                //return "Error!";
                //throw new InvalidPluginExecutionException("Did not find any ValueCode!");
                var returnError = "400 - Did not find any ValueCodes to cancel!";
                return returnError;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="refundId"></param>
        /// <returns></returns>
        public static IList<ValueCodeEntity> GetAllValueCodesForARefund(Plugin.LocalPluginContext localContext, EntityReference refundId)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = ValueCodeEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(ValueCodeEntity.Fields.statecode, ValueCodeEntity.Fields.statuscode),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(ValueCodeEntity.Fields.ed_Refund, ConditionOperator.Equal, refundId.Id)
                    }
                }
            };

            return XrmRetrieveHelper.RetrieveMultiple<ValueCodeEntity>(localContext, query);
        }
    }
}
