// Begin scoping 
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

        onLoad: function (executionContext) {
            debugger;
            if (executionContext == null)
                return;

            var formContext = executionContext.getFormContext();

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

        },

        updateFullNameField: function (executionContext) {
            var formContext = executionContext.getFormContext();
            var name = Endeavor.formscriptfunctions.GetValue("fullname", formContext);
            var lastname = formContext.getAttribute("lastname").getValue();
            if (name == null) {
                formContext.getAttribute("lastname").setValue(lastname + " ");

                formContext.data.entity.save();
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