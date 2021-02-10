
using System;
using Endeavor.Crm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json;
using Skanetrafiken.Crm.Entities;
using Skanetrafiken.Crm.Models;
using Generated = Skanetrafiken.Crm.Schema.Generated;

namespace Skanetrafiken.Crm
{
    public class SalesOrderInfo
    {
        private int informationSourceField;

        private string orderNoField;

        private string businessUnitField;

        private DateTime orderTimeField;

        private string guidField;

        private string portalIdField;
                
        private CustomerInfo customerField;

        private Productinfo[] productinfosField;

        private SalesOrderLineInfo[] salesOrderLinesField;

        public AddressBlock AddressBlock { get; set; }

        #region Köp och Skicka
        [JsonProperty(PropertyName = "ServiceType")]
        private int ServiceType { get { return informationSourceField; } set { InformationSource = value; } }
        [JsonProperty(PropertyName = "OrderId")]
        private int OrderId
        {
            get
            {
                if (int.TryParse(orderNoField, out int res))
                    return res;
                return -1;
            }
            set { OrderNo = value.ToString(); }
        }
        [JsonProperty(PropertyName = "Created")]
        private DateTime Created { get { return OrderTime; } set { OrderTime = value; } }
        public string ContactGuid { get; set; }
        #endregion

        public int InformationSource
        {
            get
            {
                return this.informationSourceField;
            }
            set
            {
                this.informationSourceField = value;
            }
        }

        public string OrderNo
        {
            get
            {
                return this.orderNoField;
            }
            set
            {
                this.orderNoField = value;
            }
        }
        
        public string BusinessUnit
        {
            get
            {
                return this.businessUnitField;
            }
            set
            {
                this.businessUnitField = value;
            }
        }
        
        public DateTime OrderTime
        {
            get
            {
                return this.orderTimeField;
            }
            set
            {
                this.orderTimeField = value;
            }
        }

        public string Guid
        {
            get
            {
                return this.guidField;
            }
            set
            {
                this.guidField = value;
            }
        }

        public string PortalId
        {
            get
            {
                return this.portalIdField;
            }
            set
            {
                this.portalIdField = value;
            }
        }

        public CustomerInfo Customer
        {
            get
            {
                return this.customerField;
            }
            set
            {
                this.customerField = value;
            }
        }
        
        [System.Xml.Serialization.XmlElementAttribute("Productinfo")]
        public Productinfo[] Productinfos
        {
            get
            {
                return this.productinfosField;
            }
            set
            {
                this.productinfosField = value;
            }
        }

        public SalesOrderLineInfo[] SalesOrderLines
        {
            get
            {
                return this.salesOrderLinesField;
            }
            set
            {
                this.salesOrderLinesField = value;
            }
        }

        internal static SalesOrderEntity GetSalesOrderEntityFromSalesOrderInfo(Plugin.LocalPluginContext localContext, SalesOrderInfo salesOrderInfo)
        {

            SalesOrderEntity soe = new SalesOrderEntity();

            soe.ed_Name = $"Order - {salesOrderInfo.OrderNo}";
            soe.ed_OrderNo = salesOrderInfo.OrderNo;
            soe.ed_OrderPlacedOn = salesOrderInfo.OrderTime;

            AccountEntity account = XrmRetrieveHelper.RetrieveFirst<AccountEntity>(localContext, new ColumnSet(false),
                                new FilterExpression()
                                {
                                    Conditions =
                                    {
                                new ConditionExpression(AccountEntity.Fields.AccountNumber, ConditionOperator.Equal, salesOrderInfo.PortalId)
                                    }
                                });

            if (account != null)
            {
                soe.ed_AccountId = account.ToEntityReference();
            }

            CompanyRoleEntity companyRole = null;
            if (account != null && salesOrderInfo.Customer != null)
            {
                if (salesOrderInfo.Customer.SocialSecurityNumber != null)
                {
                    companyRole = XrmRetrieveHelper.RetrieveFirst<CompanyRoleEntity>(localContext, new ColumnSet(false),
                        new FilterExpression()
                        {
                            Conditions =
                            {
                                new ConditionExpression(CompanyRoleEntity.Fields.ed_SocialSecurityNumber, ConditionOperator.Equal, salesOrderInfo.Customer.SocialSecurityNumber),
                                new ConditionExpression(CompanyRoleEntity.Fields.ed_Account, ConditionOperator.Equal, account.Id)
                            }
                        });
                }

                if (companyRole != null)
                {
                    soe.ed_CompanyRoleId = companyRole.ToEntityReference();
                }
            }

            return soe;
        }

