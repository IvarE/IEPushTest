using System;
using System.Xml.Serialization;

namespace CGIXrmCreateCaseService.Case.Models
{
    public class RgolSetting
    {
        #region Public Properties
        //<cgi_rgolsettingid>902721AE-B968-E411-80D3-0050569010AD</cgi_rgolsettingid>
       
        private Guid _rgolSettingId;
        [XmlElement("cgi_rgolsettingid")]
        public Guid RgolSettingId
        {
            get { return _rgolSettingId; }
            set { _rgolSettingId = value; }
        }

        //<cgi_rgolsettingno>3</cgi_rgolsettingno>
        private string _rgolSettingNo;
        [XmlElement("cgi_rgolsettingno")]
        public string RgolSettingNo
        {
            get { return _rgolSettingNo; }
            set { _rgolSettingNo = value; }
        }
        
        //<cgi_name>Värdebevis</cgi_name>
        private string _name;
        [XmlElement("cgi_name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        
        //<cgi_refundtypeid>76DD8DF2-B106-E411-80D1-0050569010AD</cgi_refundtypeid>
        private string _refundTypeId;
        [XmlElement("cgi_refundtypeid")]
        public string RefundTypeId
        {
            get { return _refundTypeId; }
            set { _refundTypeId = value; }
        }
        
        //<cgi_refundtypeidname>Övrigt</cgi_refundtypeidname>
        private string _refundTypeIdName;
        [XmlElement("cgi_refundtypeidname")]
        public string RefundTypeIdName
        {
            get { return _refundTypeIdName; }
            set { _refundTypeIdName = value; }
        }
        
        //<cgi_reimbursementformid>A7B9AD5C-F100-E411-80D1-0050569010AD</cgi_reimbursementformid>
        private string _reImBursementFormId;
        [XmlElement("cgi_reimbursementformid")]
        public string ReImBursementFormId
        {
            get { return _reImBursementFormId; }
            set { _reImBursementFormId = value; }
        }
        
        //<cgi_reimbursementformidname>Värdebevis</cgi_reimbursementformidname>
        private string _reImBursementFormIdName;
        [XmlElement("cgi_reimbursementformidname")]
        public string ReImBursementFormIdName
        {
            get { return _reImBursementFormIdName; }
            set { _reImBursementFormIdName = value; }
        }
        #endregion
    }
}