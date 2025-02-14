﻿// Begin scoping 
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

        onSave: function (executionContext) {
            debugger;
            var formContext = executionContext.getFormContext();
            var formType = formContext.ui.getFormType();

            if (formContext.getAttribute("ed_informationsource") == null)
                return;

            if (formType == 1) //Create
                formContext.getAttribute("ed_informationsource").setValue(8);  // 8-AdmSkapaKund
            else if (formType == 2) //Update
                formContext.getAttribute("ed_informationsource").setValue(12);  // Skall vara 12 men är bytt till 3-OinloggatKop 
        },

        onSocialSecurityNumberChange: function (executionContext) {
            debugger;
            var formContext = executionContext.getFormContext();
            var contactNumberControlTag = "cgi_socialsecuritynumber";

            var contactNumber = formContext.getAttribute(contactNumberControlTag);
            if (!(!contactNumber.getValue() || 0 === contactNumber.getValue().length)) {
                if (!Endeavor.Skanetrafiken.Contact.checkSocSecNumber(formContext, contactNumber.getValue()))
                    formContext.getControl(contactNumberControlTag).setNotification("Ogiltigt Persnonnummer\r\n(giltiga format: ååmmdd, ååååmmdd, ååååmmddxxxx, ååååmmdd-xxxx)");
                else
                    formContext.getControl(contactNumberControlTag).clearNotification();
            }
            else
                formContext.getControl(contactNumberControlTag).clearNotification();
        },

        checkSocSecNumber: function (formContext, nr) {
            if (Endeavor.Skanetrafiken.Contact.checkPersonnummer(nr)) {
                var sweSocSec = formContext.getAttribute("ed_hasswedishsocialsecuritynumber");
                if (sweSocSec != null) {
                    sweSocSec.setValue(true);
                    sweSocSec.setSubmitMode("always");
                } else
                    alert("Fel i formulär, dolt fält saknas. Vänligen kontakta Administratör");
                
                return true;
            } else if (Endeavor.Skanetrafiken.Contact.checkNonSwedishSocSecNumber(nr)) {
                var sweSocSec = formContext.getAttribute("ed_hasswedishsocialsecuritynumber");
                if (sweSocSec != null) {
                    sweSocSec.setValue(false);
                    sweSocSec.setSubmitMode("always");
                } else
                    alert("Fel i formulär, dolt fält saknas. Vänligen kontakta Administratör");

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