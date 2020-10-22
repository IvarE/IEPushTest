// JavaScript source code
// References:
/// <reference path="vsdoc/jquery_2.1.4.js" />
/// <reference path="SDK.Rest.js" />
/// <reference path="CrmFetchKit.js" />
/// <reference path="Endeavor.Common.Data.js" />
/// <reference path="Sdk.edp_CasCompany.min.js" />
// Begin scoping 
if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Creditsafe) == "undefined") {
    Endeavor.Creditsafe = {
    };
}

if (typeof (Endeavor.Creditsafe.CreditsafeAccount) == "undefined") {
    Endeavor.Creditsafe.CreditsafeAccount = {

        CreditsafeCompanySearchDefaultLogin: function () {
            try {
                /// <summary>
                /// Open creditsafe homepage logged in with the default username and password.
                /// </summary>
                var iMUrl = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "edp_CreditsafeConfigurationSet?$select=edp_UserName,edp_Password,edp_SearchEngine";
                var resultSet = Endeavor.Common.Data.fetchJSONResults(iMUrl);
                if (resultSet != null) {
                    var orgObject = Xrm.Page.getAttribute("edp_orgno");
                    if (orgObject != null) {
                        var orgNumber = orgObject.getValue();
                        var username = resultSet[0].edp_UserName;
                        var password = resultSet[0].edp_Password;
                        var url = "";
                        for (var i = 0; i < resultSet.length; i++) {
                            // Is sweden?
                            if (resultSet[i].edp_SearchEngine.Value == 757550000) {
                                if (orgNumber != null) {
                                    url = "https://login.creditsafe.com/?P1=" + username + "&P2=" + password + "&P3=" + orgNumber;
                                } else {
                                    url = "https://login.creditsafe.com/?P1=" + username + "&P2=" + password;
                                }

                                return url;
                                break;
                            }
                        }

                        if (url == "") {
                            throw Error("Could not find Creditsafe login credentials for current user, you need to login manually from the opened login page.");
                        }
                    }
                }

            } catch (error) {
                throw error;
            }
        },

        CreditsafeCompanySearch: function () {
            try {
                debugger;
                var url;
                var orgNumber = "";
                var orgObject = Xrm.Page.getAttribute("edp_orgno");
                if (orgObject != null)
                    orgNumber = orgObject.getValue();
                var userId = Xrm.Page.context.getUserId();
                if (userId != null) {
                    var iMUrl = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "edp_CreditsafeUserConfigurationSet?$select=*&$filter=edp_SystemUserId/Id eq (guid'" + userId.slice(1, (userId.length - 1)) + "')";
                    var resultSet = Endeavor.Common.Data.fetchJSONResults(iMUrl);
                    if (resultSet && resultSet.length > 0)
                        if (resultSet[0] != null) {
                            var result = resultSet[0];
                            if (result.edp_Username != null) {
                                //url = "https://www.creditsafe.se/CSSEWebsite/WebPages/Login.aspx?P1=" + result.edp_Username + "&P2=&P3=" + orgNumber;
                                url = "https://login.creditsafe.com/?P1=" + result.edp_Username + "&P2=&P3=" + orgNumber;
                            }
                        }
                } else {
                    url = Endeavor.Creditsafe.CreditsafeAccount.CreditsafeCompanySearchDefaultLogin();
                }
                if (url == "" || url == null || url == undefined) {
                    throw Error("Could not find Creditsafe login credentials for current user, you need to login manually from the opened login page.");
                } else {
                    window.open(url);
                }
            } catch (e) {
                Xrm.Utility.alertDialog("Exception caught in CreditsafeCompanySearch() :\n\n" + e.toString());
                window.open("https://login.creditsafe.com/");
            }
        },

        updateInfoFromCreditsafe: function () {
            /// <summary>
            /// Updates or Connects an existing CRM account to creditsafe
            /// </summary>
            try {
                // Record exists in CRM
                if (Xrm.Page.ui.getFormType() == 2) {

                    // Load resources
                    Endeavor.Creditsafe.CreditsafeAccount.getUserJavaScriptResourcesForAccount();

                    var Id = Xrm.Page.data.entity.getId();

                    var updateResult = Endeavor.Creditsafe.CreditsafeAccount.updateUsingAccountId(Id);

                    if (updateResult != undefined && updateResult != null && updateResult != "") {
                        if (updateResult != 'Account Not Connected') {
                            Xrm.Utility.alertDialog(updateResult);
                        }
                        //Refresh record/form
                        Xrm.Page.data.refresh();

                        //Xrm.Page.ui.setFormNotification(Endeavor.Creditsafe.Resources.AccountUpdatedRefresh + " (F5)", "INFORMATION");
                    }
                    else {
                        Xrm.Page.ui.setFormNotification("No results at all. Something went wrong when communicating with the server", "ERROR");
                    }
                }
            } catch (error) {
                Xrm.Utility.alertDialog(error + "\n\nException caught in updateInfoFromCreditsafe()");
            }
        },

        updateUsingAccountId: function (Id) {
            /// <summary>
            /// Try/Catch must be handled by caller
            /// </summary>
            /// <param name="Id"></param>
            var aUrl = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "AccountSet?$select=Name,edp_CreditSafeId,edp_OrgNo,edp_Address1_CountryId,edp_CreditsafeReasonCode,edp_LegalClassification&$filter=AccountId eq (guid'" + Id + "')";
            var aResultSet = Endeavor.Common.Data.fetchJSONResults(aUrl);

            if (aResultSet != null && aResultSet.length > 0) {

                var Name = aResultSet[0].Name;
                var edp_CreditSafeId = aResultSet[0].edp_CreditSafeId;
                var edp_OrgNo = aResultSet[0].edp_OrgNo;
                var edp_Address1_CountryId = aResultSet[0].edp_Address1_CountryId;
                var edp_CreditsafeReasonCode = aResultSet[0].edp_CreditsafeReasonCode;
                var countryCode = "";
                var edp_LegalClassification = aResultSet[0].edp_LegalClassification;

                if (edp_Address1_CountryId != null && edp_Address1_CountryId.Id != null) {
                    // Get country code
                    var cURL = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "edp_CountrySet?$select=edp_CountryCode&$filter=edp_CountryId eq (guid'" + edp_Address1_CountryId.Id + "')";
                    var cResultSet = Endeavor.Common.Data.fetchJSONResults(cURL);

                    if (cResultSet != null && cResultSet.length > 0)
                        countryCode = cResultSet[0].edp_CountryCode;
                }

                // Mapped?
                if (edp_CreditSafeId == null || countryCode == "") {
                    // Ask and open search dialog.
                    var msg = String.format(Endeavor.Creditsafe.Resources.MapAccountQuestion, Name);   // "Account " + Name + " is not mapped in Creditsafe. Do mapping now?"
                    if (confirm(msg)) {
                        Endeavor.Creditsafe.SearchCreditsafe.openSearchWindow(Name, Id, countryCode, edp_OrgNo);
                        return "Account Not Connected";
                    }
                }
                else {
                    // Update information from Creditsafe without GUI
                    var jsonArray = [];
                    // Create json manually, imitate frontend
                    var item = {
                        "edp_OrganisationNumber": edp_OrgNo,
                        "Name": Name,
                        "Street1": "",
                        "Address1_City": "",
                        "Address1_PostalCode": "",
                        "edp_CountryName": {
                            "Id": edp_Address1_CountryId.Id,
                            "LogicalName": edp_Address1_CountryId.LogicalName,
                            "Name": edp_Address1_CountryId.Name
                        },
                        "edp_Address1_CountryCode": countryCode,
                        "Address1_Lattitude": "",
                        "Address1_Longitude": "",
                        "Status": "",
                        "CRMId": Id,
                        "ReportType": "",
                        "ggsId": edp_CreditSafeId,
                        "EntityName": "account",
                        "ReasonCode": edp_CreditsafeReasonCode,
                        "CompanyType": edp_LegalClassification
                    };
                    jsonArray.push(item);
                    // Update silently
                    return Endeavor.Creditsafe.SearchCreditsafe.updateCRM(jsonArray, null, true);
                }
            }
            else {
                return "No Account found in Database with Guid: " + Id + ". Please refresh and try again. (F5)";
            }

        },


        updateMonitoredCompanies: function (seletedItems) {
            try {

                /// <summary>
                /// Start update of all monitored companies, both GGS and Sweden.
                /// Starts workflow that does the processing
                /// </summary>
                /// <returns type=""></returns>
                var entityId = null;

                // Load resources if not already loaded
                Endeavor.Creditsafe.CreditsafeAccount.getUserJavaScriptResourcesForAccount();

                var arrayLength = seletedItems.length;
                if (arrayLength <= 0) {
                    Xrm.Utility.alertDialog(Endeavor.Creditsafe.Resources.SelectedRowsErrorMsg);
                    return;
                }
                else {
                    // Get first account in selection
                    entityId = seletedItems[0].toString();
                }

                // Remove already started workflow.
                Endeavor.Creditsafe.CreditsafeAccount.deleteWaitingWorkflows();

                var _return = window.confirm(Endeavor.Creditsafe.Resources.StartUpdatedMonitoredCompanies);     // 'Start update of monitored Creditsafe companies?'
                if (_return) {
                    var url = Xrm.Page.context.getClientUrl();

                    var workflowId = 'BCAECB3B-E7CB-4565-8F0B-A88E666C4774';    // Run portfolio update
                    var OrgServicePath = "/XRMServices/2011/Organization.svc/web";
                    url = url + OrgServicePath;
                    var request;
                    request = "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\">" +
                        "<s:Body>" +
                        "<Execute xmlns=\"http://schemas.microsoft.com/xrm/2011/Contracts/Services\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\">" +
                        "<request i:type=\"b:ExecuteWorkflowRequest\" xmlns:a=\"http://schemas.microsoft.com/xrm/2011/Contracts\" xmlns:b=\"http://schemas.microsoft.com/crm/2011/Contracts\">" +
                        "<a:Parameters xmlns:c=\"http://schemas.datacontract.org/2004/07/System.Collections.Generic\">" +
                        "<a:KeyValuePairOfstringanyType>" +
                        "<c:key>EntityId</c:key>" +
                        "<c:value i:type=\"d:guid\" xmlns:d=\"http://schemas.microsoft.com/2003/10/Serialization/\">" + entityId + "</c:value>" +
                        "</a:KeyValuePairOfstringanyType>" +
                        "<a:KeyValuePairOfstringanyType>" +
                        "<c:key>WorkflowId</c:key>" +
                        "<c:value i:type=\"d:guid\" xmlns:d=\"http://schemas.microsoft.com/2003/10/Serialization/\">" + workflowId + "</c:value>" +
                        "</a:KeyValuePairOfstringanyType>" +
                        "</a:Parameters>" +
                        "<a:RequestId i:nil=\"true\" />" +
                        "<a:RequestName>ExecuteWorkflow</a:RequestName>" +
                        "</request>" +
                        "</Execute>" +
                        "</s:Body>" +
                        "</s:Envelope>";

                    var req = new XMLHttpRequest();
                    req.open("POST", url, true)
                    // Responses will return XML. It isn't possible to return JSON.
                    req.setRequestHeader("Accept", "application/xml, text/xml, */*");
                    req.setRequestHeader("Content-Type", "text/xml; charset=utf-8");
                    req.setRequestHeader("SOAPAction", "http://schemas.microsoft.com/xrm/2011/Contracts/Services/IOrganizationService/Execute");
                    req.onreadystatechange = function () { Endeavor.Creditsafe.CreditsafeAccount.workflowStartedStatus(req); };
                    req.send(request);
                }
            } catch (error) {
                Xrm.Utility.alertDialog(error + "\n\nException caught in updateMonitoredCompanies()");
            }
        },

        workflowStartedStatus: function (req) {
            /// <summary>
            /// Status function for workflow execution
            /// </summary>
            /// <param name="req"></param>
            if (req.readyState == 4) {
                if (req.status == 200) {
                    // Load resources if not already loaded
                    Endeavor.Creditsafe.CreditsafeAccount.getUserJavaScriptResourcesForAccount();

                    Xrm.Utility.alertDialog(Endeavor.Creditsafe.Resources.MonitoringUpdateStarted);       // Update of monitored companies started. Please see status in System Jobs (workflow)
                }
            }
        },

        deleteWaitingWorkflows: function () {
            /// <summary>
            /// Delete all waiting workflows
            /// </summary>
            try {
                // Get all waiting workflows with name 'Run portfolio update'
                var iMUrl = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "AsyncOperationSet?$select=Name,AsyncOperationId&$filter=StatusCode/Value eq 10 and Name eq 'Run portfolio update'";
                var resultSet = Endeavor.Common.Data.fetchJSONResults(iMUrl);
                if (resultSet != null) {
                    for (var i = 0; i < resultSet.length; i++) {
                        var Name = resultSet[i].Name;
                        var AsyncOperationId = resultSet[i].AsyncOperationId;

                        // Delete record
                        SDK.REST.deleteRecord(
                            AsyncOperationId,
                            "AsyncOperation",
                            function (successCallback) {
                            }, function (errorCallback) {
                                Xrm.Utility.alertDialog("deleteRecord generated an error: " + errorCallback);
                            });
                    }
                }
            } catch (error) {
                Xrm.Utility.alertDialog(error + "\n\nException caught in deleteWaitingWorkflows()");
            }

        },


        confirmUpdateCreditRating: function () {
            var lastCreditReport = Xrm.Page.getAttribute("edp_lastcreditreport");

            if (lastCreditReport === "--" || lastCreditReport === null || lastCreditReport.getValue() === null)
                lastCreditReport = "Never.";
            else
                lastCreditReport = lastCreditReport.getValue().toDateString();

            Xrm.Utility.confirmDialog(
                "Do you want to initiate a Credit Report? \n \n Last Credit Report issued: " + lastCreditReport,
                function () {
                    Endeavor.Creditsafe.CreditsafeAccount.updateCreditRating();
                });
        },

        updateCreditRating: function () {
            try {
                var orgNumber = "";

                if (Xrm.Page.getAttribute("edp_orgno") != null)
                    orgNumber = Xrm.Page.getAttribute("edp_orgno").getValue();
                else
                    Xrm.Utility.alertDialog("The organization number is missing.");


                if (orgNumber != null && orgNumber != "") {

                    var request = new Sdk.edp_GetCredentialsRequest();
                    var response = Sdk.Sync.execute(request);

                    if (response === null) {
                        Xrm.Utility.alertDialog("Was not able to retrieve credentials.");
                        throw new Error("The request did not return anything.");
                    } else {

                        var jsonString = response.getConfigcredentials();
                        var data = jsonString.split(';');

                        var userName = data[1];
                        var password = data[2];

                        var url = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "edp_CreditsafeConfigurationSet?$select=edp_CreditWorthinessTemplate&$filter=edp_SearchEngine/Value eq 757550000";
                        var resultSet = Endeavor.Common.Data.fetchJSONResults(url);

                        if (resultSet[0] != null) {
                            var result = resultSet[0];

                            if (userName != null && userName != "" && password != null && password != "" && result.edp_CreditWorthinessTemplate != null && result.edp_CreditWorthinessTemplate != "") {
                                var abReq = new Sdk.edp_IsCompanyStakeholderRequest(orgNumber, userName, password);
                                var abResp = Sdk.Sync.execute(abReq);

                                var isStakeholderCompany = abResp.getStakeholderCompany();

                                if (!isStakeholderCompany == true) {
                                    Xrm.Utility.alertDialog("This operation is only allowed for AB");
                                    // TODO: teo - Update the Form if needed with the information that this isn't a AB
                                    return;
                                }

                                var request = new Sdk.edp_CasCompanyRequest(orgNumber, userName, password, result.edp_CreditWorthinessTemplate);
                                var response = Sdk.Sync.execute(request);

                                if (response === null)
                                    throw new Error("The request update Credit Rating did not return anything.");

                                var casResult = response.getCasCompanyRes();

                                if (casResult != null && casResult != '')
                                    Endeavor.Creditsafe.CreditsafeAccount.updateFromResponse(casResult);
                            } else if (userName == null || userName == "")
                                Xrm.Utility.alertDialog("Systemparameter is missing information in the 'Username'-field.");
                            else if (password == null || password == "")
                                Xrm.Utility.alertDialog("Systemparameter is missing information in the 'Password'-field.");
                            else if (result.edp_CreditWorthinessTemplate == null || result.edp_CreditWorthinessTemplate == "")
                                Xrm.Utility.alertDialog("Systemparameter is missing information in the 'Credit Worthiness Template'-field.");
                        }
                        else
                            Xrm.Utility.alertDialog("SystemParameter is missing. Please configure.");
                    }

                }
                else
                    Xrm.Utility.alertDialog("The organization number is missing.");
            }
            catch (e) {
                Xrm.Utility.alertDialog(e.message);
            }
        },

        updateFromResponse: function (response) {
            try {
                var creditTrating = null;
                var accountid = Xrm.Page.data.entity.getId();
                var entityName = Xrm.Page.data.entity.getEntityName();

                switch (response) {
                    case "1":
                        creditTrating = 757550000;
                        break;
                    case "2":
                        creditTrating = 757550001;
                        break;
                    case "4":
                        creditTrating = 757550002;
                        break;
                    default:
                        Xrm.Utility.alertDialog("Unrecognised responsecode.");
                        break;
                }

                // Update Account with creditrating and lastcreditreport(date)
                if (creditTrating != null) {
                    var currentDateTime = new Date();
                    var accEntity = {
                        edp_lastcreditreport: currentDateTime,
                        edp_creditrating: { Value: creditTrating }
                    };
                    SDK.REST.updateRecord(accountid, accEntity, "Account", function (data) {
                        // Reload form
                        //Xrm.Page.data.refresh();
                        //Xrm.Utility.openEntityForm(entityName, accountid);
                    },
                        function (error) {
                            Xrm.Utility.alertDialog(error);
                        });
                }
                else {
                    Xrm.Utility.alertDialog("Credit worthiness according to credit template not retrieved correctly.");
                    return;
                }

                var message = null;
                switch (response) {
                    case "1":
                        Xrm.Page.ui.setFormNotification("Credit rating is 'Approved'. Please refresh page (F5).", "INFORMATION");
                        break;
                    case "2":
                        Xrm.Page.ui.setFormNotification("Credit rating is 'Not Approved'. Please refresh page (F5).", "ERROR");
                        break;
                    case "4":
                        Xrm.Page.ui.setFormNotification("Credit rating is 'Manual Assessment needed'. Please refresh page (F5).", "WARNING");
                }

                // Set notification depending on creditrating (Approved / Not Approved / Assess)
                if (message != null)
                    Xrm.Utility.alertDialog(message);
            }
            catch (exc) {
                Xrm.Utility.alertDialog(exc);
            }
        },
        getUserJavaScriptResourcesForAccount: function () {

            if (typeof jQuery.cachedScript === 'undefined') {
                jQuery.cachedScript = function (url, options) {

                    // Allow user to set any option except for dataType, cache, and url
                    options = $.extend(options || {}, {
                        dataType: "script",
                        cache: true,
                        url: url,
                        async: false
                    });

                    // Use $.ajax() since it is more flexible than $.getScript
                    // Return the jqXHR object so we can chain callbacks
                    return jQuery.ajax(options);
                };
            }

            var resourceFile = Xrm.Page.context.getClientUrl() + "/WebResources/edp_/script/Endeavor.Creditsafe.Resources.1033.js";

            // Always load English
            $.cachedScript(resourceFile).done(function (script, textStatus) {
                // Resource loaded.
            }).fail(function (jqxhr, settings, exception) {
                alert("Resources failed to load resources from \"" + resourceFile + "\". Exception: " + exception);
            });

            var lcid = Xrm.Page.context.getUserLcid();
            switch (lcid) {
                case 1053:  // Swedish
                    resourceFile = Xrm.Page.context.getClientUrl() + "/WebResources/edp_/script/Endeavor.Creditsafe.Resources.1053.js";
                    break;
                case 1031:  // German
                    resourceFile = Xrm.Page.context.getClientUrl() + "/WebResources/edp_/script/Endeavor.Creditsafe.Resources.1031.js";
                    break;
                default:
                    // Keep english 1033.
                    break;
            }

            // Don't load english again.
            if (lcid != 1033) {
                $.cachedScript(resourceFile).done(function (script, textStatus) {
                    // Resource loaded.
                }).fail(function (jqxhr, settings, exception) {
                    alert("Resources failed to load resources from \"" + resourceFile + "\". Exception: " + exception);
                });
            }
        }
    }
};