        internal static SalesOrderEntity GetSalesOrderEntityFromKopAndSkicka(Plugin.LocalPluginContext localContext, SalesOrderInfo salesOrderInfo, bool isPut)
        {

            SalesOrderEntity soe = new SalesOrderEntity();
            soe.ed_Name = $"{salesOrderInfo.OrderNo}"; 
            soe.ed_OrderPlacedOn = salesOrderInfo.OrderTime;
            soe.ed_informationsource = Generated.ed_informationsource.KopOchSkicka;

            if(!isPut)
                soe.ed_OrderNo = salesOrderInfo.OrderNo;

            return soe;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="orderId">OrderId matches OrderNo from Buy and Send body.</param>
        /// <returns></returns>
        internal static SalesOrderEntity GetSalesOrderEntity(Plugin.LocalPluginContext localContext, int orderId)
        {

            var query = new QueryExpression()
            {
                EntityName = SalesOrderEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(
                    SalesOrderEntity.Fields.ed_OrderNo,
                    SalesOrderEntity.Fields.ed_Ordertype,
                    SalesOrderEntity.Fields.ed_OrderPlacedOn,
                    SalesOrderEntity.Fields.ed_ContactId),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SalesOrderEntity.Fields.ed_OrderNo, ConditionOperator.Equal, orderId)
                    }
                }
            };

            var salesOrder = XrmRetrieveHelper.RetrieveFirst<SalesOrderEntity>(localContext, query);
            if (salesOrder != null)
            {
                
            }

