using Endeavor.Crm;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using Skanetrafiken.Crm.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Xml.Serialization;
using Generated = Skanetrafiken.Crm.Schema.Generated;

namespace Skanetrafiken.Crm.Models
{
    public class ValueCodePushMessage
    {
        public string _event { get; set; }
        public Coupon coupon { get; set; }

        protected static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        internal void UpdateValueCodeInCRM(int threadId)
        {
            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                    QueryExpression query = new QueryExpression()
                    {
                        EntityName = ValueCodeEntity.EntityLogicalName,
                        ColumnSet = new ColumnSet(true),
                        Criteria = new FilterExpression()
                        {
                            Conditions =
                                {
                                    new ConditionExpression(ValueCodeEntity.Fields.statecode, ConditionOperator.Equal, (int)Generated.ed_ValueCodeState.Active),
                                    new ConditionExpression(ValueCodeEntity.Fields.ed_CodeId, ConditionOperator.Equal, this.coupon.coupons_provider_unique_code),
                                    new ConditionExpression(ValueCodeEntity.Fields.ed_Status, ConditionOperator.Equal, "0")
                                }
                        }
                    };

                    DateTime? nullDate = null;
                    ValueCodeEntity valueCode = XrmRetrieveHelper.RetrieveFirst<ValueCodeEntity>(localContext, query);

                    DateTime parsedDate;
                    bool parsed = DateTime.TryParse(this.coupon.coupons_redeemed_timestamp, out parsedDate);

                    if (valueCode != null)
                    {
                        valueCode.ed_RedemptionDate = parsed ? parsedDate : nullDate;
                        XrmHelper.Update(localContext.OrganizationService, valueCode);

                        SetStateRequest req = new SetStateRequest()
                        {
                            EntityMoniker = valueCode.ToEntityReference(),
                            State = new OptionSetValue((int)Generated.ed_ValueCodeState.Inactive),
                            Status = new OptionSetValue((int)ValueCodeEntity.Status.Inlost)
                        };
                        SetStateResponse resp = (SetStateResponse)localContext.OrganizationService.Execute(req);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.ErrorFormat($"Th={threadId} - Error on updating ValueCode: " + ex);
            }
        }
    }


    public class ValueCodeEvent
    {
        public decimal amount { get; set; }
        public DateTime created { get; set; }
        public string tag { get; set; }
        public DateTime validFromDate { get; set; }
        public DateTime validToDate { get; set; }
        public string voucherCode { get; set; }
        public Guid voucherId { get; set; }
        public int voucherType { get; set; }
        public decimal? remainingAmount { get; set; }
        public DateTime? disabled { get; set; }
        public long? eanCode { get; set; }
        public int? couponId { get; set; }

        protected static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        internal bool UpdateValueCodeInCRM(int threadId)
        {
            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                    QueryExpression query = new QueryExpression()
                    {
                        EntityName = ValueCodeEntity.EntityLogicalName,
                        ColumnSet = new ColumnSet(true),
                        Criteria = new FilterExpression()
                        {
                            Conditions =
                                {
                                    new ConditionExpression(ValueCodeEntity.Fields.statecode, ConditionOperator.Equal, (int)Generated.ed_ValueCodeState.Active),
                                    new ConditionExpression(ValueCodeEntity.Fields.ed_CodeId, ConditionOperator.Equal, this.voucherCode)
                                }
                        }
                    };
                    
                    ValueCodeEntity valueCode = XrmRetrieveHelper.RetrieveFirst<ValueCodeEntity>(localContext, query);


                    if(valueCode == null)
                    {
                        _log.Debug($"Could not find value code '{voucherCode}'");
                        return false;
                    }

                    DateTime? redeemed = this.disabled;

                   

                    // Presentkort
                    if ((int)valueCode.ed_ValueCodeTypeGlobal.Value == 2)
                    {
                        if(this.amount <= 0)
                        {
                            valueCode.ed_Amount = new Money(this.amount);
                            valueCode.ed_RedemptionDate = redeemed;

                            XrmHelper.Update(localContext.OrganizationService, valueCode);
                            _log.Debug($"Updating value code values.");
                            _log.Debug($"New values - Amount: '{valueCode?.ed_Amount.Value}', RevemptionDate: '{valueCode?.ed_RedemptionDate}'");

                            _log.Debug($"Updating value code status.");
                            SetStateRequest req = new SetStateRequest()
                            {
                                EntityMoniker = valueCode.ToEntityReference(),
                                State = new OptionSetValue((int)Generated.ed_ValueCodeState.Inactive),
                                Status = new OptionSetValue((int)ValueCodeEntity.Status.Inlost)
                            };
                            SetStateResponse resp = (SetStateResponse)localContext.OrganizationService.Execute(req);

                            // TODO : Marcus Generate
                            //_log.Debug($"New status - State: '{Generated.ed_ValueCodeState.Inactive}', Status: '{Generated.ed_valuecodes.Inlost}'");
                        }
                        else
                        {
                            valueCode.ed_Amount = new Money(this.amount);

                            XrmHelper.Update(localContext.OrganizationService, valueCode);
                            _log.Debug($"Updating value code value.");
                            _log.Debug($"New value - Amount: '{valueCode?.ed_Amount.Value}'");

                        }
                    }
                    // Övriga
                    else if ((int)valueCode.ed_ValueCodeTypeGlobal.Value != 2)
                    {
                        valueCode.ed_Amount = new Money(0);
                        valueCode.ed_RedemptionDate = redeemed;

                        XrmHelper.Update(localContext.OrganizationService, valueCode);
                        _log.Debug($"Updating value code values.");
                        _log.Debug($"New values - Amount: '{valueCode?.ed_Amount.Value}', RedemptionDate: {valueCode?.ed_RedemptionDate}");

                        _log.Debug($"Updating value code status.");
                        SetStateRequest req = new SetStateRequest()
                        {
                            EntityMoniker = valueCode.ToEntityReference(),
                            State = new OptionSetValue((int)Generated.ed_ValueCodeState.Inactive),
                            Status = new OptionSetValue((int)ValueCodeEntity.Status.Inlost)
                        };
                        SetStateResponse resp = (SetStateResponse)localContext.OrganizationService.Execute(req);

                        // TODO : Marcus Generate
                        //_log.Debug($"New status - State: '{Generated.ed_ValueCodeState.Inactive}', Status: '{Generated.ed_valuecode_statuscode.Inlost}'")
                    }
                    
                }
            }
            catch (Exception ex)
            {
                _log.ErrorFormat($"Th={threadId} - Error on updating ValueCode: " + ex);
                return false;
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }

