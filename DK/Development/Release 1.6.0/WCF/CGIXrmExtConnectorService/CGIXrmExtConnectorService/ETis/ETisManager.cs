using System;
using CGIXrmExtConnectorService;
using CGIXrmWin;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using System.Linq;
using System.Configuration;
using Microsoft.Xrm.Sdk.Query;
using etisTTS=CGIXrmExtConnectorService.ETisTimetableService;
using etisJS=CGIXrmExtConnectorService.ETisJourneyService;
using etisSAS=CGIXrmExtConnectorService.ETisStopAreaService;
using etisTIS=CGIXrmExtConnectorService.ETisTrafficInfoService;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class EtisManager
{
    private XrmManager xrmManager;
    private XrmHelper xrmHelper;
    private etisJS.ETISJourneyServiceSoapClient etisJSClient;
    private etisTTS.ETISTimetableserviceSoapClient etisTTSClient;
    private etisSAS.ETISStopAreaServiceSoapClient etisSASClient;
    private etisTIS.ETISTrafficInfoserviceSoapClient etisTIClient;

    ObservableCollection<StopLineInfo> stopLineInfos = new ObservableCollection<StopLineInfo>();

    #region [Private Methods]

    #endregion

    public EtisManager()
    {
            xrmHelper = new XrmHelper();
            xrmManager = xrmHelper.GetXrmManagerFromAppSettings(Guid.Empty);

            etisJSClient = new etisJS.ETISJourneyServiceSoapClient();
            etisTTSClient = new etisTTS.ETISTimetableserviceSoapClient();
            etisSASClient = new etisSAS.ETISStopAreaServiceSoapClient();
            etisTIClient = new etisTIS.ETISTrafficInfoserviceSoapClient();
    }
    public EtisManager(Guid callerId)
    {
        xrmHelper = new XrmHelper();
        xrmManager = xrmHelper.GetXrmManagerFromAppSettings(callerId);

        etisJSClient = new etisJS.ETISJourneyServiceSoapClient();
        etisTTSClient = new etisTTS.ETISTimetableserviceSoapClient();
        etisSASClient = new etisSAS.ETISStopAreaServiceSoapClient();
        etisTIClient = new etisTIS.ETISTrafficInfoserviceSoapClient();
    }

    internal List<etisTTS.LineType> GetLineType()
    {
        etisTTS.GetLineTypesResult getLineTypeResult = etisTTSClient.GetLineTypes();
        if (string.IsNullOrEmpty(getLineTypeResult.Message))
        {
            

            //foreach(etisTTS.LineType lineType in getLineTypeResult.LineTypes)
            //{
            //    foreach (etisTTS.Municipality municipality in lineType.Municipalities)
            //    {
            //        etisSAS.GetStopAreaResult getStopAreaResult=etisSASClient.GetStopArea(string.Empty, Convert.ToString(municipality.Id),string.Empty, string.Empty);
            //        if (string.IsNullOrEmpty(getStopAreaResult.Message))
            //        {
            //            foreach (etisSAS.StopArea stopArea in getStopAreaResult.StopAreas)
            //            {
            //                etisSAS.GetLinePassingStopAreaResult getLinePassingStopAreaResult = etisSASClient.GetLinePassingStopArea(Convert.ToString(stopArea.Id), string.Empty);
            //                if (string.IsNullOrEmpty(getLinePassingStopAreaResult.Message))
            //                {
            //                    foreach (etisSAS.LineInfo lineInfo in getLinePassingStopAreaResult.LineInfos)
            //                    {
            //                        StopLineInfo stopLineInfo = new StopLineInfo();
            //                        stopLineInfo.StopId = stopArea.Id;   
            //                        stopLineInfo.StopName = stopArea.Name;   
            //                        stopLineInfo.MunicipalityId = stopArea.MunicipalityId;   
            //                        stopLineInfo.LineNo = lineInfo.No;
            //                        stopLineInfos.Add(stopLineInfo);
            //                    } 
            //                }
            //                else
            //                {
            //                    throw new Exception(getLinePassingStopAreaResult.Message);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            throw new Exception(getStopAreaResult.Message);
            //        }
            //    }
            //}

            return new List<etisTTS.LineType>(getLineTypeResult.LineTypes);
        }
        else
        {
            throw new Exception(getLineTypeResult.Message);
        }
        
    }

    internal List<etisTIS.LineInfo> GetLineInfo(int lineNo, string lineName, string transportMode, int[] municipalities)
    {
        etisTIS.GetLineInfoResult getLineInfoResult;
        if(lineNo>0)
            getLineInfoResult = etisTIClient.GetLineInfo(lineNo.ToString(), lineName, string.Empty);
        else
            getLineInfoResult = etisTIClient.GetLineInfo(string.Empty, lineName, string.Empty);

        if (string.IsNullOrEmpty(getLineInfoResult.Message))
        {
            List<etisTIS.LineInfo> lineInfos = (from lineInfo in getLineInfoResult.LineInfos
                                             where lineInfo.TransportModeName == transportMode 
                                             //&&                         
                                             //lineInfo.LinePassingMunicipalities.Any(x=>municipalities.Contains(x))
                                             select lineInfo).ToList();

            return lineInfos;
        }
        else
        {
            throw new Exception(getLineInfoResult.Message);
        }

    }

    internal List<etisTIS.LineInfo> GetAllLineInfo()
    {
         etisTIS.GetLineInfoResult getLineInfoResult = etisTIClient.GetLineInfo(string.Empty, string.Empty, string.Empty);

        if (string.IsNullOrEmpty(getLineInfoResult.Message))
        {
            List<etisTIS.LineInfo> lineInfos = new List<etisTIS.LineInfo>(getLineInfoResult.LineInfos);
            return lineInfos;
        }
        else
        {
            throw new Exception(getLineInfoResult.Message);
        }

    }

    internal List<etisSAS.StopArea> GetStopArea(int lineNo, string[] municipalities)
    {
        List<etisSAS.StopArea> lstStopArea = new List<etisSAS.StopArea>();
        foreach (string municipality in municipalities)
	    {
            etisSAS.GetStopAreaResult getStopAreaResult = etisSASClient.GetStopArea(string.Empty, municipality, string.Empty, string.Empty);
            if (string.IsNullOrEmpty(getStopAreaResult.Message))
            {
               
                foreach (etisSAS.StopArea stopArea in getStopAreaResult.StopAreas)
                {
                    etisSAS.GetLinePassingStopAreaResult getLinePassingStopAreaResult = etisSASClient.GetLinePassingStopArea(Convert.ToString(stopArea.Id), string.Empty);
                    
                    
                    if (string.IsNullOrEmpty(getLinePassingStopAreaResult.Message))
                    {
                        if (lineNo >0)
                        {
                            if (getLinePassingStopAreaResult.LineInfos.Any(x => x.No == lineNo))
                            {
                                lstStopArea.Add(stopArea);
                            }
                        }
                        else
                        {
                            throw new Exception("Line no:" + lineNo + ",Enter Valid Line Number");
                        }
                    }
                    else
                    {
                        throw new Exception(getLinePassingStopAreaResult.Message);
                    }
                }
                
            }
            else
            {
                throw new Exception(getStopAreaResult.Message);
            }
        }
        return lstStopArea;

    }

    internal ObservableCollection<StopLineInfo> GetStopLineInfo(string[] municipalities)
    {
            foreach (string municipality in municipalities)
            {
                etisSAS.GetStopAreaResult getStopAreaResult = etisSASClient.GetStopArea(string.Empty, Convert.ToString(municipality), string.Empty, string.Empty);
                if (string.IsNullOrEmpty(getStopAreaResult.Message))
                {
                    foreach (etisSAS.StopArea stopArea in getStopAreaResult.StopAreas)
                    {
                        etisSAS.GetLinePassingStopAreaResult getLinePassingStopAreaResult = etisSASClient.GetLinePassingStopArea(Convert.ToString(stopArea.Id), string.Empty);
                        if (string.IsNullOrEmpty(getLinePassingStopAreaResult.Message))
                        {
                            foreach (etisSAS.LineInfo lineInfo in getLinePassingStopAreaResult.LineInfos)
                            {
                                StopLineInfo stopLineInfo = new StopLineInfo();
                                stopLineInfo.StopId = stopArea.Id;
                                stopLineInfo.StopName = stopArea.Name;
                                stopLineInfo.MunicipalityId = stopArea.MunicipalityId;
                                stopLineInfo.LineNo = lineInfo.No;
                                stopLineInfos.Add(stopLineInfo);
                            }
                        }
                        else
                        {
                            throw new Exception(getLinePassingStopAreaResult.Message);
                        }
                    }
                }
                else
                {
                    throw new Exception(getStopAreaResult.Message);
                }
            }


            return stopLineInfos;
        

    }

}