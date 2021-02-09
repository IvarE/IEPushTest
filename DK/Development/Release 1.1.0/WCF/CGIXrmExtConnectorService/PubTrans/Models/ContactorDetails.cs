using System.Runtime.Serialization;
using System.Xml.Serialization;

[DataContract]
[XmlRoot("ContractorDetail")]
public class ContactorDetail
{
    private string _ContractorId;
    [DataMember]
    [XmlElement("id")]
    public string ContractorId
    {
        get { return _ContractorId; }
        set { _ContractorId = value; }
    }
    private string _ContractorGid;
    [DataMember]
    [XmlElement("Gid")]
    public string ContractorGid
    {
        get { return _ContractorGid; }
        set { _ContractorGid = value; }
    }

    
    string _ContractorName;
    [DataMember]
    [XmlElement("ContractorName")]
    public string ContractorName
    {
        get { return _ContractorName; }
        set { _ContractorName = value; }
    }
}
