using System;
using System.Linq;
using CGIXrmExtConnectorService;
using CGIXrmWin;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using System.Configuration;
using Microsoft.Xrm.Sdk.Query;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Collections;


using System.Data;
using CGIXrmExtConnectorService.SvcDirectJourneyBetweenStops;
using CGIXrmExtConnectorService.SvcCallsForServiceJourney;

using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class PubTransManager
{
    private XrmManager xrmManager;
    private XrmHelper xrmHelper;

    public PubTransManager()
    {
        xrmHelper = new XrmHelper();
        //xrmManager = xrmHelper.GetXrmManagerFromAppSettings(Guid.Empty);

    }
    public PubTransManager(Guid callerId)
    {
        xrmHelper = new XrmHelper();
        //xrmManager = xrmHelper.GetXrmManagerFromAppSettings(callerId);
    }


    private T ConvertToObject<T>(object source)
    {
        XmlSerializer xmlSerializer = new XmlSerializer(source.GetType());
        StringWriter strWriter = new StringWriter();

        XmlWriter xmlWriter = XmlWriter.Create(strWriter);
        xmlSerializer.Serialize(xmlWriter, source);
        string serializedXml = strWriter.ToString();

        XmlReaderSettings xmlSettings = new XmlReaderSettings();


        StringReader strReader = new StringReader(serializedXml);
        XmlReader xmlReader = XmlReader.Create(strReader, xmlSettings);
        XmlSerializer xmlDeSerializer = new XmlSerializer(typeof(T));
        return (T)xmlDeSerializer.Deserialize(xmlReader);
    }
    /// <summary>
    /// This method will fetch the strad bus related details from the pubtrans_staging database by executing the Storeporecedure 
    /// SP_GetLineDetails with the parameter as "STRADBUS".
    /// The result will be in the form of a XML which will be deserialized into Zone Class.
    /// </summary>
    /// <returns>List of Zones</returns>
    public List<Zone> GetStradBusDetails()
    {
        try
        {
            //XmlReader reader = GetLineDetails("STRADBUS");
            List<Zone> retList = null;
            SqlConnection sqlCon = xrmHelper.OpenSQL();
            using (SqlCommand command = new SqlCommand("SP_GetLineDetails", sqlCon))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter { ParameterName = "@LineType", SqlDbType = System.Data.SqlDbType.VarChar, SqlValue = "STRADBUS" });
                XmlReader reader = command.ExecuteXmlReader();
                if (reader != null)
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(Zones));
                    Zones zones = xmlSerializer.Deserialize(reader) as Zones;
                    if (zones == null || zones.Zone == null)
                        retList = new List<Zone>();
                    else
                        retList = new List<Zone>(zones.Zone);
                }
                reader.Close();
                xrmHelper.CloseSQL(sqlCon);

            }


            return retList;
        }
        catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
        {
            string ExceptionMsg = "The application terminated with an error. Timestamp: " + ex.Detail.Timestamp +
                                 "Code: " + ex.Detail.ErrorCode +
                                 "Message: " + ex.Detail.Message +
                                 "Plugin Trace: " + ex.Detail.TraceText +
                                 "Inner Fault: " + (null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");

            throw new Exception(ExceptionMsg, ex);
        }
        catch (System.TimeoutException ex)
        {
            string ExceptionMsg = "The application terminated with an error. Message:" + ex.Message + " Stack Trace:" + ex.StackTrace + " Inner Fault: {0}" + (null == ex.InnerException.Message ? "No Inner Fault" : ex.InnerException.Message);
            throw new Exception(ExceptionMsg, ex);
        }
        catch (System.Exception ex)
        {


            // Display the details of the inner exception.
            if (ex.InnerException != null)
            {
                string ExceptionMsg = ex.InnerException.Message;

                FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> fe = ex.InnerException
                    as FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>;

                if (fe != null)
                {
                    ExceptionMsg = ExceptionMsg + "The application terminated with an error. Timestamp: " + fe.Detail.Timestamp +
                                  "Code: " + fe.Detail.ErrorCode +
                                  "Message: " + fe.Detail.Message +
                                  "Plugin Trace: " + fe.Detail.TraceText +
                                  "Inner Fault: " + (null == fe.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                    throw new Exception(ExceptionMsg, fe);
                }
                throw new Exception(ExceptionMsg, ex);
            }
            else
            {
                string ExceptionMsg = "The application terminated with an error." + ex.Message;
                throw new Exception(ExceptionMsg, ex);
            }
        }
    }

    /// <summary>
    /// This method will fetch the region bus related details from the pubtrans_staging database by executing the Storeporecedure 
    /// SP_GetLineDetails with the parameter as "REGIONBUS".
    /// The result will be in the form of a XML which will be deserialized into Line Class.
    /// </summary>
    /// <returns>List of Line</returns>
    public List<Line> GetRegionBusDetails()
    {

        try
        {
            List<Line> retList = null;
            SqlConnection sqlCon = xrmHelper.OpenSQL();
            using (SqlCommand command = new SqlCommand("SP_GetLineDetails", sqlCon))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter { ParameterName = "@LineType", SqlDbType = System.Data.SqlDbType.VarChar, SqlValue = "REGIONBUS" });
                XmlReader reader = command.ExecuteXmlReader();
                if (reader != null)
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(Lines));
                    Lines lines = xmlSerializer.Deserialize(reader) as Lines;
                    if (lines == null || lines.Line == null)
                        retList = new List<Line>();
                    else
                        retList = new List<Line>(lines.Line);
                }
                reader.Close();
                xrmHelper.CloseSQL(sqlCon);
            }

            return retList;
        }
        catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
        {
            string ExceptionMsg = "The application terminated with an error. Timestamp: " + ex.Detail.Timestamp +
                                 "Code: " + ex.Detail.ErrorCode +
                                 "Message: " + ex.Detail.Message +
                                 "Plugin Trace: " + ex.Detail.TraceText +
                                 "Inner Fault: " + (null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");

            throw new Exception(ExceptionMsg, ex);
        }
        catch (System.TimeoutException ex)
        {
            string ExceptionMsg = "The application terminated with an error. Message:" + ex.Message + " Stack Trace:" + ex.StackTrace + " Inner Fault: {0}" + (null == ex.InnerException.Message ? "No Inner Fault" : ex.InnerException.Message);
            throw new Exception(ExceptionMsg, ex);
        }
        catch (System.Exception ex)
        {


            // Display the details of the inner exception.
            if (ex.InnerException != null)
            {
                string ExceptionMsg = ex.InnerException.Message;

                FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> fe = ex.InnerException
                    as FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>;

                if (fe != null)
                {
                    ExceptionMsg = ExceptionMsg + "The application terminated with an error. Timestamp: " + fe.Detail.Timestamp +
                                  "Code: " + fe.Detail.ErrorCode +
                                  "Message: " + fe.Detail.Message +
                                  "Plugin Trace: " + fe.Detail.TraceText +
                                  "Inner Fault: " + (null == fe.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                    throw new Exception(ExceptionMsg, fe);
                }
                throw new Exception(ExceptionMsg, ex);
            }
            else
            {
                string ExceptionMsg = "The application terminated with an error." + ex.Message;
                throw new Exception(ExceptionMsg, ex);
            }
        }


    }

    /// <summary>
    /// This method will fetch the train related details from the pubtrans_staging database by executing the Storeporecedure 
    /// SP_GetLineDetails with the parameter as "TRAIN".
    /// The result will be in the form of a XML which will be deserialized into stoparea Class.
    /// </summary>
    /// <returns>List of stoparea</returns>
    public List<StopArea> GetTrainDetails()
    {

        try
        {
            List<StopArea> retList = null;
            SqlConnection sqlCon = xrmHelper.OpenSQL();
            using (SqlCommand command = new SqlCommand("SP_GetLineDetails", sqlCon))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter { ParameterName = "@LineType", SqlDbType = System.Data.SqlDbType.VarChar, SqlValue = "TRAIN" });
                XmlReader reader = command.ExecuteXmlReader();
                if (reader != null)
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(StopAreas));
                    StopAreas stopAreas = xmlSerializer.Deserialize(reader) as StopAreas;
                    if (stopAreas == null || stopAreas.StopArea == null)
                        retList = new List<StopArea>();
                    else
                        retList = new List<StopArea>(stopAreas.StopArea);
                }
                reader.Close();
                xrmHelper.CloseSQL(sqlCon);
            }

            return retList;
        }
        catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
        {
            string ExceptionMsg = "The application terminated with an error. Timestamp: " + ex.Detail.Timestamp +
                                 "Code: " + ex.Detail.ErrorCode +
                                 "Message: " + ex.Detail.Message +
                                 "Plugin Trace: " + ex.Detail.TraceText +
                                 "Inner Fault: " + (null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");

            throw new Exception(ExceptionMsg, ex);
        }
        catch (System.TimeoutException ex)
        {
            string ExceptionMsg = "The application terminated with an error. Message:" + ex.Message + " Stack Trace:" + ex.StackTrace + " Inner Fault: {0}" + (null == ex.InnerException.Message ? "No Inner Fault" : ex.InnerException.Message);
            throw new Exception(ExceptionMsg, ex);
        }
        catch (System.Exception ex)
        {


            // Display the details of the inner exception.
            if (ex.InnerException != null)
            {
                string ExceptionMsg = ex.InnerException.Message;

                FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> fe = ex.InnerException
                    as FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>;

                if (fe != null)
                {
                    ExceptionMsg = ExceptionMsg + "The application terminated with an error. Timestamp: " + fe.Detail.Timestamp +
                                  "Code: " + fe.Detail.ErrorCode +
                                  "Message: " + fe.Detail.Message +
                                  "Plugin Trace: " + fe.Detail.TraceText +
                                  "Inner Fault: " + (null == fe.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                    throw new Exception(ExceptionMsg, fe);
                }
                throw new Exception(ExceptionMsg, ex);
            }
            else
            {
                string ExceptionMsg = "The application terminated with an error." + ex.Message;
                throw new Exception(ExceptionMsg, ex);
            }
        }
    }

    /// <summary>
    /// This is the methods which integrates with PubTrans via Biztalk for getting DirectJourneys between two Stations.
    /// </summary>
    /// <param name="fromStopAreaGid">Gid of the From StopArea</param>
    /// <param name="toStopAreaGid">Gid of the Destination StopArea</param>
    /// <param name="journeyDateTime">Journey Date and Time</param>
    /// <param name="forLineGids">If it is for a particular line then Gid of the line for which we require the journey details.</param>
    /// <param name="transportType">if</param>
    /// <returns></returns>
    internal GetDirectJourneysBetweenStopsResponse GetDirectJourneysbetweenStops0(string fromStopAreaGid, string toStopAreaGid, DateTime journeyDateTime, string forLineGids, TransportType transportType)
    {

        try
        {
            DirectJourneysBetweenStopsClient client = new DirectJourneysBetweenStopsClient();

            object[] responses = null;

            GetDirectJourneysBetweenStops getDirectJourneysBetweenStops = new GetDirectJourneysBetweenStops()
            {
                fromStopAreaGid = fromStopAreaGid,
                toStopAreaGid = toStopAreaGid,
                forTimeWindowStartDateTime = journeyDateTime,
                forTimeWindowDuration = Convert.ToString(59),
                withDepartureMaxCount = Convert.ToString(9999),
                forLineGids = transportType == TransportType.TRAIN ? string.Empty : forLineGids,
                forProducts = transportType == TransportType.TRAIN ? "AOT" : string.Empty,
                purposeOfLineGroupingCode = transportType == TransportType.TRAIN ? "DISTRICT" : string.Empty
            };

            responses = client.GetDirectJourneysBetweenStops(getDirectJourneysBetweenStops);

            GetDirectJourneysBetweenStopsResponse getDirectJourneysBetweenStopsResponse = new GetDirectJourneysBetweenStopsResponse();

            if (responses != null && responses.Length > 0)
            {

                ObservableCollection<DirectJourneyBetweenStops> directJourneysBetweenStops = new ObservableCollection<DirectJourneyBetweenStops>();
                ObservableCollection<DeviationMessageVersion> deviationMessageVersions = new ObservableCollection<DeviationMessageVersion>();
                ObservableCollection<DeviationMessageVariant> deviationMessageVariants = new ObservableCollection<DeviationMessageVariant>();
                ObservableCollection<ServiceJourneyDeviation> serviceJourneyDeviations = new ObservableCollection<ServiceJourneyDeviation>();
                ObservableCollection<ArrivalDeviation> arrivalDeviations = new ObservableCollection<ArrivalDeviation>();
                ObservableCollection<DepartureDeviation> departureDeviations = new ObservableCollection<DepartureDeviation>();

                //getDirectJourneysBetweenStopsResponse.DirectJourneysBetweenStops = new ObservableCollection<DirectJourneyBetweenStops>();
                //getDirectJourneysBetweenStopsResponse.DeviationMessageVersions = new ObservableCollection<DeviationMessageVersion>();
                //getDirectJourneysBetweenStopsResponse.DeviationMessageVariants = new ObservableCollection<DeviationMessageVariant>();
                //getDirectJourneysBetweenStopsResponse.ServiceJourneyDeviations = new ObservableCollection<ServiceJourneyDeviation>();                               
                //getDirectJourneysBetweenStopsResponse.ArrivalDeviations = new ObservableCollection<ArrivalDeviation>();
                //getDirectJourneysBetweenStopsResponse.DepartureDeviations = new ObservableCollection<DepartureDeviation>();

                


                foreach (object response in responses)
                {

                    if (response.GetType() == typeof(GetDirectJourneysBetweenStopsResponseDirectJourneysBetweenStops))
                    {
                        directJourneysBetweenStops.Add(ConvertToObject<DirectJourneyBetweenStops>(response));
                        //continue;
                    }
                    else if (response.GetType() == typeof(GetDirectJourneysBetweenStopsResponseDeviationMessageVersion))
                    {
                        DeviationMessageVersion deviationMessageVersion = ConvertToObject<DeviationMessageVersion>(response);
                        //getDirectJourneysBetweenStopsResponse.DeviationMessageVersions.Add(deviationMessageVersion);
                        deviationMessageVersions.Add(deviationMessageVersion);

                    }
                    else if (response.GetType() == typeof(GetDirectJourneysBetweenStopsResponseDeviationMessageVariant))
                    {
                        DeviationMessageVariant deviationMessageVariant = ConvertToObject<DeviationMessageVariant>(response);
                        //getDirectJourneysBetweenStopsResponse.DeviationMessageVariants.Add(deviationMessageVariant);
                        deviationMessageVariants.Add(deviationMessageVariant);
                    }
                    else if (response.GetType() == typeof(GetDirectJourneysBetweenStopsResponseArrivalDeviation))
                    {
                        ArrivalDeviation arrivalDeviation = ConvertToObject<ArrivalDeviation>(response);
                        //getDirectJourneysBetweenStopsResponse.ArrivalDeviations.Add(arrivalDeviation);
                        arrivalDeviations.Add(arrivalDeviation);
                    }
                    else if (response.GetType() == typeof(GetDirectJourneysBetweenStopsResponseDepartureDeviation))
                    {
                        DepartureDeviation departureDeviation = ConvertToObject<DepartureDeviation>(response);
                        //getDirectJourneysBetweenStopsResponse.DepartureDeviations.Add(departureDeviation);
                        departureDeviations.Add(departureDeviation);
                    }
                    else if (response.GetType() == typeof(GetDirectJourneysBetweenStopsResponseServiceJourneyDeviation))
                    {
                        ServiceJourneyDeviation serviceJourneyDeviation = ConvertToObject<ServiceJourneyDeviation>(response);
                        getDirectJourneysBetweenStopsResponse.ServiceJourneyDeviations.Add(serviceJourneyDeviation);
                        serviceJourneyDeviations.Add(serviceJourneyDeviation);
                    }
                    else
                    {
                        // should not happend!
                    }
                }
                getDirectJourneysBetweenStopsResponse.DirectJourneysBetweenStops = directJourneysBetweenStops;
                getDirectJourneysBetweenStopsResponse.DeviationMessageVersions = deviationMessageVersions;
                getDirectJourneysBetweenStopsResponse.DeviationMessageVariants = deviationMessageVariants;
                getDirectJourneysBetweenStopsResponse.ServiceJourneyDeviations = serviceJourneyDeviations;
                getDirectJourneysBetweenStopsResponse.ArrivalDeviations = arrivalDeviations;
                getDirectJourneysBetweenStopsResponse.DepartureDeviations = departureDeviations;

            }





            return getDirectJourneysBetweenStopsResponse;
        }
        catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
        {
            string ExceptionMsg = "The application terminated with an error. Timestamp: " + ex.Detail.Timestamp +
                                 "Code: " + ex.Detail.ErrorCode +
                                 "Message: " + ex.Detail.Message +
                                 "Plugin Trace: " + ex.Detail.TraceText +
                                 "Inner Fault: " + (null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");

            throw new Exception(ExceptionMsg, ex);
        }
        catch (System.TimeoutException ex)
        {
            string ExceptionMsg = "The application terminated with an error. Message:" + ex.Message + " Stack Trace:" + ex.StackTrace + " Inner Fault: {0}" + (null == ex.InnerException.Message ? "No Inner Fault" : ex.InnerException.Message);
            throw new Exception(ExceptionMsg, ex);
        }
        catch (System.Exception ex)
        {


            // Display the details of the inner exception.
            if (ex.InnerException != null)
            {
                string ExceptionMsg = ex.InnerException.Message;

                FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> fe = ex.InnerException
                    as FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>;

                if (fe != null)
                {
                    ExceptionMsg = ExceptionMsg + "The application terminated with an error. Timestamp: " + fe.Detail.Timestamp +
                                  "Code: " + fe.Detail.ErrorCode +
                                  "Message: " + fe.Detail.Message +
                                  "Plugin Trace: " + fe.Detail.TraceText +
                                  "Inner Fault: " + (null == fe.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                    throw new Exception(ExceptionMsg, fe);
                }
                throw new Exception(ExceptionMsg, ex);
            }
            else
            {
                string ExceptionMsg = "The application terminated with an error." + ex.Message;
                throw new Exception(ExceptionMsg, ex);
            }
        }


    }

    public GetDirectJourneysBetweenStopsResponse GetDirectJourneysbetweenStops(string fromStopAreaGid, string toStopAreaGid, DateTime journeyDateTime, string forLineGids, TransportType transportType)
    {

        try
        {
            DirectJourneysBetweenStopsClient client = new DirectJourneysBetweenStopsClient();

            object[] responses = null;

            GetDirectJourneysBetweenStops getDirectJourneysBetweenStops = new GetDirectJourneysBetweenStops()
            {
                fromStopAreaGid = fromStopAreaGid,
                toStopAreaGid = toStopAreaGid,
                forTimeWindowStartDateTime = journeyDateTime,
                forTimeWindowStartDateTimeSpecified=true,
                forTimeWindowDuration = Convert.ToString(120),
                //forTimeWindowDurationSpecified = true,
                withDepartureMaxCount = Convert.ToString(9999),
                //withDepartureMaxCountSpecified = true,
                forLineGids = transportType == TransportType.TRAIN ? string.Empty : forLineGids,
                forProducts = transportType == TransportType.TRAIN ? "AOT" : string.Empty,
                purposeOfLineGroupingCode = transportType == TransportType.TRAIN ? "DISTRICT" : string.Empty

            };

            responses = client.GetDirectJourneysBetweenStops(getDirectJourneysBetweenStops);

            GetDirectJourneysBetweenStopsResponse getDirectJourneysBetweenStopsResponse = new GetDirectJourneysBetweenStopsResponse();

            if (responses != null && responses.Length > 0)
            {

                //ObservableCollection<DirectJourneyBetweenStops> directJourneysBetweenStops = new ObservableCollection<DirectJourneyBetweenStops>();
                //ObservableCollection<DeviationMessageVersion> deviationMessageVersions = new ObservableCollection<DeviationMessageVersion>();
                //ObservableCollection<DeviationMessageVariant> deviationMessageVariants = new ObservableCollection<DeviationMessageVariant>();
                //ObservableCollection<ServiceJourneyDeviation> serviceJourneyDeviations = new ObservableCollection<ServiceJourneyDeviation>();
                //ObservableCollection<ArrivalDeviation> arrivalDeviations = new ObservableCollection<ArrivalDeviation>();
                //ObservableCollection<DepartureDeviation> departureDeviations = new ObservableCollection<DepartureDeviation>();

                //getDirectJourneysBetweenStopsResponse.DirectJourneysBetweenStops = new ObservableCollection<DirectJourneyBetweenStops>();
                //getDirectJourneysBetweenStopsResponse.DeviationMessageVersions = new ObservableCollection<DeviationMessageVersion>();
                //getDirectJourneysBetweenStopsResponse.DeviationMessageVariants = new ObservableCollection<DeviationMessageVariant>();
                //getDirectJourneysBetweenStopsResponse.ServiceJourneyDeviations = new ObservableCollection<ServiceJourneyDeviation>();                               
                //getDirectJourneysBetweenStopsResponse.ArrivalDeviations = new ObservableCollection<ArrivalDeviation>();
                //getDirectJourneysBetweenStopsResponse.DepartureDeviations = new ObservableCollection<DepartureDeviation>();

                List<DirectJourneyBetweenStops> lstDirectJourneyBetweenStops = (from x in responses.OfType<GetDirectJourneysBetweenStopsResponseDirectJourneysBetweenStops>()
                                                                                select ConvertToObject<DirectJourneyBetweenStops>(x)).ToList<DirectJourneyBetweenStops>();

                List<DeviationMessageVersion> lstDeviationMessageVersion = (from x in responses.OfType<GetDirectJourneysBetweenStopsResponseDeviationMessageVersion>()
                                                                            select ConvertToObject<DeviationMessageVersion>(x)).ToList<DeviationMessageVersion>();

                List<DeviationMessageVariant> lstDeviationMessageVariant = (from x in responses.OfType<GetDirectJourneysBetweenStopsResponseDeviationMessageVariant>()
                                                                            select ConvertToObject<DeviationMessageVariant>(x)).ToList<DeviationMessageVariant>();

                List<ArrivalDeviation> lstArrivalDeviation = (from x in responses.OfType<GetDirectJourneysBetweenStopsResponseArrivalDeviation>()
                                                              select ConvertToObject<ArrivalDeviation>(x)).ToList<ArrivalDeviation>();


                if(lstArrivalDeviation!=null && lstArrivalDeviation.Count>0)
                {
                    foreach (ArrivalDeviation arrivalDeviation in lstArrivalDeviation)
                    {

                        DirectJourneyBetweenStops affectedJourney = (from journey in lstDirectJourneyBetweenStops
                                                                     where journey.ArrivalId == arrivalDeviation.IsOnArrivalId
                                                                     select journey).FirstOrDefault<DirectJourneyBetweenStops>();

                        var deviationMsg = (from messageVariant in lstDeviationMessageVariant
                                            where ((messageVariant.IsPartOfDeviationMessageId == arrivalDeviation.HasDeviationMessageVersionId)
                                            &&
                                            (messageVariant.UsageTypeLongCode == "SUMMARY"))
                                            select messageVariant.Content);

                        affectedJourney.HasArrivalDeviation = true;
                        affectedJourney.ArrivalDeviationMessage += deviationMsg;
                    }
                }

                List<DepartureDeviation> lstDepartureDeviation = (from x in responses.OfType<GetDirectJourneysBetweenStopsResponseDepartureDeviation>()
                                                                  select ConvertToObject<DepartureDeviation>(x)).ToList<DepartureDeviation>();
                if (lstDepartureDeviation != null && lstDepartureDeviation.Count > 0)
                {
                    foreach (DepartureDeviation departureDeviation in lstDepartureDeviation)
                    {

                        DirectJourneyBetweenStops affectedJourney = (from journey in lstDirectJourneyBetweenStops
                                                                     where journey.DepartureId == departureDeviation.IsOnDepartureId
                                                                     select journey).FirstOrDefault<DirectJourneyBetweenStops>();

                        var deviationMsg = (from messageVariant in lstDeviationMessageVariant
                                            where ((messageVariant.IsPartOfDeviationMessageId == departureDeviation.HasDeviationMessageVersionId)
                                            &&
                                            (messageVariant.UsageTypeLongCode == "SUMMARY"))
                                            select messageVariant.Content);

                        affectedJourney.HasDepartureDeviation = true;
                        affectedJourney.DepartureDeviationMessage += deviationMsg;
                    }
                }
                List<ServiceJourneyDeviation> lstServiceJourneyDeviation = (from x in responses.OfType<GetDirectJourneysBetweenStopsResponseServiceJourneyDeviation>()
                                                                            select ConvertToObject<ServiceJourneyDeviation>(x)).ToList<ServiceJourneyDeviation>();
                if (lstServiceJourneyDeviation != null && lstServiceJourneyDeviation.Count > 0)
                {
                    foreach (ServiceJourneyDeviation serviceJourneyDeviation in lstServiceJourneyDeviation)
                    {

                        DirectJourneyBetweenStops affectedJourney = (from journey in lstDirectJourneyBetweenStops
                                                                     where journey.DatedVehicleJourneyId == serviceJourneyDeviation.IsOnDatedVehicleJourneyId
                                                                     select journey).FirstOrDefault<DirectJourneyBetweenStops>();

                        var deviationMsg = (from messageVariant in lstDeviationMessageVariant
                                            where ((messageVariant.IsPartOfDeviationMessageId == serviceJourneyDeviation.HasDeviationMessageVersionId)
                                            &&
                                            (messageVariant.UsageTypeLongCode == "SUMMARY"))
                                            select messageVariant.Content);

                        affectedJourney.HasServiceJourneyDeviation = true;
                        affectedJourney.ServiceJourneyDeviationMessage += deviationMsg;
                    }
                }

                
                getDirectJourneysBetweenStopsResponse.DirectJourneysBetweenStops = new ObservableCollection<DirectJourneyBetweenStops>(lstDirectJourneyBetweenStops);
                getDirectJourneysBetweenStopsResponse.DeviationMessageVersions = new ObservableCollection<DeviationMessageVersion>(lstDeviationMessageVersion);
                getDirectJourneysBetweenStopsResponse.DeviationMessageVariants = new ObservableCollection<DeviationMessageVariant>(lstDeviationMessageVariant);
                getDirectJourneysBetweenStopsResponse.ServiceJourneyDeviations = new ObservableCollection<ServiceJourneyDeviation>(lstServiceJourneyDeviation);
                getDirectJourneysBetweenStopsResponse.ArrivalDeviations = new ObservableCollection<ArrivalDeviation>(lstArrivalDeviation);
                getDirectJourneysBetweenStopsResponse.DepartureDeviations = new ObservableCollection<DepartureDeviation>(lstDepartureDeviation);

            }

            return getDirectJourneysBetweenStopsResponse;
        }
        catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
        {
            string ExceptionMsg = "The application terminated with an error. Timestamp: " + ex.Detail.Timestamp +
                                 "Code: " + ex.Detail.ErrorCode +
                                 "Message: " + ex.Detail.Message +
                                 "Plugin Trace: " + ex.Detail.TraceText +
                                 "Inner Fault: " + (null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");

            throw new Exception(ExceptionMsg, ex);
        }
        catch (System.TimeoutException ex)
        {
            string ExceptionMsg = "The application terminated with an error. Message:" + ex.Message + " Stack Trace:" + ex.StackTrace + " Inner Fault: {0}" + (null == ex.InnerException.Message ? "No Inner Fault" : ex.InnerException.Message);
            throw new Exception(ExceptionMsg, ex);
        }
        catch (System.Exception ex)
        {


            // Display the details of the inner exception.
            if (ex.InnerException != null)
            {
                string ExceptionMsg = ex.InnerException.Message;

                FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> fe = ex.InnerException
                    as FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>;

                if (fe != null)
                {
                    ExceptionMsg = ExceptionMsg + "The application terminated with an error. Timestamp: " + fe.Detail.Timestamp +
                                  "Code: " + fe.Detail.ErrorCode +
                                  "Message: " + fe.Detail.Message +
                                  "Plugin Trace: " + fe.Detail.TraceText +
                                  "Inner Fault: " + (null == fe.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                    throw new Exception(ExceptionMsg, fe);
                }
                throw new Exception(ExceptionMsg, ex);
            }
            else
            {
                string ExceptionMsg = "The application terminated with an error." + ex.Message;
                throw new Exception(ExceptionMsg, ex);
            }
        }


    }

    public CallsForServiceJourneyResponse GetCallsForServiceJourney(string serviceJourneyId, DateTime operatingDate, string atStopGid, bool includeArrivalTable = true, bool includeDepartureTable = true, bool includeDeviationTable = true)
    {
        CallsForServiceJourneyResponse callsForServiceJourneyResponse = new CallsForServiceJourneyResponse();

        try
        {
            CallsForServiceJourneyClient client = new CallsForServiceJourneyClient();

            GetCallsforServiceJourney callsForServiceJourney = new GetCallsforServiceJourney()
            {
                forServiceJourneyIdOrGid = serviceJourneyId,
                atOperatingDate = operatingDate,
                atOperatingDateSpecified=true,
                atStopGid = atStopGid,
                includeArrivalsTable = includeArrivalTable,
                includeArrivalsTableSpecified=true,
                includeDeparturesTable = includeDepartureTable,
                includeDeparturesTableSpecified=true,
                includeDeviationTables = includeDeviationTable,
                includeDeviationTablesSpecified=true
            };
            object[] responses = null;

            responses = client.GetCallsForServiceJourney(callsForServiceJourney);
            
            if (responses != null && responses.Length > 0)
            {

                //ObservableCollection<DatedServiceJourney> datedServiceJourneys = new ObservableCollection<DatedServiceJourney>();
                //ObservableCollection<DatedArrival> datedArrival = new ObservableCollection<DatedArrival>();
                //ObservableCollection<DatedDeparture> datedDeparture = new ObservableCollection<DatedDeparture>();
                //ObservableCollection<DeviationMessageVersion> deviationMessageVersions = new ObservableCollection<DeviationMessageVersion>();
                //ObservableCollection<DeviationMessageVariant> deviationMessageVariants = new ObservableCollection<DeviationMessageVariant>();
                //ObservableCollection<ServiceJourneyDeviation> serviceJourneyDeviations = new ObservableCollection<ServiceJourneyDeviation>();
                //ObservableCollection<ArrivalDeviation> arrivalDeviations = new ObservableCollection<ArrivalDeviation>();
                //ObservableCollection<DepartureDeviation> departureDeviations = new ObservableCollection<DepartureDeviation>();


                List<DatedServiceJourney> lstDatedServiceJourney = (from x in responses.OfType<GetCallsforServiceJourneyMethodDatedServiceJourney>()
                                                                                select ConvertToObject<DatedServiceJourney>(x)).ToList<DatedServiceJourney>();

                List<DatedArrival> lstDatedArrival = (from x in responses.OfType<GetCallsforServiceJourneyMethodDatedArrival>()
                                                             select ConvertToObject<DatedArrival>(x)).ToList<DatedArrival>();

                List<DatedDeparture> lstDatedDeparture = (from x in responses.OfType<GetCallsforServiceJourneyMethodDatedDeparture>()
                                                               select ConvertToObject<DatedDeparture>(x)).ToList<DatedDeparture>();

                List<DeviationMessageVersion1> lstDeviationMessageVersion = (from x in responses.OfType<GetCallsforServiceJourneyMethodDeviationMessageVersion>()
                                                                            select ConvertToObject<DeviationMessageVersion1>(x)).ToList<DeviationMessageVersion1>();

                List<DeviationMessageVariant1> lstDeviationMessageVariant = (from x in responses.OfType<GetCallsforServiceJourneyMethodDeviationMessageVariant>()
                                                                            select ConvertToObject<DeviationMessageVariant1>(x)).ToList<DeviationMessageVariant1>();

                List<ArrivalDeviation1> lstArrivalDeviation = (from x in responses.OfType<GetCallsforServiceJourneyMethodArrivalDeviation>()
                                                              select ConvertToObject<ArrivalDeviation1>(x)).ToList<ArrivalDeviation1>();

                List<DepartureDeviation1> lstDepartureDeviation = (from x in responses.OfType<GetCallsforServiceJourneyMethodDepartureDeviation>()
                                                                  select ConvertToObject<DepartureDeviation1>(x)).ToList<DepartureDeviation1>();

                List<ServiceJourneyDeviation1> lstServiceJourneyDeviation = (from x in responses.OfType<GetCallsforServiceJourneyMethodServiceJourneyDeviation>()
                                                                            select ConvertToObject<ServiceJourneyDeviation1>(x)).ToList<ServiceJourneyDeviation1>();



                callsForServiceJourneyResponse.DatedServiceJourneys = new ObservableCollection<DatedServiceJourney>(lstDatedServiceJourney);
                callsForServiceJourneyResponse.DatedArrivals = new ObservableCollection<DatedArrival>(lstDatedArrival);
                callsForServiceJourneyResponse.DatedDepartures = new ObservableCollection<DatedDeparture>(lstDatedDeparture);
                callsForServiceJourneyResponse.DeviationMessageVersions = new ObservableCollection<DeviationMessageVersion1>(lstDeviationMessageVersion);
                callsForServiceJourneyResponse.DeviationMessageVariants = new ObservableCollection<DeviationMessageVariant1>(lstDeviationMessageVariant);
                callsForServiceJourneyResponse.ServiceJourneyDeviations = new ObservableCollection<ServiceJourneyDeviation1>(lstServiceJourneyDeviation);
                callsForServiceJourneyResponse.ArrivalDeviations = new ObservableCollection<ArrivalDeviation1>(lstArrivalDeviation);
                callsForServiceJourneyResponse.DepartureDeviations = new ObservableCollection<DepartureDeviation1>(lstDepartureDeviation);
                callsForServiceJourneyResponse.ErrorMessage = "";
            }
            
        }
        catch(Exception ex)
        {
            //throw new Exception(ex.Message,ex);
            callsForServiceJourneyResponse.ErrorMessage = ex.Message;
        }

        return callsForServiceJourneyResponse;

    }
    
    public OrganisationalUnitResponse GetOrganisationalUnits()
    {
        OrganisationalUnitResponse _response = new OrganisationalUnitResponse();
        try
        {
            //SP_GetOrganisationalUnits
            OrganisationalUnits retList = null;
            SqlConnection sqlCon = xrmHelper.OpenSQL();
            using (SqlCommand command = new SqlCommand("SP_GetOrganisationalUnits", sqlCon))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                XmlReader reader = command.ExecuteXmlReader();
                if (reader != null)
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(OrganisationalUnits));
                    retList = xmlSerializer.Deserialize(reader) as OrganisationalUnits;
                }
                reader.Close();
                xrmHelper.CloseSQL(sqlCon);

                if (retList != null && retList.OrganisationalUnitList != null && retList.OrganisationalUnitList.Count() > 0)
                {
                    _response.OrganisationalUnitList = retList.OrganisationalUnitList.ToList();
                    _response.Errormessage = null;
                }
                else
                {
                    _response.OrganisationalUnitList = null;
                    _response.Errormessage = "Finns inga OrganisationalUnits!";
                }
            }
        }
        catch (Exception ex)
        {
            _response.OrganisationalUnitList = null;
            _response.Errormessage = ex.Message;
        }
        return _response;
    }

    public ContractorResponse GetContractors()
    {
        ContractorResponse _response = new ContractorResponse();
        try
        {
            //SP_GetContractors
            Contractors retList = null;
            SqlConnection sqlCon = xrmHelper.OpenSQL();
            using (SqlCommand command = new SqlCommand("SP_GetContractors", sqlCon))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                XmlReader reader = command.ExecuteXmlReader();
                if (reader != null)
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(Contractors));
                    retList = xmlSerializer.Deserialize(reader) as Contractors;
                }
                reader.Close();
                xrmHelper.CloseSQL(sqlCon);

                if (retList != null && retList.ContractorList != null && retList.ContractorList.Count() > 0)
                {
                    _response.ContractorList = retList.ContractorList.ToList();
                    _response.Errormessage = null;
                }
                else
                {
                    _response.ContractorList = null;
                    _response.Errormessage = "Finns inga contractors!";
                }
            }
        }
        catch (Exception ex)
        {
            _response.ContractorList = null;
            _response.Errormessage = ex.Message;
        }
        return _response;
    }
    
    ///// <summary>
    ///// This method will return the contractor details based on the gid of the contractor provided.
    ///// </summary>
    ///// <param name="ContractorGid">Gid of the Contractor Provided</param>
    ///// <returns>Contractor Detail Class.</returns>
    //internal ContactorDetail GetContractorDetail(int ContractorGid)
    //{
    //    try
    //    {
    //        ContactorDetail contractorDetail = new ContactorDetail();
    //        SqlConnection sqlCon = xrmHelper.OpenSQL();
    //        using (SqlCommand command = new SqlCommand("SP_GetContractorDetail", sqlCon))
    //        {
    //            command.CommandType = System.Data.CommandType.StoredProcedure;
    //            command.Parameters.Add(new SqlParameter { ParameterName = "@ContractorId", SqlDbType = System.Data.SqlDbType.VarChar, SqlValue = ContractorGid.ToString() });
    //            XmlReader reader = command.ExecuteXmlReader();
    //            if (reader != null)
    //            {
    //                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ContactorDetail));
    //                contractorDetail = xmlSerializer.Deserialize(reader) as ContactorDetail;

    //            }
    //            reader.Close();
    //            xrmHelper.CloseSQL(sqlCon);

    //        }
    //        return contractorDetail;
    //    }
    //    catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
    //    {
    //        string ExceptionMsg = "The application terminated with an error. Timestamp: " + ex.Detail.Timestamp +
    //                             "Code: " + ex.Detail.ErrorCode +
    //                             "Message: " + ex.Detail.Message +
    //                             "Plugin Trace: " + ex.Detail.TraceText +
    //                             "Inner Fault: " + (null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");

    //        throw new Exception(ExceptionMsg, ex);
    //    }
    //    catch (System.TimeoutException ex)
    //    {
    //        string ExceptionMsg = "The application terminated with an error. Message:" + ex.Message + " Stack Trace:" + ex.StackTrace + " Inner Fault: {0}" + (null == ex.InnerException.Message ? "No Inner Fault" : ex.InnerException.Message);
    //        throw new Exception(ExceptionMsg, ex);
    //    }
    //    catch (System.Exception ex)
    //    {


    //        // Display the details of the inner exception.
    //        if (ex.InnerException != null)
    //        {
    //            string ExceptionMsg = ex.InnerException.Message;

    //            FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> fe = ex.InnerException
    //                as FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>;

    //            if (fe != null)
    //            {
    //                ExceptionMsg = ExceptionMsg + "The application terminated with an error. Timestamp: " + fe.Detail.Timestamp +
    //                              "Code: " + fe.Detail.ErrorCode +
    //                              "Message: " + fe.Detail.Message +
    //                              "Plugin Trace: " + fe.Detail.TraceText +
    //                              "Inner Fault: " + (null == fe.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
    //                throw new Exception(ExceptionMsg, fe);
    //            }
    //            throw new Exception(ExceptionMsg, ex);
    //        }
    //        else
    //        {
    //            string ExceptionMsg = "The application terminated with an error." + ex.Message;
    //            throw new Exception(ExceptionMsg, ex);
    //        }
    //    }
    //}


}