
using System;
using System.ServiceModel;
using etisTTS = CGIXrmExtConnectorService.ETisTimetableService;
using etisJS = CGIXrmExtConnectorService.ETisJourneyService;
using etisSAS = CGIXrmExtConnectorService.ETisStopAreaService;
using etisTIS = CGIXrmExtConnectorService.ETisTrafficInfoService;
using System.Collections.Generic;

namespace CGIXrmExtConnectorService
    {
        // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ExtConnectorService" in code, svc and config file together.
        public partial class ExtConnectorService : IEtisService
        {
            public List<etisTTS.LineType> GetLineType()
            {
                EtisManager etisManager = new EtisManager();
                return etisManager.GetLineType();
            }
            public List<etisTIS.LineInfo> GetAllLineInfo()
            {
                EtisManager etisManager = new EtisManager();
                return etisManager.GetAllLineInfo();
            }
             public List<etisTIS.LineInfo> GetLineInfo(int lineNo, string lineName, string transportMode, int[] municipalities)
             {
                 EtisManager etisManager = new EtisManager();
                 return etisManager.GetLineInfo(lineNo,lineName,transportMode,municipalities);
             }
             public List<etisSAS.StopArea> GetStopArea(int lineNo, string[] municipalities)
             {
                 EtisManager etisManager = new EtisManager();
                 return etisManager.GetStopArea(lineNo, municipalities);
             }
            
        }
    }
