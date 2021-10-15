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
        public string tag { get; set; } //Skip
        public DateTime validFromDate { get; set; } //Skip
        public DateTime validToDate { get; set; } //LastRedemp
        public string voucherCode { get; set; } //CodeId
        public Guid voucherId { get; set; } //Skip
        public int voucherType { get; set; } //ed_valuecodetypeglobal
        public decimal? remainingAmount { get; set; } //Skip
        public DateTime? disabled { get; set; } //Redeemed
        public long? eanCode { get; set; } //EanCode
        public int? couponId { get; set; } //Skip
        public string ticketId { get; set; }
        public int status { get; set; } //Kommer sen för uppdateringsflödet

        protected static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        internal void UpdateValueCodeInCRM(int threadId)
        {
            try
            {
                _log.Debug($"Entering UpdateValueCodeInCRM.");
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - UpdateValueCodeInCRM: Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                    _log.Info($"Th={threadId} - UpdateValueCodeInCRM: ServiceProxy and LocalContext created Successfully.");

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

                    _log.Info($"Th={threadId} - UpdateValueCodeInCRM: Searching for ValueCode based on voucherCode - {this.voucherCode}.");
                    ValueCodeEntity valueCode = XrmRetrieveHelper.RetrieveFirst<ValueCodeEntity>(localContext, query);
                    DateTime? redeemed = this.disabled;

                    //Devop Task 745 - Round decimals
                    if (this.amount > 0)
                    {
                        var roundedValue = (decimal)Math.Round(this.amount, 0, MidpointRounding.AwayFromZero);
                        _log.Info($"Th={threadId} - UpdateValueCodeInCRM: Rounding value '{this.amount}' to {roundedValue}");
                        this.amount = roundedValue;
                    }

                    if (featureToggling.ed_RemoveControlForTypeOfValueCodeEnabled == true)
                    {
                        _log.Info($"Th={threadId} - UpdateValueCodeInCRM: Using ed_RemoveControlForTypeOfValueCodeEnabled FeatureToggle.");
                        if (valueCode == null)
                        {
                            #region No Value Code was found (create a new one)

                            _log.Info($"Th={threadId} - UpdateValueCodeInCRM: Could not find value code '{this.voucherCode}'. Creating a new value code (Should be type 5).");

                            ValueCodeEntity newValueCode = new ValueCodeEntity()
                            {
                                ed_name = voucherId.ToString(),
                                ed_Amount = new Money(amount),
                                ed_CreatedTimestamp = created,
                                ed_LastRedemptionDate = validToDate,
                                ed_ValidUntil = validToDate,
                                ed_CodeId = voucherCode,
                                ed_Ean = eanCode?.ToString(),
                                ed_OriginalAmount = amount,
                                ed_ValueCodeVoucherId = voucherId.ToString()
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
                            _log.Info($"Th={threadId} - UpdateValueCodeInCRM: ValueCode created with Id - {newValueCode.Id}.");

                            //Bellow changed with the inclusion of ed_ValueCOdeVoucherId field 05-11-20
                            if (String.IsNullOrWhiteSpace(newValueCode.ed_name)) 
                            {
                                newValueCode.ed_name = newValueCode.Id.ToString();
                                XrmHelper.Update(localContext, newValueCode);
                                _log.Debug($"Th={threadId} - UpdateValueCodeInCRM: ValueCode name updated.");
                            }

                            //Handle updates where ValueCode has been canceled by Voucher Service (status 4 = Canceled) 29/10-20
                            if (this.status == 4)
                            {
                                var updateValueCode = new ValueCodeEntity()
                                {
                                    Id = newValueCode.Id,
                                    ed_Amount = new Money(this.amount),
                                    ed_RedemptionDate = redeemed,
                                    ed_CanceledOn = (DateTime?)DateTime.UtcNow
                                };

                                UpdateValueCodeRecordAndCancel(localContext, updateValueCode);
                                _log.Info($"Th={threadId} - UpdateValueCodeInCRM: ValueCode with Id - {newValueCode.Id} - Cancelled.");
                            }
                            else if (this.amount <= 0)
                            {
                                newValueCode.ed_RedemptionDate = redeemed;
                                UpdateValueCodeRecordAndDeactivate(localContext, newValueCode);
                                _log.Info($"Th={threadId} - UpdateValueCodeInCRM: ValueCode with Id - {newValueCode.Id} - Deactivated.");
                            }
                            else
                            {
                                XrmHelper.Update(localContext.OrganizationService, newValueCode);
                                _log.Debug($"Th={threadId} - UpdateValueCodeInCRM: Updating value code value.");
                                _log.Info($"Th={threadId} - UpdateValueCodeInCRM: New ValueCode Amount - '{newValueCode?.ed_Amount.Value}'");

                                // 2020-03-03 - Marcus Stenswed
                                // Update status of value code to be "Skickad"
                                SetStateRequest req = new SetStateRequest()
                                {
                                    EntityMoniker = newValueCode.ToEntityReference(),
                                    State = new OptionSetValue((int)Generated.ed_ValueCodeState.Active),
                                    Status = new OptionSetValue((int)ValueCodeEntity.Status.Skickad)
                                };
                                SetStateResponse resp = (SetStateResponse)localContext.OrganizationService.Execute(req);

                                _log.Info($"Th={threadId} - UpdateValueCodeInCRM: ValueCode status updated to 'Skickad'.");
                            }

                            valueCode = newValueCode;

                            #endregion
                        }
                        else
                        {
                            #region Value Code was found in SeKund (update existing one)

                            _log.Info($"Th={threadId} - UpdateValueCodeInCRM: Value Code '{this.voucherCode}' found in SeKund.");

                            //Handle updates where ValueCode has been canceled by Voucher Service (status 4 = Canceled) 29/10-20
                            if (this.status == 4)
                            {
                                //Check if valuecode already has been cancled (this makes us dependent on the field being empty when when opened)
                                _log.Debug($"Th={threadId} - UpdateValueCodeInCRM: VoucherService sent Status = 4. Check that ValueCode is Active before attempt to Cancel.");
                                if (/*valueCode.ed_CanceledBy == null && */valueCode.statecode == (int)Generated.ed_ValueCodeState.Active)
                                {
                                    _log.Info($"Th={threadId} - UpdateValueCodeInCRM: ValueCode is Active -> Cancel.");
                                    var updateValueCode = new ValueCodeEntity()
                                    {
                                        Id = valueCode.Id,
                                        ed_Amount = new Money(this.amount),
                                        ed_RedemptionDate = redeemed,
                                        ed_CanceledOn = (DateTime?)DateTime.UtcNow
                                    };

                                    UpdateValueCodeRecordAndCancel(localContext, updateValueCode);
                                    _log.Info($"Th={threadId} - UpdateValueCodeInCRM: ValueCode with Id - {valueCode.Id} - Cancelled.");
                                }
                                else {
                                    _log.Info($"Th={threadId} - UpdateValueCodeInCRM: ValueCode is Inactive -> Do not cancel.");
                                }
                            }
                            else if (this.amount <= 0)
                            {
                                var updateValueCode = new ValueCodeEntity()
                                {
                                    Id = valueCode.Id,
                                    ed_Amount = new Money(this.amount),
                                    ed_RedemptionDate = redeemed
                                };
                                
                                UpdateValueCodeRecordAndDeactivate(localContext, updateValueCode);
                                _log.Info($"Th={threadId} - UpdateValueCodeInCRM: ValueCode with Id - {valueCode.Id} - Deactivated.");
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
                                        ed_Amount = new Money(this.amount)
                                    };

                                    XrmHelper.Update(localContext.OrganizationService, updateValueCode);
                                    _log.Info($"Th={threadId} - UpdateValueCodeInCRM: New value (after activate - Amount: '{valueCode?.ed_Amount.Value}'");
                                }
                                else
                                {
                                    var updateValueCode = new ValueCodeEntity()
                                    {
                                        Id = valueCode.Id,
                                        ed_Amount = new Money(this.amount)
                                    };

                                    XrmHelper.Update(localContext.OrganizationService, updateValueCode);
                                    _log.Info($"Th={threadId} - UpdateValueCodeInCRM: Value Code amount updated. New value - Amount: '{valueCode?.ed_Amount.Value}'");
                                }
                            }

                            #endregion
                        }

                        #region Create Value Code Transaction row

                        CreateValueCodeTransaction(threadId, localContext, valueCode);

                        #endregion
                    }
                    else
                    {
                        _log.Info($"Th={threadId} - UpdateValueCodeInCRM: Not using ed_RemoveControlForTypeOfValueCodeEnabled FeatureToggle.");
                        if (valueCode == null)
                        {
                            _log.Info($"Th={threadId} - UpdateValueCodeInCRM: Could not find value code '{this.voucherCode}'. Creating a new value code (Should be type 5).");

                            ValueCodeEntity newValueCode = new ValueCodeEntity()
                            {
                                ed_name = voucherId.ToString(),
                                ed_Amount = new Money(amount),
                                ed_CreatedTimestamp = created,
                                ed_LastRedemptionDate = validToDate,
                                ed_ValidUntil = validToDate,
                                ed_CodeId = voucherCode,
                                ed_Ean = eanCode?.ToString(),
                                ed_OriginalAmount = amount,
                                ed_ValueCodeVoucherId = voucherId.ToString()
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
                            _log.Info($"Th={threadId} - UpdateValueCodeInCRM: ValueCode created with Id - {newValueCode.Id}.");

                            //Bellow changed with the inclusion of ed_ValueCOdeVoucherId field 05-11-20
                            if (String.IsNullOrWhiteSpace(newValueCode.ed_name))
                            {
                                newValueCode.ed_name = newValueCode.Id.ToString();
                                XrmHelper.Update(localContext, newValueCode);
                                _log.Debug($"Th={threadId} - UpdateValueCodeInCRM: ValueCode name updated.");
                            }

                            //_log.Debug($"Creating value code type 5 (Presentkort) with values:");
                            //_log.Debug($"{ValueCodeEntity.Fields.Id}: '{newValueCode.Id}', " +
                            //    $"{ValueCodeEntity.Fields.ed_Amount}: '{newValueCode.ed_Amount}', " +
                            //    $"{ValueCodeEntity.Fields.ed_CreatedTimestamp}: '{newValueCode.ed_CreatedTimestamp ?? DateTime.MinValue}', " +
                            //    $"{ValueCodeEntity.Fields.ed_LastRedemptionDate}: '{newValueCode.ed_LastRedemptionDate ?? DateTime.MinValue}', " +
                            //    $"{ValueCodeEntity.Fields.ed_CodeId}: '{newValueCode.ed_CodeId}', " +
                            //    $"{ValueCodeEntity.Fields.ed_Ean}: '{newValueCode.ed_Ean}', " +
                            //    $"{ValueCodeEntity.Fields.ed_OriginalAmount}: '{newValueCode.ed_OriginalAmount ?? -1}'");

                            //Handle updates where ValueCode has been canceled by Voucher Service (status 4 = Canceled) 29/10-20
                            if (this.status == 4)
                            {
                                var updateValueCode = new ValueCodeEntity()
                                {
                                    Id = newValueCode.Id,
                                    ed_Amount = new Money(this.amount),
                                    ed_RedemptionDate = redeemed,
                                    ed_CanceledOn = (DateTime?)DateTime.UtcNow
                                };

                                UpdateValueCodeRecordAndCancel(localContext, updateValueCode);
                                _log.Info($"Th={threadId} - UpdateValueCodeInCRM: ValueCode with Id - {newValueCode.Id} - Cancelled.");
                            }
                            else if (this.amount <= 0)
                            {
                                newValueCode.ed_RedemptionDate = redeemed;
                                UpdateValueCodeRecordAndDeactivate(localContext, newValueCode);
                                _log.Info($"Th={threadId} - UpdateValueCodeInCRM: ValueCode with Id - {newValueCode.Id} - Deactivated.");
                            }
                            else
                            {
                                _log.Debug($"Th={threadId} - UpdateValueCodeInCRM: Updating value code value.");

                                XrmHelper.Update(localContext.OrganizationService, newValueCode);

                                _log.Info($"Th={threadId} - UpdateValueCodeInCRM: New ValueCode Amount - '{newValueCode?.ed_Amount.Value}'");
                            }

                        }
                        // Presentkort
                        else if ((int)valueCode.ed_ValueCodeTypeGlobal.Value == 2)
                        {
                            _log.Info($"Th={threadId} - UpdateValueCodeInCRM: Value code '{this.voucherCode}'. ValueCodeTypeGlobal == 2.");
                            //Handle updates where ValueCode has been canceled by Voucher Service (status 4 = Canceled) 29/10-20
                            if (this.status == 4)
                            {
                                var updateValueCode = new ValueCodeEntity()
                                {
                                    Id = valueCode.Id,
                                    ed_Amount = new Money(this.amount),
                                    ed_RedemptionDate = redeemed,
                                    ed_CanceledOn = (DateTime?)DateTime.UtcNow
                                };

                                UpdateValueCodeRecordAndCancel(localContext, updateValueCode);
                                _log.Info($"Th={threadId} - UpdateValueCodeInCRM: ValueCode with Id - {valueCode.Id} - Cancelled.");
                            }
                            else if (this.amount <= 0)
                            {
                                valueCode.ed_Amount = new Money(this.amount);
                                valueCode.ed_RedemptionDate = redeemed;

                                UpdateValueCodeRecordAndDeactivate(localContext, valueCode);
                                _log.Info($"Th={threadId} - UpdateValueCodeInCRM: ValueCode with Id - {valueCode.Id} - Deactivated.");

                                
                                SetStateRequest req = new SetStateRequest()
                                {
                                    EntityMoniker = valueCode.ToEntityReference(),
                                    State = new OptionSetValue((int)Generated.ed_ValueCodeState.Inactive),
                                    Status = new OptionSetValue((int)ValueCodeEntity.Status.Inlost)
                                };
                                SetStateResponse resp = (SetStateResponse)localContext.OrganizationService.Execute(req);

                                _log.Info($"Th={threadId} - UpdateValueCodeInCRM: Updating value code status to 'Inlost'.");
                                // TODO : Marcus Generate
                                //_log.Debug($"New status - State: '{Generated.ed_ValueCodeState.Inactive}', Status: '{Generated.ed_valuecodes.Inlost}'");
                            }
                            else
                            {
                                valueCode.ed_Amount = new Money(this.amount);

                                XrmHelper.Update(localContext.OrganizationService, valueCode);
                                _log.Info($"Th={threadId} - UpdateValueCodeInCRM: Updating value code value. New value - Amount: '{valueCode?.ed_Amount.Value}'");
                            }
                        }
                        // Övriga
                        else if ((int)valueCode.ed_ValueCodeTypeGlobal.Value != 2)
                        {
                            _log.Info($"Th={threadId} - UpdateValueCodeInCRM: Value code '{this.voucherCode}'. ValueCodeTypeGlobal != 2.");
                            //DevOps Task: 3998
                            //Handle updates where ValueCode has been canceled by Voucher Service(status 4 = Canceled) 29 / 10 - 20
                            if (this.status == 4)
                            {
                                var updateValueCode = new ValueCodeEntity()
                                {
                                    Id = valueCode.Id,
                                    ed_Amount = new Money(this.amount),
                                    ed_RedemptionDate = redeemed,
                                    ed_CanceledOn = (DateTime?)DateTime.UtcNow
                                };

                                UpdateValueCodeRecordAndCancel(localContext, updateValueCode);
                                _log.Info($"Th={threadId} - UpdateValueCodeInCRM: ValueCode with Id - {valueCode.Id} - Cancelled.");
                            }
                            else if (this.amount > 0
                                && (int)valueCode.ed_ValueCodeTypeGlobal.Value != 1
                                && (int)valueCode.ed_ValueCodeTypeGlobal.Value != 3
                                && (int)valueCode.ed_ValueCodeTypeGlobal.Value != 4) //This should be modified according to new DevOps Task: 3888
                            {
                                valueCode.ed_Amount = new Money(this.amount);
                                XrmHelper.Update(localContext, valueCode);
                                _log.Info($"Th={threadId} - UpdateValueCodeInCRM: Value Code amount updated.");
                            }
                            else
                            {
                                valueCode.ed_Amount = new Money(0);
                                valueCode.ed_RedemptionDate = redeemed;
                                UpdateValueCodeRecordAndDeactivate(localContext, valueCode);
                                _log.Info($"Th={threadId} - UpdateValueCodeInCRM: ValueCode with Id - {valueCode.Id} - Deactivated.");
                            }
                        }

                        CreateValueCodeTransaction(threadId, localContext, valueCode);
                    }

                    _log.Debug($"Exiting UpdateValueCodeInCRM.");
                }
            }
            catch (Exception ex)
            {
                _log.ErrorFormat($"Th={threadId} - Error on updating ValueCode: " + ex);
                throw ex;
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="threadId"></param>
        /// <param name="localContext"></param>
        /// <param name="valueCode"></param>
        private ValueCodeTransactionEntity CreateValueCodeTransaction(int threadId, Plugin.LocalPluginContext localContext, ValueCodeEntity valueCode)
        {
            _log.Debug($"Th={threadId} ---> Entering {nameof(CreateValueCodeTransaction)}");

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
                _log.Debug($"Th={threadId} ---> ValueCodeTransaction with Id - {valueCodeTransaction.Id}");

                _log.Debug($"Th={threadId} <--- Exiting {nameof(CreateValueCodeTransaction)}");
                return valueCodeTransaction;
            }
            catch (Exception ex)
            {
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="valueCode"></param>
        private void UpdateValueCodeRecordAndCancel(Plugin.LocalPluginContext localContext, ValueCodeEntity valueCode)
        {
            XrmHelper.Update(localContext.OrganizationService, valueCode);
            _log.Debug($"Updating value code values.");
            _log.Debug($"New values - Amount: '{valueCode?.ed_Amount.Value}', RedemptionDate: {valueCode?.ed_RedemptionDate}");

            _log.Debug($"Updating value code status.");
            SetStateRequest req = new SetStateRequest()
            {
                EntityMoniker = valueCode.ToEntityReference(),
                State = new OptionSetValue((int)Generated.ed_ValueCodeState.Inactive),
                Status = new OptionSetValue((int)ValueCodeEntity.Status.Makulerad)
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