            return null;
        }
    }


    public class Productinfo
    {

        private string referenceField;

        private int? productCodeField;

        private string nameField;

        private int qtyField;

        private bool qtyFieldSpecified;

        private string nameOnCardField;

        private string serialField;

        /// <remarks/>
        public string Reference
        {
            get
            {
                return this.referenceField;
            }
            set
            {
                this.referenceField = value;
            }
        }

        /// <remarks/>
        public int? ProductCode
        {
            get
            {
                return this.productCodeField;
            }
            set
            {
                this.productCodeField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public int Qty
        {
            get
            {
                return this.qtyField;
            }
            set
            {
                this.qtyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool QtySpecified
        {
            get
            {
                return this.qtyFieldSpecified;
            }
            set
            {
                this.qtyFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string NameOnCard
        {
            get
            {
                return this.nameOnCardField;
            }
            set
            {
                this.nameOnCardField = value;
            }
        }

        /// <remarks/>
        public string Serial
        {
            get
            {
                return this.serialField;
            }
            set
            {
                this.serialField = value;
            }
        }
    }

    public class SalesOrderLineInfo
    {

        private string portalIdField;

        private decimal amountField;

        private string commentField;

        private string journeyField;

        private string recipientField;

        private string ticketIdField;

        private int ticketOfferTypeField;

        private string ticketOfferTypeNameField;

        private string OrderNoField;

        private string OrderLineNoField;

        private SalesOrderLineTravellerInfo[] salesOrderLineTravellers;

        /// <remarks/>
        public string PortalId
        {
            get
            {
                return this.portalIdField;
            }
            set
            {
                this.portalIdField = value;
            }
        }

        /// <remarks/>
        public string OrderNo
        {
            get
            {
                return this.OrderNoField;
            }
            set
            {
                this.OrderNoField = value;
            }
        }

        /// <remarks/>
        public string OrderLineNo
        {
            get
            {
                return this.OrderLineNoField;
            }
            set
            {
                this.OrderLineNoField = value;
            }
        }

        /// <remarks/>
        public decimal Amount
        {
            get
            {
                return this.amountField;
            }
            set
            {
                this.amountField = value;
            }
        }

        /// <remarks/>
        public string Comment
        {
            get
            {
                return this.commentField;
            }
            set
            {
                this.commentField = value;
            }
        }

        /// <remarks/>
        public string Journey
        {
            get
            {
                return this.journeyField;
            }
            set
            {
                this.journeyField = value;
            }
        }

        /// <remarks/>
        public string Recipient
        {
            get
            {
                return this.recipientField;
            }
            set
            {
                this.recipientField = value;
            }
        }

        /// <remarks/>
        public string TicketId
        {
            get
            {
                return this.ticketIdField;
            }
            set
            {
                this.ticketIdField = value;
            }
        }

        /// <remarks/>
        public int TicketOfferType
        {
            get
            {
                return this.ticketOfferTypeField;
            }
            set
            {
                this.ticketOfferTypeField = value;
            }
        }

        /// <remarks/>
        public string TicketOfferTypeName
        {
            get
            {
                return this.ticketOfferTypeNameField;
            }
            set
            {
                this.ticketOfferTypeNameField = value;
            }
        }

        public int OrderId { get; set; }
        /// <summary>
        /// OrderLineId from Buy and Send, not primary key in CRM.
        /// </summary>
        public int OrderLineId { get; set; }
        public string Status { get; set; }
        public string CardNumber { get; set; }

        /// <remarks/>
        public SalesOrderLineTravellerInfo[] SalesOrderLineTravellers
        {
            get
            {
                return this.salesOrderLineTravellers;
            }
            set
            {
                this.salesOrderLineTravellers = value;
            }
        }

        internal static SalesOrderLineEntity GetSalesOrderLineEntityFromSalesOrderLineInfo(Plugin.LocalPluginContext localContext, SalesOrderLineInfo salesOrderLineInfo)
        {

            SalesOrderLineEntity sole = new SalesOrderLineEntity();

            sole.ed_name = $"Orderrad - {salesOrderLineInfo.PortalId}";
            sole.ed_SalesOrderLinePortalId = salesOrderLineInfo.portalIdField;
            sole.ed_Amount = salesOrderLineInfo.amountField;
            sole.ed_Comment = salesOrderLineInfo.commentField;
            sole.ed_Journey = salesOrderLineInfo.journeyField;
            sole.ed_Recipient = salesOrderLineInfo.recipientField;
            sole.ed_TicketId = salesOrderLineInfo.ticketIdField;
            sole.ed_TicketOfferType = salesOrderLineInfo.ticketOfferTypeField;
            sole.ed_TicketOfferTypeName = salesOrderLineInfo.ticketOfferTypeNameField;

            return sole;
        }

        internal static SalesOrderLineEntity GetSalesOrderLineEntityFromKopOchSkicka(Plugin.LocalPluginContext localContext, SalesOrderLineInfo salesOrderLineInfo, 
            EntityReference salesOrderId, EntityReference orderStatus, EntityReference skaKort)
        {
            SalesOrderLineEntity salesOrderLine = new SalesOrderLineEntity()
            {
                ed_name = $"{salesOrderLineInfo.OrderLineNo} - Order ({salesOrderLineInfo.OrderNo})",
                ed_SalesOrderId = salesOrderId,
                st_SalesOrderLineID = salesOrderLineInfo.OrderLineNo,
                ed_OrderStatus = orderStatus,
                ed_SKAkort = skaKort
            };

            return salesOrderLine;
        }
    }

    public class SalesOrderLineTravellerInfo
    {
        private string portalIdField;

        private int countField;

        private int travellerTypeField;

        private string travellerTypeNameField;

        public string PortalId
        {
            get
            {
                return this.portalIdField;
            }
            set
            {
                this.portalIdField = value;
            }
        }

        public int Count
        {
            get
            {
                return this.countField;
            }
            set
            {
                this.countField = value;
            }
        }

        public int TravellerType
        {
            get
            {
                return this.travellerTypeField;
            }
            set
            {
                this.travellerTypeField = value;
            }
        }

        public string TravellerTypeName
        {
            get
            {
                return this.travellerTypeNameField;
            }
            set
            {
                this.travellerTypeNameField = value;
            }
        }

        internal static SalesOrderLineTravellerEntity GetTravellerEntityFromTravellerInfo(Plugin.LocalPluginContext localContext, SalesOrderLineTravellerInfo travellerInfo)
        {

            SalesOrderLineTravellerEntity solte = new SalesOrderLineTravellerEntity();

            solte.ed_name = $"Resenär - {travellerInfo.PortalId}";
            solte.ed_SalesOrderLineTravellerPortalId = travellerInfo.portalIdField;
            solte.ed_Count = travellerInfo.Count;
            solte.ed_TravellerType = travellerInfo.TravellerType;
            solte.ed_TravellerTypeName = travellerInfo.TravellerTypeName;

            return solte;
        }
    }
}
