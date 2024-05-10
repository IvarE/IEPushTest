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


        internal void UpdateValueCodeInCRM(int threadId)
        {
            using (var _logger = new AppInsightsLogger())
            {


                try
                {
                    CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
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
                    _logger.LogError($"Error on updating ValueCode: " + ex);
                }
            }
        }
    }


    public class ValueCodeEvent
    {
        public decimal amount { get; set; }
        public DateTime created { get; set; }
        public string tag { get; set; } 
        public int voucherType { get; set; } //ed_valuecodetypeglobal
        public decimal? remainingAmount { get; set; } //Skip
        public DateTime? disabled { get; set; } //Redeemed
        public string ticketId { get; set; }
        public int status { get; set; } //Kommer sen för uppdateringsflödet
        //Voucher Service 2.0
        public string voucherCode { get; set; } //CodeId
        public Guid voucherId { get; set; } //Skip
        public DateTime validFromDate { get; set; } //Skip
        public DateTime validToDate { get; set; } //LastRedemp
        //public long? eanCode { get; set; } //EanCode
        //public int? couponId { get; set; } //Skip

        internal void UpdateValueCodeInCRM(int threadId)
        {
            using (var _logger = new AppInsightsLogger())
            {


                try
                {
                    CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                    // Cast the proxy client to the IOrganizationService interface.
                    using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                    {
                        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                        if (localContext.OrganizationService == null)
                            throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));


                        List<string> attributeLst = new List<string>();
                        attributeLst.Add(FeatureTogglingEntity.Fields.ed_RemoveControlForTypeOfValueCodeEnabled);
                        attributeLst.Add(FeatureTogglingEntity.Fields.ed_PaybackVoucherEnabled);

                        FeatureTogglingEntity featureToggling = FeatureTogglingEntity.GetFeatureToggling(localContext, attributeLst);


                        QueryExpression query = new QueryExpression()
                        {
                            EntityName = ValueCodeEntity.EntityLogicalName,
                            ColumnSet = new ColumnSet(true),
                            Criteria = new FilterExpression()
                            {
                                Conditions =
                            {
                                //new ConditionExpression(ValueCodeEntity.Fields.statecode, ConditionOperator.Equal, (int)Generated.ed_ValueCodeState.Active),
                                new ConditionExpression(ValueCodeEntity.Fields.ed_CodeId, ConditionOperator.Equal, this.voucherCode)
                            }
                            }
                        };

                        if (featureToggling.ed_PaybackVoucherEnabled != true)
                        {
                            ConditionExpression conditionState = new ConditionExpression(ValueCodeEntity.Fields.statecode, ConditionOperator.Equal, (int)Generated.ed_ValueCodeState.Active);
                            query.Criteria.AddCondition(conditionState);
                        }

                        ValueCodeEntity valueCode = XrmRetrieveHelper.RetrieveFirst<ValueCodeEntity>(localContext, query);
                        DateTime? redeemed = this.disabled;

                        //Devop Task 745 - Round decimals
                        if (this.amount > 0)
                        {
                            var roundedValue = (decimal)Math.Round(this.amount, 0, MidpointRounding.AwayFromZero);
                            this.amount = roundedValue;
                        }

                        if (featureToggling.ed_RemoveControlForTypeOfValueCodeEnabled == true)
                        {
                            if (valueCode == null)
                            {
                                #region No Value Code was found (create a new one)

                                ValueCodeEntity newValueCode = new ValueCodeEntity()
                                {
                                    ed_name = voucherId.ToString(),
                                    ed_Amount = new Money(amount),
                                    ed_CreatedTimestamp = created,
                                    //ed_LastRedemptionDate = validToDate, //Voucher Service 2.0
                                    //ed_ValidUntil = validToDate, //Voucher Service 2.0
                                    ed_CodeId = voucherCode,
                                    //ed_Ean = eanCode?.ToString(),
                                    ed_OriginalAmount = amount,
                                    ed_ValueCodeVoucherId = voucherId.ToString(),
                                    st_tag = tag
                                };

                                switch (this.voucherType)
                                {
                                    case (int)Generated.ed_valuecodetypeglobal.Ersattningsarende:
                                        newValueCode.ed_ValueCodeTypeGlobal = Generated.ed_valuecodetypeglobal.Ersattningsarende;
                                        break;
                                    case (int)Generated.ed_valuecodetypeglobal.Forlustgaranti:
                                        newValueCode.ed_ValueCodeTypeGlobal = Generated.ed_valuecodetypeglobal.Forlustgaranti;
                                        break;
                                    case (int)Generated.ed_valuecodetypeglobal.Forseningsersattning:
                                        newValueCode.ed_ValueCodeTypeGlobal = Generated.ed_valuecodetypeglobal.Forseningsersattning;
                                        break;
                                    case (int)Generated.ed_valuecodetypeglobal.InlostReskassa:
                                        newValueCode.ed_ValueCodeTypeGlobal = Generated.ed_valuecodetypeglobal.InlostReskassa;
                                        break;
                                    default:
                                        newValueCode.ed_ValueCodeTypeGlobal = Generated.ed_valuecodetypeglobal.Presentkort;
                                        break;
                                }

                                newValueCode.Id = XrmHelper.Create(localContext, newValueCode);

                                //Bellow changed with the inclusion of ed_ValueCOdeVoucherId field 05-11-20
                                if (String.IsNullOrWhiteSpace(newValueCode.ed_name))
                                {
                                    newValueCode.ed_name = newValueCode.Id.ToString();
                                    XrmHelper.Update(localContext, newValueCode);
                                }

                            //Handle updates where ValueCode has been canceled by Voucher Service (status 4 = Canceled)
                                if (this.status == 4)
                                {
                                    var updateValueCode = new ValueCodeEntity()
                                    {
                                        Id = newValueCode.Id,
                                        ed_Amount = new Money(this.amount),
                                        ed_RedemptionDate = redeemed,
                                        ed_CanceledOn = (DateTime?)DateTime.UtcNow,
                                        st_tag = tag
                                    };

                                        UpdateValueCodeRecordAndCancel(localContext, updateValueCode);
                                }
                                else if (this.status == 2)
                                {
                                    var updateValueCode = new ValueCodeEntity()
                                    {
                                        Id = newValueCode.Id,
                                        ed_Amount = new Money(this.amount),
                                        ed_RedemptionDate = redeemed,
                                        st_tag = tag
                                    };

                                    UpdateValueCodeRecordAndExpired(localContext, updateValueCode);
                                }
                                else if (this.amount <= 0)
                                {
                                    newValueCode.ed_RedemptionDate = redeemed;
                                    UpdateValueCodeRecordAndDeactivate(localContext, newValueCode);
                                }
                                else
                                {
                                    XrmHelper.Update(localContext.OrganizationService, newValueCode);

                                    // Update status of value code to be "Skickad"
                                    SetStateRequest req = new SetStateRequest()
                                    {
                                        EntityMoniker = newValueCode.ToEntityReference(),
                                        State = new OptionSetValue((int)Generated.ed_ValueCodeState.Active),
                                        Status = new OptionSetValue((int)ValueCodeEntity.Status.Skickad)
                                    };
                                    SetStateResponse resp = (SetStateResponse)localContext.OrganizationService.Execute(req);
                                }

                                valueCode = newValueCode;

                                #endregion
                            }
                            else
                            {
                                #region Value Code was found in SeKund (update existing one)

                                //Handle updates where ValueCode has been canceled by Voucher Service (status 4 = Canceled)
                                if (this.status == 4)
                                {
                                    //Check if valuecode already has been cancled (this makes us dependent on the field being empty when when opened)
                                    if (/*valueCode.ed_CanceledBy == null && */valueCode.statecode == (int)Generated.ed_ValueCodeState.Active)
                                    {
                                        var updateValueCode = new ValueCodeEntity()
                                        {
                                            Id = valueCode.Id,
                                            ed_Amount = new Money(this.amount),
                                            ed_RedemptionDate = redeemed,
                                            ed_CanceledOn = (DateTime?)DateTime.UtcNow,
                                            st_tag = tag
                                        };

                                        UpdateValueCodeRecordAndCancel(localContext, updateValueCode);
                                    }
                                }
                                else if (this.status == 2)
                                {
                                    var updateValueCode = new ValueCodeEntity()
                                    {
                                        Id = valueCode.Id,
                                        ed_Amount = new Money(this.amount),
                                        ed_RedemptionDate = redeemed,
                                        st_tag = tag
                                    };

                                    UpdateValueCodeRecordAndExpired(localContext, updateValueCode);
                                }
                                else if (this.amount <= 0)
                                {
                                    var updateValueCode = new ValueCodeEntity()
                                    {
                                        Id = valueCode.Id,
                                        ed_Amount = new Money(this.amount),
                                        ed_RedemptionDate = redeemed,
                                        st_tag = tag
                                    };

                                    UpdateValueCodeRecordAndDeactivate(localContext, updateValueCode);
                                }
                                else
                                {
                                    if (valueCode.statecode == Generated.ed_ValueCodeState.Inactive &&
                                        featureToggling.ed_PaybackVoucherEnabled == true &&
                                        this.amount > 0)
                                    {
                                        // Voucher Code has been redeemed and needs to be reactivated with a higher amount
                                        SetStateRequest req = new SetStateRequest()
                                        {
                                            EntityMoniker = valueCode.ToEntityReference(),
                                            State = new OptionSetValue((int)Generated.ed_ValueCodeState.Active),
                                            Status = new OptionSetValue((int)ValueCodeEntity.Status.Skickad)
                                        };
                                        SetStateResponse resp = (SetStateResponse)localContext.OrganizationService.Execute(req);

                                        var updateValueCode = new ValueCodeEntity()
                                        {
                                            Id = valueCode.Id,
                                            ed_Amount = new Money(this.amount),
                                            st_tag = tag
                                        };

                                        XrmHelper.Update(localContext.OrganizationService, updateValueCode);
                                    }
                                    else
                                    {
                                        var updateValueCode = new ValueCodeEntity()
                                        {
                                            Id = valueCode.Id,
                                            ed_Amount = new Money(this.amount),
                                            st_tag = tag
                                        };

                                        XrmHelper.Update(localContext.OrganizationService, updateValueCode);
                                    }
                                }

                                #endregion
                            }

                            #region Create Value Code Transaction row

                            CreateValueCodeTransaction(threadId, localContext, valueCode, _logger);

                            #endregion
                        }
                        else
                        {
                            if (valueCode == null)
                            {

                                ValueCodeEntity newValueCode = new ValueCodeEntity()
                                {
                                    ed_name = voucherId.ToString(),
                                    ed_Amount = new Money(amount),
                                    ed_CreatedTimestamp = created,
                                    //ed_LastRedemptionDate = validToDate, //Changed in VoucherService 2.0
                                    //ed_ValidUntil = validToDate, //Changed in VoucherService 2.0
                                    ed_CodeId = voucherCode,
                                    //ed_Ean = eanCode?.ToString(), //Changed in VoucherService 2.0
                                    ed_OriginalAmount = amount,
                                    ed_ValueCodeVoucherId = voucherId.ToString(),
                                    st_tag = tag
                                };

                                switch (voucherType)
                                {
                                    case (int)Generated.ed_valuecodetypeglobal.Ersattningsarende:
                                        newValueCode.ed_ValueCodeTypeGlobal = Generated.ed_valuecodetypeglobal.Ersattningsarende;
                                        break;
                                    case (int)Generated.ed_valuecodetypeglobal.Forlustgaranti:
                                        newValueCode.ed_ValueCodeTypeGlobal = Generated.ed_valuecodetypeglobal.Forlustgaranti;
                                        break;
                                    case (int)Generated.ed_valuecodetypeglobal.Forseningsersattning:
                                        newValueCode.ed_ValueCodeTypeGlobal = Generated.ed_valuecodetypeglobal.Forseningsersattning;
                                        break;
                                    case (int)Generated.ed_valuecodetypeglobal.InlostReskassa:
                                        newValueCode.ed_ValueCodeTypeGlobal = Generated.ed_valuecodetypeglobal.InlostReskassa;
                                        break;
                                    default:
                                        newValueCode.ed_ValueCodeTypeGlobal = Generated.ed_valuecodetypeglobal.Presentkort; //Type 5
                                        break;
                                }

                                newValueCode.Id = XrmHelper.Create(localContext, newValueCode);

                                //Bellow changed with the inclusion of ed_ValueCOdeVoucherId field 05-11-20
                                if (String.IsNullOrWhiteSpace(newValueCode.ed_name))
                                {
                                    newValueCode.ed_name = newValueCode.Id.ToString();
                                    XrmHelper.Update(localContext, newValueCode);
                                }

                                //Handle updates where ValueCode has been canceled by Voucher Service (status 4 = Canceled)
                                //1 = Activated
                                //2 = Expired
                                //3 = Used Up
                                //4 = Cancelled
                                if (/*this.status == 3 ||*/ this.status == 4)
                                {
                                    var updateValueCode = new ValueCodeEntity()
                                    {
                                        Id = newValueCode.Id,
                                        ed_Amount = new Money(this.amount),
                                        ed_RedemptionDate = redeemed,
                                        ed_CanceledOn = (DateTime?)DateTime.UtcNow,
                                        st_tag = tag
                                    };

                                    UpdateValueCodeRecordAndCancel(localContext, updateValueCode);
                                }
                                else if (this.status == 2)
                                {
                                    var updateValueCode = new ValueCodeEntity()
                                    {
                                        Id = newValueCode.Id,
                                        ed_Amount = new Money(this.amount),
                                        ed_RedemptionDate = redeemed,
                                        st_tag = tag
                                    };

                                    UpdateValueCodeRecordAndExpired(localContext, updateValueCode);
                                }
                                else if (this.amount <= 0 || this.status == 3)
                                {
                                    newValueCode.ed_RedemptionDate = redeemed;
                                    UpdateValueCodeRecordAndDeactivate(localContext, newValueCode);
                                }
                                else
                                {

                                    XrmHelper.Update(localContext.OrganizationService, newValueCode);

                                }

                            }
                        // Presentkort
                        else if ((int)valueCode.ed_ValueCodeTypeGlobal.Value == 2)
                        {
                            //Handle updates where ValueCode has been canceled by Voucher Service (status 4 = Canceled)
                            if (this.status == 4)
                            {
                                var updateValueCode = new ValueCodeEntity()
                                {
                                    Id = valueCode.Id,
                                    ed_Amount = new Money(this.amount),
                                    ed_RedemptionDate = redeemed,
                                    ed_CanceledOn = (DateTime?)DateTime.UtcNow,
                                    st_tag = tag
                                };

                                UpdateValueCodeRecordAndCancel(localContext, updateValueCode);
                            }
                            else if (this.status == 2)
                            {
                                    var updateValueCode = new ValueCodeEntity()
                                    {
                                        Id = valueCode.Id,
                                        ed_Amount = new Money(this.amount),
                                        ed_RedemptionDate = redeemed,
                                        st_tag = tag
                                    };

                                    UpdateValueCodeRecordAndExpired(localContext, updateValueCode);
                            }
                            else if (this.amount <= 0 || this.status == 3)
                            {
                                valueCode.ed_Amount = new Money(this.amount);
                                valueCode.ed_RedemptionDate = redeemed;

                                    UpdateValueCodeRecordAndDeactivate(localContext, valueCode);


                                    SetStateRequest req = new SetStateRequest()
                                    {
                                        EntityMoniker = valueCode.ToEntityReference(),
                                        State = new OptionSetValue((int)Generated.ed_ValueCodeState.Inactive),
                                        Status = new OptionSetValue((int)ValueCodeEntity.Status.Inlost)
                                    };
                                    SetStateResponse resp = (SetStateResponse)localContext.OrganizationService.Execute(req);

                                    // TODO : Marcus Generate
                            }
                            else
                            {
                                valueCode.ed_Amount = new Money(this.amount);

                                XrmHelper.Update(localContext.OrganizationService, valueCode);
                            }
                        }
                        // Övriga
                        else if ((int)valueCode.ed_ValueCodeTypeGlobal.Value != 2)
                        {
                            //DevOps Task: 3998
                            //Handle updates where ValueCode has been canceled by Voucher Service(status 4 = Canceled)
                            if (this.status == 4)
                            {
                                var updateValueCode = new ValueCodeEntity()
                                {
                                    Id = valueCode.Id,
                                    ed_Amount = new Money(this.amount),
                                    ed_RedemptionDate = redeemed,
                                    ed_CanceledOn = (DateTime?)DateTime.UtcNow,
                                    st_tag = tag
                                };

                                    UpdateValueCodeRecordAndCancel(localContext, updateValueCode);
                            }
                            else if (this.status == 2)
                            {
                                    var updateValueCode = new ValueCodeEntity()
                                    {
                                        Id = valueCode.Id,
                                        ed_Amount = new Money(this.amount),
                                        ed_RedemptionDate = redeemed,
                                        st_tag = tag
                                    };

                                    UpdateValueCodeRecordAndExpired(localContext, updateValueCode);
                            }
                            else if (this.amount > 0
                                    && (int)valueCode.ed_ValueCodeTypeGlobal.Value != 1
                                    && (int)valueCode.ed_ValueCodeTypeGlobal.Value != 3
                                    && (int)valueCode.ed_ValueCodeTypeGlobal.Value != 4) //This should be modified according to new DevOps Task: 3888
                            {
                                    valueCode.ed_Amount = new Money(this.amount);
                                    XrmHelper.Update(localContext, valueCode);
                            }
                            else
                            {
                                    valueCode.ed_Amount = new Money(0);
                                    valueCode.ed_RedemptionDate = redeemed;
                                    UpdateValueCodeRecordAndDeactivate(localContext, valueCode);
                            }
                            }

                            CreateValueCodeTransaction(threadId, localContext, valueCode, _logger);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error on updating ValueCode: " + ex);
                    throw ex;
                }
                finally
                {
                    ConnectionCacheManager.ReleaseConnection(threadId);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="threadId"></param>
        /// <param name="localContext"></param>
        /// <param name="valueCode"></param>
        private ValueCodeTransactionEntity CreateValueCodeTransaction(int threadId, Plugin.LocalPluginContext localContext, ValueCodeEntity valueCode, AppInsightsLogger _logger)
        {
            if (valueCode == null)
                throw new NullReferenceException($"Argument '{nameof(valueCode)}' is empty.");

            ValueCodeTransactionEntity valueCodeTransaction = null;

            try
            {
                valueCodeTransaction = new ValueCodeTransactionEntity()
                {
                    ed_name = $"{DateTime.UtcNow.AddHours(1)} - {this.voucherCode}",
                    ed_Balance = this.amount,
                    ed_PreviousBalance = valueCode.ed_Amount.Value,
                    ed_Redeemed = this.amount <= 0 ? true : false,
                    ed_TransactionDate = DateTime.UtcNow.AddHours(1),
                    ed_ValueCode = valueCode.ToEntityReference(),
                    ed_TicketId = !String.IsNullOrEmpty(this.ticketId) ? this.ticketId : ""
                };

                valueCodeTransaction.Id = XrmHelper.Create(localContext, valueCodeTransaction);

                return valueCodeTransaction;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in CreateValueCodeTransaction: {ex.Message}");
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="valueCode"></param>
        private void UpdateValueCodeRecordAndDeactivate(Plugin.LocalPluginContext localContext, ValueCodeEntity valueCode)
        {
            XrmHelper.Update(localContext.OrganizationService, valueCode);

            SetStateRequest req = new SetStateRequest()
            {
                EntityMoniker = valueCode.ToEntityReference(),
                State = new OptionSetValue((int)Generated.ed_ValueCodeState.Inactive),
                Status = new OptionSetValue((int)ValueCodeEntity.Status.Inlost)
            };
            SetStateResponse resp = (SetStateResponse)localContext.OrganizationService.Execute(req);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="valueCode"></param>
        private void UpdateValueCodeRecordAndCancel(Plugin.LocalPluginContext localContext, ValueCodeEntity valueCode)
        {
            XrmHelper.Update(localContext.OrganizationService, valueCode);

            SetStateRequest req = new SetStateRequest()
            {
                EntityMoniker = valueCode.ToEntityReference(),
                State = new OptionSetValue((int)Generated.ed_ValueCodeState.Inactive),
                Status = new OptionSetValue((int)ValueCodeEntity.Status.Makulerad)
            };
            SetStateResponse resp = (SetStateResponse)localContext.OrganizationService.Execute(req);
        }

        private void UpdateValueCodeRecordAndExpired(Plugin.LocalPluginContext localContext, ValueCodeEntity valueCode)
        {
            XrmHelper.Update(localContext.OrganizationService, valueCode);

            SetStateRequest req = new SetStateRequest()
            {
                EntityMoniker = valueCode.ToEntityReference(),
                State = new OptionSetValue((int)Generated.ed_ValueCodeState.Inactive),
                Status = new OptionSetValue((int)ValueCodeEntity.Status.Forfallen)
            };
            SetStateResponse resp = (SetStateResponse)localContext.OrganizationService.Execute(req);
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
