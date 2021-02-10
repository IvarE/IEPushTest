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
    GetTravelInfoForCase: function (caseid) {
        try {
            //Return the number of travelinformation posts registered on this case
            //This is done synchronously because it is part of a validation process
            var serverUrl;
            if (Xrm.Page.context.getClientUrl !== undefined) {
                serverUrl = Xrm.Page.context.getClientUrl();
            } else {
                serverUrl = Xrm.Page.context.getServerUrl();
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

    GetDefaultCustomerFromSetting: function (nowdate, CallBackFunction, CompleteFunction) {
        try {
            var _options = "$select=cgi_DefaultCustomerOnCase&";
            var _filter = "$filter=statecode/Value eq 0 and cgi_ValidFrom le datetime'" + nowdate + "' and (cgi_ValidTo ge datetime'" + nowdate + "' or cgi_ValidTo eq null)";
            var _odata = _options + _filter;
            SDK.REST.retrieveMultipleRecords("cgi_setting", _odata, CallBackFunction, function (error) { alert("SDK.REST.retrieveMultipleRecords.GetDefaultCustomerFromSetting\n\n" + error.message); }, CompleteFunction);
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetDefaultCustomerFromSetting\n\n" + e.Message);
        }
    },

    GetDefaultCaseCategory3Setting: function (nowdate, CallBackFunction, CompleteFunction) {
        try {
            var _options = "$select=cgi_category_detail3id&";
            var _filter = "$filter=statecode/Value eq 0 and cgi_ValidFrom le datetime'" + nowdate + "' and (cgi_ValidTo ge datetime'" + nowdate + "' or cgi_ValidTo eq null)";
            var _odata = _options + _filter;
            SDK.REST.retrieveMultipleRecords("cgi_setting", _odata, CallBackFunction, function (error) { alert("SDK.REST.retrieveMultipleRecords.GetDefaultCaseCategory3Setting\n\n" + error.message); }, CompleteFunction);
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetDefaultCaseCategory3Setting\n\n" + e.Message);
        }
    },

    GetBOMBUrlFromSetting: function (nowdate, CallBackFunction, CompleteFunction) {
        try {
            var _options = "$select=cgi_BOMBUrl&";
            var _filter = "$filter=statecode/Value eq 0 and cgi_ValidFrom le datetime'" + nowdate + "' and (cgi_ValidTo ge datetime'" + nowdate + "' or cgi_ValidTo eq null)";
            var _odata = _options + _filter;
            SDK.REST.retrieveMultipleRecords("cgi_setting", _odata, CallBackFunction, function (error) { alert("SDK.REST.retrieveMultipleRecords.GetBOMBUrlFromSetting\n\n" + error.message); }, CompleteFunction);
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetBOMBUrlFromSetting\n\n" + e.Message);
        }
    },

    //Returns category guid for level 1 and level 2
    GetParentCategory: function (_categoryid, CallBackFunction, CompleteFunction) {
        try {
            var _options = "$select=cgi_Parentid,cgi_parentid2&";
            var _filter = "$filter=cgi_categorydetailId eq guid'" + _categoryid + "'";
            var _odata = _options + _filter;
            SDK.REST.retrieveMultipleRecords("cgi_categorydetail", _odata, CallBackFunction, function (error) { alert("SDK.REST.retrieveMultipleRecords.GetParentCategory\n\n" + error.message); }, CompleteFunction);
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetParentCategory\n\n" + e.Message);
        }
    },

    //Get default queue from user
    GetDefaultQueue: function (userId, CallBackFunction, CompleteFunction) {
        try {
            var _options = "$select=QueueId&";
            var _filter = "$filter=SystemUserId eq guid'" + userId + "'";
            var _odata = _options + _filter;
            SDK.REST.retrieveMultipleRecords("SystemUser", _odata, CallBackFunction, function (error) { alert("SDK.REST.retrieveMultipleRecords.GetDefaultQueue\n\n" + error.message); }, CompleteFunction);
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetDefaultQueue\n\n" + e.Message);
        }
    },

    //Get letter template
    GetLetterTemplate: function (_id, CallBackFunction, CompleteFunction) {
        try {
            var _options = "$select=cgi_title,cgi_template_body&";
            var _filter = "$filter=cgi_letter_templateId eq guid'" + _id + "'";
            var _odata = _options + _filter;
            SDK.REST.retrieveMultipleRecords("cgi_letter_template", _odata, CallBackFunction, function (error) { alert("SDK.REST.retrieveMultipleRecords.GetLetterTemplate\n\n" + error.message); }, CompleteFunction);
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetLetterTemplate\n\n" + e.Message);
        }
    },
    // *** End Queries used for entity Case

    // *** Start Queries used for entity cgi_refund

    GetSecRolesName: function (userRoleId, CallBackFunction, CompleteFunction) {
        try {
            var _options = "$select=Name&";
            var _filter = "$filter=RoleId eq guid'" + userRoleId + "'";
            var _odata = _options + _filter;
            SDK.REST.retrieveMultipleRecords("Role", _odata, CallBackFunction, function (error) { alert("SDK.REST.retrieveMultipleRecords.GetSecRolesName\n\n" + error.message); }, CompleteFunction);
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetSecRolesName\n\n" + e.Message);
        }
    },

    GetAmountLimitFromSetting: function (nowdate, CallBackFunction, CompleteFunction) {
        try {
            var _options = "$select=cgi_amount_limit, cgi_amount_limit_warn&";
            var _filter = "$filter=statecode/Value eq 0 and cgi_ValidFrom le datetime'" + nowdate + "' and (cgi_ValidTo ge datetime'" + nowdate + "' or cgi_ValidTo eq null)";
            var _odata = _options + _filter;
            SDK.REST.retrieveMultipleRecords("cgi_setting", _odata, CallBackFunction, function (error) { alert("SDK.REST.retrieveMultipleRecords.GetAmountLimitFromSetting\n\n" + error.message); }, CompleteFunction);
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetAmountLimitFromSetting\n\n" + e.Message);
        }
    },

    GetRefundSetting: function (refundid, CallBackFunction, CompleteFunction) {
        try {
            var _options = "$select=cgi_RefundOption,cgi_FinancialTransaction,cgi_refundtypeId,cgi_refundtypename,cgi_refundaccountid,cgi_refundresponsibleId,cgi_refundproductid,cgi_reinvoice&";
            var _filter = "$filter=statecode/Value eq 0 and cgi_refundtypeId eq guid'" + refundid + "'";
            var _odata = _options + _filter;
            SDK.REST.retrieveMultipleRecords("cgi_refundtype", _odata, CallBackFunction, function (error) { alert("SDK.REST.retrieveMultipleRecords.GetRefundSetting\n\n" + error.message); }, CompleteFunction);
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetRefundSetting\n\n" + e.Message);
        }
    },

    GetDefaultMilageContributionFromSetting: function (nowdate, CallBackFunction, CompleteFunction) {
        try {
            var _options = "$select=cgi_milage_contribution&";
            var _filter = "$filter=statecode/Value eq 0 and cgi_ValidFrom le datetime'" + nowdate + "' and (cgi_ValidTo ge datetime'" + nowdate + "' or cgi_ValidTo eq null)";
            var _odata = _options + _filter;
            SDK.REST.retrieveMultipleRecords("cgi_setting", _odata, CallBackFunction, function (error) { alert("SDK.REST.retrieveMultipleRecords.GetDefaultCustomerFromSetting\n\n" + error.message); }, CompleteFunction);
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetDefaultCustomerFromSetting\n\n" + e.Message);
        }
    },

    GetCaseNumber: function (caseid, CallBackFunction, CompleteFunction) {
        try {
            var _options = "$select=TicketNumber,cgi_UnregisterdTravelCard&";
            var _filter = "$filter=IncidentId eq guid'" + caseid + "'";
            var _odata = _options + _filter;
            SDK.REST.retrieveMultipleRecords("Incident", _odata, CallBackFunction, function (error) { alert("SDK.REST.retrieveMultipleRecords.GetCaseNumber\n\n" + error.message); }, CompleteFunction);
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetCaseNumber\n\n" + e.Message);
        }
    },

    GetEmailAddress: function (caseid, CallBackFunction, CompleteFunction) {
        try {
            var _options = "$select=cgi_customer_email&";
            var _filter = "$filter=IncidentId eq guid'" + caseid + "'";
            var _odata = _options + _filter;
            SDK.REST.retrieveMultipleRecords("Incident", _odata, CallBackFunction, function (error) { alert("SDK.REST.retrieveMultipleRecords.GetEmailAddress\n\n" + error.message); }, CompleteFunction);
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
    GetReimbursementForm: function (reimbursementid, CallBackFunction, CompleteFunction) {
        try {
            var _options = "$select=cgi_reimbursementformId,cgi_reimbursementname,cgi_ReInvoicing,cgi_UseAccount,cgi_useresponsible,cgi_UseProduct,cgi_loadcard,cgi_payment,cgi_payment_abroad,cgi_time_valid,cgi_attestation,cgi_giftcard,cgi_sendtostralfors,cgi_couponsms&";
            var _filter = "$filter=statecode/Value eq 0 and cgi_reimbursementformId eq guid'" + reimbursementid + "'";
            var _odata = _options + _filter;
            SDK.REST.retrieveMultipleRecords("cgi_reimbursementform", _odata, CallBackFunction, function (error) { alert("SDK.REST.retrieveMultipleRecords.GetReimbursementForm\n\n" + error.message); }, CompleteFunction);
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetReimbursementForm\n\n" + e.Message);
        }
    },

    //Get social sec number from incident.
    GetSocSecNumber: function (caseid, CallBackFunction, CompleteFunction) {
        try {
            var _options = "$select=cgi_soc_sec_number&";
            var _filter = "$filter=IncidentId eq guid'" + caseid + "'";
            var _odata = _options + _filter;
            SDK.REST.retrieveMultipleRecords("Incident", _odata, CallBackFunction, function (error) { alert("SDK.REST.retrieveMultipleRecords.GetCaseNumber\n\n" + error.message); }, CompleteFunction);
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetGetSocSecNumber\n\n" + e.Message);
        }
    },

    //Get mobilenumber from incident.
    GetMobileNumber: function (caseid, CallBackFunction, CompleteFunction) {
        try {
            var _options = "$select=cgi_customer_telephonenumber&";
            var _filter = "$filter=IncidentId eq guid'" + caseid + "'";
            var _odata = _options + _filter;
            SDK.REST.retrieveMultipleRecords("Incident", _odata, CallBackFunction, function (error) { alert("SDK.REST.retrieveMultipleRecords.GetMobileNumber\n\n" + error.message); }, CompleteFunction);
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetMobileNumber\n\n" + e.Message);
        }
    },

    //Get cgi_contact and cgi_account from incident.
    GetContactAccount: function (caseid, CallBackFunction, CompleteFunction) {
        try {
            var _options = "$select=cgi_Contactid,cgi_Accountid&";
            var _filter = "$filter=IncidentId eq guid'" + caseid + "'";
            var _odata = _options + _filter;
            SDK.REST.retrieveMultipleRecords("Incident", _odata, CallBackFunction, function (error) { alert("SDK.REST.retrieveMultipleRecords.GetContactAccount\n\n" + error.message); }, CompleteFunction);
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetContactAccount\n\n" + e.Message);
        }
    },

    //Get RSID from user
    GetRSID: function (userId, CallBackFunction, CompleteFunction) {
        try {
            var _options = "$select=cgi_RSID&";
            var _filter = "$filter=SystemUserId eq guid'" + userId + "'";
            var _odata = _options + _filter;
            SDK.REST.retrieveMultipleRecords("SystemUser", _odata, CallBackFunction, function (error) { alert("SDK.REST.retrieveMultipleRecords.GetRSID\n\n" + error.message); }, CompleteFunction);
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetRSIDe\n\n" + e.Message);
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

    GetDefaultUserFromSetting: function (nowdate, CallBackFunction, CompleteFunction) {
        try {
            var _options = "$select=cgi_userid&";
            var _filter = "$filter=statecode/Value eq 0 and cgi_ValidFrom le datetime'" + nowdate + "' and (cgi_ValidTo ge datetime'" + nowdate + "' or cgi_ValidTo eq null)";
            var _odata = _options + _filter;
            SDK.REST.retrieveMultipleRecords("cgi_setting", _odata, CallBackFunction, function (error) { alert("SDK.REST.retrieveMultipleRecords.GetDefaultCustomerFromSetting\n\n" + error.message); }, CompleteFunction);
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetDefaultCustomerFromSetting\n\n" + e.Message);
        }
    },




    // *** End Queries used for entity email

    // *** Start Queries used for contact

    GetSecRolesName: function (userRoleId, CallBackFunction, CompleteFunction) {
        try {
            var _options = "$select=Name&";
            var _filter = "$filter=RoleId eq guid'" + userRoleId + "'";
            var _odata = _options + _filter;
            SDK.REST.retrieveMultipleRecords("Role", _odata, CallBackFunction, function (error) { alert("SDK.REST.retrieveMultipleRecords.GetSecRolesName\n\n" + error.message); }, CompleteFunction);
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetSecRolesName\n\n" + e.Message);
        }
    },

    // *** End Queries used for contact

    // *** Start Queries for entity article

    GetSecRolesNameArticle: function (userRoleId, CallBackFunction, CompleteFunction) {
        try {
            var _options = "$select=Name&";
            var _filter = "$filter=RoleId eq guid'" + userRoleId + "'";
            var _odata = _options + _filter;
            SDK.REST.retrieveMultipleRecords("Role", _odata, CallBackFunction, function (error) { alert("SDK.REST.retrieveMultipleRecords.GetSecRolesNameArticle\n\n" + error.message); }, CompleteFunction);
        }
        catch (e) {
            alert("Fel i CGISweden.odata.GetSecRolesNameArticle\n\n" + e.Message);
        }
    }

    // *** End Queries for entity article

};

