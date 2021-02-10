using System.Runtime.Serialization;

[DataContract]
public class ExtConnectorServiceFault
{
    [DataMember]
    public string ApplicationName { get; set; }
    
    [DataMember]
    public string Source { get; set; }

    [DataMember]
    public int Code { get; set; }

    [DataMember]
    public string Message { get; set; }

    [DataMember]
    public string Detail { get; set; }

    
}