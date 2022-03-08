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

            Order order = new Order();

            if(marketingList.OwnerId == null)
                throw new InvalidPluginExecutionException($"OwnerId of Marketing List is null.");

            if (marketingList.OwnerId.Name.Length < 2 || marketingList.OwnerId.Name.Length > 30)
                throw new InvalidPluginExecutionException($"OwnerId's Name is not in the valid range of 2-30 caracters.");

            order.createdby = marketingList.OwnerId != null ? marketingList.OwnerId.Name : "";
            order.created = marketingList.CreatedOn != null ? String.Format("{0:s}", marketingList.CreatedOn.Value) + "Z" : "";
            order.offerId = "d1425509-a769-4a34-9baa-2daaf33b93c1";
            order.campaignid = campaign.Id != null ? campaign.Id.ToString() : "";
            order.priceid = "TODO STRING";
            order.pricemodelid = "TODO STRING";
            order.travelareaids.Add("TODO STRING");
            order.travelareaids.Add("TODO STRING");

            MarketingInfo marketingInfo = new MarketingInfo();
            marketingInfo.order = order;
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
                orderrow.mklid = contact.ed_MklId;
                orderrow.telephone = contact.Telephone2;

                marketingInfo.orderrows.Add(orderrow);
            }

            string sMarketingInfo = JsonHelper.JsonSerializer<MarketingInfo>(marketingInfo);

            response.OK = true;
            response.Message = "Marketing List Information was sent successfully!";
            response.Message = sMarketingInfo;
            return response;
        }

    }
}
