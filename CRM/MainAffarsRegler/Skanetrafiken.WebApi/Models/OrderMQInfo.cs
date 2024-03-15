using DocumentFormat.OpenXml.EMMA;
using Endeavor.Crm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Skanetrafiken.Crm.Entities;
using Skanetrafiken.Crm.Schema.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Skanetrafiken.Crm.Models
{
    public class Metadata
    {
        public int total { get; set; }
        public int? limit { get; set; }
        public int offset { get; set; }
    }

    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public object role { get; set; }
        public string email { get; set; }
    }

    public class Client
    {
        public string name { get; set; }
        public int id { get; set; }
        public IList<User> users { get; set; }
    }

    public class Stage
    {
        public string name { get; set; }
        public int id { get; set; }
    }

    public class Custom
    {
        public string value { get; set; }
        public string valueDate { get; set; }
        public int orgNumber { get; set; }
        public int fieldId { get; set; }
    }

    public class Product
    {
        public string name { get; set; }
        public int id { get; set; }
        public object category { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
    }

    public class OrderRow
    {
        public int id { get; set; }
        public int quantity { get; set; }
        public int price { get; set; }
        public int discount { get; set; }
        public IList<object> custom { get; set; }
        public int productId { get; set; }
        public int sortId { get; set; }
        public int listPrice { get; set; }
        public Product product { get; set; }
    }

    public class OrderMQ
    {
        public string id { get; set; }
        public string description { get; set; }
        public string closeDate { get; set; }
        public string date { get; set; }
        public string notes { get; set; }
        public bool deliveryReportCreated { get; set; }
        public User user { get; set; }
        public Client client { get; set; }
        public object contact { get; set; }
        public object project { get; set; }
        public DateTime regDate { get; set; }
        public Stage stage { get; set; }
        public int probability { get; set; }
        public DateTime modDate { get; set; }
        public object clientConnection { get; set; }
        public int currencyRate { get; set; }
        public string currency { get; set; }
        public int locked { get; set; }
        public IList<Custom> custom { get; set; }
        public IList<OrderRow> orderRow { get; set; }
        public int value { get; set; }
        public int weightedValue { get; set; }
        public int valueInMasterCurrency { get; set; }
        public int weightedValueInMasterCurrency { get; set; }
        public object agreement { get; set; }
        public bool userRemovable { get; set; }
        public bool userEditable { get; set; }

        internal static bool CheckIfOrderAlreadyHasFile(Plugin.LocalPluginContext localContext, Guid orderId, string orderNumber)
        {
            bool hasFile = false;

            QueryExpression queryAnnotation = new QueryExpression(Annotation.EntityLogicalName);
            queryAnnotation.NoLock = true;
            queryAnnotation.Criteria.AddCondition(Annotation.Fields.Subject, ConditionOperator.Like, "%" + orderNumber + "%");

            LinkEntity link_salesorder = queryAnnotation.AddLink(OrderEntity.EntityLogicalName, Annotation.Fields.ObjectId, OrderEntity.Fields.SalesOrderId);
            link_salesorder.EntityAlias = "ac";
            link_salesorder.LinkCriteria.AddCondition(OrderEntity.Fields.SalesOrderId, ConditionOperator.Equal, orderId);

            List<Annotation> lAnnotations = XrmRetrieveHelper.RetrieveMultiple<Annotation>(localContext, queryAnnotation);

            if (lAnnotations.Count == 1)
                return !hasFile;

            return hasFile;
        }

        internal static List<OrderRow> GetOrderProductsFromOrder(Plugin.LocalPluginContext localContext, Guid orderId, string pattern)
        {
            if (orderId == null)
                return new List<OrderRow>();

            QueryExpression queryOrderProducts = new QueryExpression(OrderProductEntity.EntityLogicalName);
            queryOrderProducts.NoLock = true;
            queryOrderProducts.ColumnSet.AddColumns(OrderProductEntity.Fields.SalesOrderDetailId, OrderProductEntity.Fields.ProductId,
                OrderProductEntity.Fields.ed_FromDate, OrderProductEntity.Fields.ed_ToDate);
            queryOrderProducts.Criteria.AddCondition(OrderProductEntity.Fields.SalesOrderId, ConditionOperator.Equal, orderId);


            List<OrderRow> lOrderRows = new List<OrderRow>();
            List<OrderProductEntity> lOrderProducts = XrmRetrieveHelper.RetrieveMultiple<OrderProductEntity>(localContext, queryOrderProducts);

            foreach (OrderProductEntity orderProduct in lOrderProducts)
            {
                OrderRow orderRow = new OrderRow();

                orderRow.id = 1;
                orderRow.quantity = 1;
                orderRow.discount = 0;
                orderRow.custom = new List<object>();
                orderRow.productId = 1;
                orderRow.sortId = 1;
                orderRow.listPrice = 10000;

                Product productObject = new Product();
                productObject.name = orderProduct.ProductId?.Name;
                productObject.id = 1;
                productObject.category = null;

                if (orderProduct.ed_FromDate != null)
                    productObject.startDate = orderProduct.ed_FromDate?.ToString(pattern);
                else
                    productObject.startDate = "2020-01-01";

                if (orderProduct.ed_ToDate != null)
                    productObject.endDate = orderProduct.ed_ToDate?.ToString(pattern);
                else
                    productObject.endDate = "2020-12-31";

                orderRow.product = productObject;
                lOrderRows.Add(orderRow);
            }


            return lOrderRows;
        }

        internal static OrderMQ GetOrderMQInfoFromOrderEntity(Plugin.LocalPluginContext localContext, OrderEntity orderCRM)
        {
            using (var _logger = new AppInsightsLogger())
            {

                DateTime now = DateTime.Now;
                string pattern = "yyyy-MM-dd";
                string dateNow = DateTime.Now.ToString(pattern);

                OrderMQ orderMQ = new OrderMQ();
                orderMQ.id = orderCRM.OrderNumber;
                orderMQ.description = orderCRM.Name;
                orderMQ.closeDate = dateNow;
                orderMQ.date = dateNow;
                orderMQ.notes = "";

                bool hasFile = CheckIfOrderAlreadyHasFile(localContext, (Guid)orderCRM.SalesOrderId, orderCRM.OrderNumber);

                if (hasFile)
                {
                    orderMQ.deliveryReportCreated = true;
                }
                else
                {
                    orderMQ.deliveryReportCreated = false;
                }

                EntityReference erOwner = orderCRM.OwnerId;

                User userMQ = null;

                if (erOwner != null)
                {
                    SystemUserEntity erUser = XrmRetrieveHelper.Retrieve<SystemUserEntity>(localContext, erOwner, new ColumnSet(SystemUserEntity.Fields.FullName, SystemUserEntity.Fields.InternalEMailAddress));

                    userMQ = new User();
                    userMQ.name = erUser.FullName;
                    userMQ.email = erUser.InternalEMailAddress;

                    orderMQ.user = userMQ;
                }

                EntityReference erCustomer = orderCRM.CustomerId;

                try
                {
                    if (erCustomer != null && erCustomer.LogicalName == AccountEntity.EntityLogicalName)
                    {
                        Client clientMQ = new Client();
                        clientMQ.name = erCustomer.Name;

                        if (userMQ != null)
                        {
                            clientMQ.users = new List<User>();
                            clientMQ.users.Add(userMQ);
                        }

                        orderMQ.client = clientMQ;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"GetOrderMQInfoFromOrderEntity: Error adding ClientMQ. Ex: {ex.Message}");
                }

                orderMQ.contact = null;
                orderMQ.project = null;
                orderMQ.regDate = now;

                orderMQ.stage = null;
                orderMQ.probability = orderCRM.ed_Probability != null ? (int)orderCRM.ed_Probability : int.MinValue;
                orderMQ.modDate = now;
                orderMQ.clientConnection = null;
                orderMQ.currencyRate = 1;
                orderMQ.currency = "SEK";
                orderMQ.locked = 0;


                Custom custom = new Custom();

                try
                {
                    string endDate = orderCRM.ed_campaigndateend != null ? orderCRM.ed_campaigndateend.Value.ToString(pattern) : null;
                    custom.value = endDate;
                    custom.valueDate = endDate;
                    custom.fieldId = 2; //End Date

                    orderMQ.custom = new List<Custom>();
                    orderMQ.custom.Add(custom);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"GetOrderMQInfoFromOrderEntity: Error from custom object 1. Ex: {ex.Message}");
                }

                custom = new Custom();

                try
                {
                    string startDate = orderCRM.ed_campaigndatestart != null ? orderCRM.ed_campaigndatestart.Value.ToString(pattern) : null;
                    custom.value = startDate;
                    custom.valueDate = startDate;
                    custom.fieldId = 1; //Start Date

                    orderMQ.custom.Add(custom);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"GetOrderMQInfoFromOrderEntity: Error from custom object 2. Ex: {ex.Message}");
                }

                try
                {
                    orderMQ.value = int.MinValue;
                    orderMQ.weightedValue = int.MinValue;
                    orderMQ.valueInMasterCurrency = int.MinValue;
                    orderMQ.weightedValueInMasterCurrency = int.MinValue;
                    orderMQ.agreement = null;
                    orderMQ.userRemovable = true;
                    orderMQ.userEditable = true;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"GetOrderMQInfoFromOrderEntity: Error from other values. Ex: {ex.Message}");
                }

                try
                {
                    List<OrderRow> lOrderProducts = GetOrderProductsFromOrder(localContext, orderCRM.Id, pattern);
                    orderMQ.orderRow = lOrderProducts;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"GetOrderMQInfoFromOrderEntity: Error when adding OrderProducts. Ex: {ex.Message}");
                }

                return orderMQ;
            }
        }
    }

    public class OrderMQInfo
    {
        public object error { get; set; }
        public Metadata metadata { get; set; }
        public IList<OrderMQ> data { get; set; }
    }

}