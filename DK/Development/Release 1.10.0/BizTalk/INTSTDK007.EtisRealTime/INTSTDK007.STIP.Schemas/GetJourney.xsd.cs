namespace INTSTDK007.STIP.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"GetJourney", @"GetJourneyResult"})]
    public sealed class GetJourneyType : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://www.skanetrafiken.com/DK/INTSTDK007/GetJourney/20141216"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" targetNamespace=""http://www.skanetrafiken.com/DK/INTSTDK007/GetJourney/20141216"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""GetJourney"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""1"" maxOccurs=""1"" name=""FromPointId"" type=""xs:int"">
          <xs:annotation>
            <xs:documentation>
              From Id as obtained in response from GetStartEndPoint
              <xs:restriction base=""xs:int"" xmlns:xs=""http://www.w3.org/2001/XMLSchema""></xs:restriction></xs:documentation>
          </xs:annotation>
        </xs:element>
        <xs:element minOccurs=""1"" maxOccurs=""1"" name=""FromPointType"" type=""PointType"">
          <xs:annotation>
            <xs:documentation>
              Type of point - 0 (STOP_AREA), 1 (ADDRESS) or 2 (POI)
              <xs:restriction base=""xs:string"" xmlns:xs=""http://www.w3.org/2001/XMLSchema""><xs:enumeration value=""PointType""></xs:enumeration></xs:restriction></xs:documentation>
          </xs:annotation>
        </xs:element>
        <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ToPointId"" type=""xs:int"">
          <xs:annotation>
            <xs:documentation>
              To Id as obtained in response from GetStartEndPoint
              <xs:restriction base=""xs:int"" xmlns:xs=""http://www.w3.org/2001/XMLSchema""></xs:restriction></xs:documentation>
          </xs:annotation>
        </xs:element>
        <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ToPointType"" type=""PointType"">
          <xs:annotation>
            <xs:documentation>
              Type of point - 0 (STOP_AREA), 1 (ADDRESS) or 2 (POI)
              <xs:restriction base=""xs:string"" xmlns:xs=""http://www.w3.org/2001/XMLSchema""><xs:enumeration value=""PointType""></xs:enumeration></xs:restriction></xs:documentation>
          </xs:annotation>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""ViaPointId"" type=""xs:int"">
          <xs:annotation>
            <xs:documentation>
              [Optional] Via Id as obtained in response from GetStartEndPoint
              <xs:restriction base=""xs:int"" xmlns:xs=""http://www.w3.org/2001/XMLSchema""></xs:restriction></xs:documentation>
          </xs:annotation>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""WaitingTime"" type=""xs:int"">
          <xs:annotation>
            <xs:documentation>
              [Optional] Wait time at the via point in minutes, the default is 30
              <xs:restriction base=""xs:positiveInteger"" xmlns:xs=""http://www.w3.org/2001/XMLSchema""><xs:minInclusive value=""1"" /><xs:maxInclusive value=""999"" /></xs:restriction></xs:documentation>
          </xs:annotation>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""JourneyDateTime"" type=""xs:dateTime"">
          <xs:annotation>
            <xs:documentation>
              [Optional] Date and time for journey (yyyy-mm-dd hh:mm:ss), the default is current datetime
              <xs:restriction base=""xs:dateTime""></xs:restriction></xs:documentation>
          </xs:annotation>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Direction"" type=""DirectionType"">
          <xs:annotation>
            <xs:documentation>
              [Optional] Denotes if JourneyDateTime element reffers to Deparure or Arrival time, the default is 0 (DEPARTURE)
              <xs:restriction base=""xs:string""><xs:enumeration value=""DirectionType""></xs:enumeration></xs:restriction></xs:documentation>
          </xs:annotation>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""NoOfJourneysBefore"" type=""xs:int"">
          <xs:annotation>
            <xs:documentation>
              [Optional] No of journey alternatives in result before the searched DateTime, the default is 1
              <xs:restriction base=""xs:positiveInteger""><xs:minInclusive value=""0"" /><xs:maxInclusive value=""10"" /></xs:restriction></xs:documentation>
          </xs:annotation>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""NoOfJourneysAfter"" type=""xs:int"">
          <xs:annotation>
            <xs:documentation>
              [Optional] No of journey alternatives in result after the searched DateTime, the default is 3
              <xs:restriction base=""xs:positiveInteger""><xs:minInclusive value=""0"" /><xs:maxInclusive value=""10"" /></xs:restriction></xs:documentation>
          </xs:annotation>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""ChangeTime"" type=""ChangeTimeType"">
          <xs:annotation>
            <xs:documentation>
              [Optional] Change time (walk time between connection points), the default is 0 (STANDARD)
              <xs:restriction base=""xs:string""><xs:enumeration value=""ChangeTimeType""></xs:enumeration></xs:restriction></xs:documentation>
          </xs:annotation>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Priority"" type=""PriorityType"">
          <xs:annotation>
            <xs:documentation>
              [Optional] Shortest journey time alt. fewer transfers, the default is 0 (SHORTEST_TIME)
              <xs:restriction base=""xs:string""><xs:enumeration value=""PriorityType""></xs:enumeration></xs:restriction></xs:documentation>
          </xs:annotation>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""SelectedMeansOfTransport"" type=""xs:int"">
          <xs:annotation>
            <xs:documentation>
              [Optional] Search for journeys with particular selection of transport means, int value = Sum(selected Id's). The default is all available transport means
            </xs:documentation>
          </xs:annotation>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""SelectionType"" type=""MeansOfTransportType"">
          <xs:annotation>
            <xs:documentation>
              [Optional] Denotes if items in MeansOfTransportSelection are TRANSPORT_MODES or LINE_TYPES. The default is 0 (TRANSPORT_MODES)
              <xs:restriction base=""xs:string""><xs:enumeration value=""MeansOfTransportType""></xs:enumeration></xs:restriction></xs:documentation>
          </xs:annotation>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Accessibility"" type=""DisabilityType"">
          <xs:annotation>
            <xs:documentation>
              [Optional] Journey must be adopted to customer with some kind of disabilities (R,S or H). The default is 0 (without any dissability)
              <xs:restriction base=""xs:string""><xs:enumeration value=""DisabilityType""></xs:enumeration></xs:restriction></xs:documentation>
          </xs:annotation>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""MaxWalkDistance"" type=""xs:int"">
          <xs:annotation>
            <xs:documentation>
              [Optional] Max walk distance in meters, the default is 3000
              <xs:restriction base=""xs:positiveInteger""><xs:minInclusive value=""10"" /><xs:maxInclusive value=""5000"" /></xs:restriction></xs:documentation>
          </xs:annotation>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""DetailedResult"" type=""xs:boolean"">
          <xs:annotation>
            <xs:documentation>
              [Optional] Denotes if detailed itinerary (PointsOnRouteLink) should be included in results, the default is False - without PointsOnRouteLink info
              <xs:restriction base=""xs:boolean""></xs:restriction></xs:documentation>
          </xs:annotation>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""WalkSpeed"" type=""WalkSpeedType"">
          <xs:annotation>
            <xs:documentation>
              [Optional] Walk speed preference
            </xs:documentation>
          </xs:annotation>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""VehicleAccessibility"" type=""VehicleAccessibilityType"">
          <xs:annotation>
            <xs:documentation>
              [Optional] Vehicle accessibility type L N R (low, normal, wheelchair)
            </xs:documentation>
          </xs:annotation>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:simpleType name=""PointType"">
    <xs:annotation>
      <xs:documentation>Denotes types of from / to points.</xs:documentation>
    </xs:annotation>
    <xs:restriction base=""xs:token"">
      <xs:enumeration value=""STOP_AREA"" />
      <xs:enumeration value=""ADDRESS"" />
      <xs:enumeration value=""POI"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""ChangeTimeType"">
    <xs:annotation>
      <xs:documentation>Denotes types of from / to points.</xs:documentation>
    </xs:annotation>
    <xs:restriction base=""xs:token"">
      <xs:enumeration value=""NORMAL"" />
      <xs:enumeration value=""NORMAL_PLUS_5MIN"" />
      <xs:enumeration value=""NORMAL_PLUS_10MIN"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""PriorityType"">
    <xs:annotation>
      <xs:documentation>Denotes types of from / to points.</xs:documentation>
    </xs:annotation>
    <xs:restriction base=""xs:token"">
      <xs:enumeration value=""SHORTEST_TIME"" />
      <xs:enumeration value=""FEWER_CHANGES"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""MeansOfTransportType"">
    <xs:annotation>
      <xs:documentation>Denotes two different types for defining means of transportation.</xs:documentation>
    </xs:annotation>
    <xs:restriction base=""xs:token"">
      <xs:enumeration value=""TRANSPORT_MODES"" />
      <xs:enumeration value=""LINE_TYPES"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""DisabilityType"">
    <xs:annotation>
      <xs:documentation>Denotes types of handicap disabilities.</xs:documentation>
    </xs:annotation>
    <xs:restriction base=""xs:token"">
      <xs:enumeration value=""WITHOUT"" />
      <xs:enumeration value=""R"" />
      <xs:enumeration value=""S"" />
      <xs:enumeration value=""H"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""DirectionType"">
    <xs:annotation>
      <xs:documentation>Denotes if searched datetime is departure time or arrival time</xs:documentation>
    </xs:annotation>
    <xs:restriction base=""xs:token"">
      <xs:enumeration value=""DEPARTURE"" />
      <xs:enumeration value=""ARRIVAL"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""WalkSpeedType"">
    <xs:annotation>
      <xs:documentation>Denotes types of from / to points.</xs:documentation>
    </xs:annotation>
    <xs:restriction base=""xs:token"">
      <xs:enumeration value=""NORMAL"" />
      <xs:enumeration value=""SLOW"" />
      <xs:enumeration value=""FAST"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""VehicleAccessibilityType"">
    <xs:annotation>
      <xs:documentation>Denotes types of handicap disabilities.</xs:documentation>
    </xs:annotation>
    <xs:restriction base=""xs:token"">
      <xs:enumeration value=""WITHOUT"" />
      <xs:enumeration value=""L"" />
      <xs:enumeration value=""R"" />
      <xs:enumeration value=""N"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:element name=""GetJourneyResult"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Code"" type=""xs:int"">
          <xs:annotation>
            <xs:documentation>Response code, 0 if successful operation</xs:documentation>
          </xs:annotation>
        </xs:element>
        <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Message"" type=""xs:string"">
          <xs:annotation>
            <xs:documentation xml:lang=""se"">Error message, empty if response code = 0</xs:documentation>
          </xs:annotation>
        </xs:element>
        <xs:element name=""JourneyResultKey"" type=""xs:string"">
          <xs:annotation>
            <xs:documentation>
              Used to uniquely identify resultset. This reference can be later passed to GetCachedResult function
              in order to retrieve previous resultset without making a new call to search engines
            </xs:documentation>
          </xs:annotation>
        </xs:element>
        <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Journeys"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""Journey"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element minOccurs=""1"" maxOccurs=""1"" name=""SequenceNo"" type=""xs:int"">
                      <xs:annotation>
                        <xs:documentation>Position in an ordered list of Journeys</xs:documentation>
                      </xs:annotation>
                    </xs:element>
                    <xs:element minOccurs=""1"" maxOccurs=""1"" name=""DepDateTime"" type=""xs:dateTime"">
                      <xs:annotation>
                        <xs:documentation>Departure date and time. </xs:documentation>
                      </xs:annotation>
                    </xs:element>
                    <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ArrDateTime"" type=""xs:dateTime"">
                      <xs:annotation>
                        <xs:documentation>Arrival date and time.</xs:documentation>
                      </xs:annotation>
                    </xs:element>
                    <xs:element minOccurs=""1"" maxOccurs=""1"" name=""DepWalkDist"" type=""xs:int"">
                      <xs:annotation>
                        <xs:documentation>Walk distance in m. between starting point for journey (if address or POI) and Stop Area for departure</xs:documentation>
                      </xs:annotation>
                    </xs:element>
                    <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ArrWalkDist"" type=""xs:int"">
                      <xs:annotation>
                        <xs:documentation>Walk distance in m. between Stop Area for arrival and journey end point (if address or POI)</xs:documentation>
                      </xs:annotation>
                    </xs:element>
                    <xs:element minOccurs=""1"" maxOccurs=""1"" name=""NoOfChanges"" type=""xs:int"">
                      <xs:annotation>
                        <xs:documentation>No of changes</xs:documentation>
                      </xs:annotation>
                    </xs:element>
                    <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Guaranteed"" type=""xs:boolean"">
                      <xs:annotation>
                        <xs:documentation>Denotes if journey is guaranteed by transport authority, according to rules for ""Travel Guarnantee""</xs:documentation>
                      </xs:annotation>
                    </xs:element>
                    <xs:element minOccurs=""1"" maxOccurs=""1"" name=""CO2factor"" type=""xs:int"">
                      <xs:annotation>
                        <xs:documentation>
                          Journeys impact on the environment - environmental index based on the carbon dioxide (CO2) emissions, Values between 0(lowest impact) and 100
                          <xs:restriction base=""xs:positiveInteger""><xs:minInclusive value=""0"" /><xs:maxInclusive value=""100"" /></xs:restriction></xs:documentation>
                      </xs:annotation>
                    </xs:element>
                    <xs:element minOccurs=""1"" maxOccurs=""1"" name=""NoOfZones"" type=""xs:int"">
                      <xs:annotation>
                        <xs:documentation>No of passing zones in a zoned fare stucture defined by transport authority</xs:documentation>
                      </xs:annotation>
                    </xs:element>
                    <xs:element minOccurs=""1"" maxOccurs=""1"" name=""PriceZoneList"" type=""xs:string"">
                      <xs:annotation>
                        <xs:documentation>Comma separated list of passing price zones</xs:documentation>
                      </xs:annotation>
                    </xs:element>
                    <xs:element minOccurs=""0"" maxOccurs=""1"" name=""FareType"" type=""xs:string"">
                      <xs:annotation>
                        <xs:documentation>Current fare type</xs:documentation>
                      </xs:annotation>
                    </xs:element>
                    <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Prices"">
                      <xs:complexType>
                        <xs:sequence>
                          <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""PriceInfo"">
                            <xs:complexType>
                              <xs:sequence>
                                <xs:element minOccurs=""1"" maxOccurs=""1"" name=""PriceType"" type=""xs:string"">
                                  <xs:annotation>
                                    <xs:documentation>Price type description (single fare, discount card etc.)</xs:documentation>
                                  </xs:annotation>
                                </xs:element>
                                <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Price"" type=""xs:float"">
                                  <xs:annotation>
                                    <xs:documentation>Price for journey, in SEK</xs:documentation>
                                  </xs:annotation>
                                </xs:element>
                                <xs:element minOccurs=""1"" maxOccurs=""1"" name=""VAT"" type=""xs:float"">
                                  <xs:annotation>
                                    <xs:documentation>VAT, in SEK</xs:documentation>
                                  </xs:annotation>
                                </xs:element>
                                <xs:element minOccurs=""0"" maxOccurs=""1"" name=""JourneyTicketKey"" type=""xs:string"">
                                  <xs:annotation>
                                    <xs:documentation>Unique ticket key</xs:documentation>
                                  </xs:annotation>
                                </xs:element>
                                <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Counties"">
                                  <xs:complexType>
                                    <xs:sequence>
                                      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""CountyInfo"">
                                        <xs:complexType>
                                          <xs:sequence>
                                            <xs:element minOccurs=""1"" maxOccurs=""1"" name=""CountyCode"" type=""xs:string"">
                                              <xs:annotation>
                                                <xs:documentation>County code if journey through several counties</xs:documentation>
                                              </xs:annotation>
                                            </xs:element>
                                            <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Price"" type=""xs:float"">
                                              <xs:annotation>
                                                <xs:documentation>Price for journey in county, in SEK</xs:documentation>
                                              </xs:annotation>
                                            </xs:element>
                                          </xs:sequence>
                                        </xs:complexType>
                                      </xs:element>
                                    </xs:sequence>
                                  </xs:complexType>
                                </xs:element>
                              </xs:sequence>
                            </xs:complexType>
                          </xs:element>
                        </xs:sequence>
                      </xs:complexType>
                    </xs:element>
                    <xs:element minOccurs=""1"" maxOccurs=""1"" name=""JourneyKey"" type=""xs:string"">
                      <xs:annotation>
                        <xs:documentation>
                          Used by the Elmer seach engine to identify an object uniquely in the scope of a traffic data.
                          Information may be used by back-end services like Map Service to draw itinerary on map.
                        </xs:documentation>
                      </xs:annotation>
                    </xs:element>
                    <xs:element minOccurs=""1"" maxOccurs=""1"" name=""RouteLinks"">
                      <xs:complexType>
                        <xs:sequence>
                          <xs:element minOccurs=""1"" maxOccurs=""unbounded"" name=""RouteLink"">
                            <xs:complexType>
                              <xs:sequence>
                                <xs:element minOccurs=""1"" maxOccurs=""1"" name=""RouteLinkKey"" type=""xs:string"">
                                  <xs:annotation>
                                    <xs:documentation>Used by the Elmer seach engine to identify an object uniquely in the scope of a traffic data.</xs:documentation>
                                  </xs:annotation>
                                </xs:element>
                                <xs:element minOccurs=""1"" maxOccurs=""1"" name=""DepDateTime"" type=""xs:dateTime"">
                                  <xs:annotation>
                                    <xs:documentation>Departure date and time</xs:documentation>
                                  </xs:annotation>
                                </xs:element>
                                <xs:element minOccurs=""1"" maxOccurs=""1"" name=""DepIsTimingPoint"" type=""xs:boolean"">
                                  <xs:annotation>
                                    <xs:documentation>Denotes if Departure node is a timing point. False means that DepDateTime is aproximated time</xs:documentation>
                                  </xs:annotation>
                                </xs:element>
                                <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ArrDateTime"" type=""xs:dateTime"">
                                  <xs:annotation>
                                    <xs:documentation>Arrival date and time</xs:documentation>
                                  </xs:annotation>
                                </xs:element>
                                <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ArrIsTimingPoint"" type=""xs:boolean"">
                                  <xs:annotation>
                                    <xs:documentation>Denotes if Arrival node is a timing point. False means that ArrDateTime is aproximated time</xs:documentation>
                                  </xs:annotation>
                                </xs:element>
                                <xs:element minOccurs=""1"" maxOccurs=""1"" name=""CallTrip"" type=""xs:boolean"">
                                  <xs:annotation>
                                    <xs:documentation>Denotes if vehicle run must be called in advance ex. by phone</xs:documentation>
                                  </xs:annotation>
                                </xs:element>
                                <xs:element minOccurs=""1"" maxOccurs=""1"" name=""PriceZones"">
                                  <xs:complexType>
                                    <xs:sequence>
                                      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""PriceZone"">
                                        <xs:complexType>
                                          <xs:sequence>
                                            <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Id"" type=""xs:int"">
                                              <xs:annotation>
                                                <xs:documentation>Price zone's id</xs:documentation>
                                              </xs:annotation>
                                            </xs:element>
                                          </xs:sequence>
                                        </xs:complexType>
                                      </xs:element>
                                    </xs:sequence>
                                  </xs:complexType>
                                </xs:element>
                                <xs:element minOccurs=""1"" maxOccurs=""1"" name=""RealTime"">
                                  <xs:complexType>
                                    <xs:sequence>
                                      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""RealTimeInfo"">
                                        <xs:complexType>
                                          <xs:sequence>
                                            <xs:element minOccurs=""0"" maxOccurs=""1"" name=""NewDepPoint"" type=""xs:string"">
                                              <xs:annotation>
                                                <xs:documentation>Stop Point deviation from timetable (on departure side). Information about new Stop Point</xs:documentation>
                                              </xs:annotation>
                                            </xs:element>
                                            <xs:element minOccurs=""0"" maxOccurs=""1"" name=""NewArrPoint"" type=""xs:string"">
                                              <xs:annotation>
                                                <xs:documentation>Stop Point deviation from timetable (on arrival side). Information about new Stop Point</xs:documentation>
                                              </xs:annotation>
                                            </xs:element>
                                            <xs:element minOccurs=""0"" maxOccurs=""1"" name=""DepTimeDeviation"" type=""xs:int"">
                                              <xs:annotation>
                                                <xs:documentation>Deviation from timetable time in min. (on departure side). Delays are positive integer values and earlier times are negative.</xs:documentation>
                                              </xs:annotation>
                                            </xs:element>
                                            <xs:element minOccurs=""0"" maxOccurs=""1"" name=""DepDeviationAffect"" type=""RealTimeAffect"">
                                              <xs:annotation>
                                                <xs:documentation>Describes how departure time deviation affects the journey.</xs:documentation>
                                              </xs:annotation>
                                            </xs:element>
                                            <xs:element minOccurs=""0"" maxOccurs=""1"" name=""ArrTimeDeviation"" type=""xs:int"">
                                              <xs:annotation>
                                                <xs:documentation>Deviation from timetable time in min. (on arrival side). Delays are positive integer values and earlier times are negative.</xs:documentation>
                                              </xs:annotation>
                                            </xs:element>
                                            <xs:element minOccurs=""0"" maxOccurs=""1"" name=""ArrDeviationAffect"" type=""RealTimeAffect"">
                                              <xs:annotation>
                                                <xs:documentation>Describes how arrival time deviation affects the journey.</xs:documentation>
                                              </xs:annotation>
                                            </xs:element>
                                            <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Canceled"" type=""xs:boolean"">
                                              <xs:annotation>
                                                <xs:documentation>Denotes if vehicle run is canceled. This event can impact the whole journey</xs:documentation>
                                              </xs:annotation>
                                            </xs:element>
                                          </xs:sequence>
                                        </xs:complexType>
                                      </xs:element>
                                      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""RealTimeRoadInfo"">
                                        <xs:annotation>
                                          <xs:documentation>RoadInfo can be present only if journey is a car route.</xs:documentation>
                                        </xs:annotation>
                                        <xs:complexType>
                                          <xs:sequence>
                                            <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Road"" type=""xs:string"">
                                              <xs:annotation>
                                                <xs:documentation>Road number</xs:documentation>
                                              </xs:annotation>
                                            </xs:element>
                                            <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Reason"" type=""RoadInfoType"">
                                              <xs:annotation>
                                                <xs:documentation>Type of event</xs:documentation>
                                              </xs:annotation>
                                            </xs:element>
                                            <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Info"" type=""xs:string"">
                                              <xs:annotation>
                                                <xs:documentation>Info text</xs:documentation>
                                              </xs:annotation>
                                            </xs:element>
                                          </xs:sequence>
                                        </xs:complexType>
                                      </xs:element>
                                    </xs:sequence>
                                  </xs:complexType>
                                </xs:element>
                                <xs:element minOccurs=""1"" maxOccurs=""1"" name=""From"">
                                  <xs:complexType>
                                    <xs:sequence>
                                      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Id"" type=""xs:int"">
                                        <xs:annotation>
                                          <xs:documentation>Unique Stop area ID</xs:documentation>
                                        </xs:annotation>
                                      </xs:element>
                                      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Name"" type=""xs:string"">
                                        <xs:annotation>
                                          <xs:documentation>Stop area name</xs:documentation>
                                        </xs:annotation>
                                      </xs:element>
                                      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""StopPoint"" type=""xs:string"">
                                        <xs:annotation>
                                          <xs:documentation>Stop point name within the Stop Area</xs:documentation>
                                        </xs:annotation>
                                      </xs:element>
                                    </xs:sequence>
                                  </xs:complexType>
                                </xs:element>
                                <xs:element minOccurs=""1"" maxOccurs=""1"" name=""To"">
                                  <xs:complexType>
                                    <xs:sequence>
                                      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Id"" type=""xs:int"">
                                        <xs:annotation>
                                          <xs:documentation>Unique Stop area ID</xs:documentation>
                                        </xs:annotation>
                                      </xs:element>
                                      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Name"" type=""xs:string"">
                                        <xs:annotation>
                                          <xs:documentation>Stop area name</xs:documentation>
                                        </xs:annotation>
                                      </xs:element>
                                      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""StopPoint"" type=""xs:string"">
                                        <xs:annotation>
                                          <xs:documentation>Stop point name</xs:documentation>
                                        </xs:annotation>
                                      </xs:element>
                                    </xs:sequence>
                                  </xs:complexType>
                                </xs:element>
                                <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Line"">
                                  <xs:complexType>
                                    <xs:sequence>
                                      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Name"" type=""xs:string"">
                                        <xs:annotation>
                                          <xs:documentation>Line's name</xs:documentation>
                                        </xs:annotation>
                                      </xs:element>
                                      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""No"" type=""xs:int"">
                                        <xs:annotation>
                                          <xs:documentation>Line's number</xs:documentation>
                                        </xs:annotation>
                                      </xs:element>
                                      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""RunNo"" type=""xs:int"">
                                        <xs:annotation>
                                          <xs:documentation>Line's run number</xs:documentation>
                                        </xs:annotation>
                                      </xs:element>
                                      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""LineTypeId"" type=""xs:int"">
                                        <xs:annotation>
                                          <xs:documentation>Reference to one line type in line types collection defined by transport authority. All available line types and ids can be retreved from GetMeansOfTransport function</xs:documentation>
                                        </xs:annotation>
                                      </xs:element>
                                      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""LineTypeName"" type=""xs:string"">
                                        <xs:annotation>
                                          <xs:documentation>Line type name</xs:documentation>
                                        </xs:annotation>
                                      </xs:element>
                                      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""TransportModeId"" type=""xs:int"">
                                        <xs:annotation>
                                          <xs:documentation>Reference to one TransportMode in modes collection defined by transport authority. All available TransportModes and ids can be retreved from GetMeansOfTransport function</xs:documentation>
                                        </xs:annotation>
                                      </xs:element>
                                      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""TransportModeName"" type=""xs:string"">
                                        <xs:annotation>
                                          <xs:documentation>Transport mode name</xs:documentation>
                                        </xs:annotation>
                                      </xs:element>
                                      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""TrainNo"" type=""xs:int"">
                                        <xs:annotation>
                                          <xs:documentation>Additional info about train number if route link's line type is train</xs:documentation>
                                        </xs:annotation>
                                      </xs:element>
                                      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Towards"" type=""xs:string"">
                                        <xs:annotation>
                                          <xs:documentation>Destination text</xs:documentation>
                                        </xs:annotation>
                                      </xs:element>
                                      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""OperatorId"" type=""xs:int"">
                                        <xs:annotation>
                                          <xs:documentation>Vehicle operators unique id</xs:documentation>
                                        </xs:annotation>
                                      </xs:element>
                                      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""OperatorName"" type=""xs:string"">
                                        <xs:annotation>
                                          <xs:documentation>Vehicle operators name</xs:documentation>
                                        </xs:annotation>
                                      </xs:element>
                                      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""FootNotes"">
                                        <xs:complexType>
                                          <xs:sequence>
                                            <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""FootNote"">
                                              <xs:complexType>
                                                <xs:sequence>
                                                  <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Index"" type=""xs:string"">
                                                    <xs:annotation>
                                                      <xs:documentation>FootNote's index, allways unique in scope of journey</xs:documentation>
                                                    </xs:annotation>
                                                  </xs:element>
                                                  <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Text"" type=""xs:string"">
                                                    <xs:annotation>
                                                      <xs:documentation>FootNote's text</xs:documentation>
                                                    </xs:annotation>
                                                  </xs:element>
                                                </xs:sequence>
                                              </xs:complexType>
                                            </xs:element>
                                          </xs:sequence>
                                        </xs:complexType>
                                      </xs:element>
                                      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""PointsOnRouteLink"">
                                        <xs:annotation>
                                          <xs:documentation>Optional, this block of information is retrieved only if DetailedResult = True in request</xs:documentation>
                                        </xs:annotation>
                                        <xs:complexType>
                                          <xs:sequence>
                                            <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""PointOnRouteLink"">
                                              <xs:complexType>
                                                <xs:sequence>
                                                  <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Id"" type=""xs:int"">
                                                    <xs:annotation>
                                                      <xs:documentation>Stop Area id</xs:documentation>
                                                    </xs:annotation>
                                                  </xs:element>
                                                  <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Name"" type=""xs:string"">
                                                    <xs:annotation>
                                                      <xs:documentation>Stop Area name</xs:documentation>
                                                    </xs:annotation>
                                                  </xs:element>
                                                  <xs:element minOccurs=""1"" maxOccurs=""1"" name=""StopPoint"" type=""xs:string"">
                                                    <xs:annotation>
                                                      <xs:documentation>Stop Point name</xs:documentation>
                                                    </xs:annotation>
                                                  </xs:element>
                                                  <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ArrDateTime"" type=""xs:dateTime"">
                                                    <xs:annotation>
                                                      <xs:documentation>Arrival date and time</xs:documentation>
                                                    </xs:annotation>
                                                  </xs:element>
                                                  <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ArrIsTimingPoint"" type=""xs:boolean"">
                                                    <xs:annotation>
                                                      <xs:documentation>Denotes if Arrival node is timing point. False means that ArrDateTime is approximated time</xs:documentation>
                                                    </xs:annotation>
                                                  </xs:element>
                                                  <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Distance"" type=""xs:int"">
                                                    <xs:annotation>
                                                      <xs:documentation>Distance i meters, available if car or bike journey</xs:documentation>
                                                    </xs:annotation>
                                                  </xs:element>
                                                </xs:sequence>
                                              </xs:complexType>
                                            </xs:element>
                                          </xs:sequence>
                                        </xs:complexType>
                                      </xs:element>
                                      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Provider"" type=""xs:int"">
                                        <xs:annotation>
                                          <xs:documentation>Information provider</xs:documentation>
                                        </xs:annotation>
                                      </xs:element>
                                    </xs:sequence>
                                  </xs:complexType>
                                </xs:element>
                                <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Deviations"">
                                  <xs:complexType>
                                    <xs:sequence>
                                      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""Deviation"">
                                        <xs:complexType>
                                          <xs:sequence>
                                            <xs:element minOccurs=""1"" maxOccurs=""1"" name=""DeviationScopes"">
                                              <xs:complexType>
                                                <xs:sequence>
                                                  <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""DeviationScope"">
                                                    <xs:complexType>
                                                      <xs:sequence>
                                                        <xs:element minOccurs=""1"" maxOccurs=""unbounded"" name=""ScopeAttribute"" type=""ScopeAttributeType"" />
                                                        <xs:element name=""FromDateTime"" type=""xs:dateTime"" />
                                                        <xs:element name=""ToDateTime"" type=""xs:dateTime"" />
                                                      </xs:sequence>
                                                    </xs:complexType>
                                                  </xs:element>
                                                </xs:sequence>
                                              </xs:complexType>
                                            </xs:element>
                                            <xs:element minOccurs=""1"" maxOccurs=""1"" name=""PublicNote"" type=""xs:string"" />
                                            <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Header"" type=""xs:string"" />
                                            <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Details"" type=""xs:string"" />
                                            <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Summary"" type=""xs:string"" />
                                            <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ShortText"" type=""xs:string"">
                                              <xs:annotation>
                                                <xs:documentation>
                                                  SMS Text<xs:restriction><xs:maxLength value=""190"" /></xs:restriction></xs:documentation>
                                              </xs:annotation>
                                            </xs:element>
                                            <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Importance"" type=""xs:int"" />
                                            <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Influence"" type=""xs:int"" />
                                            <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Urgency"" type=""xs:int"" />
                                            <xs:element minOccurs=""1"" maxOccurs=""1"" name=""WebLinks"">
                                              <xs:complexType>
                                                <xs:sequence>
                                                  <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""WebLink"">
                                                    <xs:complexType>
                                                      <xs:sequence>
                                                        <xs:element minOccurs=""1"" maxOccurs=""1"" name=""URL"" type=""xs:string"" />
                                                      </xs:sequence>
                                                    </xs:complexType>
                                                  </xs:element>
                                                </xs:sequence>
                                              </xs:complexType>
                                            </xs:element>
                                          </xs:sequence>
                                        </xs:complexType>
                                      </xs:element>
                                    </xs:sequence>
                                  </xs:complexType>
                                </xs:element>
                                <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Accessibility"" type=""xs:int"">
                                  <xs:annotation>
                                    <xs:documentation>Accessibility, 1=R, 2=S, 4=H</xs:documentation>
                                  </xs:annotation>
                                </xs:element>
                              </xs:sequence>
                            </xs:complexType>
                          </xs:element>
                        </xs:sequence>
                      </xs:complexType>
                    </xs:element>
                    <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Distance"" type=""xs:int"">
                      <xs:annotation>
                        <xs:documentation>
                          Distance i meters.
                        </xs:documentation>
                      </xs:annotation>
                    </xs:element>
                    <xs:element minOccurs=""1"" maxOccurs=""1"" name=""CO2value"" type=""xs:float"">
                      <xs:annotation>
                        <xs:documentation>
                          CO2 value in kg/person/km
                        </xs:documentation>
                      </xs:annotation>
                    </xs:element>
                    <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Recalculated"" type=""xs:boolean"">
                      <xs:annotation>
                        <xs:documentation>
                          Denotes if journey is not acc. to planned timetable. Journey has been recalculated due to real-time affect
                        </xs:documentation>
                      </xs:annotation>
                    </xs:element>
                    <xs:element minOccurs=""0"" maxOccurs=""1"" name=""OriginalTime"" type=""xs:string"">
                      <xs:annotation>
                        <xs:documentation>
                          If journey is recalculated, original dep time acc. to planned timetable.
                        </xs:documentation>
                      </xs:annotation>
                    </xs:element>
                    <xs:element minOccurs=""1"" maxOccurs=""1"" name=""SalesRestriction"" type=""xs:string"">
                      <xs:annotation>
                        <xs:documentation>
                          If any sales restrictions for journey, description of reason
                        </xs:documentation>
                      </xs:annotation>
                    </xs:element>
                    <xs:element minOccurs=""1"" maxOccurs=""1"" name=""PriceZoneNamesList"" type=""xs:string"">
                      <xs:annotation>
                        <xs:documentation>
                          Pipe separated list of passing price zones names
                        </xs:documentation>
                      </xs:annotation>
                    </xs:element>
                    <xs:element minOccurs=""1"" maxOccurs=""1"" name=""StartEndBigZoneList"" type=""xs:string"">
                      <xs:annotation>
                        <xs:documentation>
                          Pipe separated list of start and end big zones
                        </xs:documentation>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:simpleType name=""RoadInfoType"">
    <xs:restriction base=""xs:token"">
      <xs:enumeration value=""UNKNOWN"" />
      <xs:enumeration value=""WEATHER"" />
      <xs:enumeration value=""ROAD_ACCIDENT"" />
      <xs:enumeration value=""ROAD_WORK"" />
      <xs:enumeration value=""ROAD_CONDITION"" />
      <xs:enumeration value=""EVENT"" />
      <xs:enumeration value=""HOLIDAY"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""RealTimeAffect"">
    <xs:restriction base=""xs:token"">
      <xs:enumeration value=""CRITICAL"" />
      <xs:enumeration value=""NON_CRITICAL"" />
      <xs:enumeration value=""PASSED"" />
      <xs:enumeration value=""NONE"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""ScopeAttributeType"">
    <xs:restriction base=""xs:token"">
      <xs:enumeration value=""CONCERNS_DEPARTURE"" />
      <xs:enumeration value=""CONCERNS_ARRIVAL"" />
      <xs:enumeration value=""CONCERNS_LINE"" />
      <xs:enumeration value=""CONCERNS_DEPARR"" />
    </xs:restriction>
  </xs:simpleType>
</xs:schema>";
        
        public GetJourneyType() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [2];
                _RootElements[0] = "GetJourney";
                _RootElements[1] = "GetJourneyResult";
                return _RootElements;
            }
        }
        
        protected override object RawSchema {
            get {
                return _rawSchema;
            }
            set {
                _rawSchema = value;
            }
        }
        
        [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK007/GetJourney/20141216",@"GetJourney")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"GetJourney"})]
        public sealed class GetJourney : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public GetJourney() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "GetJourney";
                    return _RootElements;
                }
            }
            
            protected override object RawSchema {
                get {
                    return _rawSchema;
                }
                set {
                    _rawSchema = value;
                }
            }
        }
        
        [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK007/GetJourney/20141216",@"GetJourneyResult")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"GetJourneyResult"})]
        public sealed class GetJourneyResult : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public GetJourneyResult() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "GetJourneyResult";
                    return _RootElements;
                }
            }
            
            protected override object RawSchema {
                get {
                    return _rawSchema;
                }
                set {
                    _rawSchema = value;
                }
            }
        }
    }
}
