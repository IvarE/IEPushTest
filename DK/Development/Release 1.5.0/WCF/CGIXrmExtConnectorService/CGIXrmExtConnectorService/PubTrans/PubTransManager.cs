using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Xml;
using System.Xml.Serialization;
using CGIXrmExtConnectorService.PubTrans.Models;
using CGIXrmExtConnectorService.Shared;
using CGIXrmExtConnectorService.Shared.Models;
using CGIXrmExtConnectorService.SvcCallsForServiceJourney;
using CGIXrmExtConnectorService.SvcDirectJourneyBetweenStops;
using GetDirectJourneysBetweenStopsResponse = CGIXrmExtConnectorService.PubTrans.Models.GetDirectJourneysBetweenStopsResponse;

namespace CGIXrmExtConnectorService.PubTrans
{
    public class PubTransManager
    {
        #region Declarations ------------------------------------------------------------------------------------------

        private readonly XrmHelper _xrmHelper;

        #endregion

        #region Public Methods ----------------------------------------------------------------------------------------

        public PubTransManager(XrmHelper xrmHelper)
        {
            _xrmHelper = xrmHelper;
        }

        public PubTransManager()
        {
            _xrmHelper = new XrmHelper();
        }

        #endregion

        #region Private Methods ---------------------------------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        private static T ConvertToObject<T>(object source)
        {
            var xmlSerializer = new XmlSerializer(source.GetType());
            var strWriter = new StringWriter();
            var xmlWriter = XmlWriter.Create(strWriter);

            xmlSerializer.Serialize(xmlWriter, source);

            var serializedXml = strWriter.ToString();
            var xmlSettings = new XmlReaderSettings();
            var strReader = new StringReader(serializedXml);
            var xmlReader = XmlReader.Create(strReader, xmlSettings);
            var xmlDeSerializer = new XmlSerializer(typeof (T));

            return (T) xmlDeSerializer.Deserialize(xmlReader);
        }

        #endregion

        #region Internal Methods --------------------------------------------------------------------------------------

        /// <summary>
        /// This method will fetch the strad bus related details from the pubtrans_staging database by executing the Storeporecedure 
        /// SP_GetLineDetails with the parameter as "STRADBUS".
        /// The result will be in the form of a XML which will be deserialized into Zone Class.
        /// </summary>
        /// <returns>List of Zones</returns>
        /// 
        internal List<Zone> GetStradBusDetails()
        {
            try
            {
                List<Zone> retList = null;
                var sqlCon = _xrmHelper.OpenSql();
                using (var command = new SqlCommand("SP_GetLineDetails", sqlCon))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@LineType",
                        SqlDbType = System.Data.SqlDbType.VarChar,
                        SqlValue = "STRADBUS"
                    });

                    var reader = command.ExecuteXmlReader();
                    {
                        var xmlSerializer = new XmlSerializer(typeof (Zones));
                        var zones = xmlSerializer.Deserialize(reader) as Zones;
                        if (zones != null) retList = new List<Zone>(zones.Zone);
                    }
                    reader.Close();

                    _xrmHelper.CloseSql(sqlCon);
                }


