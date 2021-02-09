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

    public class Datum
    {
        public int id { get; set; }
        public string description { get; set; }
        public string closeDate { get; set; }
        public string date { get; set; }
        public string notes { get; set; }
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
    }

    public class OrderMQInfo
    {
        public object error { get; set; }
        public Metadata metadata { get; set; }
        public IList<Datum> data { get; set; }
    }
}