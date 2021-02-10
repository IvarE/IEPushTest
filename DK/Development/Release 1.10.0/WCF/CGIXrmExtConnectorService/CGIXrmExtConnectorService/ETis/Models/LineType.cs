using CGIXrmExtConnectorService.ETisTimetableService;
using System.Collections.Generic;
public class GetLineTypeResult
{
    int _Code;
    public int Code
    {
        get { return _Code; }
        set { _Code = value; }
    }

    string _Message;
    public string Message
    {
        get { return _Message; }
        set { _Message = value; }
    }

    LineType[] _LineTypes;
    public LineType[] LineTypes
    {
        get { return _LineTypes; }
        set { _LineTypes = value; }
    }
    
    Municipality[] _Municipalities;
    public Municipality[] Municipalities
    {
        get { return _Municipalities; }
        set { _Municipalities = value; }
    }
}
