if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {
    Endeavor.Skanetrafiken = {
    };
}


//JS TO DELETE ACTIVITIES FROM MKLNOTIFY CORRUPT DATA
if (typeof (Endeavor.Skanetrafiken.Activity) == "undefined") {
    Endeavor.Skanetrafiken.Activity = {

        testActivities: function () {
            var fetchXml = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' no-lock='true'>" +
                "<entity name='activitypointer'>" +
                "<attribute name='activityid' />" +
                "<filter type='and'>" +
                "<condition attribute='activitytypecode' operator='eq' value='10115' />" +
                "<condition attribute='createdon' operator='olderthan-x-months' value='6' />" +
                "</filter>" +
                "</entity>" +
                "</fetch>";

            var globalContext = Xrm.Utility.getGlobalContext();
            var clientUrl = globalContext.getClientUrl();

            try {
                var url = clientUrl + "/api/data/v9.0/activitypointers?fetchXml=" + encodeURIComponent(fetchXml);
                var activities = Endeavor.formscriptfunctions.fetchJSONResults(url);

                for (var j = 0; j < activities.length; j++) {

                    Xrm.WebApi.deleteRecord("ed_notifymkl", activities[j].activityid).then(
                        function success(result) {
                            console.log(result);
                        },
                        function (error) {
                            console.log(error.message);
                        }
                    );
                }
            } catch (e) {
                console.log(e.message);
            }

            try {
                var url = clientUrl + "/api/data/v9.0/activitypointers?fetchXml=" + encodeURIComponent(fetchXml);
                var activities = Endeavor.formscriptfunctions.fetchJSONResults(url);

                for (var j = 0; j < activities.length; j++) {

                    Xrm.WebApi.deleteRecord("ed_notifymkl", activities[j].activityid).then(
                        function success(result) {
                            console.log(result);
                        },
                        function (error) {
                            console.log(error.message);
                        }
                    );
                }
            } catch (e) {
                console.log(e.message);
            }

            try {
                var url = clientUrl + "/api/data/v9.0/activitypointers?fetchXml=" + encodeURIComponent(fetchXml);
                var activities = Endeavor.formscriptfunctions.fetchJSONResults(url);

                for (var j = 0; j < activities.length; j++) {

                    Xrm.WebApi.deleteRecord("ed_notifymkl", activities[j].activityid).then(
                        function success(result) {
                            console.log(result);
                        },
                        function (error) {
                            console.log(error.message);
                        }
                    );
                }
            } catch (e) {
                console.log(e.message);
            }
           
        },

        onLoad: function () {
            Endeavor.Skanetrafiken.Activity.testActivities();
        }
    };
}