                return retList;
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                var exceptionMsg = "The application terminated with an error. Timestamp: " + ex.Detail.Timestamp +
                                   "Code: " + ex.Detail.ErrorCode +
                                   "Message: " + ex.Detail.Message +
                                   "Plugin Trace: " + ex.Detail.TraceText +
                                   "Inner Fault: " +
                                   (null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");

                throw new Exception(exceptionMsg, ex);
            }
            catch (TimeoutException ex)
            {
                var exceptionMsg = "The application terminated with an error. Message:" +
                                   ex.Message + " Stack Trace:" +
                                   ex.StackTrace + " Inner Fault: {0}" +
                                   ex.InnerException.Message;
                throw new Exception(exceptionMsg, ex);
            }
            catch (Exception ex)
            {
                // Display the details of the inner exception.
                if (ex.InnerException != null)
                {
                    var exceptionMsg = ex.InnerException.Message;

                    var fe = ex.InnerException
                        as FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>;

                    if (fe == null) throw new Exception(exceptionMsg, ex);

                    exceptionMsg = exceptionMsg + "The application terminated with an error. Timestamp: " +
                                   fe.Detail.Timestamp +
                                   "Code: " + fe.Detail.ErrorCode +
                                   "Message: " + fe.Detail.Message +
                                   "Plugin Trace: " + fe.Detail.TraceText +
                                   "Inner Fault: " +
                                   (null == fe.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");

                    throw new Exception(exceptionMsg, fe);
                }

                var exceptionMessage = "The application terminated with an error." + ex.Message;
                throw new Exception(exceptionMessage, ex);
            }
        }


