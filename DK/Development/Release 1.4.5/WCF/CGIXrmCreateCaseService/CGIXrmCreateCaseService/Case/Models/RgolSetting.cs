using System;
using System.Xml.Serialization;

namespace CGIXrmCreateCaseService.Case.Models
{
    public class RgolSetting
    {
        #region Public Properties
        //<cgi_rgolsettingid>902721AE-B968-E411-80D3-0050569010AD</cgi_rgolsettingid>

        [XmlElement("cgi_rgolsettingid")]
        public Guid RgolSettingId { get; set; }

        //<cgi_rgolsettingno>3</cgi_rgolsettingno>
        [XmlElement("cgi_rgolsettingno")]
        public string RgolSettingNo { get; set; }

        //<cgi_name>Värdebevis</cgi_name>
        [XmlElement("cgi_name")]
        public string Name { get; set; }

        //<cgi_refundtypeid>76DD8DF2-B106-E411-80D1-0050569010AD</cgi_refundtypeid>
        [XmlElement("cgi_refundtypeid")]
        public string RefundTypeId { get; set; }

        //<cgi_refundtypeidname>Övrigt</cgi_refundtypeidname>
        [XmlElement("cgi_refundtypeidname")]
        public string RefundTypeIdName { get; set; }

        //<cgi_reimbursementformid>A7B9AD5C-F100-E411-80D1-0050569010AD</cgi_reimbursementformid>
        [XmlElement("cgi_reimbursementformid")]
        public string ReImBursementFormId { get; set; }

        //<cgi_reimbursementformidname>Värdebevis</cgi_reimbursementformidname>
        [XmlElement("cgi_reimbursementformidname")]
        public string ReImBursementFormIdName { get; set; }

        #endregion
    }
}