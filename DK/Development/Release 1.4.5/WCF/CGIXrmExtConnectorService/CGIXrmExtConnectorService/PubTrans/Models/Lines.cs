using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CGIXrmExtConnectorService.PubTrans.Models
{
    [Serializable]
    [DataContract]
    [XmlRoot("Lines")]
    public class Lines
    {
        #region Public Properties -------------------------------------------------------------------------------------

        [XmlElement("Line")]
        public Line[] Line { get; set; }

        #endregion
    }
}