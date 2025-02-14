﻿if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {

    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.TravelInformation) == "undefined") {

    Endeavor.Skanetrafiken.TravelInformation = {

        contractors: null,
        organisations: null,
        cities: null,
        stopareas: null,
        lines: null,

        currentLine: null,
        currentCity: "",

        formContext: null,
        document: null,
        saveInProgress: false,
        response: null,

        onLoad: function (executionContext) {
            try {
                Endeavor.Skanetrafiken.TravelInformation.formContext = executionContext.getFormContext();
                Endeavor.Skanetrafiken.TravelInformation.getContractors();
                Endeavor.Skanetrafiken.TravelInformation.getOrganisationalUnits();

                debugger;
                if (Endeavor.Skanetrafiken.TravelInformation.document != null) {
                    var timestamp_label = Endeavor.Skanetrafiken.TravelInformation.document.getElementById('timestamp');
                    Endeavor.Skanetrafiken.TravelInformation.getCaseEventDate(timestamp_label);
                }
                
            } catch (e) {
                Endeavor.formscriptfunctions.AlertCustomDialog(e.message);
            }
        },

        setDocument: function (document) {
            Endeavor.Skanetrafiken.TravelInformation.document = document;
        },

        getDocXml: function (response, innerResponseTag, parentTag) {

            var parser = new DOMParser();
            var innerReponse = JSON.parse(response.responseText)[innerResponseTag];

            if (parentTag != null)
                innerReponse = "<" + parentTag + ">" + innerReponse + "</" + parentTag + ">";

            return parser.parseFromString(innerReponse, "text/xml");
        },

        search: function () {

            var document = Endeavor.Skanetrafiken.TravelInformation.document;

            var transportlist = document.getElementById("transportlist");
            var transport = transportlist.value;

            var citylist = document.getElementById("citylist");
            var city = citylist.value;

            var linelist = document.getElementById("linelist");
            var line = linelist.value;

            var timestamp = document.getElementById('timestamp');

            var time = timestamp.value;
            if (time && time.length == 16) {
                time = time.substring(0, 16).replace(" ", "T") + "Z";
            }

            var date = new Date(time);
            var offSetHours = date.getTimezoneOffset() / 60;
            date.setHours(date.getHours() + offSetHours);

            if (!isNaN(date.getTime()))
                time = date.toISOString();
            else {
                Endeavor.formscriptfunctions.AlertCustomDialog("Timestamp är inte giltigt");
                return;
            }

            var fromlist = document.getElementById("fromlist");
            var fromarea = fromlist.value;

            var tolist = document.getElementById("tolist");
            var toarea = tolist.value;

            if (fromarea && toarea) {
                // var response = '<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/"><s:Body><ns0:GetDirectJourneysBetweenStopsResponse xmlns:ns0="http://www.skanetrafiken.com/DK/INTSTDK001/GetDirectJourneysBetweenStopsResponse/20141023"><DirectJourneysBetweenStops><DatedVehicleJourneyId>1120502564352389</DatedVehicleJourneyId><ServiceJourneyGid>9015012060800050</ServiceJourneyGid><OperatingDayDate>2016-02-09T00:00:00+01:00</OperatingDayDate><ContractorGid>9013012002200000</ContractorGid><LineDesignation>8</LineDesignation><JourneyNumber>50</JourneyNumber><DirectionOfLineDescription>Lundsbäck</DirectionOfLineDescription><PrimaryDestinationName>Lundsbäck</PrimaryDestinationName><SecondaryDestinationName>Knutpunkten</SecondaryDestinationName><DepartureId>1120000154959048</DepartureId><DepartureStopPointGid>9025012008306402</DepartureStopPointGid><DepartureType>2</DepartureType><DepartureSequenceNumber>3</DepartureSequenceNumber><PlannedDepartureDateTime>2016-02-09T10:35:15+01:00</PlannedDepartureDateTime><ArrivalId>1120000154959049</ArrivalId><ArrivalStopPointGid>9025012008328101</ArrivalStopPointGid><ArrivalType>2</ArrivalType><ArrivalSequenceNumber>4</ArrivalSequenceNumber><PlannedArrivalDateTime>2016-02-09T10:35:58+01:00</PlannedArrivalDateTime><ExpectedToBeMonitored>0</ExpectedToBeMonitored><TargetDepartureStopPointGid>9025012008306402</TargetDepartureStopPointGid><TargetDepartureDateTime>2016-02-09T10:35:15+01:00</TargetDepartureDateTime><ArrivalStopPointGid1>9025012008328101</ArrivalStopPointGid1></DirectJourneysBetweenStops><DirectJourneysBetweenStops><DatedVehicleJourneyId>1120502564352474</DatedVehicleJourneyId><ServiceJourneyGid>9015012060800054</ServiceJourneyGid><OperatingDayDate>2016-02-09T00:00:00+01:00</OperatingDayDate><ContractorGid>9013012002200000</ContractorGid><LineDesignation>8</LineDesignation><JourneyNumber>54</JourneyNumber><DirectionOfLineDescription>Lundsbäck</DirectionOfLineDescription><PrimaryDestinationName>Lundsbäck</PrimaryDestinationName><SecondaryDestinationName>Knutpunkten</SecondaryDestinationName><DepartureId>1120000154959098</DepartureId><DepartureStopPointGid>9025012008306402</DepartureStopPointGid><DepartureType>2</DepartureType><DepartureSequenceNumber>3</DepartureSequenceNumber><PlannedDepartureDateTime>2016-02-09T11:05:15+01:00</PlannedDepartureDateTime><ArrivalId>1120000154959099</ArrivalId><ArrivalStopPointGid>9025012008328101</ArrivalStopPointGid><ArrivalType>2</ArrivalType><ArrivalSequenceNumber>4</ArrivalSequenceNumber><PlannedArrivalDateTime>2016-02-09T11:05:58+01:00</PlannedArrivalDateTime><ExpectedToBeMonitored>0</ExpectedToBeMonitored><TargetDepartureStopPointGid>9025012008306402</TargetDepartureStopPointGid><TargetDepartureDateTime>2016-02-09T11:05:15+01:00</TargetDepartureDateTime><ArrivalStopPointGid1>9025012008328101</ArrivalStopPointGid1></DirectJourneysBetweenStops><DirectJourneysBetweenStops><DatedVehicleJourneyId>1120502564352559</DatedVehicleJourneyId><ServiceJourneyGid>9015012060800058</ServiceJourneyGid><OperatingDayDate>2016-02-09T00:00:00+01:00</OperatingDayDate><ContractorGid>9013012002200000</ContractorGid><LineDesignation>8</LineDesignation><JourneyNumber>58</JourneyNumber><DirectionOfLineDescription>Lundsbäck</DirectionOfLineDescription><PrimaryDestinationName>Lundsbäck</PrimaryDestinationName><SecondaryDestinationName>Knutpunkten</SecondaryDestinationName><DepartureId>1120000154959148</DepartureId><DepartureStopPointGid>9025012008306402</DepartureStopPointGid><DepartureType>2</DepartureType><DepartureSequenceNumber>3</DepartureSequenceNumber><PlannedDepartureDateTime>2016-02-09T11:35:15+01:00</PlannedDepartureDateTime><ArrivalId>1120000154959149</ArrivalId><ArrivalStopPointGid>9025012008328101</ArrivalStopPointGid><ArrivalType>2</ArrivalType><ArrivalSequenceNumber>4</ArrivalSequenceNumber><PlannedArrivalDateTime>2016-02-09T11:35:58+01:00</PlannedArrivalDateTime><ExpectedToBeMonitored>0</ExpectedToBeMonitored><TargetDepartureStopPointGid>9025012008306402</TargetDepartureStopPointGid><TargetDepartureDateTime>2016-02-09T11:35:15+01:00</TargetDepartureDateTime><ArrivalStopPointGid1>9025012008328101</ArrivalStopPointGid1></DirectJourneysBetweenStops><DirectJourneysBetweenStops><DatedVehicleJourneyId>1120502564352644</DatedVehicleJourneyId><ServiceJourneyGid>9015012060800062</ServiceJourneyGid><OperatingDayDate>2016-02-09T00:00:00+01:00</OperatingDayDate><ContractorGid>9013012002200000</ContractorGid><LineDesignation>8</LineDesignation><JourneyNumber>62</JourneyNumber><DirectionOfLineDescription>Lundsbäck</DirectionOfLineDescription><PrimaryDestinationName>Lundsbäck</PrimaryDestinationName><SecondaryDestinationName>Knutpunkten</SecondaryDestinationName><DepartureId>1120000154959198</DepartureId><DepartureStopPointGid>9025012008306402</DepartureStopPointGid><DepartureType>2</DepartureType><DepartureSequenceNumber>3</DepartureSequenceNumber><PlannedDepartureDateTime>2016-02-09T12:05:15+01:00</PlannedDepartureDateTime><ArrivalId>1120000154959199</ArrivalId><ArrivalStopPointGid>9025012008328101</ArrivalStopPointGid><ArrivalType>2</ArrivalType><ArrivalSequenceNumber>4</ArrivalSequenceNumber><PlannedArrivalDateTime>2016-02-09T12:05:58+01:00</PlannedArrivalDateTime><ExpectedToBeMonitored>0</ExpectedToBeMonitored><TargetDepartureStopPointGid>9025012008306402</TargetDepartureStopPointGid><TargetDepartureDateTime>2016-02-09T12:05:15+01:00</TargetDepartureDateTime><ArrivalStopPointGid1>9025012008328101</ArrivalStopPointGid1></DirectJourneysBetweenStops></ns0:GetDirectJourneysBetweenStopsResponse></s:Body></s:Envelope>';

                // (transportType, fromStopAreaGid, toStopAreaGid, tripDateTime, forLineGids);
                var inputParameters = [{ "Field": "TransportType", "Value": transport, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 },
                                    { "Field": "FromStopAreaGid", "Value": fromarea, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 },
                                    { "Field": "ToStopAreaGid", "Value": toarea, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 },
                                    { "Field": "TripDateTime", "Value": time, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 },
                                    { "Field": "ForLineGids", "Value": line, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 }];

                Endeavor.formscriptfunctions.callGlobalAction("ed_GetDirectJourneys", inputParameters,
                    function (response) {
                        var responsedoc = Endeavor.Skanetrafiken.TravelInformation.getDocXml(response, "DirectJourneysResponse", null);
                        Endeavor.Skanetrafiken.TravelInformation.populateTravelInformation(transport, city, responsedoc);
                    },
                    function (error) {
                        var errorMessage = "Get Direct Journeys service is unavailable. Please contact your systems administrator. Details: " + error.message;
                        console.log(errorMessage);
                        Endeavor.formscriptfunctions.AlertCustomDialog(errorMessage);
                    });
            }
            else if ((transport == "REGIONBUS" || transport == "STADSBUSS") && line) {
                // GetContractors called onLoad, i.e. no request made in method populateContractorInformation
                Endeavor.Skanetrafiken.TravelInformation.populateContractorInformation(transport, city, line);
            }
        },

        getDeviationMessage: function (lDeviationMessage, lDepartureDeviation, lArrivalDeviation, departureId, arrivalId) {

            // LOCAL FUNCTION
            getElementValue = function (element, value) {
                if (element != null) {
                    var elements = element.getElementsByTagName(value);
                    if (elements && elements[0] && elements[0].firstChild.nodeValue)
                        return elements[0].firstChild.nodeValue;
                    else
                        return "X";
                }
                else
                    return "X";
            }

            if (departureId == "X" && arrivalId == "X")
                return "";

            var finalDeviation = [];

            if (departureId != "X") {

                var lDeparture = [];

                for (var i = 0; i < lDepartureDeviation.length; i++) {
                    var isOnDepartureId = getElementValue(lDepartureDeviation[i], "IsOnDepartureId");

                    if (isOnDepartureId != departureId)
                        continue;
                    else
                        lDeparture.push(getElementValue(lDepartureDeviation[i], "HasDeviationMessageVersionId"));
                }

                for (var j = 0; j < lDeviationMessage.length; j++) {
                    var isPartOfDeviationMessageId = getElementValue(lDeviationMessage[j], "IsPartOfDeviationMessageId");
                    var usageTypeLongCode = getElementValue(lDeviationMessage[j], "UsageTypeLongCode");

                    if (usageTypeLongCode != "DETAILS")
                        continue;

                    if (lDeparture.indexOf(isPartOfDeviationMessageId) >= 0) {
                        var content = getElementValue(lDeviationMessage[j], "Content");
                        if (finalDeviation.indexOf(content) === -1)
                            finalDeviation.push(content);
                    }
                }
            }

            if (arrivalId != "X") {

                var lArrival = [];

                for (var i = 0; i < lArrivalDeviation.length; i++) {
                    var isOnArrivalId = getElementValue(lArrivalDeviation[i], "IsOnArrivalId");

                    if (isOnArrivalId != arrivalId)
                        continue;
                    else
                        lArrival.push(getElementValue(lArrivalDeviation[i], "HasDeviationMessageVersionId"));
                }

                for (var j = 0; j < lDeviationMessage.length; j++) {
                    var isPartOfDeviationMessageId = getElementValue(lDeviationMessage[j], "IsPartOfDeviationMessageId");
                    var usageTypeLongCode = getElementValue(lDeviationMessage[j], "UsageTypeLongCode");

                    if (usageTypeLongCode != "DETAILS")
                        continue;

                    if (lArrival.indexOf(isPartOfDeviationMessageId) >= 0) {
                        var content = getElementValue(lDeviationMessage[j], "Content");
                        if (finalDeviation.indexOf(content) === -1)
                            finalDeviation.push(content);
                    }
                }
            }

            var finalStringDeviation = "";
            if (finalDeviation.length > 0) {
                for (var k = 0; k < finalDeviation.length; k++)
                    finalStringDeviation += finalDeviation[k] + "\n";
            }

            return finalStringDeviation;
        },

        getContractorFromGid: function (gid, caseDate) {

            var contractorsdoc = Endeavor.Skanetrafiken.TravelInformation.contractors;
            var xContractor = "X";
            if (contractorsdoc && contractorsdoc.firstChild && contractorsdoc.firstChild.childNodes && contractorsdoc.firstChild.childNodes.length > 0) {
                /*
                for (var i = 0; i < contractorsdoc.firstChild.childNodes.length; i++) {

                    var contractor = contractorsdoc.firstChild.childNodes[i];

                    if (gid == contractor.getAttribute("Gid"))
                        return Endeavor.Skanetrafiken.TravelInformation.getOrganisationFromId(contractor.getAttribute("IsOrganisationId"));
                } */

                var contractors = Array.from(contractorsdoc.firstChild.childNodes).filter(x => x.getAttribute && x.getAttribute("Gid") == gid);
                if (contractors.length != 0) { //if multipe contractors found
                    if (contractors.length > 1)
                    {
                        if (caseDate) { //check the exists from date and exists up to date with current Case date
                            caseDate = new Date(caseDate.split(' ')[0]);
                            var contractorFilter = contractors.filter(c => (c.getAttribute("ExistsUptoDate") == null
                                                                                || new Date(c.getAttribute("ExistsUptoDate")) >= caseDate)
                                                                                && (c.getAttribute("ExistsFromDate") == null
                                                                                || new Date(c.getAttribute("ExistsFromDate")) <= caseDate));
                            if (contractorFilter.length > 0)
                                xContractor = Endeavor.Skanetrafiken.TravelInformation.getOrganisationFromId(contractorFilter[0].getAttribute("IsOrganisationId"));
                        }
                        else xContractor = Endeavor.Skanetrafiken.TravelInformation.getOrganisationFromId(contractors[0].getAttribute("IsOrganisationId"));
                    }
                    else xContractor = Endeavor.Skanetrafiken.TravelInformation.getOrganisationFromId(contractors[0].getAttribute("IsOrganisationId"));
                }

            }

            return xContractor;
        },

        getContractorFromLine: function (cityGid, lineGid) {

            var organisationsdoc = Endeavor.Skanetrafiken.TravelInformation.organisations;
            var line = Endeavor.Skanetrafiken.TravelInformation.getLine(cityGid, lineGid);

            if (line && line.getElementsByTagName("ContractorName") && line.getElementsByTagName("ContractorName")[0])
                return line.getElementsByTagName("ContractorName")[0].firstChild.nodeValue;
            else {
                var lineOpCode = line.getElementsByTagName("ContractorCode")[0].firstChild.nodeValue;

                if (organisationsdoc && organisationsdoc.firstChild && organisationsdoc.firstChild.childNodes && organisationsdoc.firstChild.childNodes.length > 0) {

                    for (var i = 0; i < organisationsdoc.firstChild.childNodes.length; i++) {

                        var organisation = organisationsdoc.firstChild.childNodes[i];

                        if (lineOpCode == organisation.getAttribute("Code"))
                            return organisation != "" ? organisation.getAttribute("Name") : "";
                    }
                }
            }

            return "";
        },

        getOrganisationFromLine: function (cityGid, lineGid) {

            var responsedoc = Endeavor.Skanetrafiken.TravelInformation.response;
            var organisationsdoc = Endeavor.Skanetrafiken.TravelInformation.organisations;

            var line = Endeavor.Skanetrafiken.TravelInformation.getLine(cityGid, lineGid);

            if (line && line.getElementsByTagName("LineOperatorCode") && line.getElementsByTagName("LineOperatorCode")[0]) {

                var lineOpCode = line.getElementsByTagName("LineOperatorCode")[0].firstChild.nodeValue;

                if (organisationsdoc && organisationsdoc.firstChild && organisationsdoc.firstChild.childNodes && organisationsdoc.firstChild.childNodes.length > 0) {

                    for (var i = 0; i < organisationsdoc.firstChild.childNodes.length; i++) {

                        var organisation = organisationsdoc.firstChild.childNodes[i];

                        if (lineOpCode == organisation.getAttribute("Code"))
                            return organisation;
                    }
                }
            }

            return "";
        },

        getOrganisationFromId: function (id) {

            var organisationsdoc = Endeavor.Skanetrafiken.TravelInformation.organisations;

            if (organisationsdoc && organisationsdoc.firstChild && organisationsdoc.firstChild.childNodes && organisationsdoc.firstChild.childNodes.length > 0) {

                for (var i = 0; i < organisationsdoc.firstChild.childNodes.length; i++) {

                    var organisation = organisationsdoc.firstChild.childNodes[i];

                    if (id == organisation.getAttribute("Id"))
                        return organisation.getAttribute("Name");
                }
            }

            return "";
        },

        setTransportType: function () {

            var document = Endeavor.Skanetrafiken.TravelInformation.document;

            var transportlist = document.getElementById("transportlist");
            var citylist = document.getElementById("citylist");
            citylist.innerHTML = '<option value="" selected disabled hidden >Välj</option>';
            var linelist = document.getElementById("linelist");
            linelist.innerHTML = '<option value="" selected disabled hidden >Välj</option>';
            var fromlist = document.getElementById("fromlist");
            fromlist.innerHTML = '<option value="" selected disabled hidden >Välj</option>';
            var tolist = document.getElementById("tolist");
            tolist.innerHTML = '<option value="" selected disabled hidden >Välj</option>';

            var value = transportlist.value;

            var inputParameters = [{ "Field": "LineType", "Value": value, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 }];

            Endeavor.formscriptfunctions.callGlobalAction("ed_GetLineDetails", inputParameters,
                function (response) {

                    var regionBusXmlDoc = Endeavor.Skanetrafiken.TravelInformation.getDocXml(response, "GetLineDetailsResponse", null);
                    Endeavor.Skanetrafiken.TravelInformation.response = regionBusXmlDoc;

                    if (value == "TRAIN") {
                        citylist.style.visibility = "hidden";
                        linelist.style.visibility = "hidden";

                        Endeavor.Skanetrafiken.TravelInformation.dropDownTrain();
                    }
                    else if (value == "REGIONBUS") {
                        citylist.style.visibility = "hidden";
                        linelist.style.visibility = "visible";

                        Endeavor.Skanetrafiken.TravelInformation.dropDownRegionbus();
                    }
                    else if (value == "STADSBUSS") {
                        citylist.style.visibility = "visible";
                        linelist.style.visibility = "visible";

                        Endeavor.Skanetrafiken.TravelInformation.dropDownStradbus();
                    }
                },
                function (error) {
                    var errorMessage = "TravelInformationDB is unavailable. Please contact your systems administrator. Details: " + error.message;
                    console.log(errorMessage);
                    Endeavor.formscriptfunctions.AlertCustomDialog(errorMessage);
                });
        },

        filterRelevantValues: function (childNodes, nameTag, idTag, existsFromTag, existsUpToTag, timestamp) {
            // LOCAL FUNCTION
            getElementValue = function (element, value) {
                if (element != null) {
                    var elements = element.getElementsByTagName(value);
                    if (elements && elements[0] && elements[0].firstChild.nodeValue) {
                        return elements[0].firstChild.nodeValue;
                    }
                    else {
                        return "X";
                    }
                }
                else {
                    return "X";
                }
            }

            var options = [];
            for (var i = 0; i < childNodes.length; i++) {

                var child = childNodes[i];
                var isRelevant = false;

                if (existsFromTag == null && existsUpToTag == null)
                    isRelevant = true;

                var existsFromDate = child.getElementsByTagName(existsFromTag);
                var existsUpToDate = child.getElementsByTagName(existsUpToTag);

                if (existsFromDate.length > 0 && existsUpToDate.length > 0) {
                    if (timestamp >= new Date(existsFromDate[0].firstChild.nodeValue) && timestamp <= new Date(existsUpToDate[0].firstChild.nodeValue))
                        isRelevant = true;
                }
                else if (existsFromDate.length > 0 && existsUpToDate.length == 0) {
                    if (timestamp >= new Date(existsFromDate[0].firstChild.nodeValue))
                        isRelevant = true;
                }
                else if (existsFromDate.length == 0 && existsUpToDate.length > 0) {
                    if (timestamp <= new Date(existsUpToDate[0].firstChild.nodeValue))
                        isRelevant = true;
                }

                if (isRelevant) {

                    var name = getElementValue(child, nameTag);
                    var id = getElementValue(child, idTag);

                    var option = { name: name, value: id };
                    var checkExist = options.filter(x => x.id == option.id && x.value == option.value);
                    if (checkExist.length == 0)
                        options.push(option);
                }
            }

            return options;
        },

        dropDownTrain: function () {
            debugger;
            var document = Endeavor.Skanetrafiken.TravelInformation.document;
            var timestamp = new Date(document.getElementById("timestamp").value);

            var trainXmlDoc = Endeavor.Skanetrafiken.TravelInformation.response;
            var error = trainXmlDoc.getElementsByTagName('ErrorMessage');

            if (error.length > 0)
                Endeavor.formscriptfunctions.AlertCustomDialog(error[0]);
            else {

                var fromlist = document.getElementById('fromlist');
                var stopareas = trainXmlDoc.getElementsByTagName("StopAreas")[0].childNodes;

                var options = Endeavor.Skanetrafiken.TravelInformation.filterRelevantValues(stopareas, "StopAreaName", "StopAreaGid", "ExistsFromDate", "ExistsUptoDate", timestamp);

                Endeavor.Skanetrafiken.TravelInformation.stopareas = options;
                Endeavor.Skanetrafiken.TravelInformation.lines = trainXmlDoc.getElementsByTagName("Lines")[0].childNodes;
                Endeavor.Skanetrafiken.TravelInformation.populateSelectionList(fromlist, options, true);
            }
        },

        dropDownRegionbus: function () {
            debugger;
            var document = Endeavor.Skanetrafiken.TravelInformation.document;
            var timestamp = new Date(document.getElementById("timestamp").value);

            var regionBusXmlDoc = Endeavor.Skanetrafiken.TravelInformation.response;
            var error = regionBusXmlDoc.getElementsByTagName('ErrorMessage');

            if (error.length > 0)
                Endeavor.formscriptfunctions.AlertCustomDialog(error[0]);
            else {

                var linelist = document.getElementById('linelist');
                var lines = regionBusXmlDoc.getElementsByTagName("Lines")[0].childNodes;

                var options = Endeavor.Skanetrafiken.TravelInformation.filterRelevantValues(lines, "LineNumber", "LineGid", null, null, timestamp);

                Endeavor.Skanetrafiken.TravelInformation.populateSelectionList(linelist, options, false);
            }
        },

        dropDownStradbus: function () {
            debugger;
            var document = Endeavor.Skanetrafiken.TravelInformation.document;
            var timestamp = new Date(document.getElementById("timestamp").value);

            var stradBusXmlDoc = Endeavor.Skanetrafiken.TravelInformation.response;
            var error = stradBusXmlDoc.getElementsByTagName('ErrorMessage');

            if (error.length > 0)
                Endeavor.formscriptfunctions.AlertCustomDialog(error[0]);
            else {

                var citylist = document.getElementById('citylist');
                var cities = stradBusXmlDoc.getElementsByTagName("Zones")[0].childNodes;

                var options = Endeavor.Skanetrafiken.TravelInformation.filterRelevantValues(cities, "ZoneName", "ZoneId", "ZoneExistsFromDate", "ZoneExistsUptoDate", timestamp);

                Endeavor.Skanetrafiken.TravelInformation.cities = options;
                Endeavor.Skanetrafiken.TravelInformation.populateSelectionList(citylist, options, true);
            }
        },

        setCity: function () {
            debugger;
            var document = Endeavor.Skanetrafiken.TravelInformation.document;
            var timestamp = new Date(document.getElementById("timestamp").value);

            var fromlist = document.getElementById("fromlist");
            var tolist = document.getElementById("tolist");
            var linelist = document.getElementById("linelist");
            var citylist = document.getElementById("citylist");

            var cityid = citylist.value; // WHAT IF CITY IS NOT SELECTED? POSSIBLE? NO?
            var city = Endeavor.Skanetrafiken.TravelInformation.getCity(cityid);
            var lines = city.getElementsByTagName("Lines")[0].childNodes;

            var options = Endeavor.Skanetrafiken.TravelInformation.filterRelevantValues(lines, "LinePublicName", "LineGid", "ExistsFromDate", "ExistsUptoDate", timestamp);

            if (options == null || options.length < 1)
                Endeavor.formscriptfunctions.AlertCustomDialog("Staden saknar aktiva linjer för angivet datum.");

            fromlist.innerHTML = '<option value="" selected disabled hidden >Välj</option>';
            tolist.innerHTML = '<option value="" selected disabled hidden >Välj</option>';
            Endeavor.Skanetrafiken.TravelInformation.populateSelectionList(linelist, options, false);
        },

        getCityFromGid: function (cityGid) {

            var cities = Endeavor.Skanetrafiken.TravelInformation.cities;

            if (cities) {

                for (var i = 0; i < cities.length; i++) {
                    if (cityGid == cities[i].value)
                        return cities[i].name;
                }
            }

            return "";
        },

        getStopAreaFromGid: function (areaGid) {

            var stopareas = Endeavor.Skanetrafiken.TravelInformation.stopareas;

            if (stopareas) {

                for (var i = 0; i < stopareas.length; i++) {
                    if (areaGid == stopareas[i].value)
                        return stopareas[i].name;
                }
            }

            return "";
        },

        setLine: function () {
            debugger;
            var document = Endeavor.Skanetrafiken.TravelInformation.document;
            var timestamp = new Date(document.getElementById("timestamp").value);

            var fromlist = document.getElementById("fromlist");
            var tolist = document.getElementById("tolist");

            var cityid = document.getElementById("citylist").value;
            var lineid = document.getElementById("linelist").value;

            var line = Endeavor.Skanetrafiken.TravelInformation.getLine(cityid, lineid);
            Endeavor.Skanetrafiken.TravelInformation.currentLine = line;
            Endeavor.Skanetrafiken.TravelInformation.currentCity = document.getElementById("citylist").value;

            if (line.getElementsByTagName("StopAreas").length == 0)
                Endeavor.formscriptfunctions.AlertCustomDialog("Linjen saknar hållplatser.");

            var stopareas = line.getElementsByTagName("StopAreas")[0].childNodes;
            var options = Endeavor.Skanetrafiken.TravelInformation.filterRelevantValues(stopareas, "StopAreaName", "StopAreaGid", "StopExistsFromDate", "StopExistsUptoDate", timestamp);

            Endeavor.Skanetrafiken.TravelInformation.stopareas = options;
            Endeavor.Skanetrafiken.TravelInformation.populateSelectionList(fromlist, options, true);
            Endeavor.Skanetrafiken.TravelInformation.populateSelectionList(tolist, options, true);
        },

        setFrom: function () {
            debugger;
            var document = Endeavor.Skanetrafiken.TravelInformation.document;
            var timestamp = new Date(document.getElementById("timestamp").value);

            var transporttype = document.getElementById("transportlist").value;

            if (transporttype == "TRAIN") {

                var fromid = document.getElementById("fromlist").value;
                var tolist = document.getElementById("tolist");

                var fromstoparea = Endeavor.Skanetrafiken.TravelInformation.getStopArea(fromid);
                var uptostopareas = fromstoparea.getElementsByTagName("UptoStopAreas")[0].childNodes;

                var options = Endeavor.Skanetrafiken.TravelInformation.filterRelevantValues(uptostopareas, "StopAreaName", "StopAreaGid", "ExistsFromDate", "ExistsUpToDate", timestamp);

                Endeavor.Skanetrafiken.TravelInformation.populateSelectionList(tolist, options, true);
            }
        },

        populateSelectionList: function (list, options, sorting) {

            var document = Endeavor.Skanetrafiken.TravelInformation.document;

            list.innerHTML = '<option value="" selected disabled hidden >Välj</option>';

            if (sorting) {  // IF TEXT
                // SORTING FUNCTION
                Array.prototype.sortOn = function (key) {
                    this.sort(function (a, b) {
                        if (a[key] < b[key]) {
                            return -1;
                        } else if (a[key] > b[key]) {
                            return 1;
                        }
                        return 0;
                    });
                }

                options.sortOn('name');
            }
            else { // ELSE NUMERICAL
                // SORTING FUNCTION
                Array.prototype.sortOn = function (key) {
                    this.sort(function (a, b) {
                        return a[key] - b[key];
                    });
                }

                options.sortOn('name');
            }

            for (var i = 0; i < options.length; i++) {

                var option = document.createElement("option");
                option.value = options[i].value;
                option.text = options[i].name;

                list.appendChild(option);
            }
        },

        getCity: function (cityid) {

            var returncity = null;
            var cityXmlDoc = Endeavor.Skanetrafiken.TravelInformation.response;

            var cities = cityXmlDoc.getElementsByTagName("Zones")[0].childNodes;
            for (var i = 0; i < cities.length; i++) {

                var city = cities[i];
                var id = city.getElementsByTagName("ZoneId")[0].firstChild.nodeValue;

                if (id == cityid)
                    returncity = city;
            }

            return returncity;
        },

        getLine: function (cityid, lineid) {

            var returnline = null;
            var lineXmlDoc = Endeavor.Skanetrafiken.TravelInformation.response;

            var city;

            if (cityid)   // OM CITYID EJ ANGIVET SÅ ANVÄNDS REGION ISTÄLLET...
                city = Endeavor.Skanetrafiken.TravelInformation.getCity(cityid);
            else
                city = lineXmlDoc;

            var lines = city.getElementsByTagName("Lines")[0].childNodes;
            for (var i = 0; i < lines.length; i++) {

                var line = lines[i];
                var id = line.getElementsByTagName("LineGid")[0].firstChild.nodeValue;

                if (id == lineid) {
                    returnline = line;
                    break;
                }
            }

            return returnline;
        },
        
        getStopArea: function (stopareaid) {

            var returnarea = null;
            var stopAreaXmlDoc = Endeavor.Skanetrafiken.TravelInformation.response;

            var stopareas = stopAreaXmlDoc.getElementsByTagName("StopAreas")[0].childNodes;
            for (var i = 0; i < stopareas.length; i++) {

                var stoparea = stopareas[i];
                var id = stoparea.getElementsByTagName("StopAreaGid")[0].firstChild.nodeValue;

                if (id == stopareaid)
                    returnarea = stoparea;
            }

            return returnarea;
        },

        getLineTrainType: function (lines, serviceGid) {
            debugger;
            var line = null;
            var firstDigit = serviceGid.substring(7, 8);

            var _lineNumber = null;
            if (firstDigit == "0")
                _lineNumber = serviceGid.substring(8, 11);
            else
                _lineNumber = serviceGid.substring(7, 11);

            for (var i = 0; i < lines.length; i++) {

                var lineNumber = lines[i].getElementsByTagName("Number")[0].firstChild.nodeValue;
                if (lineNumber == _lineNumber) {
                    var designation = lines[i].getElementsByTagName("Designation")[0].firstChild.nodeValue;
                    line = { lineNumber: lineNumber, lineDesignation: designation };
                    break;
                }
            }

            return line;
        },

        populateContractorInformation: function (transporttype, city, line) {

            var document = Endeavor.Skanetrafiken.TravelInformation.document;

            var travelinformationbody = document.getElementById("travelinformationbody");
            travelinformationbody.innerHTML = "";

            // LOCAL FUNCTION
            getElementValue = function (element, value) {
                if (element != null) {
                    var elements = element.getElementsByTagName(value);
                    if (elements && elements[0] && elements[0].firstChild.nodeValue) {
                        return elements[0].firstChild.nodeValue;
                    }
                    else {
                        return "-";
                    }
                }
                else {
                    return "-";
                }
            }

            var contractor = Endeavor.Skanetrafiken.TravelInformation.getContractorFromLine(city, line);
            if (contractor == null)
                contractor = "";

            var contractorrow = travelinformationbody.insertRow();
            var cell = contractorrow.insertCell();
            cell.style = "text-align: center";

            var saveEntity = {
                transporttype: transporttype,
                city: city,
                directjourney: null,
                line: line,
                contractorName: contractor
            };

            var savebutton = document.createElement("BUTTON");
            savebutton.style = "font-weight: bold; background-color: Transparent; width: 100%; height: 100%; cursor:pointer";
            savebutton.onclick = Endeavor.Skanetrafiken.TravelInformation.saveFunction(saveEntity);
            savebutton.innerHTML = '+';
            cell.appendChild(savebutton);

            cell = contractorrow.insertCell(); //Planerad Avgång
            cell = contractorrow.insertCell(); //Aktuell Avgång
            cell = contractorrow.insertCell(); //Planerad Ankomst
            cell = contractorrow.insertCell(); //Aktuell Ankomst
            cell = contractorrow.insertCell(); //Operatör
            cell.innerHTML = contractor;
        },

        populateTravelInformation: function (transporttype, city, response) {

            var document = Endeavor.Skanetrafiken.TravelInformation.document;
            var directjourneys = response.getElementsByTagName("DirectJourneysBetweenStops");
            var deviationMessageVariant = response.getElementsByTagName("DeviationMessageVariant");
            var departureDeviation = response.getElementsByTagName("DepartureDeviation");
            var arrivalDeviation = response.getElementsByTagName("ArrivalDeviation");

            var travelinformationbody = document.getElementById("travelinformationbody");
            travelinformationbody.innerHTML = "";

            // LOCAL FUNCTION
            getElementValue = function (element, value) {
                if (element != null) {
                    var elements = element.getElementsByTagName(value);
                    if (elements && elements[0] && elements[0].firstChild.nodeValue)
                        return elements[0].firstChild.nodeValue;
                    else
                        return "X";
                }
                else
                    return "X";
            }

            // LOCAL FUNCTION
            getFormattedDate = function (datestring) {

                datestring = datestring.replace("T", " ");

                if (datestring != null && datestring.length > 10)
                    return datestring.substring(0, 16);
                else
                    return "X";
            }

            // LOCAL FUNCTION
            getTime = function (datestring) {

                if (datestring != null && datestring.length > 10)
                    return datestring.substring(11, 16);
                else
                    return "X";
            }

            var caseDate = Endeavor.Skanetrafiken.TravelInformation.document.getElementById('timestamp').value;
            for (var i = 0; i < directjourneys.length; i++) {

                var directjourney = directjourneys[i];
                var journeyrow = travelinformationbody.insertRow();

                var cell = journeyrow.insertCell();
                cell.style = "text-align: center";

                var departureId = getElementValue(directjourney, "DepartureId");
                var arrivalId = getElementValue(directjourney, "ArrivalId");
                var deviationMessage = Endeavor.Skanetrafiken.TravelInformation.getDeviationMessage(deviationMessageVariant, departureDeviation, arrivalDeviation, departureId, arrivalId);

                var _line = null;
                debugger;
                if (transporttype == "TRAIN") {

                    var serviceGid = directjourney.getElementsByTagName("ServiceJourneyGid")[0].firstChild.nodeValue;
                    if (serviceGid != null)
                        _line = Endeavor.Skanetrafiken.TravelInformation.getLineTrainType(Endeavor.Skanetrafiken.TravelInformation.lines, serviceGid);
                }

                var saveEntity = {
                    transporttype: transporttype,
                    city: city,
                    directjourney: directjourney,
                    line: _line,
                    contractorName: null,
                    deviationMessage: deviationMessage,
                };

                //draw save button
                var savebutton = document.createElement("BUTTON");
                savebutton.style = "font-weight: bold; background-color: Transparent; width: 100%; height: 100%; cursor:pointer";
                savebutton.onclick = Endeavor.Skanetrafiken.TravelInformation.saveFunction(saveEntity); //(transporttype, city, directjourney);
                savebutton.innerHTML = '+';
                cell.appendChild(savebutton);

                cell = journeyrow.insertCell();
                cell.innerHTML = getFormattedDate(getElementValue(directjourney, "PlannedDepartureDateTime"));

                cell = journeyrow.insertCell();
                cell.innerHTML = getTime(getElementValue(directjourney, "ObservedDepartureDateTime"));

                cell = journeyrow.insertCell();
                cell.innerHTML = getFormattedDate(getElementValue(directjourney, "PlannedArrivalDateTime"));

                cell = journeyrow.insertCell();
                cell.innerHTML = getTime(getElementValue(directjourney, "ObservedArrivalDateTime"));

                cell = journeyrow.insertCell();
                var contractorGid = getElementValue(directjourney, "ContractorGid");
                cell.innerHTML = Endeavor.Skanetrafiken.TravelInformation.getContractorFromGid(contractorGid, caseDate);

                var journeyNumber = getElementValue(directjourney, "JourneyNumber") + " ";
                var directionOfLineDescription = getElementValue(directjourney, "DirectionOfLineDescription");

                cell = journeyrow.insertCell();
                cell.innerHTML = journeyNumber.concat(directionOfLineDescription);

                cell = journeyrow.insertCell();
                cell.style.whiteSpace = "pre-wrap";
                cell.innerHTML = deviationMessage;
            }
        },

        populateSavedTravelInformationTable: function () {

            var document = Endeavor.Skanetrafiken.TravelInformation.document;

            var entityName = Endeavor.Skanetrafiken.TravelInformation.formContext.data.entity.getEntityName();
            var formType = Endeavor.Skanetrafiken.TravelInformation.formContext.ui.getFormType();
            if (entityName.toUpperCase() == "INCIDENT" && formType && formType != 1) {

                var cgi_caseid = Endeavor.Skanetrafiken.TravelInformation.formContext.data.entity.getId();
                cgi_caseid = cgi_caseid.substring(1, cgi_caseid.length - 1);

                var travelinformationtable = document.getElementById("savedtravelsbody");
                travelinformationtable.innerHTML = "";

                var columnSet = "cgi_travelinformationid,cgi_displaytext,cgi_deviationmessage";

                Xrm.WebApi.retrieveMultipleRecords("cgi_travelinformation", "?$select=" + columnSet + "&$filter=_cgi_caseid_value eq " + cgi_caseid).then(
                    function success(savedtravelsresults) {

                        if (savedtravelsresults && savedtravelsresults.entities) {
                            for (var i = 0; i < savedtravelsresults.entities.length; i++) {

                                var row = travelinformationtable.insertRow();

                                var cell = row.insertCell();
                                var del = document.createElement("BUTTON");
                                del.style = "font-weight: bold; background-color: Transparent; width: 100%; height: 100%; cursor:pointer";
                                cell.onclick = Endeavor.Skanetrafiken.TravelInformation.deleteTravelInformation(savedtravelsresults.entities[i].cgi_travelinformationid);
                                del.innerHTML = "-";
                                cell.appendChild(del);
                                cell.style = "text-align: center";

                                cell = row.insertCell();
                                cell.innerHTML = savedtravelsresults.entities[i].cgi_displaytext;
                                cell = row.insertCell();
                                cell.innerHTML = savedtravelsresults.entities[i].cgi_deviationmessage;
                            }
                        }
                    },
                    function (error) {
                        console.log(error.message);
                        Endeavor.formscriptfunctions.AlertCustomDialog(error.message);
                    }
                );
            }
        },

        saveFunction: function (saveEntity) {
            return function () {

                // LOCAL FUNCTION
                getElementValue = function (element, value) {
                    if (element != null) {
                        var elements = element.getElementsByTagName(value);
                        if (elements && elements[0] && elements[0].firstChild.nodeValue) {
                            return elements[0].firstChild.nodeValue;
                        }
                        else {
                            return "X";
                        }
                    }
                    else {
                        return "X";
                    }
                }

                getElementValueReturnNull = function (element, value) {
                    if (element != null) {
                        var elements = element.getElementsByTagName(value);
                        if (elements && elements[0] && elements[0].firstChild.nodeValue) {
                            return elements[0].firstChild.nodeValue;
                        }
                        else {
                            return null;
                        }
                    }
                    else {
                        return null;
                    }
                }

                getISOStringDate = function (datetime) {
                    if (datetime == null || datetime == "X")
                        return null;

                    return new Date(datetime).toISOString();
                }

                if (!Endeavor.Skanetrafiken.TravelInformation.saveInProgress) {

                    var entity = null;
                    var document = Endeavor.Skanetrafiken.TravelInformation.document;
                    var line = Endeavor.Skanetrafiken.TravelInformation.currentLine;
                    var incidentId = Endeavor.Skanetrafiken.TravelInformation.formContext.data.entity.getId().substring(1, 37);
                    var caseDate = Endeavor.Skanetrafiken.TravelInformation.document.getElementById('timestamp').value;
                    if (saveEntity.transporttype == "TRAIN") {

                        var travelinformation = ((new Date()).toISOString().substring(0, 19)).replace("T", " ") + " => " + getElementValue(saveEntity.directjourney, "DirectionOfLineDescription");

                        debugger;
                        entity = {
                            "cgi_displaytext": "",
                            "cgi_transport": saveEntity.line == null ? getElementValue(saveEntity.directjourney, "LineDesignation") : saveEntity.line.lineDesignation,
                            "cgi_travelinformation": travelinformation,
                            "cgi_caseid@odata.bind": "/incidents(" + incidentId + ")",

                            "cgi_line": saveEntity.line == null ? "" : saveEntity.line.lineNumber,
                            "cgi_tour": getElementValue(saveEntity.directjourney, "JourneyNumber"),
                            "cgi_trainnumber": getElementValue(saveEntity.directjourney, "JourneyNumber"),
                            "cgi_journeynumber": getElementValue(saveEntity.directjourney, "JourneyNumber"),
                            "cgi_linedesignation": getElementValue(saveEntity.directjourney, "PrimaryDestinationName"),

                            "cgi_start": Endeavor.Skanetrafiken.TravelInformation.getStopAreaFromGid(document.getElementById("fromlist").value),
                            "cgi_stop": Endeavor.Skanetrafiken.TravelInformation.getStopAreaFromGid(document.getElementById("tolist").value),

                            "cgi_directiontext": getElementValue(saveEntity.directjourney, "DirectionOfLineDescription"),
                            "cgi_contractor": Endeavor.Skanetrafiken.TravelInformation.getContractorFromGid(getElementValue(saveEntity.directjourney, "ContractorGid"), caseDate), // NUMERICAL
                            "cgi_deviationmessage": saveEntity.deviationMessage,//getElementValue(saveEntity.directjourney, "Details"),

                            "cgi_startplanned": getISOStringDate(getElementValue(saveEntity.directjourney, "PlannedDepartureDateTime")),
                            "cgi_startactual": getElementValue(saveEntity.directjourney, "ObservedDepartureDateTime"), //ActualDepartureTime
                            "cgi_arivalplanned": getISOStringDate(getElementValue(saveEntity.directjourney, "PlannedArrivalDateTime")),
                            "cgi_arivalactual": getElementValue(saveEntity.directjourney, 'ObservedArrivalDateTime'), //ActualArrivalTime
                            "cgi_startactualdatetime": getElementValueReturnNull(saveEntity.directjourney, "ObservedDepartureDateTime"), //ActualDepartureTime
                            "cgi_arrivalactualdatetime": getElementValueReturnNull(saveEntity.directjourney, 'ObservedArrivalDateTime'), //ActualArrivalTime
                        }

                        if (entity != null)
                            Endeavor.Skanetrafiken.TravelInformation.setDisplayTextTrain(entity);
                    }
                    else if (saveEntity.transporttype == "REGIONBUS") {

                        var travelinformation = ((new Date()).toISOString().substring(0, 19)).replace("T", " ") + " => " + getElementValue(saveEntity.directjourney, "DirectionOfLineDescription");

                        if (saveEntity.directjourney == null) {

                            entity = {
                                "cgi_displaytext": "",
                                "cgi_transport": "Regionbuss",
                                "cgi_city": "",
                                "cgi_line": getElementValue(Endeavor.Skanetrafiken.TravelInformation.getLine(saveEntity.city, saveEntity.line), "LineNumber"),
                                "cgi_tour": "",
                                "cgi_start": "",
                                "cgi_stop": "",
                                "cgi_caseid@odata.bind": "/incidents(" + incidentId + ")",
                                "cgi_travelinformation": travelinformation,
                                "cgi_journeynumber": "",
                                "cgi_linedesignation": getElementValue(line, "LineDesignation"),
                                "cgi_directiontext": "",
                                "cgi_contractor": saveEntity.contractorName,
                                "cgi_deviationmessage": "",

                                "cgi_startactual": "",
                                "cgi_arivalactual": "",
                                "cgi_startplanned": null,
                                "cgi_arivalplanned": null,
                                "cgi_startactualdatetime": null,
                                "cgi_arrivalactualdatetime": null
                            }
                        } else {
                            entity = {
                                "cgi_displaytext": "",
                                "cgi_transport": "Regionbuss",
                                "cgi_travelinformation": travelinformation,
                                "cgi_caseid@odata.bind": "/incidents(" + incidentId + ")",

                                "cgi_line": getElementValue(line, "LineNumber"),
                                "cgi_tour": getElementValue(saveEntity.directjourney, "JourneyNumber"),
                                "cgi_linedesignation": getElementValue(saveEntity.directjourney, "PrimaryDestinationName"),

                                "cgi_start": Endeavor.Skanetrafiken.TravelInformation.getStopAreaFromGid(document.getElementById("fromlist").value),
                                "cgi_stop": Endeavor.Skanetrafiken.TravelInformation.getStopAreaFromGid(document.getElementById("tolist").value),

                                "cgi_directiontext": getElementValue(saveEntity.directjourney, "DirectionOfLineDescription"),
                                "cgi_contractor": Endeavor.Skanetrafiken.TravelInformation.getContractorFromGid(getElementValue(saveEntity.directjourney, "ContractorGid"), caseDate), // NUMERICAL
                                "cgi_deviationmessage": saveEntity.deviationMessage,//getElementValue(saveEntity.directjourney, "Details"),

                                "cgi_startplanned": getISOStringDate(getElementValue(saveEntity.directjourney, "PlannedDepartureDateTime")),
                                "cgi_startactual": getElementValue(saveEntity.directjourney, "ObservedDepartureDateTime"), //ActualDepartureTime
                                "cgi_arivalplanned": getISOStringDate(getElementValue(saveEntity.directjourney, "PlannedArrivalDateTime")),
                                "cgi_arivalactual": getElementValue(saveEntity.directjourney, 'ObservedArrivalDateTime'), //ActualArrivalTime
                                "cgi_startactualdatetime": getElementValueReturnNull(saveEntity.directjourney, "ObservedDepartureDateTime"), //ActualDepartureTime
                                "cgi_arrivalactualdatetime": getElementValueReturnNull(saveEntity.directjourney, 'ObservedArrivalDateTime') //ActualArrivalTime
                            }
                        }

                        if (entity != null)
                            Endeavor.Skanetrafiken.TravelInformation.setDisplayTextRegionbus(entity);
                    }
                    else if (saveEntity.transporttype == "STADSBUSS") {

                        var travelinformation = ((new Date()).toISOString().substring(0, 19)).replace("T", " ") + " => " + getElementValue(saveEntity.directjourney, "DirectionOfLineDescription");

                        if (saveEntity.directjourney == null) {

                            entity = {
                                "cgi_displaytext": "",
                                "cgi_transport": "Stadsbuss",
                                "cgi_travelinformation": travelinformation,
                                "cgi_caseid@odata.bind": "/incidents(" + incidentId + ")",
                                "cgi_line": getElementValue(Endeavor.Skanetrafiken.TravelInformation.getLine(saveEntity.city, saveEntity.line), "LineNumber"),
                                "cgi_tour": "",
                                "cgi_journeynumber": "",
                                "cgi_linedesignation": getElementValue(line, "LineDesignation"),
                                "cgi_city": Endeavor.Skanetrafiken.TravelInformation.getCityFromGid(saveEntity.city),
                                "cgi_start": "",
                                "cgi_stop": "",
                                "cgi_directiontext": "",
                                "cgi_deviationmessage": "",
                                "cgi_contractor": saveEntity.contractorName,
                                "cgi_startactual": "",
                                "cgi_arivalactual": "",
                                "cgi_arivalplanned": null,
                                "cgi_startplanned": null,
                                "cgi_startactualdatetime": null,
                                "cgi_arrivalactualdatetime": null
                            }

                        } else {
                            entity = {
                                "cgi_displaytext": "",
                                "cgi_transport": "Stadsbuss",
                                "cgi_travelinformation": travelinformation,
                                "cgi_caseid@odata.bind": "/incidents(" + incidentId + ")",

                                "cgi_line": getElementValue(line, "LineNumber"),  //Rebusnummer
                                "cgi_tour": getElementValue(saveEntity.directjourney, "JourneyNumber"),
                                "cgi_linedesignation": getElementValue(line, "LineDesignation"), //Publikt linjenummer

                                "cgi_city": Endeavor.Skanetrafiken.TravelInformation.getCityFromGid(saveEntity.city),
                                "cgi_start": Endeavor.Skanetrafiken.TravelInformation.getStopAreaFromGid(document.getElementById("fromlist").value),
                                "cgi_stop": Endeavor.Skanetrafiken.TravelInformation.getStopAreaFromGid(document.getElementById("tolist").value),

                                "cgi_directiontext": getElementValue(saveEntity.directjourney, "DirectionOfLineDescription"),
                                "cgi_deviationmessage": saveEntity.deviationMessage, //getElementValue(saveEntity.directjourney, "Details"),
                                "cgi_contractor": Endeavor.Skanetrafiken.TravelInformation.getContractorFromGid(getElementValue(saveEntity.directjourney, "ContractorGid"), caseDate), // NUMERICAL

                                "cgi_startplanned": getISOStringDate(getElementValue(saveEntity.directjourney, "PlannedDepartureDateTime")),
                                "cgi_startactual": getElementValue(saveEntity.directjourney, 'ObservedDepartureDateTime'), //ActualDepartureTime
                                "cgi_arivalplanned": getISOStringDate(getElementValue(saveEntity.directjourney, "PlannedArrivalDateTime")),
                                "cgi_arivalactual": getElementValue(saveEntity.directjourney, 'ObservedArrivalDateTime'), //ActualArrivalTime
                                "cgi_startactualdatetime": getElementValueReturnNull(saveEntity.directjourney, "ObservedDepartureDateTime"), //ActualDepartureTime
                                "cgi_arrivalactualdatetime": getElementValueReturnNull(saveEntity.directjourney, 'ObservedArrivalDateTime') //ActualArrivalTime
                            }
                        }

                        if (entity != null)
                            Endeavor.Skanetrafiken.TravelInformation.setDisplayTextCitybus(entity);
                    }

                    if (entity == null)
                        Endeavor.formscriptfunctions.AlertCustomDialog("Kunde inte skapa Reseinformation. Vänligen kontakta admin.");

                    /* Do the create*/
                    if (Endeavor.Skanetrafiken.TravelInformation.formContext.getAttribute("statecode") && Endeavor.Skanetrafiken.TravelInformation.formContext.getAttribute("statecode").getValue() == 0) {
                        Endeavor.Skanetrafiken.TravelInformation.saveInProgress = true;

                        Xrm.WebApi.createRecord("cgi_travelinformation", entity).then(
                            function success(CompletedResponse) {
                                Endeavor.Skanetrafiken.TravelInformation.populateSavedTravelInformationTable();
                                Endeavor.Skanetrafiken.TravelInformation.saveInProgress = false;
                            },
                            function (error) {
                                console.log(error.message);
                                Endeavor.formscriptfunctions.AlertCustomDialog("An error occurred when saving Travel Information: " + error.message);
                                Endeavor.Skanetrafiken.TravelInformation.saveInProgress = false;
                            }
                        );
                    }
                    else
                        Endeavor.formscriptfunctions.AlertCustomDialog("Kan inte spara resa för avslutat ärende.");
                }
            }
        },

        deleteTravelInformation: function (travelinformationid) {
            return function () {

                if (!Endeavor.Skanetrafiken.TravelInformation.saveInProgress) {
                    /* Do the delete*/
                    Endeavor.Skanetrafiken.TravelInformation.saveInProgress = true;

                    Xrm.WebApi.deleteRecord("cgi_travelinformation", travelinformationid).then(
                        function success(CompletedResponse) {

                            Endeavor.Skanetrafiken.TravelInformation.populateSavedTravelInformationTable();
                            Endeavor.Skanetrafiken.TravelInformation.saveInProgress = false;
                        },
                        function (errorHandler) {
                            console.log(error.message);
                            Endeavor.formscriptfunctions.AlertCustomDialog("An error occurred when deleting Travel Information: " + errorHandler.message);
                            Endeavor.Skanetrafiken.TravelInformation.saveInProgress = false;
                        }
                    );
                }
            }
        },

        getContractors: function () {

            Endeavor.formscriptfunctions.callGlobalAction("ed_GetContractors", null,
                function (response) {
                    var responsedoc = Endeavor.Skanetrafiken.TravelInformation.getDocXml(response, "GetContractorsResponse", "Contractors");
                    Endeavor.Skanetrafiken.TravelInformation.contractors = responsedoc;
                },
                function (error) {
                    var errorMessage = "Contractors service is unavailable. Please contact your systems administrator. Details: " + error.message;
                    console.log(errorMessage);
                    Endeavor.formscriptfunctions.AlertCustomDialog(errorMessage);
                });
        },

        getOrganisationalUnits: function () {

            Endeavor.formscriptfunctions.callGlobalAction("ed_GetOrganisationalUnits", null,
                function (response) {
                    var responsedoc = Endeavor.Skanetrafiken.TravelInformation.getDocXml(response, "GetOrganisationalUnitsResponse", "Organisations");
                    Endeavor.Skanetrafiken.TravelInformation.organisations = responsedoc;
                },
                function (error) {
                    var errorMessage = "Organisations service is unavailable. Please contact your systems administrator. Details: " + error.message;
                    console.log(errorMessage);
                    Endeavor.formscriptfunctions.AlertCustomDialog(errorMessage);
                });
        },

        minuteDiff: function (dateActual, datePlanned) {

            var minDate = new Date('0001-01-01T12:00:00Z');
            dateActual = dateActual.setSeconds(0, 0);
            datePlanned = datePlanned.setSeconds(0, 0);

            var plannedMinutes = minDate - datePlanned / 60000;
            var realtimeMinutes = minDate - dateActual / 60000;

            return realtimeMinutes - plannedMinutes >= 0 ? "+" + realtimeMinutes - plannedMinutes : realtimeMinutes - plannedMinutes;
        },

        getTour: function (entity) {

            if (entity.cgi_start === "" && entity.cgi_stop === "")
                return "";

            var timeFormat = "HH:mm";
            var isInvalidDate = "Invalid Date";

            var dStartPlanned = new Date(entity.cgi_startplanned);
            var startplannedtime = "X";
            if (entity.cgi_startplanned != null && dStartPlanned.toString() != isInvalidDate)
                startplannedtime = dStartPlanned.format(timeFormat);

            var dArrivalPlanned = new Date(entity.cgi_arivalplanned);
            var arrivalplannedtime = "X";
            if (entity.cgi_arivalplanned != null && dArrivalPlanned.toString() != isInvalidDate)
                arrivalplannedtime = dArrivalPlanned.format(timeFormat);

            var dStartActual = new Date(entity.cgi_startactual);
            var startactualtime = "X";
            if (dStartActual.toString() != isInvalidDate)
                startactualtime = dStartActual.format(timeFormat);

            var dArrivalActual = new Date(entity.cgi_arivalactual);
            var arrivalactualtime = "X";
            if (dArrivalActual.toString() != isInvalidDate)
                arrivalactualtime = dArrivalActual.format(timeFormat);

            var actualStartTimes = " [X] ";
            if (startactualtime != "X" && startplannedtime != "X") {
                var diferenceMinuts = Endeavor.Skanetrafiken.TravelInformation.minuteDiff(dStartActual, dStartPlanned);
                actualStartTimes = " [" + startactualtime + " (" + diferenceMinuts + ")] ";
            }

            var actualArrivalTimes = " [X] ";
            if (arrivalactualtime != "X" && arrivalplannedtime != "X") {
                var diferenceMinuts = Endeavor.Skanetrafiken.TravelInformation.minuteDiff(dArrivalActual, dArrivalPlanned);
                actualArrivalTimes = " [" + arrivalactualtime + " (" + diferenceMinuts + ")] ";
            }

            return "Tur: [" + entity.cgi_tour + "] " + startplannedtime + actualStartTimes + entity.cgi_start + " - " + arrivalplannedtime + actualArrivalTimes + entity.cgi_stop;
        },

        setDisplayTextTrain: function (entity) {

            var trafik = "Trafikslag: " + entity.cgi_transport;
            var line = "Linje: " + entity.cgi_line + " (" + entity.cgi_directiontext + ")";
            var tour = Endeavor.Skanetrafiken.TravelInformation.getTour(entity);
            var contractor = "Entreprenör: " + entity.cgi_contractor;

            entity.cgi_displaytext = trafik + " " + line + " " + tour + " " + contractor;
        },

        setDisplayTextRegionbus: function (entity) {

            var trafik = "Trafikslag: ";
            var line = "Linje: " + entity.cgi_line + " (" + entity.cgi_directiontext + ")";

            var lineNumber = parseInt(entity.cgi_line);
            if (lineNumber > 400 && lineNumber < 430)
                trafik += "SkåneExpressen";
            else
                trafik += entity.cgi_transport;

            var tour = Endeavor.Skanetrafiken.TravelInformation.getTour(entity);
            var contractor = "Entreprenör: " + entity.cgi_contractor;

            entity.cgi_displaytext = trafik + " " + line + " " + tour + " " + contractor;
        },

        setDisplayTextCitybus: function (entity) {

            var trafik = "Trafikslag: " + entity.cgi_transport;
            var stad = "Stad: " + entity.cgi_city;
            var line = "Linje: " + entity.cgi_linedesignation + " (" + entity.cgi_directiontext + ")";
            var tour = Endeavor.Skanetrafiken.TravelInformation.getTour(entity);
            var contractor = "Entreprenör: " + entity.cgi_contractor;

            entity.cgi_displaytext = trafik + " " + stad + " " + line + " " + tour + " " + contractor;
        },

        setDisplayTextCitybusBackUpOldFunction: function (entity) {
            //DEPRECATED NOT IN USE
            var line = Endeavor.Skanetrafiken.TravelInformation.currentLine;

            var trafik = "Trafikslag: ";
            if(line != null)
                trafik += line.getElementsByTagName("LineName")[0].firstChild.nodeValue;

            var city = "Stad: " + entity.cgi_city;
            var tour = "Linje: " + entity.cgi_tour + " [" + entity.cgi_journeynumber + "] (" + entity.cgi_directiontext + ")";

            var startplanned = entity.cgi_startplanned;
            if (startplanned && startplanned.length > 16) {
                startplanned = startplanned.substring(0, 16).replace("T", " ");
            }
            var arrivalplanned = entity.cgi_arivalplanned;
            if (arrivalplanned && arrivalplanned.length > 16) {
                arrivalplanned = arrivalplanned.substring(0, 16).replace("T", " ");
            }
            var startactual = entity.cgi_startactual;
            if (startactual && startactual.length > 16) {
                startactual = startactual.substring(0, 16).replace("T", " ");
            }
            var arrivalactual = entity.cgi_arivalactual;
            if (arrivalactual && arrivalactual.length > 16) {
                arrivalactual = arrivalactual.substring(0, 16).replace("T", " ");
            }

            var line = "Tur: " + startplanned + " [" + startactual + "] " + entity.cgi_start + " - " + arrivalplanned + " [" + arrivalactual + "] " + entity.cgi_stop;
            var contractor = "Entreprenör: " + entity.cgi_contractor;

            entity.cgi_displaytext = trafik + " " + city + " " + tour + " " + line + " " + contractor;
        },

        getCaseEventDate: function (timestamp_label) {
            try {
                debugger;
                var formType = Endeavor.Skanetrafiken.TravelInformation.formContext.ui.getFormType();
                if (formType == 1)
                    return;

                var actionDateAttr = Endeavor.Skanetrafiken.TravelInformation.formContext.getAttribute("cgi_actiondate");
                var arrivalDateAttr = Endeavor.Skanetrafiken.TravelInformation.formContext.getAttribute("cgi_arrival_date");
                var createdOnAttr = Endeavor.Skanetrafiken.TravelInformation.formContext.getAttribute("createdon");

                if (actionDateAttr && actionDateAttr.getValue()) {
                    var actionDateValue = actionDateAttr.getValue();
                    var month = (actionDateValue.getMonth() + 1) < 10 ? "0" + (actionDateValue.getMonth() + 1) : (actionDateValue.getMonth() + 1);
                    var day = actionDateValue.getDate() < 10 ? "0" + actionDateValue.getDate() : actionDateValue.getDate();
                    var hours = actionDateValue.getHours() < 10 ? "0" + actionDateValue.getHours() : actionDateValue.getHours();
                    var minuts = actionDateValue.getMinutes() < 10 ? "0" + actionDateValue.getMinutes() : actionDateValue.getMinutes();
                    timestamp_label.value = actionDateValue.getFullYear() + "-" + month + "-" + day + " " + hours + ":" + minuts;
                }
                else if (arrivalDateAttr && arrivalDateAttr.getValue()) {

                    var arrivalDateValue = arrivalDateAttr.getValue();
                    var month = (arrivalDateValue.getMonth() + 1) < 10 ? "0" + (arrivalDateValue.getMonth() + 1) : (arrivalDateValue.getMonth() + 1);
                    var day = arrivalDateValue.getDate() < 10 ? "0" + arrivalDateValue.getDate() : arrivalDateValue.getDate();
                    var hours = arrivalDateValue.getHours() < 10 ? "0" + arrivalDateValue.getHours() : arrivalDateValue.getHours();
                    var minuts = arrivalDateValue.getMinutes() < 10 ? "0" + arrivalDateValue.getMinutes() : arrivalDateValue.getMinutes();
                    timestamp_label.value = arrivalDateValue.getFullYear() + "-" + month + "-" + day + " " + hours + ":" + minuts;
                }
                else if (createdOnAttr && createdOnAttr.getValue()) {
                    var createdOnValue = createdOnAttr.getValue();
                    var month = (createdOnValue.getMonth() + 1) < 10 ? "0" + (createdOnValue.getMonth() + 1) : (createdOnValue.getMonth() + 1);
                    var day = createdOnValue.getDate() < 10 ? "0" + createdOnValue.getDate() : createdOnValue.getDate();
                    var hours = createdOnValue.getHours() < 10 ? "0" + createdOnValue.getHours() : createdOnValue.getHours();
                    var minuts = createdOnValue.getMinutes() < 10 ? "0" + createdOnValue.getMinutes() : createdOnValue.getMinutes();
                    timestamp_label.value = createdOnValue.getFullYear() + "-" + month + "-" + day + " " + hours + ":" + minuts;
                }

            } catch (e) {
                Endeavor.formscriptfunctions.AlertCustomDialog("Exception caught in Endeavor.Skanetrafiken.TravelInformation.getCaseEventDate. Error: " + e.message);
            }
        },

        getCaseEventDateOldFunction: function (timestamp_label) {
            try {

                var formType = Endeavor.Skanetrafiken.TravelInformation.formContext.ui.getFormType();
                if (formType == 1)
                    return;

                var handelseDatumAttr = Endeavor.Skanetrafiken.TravelInformation.formContext.getAttribute("cgi_handelsedatum");
                var handelseDatumVal = null;

                var actionDatumAttr = Endeavor.Skanetrafiken.TravelInformation.formContext.getAttribute("cgi_actiondate");
                var actionDatumVal = null;

                var arrivalAttr = Endeavor.Skanetrafiken.TravelInformation.formContext.getAttribute("cgi_arrival_date");
                var arrivalVal = null;

                if (handelseDatumAttr)
                    handelseDatumVal = handelseDatumAttr.getValue();
                if (actionDatumAttr)
                    actionDatumVal = actionDatumAttr.getValue();
                if (arrivalAttr)
                    arrivalVal = arrivalAttr.getValue();
                else {
                    timestamp_label.value = new Date();
                    return;
                }

                var year = null;
                var month = null;
                var day = null;
                var hour = null;
                var minute = null;
                var dateTime = null;

                if (handelseDatumVal != null && handelseDatumVal != "") {
                    handelseDatumVal = handelseDatumVal.replace(" ", "");
                    handelseDatumVal = handelseDatumVal.replace("-", "");
                    handelseDatumVal = handelseDatumVal.replace("kl", "");

                    if (isNaN(handelseDatumVal)) {
                        console.log("Handelsedatum is not a number.");
                        dateTime = new Date();
                    }

                    //Ex 20190105
                    if (handelseDatumVal.length == 8) {
                        year = handelseDatumVal.substring(0, 4);
                        month = handelseDatumVal.substring(4, 6);
                        day = handelseDatumVal.substring(6, 8);
                        dateTime = new Date(year, month, day);
                    }

                    //Ex 201901051424
                    else if (handelseDatumVal.length == 12) {
                        year = handelseDatumVal.substring(0, 4);
                        month = handelseDatumVal.substring(4, 6);
                        day = handelseDatumVal.substring(6, 8);
                        hour = handelseDatumVal.substring(8, 10);
                        minute = handelseDatumVal.substring(10, 12);

                        //In JS month start with index 0.
                        var parsedMonth = parseInt(month);
                        parsedMonth--;
                        month = "0" + parsedMonth;

                        dateTime = new Date(year, month, day, hour, minute);
                    }

                    //Ex 1901051424
                    else if (handelseDatumVal.length == 10) {
                        year = "20" + handelseDatumVal.substring(0, 2);
                        month = handelseDatumVal.substring(2, 4);
                        day = handelseDatumVal.substring(4, 6);
                        hour = handelseDatumVal.substring(6, 8);
                        minute = handelseDatumVal.substring(8, 10);

                        //In JS month start with index 0.
                        var parsedMonth = parseInt(month);
                        parsedMonth--;
                        month = "0" + parsedMonth;

                        dateTime = new Date(year, month, day, hour, minute);
                    }
                }

                else if (actionDatumVal != null && actionDatumVal != "")
                    dateTime = actionDatumVal;
                else if (arrivalVal != null && arrivalVal != "")
                    dateTime = arrivalVal;
                else
                    dateTime = new Date();

                timestamp_label.value = dateTime;

            } catch (e) {
                Endeavor.formscriptfunctions.AlertCustomDialog("Exception caught in Endeavor.Skanetrafiken.TravelInformation.getCaseEventDate. Error: " + e.message);
            }
        },
    }
}