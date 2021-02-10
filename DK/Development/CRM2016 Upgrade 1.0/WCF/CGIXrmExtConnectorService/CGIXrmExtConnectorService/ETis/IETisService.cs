using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using etisTTS = CGIXrmExtConnectorService.ETisTimetableService;
using etisJS = CGIXrmExtConnectorService.ETisJourneyService;
using etisSAS = CGIXrmExtConnectorService.ETisStopAreaService;
using etisTIS = CGIXrmExtConnectorService.ETisTrafficInfoService;

namespace CGIXrmExtConnectorService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IExtConnectorService" in both code and config file together.
    [ServiceContract]
    public interface IEtisService
    {
        [OperationContract]
        List<etisTTS.LineType> GetLineType();
        [OperationContract]
        List<etisTIS.LineInfo> GetAllLineInfo();
        [OperationContract]
        List<etisTIS.LineInfo> GetLineInfo(int lineNo, string lineName, string transportMode, int[] municipalities);
        [OperationContract]
        List<etisSAS.StopArea> GetStopArea(int lineNo, string[] municipalities);
    }
}
