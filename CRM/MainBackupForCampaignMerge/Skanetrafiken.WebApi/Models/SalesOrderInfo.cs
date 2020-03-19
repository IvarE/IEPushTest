
using System;

namespace Skanetrafiken.Crm
{
    public class SalesOrderInfo
    {
        private string orderNoField;

        private string businessUnitField;

        private DateTime orderTimeField;

        private string guidField;

        private CustomerInfo customerField;

        private Productinfo[] productinfosField;
        
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
}