            return true;
        }
    }


    public class Coupon
    {
        public int coupons_companies_id { get; set; }
        public int coupons_admins_id { get; set; }
        public int coupons_clients_id { get; set; }
        public int coupons_templates_id { get; set; }
        public object coupons_ticket_reference { get; set; }
        public string coupons_amount { get; set; }
        public string coupons_reason { get; set; }
        public string coupons_type { get; set; }
        public string coupons_delivery_type { get; set; }
        public string coupons_status { get; set; }
        public string coupons_sent { get; set; }
        public object coupons_custom_image { get; set; }
        public object coupons_custom_text { get; set; }
        public string coupons_ean { get; set; }
        public Coupons_Last_Redemption_Date coupons_last_redemption_date { get; set; }
        public string coupons_provider_unique_code { get; set; }
        public string coupons_created_timestamp { get; set; }
        public int coupons_id { get; set; }
        public string coupons_image { get; set; }
        public int coupons_redeemed { get; set; }
        public string coupons_redeemed_store_id { get; set; }
        public string coupons_redeemed_timestamp { get; set; }
    }

    public class Coupons_Last_Redemption_Date
    {
        public string date { get; set; }
        public int timezone_type { get; set; }
        public string timezone { get; set; }
    }




    [XmlRoot(ElementName = "Coupon", Namespace = "http://v1_0.model.loopback.kuponginlosen.se")]
    public class ValueCodePushMessageResult
    {
        protected static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [XmlElement(ElementName = "Issuer", Namespace = "http://v1_0.model.loopback.kuponginlosen.se")]
        public string Issuer { get; set; }
        [XmlElement(ElementName = "StoreId", Namespace = "http://v1_0.model.loopback.kuponginlosen.se")]
        public string StoreId { get; set; }
        [XmlElement(ElementName = "CampaignNumber", Namespace = "http://v1_0.model.loopback.kuponginlosen.se")]
        public string CampaignNumber { get; set; }
        [XmlElement(ElementName = "EanCode", Namespace = "http://v1_0.model.loopback.kuponginlosen.se")]
        public string EanCode { get; set; }
        [XmlElement(ElementName = "UniqueCode", Namespace = "http://v1_0.model.loopback.kuponginlosen.se")]
        public string UniqueCode { get; set; }
        [XmlElement(ElementName = "RedeemedDT", Namespace = "http://v1_0.model.loopback.kuponginlosen.se")]
        public string RedeemedDT { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }

        internal void UpdateValueCodeInCRM(int threadId)
        {
            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                    QueryExpression query = new QueryExpression()
                    {
                        EntityName = ValueCodeEntity.EntityLogicalName,
                        ColumnSet = new ColumnSet(true),
                        Criteria = new FilterExpression()
                        {
                            Conditions =
                                {
                                    new ConditionExpression(ValueCodeEntity.Fields.statecode, ConditionOperator.Equal, (int)Generated.ed_ValueCodeState.Active),
                                    new ConditionExpression(ValueCodeEntity.Fields.ed_CodeId, ConditionOperator.Equal, this.UniqueCode),
                                    new ConditionExpression(ValueCodeEntity.Fields.ed_Status, ConditionOperator.Equal, "0")
                                }
                        }
                    };

                    DateTime? nullDate = null;
                    ValueCodeEntity valueCode = XrmRetrieveHelper.RetrieveFirst<ValueCodeEntity>(localContext, query);

                    DateTime parsedDate;
                    bool parsed = DateTime.TryParse(RedeemedDT, out parsedDate);

                    if (valueCode != null)
                    {
                        valueCode.ed_CampaignNumber = this.CampaignNumber;
                        valueCode.ed_Issuer = this.Issuer;
                        valueCode.ed_StoreId = this.StoreId;
                        valueCode.ed_RedemptionDate = parsed ? parsedDate : nullDate;
                        XrmHelper.Update(localContext.OrganizationService, valueCode);

                        SetStateRequest req = new SetStateRequest()
                        {
                            EntityMoniker = valueCode.ToEntityReference(),
                            State = new OptionSetValue((int)Generated.ed_ValueCodeState.Inactive),
                            Status = new OptionSetValue((int)ValueCodeEntity.Status.Inlost)
                        };
                        SetStateResponse resp = (SetStateResponse)localContext.OrganizationService.Execute(req);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
