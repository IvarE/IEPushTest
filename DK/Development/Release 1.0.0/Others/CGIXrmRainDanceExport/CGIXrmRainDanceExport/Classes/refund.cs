using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CGIXrmWin;
using Microsoft.Crm;
using Microsoft.Xrm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using System.Configuration;
using System.ServiceModel;

namespace CGIXrmRainDanceExport
{
    public class refund : XrmBaseEntity
    {
        //"<attribute name='cgi_refundid' />";
        private Guid _refundid;
        [Xrm("cgi_refundid")]
        public Guid Refundid
        {
            get { return _refundid; }
            set { _refundid = value; }
        }

        //"<attribute name='cgi_refundnumber' />";
        private string _refundnumber;
        [Xrm("cgi_refundnumber")]
        public string Refundnumber
        {
            get { return _refundnumber; }
            set { _refundnumber = value; }
        }

        ////"<attribute name='createdon' />";
        //private DateTime _createdon;
        //[Xrm("createdon")]
        //public DateTime Createdon
        //{
        //    get { return _createdon; }
        //    set { _createdon = value; }
        //}

        //"<attribute name='cgi_vat_code' />";
        private int _vat_code;
        [Xrm("cgi_vat_code", DecodePart=XrmDecode.Value)]
        public int Vat_code
        {
            get { return _vat_code; }
            set { _vat_code = value; }
        }

        private string _vat_code_name;
        [Xrm("cgi_vat_code", DecodePart = XrmDecode.Name)]
        public string Vat_code_name
        {
            get { return _vat_code_name; }
            set { _vat_code_name = value; }
        }

        //"<attribute name='cgi_value_code' />";
        private string _value_code;
        [Xrm("cgi_value_code")]
        public string Value_code
        {
            get { return _value_code; }
            set { _value_code = value; }
        }

        //"<attribute name='cgi_travelcard_number' />";
        private string _travelcard_number;
        [Xrm("cgi_travelcard_number")]
        public string Travelcard_number
        {
            get { return _travelcard_number; }
            set { _travelcard_number = value; }
        }

        //"<attribute name='cgi_transportcompanyid' />";
        private Guid? _transportcompanyid;
        [Xrm("cgi_transportcompanyid", DecodePart = XrmDecode.Value)]
        public Guid? Transportcompanyid
        {
            get { return _transportcompanyid; }
            set { _transportcompanyid = value; }
        }

        //"<attribute name='cgi_taxi_company' />";
        private string _taxi_company;
        [Xrm("cgi_taxi_company")]
        public string Taxi_company
        {
            get { return _taxi_company; }
            set { _taxi_company = value; }
        }

        //"<attribute name='cgi_swift' />";
        private string _swift;
        [Xrm("cgi_swift")]
        public string Swift
        {
            get { return _swift; }
            set { _swift = value; }
        }

        //"<attribute name='cgi_soc_sec_number' />";
        private string _soc_sec_number;
        [Xrm("cgi_soc_sec_number")]
        public string Soc_sec_number
        {
            get { return _soc_sec_number; }
            set { _soc_sec_number = value; }
        }

        //"<attribute name='cgi_responsibleid' />";
        private Guid? _responsibleid;
        [Xrm("cgi_responsibleid", DecodePart = XrmDecode.Value)]
        public Guid? Responsibleid
        {
            get { return _responsibleid; }
            set { _responsibleid = value; }
        }

        //"<attribute name='cgi_reinvoicing' />";
        private bool _reinvoicing;
        [Xrm("cgi_reinvoicing")]
        public bool Reinvoicing
        {
            get { return _reinvoicing; }
            set { _reinvoicing = value; }
        }

        //"<attribute name='cgi_reimbursementformid' />";
        private Guid? _reimbursementformid;
        [Xrm("cgi_reimbursementformid", DecodePart = XrmDecode.Value)]
        public Guid? Reimbursementformid
        {
            get { return _reimbursementformid; }
            set { _reimbursementformid = value; }
        }

        //"<attribute name='cgi_car_reg' />";
        private string _car_reg;
        [Xrm("cgi_car_reg")]
        public string Car_reg
        {
            get { return _car_reg; }
            set { _car_reg = value; }
        }

        //"<attribute name='cgi_refundtypeid' />";
        private Guid? _refundtypeid;
        [Xrm("cgi_refundtypeid", DecodePart = XrmDecode.Value)]
        public Guid? Refundtypeid
        {
            get { return _refundtypeid; }
            set { _refundtypeid = value; }
        }

