if (typeof (Endeavor) == "undefined") {
    var Endeavor = {

    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {
    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.TextMessage) == "undefined") {
    Endeavor.Skanetrafiken.TextMessage = {

        SaveAndSend: function (formContext) {
            debugger;
            formContext.data.save().then(function () { Endeavor.Skanetrafiken.TextMessage.SendSMS(formContext); }, function (error) { Endeavor.Skanetrafiken.TextMessage.ErrorOnSave(formContext, error); });
        },

        SendSMS: function (formContext) {
            debugger;
            formContext.ui.setFormNotification("Sending Text Message. Please Wait.", "INFO", "SendSMSNotification");

            var idRecord = formContext.data.entity.getId();

            Endeavor.formscriptfunctions.callAction("ed_SendTextMessage", "ed_textmessage", idRecord, null,
                function () {
                    formContext.data.refresh();
                    formContext.ui.setFormNotification("Text Message successfully delivered.", "INFO", "SendSMSNotification");
                }, function () {
                    // Error
                    formContext.ui.setFormNotification("Something went wrong: " + e, "INFO", "SendSMSNotification");

                    // Write the trace log to the dev console
                    if (window.console && console.error) {
                        console.error(e + "\n" + t);
                    }
                });
        },

        ErrorOnSave: function (formContext, error) {
            debugger;
            formContext.ui.setFormNotification("Error on save: " + error.message);
        }
    }
}