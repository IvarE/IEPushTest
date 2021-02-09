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

if (typeof (Endeavor.Skanetrafiken.Contact) == "undefined") {
    Endeavor.Skanetrafiken.Contact = {

        onLoad: function(){
            switch (Xrm.Page.ui.getFormType()) {
                case FORM_TYPE_CREATE:
                    debugger;
                    var emailaddress1 = Xrm.Page.getAttribute("emailaddress1");
                    var emailaddress2 = Xrm.Page.getAttribute("emailaddress2");
                    // Objects in form?
                    if (emailaddress1 != null && emailaddress2 != null) {
                        // email1 present? Move to email2...
                        if (emailaddress1.getValue() != null) {
                            emailaddress2.setValue(emailaddress1.getValue());
                            emailaddress1.setValue(null);
                        }
                    }
                    break;
                case FORM_TYPE_UPDATE:
                case FORM_TYPE_READONLY:
                case FORM_TYPE_DISABLED:
                    break;
                case FORM_TYPE_BULKEDIT:
                    break;
                default:
                    break;
            }
        },

        // Install on onSave
        onSave: function(ctx) {
            debugger;
            var formType = Xrm.Page.ui.getFormType();
            if (formType == 1) { //Create
                Xrm.Page.getAttribute("ed_informationsource").setValue(8); // 8-AdmSkapaKund
            } else if (formType == 2) { //Update
                Xrm.Page.getAttribute("ed_informationsource").setValue(12); // 12-AdmAndraKund
            } else {
                // TODO teo - Vad gör vi om formuläret sparas och det varken är Create eller Update?
            }
        },

        // 
        onSocialSecurityNumberChange: function () {
            var contactNumberControlTag = "cgi_socialsecuritynumber";

            var contactNumber = Xrm.Page.getAttribute(contactNumberControlTag);
            if (!(!contactNumber.getValue() || 0 === contactNumber.getValue().length)) {
                if (!Endeavor.Skanetrafiken.Contact.checkSocSecNumber(contactNumber.getValue())) {
                    Xrm.Page.getControl(contactNumberControlTag).setNotification("Ogiltigt Persnonnummer<BR>(giltiga format: ååmmdd, ååååmmdd, ååååmmddxxxx, ååååmmdd-xxxx)");
                } else {
                    Xrm.Page.getControl(contactNumberControlTag).clearNotification();
                }
            } else {
                Xrm.Page.getControl(contactNumberControlTag).clearNotification();
            }
        },

        onMarkForCreditsafeUpdate: function () {
            debugger;
            var guid = Xrm.Page.data.entity.getId().replace("{", "").replace("}", "");

            if (!Xrm.Page.getAttribute("cgi_socialsecuritynumber")) {
                Xrm.Utility.alertDialog("Inget personnummer funnet på formuläret. Vänligen lägg till.");
                return;
            }
            var socSecNr = Xrm.Page.getAttribute("cgi_socialsecuritynumber").getValue();

            if (Xrm.Page.getAttribute("firstname") && Xrm.Page.getAttribute("lastname"))
                var fullName = Xrm.Page.getAttribute("firstname").getValue() + " " + Xrm.Page.getAttribute("lastname").getValue();
            else if (Xrm.Page.getAttribute("lastname"))
                var fullName = Xrm.Page.getAttribute("lastname").getValue();
            else if (Xrm.Page.getAttribute("firstname"))
                var fullName = Xrm.Page.getAttribute("firstname").getValue();
            else
                var fullName = "Namn saknas";

            var deltabatchQueue = {};
            deltabatchQueue.ed_ContactGuid = guid;
            deltabatchQueue.ed_ContactNumber = socSecNr;
            deltabatchQueue.ed_DeltabatchOperation = { Value: 899310000 };
            deltabatchQueue.ed_name = "ForceUpdate: " + fullName + ", " + Endeavor.Common.Data.dateToString(new Date(), "yyyy/MM/dd", "-");

            SDK.REST.createRecord(
                deltabatchQueue,
                "ed_DeltabatchQueue",
                function (deltabatchQueue) {
                    if (!deltabatchQueue)
                        Xrm.Utility.alertDialog("Uppdatering kanske inte är schemalagd. Inget returvärde då köpost skapades");
                },
                function (deltabatchQueueError) {
                    Xrm.Utility.alertDialog("Något gick fel när uppdatering skulle schemaläggas:\n\n" + deltabatchQueueError.message);
                });
        },

        checkSocSecNumber: function (nr) {
            if (Endeavor.Skanetrafiken.Contact.checkPersonnummer(nr)) {
                var sweSocSec = Xrm.Page.getAttribute("ed_hasswedishsocialsecuritynumber");
                if (sweSocSec != null) {
                    sweSocSec.setValue(true);
                    sweSocSec.setSubmitMode("always");
                } else {
                    alert("Fel i formulär, dolt fält saknas. Vänligen kontakta Administratör");
                }
                return true;
            } else if (Endeavor.Skanetrafiken.Contact.checkNonSwedishSocSecNumber(nr)) {
                var sweSocSec = Xrm.Page.getAttribute("ed_hasswedishsocialsecuritynumber");
                if (sweSocSec != null) {
                    sweSocSec.setValue(false);
                    sweSocSec.setSubmitMode("always");
                } else {
                    alert("Fel i formulär, dolt fält saknas. Vänligen kontakta Administratör");
                }
                return true;
            }
            return false;
        },

        checkPersonnummer: function (nr) {
            this.valid = false;
            //if(!nr.match(/^(\d{2})(\d{2})(\d{2})\-(\d{4})$/)){ return false; }
            if (nr.match(/^(\d{4})(\d{2})(\d{2})-(\d{4})$/)) {
                nr = nr.replace("-", "");
            }
            if (!nr.match(/^(\d{4})(\d{2})(\d{2})(\d{4})$/)) {
                return false;
            }

            this.fullYear = RegExp.$1;
            this.year = this.fullYear.substring(2, 4);
            this.month = RegExp.$2;
            this.day = RegExp.$3;
            this.controldigits = RegExp.$4;

            if (!Endeavor.Skanetrafiken.Contact.checkDateFormat()) {
                return false;
            }

            this.alldigits = this.year + this.month + this.day + this.controldigits;

            var nn = "";
            for (var n = 0; n < this.alldigits.length; n++) {
                nn += ((((n + 1) % 2) + 1) * this.alldigits.substring(n, n + 1));
            }
            this.checksum = 0;

            for (var n = 0; n < nn.length; n++) {
                this.checksum += nn.substring(n, n + 1) * 1;
            }
            this.valid = (this.checksum % 10 == 0) ? true : false;
            this.sex = parseInt(this.controldigits.substring(2, 3)) % 2;
            return this.valid;
        },

        checkNonSwedishSocSecNumber: function (nr) {
            if (nr.match(/^(\d{4})(\d{2})(\d{2})$/)) {
                this.fullYear = RegExp.$1;
                this.year = this.fullYear.substring(2, 4);
                this.month = RegExp.$2;
                this.day = RegExp.$3;
            } else if (nr.match(/^(\d{2})(\d{2})(\d{2})$/)) {
                this.year = RegExp.$1;
                this.fullYear = parseInt(this.year > (new Date()).getFullYear() % 100) ? "19" : "20" + this.year;
                this.month = RegExp.$2;
                this.day = RegExp.$3;
            } else {
                return false;
            }
            if (!Endeavor.Skanetrafiken.Contact.checkDateFormat()) {
                return false;
            }
            return true;
        },

        checkDateFormat: function () {
            var months = new Array(31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31);
            if (this.fullYear % 400 == 0 || this.fullYear % 4 == 0 && this.fullYear % 100 != 0) {
                months[1] = 29;
            }

            if (this.month * 1 < 1 || this.month * 1 > 12 || this.day * 1 < 1 || this.day * 1 > months[this.month * 1 - 1]) {
                return false;
            }
            return true;
        }
    };
}