        //"<attribute name='cgi_reference' />";
        private string _reference;
        [Xrm("cgi_reference")]
        public string Reference
        {
            get { return _reference; }
            set { _reference = value; }
        }

        //"<attribute name='cgi_quantity' />";
        private int? _quantity;
        [Xrm("cgi_quantity")]
        public int? Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        //"<attribute name='cgi_productid' />";
        private Guid? _productid;
        [Xrm("cgi_productid", DecodePart = XrmDecode.Value)]
        public Guid? Productid
        {
            get { return _productid; }
            set { _productid = value; }
        }

        //"<attribute name='cgi_milage_compensation' />";
        private decimal? _milage_compensation;
        [Xrm("cgi_milage_compensation")]
        public decimal? Milage_compensation
        {
            get { return _milage_compensation; }
            set { _milage_compensation = value; }
        }

        //"<attribute name='cgi_milage' />";
        private decimal _milage;
        [Xrm("cgi_milage")]
        public decimal Milage
        {
            get { return _milage; }
            set { _milage = value; }
        }

        //"<attribute name='cgi_last_valid' />";
        private DateTime? _last_valid;
        [Xrm("cgi_last_valid")]
        public DateTime? Last_valid
        {
            get { return _last_valid; }
            set { _last_valid = value; }
        }

        //"<attribute name='cgi_invoicerecipient' />";
        private Guid? _invoicerecipient;
        [Xrm("cgi_invoicerecipient", DecodePart = XrmDecode.Value)]
        public Guid? Invoicerecipient
        {
            get { return _invoicerecipient; }
            set { _invoicerecipient = value; }
        }

        //"<attribute name='cgi_iban' />";
        private string _iban;
        [Xrm("cgi_iban")]
        public string Iban
        {
            get { return _iban; }
            set { _iban = value; }
        }

        //"<attribute name='cgi_foreign_payment' />";
        private string _foreign_payment;
        [Xrm("cgi_foreign_payment")]
        public string Foreign_payment
        {
            get { return _foreign_payment; }
            set { _foreign_payment = value; }
        }

        //"<attribute name='cgi_contactid' />";
        private Guid? _contactid;
        [Xrm("cgi_contactid", DecodePart = XrmDecode.Value)]
        public Guid? Contactid
        {
            get { return _contactid; }
            set { _contactid = value; }
        }

        //"<attribute name='cgi_comments' />";
        private string _comments;
        [Xrm("cgi_comments")]
        public string Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }

        //"<attribute name='cgi_caseid' />";
        private Guid? _caseid;
        [Xrm("cgi_caseid", DecodePart=XrmDecode.Value)]
        public Guid? Caseid
        {
            get { return _caseid; }
            set { _caseid = value; }
        }

        //"<attribute name='cgi_attestation' />";
        private int? _attestation;
        [Xrm("cgi_attestation", DecodePart=XrmDecode.Value)]
        public int? Attestation
        {
            get { return _attestation; }
            set { _attestation = value; }
        }
        
        //"<attribute name='cgi_amountwithtax' />";
        private Money _amountwithtax;
        [Xrm("cgi_amountwithtax")]
        public Money Amountwithtax
        {
            get { return _amountwithtax; }
            set { _amountwithtax = value; }
        }

        //"<attribute name='cgi_amount' />";
        private Money _amount;
        [Xrm("cgi_amount")]
        public Money Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        //"<attribute name='cgi_accountno' />";
        private string _accountno;
        [Xrm("cgi_accountno")]
        public string Accountno
        {
            get { return _accountno; }
            set { _accountno = value; }
        }

        //"<attribute name='cgi_accountid' />";
        private Guid? _accountid;
        [Xrm("cgi_accountid", DecodePart = XrmDecode.Value)]
        public Guid? Accountid
        {
            get { return _accountid; }
            set { _accountid = value; }
        
        }

        //"<attribute name='cgi_accountid' />";
        private string _accountidName;
        [Xrm("cgi_accountid", DecodePart = XrmDecode.Name)]
        public string AccountidName
        {
            get { return _accountidName; }
            set { _accountidName = value; }

        }


        [Xrm("cgi_invoicerecipient", DecodePart = XrmDecode.Value)]
        public Guid? cgi_invoicerecipient
        {
            get;
            set;

        }
    }
}
