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
        public int limit { get; set; }
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

        internal static List<OrderRow> GetOrderProductsFromOrder(Plugin.LocalPluginContext localContext, Guid orderId)
        {
            if (orderId == null)
                return new List<OrderRow>();

            QueryExpression queryOrderProducts = new QueryExpression(OrderProductEntity.EntityLogicalName);
            queryOrderProducts.NoLock = true;
            queryOrderProducts.ColumnSet.AddColumns(OrderProductEntity.Fields.SalesOrderDetailId);
            queryOrderProducts.Criteria.AddCondition(OrderProductEntity.Fields.SalesOrderId, ConditionOperator.Equal, orderId);

            List<OrderProductEntity> lOrderProducts = XrmRetrieveHelper.RetrieveMultiple<OrderProductEntity>(localContext, queryOrderProducts);

            //TODO CREATE NEW LIST OF ORDER ROWS

            return new List<OrderRow>();
        }

        internal static OrderMQ GetOrderMQInfoFromOrderEntity(Plugin.LocalPluginContext localContext, OrderEntity orderCRM)
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

            if (orderCRM.ed_DeliveryReportStatus != null && orderCRM.ed_DeliveryReportStatus.Value == ed_deliveryreportstatus.Createduploaded)
                orderMQ.deliveryReportCreated = true;
            else if (orderCRM.ed_DeliveryReportStatus != null && (orderCRM.ed_DeliveryReportStatus.Value == ed_deliveryreportstatus.Notcreated ||
                                                                orderCRM.ed_DeliveryReportStatus.Value == ed_deliveryreportstatus.Creatednotuploaded))
                orderMQ.deliveryReportCreated = false;

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

            if (erCustomer != null && erCustomer.LogicalName == AccountEntity.EntityLogicalName)
            {
                Client clientMQ = new Client();
                clientMQ.name = erCustomer.Name;

                if (userMQ != null)
                    clientMQ.users.Add(userMQ);
            }

            orderMQ.contact = null;
            orderMQ.project = null;
            orderMQ.regDate = now;

            orderMQ.stage = null;
            orderMQ.probability = (int)orderCRM.ed_Probability;
            orderMQ.modDate = now;
            orderMQ.clientConnection = null;
            orderMQ.currencyRate = 1;
            orderMQ.currency = "SEK";
            orderMQ.locked = 0;

            Custom custom = new Custom();

            string endDate = orderCRM.ed_campaigndateend != null ? orderCRM.ed_campaigndateend.Value.ToString(pattern) : null;
            custom.value = endDate;
            custom.valueDate = endDate;
            custom.fieldId = 2; //End Date

            orderMQ.custom.Add(custom);

            custom = new Custom();

            string startDate = orderCRM.ed_campaigndatestart != null ? orderCRM.ed_campaigndatestart.Value.ToString(pattern) : null;
            custom.value = startDate;
            custom.valueDate = startDate;
            custom.fieldId = 1; //Start Date

            orderMQ.custom.Add(custom);

            orderMQ.value = int.MinValue;
            orderMQ.weightedValue = int.MinValue;
            orderMQ.valueInMasterCurrency = int.MinValue;
            orderMQ.weightedValueInMasterCurrency = int.MinValue;
            orderMQ.agreement = null;
            orderMQ.userRemovable = true;
            orderMQ.userEditable = true;

            //The Products are missing TODO
            List<OrderRow> lOrderProducts = GetOrderProductsFromOrder(localContext, orderCRM.Id);
            orderMQ.orderRow = lOrderProducts;

            return orderMQ;
        }
    }

    public class OrderMQInfo
    {
        public object error { get; set; }
        public Metadata metadata { get; set; }
        public IList<OrderMQ> data { get; set; }
    }

}