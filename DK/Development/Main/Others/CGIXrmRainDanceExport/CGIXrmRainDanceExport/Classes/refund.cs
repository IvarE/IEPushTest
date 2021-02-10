//using System;
//using CGIXrmWin;
//using Microsoft.Xrm.Sdk;

//namespace CGIXrmRainDanceExport.Classes
//{
//    public class refund : XrmBaseEntity
//    {
//        #region Public Properties
//        //"<attribute name='cgi_refundid' />";
//        [Xrm("cgi_refundid")]
//        public Guid Refundid { get; set; }

//        //"<attribute name='cgi_refundnumber' />";
//        [Xrm("cgi_refundnumber")]
//        public string Refundnumber { get; set; }

//        //"<attribute name='cgi_vat_code' />";
//        [Xrm("cgi_vat_code", DecodePart=XrmDecode.Value)]
//        public int Vat_code { get; set; }

//        [Xrm("cgi_vat_code", DecodePart = XrmDecode.Name)]
//        public string Vat_code_name { get; set; }

//        //"<attribute name='cgi_value_code' />";
//        [Xrm("cgi_value_code")]
//        public string Value_code { get; set; }

//        //"<attribute name='cgi_travelcard_number' />";
//        [Xrm("cgi_travelcard_number")]
//        public string Travelcard_number { get; set; }

//        //"<attribute name='cgi_transportcompanyid' />";
//        [Xrm("cgi_transportcompanyid", DecodePart = XrmDecode.Value)]
//        public Guid? Transportcompanyid { get; set; }

//        //"<attribute name='cgi_taxi_company' />";
//        [Xrm("cgi_taxi_company")]
//        public string Taxi_company { get; set; }

//        //"<attribute name='cgi_swift' />";
//        [Xrm("cgi_swift")]
//        public string Swift { get; set; }

//        //"<attribute name='cgi_soc_sec_number' />";
//        [Xrm("cgi_soc_sec_number")]
//        public string Soc_sec_number { get; set; }

//        //"<attribute name='cgi_responsibleid' />";
//        [Xrm("cgi_responsibleid", DecodePart = XrmDecode.Value)]
//        public Guid? Responsibleid { get; set; }

//        //"<attribute name='cgi_reinvoicing' />";
//        [Xrm("cgi_reinvoicing")]
//        public bool Reinvoicing { get; set; }

//        //"<attribute name='cgi_reimbursementformid' />";
//        [Xrm("cgi_reimbursementformid", DecodePart = XrmDecode.Value)]
//        public Guid? Reimbursementformid { get; set; }

//        //"<attribute name='cgi_car_reg' />";
//        [Xrm("cgi_car_reg")]
//        public string Car_reg { get; set; }

//        //"<attribute name='cgi_refundtypeid' />";
//        [Xrm("cgi_refundtypeid", DecodePart = XrmDecode.Value)]
//        public Guid? Refundtypeid { get; set; }

//        //"<attribute name='cgi_reference' />";
//        [Xrm("cgi_reference")]
//        public string Reference { get; set; }

//        //"<attribute name='cgi_quantity' />";
//        [Xrm("cgi_quantity")]
//        public int? Quantity { get; set; }

//        //"<attribute name='cgi_productid' />";
//        [Xrm("cgi_productid", DecodePart = XrmDecode.Value)]
//        public Guid? Productid { get; set; }

//        //"<attribute name='cgi_milage_compensation' />";
//        [Xrm("cgi_milage_compensation")]
//        public decimal? Milage_compensation { get; set; }

//        //"<attribute name='cgi_milage' />";
//        [Xrm("cgi_milage")]
//        public decimal Milage { get; set; }

//        //"<attribute name='cgi_last_valid' />";
//        [Xrm("cgi_last_valid")]
//        public DateTime? Last_valid { get; set; }

//        //"<attribute name='cgi_invoicerecipient' />";
//        [Xrm("cgi_invoicerecipient", DecodePart = XrmDecode.Value)]
//        public Guid? Invoicerecipient { get; set; }

//        //"<attribute name='cgi_iban' />";
//        [Xrm("cgi_iban")]
//        public string Iban { get; set; }

//        //"<attribute name='cgi_foreign_payment' />";
//        [Xrm("cgi_foreign_payment")]
//        public string Foreign_payment { get; set; }

//        //"<attribute name='cgi_contactid' />";
//        [Xrm("cgi_contactid", DecodePart = XrmDecode.Value)]
//        public Guid? Contactid { get; set; }

//        //"<attribute name='cgi_comments' />";
//        [Xrm("cgi_comments")]
//        public string Comments { get; set; }

//        //"<attribute name='cgi_caseid' />";
//        [Xrm("cgi_caseid", DecodePart=XrmDecode.Value)]
//        public Guid? Caseid { get; set; }

//        //"<attribute name='cgi_attestation' />";
//        [Xrm("cgi_attestation", DecodePart=XrmDecode.Value)]
//        public int? Attestation { get; set; }

//        //"<attribute name='cgi_amountwithtax' />";
//        [Xrm("cgi_amountwithtax")]
//        public Money Amountwithtax { get; set; }

//        //"<attribute name='cgi_amount' />";
//        [Xrm("cgi_amount")]
//        public Money Amount { get; set; }

//        //"<attribute name='cgi_accountno' />";
//        [Xrm("cgi_accountno")]
//        public string Accountno { get; set; }

//        //"<attribute name='cgi_accountid' />";
//        [Xrm("cgi_accountid", DecodePart = XrmDecode.Value)]
//        public Guid? Accountid { get; set; }

//        //"<attribute name='cgi_accountid' />";
//        [Xrm("cgi_accountid", DecodePart = XrmDecode.Name)]
//        public string AccountidName { get; set; }

//        public DateTime CreatedOn { get; set; };

//        [Xrm("cgi_invoicerecipient", DecodePart = XrmDecode.Value)]
//        public Guid? cgi_invoicerecipient
//        {
//            get;
//            set;
//        }
//        #endregion
//    }
//}
