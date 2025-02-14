﻿// Begin scoping 
if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {
    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.Lead) == "undefined") {
    Endeavor.Skanetrafiken.Lead = {

        infotainmentForm: ["Lead - Annons"],

        onLoad: function (executionContext) {
            debugger;
            if (executionContext == null)
                return;

            var formContext = executionContext.getFormContext();

            var roles = [];
            roles[0] = "Skånetrafiken Annons"; 

            var isUserCheck = Endeavor.Skanetrafiken.Lead.currentUserHasSecurityRole(roles);

            if (isUserCheck) {
                Endeavor.Skanetrafiken.Lead.setInfotainmentValues(formContext);
            }

            //var url = parent.parent.formContext.getUrl();
            //var recordId = url.split('&id=')[1];

            //if (recordId == "" || recordId == null)
            //    return;

            //var accountId = decodeURIComponent(recordId).replace("{", "").replace("}", "");

            //Xrm.WebApi.retrieveRecord("account", accountId, "?$select=_primarycontactid_value,name").then(
            //    function success(result) {

            //        if (result._primarycontactid_value != null) {
            //            debugger;
            //            var contactName = result["_primarycontactid_value@OData.Community.Display.V1.FormattedValue"];
            //            var contactId = result._primarycontactid_value;

            //            Endeavor.formscriptfunctions.SetLookup("parentcontactid", "contact", contactId, contactName, formContext);
            //            Endeavor.Skanetrafiken.Lead.onChangeParentContactId(executionContext);
            //        }

            //        var accountName = result.name;
            //        var accountId = result.accountid;
            //        Endeavor.formscriptfunctions.SetLookup("parentaccountid", "account", accountId, accountName, formContext);
            //        Endeavor.Skanetrafiken.Lead.onChangeParentAccountId(executionContext);
            //    },
            //    function (error) {
            //        console.log(error.message);
            //        Endeavor.formscriptfunctions.AlertCustomDialog(error.message);
            //    }
            //);



            Endeavor.Skanetrafiken.Lead.updateFullNameField(executionContext);
            Endeavor.Skanetrafiken.Lead.setCompanyFieldValue(executionContext);
            //Endeavor.Skanetrafiken.Lead.setInfotainmentFilters(formContext);

        },

        setInfotainmentFilters: function (formContext) {
            var formItem = formContext.ui.formSelector.getCurrentItem();

            if (formItem == null)
                return;

            var formName = formItem.getLabel();


            var valueList = Endeavor.Skanetrafiken.Lead.infotainmentForm[0];
            if (formName == valueList) {

                var company = formContext.getAttribute("parrentaccountid");
                //var companyGuid = company[0].id;
                if (company == null) {
                    return;
                }

                company = formContext.getAttribute("parrentaccountid").getValue();

                formContext.getControl("parentcontactid").addPreSearch(function () {

                    var fetchXml = "<filter type='and'><condition attribute='ed_infotainmentcontact' operator='eq' value='1' /><condition attribute='parentcustomerid' operator='eq' uitype='account' value='" + company[0].id + "' /></filter>";

                    formContext.getControl("parentcontactid").addCustomFilter(fetchXml);

                });

            }


        },

        setInfotainmentValues: function (formContext) {
            formContext.getAttribute("ed_organizationtype").setValue(899310004);

        },

        updateFullNameField: function (executionContext) {
            var formContext = executionContext.getFormContext();
            var name = Endeavor.formscriptfunctions.GetValue("fullname", formContext);
            var lastname = formContext.getAttribute("lastname").getValue();
            if (name == null) {
                formContext.getAttribute("lastname").setValue(lastname + " ");
            }
        },

        setCompanyFieldValue: function (executionContext) {
            var formContext = executionContext.getFormContext();

            var parentCompany = formContext.getAttribute("parentaccountid").getValue();
            var company = formContext.getAttribute("ed_company").getValue();
            if (parentCompany != null && company == null) {

                formContext.getAttribute("ed_company").setValue(parentCompany);

            }
        },

        onChangeParentAccountId: function (executionContext) {
            debugger;
            if (executionContext == null)
                return;

            var formContext = executionContext.getFormContext();
            var parentAccountId = Endeavor.formscriptfunctions.GetValue("parentaccountid", formContext);

            if (parentAccountId)
                Endeavor.formscriptfunctions.SetState("companyname", true, formContext);
            else
                Endeavor.formscriptfunctions.SetState("companyname", false, formContext);
        },

        currentUserHasSecurityRole: function (roles) {
            var fetchXml =
                "<fetch mapping='logical'>" +
                "<entity name='systemuser'>" +
                "<attribute name='systemuserid' />" +
                "<filter type='and'>" +
                "<condition attribute='systemuserid' operator='eq-userid' />" +
                "</filter>" +
                "<link-entity name='systemuserroles' from='systemuserid' to='systemuserid' visible='false' intersect='true'>" +
                "<link-entity name='role' from='roleid' to='roleid' alias='r'>" +
                "<filter type='or'>";

            for (var i = 0; i < roles.length; i++) {
                fetchXml += "<condition attribute='name' operator='eq' value='" + roles[i] + "' />";
            }

            fetchXml += "            </filter>" +
                "</link-entity>" +
                "</link-entity>" +
                "</entity>" +
                "</fetch>";
            var modifiedFetchXml = fetchXml.replace("&", "&amp;");

            var users = Endeavor.formscriptfunctions.executeFetchGetCount(modifiedFetchXml, "systemusers");
            if (users > 0)
                return true;
            else
                return false;
        },

        onChangeParentContactId: function (executionContext) {
            debugger;
            if (executionContext == null)
                return;

            var formContext = executionContext.getFormContext();
            var parentContactId = Endeavor.formscriptfunctions.GetValue("parentcontactid", formContext);

            if (parentContactId) {
                Endeavor.formscriptfunctions.SetState("firstname", true, formContext);
                Endeavor.formscriptfunctions.SetState("lastname", true, formContext);
                Endeavor.formscriptfunctions.SetState("jobtitle", true, formContext);
                Endeavor.formscriptfunctions.SetState("emailaddress1", true, formContext);
                Endeavor.formscriptfunctions.SetState("mobilephone", true, formContext);
            }
            else {
                Endeavor.formscriptfunctions.SetState("firstname", false, formContext);
                Endeavor.formscriptfunctions.SetState("lastname", false, formContext);
                Endeavor.formscriptfunctions.SetState("jobtitle", false, formContext);
                Endeavor.formscriptfunctions.SetState("emailaddress1", false, formContext);
                Endeavor.formscriptfunctions.SetState("mobilephone", false, formContext);
            }
        }
    };
}