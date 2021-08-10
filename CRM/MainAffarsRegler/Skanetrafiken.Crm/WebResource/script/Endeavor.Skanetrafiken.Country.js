// Begin scoping 
if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {
    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.Country) == "undefined") {
    Endeavor.Skanetrafiken.Country = {

        onPhoneCodeChange: function (executionContext) {
            debugger;
            var formContext = executionContext.getFormContext();

            var phoneControl = formContext.getControl("ed_phonecode");
            var phoneCode = Endeavor.formscriptfunctions.GetValue("ed_phonecode", formContext);

            if (phoneCode == null)
                return;

            if (phoneCode.indexOf("+") > -1)
                phoneControl.setNotification("Ange telefonnummer utan landsprefix.");
            else
                phoneControl.clearNotification();

            if (!phoneCode.match(/^(\d{1,3}|\d{1,4})$/)) 
                phoneControl.setNotification("Ange telefonnummer med fyra siffror.");
            else
                phoneControl.clearNotification();
        },
        
    };
}