        /// <summary>
        /// This method will fetch the region bus related details from the pubtrans_staging database by executing the Storeporecedure 
        /// SP_GetLineDetails with the parameter as "REGIONBUS".
        /// The result will be in the form of a XML which will be deserialized into Line Class.
        /// </summary>
        /// <returns>List of Line</returns>
        internal List<Line> GetRegionBusDetails()
        {
            try
            {
                List<Line> retList = null;
                var sqlCon = _xrmHelper.OpenSql();
                using (var command = new SqlCommand("SP_GetLineDetails", sqlCon))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@LineType",
                        SqlDbType = System.Data.SqlDbType.VarChar,
                        SqlValue = "REGIONBUS"
                    });
                    var reader = command.ExecuteXmlReader();
                    {
                        var xmlSerializer = new XmlSerializer(typeof (Lines));
                        var lines = xmlSerializer.Deserialize(reader) as Lines;

                        if (lines != null) retList = new List<Line>(lines.Line);
                    }
                    reader.Close();
                    _xrmHelper.CloseSql(sqlCon);
                }

                return retList;
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                var exceptionMsg = "The application terminated with an error. Timestamp: " + ex.Detail.Timestamp +
                                   "Code: " + ex.Detail.ErrorCode +
                                   "Message: " + ex.Detail.Message +
                                   "Plugin Trace: " + ex.Detail.TraceText +
                                   "Inner Fault: " +
                                   (null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");

                throw new Exception(exceptionMsg, ex);
            }
            catch (TimeoutException ex)
            {
                var exceptionMsg = "The application terminated with an error. Message:" + ex.Message +
                                   " Stack Trace:" + ex.StackTrace + " Inner Fault: {0}" +
                                   ex.InnerException.Message;

                throw new Exception(exceptionMsg, ex);
            }
            catch (Exception ex)
            {
                // Display the details of the inner exception.
                if (ex.InnerException != null)
                {
                    var exceptionMsg = ex.InnerException.Message;

                    var fe = ex.InnerException
                        as FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>;

                    if (fe == null) throw new Exception(exceptionMsg, ex);

                    exceptionMsg = exceptionMsg + "The application terminated with an error. Timestamp: " +
                                   fe.Detail.Timestamp +
                                   "Code: " + fe.Detail.ErrorCode +
                                   "Message: " + fe.Detail.Message +
                                   "Plugin Trace: " + fe.Detail.TraceText +
                                   "Inner Fault: " +
                                   (null == fe.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");

                    throw new Exception(exceptionMsg, fe);
                }

                var exceptionMessage = "The application terminated with an error." + ex.Message;
                throw new Exception(exceptionMessage, ex);
            }
        }

        /// <summary>
        /// This method will fetch the train related details from the pubtrans_staging database by executing the Storeporecedure 
        /// SP_GetLineDetails with the parameter as "TRAIN".
        /// The result will be in the form of a XML which will be deserialized into stoparea Class.
        /// </summary>
        /// <returns>List of stoparea</returns>
        internal List<StopArea> GetTrainDetails()
        {
            try
            {
                List<StopArea> retList = null;
                var sqlCon = _xrmHelper.OpenSql();

                using (var command = new SqlCommand("SP_GetLineDetails", sqlCon))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@LineType",
                        SqlDbType = System.Data.SqlDbType.VarChar,
                        SqlValue = "TRAIN"
                    });

                    var reader = command.ExecuteXmlReader();
                    {
                        var xmlSerializer = new XmlSerializer(typeof (StopAreas));
                        var stopAreas = xmlSerializer.Deserialize(reader) as StopAreas;

                        if (stopAreas != null) retList = new List<StopArea>(stopAreas.StopArea);
                    }

                    reader.Close();
                    _xrmHelper.CloseSql(sqlCon);
                }

                return retList;
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                var exceptionMsg = "The application terminated with an error. Timestamp: " + ex.Detail.Timestamp +
                                   "Code: " + ex.Detail.ErrorCode +
                                   "Message: " + ex.Detail.Message +
                                   "Plugin Trace: " + ex.Detail.TraceText +
                                   "Inner Fault: " +
                                   (null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");

                throw new Exception(exceptionMsg, ex);
            }
            catch (TimeoutException ex)
            {
                var exceptionMsg = "The application terminated with an error. Message:" +
                                   ex.Message + " Stack Trace:" +
                                   ex.StackTrace + " Inner Fault: {0}" +
                                   ex.InnerException.Message;
                throw new Exception(exceptionMsg, ex);
            }
            catch (Exception ex)
            {
                // Display the details of the inner exception.
                if (ex.InnerException != null)
                {
                    var exceptionMsg = ex.InnerException.Message;

                    var fe = ex.InnerException
                        as FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>;

                    if (fe == null) throw new Exception(exceptionMsg, ex);

                    exceptionMsg = exceptionMsg + "The application terminated with an error. Timestamp: " +
                                   fe.Detail.Timestamp +
                                   "Code: " + fe.Detail.ErrorCode +
                                   "Message: " + fe.Detail.Message +
                                   "Plugin Trace: " + fe.Detail.TraceText +
                                   "Inner Fault: " +
                                   (null == fe.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                    throw new Exception(exceptionMsg, fe);
                }

                var exceptionMessage = "The application terminated with an error." + ex.Message;
                throw new Exception(exceptionMessage, ex);
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
        internal GetDirectJourneysBetweenStopsResponse GetDirectJourneysbetweenStops0(
            string fromStopAreaGid,
            string toStopAreaGid,
            DateTime journeyDateTime,
            string forLineGids,
            TransportType transportType)
        {
            try
            {
                var client = new DirectJourneysBetweenStopsClient();

                var getDirectJourneysBetweenStops = new GetDirectJourneysBetweenStops()
                {
                    fromStopAreaGid = fromStopAreaGid,
                    toStopAreaGid = toStopAreaGid,
                    forTimeWindowStartDateTime = journeyDateTime,
                    forTimeWindowDuration = 59,
                    withDepartureMaxCount = 9999,
                    forLineGids = transportType == TransportType.Train ? string.Empty : forLineGids,
                    forProducts = transportType == TransportType.Train ? "AOT" : string.Empty,
                    purposeOfLineGroupingCode = transportType == TransportType.Train ? "DISTRICT" : string.Empty
                };

                var responses = client.GetDirectJourneysBetweenStops(getDirectJourneysBetweenStops);

                var getDirectJourneysBetweenStopsResponse = new GetDirectJourneysBetweenStopsResponse();

                if (responses != null && responses.Length > 0)
                {

                    var directJourneysBetweenStops = new ObservableCollection<DirectJourneyBetweenStops>();
                    var deviationMessageVersions = new ObservableCollection<DeviationMessageVersion>();
                    var deviationMessageVariants = new ObservableCollection<DeviationMessageVariant>();
                    var serviceJourneyDeviations = new ObservableCollection<ServiceJourneyDeviation>();
                    var arrivalDeviations = new ObservableCollection<ArrivalDeviation>();
                    var departureDeviations = new ObservableCollection<DepartureDeviation>();

                    foreach (var response in responses)
                    {

                        if (response.GetType() ==
                            typeof (GetDirectJourneysBetweenStopsResponseDirectJourneysBetweenStops))
                        {
                            directJourneysBetweenStops.Add(ConvertToObject<DirectJourneyBetweenStops>(response));
                        }
                        else if (response.GetType() ==
                                 typeof (GetDirectJourneysBetweenStopsResponseDeviationMessageVersion))
                        {
                            var deviationMessageVersion = ConvertToObject<DeviationMessageVersion>(response);
                            deviationMessageVersions.Add(deviationMessageVersion);

                        }
                        else if (response.GetType() ==
                                 typeof (GetDirectJourneysBetweenStopsResponseDeviationMessageVariant))
                        {
                            var deviationMessageVariant = ConvertToObject<DeviationMessageVariant>(response);
                            deviationMessageVariants.Add(deviationMessageVariant);
                        }
                        else if (response.GetType() ==
                                 typeof (GetDirectJourneysBetweenStopsResponseArrivalDeviation))
                        {
                            var arrivalDeviation = ConvertToObject<ArrivalDeviation>(response);
                            arrivalDeviations.Add(arrivalDeviation);
                        }
                        else if (response.GetType() ==
                                 typeof (GetDirectJourneysBetweenStopsResponseDepartureDeviation))
                        {
                            var departureDeviation = ConvertToObject<DepartureDeviation>(response);
                            departureDeviations.Add(departureDeviation);
                        }
                        else if (response.GetType() ==
                                 typeof (GetDirectJourneysBetweenStopsResponseServiceJourneyDeviation))
                        {
                            var serviceJourneyDeviation =
                                ConvertToObject<ServiceJourneyDeviation>(response);
                            getDirectJourneysBetweenStopsResponse.ServiceJourneyDeviations.Add(
                                serviceJourneyDeviation);
                            serviceJourneyDeviations.Add(serviceJourneyDeviation);
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
                var exceptionMsg = "The application terminated with an error. Timestamp: " + ex.Detail.Timestamp +
                                   "Code: " + ex.Detail.ErrorCode +
                                   "Message: " + ex.Detail.Message +
                                   "Plugin Trace: " + ex.Detail.TraceText +
                                   "Inner Fault: " +
                                   (null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");

                throw new Exception(exceptionMsg, ex);
            }
            catch (TimeoutException ex)
            {
                var exceptionMsg = "The application terminated with an error. Message:" +
                                   ex.Message + " Stack Trace:" +
                                   ex.StackTrace + " Inner Fault: {0}" +
                                   ex.InnerException.Message;

                throw new Exception(exceptionMsg, ex);
            }
            catch (Exception ex)
            {
                // Display the details of the inner exception.
                if (ex.InnerException != null)
                {
                    var exceptionMsg = ex.InnerException.Message;

                    var fe = ex.InnerException
                        as FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>;

                    if (fe == null) throw new Exception(exceptionMsg, ex);

                    exceptionMsg = exceptionMsg + "The application terminated with an error. Timestamp: " +
                                   fe.Detail.Timestamp +
                                   "Code: " + fe.Detail.ErrorCode +
                                   "Message: " + fe.Detail.Message +
                                   "Plugin Trace: " + fe.Detail.TraceText +
                                   "Inner Fault: " +
                                   (null == fe.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                    throw new Exception(exceptionMsg, fe);
                }

                var exceptionMessage = "The application terminated with an error." + ex.Message;
                throw new Exception(exceptionMessage, ex);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromStopAreaGid"></param>
        /// <param name="toStopAreaGid"></param>
        /// <param name="journeyDateTime"></param>
        /// <param name="forLineGids"></param>
        /// <param name="transportType"></param>
        /// <returns></returns>
        internal GetDirectJourneysBetweenStopsResponse GetDirectJourneysbetweenStops(
            string fromStopAreaGid,
            string toStopAreaGid,
            DateTime journeyDateTime,
            string forLineGids,
            TransportType transportType)
        {
            try
            {
                var client = new DirectJourneysBetweenStopsClient();

                var getDirectJourneysBetweenStops = new GetDirectJourneysBetweenStops()
                {
                    fromStopAreaGid = fromStopAreaGid,
                    toStopAreaGid = toStopAreaGid,
                    forTimeWindowStartDateTime = journeyDateTime,
                    forTimeWindowStartDateTimeSpecified = true,
                    forTimeWindowDuration = 120,
                    forTimeWindowDurationSpecified = true,
                    withDepartureMaxCount = 9999,
                    withDepartureMaxCountSpecified = true,
                    forLineGids = transportType == TransportType.Train ? string.Empty : forLineGids,
                    forProducts = transportType == TransportType.Train ? "AOT" : string.Empty,
                    purposeOfLineGroupingCode = transportType == TransportType.Train ? "DISTRICT" : string.Empty

                };

                var responses = client.GetDirectJourneysBetweenStops(getDirectJourneysBetweenStops);

                var getDirectJourneysBetweenStopsResponse = new GetDirectJourneysBetweenStopsResponse();

                if (responses == null || responses.Length <= 0) return getDirectJourneysBetweenStopsResponse;

                var lstDirectJourneyBetweenStops =
                    (from x in responses.OfType<GetDirectJourneysBetweenStopsResponseDirectJourneysBetweenStops>()
                        select ConvertToObject<DirectJourneyBetweenStops>(x)).ToList();

                var lstDeviationMessageVersion =
                    (from x in responses.OfType<GetDirectJourneysBetweenStopsResponseDeviationMessageVersion>()
                        select ConvertToObject<DeviationMessageVersion>(x)).ToList();

                var lstDeviationMessageVariant =
                    (from x in responses.OfType<GetDirectJourneysBetweenStopsResponseDeviationMessageVariant>()
                        select ConvertToObject<DeviationMessageVariant>(x)).ToList();

                var lstArrivalDeviation =
                    (from x in responses.OfType<GetDirectJourneysBetweenStopsResponseArrivalDeviation>()
                        select ConvertToObject<ArrivalDeviation>(x)).ToList();


                if (lstArrivalDeviation.Count > 0)
                {
                    foreach (var arrivalDeviation in lstArrivalDeviation)
                    {

                        var affectedJourney = (from journey in lstDirectJourneyBetweenStops
                            where journey.ArrivalId == arrivalDeviation.IsOnArrivalId
                            select journey).FirstOrDefault();

                        var deviation = arrivalDeviation;
                        var deviationMsg = (from messageVariant in lstDeviationMessageVariant
                            where ((messageVariant.IsPartOfDeviationMessageId == deviation.HasDeviationMessageVersionId)
                                   &&
                                   (messageVariant.UsageTypeLongCode == "SUMMARY"))
                            select messageVariant.Content);

                        if (affectedJourney == null) continue;

                        affectedJourney.HasArrivalDeviation = true;
                        affectedJourney.ArrivalDeviationMessage += deviationMsg;
                    }
                }

                var lstDepartureDeviation =
                    (from x in responses.OfType<GetDirectJourneysBetweenStopsResponseDepartureDeviation>()
                        select ConvertToObject<DepartureDeviation>(x)).ToList();
                if (lstDepartureDeviation.Count > 0)
                {
                    foreach (var departureDeviation in lstDepartureDeviation)
                    {

                        var affectedJourney = (from journey in lstDirectJourneyBetweenStops
                            where journey.DepartureId == departureDeviation.IsOnDepartureId
                            select journey).FirstOrDefault();

                        var deviation = departureDeviation;
                        var deviationMsg = (from messageVariant in lstDeviationMessageVariant
                            where ((messageVariant.IsPartOfDeviationMessageId == deviation.HasDeviationMessageVersionId)
                                   &&
                                   (messageVariant.UsageTypeLongCode == "SUMMARY"))
                            select messageVariant.Content);

                        if (affectedJourney == null) continue;
                        affectedJourney.HasDepartureDeviation = true;
                        affectedJourney.DepartureDeviationMessage += deviationMsg;
                    }
                }
                var lstServiceJourneyDeviation =
                    (from x in responses.OfType<GetDirectJourneysBetweenStopsResponseServiceJourneyDeviation>()
                        select ConvertToObject<ServiceJourneyDeviation>(x)).ToList();

                if (lstServiceJourneyDeviation.Count > 0)
                {
                    foreach (var serviceJourneyDeviation in lstServiceJourneyDeviation)
                    {

                        var affectedJourney = (from journey in lstDirectJourneyBetweenStops
                            where journey.DatedVehicleJourneyId == serviceJourneyDeviation.IsOnDatedVehicleJourneyId
                            select journey).FirstOrDefault();

                        var deviation = serviceJourneyDeviation;
                        var deviationMsg = (from messageVariant in lstDeviationMessageVariant
                            where ((messageVariant.IsPartOfDeviationMessageId == deviation.HasDeviationMessageVersionId)
                                   &&
                                   (messageVariant.UsageTypeLongCode == "SUMMARY"))
                            select messageVariant.Content);

                        if (affectedJourney != null)
                        {
                            affectedJourney.HasServiceJourneyDeviation = true;
                            affectedJourney.ServiceJourneyDeviationMessage += deviationMsg;
                        }
                    }
                }

                getDirectJourneysBetweenStopsResponse.DirectJourneysBetweenStops =
                    new ObservableCollection<DirectJourneyBetweenStops>(lstDirectJourneyBetweenStops);
                getDirectJourneysBetweenStopsResponse.DeviationMessageVersions =
                    new ObservableCollection<DeviationMessageVersion>(lstDeviationMessageVersion);
                getDirectJourneysBetweenStopsResponse.DeviationMessageVariants =
                    new ObservableCollection<DeviationMessageVariant>(lstDeviationMessageVariant);
                getDirectJourneysBetweenStopsResponse.ServiceJourneyDeviations =
                    new ObservableCollection<ServiceJourneyDeviation>(lstServiceJourneyDeviation);
                getDirectJourneysBetweenStopsResponse.ArrivalDeviations =
                    new ObservableCollection<ArrivalDeviation>(lstArrivalDeviation);
                getDirectJourneysBetweenStopsResponse.DepartureDeviations =
                    new ObservableCollection<DepartureDeviation>(lstDepartureDeviation);

                return getDirectJourneysBetweenStopsResponse;
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                string exceptionMsg = "The application terminated with an error. Timestamp: " + ex.Detail.Timestamp +
                                      "Code: " + ex.Detail.ErrorCode +
                                      "Message: " + ex.Detail.Message +
                                      "Plugin Trace: " + ex.Detail.TraceText +
                                      "Inner Fault: " +
                                      (null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");

                throw new Exception(exceptionMsg, ex);
            }
            catch (TimeoutException ex)
            {
                var exceptionMsg = "The application terminated with an error. Message:" +
                                   ex.Message + " Stack Trace:" +
                                   ex.StackTrace + " Inner Fault: {0}" +
                                   ex.InnerException.Message;

                throw new Exception(exceptionMsg, ex);
            }
            catch (Exception ex)
            {
                // Display the details of the inner exception.
                if (ex.InnerException != null)
                {
                    var exceptionMsg = ex.InnerException.Message;

                    var fe = ex.InnerException
                        as FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>;

                    if (fe == null) throw new Exception(exceptionMsg, ex);

                    exceptionMsg = exceptionMsg + "The application terminated with an error. Timestamp: " +
                                   fe.Detail.Timestamp +
                                   "Code: " + fe.Detail.ErrorCode +
                                   "Message: " + fe.Detail.Message +
                                   "Plugin Trace: " + fe.Detail.TraceText +
                                   "Inner Fault: " +
                                   (null == fe.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");

                    throw new Exception(exceptionMsg, fe);
                }

                var exceptionMessage = "The application terminated with an error." + ex.Message;
                throw new Exception(exceptionMessage, ex);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceJourneyId"></param>
        /// <param name="operatingDate"></param>
        /// <param name="atStopGid"></param>
        /// <param name="includeArrivalTable"></param>
        /// <param name="includeDepartureTable"></param>
        /// <param name="includeDeviationTable"></param>
        /// <returns></returns>
        internal CallsForServiceJourneyResponse GetCallsForServiceJourney(string serviceJourneyId,
            DateTime operatingDate, string atStopGid, bool includeArrivalTable = true, bool includeDepartureTable = true,
            bool includeDeviationTable = true)
        {
            var callsForServiceJourneyResponse = new CallsForServiceJourneyResponse();

            try
            {
                CallsForServiceJourneyClient client = new CallsForServiceJourneyClient();

                GetCallsforServiceJourney callsForServiceJourney = new GetCallsforServiceJourney()
                {
                    forServiceJourneyIdOrGid = serviceJourneyId,
                    atOperatingDate = operatingDate,
                    atOperatingDateSpecified = true,
                    atStopGid = atStopGid,
                    includeArrivalsTable = includeArrivalTable,
                    includeArrivalsTableSpecified = true,
                    includeDeparturesTable = includeDepartureTable,
                    includeDeparturesTableSpecified = true,
                    includeDeviationTables = includeDeviationTable,
                    includeDeviationTablesSpecified = true
                };

                var responses = client.GetCallsForServiceJourney(callsForServiceJourney);

                if (responses != null && responses.Length > 0)
                {

                    var lstDatedServiceJourney =
                        (from x in responses.OfType<GetCallsforServiceJourneyMethodDatedServiceJourney>()
                            select ConvertToObject<DatedServiceJourney>(x)).ToList();

                    var lstDatedArrival =
                        (from x in responses.OfType<GetCallsforServiceJourneyMethodDatedArrival>()
                            select ConvertToObject<DatedArrival>(x)).ToList();

                    var lstDatedDeparture =
                        (from x in responses.OfType<GetCallsforServiceJourneyMethodDatedDeparture>()
                            select ConvertToObject<DatedDeparture>(x)).ToList();

                    var lstDeviationMessageVersion =
                        (from x in responses.OfType<GetCallsforServiceJourneyMethodDeviationMessageVersion>()
                            select ConvertToObject<DeviationMessageVersion1>(x)).ToList();

                    var lstDeviationMessageVariant =
                        (from x in responses.OfType<GetCallsforServiceJourneyMethodDeviationMessageVariant>()
                            select ConvertToObject<DeviationMessageVariant1>(x)).ToList();

                    var lstArrivalDeviation =
                        (from x in responses.OfType<GetCallsforServiceJourneyMethodArrivalDeviation>()
                            select ConvertToObject<ArrivalDeviation1>(x)).ToList();

                    var lstDepartureDeviation =
                        (from x in responses.OfType<GetCallsforServiceJourneyMethodDepartureDeviation>()
                            select ConvertToObject<DepartureDeviation1>(x)).ToList();

                    var lstServiceJourneyDeviation =
                        (from x in responses.OfType<GetCallsforServiceJourneyMethodServiceJourneyDeviation>()
                            select ConvertToObject<ServiceJourneyDeviation1>(x)).ToList();

                    callsForServiceJourneyResponse.DatedServiceJourneys =
                        new ObservableCollection<DatedServiceJourney>(lstDatedServiceJourney);
                    callsForServiceJourneyResponse.DatedArrivals =
                        new ObservableCollection<DatedArrival>(lstDatedArrival);
                    callsForServiceJourneyResponse.DatedDepartures =
                        new ObservableCollection<DatedDeparture>(lstDatedDeparture);
                    callsForServiceJourneyResponse.DeviationMessageVersions =
                        new ObservableCollection<DeviationMessageVersion1>(lstDeviationMessageVersion);
                    callsForServiceJourneyResponse.DeviationMessageVariants =
                        new ObservableCollection<DeviationMessageVariant1>(lstDeviationMessageVariant);
                    callsForServiceJourneyResponse.ServiceJourneyDeviations =
                        new ObservableCollection<ServiceJourneyDeviation1>(lstServiceJourneyDeviation);
                    callsForServiceJourneyResponse.ArrivalDeviations =
                        new ObservableCollection<ArrivalDeviation1>(lstArrivalDeviation);
                    callsForServiceJourneyResponse.DepartureDeviations =
                        new ObservableCollection<DepartureDeviation1>(lstDepartureDeviation);
                    callsForServiceJourneyResponse.ErrorMessage = string.Empty;
                }

            }
            catch (Exception ex)
            {
                callsForServiceJourneyResponse.ErrorMessage = ex.Message;
            }

            return callsForServiceJourneyResponse;

        }

        internal OrganisationalUnitResponse GetOrganisationalUnits()
        {
            var response = new OrganisationalUnitResponse();
            try
            {
                //SP_GetOrganisationalUnits
                var sqlCon = _xrmHelper.OpenSql();
                using (var command = new SqlCommand("SP_GetOrganisationalUnits", sqlCon))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    var reader = command.ExecuteXmlReader();
                    OrganisationalUnits retList;
                    {
                        var xmlSerializer = new XmlSerializer(typeof (OrganisationalUnits));
                        retList = xmlSerializer.Deserialize(reader) as OrganisationalUnits;
                    }
                    reader.Close();
                    _xrmHelper.CloseSql(sqlCon);

                    if (retList != null && retList.OrganisationalUnitList != null &&
                        retList.OrganisationalUnitList.Any())
                    {
                        response.OrganisationalUnitList = retList.OrganisationalUnitList.ToList();
                        response.Errormessage = null;
                    }
                    else
                    {
                        response.OrganisationalUnitList = null;
                        response.Errormessage = "Finns inga OrganisationalUnits!";
                    }
                }
            }
            catch (Exception ex)
            {
                response.OrganisationalUnitList = null;
                response.Errormessage = ex.Message;
            }
            return response;
        }

        internal ContractorResponse GetContractors()
        {
            var response = new ContractorResponse();
            try
            {
                //SP_GetContractors
                var sqlCon = _xrmHelper.OpenSql();
                using (var command = new SqlCommand("SP_GetContractors", sqlCon))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    var reader = command.ExecuteXmlReader();
                    Contractors retList;
                    {
                        var xmlSerializer = new XmlSerializer(typeof (Contractors));
                        retList = xmlSerializer.Deserialize(reader) as Contractors;
                    }
                    reader.Close();
                    _xrmHelper.CloseSql(sqlCon);

                    if (retList != null && retList.ContractorList != null && retList.ContractorList.Any())
                    {
                        response.ContractorList = retList.ContractorList.ToList();
                        response.Errormessage = null;
                    }
                    else
                    {
                        response.ContractorList = null;
                        response.Errormessage = "Finns inga contractors!";
                    }
                }
            }
            catch (Exception ex)
            {
                response.ContractorList = null;
                response.Errormessage = ex.Message;
            }
            return response;

        }

        #endregion
    }
}