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

        callAction: function (actionName, entityName, targetId, sucessCallback, errorCallback) {

            var target = {};
            target.entityType = entityName;
            target.id = targetId;

            var req = {};
            req.entity = target;
            req.getMetadata = function () {
                return {
                    boundParameter: "entity",
                    parameterTypes: {
                        "entity": {
                            typeName: "mscrm." + entityName,
                            structuralProperty: 5
                        }
                    },
                    operationType: 0,
                    operationName: actionName
                };
            };

            Xrm.WebApi.online.execute(req).then(sucessCallback, errorCallback);
        },

        SaveAndSend: function (executionContext) {
            debugger;
            var formContext = executionContext.getFormContext();
            formContext.data.save().then(function () { Endeavor.Skanetrafiken.TextMessage.SendSMS(formContext); }, function (error) { Endeavor.Skanetrafiken.TextMessage.ErrorOnSave(formContext, error); });
        },

        SendSMS: function (formContext) {
            debugger;
            formContext.ui.setFormNotification("Sending Text Message. Please Wait.", "INFO", "SendSMSNotification");

            var idRecord = formContext.data.entity.getId();

            Endeavor.Skanetrafiken.TextMessage.callAction("ed_SendTextMessage", "ed_textmessage", idRecord,
                function () {
                    formContext.data.refresh();
                    formContext.ui.setFormNotification("Text Message successfully delivered.", "INFO", "SendSMSNotification");
                },
                function (e, t) {
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