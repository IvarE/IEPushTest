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

        CreditsafeCompanySearchDefaultLogin: function (formContext) {
            try {
                /// <summary>
                /// Open creditsafe homepage logged in with the default username and password.
                /// </summary>

                var request = {
                    getMetadata: function () {
                        return {
                            boundParameter: null,
                            operationType: 0,
                            operationName: "edp_GetCredentials"
                        };
                    }
                };

                var orgObject = formContext.getAttribute("edp_orgno");
                if (orgObject != null) {
                    var orgNumber = orgObject.getValue();
                    Xrm.WebApi.online.execute(request).then(
                        function (result) {
                            if (result.ok) {
                                result.json().then(
                                    function (response) {
                                        var jsonString = response.configcredentials;
                                        var data = jsonString.split(';');
                                        var username = data[1];
                                        var password = data[2];
                                        var url = "https://login.creditsafe.com/?P1=" + username + "&P2=" + password + "&P3=" + orgNumber;
                                        window.open(url);
                                    }
                                );
                            } else {
                                throw new Error("The request failed. Please try again.");
                            }
                        }, function (error) {
                            throw new Error("An error occurred. " + error);
                        }
                    );
                }

                /*var iMUrl = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "edp_CreditsafeConfigurationSet?$select=edp_UserName,edp_Password,edp_SearchEngine";
                var resultSet = Endeavor.Common.Data.fetchJSONResults(iMUrl);
                if (resultSet != null) {
                    var orgObject = formContext.getAttribute("edp_orgno");
                    if (orgObject != null) {
                        var orgNumber = orgObject.getValue();
                        var username = resultSet[0].edp_UserName;
                        var password = resultSet[0].edp_Password;
                        var url = "";
                        for (var i = 0; i < resultSet.length; i++) {
                            // Is sweden?
                            if (resultSet[i].edp_SearchEngine.Value == 757550000) {
                                if (orgNumber != null) {
                                    url = "https://www.creditsafe.se/CSSEWebsite/WebPages/Login.aspx?P1=" + username + "&P2=" + password + "&P3=" + orgNumber;
                                } else {
                                    url = "https://www.creditsafe.se/CSSEWebsite/WebPages/Login.aspx?P1=" + username + "&P2=" + password;
                                }

                                return url;
                                break;
                            }
                        }

                        if (url == "") {
                            throw Error("Could not find Creditsafe login credentials for current user, you need to login manually from the opened login page.");
                        }
                    }
                }*/

            } catch (error) {
                throw error;
            }
        },

        CreditsafeCompanySearch: function (primaryControl) {
            try {
                //window.open("https://login.creditsafe.com/");
                //The url base url below doesn't seem to work anymore. For now use https://login.creditsafe.com/
                debugger;
                var formContext = primaryControl;
                var url;
                var orgNumber = "";
                var orgObject = formContext.getAttribute("edp_orgno");
                if (orgObject != null)
                    orgNumber = orgObject.getValue();

                var userId = Xrm.Utility.getGlobalContext().userSettings.userId;
                if (userId) {
                    var iMUrl = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "edp_CreditsafeUserConfigurationSet?$select=*&$filter=edp_SystemUserId/Id eq (guid'" + userId.slice(1, (userId.length - 1)) + "')";
                    var resultSet = Endeavor.Common.Data.fetchJSONResults(iMUrl);
                    if (resultSet && resultSet.length > 0)
                        if (resultSet[0] != null) {
                            var result = resultSet[0];
                            var password = result.edp_Password === null ? "empty" : result.edp_Password;
                            if (result.edp_Username != null) {
                                //url = "https://www.creditsafe.se/CSSEWebsite/WebPages/Login.aspx?P1=" + result.edp_Username + "&P2=&P3=" + orgNumber;
                                url = "https://login.creditsafe.com/?P1=" + result.edp_Username + "&P2=" + password + "&P3=" + orgNumber;
                            }
                        }
                } else {
                    Endeavor.Creditsafe.CreditsafeAccount.CreditsafeCompanySearchDefaultLogin(formContext);
                }
                if (!url) {
                    throw Error("Could not find Creditsafe login credentials for current user, you need to login manually from the opened login page.");
                } else {
                    window.open(url);
                }
            } catch (e) {
                window.open("https://login.creditsafe.com/");
                Xrm.Navigation.openAlertDialog({ text: "Exception caught in CreditsafeCompanySearch() :\n\n" + e.toString() });
            }
        },

        updateInfoFromCreditsafe: function (primaryControl) {
            /// <summary>
            /// Updates or Connects an existing CRM account to creditsafe
            /// </summary>
            try {

                debugger;

                var formContext = primaryControl;
                // Record exists in CRM
                if (formContext.ui.getFormType() == 2) {

                    // Load resources
                    Endeavor.Creditsafe.CreditsafeAccount.getUserJavaScriptResourcesForAccount();

                    var Id = formContext.data.entity.getId();

                    //var updateResult = Endeavor.Creditsafe.CreditsafeAccount.updateUsingAccountId(Id);
                    Endeavor.Creditsafe.CreditsafeAccount.updateUsingAccountId(formContext, Id);

                }
            } catch (error) {
                Xrm.Navigation.openAlertDialog({ text: error + "\n\nException caught in updateInfoFromCreditsafe()" });
            }
        },

        unescapeXml: function (str) {
            str = str.replace(/&lt;/g, '<');
            str = str.replace(/&gt;/g, '>');
            str = str.replace(/&amp;/g, '&');
            str = str.replace(/&apos;/g, '\'');
            str = str.replace(/&quot;/g, '"');
            return str;
        },

        escapeXml: function (unsafe) {
            return unsafe.replace(/[<>&'"]/g, function (c) {
                switch (c) {
                    case '<': return '&lt;';
                    case '>': return '&gt;';
                    case '&': return '&amp;';
                    case '\'': return '&apos;';
                    case '"': return '&quot;';
                }
            });
        },

        updateMultiAccounts: function (accountRefs, selectedControl) { //SelectedControlSelectedItemReferences, SelectedControl
            debugger;
            Endeavor.Creditsafe.CreditsafeAccount.getUserJavaScriptResourcesForAccount();
            Xrm.Utility.showProgressIndicator(Endeavor.Creditsafe.Resources.AccountsUpdateProgress);
            var promises = [];
            var notUpdated = 0;
            var updated = 0;
            for (var i = 0; i < accountRefs.length; i++) {
                var id = accountRefs[i].Id;
                var info = Endeavor.Creditsafe.CreditsafeAccount.getAccountInfo(id);
                if (info && info.CreditsafeId) {
                    promise = Endeavor.Creditsafe.SearchCreditsafe.forceUpdateCRM(info.jsonArray, true, null, "nothing");
                    promises.push(promise);
                } else {
                    notUpdated++;
                }
            }

            //this uses blue bird promises, which works for native promises
            //and bluebird promises that are returened from xrm.webapi in 
            //internet explorer
            Promise.all(promises).then(
                function () {
                    debugger;
                    Xrm.Utility.closeProgressIndicator();
                    var message = "Results: \n";
                    for (var i = 0; i < arguments[0].length; i++) {
                        if (arguments[0][i] != null) {
                            if (arguments[0][i].indexOf("successfully") == -1) {
                                notUpdated++;
                            } else {
                                updated++;
                            }
                        } else {
                            notUpdated++;
                        }
                    }
                    message += updated + " " + Endeavor.Creditsafe.Resources.AccountsUpdateSucc + "\n";
                    message += notUpdated + " " + Endeavor.Creditsafe.Resources.AccountsUpdateFail;
                    var alertStrings = { confirmButtonLabel: "Ok", text: message };
                    var alertOptions = { height: 240, width: 360 };
                    Xrm.Navigation.openAlertDialog(alertStrings, alertOptions).then(
                        function () {
                            selectedControl.refresh();
                        },
                        function () {
                            console.log('error in account multi-update.');
                        }
                    );
                },
                function () {
                    Xrm.Utility.closeProgressIndicator();
                    var msg = arguments.length > 0 ? arguments[0].message : "Error message not found";
                    alert("Error in updateMultiAccounts :" + error.message)
                }
            );
        },

        updateUsingAccountId: function (formContext, Id) {
            var info = Endeavor.Creditsafe.CreditsafeAccount.getAccountInfo(Id);
            var jsonArray;
            var edp_CreditSafeId;
            var countryCode;
            var edp_OrgNo;
            var Name;
            if (info) {
                Name = Endeavor.Creditsafe.CreditsafeAccount.unescapeXml(info.Name);
                edp_CreditSafeId = info.CreditsafeId;
                jsonArray = info.jsonArray;
                countryCode = info.CountryCode;
                edp_OrgNo = info.OrgNo;
            } else {
                return null;
            }
            // Mapped?
            if (!edp_CreditSafeId || countryCode == "" || (countryCode == "SE" && !edp_OrgNo)) {
                // Ask and open search dialog.
                var msg = String.format(Endeavor.Creditsafe.Resources.MapAccountQuestion, Name);   // "Account " + Name + " is not mapped in Creditsafe. Do mapping now?"
                if (confirm(msg)) {
                    Endeavor.Creditsafe.SearchCreditsafe.openSearchWindow(Name, Id, countryCode, edp_OrgNo);
                    return "Account Not Connected";
                }
            } else {
                return Endeavor.Creditsafe.SearchCreditsafe.updateCRM(formContext, jsonArray, null, true, null, 0);
            }
        },

        getAccountInfo: function (Id) {
            var aUrl = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "AccountSet?$select=Name,edp_CreditSafeId,edp_OrgNo,Address1_City,Address1_Line1,Address1_PostalCode,edp_Address1_CountryId,edp_CreditsafeReasonCode,edp_LegalClassification&$filter=AccountId eq (guid'" + Id + "')";
            var aResultSet = Endeavor.Common.Data.fetchJSONResults(aUrl);

            if (aResultSet != null && aResultSet.length > 0) {

                var Name = aResultSet[0].Name;
                var nameStr = Endeavor.Creditsafe.CreditsafeAccount.escapeXml(Name);
                var edp_CreditSafeId = aResultSet[0].edp_CreditSafeId;
                var edp_OrgNo = aResultSet[0].edp_OrgNo;
                var edp_Address1_CountryId = aResultSet[0].edp_Address1_CountryId;
                var address1_line1 = aResultSet[0].Address1_Line1;
                var address1_city = aResultSet[0].Address1_City;
                var address1_postalcode = aResultSet[0].Address1_PostalCode;
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
                // Update information from Creditsafe without GUI
                var jsonArray = [];
                // Create json manually, imitate frontend
                var item = {
                    "edp_OrganisationNumber": edp_OrgNo,
                    "Name": nameStr,
                    "Street1": address1_line1,
                    "Address1_City": address1_city,
                    "Address1_PostalCode": address1_postalcode,
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
                return { jsonArray: jsonArray, CreditsafeId: edp_CreditSafeId, CountryCode: countryCode, OrgNo: edp_OrgNo, Name: nameStr }
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
                    Xrm.Navigation.openAlertDialog({ text: Endeavor.Creditsafe.Resources.SelectedRowsErrorMsg });
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
                    var url = Xrm.Utility.getGlobalContext().getClientUrl();

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
                Xrm.Navigation.openAlertDialog({ text: error + "\n\nException caught in updateMonitoredCompanies()" });
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
                    Xrm.Navigation.openAlertDialog({ text: Endeavor.Creditsafe.Resources.MonitoringUpdateStarted });       // Update of monitored companies started. Please see status in System Jobs (workflow)
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

                        Xrm.WebApi.online.deleteRecord("AsyncOperation", AsyncOperationId).then(
                            function (result) {
                                console.log("deleted a workflow succssfully");
                            },
                            function (error) {
                                Xrm.Navigation.openAlertDialog({ text: "deleteRecord generated an error: " + errorCallback });
                            }
                        );
                    }
                }
            } catch (error) {
                Xrm.Navigation.openAlertDialog({ text: error + "\n\nException caught in deleteWaitingWorkflows()" });
            }

        },

        oDataDateToDate: function (oDataDate) {
            if (oDataDate == null || oDataDate == "") { return null; };

            var dt = oDataDate.replace("/Date(", "").replace(")/", "");

            var dateValue = new Date(parseInt(dt, 10));

            return dateValue;
        },

        confirmUpdateCreditRating: function (primaryControl) {
            var formContext = primaryControl;

            Endeavor.Creditsafe.CreditsafeAccount.getUserJavaScriptResourcesForAccount();

            var lastCreditReport = formContext.getAttribute("edp_lastcreditreport");

            if (lastCreditReport === "--" || lastCreditReport === null || lastCreditReport.getValue() === null) {
                //the field may not be available on this form, look up real value from database
                var Id = formContext.data.entity.getId();
                if (Id != null) {
                    var aUrl = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "AccountSet?$select=edp_lastcreditreport&$filter=AccountId eq (guid'" + Id + "')";
                    var aResultSet = Endeavor.Common.Data.fetchJSONResults(aUrl);
                    if (aResultSet != null && aResultSet.length > 0) {

                        if (aResultSet[0].edp_lastcreditreport != null) {
                            lastCreditReport = Endeavor.Creditsafe.CreditsafeAccount.oDataDateToDate(aResultSet[0].edp_lastcreditreport).toISOString();
                        }
                    }
                }

                if (lastCreditReport === null || (lastCreditReport.getValue && lastCreditReport.getValue() === null)) {

                    lastCreditReport = Endeavor.Creditsafe.Resources.Never ? Endeavor.Creditsafe.Resources.Never : 'Never';
                }
            }
            else {
                lastCreditReport = lastCreditReport.getValue().toISOString();
            }

            lastCreditReport = lastCreditReport.substring(0, 10);

            var confirmationText = Endeavor.Creditsafe.Resources.ConfirmAccountCreditRating ? Endeavor.Creditsafe.Resources.ConfirmAccountCreditRating : 'Do you want to initiate a Credit Rating Request?\n\nLast Credit Rating:';
            var confirmationTitle = Endeavor.Creditsafe.Resources.CreditRatingTitle ? Endeavor.Creditsafe.Resources.CreditRatingTitle : 'Update Credit Rating';

            var confirmStrings = { text: confirmationText + ' ' + lastCreditReport, title: confirmationTitle };
            var confirmOptions = { height: 200, width: 450 };
            Xrm.Navigation.openConfirmDialog(confirmStrings, confirmOptions).then(
                function (result) {
                    if (result.confirmed) {
                        Endeavor.Creditsafe.CreditsafeAccount.updateCreditRating(formContext);
                    }
                }
            );
        },

        isCompanyStakeHolder: function (orgNumber, username, password) {
            var request = {
                orgNo: orgNumber,
                username: username,
                password: password,
                getMetadata: function () {
                    return {
                        boundParameter: null,
                        operationType: 0,
                        operationName: "edp_IsCompanyStakeholder",
                        parameterTypes: {
                            "orgNo": {
                                "typeName": "Edm.String",
                                "structuralProperty": 1
                            },
                            "username": {
                                "typeName": "Edm.String",
                                "structuralProperty": 1
                            },
                            "password": {
                                "typeName": "Edm.String",
                                "structuralProperty": 1
                            }
                        }
                    }
                }
            };
            return Xrm.WebApi.online.execute(request).then(
                function (result) {
                    if (result.ok) {
                        return result.json();
                    }
                },
                function (error) {
                    alert("Error in IsCompanyStakeHolder: " + error.message);
                })
                .then(function (response) {
                    return response && response.stakeholderCompany ? response.stakeholderCompany : false;
                },
                    function (error) {
                        alert("Error in IsCompanyStakeHolder: " + error.message);
                    }
                );
        },

        fetchConfigCredentials: function () {
            var request = {
                getMetadata: function () {
                    return {
                        boundParameter: null,
                        operationType: 0,
                        operationName: "edp_GetCredentials"
                    };
                }
            };
            return Xrm.WebApi.online.execute(request).then(
                function (result) {
                    if (result.ok) {
                        return result.json();
                    }
                },
                function (error) {
                    alert("Error in " + error.message);
                })

                .then(function (res) {
                    return res.configcredentials;

                },
                    function (error) {
                        alert("Error in " + error.message);
                    });
        },

        fetchAndUpdateCasCompany: function (formContext, orgNumber, userName, passWord, tmplte) {
            var request = {
                template: tmplte,
                orgNo: orgNumber,
                username: userName,
                password: passWord,
                getMetadata: function () {
                    return {
                        boundParameter: null,
                        parameterTypes: {
                            "template": {
                                "typeName": "Edm.String",
                                "structuralProperty": 1 // Primitive Type
                            },
                            "orgNo": {
                                "typeName": "Edm.String",
                                "structuralProperty": 1 // Primitive Type
                            },
                            "username": {
                                "typeName": "Edm.String",
                                "structuralProperty": 1 // Primitive Type
                            },
                            "password": {
                                "typeName": "Edm.String",
                                "structuralProperty": 1 // Primitive Type
                            }
                        },
                        operationType: 0,
                        operationName: "edp_CasCompany"
                    };
                }
            };

            Xrm.WebApi.online.execute(request).then(
                function (response) {
                    if (response.ok) {
                        response.json().then(
                            function (result) {
                                var casResult = result && result.casCompanyRes ? result.casCompanyRes : null;
                                if (casResult != null && casResult != '') {
                                    Endeavor.Creditsafe.CreditsafeAccount.updateFromResponse(formContext, casResult);
                                } else {
                                    console.log("error in cascompany: casresult is null or empty");
                                }
                            });
                    }


                },
                function (error) {
                    console.log("Error in casCompany: " + error.message);
                }
            );

        },

        updateCreditRating: function (formContext) {
            try {
                var orgNumber = "";

                if (formContext.getAttribute("edp_orgno") != null)
                    orgNumber = formContext.getAttribute("edp_orgno").getValue();
                else
                    Xrm.Navigation.openAlertDialog({ text: "The organization number is missing." });

                if (orgNumber != null && orgNumber != "") {

                    Endeavor.Creditsafe.CreditsafeAccount.fetchConfigCredentials().then(
                        function (credentials) {
                            if (credentials === null || credentials.configcredentials === null) {
                                Xrm.Navigation.openAlertDialog({ text: "Was not able to retrieve credentials." });
                                throw new Error("The request did not return anything.");
                            } else {
                                //split credentials into samller parts
                                var data = credentials.split(';');
                                var userName = data[1];
                                var password = data[2];

                                //fetch creditsafe configs
                                var url = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "edp_CreditsafeConfigurationSet?$select=edp_CreditWorthinessTemplate&$filter=edp_SearchEngine/Value eq 757550000";
                                var resultSet = Endeavor.Common.Data.fetchJSONResults(url);

                                if (resultSet[0] != null) {
                                    var result = resultSet[0];
                                    if (userName != null && userName != "" && password != null && password != "" && result.edp_CreditWorthinessTemplate != null && result.edp_CreditWorthinessTemplate != "") {

                                        Endeavor.Creditsafe.CreditsafeAccount.isCompanyStakeHolder(orgNumber, userName, password).then(
                                            function (isStakeholderCompany) {

                                                if (isStakeholderCompany) {
                                                    Endeavor.Creditsafe.CreditsafeAccount.fetchAndUpdateCasCompany(formContext, orgNumber, userName, password, result.edp_CreditWorthinessTemplate);
                                                } else {
                                                    Xrm.Navigation.openAlertDialog({ text: "This operation is only allowed for AB" });
                                                    // TODO: teo - Update the Form if needed with the information that this isn't a AB
                                                    return;
                                                }
                                            }
                                        );
                                    } else if (userName == null || userName == "")
                                        Xrm.Navigation.openAlertDialog({ text: "Systemparameter is missing information in the 'Username'-field." });
                                    else if (password == null || password == "")
                                        Xrm.Navigation.openAlertDialog({ text: "Systemparameter is missing information in the 'Password'-field." });
                                    else if (result.edp_CreditWorthinessTemplate == null || result.edp_CreditWorthinessTemplate == "")
                                        Xrm.Navigation.openAlertDialog({ text: "Systemparameter is missing information in the 'Credit Worthiness Template'-field." });
                                } else {
                                    Xrm.Navigation.openAlertDialog({ text: "SystemParameter is missing. Please configure." });
                                }
                            }

                        }
                    );
                } else {
                    Xrm.Navigation.openAlertDialog({ text: "The organization number is missing." });
                }
            } catch (e) {
                Xrm.Navigation.openAlertDialog({ text: e.message });
            }
        },

        updateAccountRecord: function (id, data) {

            return Xrm.WebApi.online.updateRecord("account", id, data).then(
                function (response) {
                    console.log("successfully updated the account record");
                },
                function (error) {
                    throw new Error(error.message);
                }
            );
        },

        updateFromResponse: function (formContext, casResponse) {
            try {
                var creditTrating = null;
                var accountid = formContext.data.entity.getId();
                var entityName = formContext.data.entity.getEntityName();

                switch (casResponse) {
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
                        Xrm.Navigation.openAlertDialog({ text: "Unrecognised responsecode." });
                        break;
                }

                // Update Account with creditrating and lastcreditreport(date)
                if (creditTrating != null) {
                    var currentDateTime = new Date();

                    var accEntity = {
                        "edp_lastcreditreport": currentDateTime,
                        "edp_creditrating": creditTrating
                    };
                    debugger;
                    Endeavor.Creditsafe.CreditsafeAccount.updateAccountRecord(accountid, accEntity).then(
                        function () {
                            switch (casResponse) {
                                case "1":
                                    formContext.ui.setFormNotification("Credit rating is 'Approved'. Please refresh page (F5).", "INFORMATION");
                                    break;
                                case "2":
                                    formContext.ui.setFormNotification("Credit rating is 'Not Approved'. Please refresh page (F5).", "ERROR");
                                    break;
                                case "4":
                                    formContext.ui.setFormNotification("Credit rating is 'Manual Assessment needed'. Please refresh page (F5).", "WARNING");
                            }
                        },
                        function (error) {
                            Xrm.Navigation.openAlertDialog({ text: "Something went wrong when updating the account record" });
                        }
                    );

                } else {
                    Xrm.Navigation.openAlertDialog({ text: "Credit worthiness according to credit template not retrieved correctly." });
                    return;
                }
            } catch (exc) {
                Xrm.Navigation.openAlertDialog({ text: exc });
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
            var globalContext = Xrm.Utility.getGlobalContext();
            var resourceFile = globalContext.getClientUrl() + "/WebResources/edp_/script/Endeavor.Creditsafe.Resources.1033.js";

            // Always load English
            $.cachedScript(resourceFile).done(function (script, textStatus) {
                // Resource loaded.
            }).fail(function (jqxhr, settings, exception) {
                alert("Resources failed to load resources from \"" + resourceFile + "\". Exception: " + exception);
            });

            var lcid = globalContext.userSettings.languageId;
            switch (lcid) {
                case 1053:  // Swedish
                    resourceFile = globalContext.getClientUrl() + "/WebResources/edp_/script/Endeavor.Creditsafe.Resources.1053.js";
                    break;
                case 1031:  // German
                    resourceFile = globalContext.getClientUrl() + "/WebResources/edp_/script/Endeavor.Creditsafe.Resources.1031.js";
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