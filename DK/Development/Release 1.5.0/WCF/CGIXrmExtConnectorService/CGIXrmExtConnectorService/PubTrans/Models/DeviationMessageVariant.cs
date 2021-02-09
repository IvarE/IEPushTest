using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CGIXrmExtConnectorService.PubTrans.Models
{
    [DataContract]
    [XmlRoot("GetDirectJourneysBetweenStopsResponseDeviationMessageVariant")]
    public class DeviationMessageVariant
    {
        #region Public Properties -------------------------------------------------------------------------------------

        [DataMember]
        [XmlElement("Id")]
        public string Id { get; set; }


        [DataMember]
        [XmlElement("IsPartOfDeviationMessageId")]
        public string IsPartOfDeviationMessageId { get; set; }


        [DataMember]
        [XmlElement("Content")]
        public string Content { get; set; }


        [DataMember]
        [XmlElement("ContentTypeLongCode")]
        public string ContentTypeLongCode { get; set; }


        [DataMember]
        [XmlElement("UsageTypeLongCode")]
        public string UsageTypeLongCode { get; set; }


        [DataMember]
        [XmlElement("LanguageCode")]
        public string LanguageCode { get; set; }

        #endregion
    }
}