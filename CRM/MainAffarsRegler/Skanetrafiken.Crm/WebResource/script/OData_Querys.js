if (typeof (Endeavor) === "undefined") {
    var Endeavor = {
    };
}

Endeavor.OData_Querys = {

    // *** Start Queries used for entity Case
    GetTravelInfoForCase: function (caseid, formContext) {

        try {

            //Return the number of travelinformation posts registered on this case
            //This is done synchronously because it is part of a validation process
            //TODO - Pedro
            Xrm.WebApi.retrieveRecord("cgi_travelinformation", caseid, "?$select=cgi_travelinformationid")
                .then(function (cgi_travelinformation) {
                    if (cgi_travelinformation.result.length === 0) {

                        var cgi_casdet_row1_cat3idLookup = formContext.getAttribute("cgi_casdet_row1_cat3id").getValue();

                        return Xrm.WebApi.retrieveRecord("cgi_categorydetail", cgi_casdet_row1_cat3idLookup[0].id, "?$select=cgi_requirestravelinfo")
                    }
                    else {
                        return;
                    }
                })
                .then(function (cgi_categorydetail) {
                    var cgi_categorydetails = cgi_categorydetail.result[0];

                    if (cgi_categorydetails["cgi_requirestravelinfo"] == 1) {

                        //ge användaren en möljighet att avsluta ärendet ändå utan trafikinfo genom att klicka ok
                        if (confirm("Ärenden i kategori " + cgi_casdet_row1_cat3idLookup[0].name + " förväntas innehålla trafikinformation, vilken saknas i detta ärende. Vill du verkligen avsluta ärendet utan trafikinformation? ") == true) {

                            //ange explicit att ärendet ska sparas utan trafikinfo. Annars kommer en plugin förhindra att det avslutas utan trafikinformation
                            formContext.getAttribute("cgi_notravelinfo").setValue(1);
                        }
                        else {
                            //genom att avbryta exekveringen undviks att ärendet att avslutas
                            return;
                        }
                    }
                    else {
                        return;
                    }
                });
        }
        catch (e) {
            alert("Fel i Endeavor.OData_Querys.GetTravelInfoForCase\n\n" + e.Message);
        }
    },

    GetDefaultCustomerFromSetting: function (nowdate, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("cgi_setting", "?$select=_cgi_defaultcustomeroncase_value&$filter=statecode eq 0 and cgi_validfrom le " + nowdate + "Z and (cgi_validto ge " + nowdate + "Z or cgi_validto eq null)").then(
                function success(result) {
                    Endeavor.Skanetrafiken.Incident.setCustomerOnLoad_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    alert("Fel i Endeavor.OData_Querys.GetDefaultCustomerFromSetting\n\n" + e.Message);
                }
            );
        }
        catch (e) {
            alert("Fel i Endeavor.OData_Querys.GetDefaultCustomerFromSetting\n\n" + e.Message);
        }
    },

    GetDefaultCaseCategory3Setting: function (nowdate, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("cgi_setting", "?$select=_cgi_category_detail3id_value&$filter=statecode eq 0 and cgi_validfrom le " + nowdate + "Z and (cgi_validto ge " + nowdate + "Z or cgi_validto eq null)").then(
                function success(result) {
                    Endeavor.Skanetrafiken.Incident.casetypecode_onchange_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    alert("Fel i Endeavor.OData_Querys.GetDefaultCaseCategory3Setting\n\n" + e.Message);
                }
            );
        }
        catch (e) {
            alert("Fel i Endeavor.OData_Querys.GetDefaultCaseCategory3Setting\n\n" + e.Message);
        }

    },

    Getcgi_refundtypeproductnotrequiredidSetting: function (nowdate, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("cgi_setting", "?$select=_cgi_refundtypeproductnotrequiredid_value&$filter=statecode eq 0 and cgi_validfrom le " + nowdate + "Z and (cgi_validto ge " + nowdate + "Z or cgi_validto eq null)").then(
                function success(result) {
                    Endeavor.Skanetrafiken.cgi_refund.Getcgi_refundtypeproductnotrequiredidSetting_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    alert("Fel i Endeavor.OData_Querys.Getcgi_refundtypeproductnotrequiredidSetting\n\n" + e.Message);
                }
            );
        }
        catch (e) {
            alert("Fel i Endeavor.OData_Querys.Getcgi_refundtypeproductnotrequiredidSetting\n\n" + e.Message);
        }
    },

    GetBOMBUrlFromSetting: function (nowdate, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("cgi_setting", "?$select=cgi_bomburl&$filter=statecode eq 0 and cgi_validfrom le " + nowdate + "Z and (cgi_validto ge " + nowdate + "Z or cgi_validto eq null)").then(
                function success(result) {
                    Endeavor.Skanetrafiken.Incident.openBombApp_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    alert("Fel i Endeavor.OData_Querys.GetBOMBUrlFromSetting\n\n" + e.Message);
                }
            );
        }
        catch (e) {
            alert("Fel i Endeavor.OData_Querys.GetBOMBUrlFromSetting\n\n" + e.Message);
        }
    },

    GetRGOLUrlFromSetting: function (nowdate, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("cgi_setting", "?$select=cgi_rgolurl&$filter=statecode eq 0 and cgi_validfrom le " + nowdate + "Z and (cgi_validto ge " + nowdate + "Z or cgi_validto eq null)").then(
                function success(result) {
                    Endeavor.Skanetrafiken.Incident.getRGOLapiurl_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    alert("Fel i Endeavor.OData_Querys.GetRGOLUrlFromSetting\n\n" + e.Message);
                }
            );
        }
        catch (e) {
            alert("Fel i Endeavor.OData_Querys.GetRGOLUrlFromSetting\n\n" + e.Message);
        }
    },

    GetRGOLUrlFromSettingNew: function (nowdate, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("cgi_setting", "?$select=cgi_rgolurl&$filter=statecode eq 0 and cgi_validfrom le " + nowdate + "Z and (cgi_validto ge " + nowdate + "Z or cgi_validto eq null)").then(
                function success(result) {
                    Endeavor.Skanetrafiken.Incident.getRGOLapiurl_callbackNew(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    alert("Fel i Endeavor.OData_Querys.GetRGOLUrlFromSetting\n\n" + e.Message);
                }
            );
        }
        catch (e) {
            alert("Fel i Endeavor.OData_Querys.GetRGOLUrlFromSetting\n\n" + e.Message);
        }
    },

    //Returns category guid for level 1 and level 2
    GetParentCategory: function (_categoryid, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("cgi_categorydetail", "?$select=_cgi_parentid_value,_cgi_parentid2_value&$filter=cgi_categorydetailid eq" + _categoryid).then(
                function success(result) {
                    Endeavor.Skanetrafiken.Incident.category2_onchange_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    alert("Fel i Endeavor.OData_Querys.GetParentCategory\n\n" + e.Message);
                }
            );
        }
        catch (e) {
            alert("Fel i Endeavor.OData_Querys.GetParentCategory\n\n" + e.Message);
        }
    },

    //Returns category guid for level 1 and level 2
    GetParentCategory3: function (_categoryid, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("cgi_categorydetail", "?$select=_cgi_parentid_value,_cgi_parentid2_value&$filter=cgi_categorydetailid eq" + _categoryid).then(
                function success(result) {
                    Endeavor.Skanetrafiken.Incident.category3_onchange_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    alert("Fel i Endeavor.OData_Querys.GetParentCategory\n\n" + e.Message);
                }
            );
        }
        catch (e) {
            alert("Fel i Endeavor.OData_Querys.GetParentCategory\n\n" + e.Message);
        }
    },

    //Get default queue from user
    GetDefaultQueue: function (userId, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("systemuser", "?$select=_queueid_value&$filter=systemuserid eq " + userId).then(
                function success(result) {
                    Endeavor.Skanetrafiken.Email.SetSenderEmail_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    alert("Fel i Endeavor.OData_Querys.GetDefaultQueue\n\n" + e.Message);
                }
            );
        }
        catch (e) {
            alert("Fel i Endeavor.OData_Querys.GetDefaultQueue\n\n" + e.Message);
        }
    },

    //Get letter template
    GetLetterTemplate: function (_id, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("cgi_letter_template", "?$select=cgi_title,cgi_template_body&$filter=cgi_letter_templateid eq " + _id).then(
                function success(result) {
                    Endeavor.Skanetrafiken.Incident.letter_template_onchange_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    alert("Fel i Endeavor.OData_Querys.GetLetterTemplate\n\n" + e.Message);
                }
            );
        }
        catch (e) {
            alert("Fel i Endeavor.OData_Querys.GetLetterTemplate\n\n" + e.Message);
        }
    },
    // *** End Queries used for entity Case

    // *** Start Queries used for entity account
    GetSecRolesNameAccount: function (userRoleId, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("role", "?$select=name&$filter=roleid eq" + userRoleId).then(
                function success(result) {
                    Endeavor.Skanetrafiken.Account.checkIfUserHasRole_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    alert("Fel i Endeavor.OData_Querys.GetSecRolesNameAccount\n\n" + e.Message);
                }
            );
        }
        catch (e) {
            alert("Fel i Endeavor.OData_Querys.GetSecRolesNameAccount\n\n" + e.Message);
        }
    },
    // *** End Queries used for entity account

    // *** Start Queries used for entity cgi_refund
    GetAmountLimitFromSetting: function (nowdate, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("cgi_setting", "?$select=cgi_amount_limit, cgi_amount_limit_warn&$filter=statecode eq 0 and cgi_validfrom le " + nowdate + "Z and(cgi_validto ge " + nowdate + "Z or cgi_validto eq null)").then(
                function success(result) {
                    Endeavor.Skanetrafiken.cgi_refund.ctrlAmountLimit_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    alert("Fel i Endeavor.OData_Querys.GetAmountLimitFromSetting\n\n" + e.Message);
                }
            );
        }
        catch (e) {
            alert("Fel i Endeavor.OData_Querys.GetAmountLimitFromSetting\n\n" + e.Message);
        }


    },

    GetRefundSetting: function (refundid, formContext) {
        try {
            Xrm.WebApi.retrieveRecord("cgi_refundtype", refundid,"?$select=cgi_refundoption,cgi_financialtransaction,cgi_refundtypeid,cgi_refundtypename,_cgi_refundaccountid_value,_cgi_refundresponsibleid_value,_cgi_refundproductid_value,cgi_reinvoice,statecode").then(
                function success(result) {
                    
                        Endeavor.Skanetrafiken.cgi_refund.refundtypeid_OnChange_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    alert("Fel i Endeavor.OData_Querys.GetRefundSetting\n\n" + e.Message);
                }
            );

        }
        catch (e) {
            alert("Fel i Endeavor.OData_Querys.GetRefundSetting\n\n" + e.Message);
        }
    },

    GetRefundSettingOnLoad: function (refundid, formContext) {
        try {
            Xrm.WebApi.retrieveRecord("cgi_refundtype", refundid, "?$select=cgi_financialtransaction,_cgi_refundaccountid_value,cgi_refundoption,_cgi_refundproductid_value,_cgi_refundresponsibleid_value,cgi_refundtypeid,cgi_refundtypename,cgi_reinvoice,statecode").then(
                function success(result) {

                        Endeavor.Skanetrafiken.cgi_refund.refundtypeid_Onload_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    alert("Fel i Endeavor.OData_Querys.GetRefundSettingOnLoad\n\n" + e.Message);
                }
            );

        }
        catch (e) {
            alert("Fel i Endeavor.OData_Querys.GetRefundSettingOnLoad\n\n" + e.Message);
        }
    },

    Getcgi_refundaccountNumber: function (cgi_refundaccountid, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("cgi_refundaccount", "?$select=cgi_account,cgi_refundaccountid&$filter=cgi_refundaccountid eq " + cgi_refundaccountid).then(
                function success(result) {
                    Endeavor.Skanetrafiken.cgi_refund.Getcgi_refundaccountNumber_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    alert("Fel i Endeavor.OData_Querys.Getcgi_refundaccountNumber\n\n" + e.Message);
                }
            );
        }
        catch (e) {
            alert("Fel i Endeavor.OData_Querys.Getcgi_refundaccountNumber\n\n" + e.Message);
        }
    },

    GetDefaultMilageContributionFromSetting: function (nowdate, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("cgi_setting", "?$select=cgi_milage_contribution&$filter=statecode eq 0 and cgi_validfrom le " + nowdate + "Z and (cgi_validto ge " + nowdate + "Z or cgi_validto eq null)").then(
                function success(result) {
                    Endeavor.Skanetrafiken.cgi_refund.setMilageContributionOnLoad_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    alert("Fel i Endeavor.OData_Querys.GetDefaultCustomerFromSetting\n\n" + e.Message);
                }
            );
        }
        catch (e) {
            alert("Fel i Endeavor.OData_Querys.GetDefaultCustomerFromSetting\n\n" + e.Message);
        }
    },
    //MaxP 2016-04-20 ytterligare en kolumner som returneras i svaret. Ursprung, ärendetyp och flagga för manuella ärenden
    GetCaseNumber: function (caseid, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("incident", "?$select=caseorigincode,casetypecode,cgi_compensationclaimfromrgol,cgi_iscompleted,cgi_milagefrom,cgi_milagekilometers,cgi_milagelicenseplatenumber,cgi_milageto,_cgi_refundreimbursementform_value,_cgi_refundtypes_value,cgi_taxiclaimedamount,cgi_taxifrom,cgi_taxito,cgi_unregisterdtravelcard,ticketnumber&$filter=incidentid eq " + caseid).then(
                function success(result) {
                    Endeavor.Skanetrafiken.cgi_refund.getCaseNumber_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    alert("Fel i Endeavor.OData_Querys.GetCaseNumber\n\n" + e.Message);
                }
            );
        }
        catch (e) {
            alert("Fel i Endeavor.OData_Querys.GetCaseNumber\n\n" + e.Message);
        }
    },
    //MaxP 2016-05-09 Ytterligare kolulmn cgi_emailaddress för att sätta leverans epostadress på beslut
    //MaxP 2016-05-11 Hämtar nytt fält (cgi_rgol_delivery_email) från ärende för att sätta leverans epostadress på beslut
    GetEmailAddress: function (caseid, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("incident", "?$select=cgi_customer_email,cgi_emailaddress,cgi_iscompleted,cgi_rgol_delivery_email&$filter=incidentid eq " + caseid).then(
                function success(result) {
                    Endeavor.Skanetrafiken.cgi_refund.getGetEmailAddress_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    alert("Fel i Endeavor.OData_Querys.GetEmailAddress\n\n" + e.Message);
                }
            );
        }
        catch (e) {
            alert("Fel i Endeavor.OData_Querys.GetEmailAddress\n\n" + e.Message);
        }
    },

    //
    GetReimbursementForm: function (reimbursementid, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("cgi_reimbursementform", "?$select=cgi_attestation,cgi_couponsms,cgi_giftcard,cgi_loadcard,cgi_payment,cgi_payment_abroad,cgi_reimbursementformid,cgi_reimbursementname,cgi_reinvoicing,cgi_sendtostralfors,cgi_time_valid,cgi_useaccount,cgi_useproduct,cgi_useresponsible&$filter=statecode eq 0 and cgi_reimbursementformid eq " + reimbursementid).then(
                function success(result) {
                    Endeavor.Skanetrafiken.cgi_refund.reimbursementformid_OnChange_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    alert("Fel i Endeavor.OData_Querys.GetReimbursementForm\n\n" + e.Message);
                }
            );
        }
        catch (e) {
            alert("Fel i Endeavor.OData_Querys.GetReimbursementForm\n\n" + e.Message);
        }
    },

    GetReimbursementFormOnLoad: function (reimbursementid, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("cgi_reimbursementform", "?$select=cgi_attestation,cgi_couponsms,cgi_giftcard,cgi_loadcard,cgi_payment,cgi_payment_abroad,cgi_reimbursementformid,cgi_reimbursementname,cgi_reinvoicing,cgi_sendtostralfors,cgi_time_valid,cgi_useaccount,cgi_useproduct,cgi_useresponsible&$filter=statecode eq 0 and cgi_reimbursementformid eq " + reimbursementid).then(
                function success(result) {
                    Endeavor.Skanetrafiken.cgi_refund.reimbursementformid_OnLoad_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    alert("Fel i Endeavor.OData_Querys.GetReimbursementFormOnLoad\n\n" + e.Message);
                }
            );
        }
        catch (e) {
            alert("Fel i Endeavor.OData_Querys.GetReimbursementFormOnLoad\n\n" + e.Message);
        }
    },

    //Get bic and iban from incident. MaxP 2016-04-20
    GetBicIban: function (caseid, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("incident", "?$select=cgi_bic,cgi_iban,cgi_iscompleted&$filter=incidentid eq " + caseid).then(
                function success(result) {
                    Endeavor.Skanetrafiken.cgi_refund.setBicIban_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    alert("Fel i Endeavor.OData_Querys.GetBicIban\n\n" + e.Message);
                }
            );
        }
        catch (e) {
            alert("Fel i Endeavor.OData_Querys.GetBicIban\n\n" + e.Message);
        }
    },

    //Get social sec number from incident.
    GetSocSecNumber: function (caseid, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("incident", "?$select=cgi_soc_sec_number,cgi_rgol_socialsecuritynumber,cgi_iscompleted&$filter=incidentid eq" + caseid).then(
                function success(result) {
                    Endeavor.Skanetrafiken.cgi_refund.setSocSecOnLoad_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    alert("Fel i Endeavor.OData_Querys.GetGetSocSecNumber\n\n" + e.Message);
                }
            );
        }
        catch (e) {
            alert("Fel i Endeavor.OData_Querys.GetGetSocSecNumber\n\n" + e.Message);
        }
    },

    //Get mobilenumber from incident.
    GetMobileNumber: function (caseid, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("incident", "?$select=cgi_customer_telephonenumber,cgi_rgol_telephonenumber,cgi_iscompleted&$filter=incidentid eq" + caseid).then(
                function success(result) {
                    Endeavor.Skanetrafiken.cgi_refund.setMobileNumber_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    alert("Fel i Endeavor.OData_Querys.GetMobileNumber\n\n" + e.Message);
                }
            );
        }
        catch (e) {
            alert("Fel i Endeavor.OData_Querys.GetMobileNumber\n\n" + e.Message);
        }
    },

    //Get cgi_contact and cgi_account from incident.
    GetContactAccount: function (caseid, formContext) {
        try {
            Xrm.WebApi.retrieveRecord("incident", caseid , "?$select=_cgi_contactid_value,_cgi_accountid_value").then(
                function success(result) {
                    Endeavor.Skanetrafiken.cgi_refund.GetContactAccount_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    alert("Fel i Endeavor.OData_Querys.GetContactAccount\n\n" + e.Message);
                }
            );
        }
        catch (e) {
            alert("Fel i Endeavor.OData_Querys.GetContactAccount\n\n" + e.Message);
        }
    },

    //Get RSID from user
    GetRSID: function (userId, formContext) {
        try {
            Xrm.WebApi.retrieveRecord("systemuser", userId, "?$select=cgi_rsid").then(
                function success(result) {
                    Endeavor.Skanetrafiken.cgi_refund.ctrlAttest_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    alert("Fel i Endeavor.OData_Querys.GetRSIDe\n\n" + e.Message);
                }
            );
        }
        catch (e) {
            alert("Fel i Endeavor.OData_Querys.GetRSIDe\n\n" + e.Message);
        }
    },

    GetSecRolesNameRefund: function (userRoleId, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("role", "?$select=name&$filter=roleid eq" + userRoleId).then(
                function success(result) {
                    Endeavor.Skanetrafiken.cgi_refund.CheckUserRoleOnchange_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    alert("Fel i Endeavor.OData_Querys.GetSecRolesName\n\n" + e.Message);
                }
            );
        }
        catch (e) {
            alert("Fel i Endeavor.OData_Querys.GetSecRolesName\n\n" + e.Message);
        }
    },

    // *** End Queries used for entity cgi_refund

    // *** Start Queries used for entity email
    GetDefaultUserFromSetting: function (nowdate, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("cgi_setting", "?$select=cgi_userid&$filter=statecode eq 0 and cgi_validfrom le " + nowdate + "Z and (cgi_validto ge '" + nowdate + "Z or cgi_validto eq null)").then(
                function success(result) {
                    Endeavor.Skanetrafiken.Email.SetSenderEmailNoReply_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    alert("Fel i Endeavor.OData_Querys.GetDefaultCustomerFromSetting\n\n" + e.Message);
                }
            );
        }
        catch (e) {
            alert("Fel i Endeavor.OData_Querys.GetDefaultCustomerFromSetting\n\n" + e.Message);
        }
    },

    // *** End Queries used for entity email

    // *** Start Queries used for contact
    GetSecRolesNameContact: function (userRoleId, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("role", "?$select=name&$filter=roleid eq" + userRoleId).then(
                function success(result) {
                    Endeavor.Skanetrafiken.Contact.checkIfUserHasRole_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    alert("Fel i Endeavor.OData_Querys.GetSecRolesNameContact\n\n" + e.Message);
                }
            );
        }
        catch (e) {
            alert("Fel i Endeavor.OData_Querys.GetSecRolesNameContact\n\n" + e.Message);
        }
    },

    // *** End Queries used for contact

    // *** Start Queries for entity filelinks

    GetFilelinks: function (incidentId, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("cgi_filelink", "?$select=cgi_url&$filter=cgi_incidentid eq" + incidentId).then(
                function success(result) {
                    Endeavor.Skanetrafiken.Email.set_filelinks_onchange_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    alert("Fel i Endeavor.OData_Querys.GetFilelinks\n\n" + e.Message);
                }
            );
        }
        catch (e) {
            alert("Fel i Endeavor.OData_Querys.GetFilelinks\n\n" + e.Message);
        }
    },

    // *** End Queries for entity filelinks

    // *** Start Queries for entity article

    GetSecRolesNameArticle: function (userRoleId, formContext) {
        try {
            Xrm.WebApi.retrieveMultipleRecords("role", "?$select=name&$filter=roleid eq" + userRoleId).then(
                function success(result) {
                    Endeavor.Skanetrafiken.KBArticle.checkIfUserHasMarketingRole_callback(result, formContext);
                },
                function (error) {
                    console.log(error.message);
                    alert("Fel i Endeavor.OData_Querys.GetSecRolesNameArticle\n\n" + e.Message);
                }
            );
        }
        catch (e) {
            alert("Fel i Endeavor.OData_Querys.GetSecRolesNameArticle\n\n" + e.Message);
        }

    }

    // *** End Queries for entity article
}