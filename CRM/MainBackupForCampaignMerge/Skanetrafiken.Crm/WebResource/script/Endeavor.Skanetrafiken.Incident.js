FORM_TYPE_CREATE = 1;
FORM_TYPE_UPDATE = 2;
FORM_TYPE_READONLY = 3;
FORM_TYPE_DISABLED = 4;
FORM_TYPE_QUICKCREATE_DEPRECATED = 5;
FORM_TYPE_BULKEDIT = 6;


// Begin scoping 
if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {
    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.Incident) == "undefined") {
    Endeavor.Skanetrafiken.Incident = {

        onLoad: function () {
            debugger;
            var contactAttribute = Xrm.Page.getAttribute("cgi_contactid")
            var customerEmailAttribute = Xrm.Page.getAttribute("cgi_customer_email");
            if (contactAttribute && customerEmailAttribute) {
                if (customerEmailAttribute.getValue && !customerEmailAttribute.getValue()) {
                    if (contactAttribute.getValue && contactAttribute.getValue() && contactAttribute.getValue().length > 0) {
                        var url = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "ContactSet?$select=EMailAddress1,EMailAddress2&$filter=ContactId eq (guid'" + contactAttribute.getValue()[0].id + "')";
                        var contactResultSet = Endeavor.Common.Data.fetchJSONResults(url);

                        if (contactResultSet && contactResultSet.length > 0) {
                            if (contactResultSet[0].EMailAddress1)
                                customerEmailAttribute.setValue(contactResultSet[0].EMailAddress1);
                            else if (contactResultSet[0].EMailAddress2)
                                customerEmailAttribute.setValue(contactResultSet[0].EMailAddress2);
                        }
                    }
                }
            }
            

        },

    };
}