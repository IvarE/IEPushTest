if (typeof (CGISweden) == "undefined")
{ CGISweden = {}; }

CGISweden.odata =
{

    // *** Start Queries used for entity Case
    IsTravelInformationRequired: function (cgi_categorydetailId) {

        try {
            //This is done synchronously because it is part of a validation process
            var serverUrl;
            if (Xrm.Page.context.getClientUrl !== undefined) {
                serverUrl = Xrm.Page.context.getClientUrl();
            } else {
                serverUrl = Xrm.Page.context.getServerUrl();
            }
            var ODataPath = serverUrl + "/XRMServices/2011/OrganizationData.svc";

            var _options = "$select=cgi_requirestravelinfo&";
            var _filter = "$filter=cgi_categorydetailId eq guid'" + cgi_categorydetailId + "'";
            var _odata = _options + _filter;

            var cgi_categorydetailQueryUrl = ODataPath + "/cgi_categorydetailSet?" + _odata;

            var ODataRequest = new XMLHttpRequest();
            ODataRequest.open("GET", cgi_categorydetailQueryUrl, false); // false = synchronous request
            ODataRequest.setRequestHeader("Accept", "application/json");
            ODataRequest.setRequestHeader("Content-Type", "application/json; charset=utf-8");
            ODataRequest.send();

            if (ODataRequest.status === 200) {
                var parsedResults = JSON.parse(ODataRequest.responseText).d;
                if (parsedResults != null && parsedResults.results != null && parsedResults.results.length > 0) {
                    var cgi_categorydetail = parsedResults.results[0];
                    if (cgi_categorydetail["cgi_requirestravelinfo"] == 1)
                        return true;
                    else
                        return false;
                }
                else {
                    return false;
                }
            }
            else {
                return false;
            }
        }
        catch (e) {
            alert("Fel i CGISweden.odata.IsTravelInformationRequired\n\n" + e.Message);
        }
    },
    GetTravelInfoForCase: function (caseid, formContext) {
        try {
            var globalContext = Xrm.Utility.getGlobalContext();
            //Return the number of travelinformation posts registered on this case
            //This is done synchronously because it is part of a validation process
            var serverUrl;
            if (globalContext.getClientUrl !== undefined) {
                serverUrl = globalContext.getClientUrl();
            } else {
                serverUrl = globalContext.getServerUrl();
            }
            var ODataPath = serverUrl + "/XRMServices/2011/OrganizationData.svc";

            var _options = "$select=cgi_travelinformationId&";
            var _filter = "$filter=cgi_Caseid/Id eq guid'" + caseid + "'";
            var _odata = _options + _filter;

            var cgi_travelinformationQueryUrl = ODataPath + "/cgi_travelinformationSet?" + _odata;

            var ODataRequest = new XMLHttpRequest();
            ODataRequest.open("GET", cgi_travelinformationQueryUrl, false); // false = synchronous request
            ODataRequest.setRequestHeader("Accept", "application/json");
            ODataRequest.setRequestHeader("Content-Type", "application/json; charset=utf-8");
            ODataRequest.send();

            if (ODataRequest.status === 200) {
                var parsedResults = JSON.parse(ODataRequest.responseText).d;
                if (parsedResults != null && parsedResults.results != null) {
                    return parsedResults.results.length;

                }
                else {
                    return -2;
                }
            }
            else {
                return -1;
            }
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetTravelInfoForCase\n\n" + e.Message);
        }
    },

    GetCategoriesForCase: function (caseid, CallBackFunction, CompleteFunction) {
        try {
            var _options = "$select=cgi_casecategoryname&";
            var _filter = "$filter=statecode/Value eq 0 and cgi_Caseid/Id eq guid'" + caseid + "'&";
            var _sort = "$orderby=cgi_casecategoryname asc";
            var _odata = _options + _filter + _sort;
            SDK.REST.retrieveMultipleRecords("cgi_casecategory", _odata, CallBackFunction, function (error) { alert("SDK.REST.retrieveMultipleRecords.GetCategoriesForCase\n\n" + error.message); }, CompleteFunction);
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetCategoriesForCase\n\n" + e.Message);
        }
    },

    GetDefaultCustomerFromSetting: function (nowdate, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("incident", "?$select=cgi_defaultcustomeroncase&$filter=statecode eq 0 and cgi_validfrom le datetime'" + nowdate + "' and (cgi_validto ge datetime'" + nowdate + "' or cgi_validto eq null)").then(
                function success(result) {
                    CGISweden.incident.setCustomerOnLoad_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    // handle error conditions
                }
            );
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetDefaultCustomerFromSetting\n\n" + e.Message);
        }
    },

    GetDefaultCaseCategory3Setting: function (nowdate, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("incident", "?$select=cgi_category_detail3id&$filter=statecode eq 0 and cgi_validfrom le datetime'" + nowdate + "' and (cgi_validto ge datetime'" + nowdate + "' or cgi_validto eq null)").then(
                function success(result) {
                    CGISweden.incident.casetypecode_onchange_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    // handle error conditions
                }
            );
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetDefaultCaseCategory3Setting\n\n" + e.Message);
        }
        
    },

    Getcgi_refundtypeproductnotrequiredidSetting: function (nowdate, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("cgi_refund", "?$select=cgi_refundtypeproductnotrequiredid&$filter=statecode eq 0 and cgi_validfrom le datetime'" + nowdate + "' and (cgi_validto ge datetime'" + nowdate + "' or cgi_validto eq null)").then(
                function success(result) {
                    CGISweden.refund.Getcgi_refundtypeproductnotrequiredidSetting_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    // handle error conditions
                }
            );
        }
        catch (e) {
            alert("Fel i CGISweden.odata.Getcgi_refundtypeproductnotrequiredidSetting\n\n" + e.Message);
        }
    },
    GetBOMBUrlFromSetting: function (nowdate, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("incident", "?$select=cgi_bomburl&$filter=statecode eq 0 and cgi_validfrom le datetime'" + nowdate + "' and (cgi_validto ge datetime'" + nowdate + "' or cgi_validto eq null)").then(
                function success(result) {
                    CGISweden.incident.Ribbon.openBombApp_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    // handle error conditions
                }
            );
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetBOMBUrlFromSetting\n\n" + e.Message);
        }
    },

    GetRGOLUrlFromSetting: function (nowdate, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("incident", "?$select=cgi_rgolurl&$filter=statecode eq 0 and cgi_validfrom le datetime'" + nowdate + "' and (cgi_validto ge datetime'" + nowdate + "' or cgi_validto eq null)").then(
                function success(result) {
                    CGISweden.incident.getRGOLapiurl_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    // handle error conditions
                }
            );
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetRGOLUrlFromSetting\n\n" + e.Message);
        }
    },

    GetRGOLUrlFromSettingNew: function (nowdate, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("incident", "?$select=cgi_rgolurl&$filter=statecode eq 0 and cgi_validfrom le datetime'" + nowdate + "' and (cgi_validto ge datetime'" + nowdate + "' or cgi_validto eq null)").then(
                function success(result) {
                    CGISweden.incident.getRGOLapiurl_callbackNew(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    // handle error conditions
                }
            );
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetRGOLUrlFromSetting\n\n" + e.Message);
        }
    },

    //Returns category guid for level 1 and level 2
    GetParentCategory: function (_categoryid, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("incident", "?$select=cgi_parentid,cgi_parentid2&$filter=cgi_categorydetailid eq" + _categoryid).then(
                function success(result) {
                    CGISweden.incident.category2_onchange_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    // handle error conditions
                }
            );
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetParentCategory\n\n" + e.Message);
        }
    },

    //Returns category guid for level 1 and level 2
    GetParentCategory3: function (_categoryid, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("incident", "?$select=cgi_parentid,cgi_parentid2&$filter=cgi_categorydetailid eq" + _categoryid).then(
                function success(result) {
                    CGISweden.incident.category3_onchange_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    // handle error conditions
                }
            );
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetParentCategory\n\n" + e.Message);
        }
    },

    //Get default queue from user
    GetDefaultQueue: function (userId, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("email", "?$select=queueid&$filter=systemuserid eq" + userId).then(
                function success(result) {
                    CGISweden.email.SetSenderEmail_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    // handle error conditions
                }
            );
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetDefaultQueue\n\n" + e.Message);
        }
    },

    //Get letter template
    GetLetterTemplate: function (_id, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("incident", "?$select=cgi_title,cgi_template_body&$filter=cgi_letter_templateid eq" + _id).then(
                function success(result) {
                    CGISweden.incident.letter_template_onchange_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    // handle error conditions
                }
            );
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetLetterTemplate\n\n" + e.Message);
        }
    },
    // *** End Queries used for entity Case

    // *** Start Queries used for entity account

    GetSecRolesNameAccount: function (userRoleId, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("account", "?$select=name&$filter=roleid eq" + userRoleId).then(
                function success(result) {
                    CGISweden.account.checkIfUserHasRole_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    // handle error conditions
                }
            );
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetSecRolesNameAccount\n\n" + e.Message);
        }
    },

    // *** End Queries used for entity account

    // *** Start Queries used for entity cgi_refund



    GetAmountLimitFromSetting: function (nowdate, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("cgi_refund", "?$select=cgi_amount_limit, cgi_amount_limit_warn&$filter=statecode eq 0 and cgi_validfrom le datetime'" + nowdate + "' and (cgi_validto ge datetime'" + nowdate + "' or cgi_validto eq null)").then(
                function success(result) {
                    CGISweden.refund.ctrlAmountLimit_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    // handle error conditions
                }
            );
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetAmountLimitFromSetting\n\n" + e.Message);
        }

        
    },

    GetRefundSetting: function (refundid, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("cgi_refund", "?select=cgi_refundoption,cgi_financialtransaction,cgi_refundtypeid,cgi_refundtypename,cgi_refundaccountid,cgi_refundresponsibleid,cgi_refundproductid,cgi_reinvoice&$filter=statecode eq 0 and cgi_refundtypeid eq " + refundid).then(
                function success(result) {
                    CGISweden.refund.refundtypeid_OnChange_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    // handle error conditions
                }
            );

        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetRefundSetting\n\n" + e.Message);
        }
    },

    GetRefundSettingOnLoad: function (refundid, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("cgi_refund", "?select=cgi_refundoption,cgi_financialtransaction,cgi_refundtypeid,cgi_refundtypename,cgi_refundaccountid,cgi_refundresponsibleid,cgi_refundproductid,cgi_reinvoice&$filter=statecode eq 0 and cgi_refundtypeid eq " + refundid).then(
                function success(result) {
                    CGISweden.refund.refundtypeid_Onload_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    // handle error conditions
                }
            );

        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetRefundSettingOnLoad\n\n" + e.Message);
        }
    },

    Getcgi_refundaccountNumber: function (cgi_refundaccountid, formContext) {
        try {   
            Xrm.WebApi.retrieveMultipleRecords("cgi_refund", "?$select=cgi_account,cgi_refundaccountid&$filter=cgi_refundaccountid eq" + cgi_refundaccountid).then(
                function success(result) {
                    CGISweden.refund.Getcgi_refundaccountNumber_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    // handle error conditions
                }
            );
        }
        catch (e) {
            alert("Fel i CGISweden.odata.Getcgi_refundaccountNumber\n\n" + e.Message);
        }
    },

    GetDefaultMilageContributionFromSetting: function (nowdate, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("cgi_refund", "?$select=cgi_milage_contribution&$filter=statecode eq 0 and cgi_validfrom le datetime'" + nowdate + "' and (cgi_validto ge datetime'" + nowdate + "' or cgi_validto eq null)").then(
                function success(result) {
                    CGISweden.refund.setMilageContributionOnLoad_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    // handle error conditions
                }
            );
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetDefaultCustomerFromSetting\n\n" + e.Message);
        }
    },
    //MaxP 2016-04-20 ytterligare en kolumner som returneras i svaret. Ursprung, ärendetyp och flagga för manuella ärenden
    GetCaseNumber: function (caseid, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("cgi_refund", "?$select=ticketnumber,cgi_unregisterdtravelcard,caseorigincode,casetypecode,cgi_iscompleted,cgi_milagekilometers,cgi_milagelicenseplatenumber,cgi_milagefrom,cgi_milageto,cgi_taxifrom,cgi_taxito,cgi_taxiclaimedamount,cgi_compensationclaimfromrgol,cgi_refundtypes,cgi_refundreimbursementform&$filter=incidentid eq" + caseid).then(
                function success(result) {
                    CGISweden.refund.getCaseNumber_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    // handle error conditions
                }
            );
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetCaseNumber\n\n" + e.Message);
        }
    },
    //MaxP 2016-05-09 Ytterligare kolulmn cgi_emailaddress för att sätta leverans epostadress på beslut
    //MaxP 2016-05-11 Hämtar nytt fält (cgi_rgol_delivery_email) från ärende för att sätta leverans epostadress på beslut
    GetEmailAddress: function (caseid, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("cgi_refund", "?$select=cgi_customer_email,cgi_emailaddress,cgi_rgol_delivery_email,cgi_iscompleted&$filter=incidentid eq" + caseid).then(
                function success(result) {
                    CGISweden.refund.getGetEmailAddress_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    // handle error conditions
                }
            );

        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetEmailAddress\n\n" + e.Message);
        }

        
    },

    GetTravelCardNumber: function (travelcardid, CallBackFunction, CompleteFunction) {
        try {

            //$select=cgi_travelcardnumber&
            //$filter=cgi_travelcardId eq guid'3704AE4A-1D64-E411-80D3-0050569010AD'

            var _options = "$select=cgi_travelcardnumber&";
            var _filter = "$filter=cgi_travelcardId eq guid'" + travelcardid + "'";
            var _odata = _options + _filter;
            SDK.REST.retrieveMultipleRecords("cgi_travelcard", _odata, CallBackFunction, function (error) { alert("SDK.REST.retrieveMultipleRecords.GetTravelCardNumber\n\n" + error.message); }, CompleteFunction);
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetTravelCardNumber\n\n" + e.Message);
        }
    },

    //
    GetReimbursementForm: function (reimbursementid, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("cgi_refund", "?$select=cgi_reimbursementformid,cgi_reimbursementname,cgi_reinvoicing,cgi_useaccount,cgi_useresponsible,cgi_useproduct,cgi_loadcard,cgi_payment,cgi_payment_abroad,cgi_time_valid,cgi_attestation,cgi_giftcard,cgi_sendtostralfors,cgi_couponsms&$filter=statecode eq 0 and cgi_reimbursementformid eq" + reimbursementid).then(
                function success(result) {
                    CGISweden.refund.reimbursementformid_OnChange_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    // handle error conditions
                }
            );
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetReimbursementForm\n\n" + e.Message);
        }
    },

    GetReimbursementFormOnLoad: function (reimbursementid, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("cgi_refund", "?$select=cgi_reimbursementformid,cgi_reimbursementname,cgi_reinvoicing,cgi_useaccount,cgi_useresponsible,cgi_useproduct,cgi_loadcard,cgi_payment,cgi_payment_abroad,cgi_time_valid,cgi_attestation,cgi_giftcard,cgi_sendtostralfors,cgi_couponsms&$filter=statecode eq 0 and cgi_reimbursementformid eq" + reimbursementid).then(
                function success(result) {
                    CGISweden.refund.reimbursementformid_OnLoad_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    // handle error conditions
                }
            );
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetReimbursementFormOnLoad\n\n" + e.Message);
        }
    },

    //Get bic and iban from incident. MaxP 2016-04-20
    GetBicIban: function (caseid, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("cgi_refund", "?$select=cgi_bic,cgi_iban,cgi_iscompleted&$filter=incidentid eq" + caseid).then(
                function success(result) {
                    CGISweden.refund.setBicIban_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    // handle error conditions
                }
            );

        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetBicIban\n\n" + e.Message);
        }
    },

    //Get social sec number from incident.
    GetSocSecNumber: function (caseid, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("cgi_refund", "?$select=cgi_soc_sec_number,cgi_rgol_socialsecuritynumber,cgi_iscompleted&$filter=incidentid eq" + caseid).then(
                function success(result) {
                    CGISweden.refund.setSocSecOnLoad_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    // handle error conditions
                }
            );
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetGetSocSecNumber\n\n" + e.Message);
        }

        
    },

    //Get mobilenumber from incident.
    GetMobileNumber: function (caseid, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("cgi_refund", "?$select=cgi_customer_telephonenumber,cgi_rgol_telephonenumber,cgi_iscompleted&$filter=incidentid eq" + caseid).then(
                function success(result) {
                    CGISweden.refund.setMobileNumber_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    // handle error conditions
                }
            );
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetMobileNumber\n\n" + e.Message);
        }
    },

    //Get cgi_contact and cgi_account from incident.
    GetContactAccount: function (caseid, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("cgi_refund", "?$select=cgi_contactid,cgi_accountid&$filter=incidentid eq" + caseid).then(
                function success(result) {
                    CGISweden.refund.GetContactAccount_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    // handle error conditions
                }
            );
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetContactAccount\n\n" + e.Message);
        }
    },

    //Get RSID from user
    GetRSID: function (userId, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("cgi_refund", "?$select=cgi_rsid&$filter=systemuserid eq" + userId).then(
                function success(result) {
                    CGISweden.refund.ctrlAttest_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    // handle error conditions
                }
            );
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetRSIDe\n\n" + e.Message);
        }
    },

    GetSecRolesNameRefund: function (userRoleId, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("cgi_refund", "?$select=name&$filter=roleid eq" + userRoleId).then(
                function success(result) {
                    CGISweden.refund.CheckUserRoleOnchange_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    // handle error conditions
                }
            );
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetSecRolesName\n\n" + e.Message);
        }
        
    },

    // *** End Queries used for entity cgi_refund

    // *** Start Queries used for entity email

    GetEntityNoFromSetting: function (CallBackFunction, CompleteFunction) {
        try {
            var _options = "$select=cgi_recipient_entity_id&";
            var _filter = "$filter=statecode/Value eq 0";
            var _odata = _options + _filter;
            SDK.REST.retrieveMultipleRecords("cgi_setting", _odata, CallBackFunction, function (error) { alert("SDK.REST.retrieveMultipleRecords.GetEntityNoFromSetting\n\n" + error.message); }, CompleteFunction);
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetEntityNoFromSetting\n\n" + e.Message);
        }
    },

    GetDefaultUserFromSetting: function (nowdate, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("email", "?$select=cgi_userid&$filter=statecode eq 0 and cgi_validfrom le datetime'" + nowdate + "' and (cgi_validto ge datetime'" + nowdate + "' or cgi_validto eq null)").then(
                function success(result) {
                    CGISweden.email.SetSenderEmailNoReply_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    // handle error conditions
                }
            );
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetDefaultCustomerFromSetting\n\n" + e.Message);
        }
    },




    // *** End Queries used for entity email

    // *** Start Queries used for contact

    GetSecRolesNameContact: function (userRoleId, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("contact", "?$select=name&$filter=roleid eq" + userRoleId).then(
                function success(result) {
                    CGISweden.contact.checkIfUserHasRole_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    // handle error conditions
                }
            );
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetSecRolesNameContact\n\n" + e.Message);
        }
    },

    // *** End Queries used for contact

    // *** Start Queries for entity filelinks

    GetFilelinks: function (incidentId, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("email", "?$select=cgi_url&$filter=cgi_incidentid eq" + incidentId).then(
                function success(result) {
                    CGISweden.email.set_filelinks_onchange_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    // handle error conditions
                }
            );
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetFilelinks\n\n" + e.Message);
        }
    },

    // *** End Queries for entity filelinks

    // *** Start Queries for entity article

    GetSecRolesNameArticle: function (userRoleId, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("kbarticle", "?$select=name&$filter=roleid eq" + userRoleId).then(
                function success(result) {
                    CGISweden.article.checkIfUserHasMarketingRole_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    // handle error conditions
                }
            );
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetSecRolesNameArticle\n\n" + e.Message);
        }
        
    }

    // *** End Queries for entity article

};

