using Endeavor.Crm;
using Microsoft.Xrm.Sdk.Query;
using Skanetrafiken.Crm.Helper;
using Skanetrafiken.Crm.Schema.Generated;

using System;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Microsoft.Xrm.Sdk;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Skanetrafiken.Crm.Entities
{
    public class MarketingListEntity : Generated.List
    {
        [DataContract]
        public class SendMarketingInfoResponse
        {
            [DataMember]
            public bool OK { get; set; }

            [DataMember]
            public string Message { get; set; }
        }

        public static SendMarketingInfoResponse SendMarketingInfo(Plugin.LocalPluginContext localContext, Guid gMarketingListId)
        {
            localContext.Trace("Inside SendMarketingInfo.");

            SendMarketingInfoResponse response = new SendMarketingInfoResponse();

            var queryCampaigns = new QueryExpression(CampaignEntity.EntityLogicalName);
            queryCampaigns.Distinct = true;
            queryCampaigns.NoLock = true;
            queryCampaigns.ColumnSet.AddColumns(CampaignEntity.Fields.CampaignId);
            var queryCampaignItem = queryCampaigns.AddLink(CampaignItemEntity.EntityLogicalName, CampaignItemEntity.Fields.CampaignId, CampaignEntity.Fields.CampaignId);
            var ab = queryCampaignItem.AddLink(MarketingListEntity.EntityLogicalName, CampaignItemEntity.Fields.EntityId, MarketingListEntity.Fields.ListId);
            ab.LinkCriteria.AddCondition(MarketingListEntity.Fields.ListId, ConditionOperator.Equal, gMarketingListId);

            List<CampaignEntity> lCampaigns = XrmRetrieveHelper.RetrieveMultiple<CampaignEntity>(localContext, queryCampaigns);

            if (lCampaigns.Count > 1)
            {
                response.OK = false;
                response.Message = "There is more than one Campaign connected to the Marketing List.";
                return response;
            }
            else if(lCampaigns.Count == 0)
            {
                response.OK = false;
                response.Message = "There is no Campaigns connected to the Marketing List.";
                return response;
            }

            CampaignEntity campaign = lCampaigns.FirstOrDefault();
            MarketingListEntity marketingList = XrmRetrieveHelper.Retrieve<MarketingListEntity>(localContext, gMarketingListId,
                new ColumnSet(MarketingListEntity.Fields.OwnerId, MarketingListEntity.Fields.CreatedOn));

            if (marketingList.OwnerId == null)
                throw new InvalidPluginExecutionException($"OwnerId of Marketing List is null.");

            if (marketingList.OwnerId.Name.Length < 2 || marketingList.OwnerId.Name.Length > 30)
                throw new InvalidPluginExecutionException($"OwnerId's Name is not in the valid range of 2-30 caracters.");

            MarketingInfo marketingInfo = new MarketingInfo();
            marketingInfo.bearerCategory = "SkåApp"; //Hardcoded - This should always be the value
            marketingInfo.campaignId = campaign.Id != null ? campaign.Id.ToString() : "";
            CgiSettingEntity settings = XrmRetrieveHelper.RetrieveFirst<CgiSettingEntity>(localContext, new ColumnSet(CgiSettingEntity.Fields.ed_SingaporeTicketOfferId, CgiSettingEntity.Fields.ed_ProvaPaCampaignURL));
            if (settings != null && !string.IsNullOrWhiteSpace(settings.ed_SingaporeTicketOfferId))
            {
                marketingInfo.offerId = settings.ed_SingaporeTicketOfferId; //Grab this from the settings entity - This should be manually uppdated - Or we should send a request to a service to get the latest guid
            }

            //marketingInfo.createdBy = marketingList.OwnerId != null ? marketingList.OwnerId.Name : "CRM";
            //marketingInfo.created = marketingList.CreatedOn != null ? String.Format("{0:s}", marketingList.CreatedOn.Value) + "Z" : "2022-03-29T08:00:09Z"; //This might not be needed anymore

            marketingInfo.orderrows = new List<OrderRow>();

            var queryContacts = new QueryExpression(ContactEntity.EntityLogicalName);
            queryContacts.Distinct = true;
            queryContacts.NoLock = true;
            queryContacts.ColumnSet.AddColumns(ContactEntity.Fields.Telephone2, ContactEntity.Fields.ed_MklId);
            queryContacts.Criteria.AddCondition(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)ContactState.Active);
            queryContacts.Criteria.AddCondition(ContactEntity.Fields.ed_MklId, ConditionOperator.NotNull);

            var queryListMember = queryContacts.AddLink("listmember", ContactEntity.Fields.ContactId, "entityid");
            var ah = queryListMember.AddLink(MarketingListEntity.EntityLogicalName, MarketingListEntity.Fields.ListId, MarketingListEntity.Fields.ListId);
            ah.LinkCriteria.AddCondition(MarketingListEntity.Fields.ListId, ConditionOperator.Equal, gMarketingListId);

            List<ContactEntity> lContacts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, queryContacts);

            foreach (ContactEntity contact in lContacts)
            {
                OrderRow orderrow = new OrderRow();
                int mklIdNr = 0;
                if (Int32.TryParse(contact.ed_MklId, out mklIdNr))
                {
                    orderrow.mklId = mklIdNr;
                }
                
                orderrow.telephone = contact.Telephone2;

                marketingInfo.orderrows.Add(orderrow);
            }

            string sMarketingInfo = JsonHelper.JsonSerializer<MarketingInfo>(marketingInfo);

            //Create a memmory stream? and send it to Kai - He wants s to create a file and send it to them...
            using (var jsonStream = GenerateStreamFromString(sMarketingInfo))
            {
                // ... Do stuff to stream
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient _httpClient = new HttpClient();
                HttpResponseMessage resposneTjp = new HttpResponseMessage();

                if (settings == null || string.IsNullOrWhiteSpace(settings.ed_ProvaPaCampaignURL) || string.IsNullOrWhiteSpace(settings.ed_ProvaPaCampaignURL))
                {
                    //Bad request -> exception
                    throw new Exception($"Exception caught: ProvaPa Campaign URL missing from Settings parameter.");
                }

                try
                {
                    using (var mfdc = new MultipartFormDataContent())
                    {
                        var fileStreamContent = new StreamContent(jsonStream);
                        fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
                        mfdc.Add(fileStreamContent, name: "blob", fileName: "order.json");
                        
                        Task.Run(async () =>
                        {
                            resposneTjp = await _httpClient.PostAsync(settings.ed_ProvaPaCampaignURL, mfdc);

                        }).Wait();
                    }

                    if (resposneTjp.StatusCode == HttpStatusCode.OK || resposneTjp.StatusCode == HttpStatusCode.Accepted)
                    {
                        response.OK = true;
                        response.Message = $"Marketing List Information was sent successfully!";
                        response.Message = $"Marketing List Information was sent successfully!\r\nResponse: {resposneTjp.RequestMessage}\r\nInfo: {sMarketingInfo}"; //for test
                        return response;
                    }
                    else
                    {
                        response.OK = false;
                        response.Message = $"Marketing List Information could not be sent.\r\nStatus: {resposneTjp.StatusCode}\r\nEx: {resposneTjp.RequestMessage}";
                        response.Message = $"Marketing List Information could not be sent.\r\nStatus: {resposneTjp.StatusCode}\r\nEx: {resposneTjp.RequestMessage}\r\nInfo: {sMarketingInfo}"; //for test
                        return response;
                    }
                }
                catch (WebException webEx) when (webEx.Status == WebExceptionStatus.NameResolutionFailure)
                { /* Do something with e, please.*/
                    throw new InvalidPluginExecutionException($"HttpRequestException: {webEx.Message}.\r\nInnerException: {webEx.InnerException}.");
                }
                catch (HttpRequestException httpEx)
                {
                    throw new InvalidPluginExecutionException($"HttpRequestException: {httpEx.Message}.\r\nInnerException: {httpEx.InnerException}.");
                }
                catch (Exception ex)
                {
                    throw new InvalidPluginExecutionException($"Exception: {ex.Message}.\r\nInnerException: {ex.InnerException}.");
                }
            }

            response.OK = false;
            response.Message = "Marketing List Information could not be sent.";
            response.Message = $"Marketing List Information could not be sent.\r\nInfo: {sMarketingInfo}"; ; //This is for testing
            return response;
        }

        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

    }
}
