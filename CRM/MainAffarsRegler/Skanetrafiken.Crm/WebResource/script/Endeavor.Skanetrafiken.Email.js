FORM_TYPE_CREATE = 1;
FORM_TYPE_UPDATE = 2;
FORM_TYPE_READONLY = 3;
FORM_TYPE_DISABLED = 4;
FORM_TYPE_QUICKCREATE = 5;
FORM_TYPE_BULKEDIT = 6;

// Begin scoping
if (typeof (Endeavor) == "undefined") {
	var Endeavor = {};
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {
	Endeavor.Skanetrafiken = {};
}

if (typeof (Endeavor.Skanetrafiken.Email) == "undefined") {
	Endeavor.Skanetrafiken.Email = {

		attachmentUrls: "",

		tempFormContext: function () { },

		saveAndSendEmail: function (formContext) {
			debugger;
			formContext.data.save().then(function () {
				Endeavor.Skanetrafiken.Email.SendEmail(formContext);
			}, function () {
				Endeavor.Skanetrafiken.Email.ErrorOnSave(formContext);
			});
		},

		SendEmail: function (formContext) {
			debugger;
			formContext.ui.setFormNotification("Skickar e-post. Vänligen vänta.", "INFO");

			var idRecord = formContext.data.entity.getId().replace("{", "").replace("}", "");

			Endeavor.formscriptfunctions.callAction("new_SendEmailFromRibbon", "email", idRecord, null,
				function () {
					formContext.ui.close();
				},
				function (e) {
					formContext.ui.setFormNotification("Någonting gick fel: " + e.message, "INFO");
					console.error(e.message);
				});
		},

		ErrorOnSave: function (formContext, error) {
			debugger;
			formContext.ui.setFormNotification("Fel vid mejlutskick: " + error);
		},

		//Form Methods CGI Email (from emailLibrary.js)
		onFormLoad: function (executionContext) {

			var formContext = executionContext.getFormContext();

			Endeavor.Skanetrafiken.Email.addLookupFilter(formContext);

			var _form_type = formContext.ui.getFormType();

			switch (formContext.ui.getFormType()) {
				case FORM_TYPE_CREATE:
					Endeavor.Skanetrafiken.Email.removeMailToOnLoad(formContext);
					Endeavor.Skanetrafiken.Email.SetSenderEmail(_form_type, formContext);
					Endeavor.Skanetrafiken.Email.setToFromQuerystringParam(formContext);
					Endeavor.Skanetrafiken.Email.setRegardingObjectidFromQuerystringParam(formContext);
					break;
				case FORM_TYPE_UPDATE:
					Endeavor.Skanetrafiken.Email.SetSenderEmail(_form_type, formContext);
					break;
				case FORM_TYPE_READONLY:
				case FORM_TYPE_DISABLED:
				case FORM_TYPE_QUICKCREATE:
				case FORM_TYPE_BULKEDIT:
					break;
				default:
					alert("Form type error!");
					break;
			}
		},

		setEmailRecipientFocus: function (formContext) {
			formContext.getControl("cgi_email_recipient_id").setFocus();
		},

		removeMailToOnLoad: function (formContext) {
			try {
				if (Endeavor.formscriptfunctions.GetValue("cgi_attention", formContext) == true) {
					//Endeavor.formscriptfunctions.SetValue("from", null);
					Endeavor.formscriptfunctions.SetValue("to", null, formContext);
					Endeavor.formscriptfunctions.SetValue("cc", null, formContext);
					Endeavor.formscriptfunctions.SetValue("bcc", null, formContext);
					Endeavor.formscriptfunctions.HideOrDisplayField("cgi_remittance", false, formContext);
					Endeavor.formscriptfunctions.HideOrDisplayField("cgi_ask_customer", false, formContext);
					Endeavor.formscriptfunctions.HideOrDisplayField("to", false, formContext);
					Endeavor.formscriptfunctions.HideOrDisplayField("cc", false, formContext);
					Endeavor.formscriptfunctions.HideOrDisplayField("bcc", false, formContext);
					Endeavor.formscriptfunctions.HideOrDisplayField("cgi_email_recipient_id", true, formContext);
					Endeavor.formscriptfunctions.HideOrDisplayField("cgi_cc_emailrecipient", true, formContext);
					Endeavor.formscriptfunctions.HideOrDisplayField("cgi_bcc_emailrecipient", true, formContext);
					window.setTimeout(this.setEmailRecipientFocus(formContext), 50);
				} else if (Endeavor.formscriptfunctions.GetValue("cgi_remittance", formContext) == true) {
					Endeavor.formscriptfunctions.SetValue("from", null, formContext);
					Endeavor.formscriptfunctions.SetValue("to", null, formContext);
					Endeavor.formscriptfunctions.SetValue("cc", null, formContext);
					Endeavor.formscriptfunctions.SetValue("bcc", null, formContext);
					Endeavor.formscriptfunctions.HideOrDisplayField("cgi_attention", false, formContext);
					Endeavor.formscriptfunctions.HideOrDisplayField("cgi_ask_customer", false, formContext);
					Endeavor.formscriptfunctions.HideOrDisplayField("to", false, formContext);
					Endeavor.formscriptfunctions.HideOrDisplayField("cc", false, formContext);
					Endeavor.formscriptfunctions.HideOrDisplayField("bcc", false, formContext);
					Endeavor.formscriptfunctions.HideOrDisplayField("cgi_email_recipient_id", true, formContext);
					Endeavor.formscriptfunctions.HideOrDisplayField("cgi_cc_emailrecipient", true, formContext);
					Endeavor.formscriptfunctions.HideOrDisplayField("cgi_bcc_emailrecipient", true, formContext);
					window.setTimeout(this.setEmailRecipientFocus(formContext), 50);
				} else {
					Endeavor.formscriptfunctions.HideOrDisplayField("cgi_attention", false, formContext);
					Endeavor.formscriptfunctions.HideOrDisplayField("cgi_remittance", false, formContext);
				}
			} catch (e) {
				alert("Fel i Endeavor.Skanetrafiken.Email.removeMailToOnLoad\n\n" + e.message);
			}
		},

		set_filelinks_onchange: function (executionContext) {
			try {
				var formContext = executionContext.getFormContext();
				switch (formContext.ui.getFormType()) {
					case FORM_TYPE_CREATE:
						alert("The Email must be saved before attaching attachments!");
						formContext.data.entity.attributes.get("cgi_getfilelink").setValue(false);
						break;
					case FORM_TYPE_UPDATE:
						Endeavor.Skanetrafiken.Email.populateAttachmentArray(formContext);
						break;
					case FORM_TYPE_READONLY:
					case FORM_TYPE_DISABLED:
					case FORM_TYPE_QUICKCREATE:
					case FORM_TYPE_BULKEDIT:
						break;
					default:
						alert("Form type error!");
						break;
				}

				//var _act_value = formContext.data.entity.attributes.get("cgi_getfilelink").getValue();
				//var _attribute = formContext.getAttribute("regardingobjectid");

				//if (_attribute == null) {
				//    alert("Den här e-postadressen har ingen associerad post.");
				//    return;
				//}

				//var _lookup = _attribute.getValue();

				//if (_lookup != null) {
				//    var entity_name = _lookup[0].entityType;
				//    var _incidentid = _lookup[0].id.replace("{", "").replace("}", "");

				//    if (entity_name == "incident" && _act_value == true)
				//        Endeavor.OData_Querys.GetFilelinks(_incidentid, formContext);
				//}
			} catch (e) {
				alert("Fel i Endeavor.Skanetrafiken.KBArticle.set_filelinks_onchange\n\n" + e.message);
			}
		},

		set_filelinks_onchange_callback: function (result, formContext) {
			try {
				if (result == null)
					alert("Det finns inga bifogade filer kopplade till ärendet");
				else if (result.entities.length == 0)
					alert("Det finns inga bifogade filer kopplade till ärendet");
				else {
					var _cgi_Url1 = "";
					for (var i = 0; i < result.entities.length; i++) {
						if (i == 0)
							_cgi_Url1 = formContext.getAttribute("description").getValue() + '<br />' + result.entities[i].cgi_url;
						else
							_cgi_Url1 += '<br />' + result.entities[i].cgi_url;
					}
					formContext.getAttribute("description").setValue(_cgi_Url1);
				}
			} catch (e) {
				alert("Fel i Endeavor.Skanetrafiken.Email.set_filelinks_onchange_callback\n\n" + e.message);
			}
		},

		set_to_onchange: function (executionContext) {
			try {
				var formContext = executionContext.getFormContext();

				var _id = Endeavor.formscriptfunctions.GetLookupid("cgi_email_recipient_id", formContext);
				var _name = Endeavor.formscriptfunctions.GetLookupName("cgi_email_recipient_id", formContext);
				var _logicalname = "cgi_emailrecipient";

				Endeavor.formscriptfunctions.SetLookup("to", _logicalname, _id, _name, formContext);
				formContext.getControl("subject").setFocus();
			} catch (e) {
				alert("Fel i Endeavor.Skanetrafiken.Email.set_to_onchange\n\n" + e.message);
			}
		},

		set_cc_onchange: function (executionContext) {
			try {
				var formContext = executionContext.getFormContext();

				var id = Endeavor.formscriptfunctions.GetLookupid("cgi_cc_emailrecipient", formContext),
					name = Endeavor.formscriptfunctions.GetLookupName("cgi_cc_emailrecipient", formContext),
					logicalName = "cgi_emailrecipient";

				Endeavor.formscriptfunctions.SetLookup("cc", logicalName, id, name, formContext);
			} catch (e) {
				alert("Fel i Endeavor.Skanetrafiken.Email.set_cc_onchange\n\n" + e.message);
			}
		},

		set_bcc_onchange: function (executionContext) {
			try {
				var formContext = executionContext.getFormContext();

				var id = Endeavor.formscriptfunctions.GetLookupid("cgi_bcc_emailrecipient", formContext),
					name = Endeavor.formscriptfunctions.GetLookupName("cgi_bcc_emailrecipient", formContext),
					logicalName = "cgi_emailrecipient";

				Endeavor.formscriptfunctions.SetLookup("bcc", logicalName, id, name, formContext);
			} catch (e) {
				alert("Fel i Endeavor.Skanetrafiken.Email.set_bcc_onchange\n\n" + e.message);
			}
		},

		addLookupFilter: function (formContext) {

			try {
				//////// Email Recipient

				// use randomly generated GUID Id for the view
				var viewId = "{0CBC820C-7033-4AFF-9CE8-FB610464DBD3}";
				var entityName = "cgi_emailrecipient";

				// give the custom view a name
				var viewDisplayName = "Epost";

				var _fetch = "";
				_fetch += "<fetch version='1.0' mapping='logical' distinct='false'>";
				_fetch += "<entity name='cgi_emailrecipient'>";
				_fetch += "<attribute name='cgi_emailrecipientid' />";
				_fetch += "<attribute name='cgi_emailrecipientname' />";
				_fetch += "<attribute name='cgi_emailgroupid' />";
				_fetch += "<attribute name='cgi_role' />";
				_fetch += "<attribute name='cgi_emailaddress' />";
				_fetch += "</entity>";
				_fetch += "</fetch>";

				// build Grid Layout
				var _layoutXml = "<grid name='resultset' " +
					"object='10014' " +
					"jump='cgi_emailrecipientid' " +
					"select='1' " +
					"icon='0' " +
					"preview='0'>" +
					"<row name='result' id='cgi_emailrecipientid'>" +
					"<cell name='cgi_emailrecipientname' width='200' />" +
					"<cell name='cgi_emailgroupid' width='200' />" +
					"<cell name='cgi_role' width='200' />" +
					"<cell name='cgi_emailaddress' width='200' />" +
					"</row>" +
					"</grid>";

				// add the Custom View to the indicated [lookupFieldName] Control
				formContext.getControl("to").addCustomView(viewId, entityName, viewDisplayName, _fetch, _layoutXml, true);

			} catch (e) {
				alert("Fel i Endeavor.Skanetrafiken.Email.addLookupFilter\n\n" + e.message);
			}
		},

		setRegardingObjectidFromQuerystringParam: function (formContext) {
			try {
				var _xrmObject = formContext.context.getQueryStringParameters();
				var _caseId = _xrmObject["parameter_regardingid"];
				var _caseName = _xrmObject["parameter_regardingname"];
				var _entityType = _xrmObject["parameter_regardingtype"];

				if (_caseId == "undefined")
					return;

				if (_caseId != null && _caseName != null && _entityType != null)
					Endeavor.formscriptfunctions.SetLookup("regardingobjectid", _entityType, _caseId, _caseName, formContext);
			} catch (e) {
				alert("Fel i Endeavor.Skanetrafiken.Email.getQuerystringParam\n\n" + e.message);
			}
		},

		setToFromQuerystringParam: function (formContext) {
			try {
				var _xrmObject = formContext.context.getQueryStringParameters();
				var _accountId = _xrmObject["parameter_customerid"];
				var _accountName = _xrmObject["parameter_customername"];
				var _entityType = _xrmObject["parameter_customertype"];

				if (_accountId == "undefined")
					return;

				if (_accountId != null && _accountName != null && _entityType != null)
					Endeavor.formscriptfunctions.SetLookup("to", _entityType, _accountId, _accountName, formContext);
			} catch (e) {
				alert("Fel i Endeavor.Skanetrafiken.Email.getQuerystringParam\n\n" + e.message);
			}
		},

		SetSenderEmail: function (_form_type, formContext) {
			try {
				var globalContext = Xrm.Utility.getGlobalContext();

				var _directioncode = formContext.getAttribute("directioncode").getValue();
				var _emailstatus = formContext.data.entity.attributes.get('statuscode').getValue();
				var _infomail = formContext.data.entity.attributes.get('cgi_attention').getValue();
				var _tocustomer = formContext.data.entity.attributes.get('cgi_button_customer').getValue();

				if (_infomail == false && _tocustomer != 1) {
					if (_form_type == 1 || (_form_type == 2 && _emailstatus == 1 && _directioncode == true)) {
						var userId = globalContext.userSettings.userId.replace("{", "").replace("}", "");
						Endeavor.OData_Querys.GetDefaultQueue(userId, formContext);
					}
				}

				if (_tocustomer == '1' && !Endeavor.formscriptfunctions.GetValue("cgi_ask_customer", formContext)) {
					var _currentdate = Endeavor.formscriptfunctions.GetDateTime();
					Endeavor.OData_Querys.GetDefaultUserFromSetting(_currentdate, formContext);
				}
			} catch (e) {
				alert("Fel i Endeavor.Skanetrafiken.Email.SetSenderEmail\n\n" + e.message);
			}
		},

		SetSenderEmail_callback: function (result, formContext) {
			try {
				debugger;
				if (result == null || result.entities == null || result.entities[0] == null) {
					alert("Ingen default kö är definierad på användaren!");
				} else {
					_queueId = result.entities[0]["_queueid_value"];
					_queueLogicalName = result.entities[0]["_queueid_value@Microsoft.Dynamics.CRM.lookuplogicalname"];
					_queueName = result.entities[0]["_queueid_value@OData.Community.Display.V1.FormattedValue"];

					// Make sure field is changed.
					// Unchanged = Do nothing
					var fromId = Endeavor.formscriptfunctions.GetLookupid("from", formContext);
					if (fromId != null)
						if (fromId.toUpperCase() == ("{" + _queueId + "}").toUpperCase())
							return;

					Endeavor.formscriptfunctions.SetLookup("from", _queueLogicalName, _queueId, _queueName, formContext);
				}
			} catch (e) {
				alert("Fel i Endeavor.Skanetrafiken.Email.SetSenderEmail_callback\n\n" + e.message);
			}
		},

		SetSenderEmailNoReply_callback: function (result, formContext) {
			try {
				if (result == null || result.entities == null || result.entities[0] == null) {
					alert("Ingen default användare för noreplay adress är definierad!");
				} else {
					_userId = result.entities[0]["_cgi_userid_value"];
					_userLogicalName = result.entities[0]["_cgi_userid_value@Microsoft.Dynamics.CRM.lookuplogicalname"];
					_userName = result.entities[0]["_cgi_userid_value@OData.Community.Display.V1.FormattedValue"];

					Endeavor.formscriptfunctions.SetLookup("from", _userLogicalName, _userId, _userName, formContext);
				}
			} catch (e) {
				alert("Fel i Endeavor.Skanetrafiken.Email.SetSenderEmailNoReply_callback\n\n" + e.message);
			}
		},

		AskCustomer_onchange: function (executionContext) {
			var formContext = executionContext.getFormContext();
			var globalContext = Xrm.Utility.getGlobalContext();

			var _askcustomer = formContext.data.entity.attributes.get('cgi_ask_customer').getValue();
			if (_askcustomer == true) {
				var userId = globalContext.userSettings.userId.replace("{", "").replace("}", "");;
				Endeavor.OData_Querys.GetDefaultQueue(userId, formContext);
			} else {
				var _currentdate = Endeavor.formscriptfunctions.GetDateTime();
				Endeavor.OData_Querys.GetDefaultUserFromSetting(_currentdate, formContext);
			}
		},

		populateAttachmentArray: function (formContext) {
			try {
				debugger;
				//var formContext = executionContext.getFormContext();
				Endeavor.Skanetrafiken.Email.attachmentUrls = formContext.data.entity.getId();
				Endeavor.Skanetrafiken.Email.tempFormContext = formContext;

				//Get a list of all the attachment URLS
				var incAtr = formContext.getAttribute("regardingobjectid").getValue();
				var getFilelinkAtr = formContext.getAttribute("cgi_getfilelink").getValue();

				if (getFilelinkAtr != undefined && getFilelinkAtr != false && incAtr != undefined && incAtr != null && incAtr.length > 0) {
					var incidentId = incAtr[0].id.replace("{", "").replace("}", "");

					var fetchXml = [
						"<fetch>",
						"  <entity name='cgi_filelink'>",
						"    <attribute name='cgi_url' />",
						"    <attribute name='cgi_incidentid' />",
						"    <filter type='and'>",
						"      <condition attribute='cgi_incidentid' operator='eq' value='", incidentId, "'/>",
						"    </filter>",
						"  </entity>",
						"</fetch>",
					].join("");

					fetchXml = "?fetchXml=" + fetchXml;

					Xrm.WebApi.retrieveMultipleRecords("cgi_filelink", fetchXml).then(
						function success(result) {
							debugger;
							if (result.entities.length > 0) {
								//Loop
								if (confirm("Vill du hämta och infoga bilagor från ärende?") == true) {

									Xrm.Utility.showProgressIndicator("Hämtar bilagor...");

									var fileLink = "";
									var fileLinkString = "";
									var fileLinkArray = [];
									for (var i = 0; i < result.entities.length; i++) {

										var cgiUrlString = "";

										if (result.entities[i].cgi_url != undefined && result.entities[i].cgi_url != "") {

											fileLink = result.entities[i].cgi_url;

											//Temp string 
											cgiUrlString = result.entities[i].cgi_url;
											//If old Fileshare - open as usual
											if (cgiUrlString.startsWith("https://www.skanetrafiken.se/CRMFiles") ||
												cgiUrlString.startsWith("http://www.skanetrafiken.se/CRMFiles") ||
												cgiUrlString.startsWith("https://dkcmsprod.skanetrafiken.se/CRMFiles/")) {

												//do something else - Update the emails text with the attachment text from before
												var _cgi_Url1 = "";
												if (i == 0)
													_cgi_Url1 = formContext.getAttribute("description").getValue() + '<br />' + result.entities[i].cgi_url;
												else
													_cgi_Url1 += '<br />' + result.entities[i].cgi_url;
												formContext.getAttribute("description").setValue(_cgi_Url1);
											} else {
												debugger;
												if (cgiUrlString.startsWith("https:")) {
													if (cgiUrlString.startsWith("https://webpublicwebacc.blob.core.windows.net/")) { //https://webpublicwebprod.blob.core.windows.net/ - https://webpublicwebacc.blob.core.windows.net/
														fileLinkString += cgiUrlString.split("https://webpublicwebacc.blob.core.windows.net/").pop(); //https://webpublicwebprod.blob.core.windows.net/ - https://webpublicwebacc.blob.core.windows.net/
													} else {
														fileLinkString += cgiUrlString.split("https://").pop();
													}
												} else if (cgiUrlString.startsWith("http:")) {
													if (cgiUrlString.startsWith("http://webpublicwebacc.blob.core.windows.net/")) { //https://webpublicwebprod.blob.core.windows.net/ - http://webpublicwebacc.blob.core.windows.net/
														fileLinkString += cgiUrlString.split("http://webpublicwebacc.blob.core.windows.net/").pop(); //https://webpublicwebprod.blob.core.windows.net/ - http://webpublicwebacc.blob.core.windows.net/
													} else {
														fileLinkString += cgiUrlString.split("http://").pop();
													}
												} else {
													fileLinkString += cgiUrlString;
												}
											}
										}

										if (fileLinkString != undefined && fileLinkString != "" && cgiUrlString != undefined && cgiUrlString != "") {
											fileLinkString += ";";
										}
									}

									//Add the current email guid to the string
									if (Endeavor.Skanetrafiken.Email.attachmentUrls != "") {
										fileLinkString += Endeavor.Skanetrafiken.Email.attachmentUrls.replace("{", "").replace("}", "");
									} else {
										fileLinkString += "EMAILGUID";
									}
									console.log("File Link String: " + fileLinkString);

									if ((cgiUrlString.startsWith("https://www.skanetrafiken.se/CRMFiles") == false) &&
										(cgiUrlString.startsWith("http://www.skanetrafiken.se/CRMFiles") == false) &&
										(cgiUrlString.startsWith("https://dkcmsprod.skanetrafiken.se/CRMFiles/") == false)) {
										var inputParameters = [{
											"Field": "EncryptedString",
											"Value": fileLinkString,
											"TypeName": "Edm.String",
											"StructuralProperty": 1
										}];

										Endeavor.Skanetrafiken.Email.callGlobalAction("ed_DecryptAttachmentFile", inputParameters,
											function (result) {
												debugger;
												Xrm.Utility.closeProgressIndicator();
												if (result != null && result != "undefined") {
													console.log("Result: " + result.responseText);
												}

												var parsedResult = JSON.parse(result.responseText);
												if (parsedResult.Result != undefined && parsedResult.Result != "") {

													if (parsedResult.Result.startsWith("Kunde inte hämta filen")) {
														var confirmaError = confirm(parsedResult.Result);
														if (confirmaError) {
															console.log("Confirmed: " + parsedResult.Result);
														} else {
															console.log("Confirmed: " + parsedResult.Result);
														}
													} else {
														debugger;
														var failedCount = "";
														if (parsedResult.Result.startsWith("Created"))
														{
															failedCount = parsedResult.Result.split(":").pop();
															if (failedCount != "0")
															{
																if (failedCount == "1") {
																	Endeavor.Skanetrafiken.Email.tempFormContext.ui.setFormNotification(failedCount + " bilaga gick inte att hämta.", "INFO");
																}
																else
																{
																	Endeavor.Skanetrafiken.Email.tempFormContext.ui.setFormNotification(failedCount + " bilagor gick inte att hämta.", "INFO");
																}
															}
														}
														Endeavor.Skanetrafiken.Email.tempFormContext.data.entity.save();
													}
												}
											},
											function (e) {
												// Error
												debugger;
												Xrm.Utility.closeProgressIndicator();
												var confirmationAttachment = confirm("Filen kunde ej hämtas. Execution returned: " + e.message);
												if (confirmationAttachment) {
													console.log("Confirmed error: " + e.message);
												} else {
													console.log("Confirmed error: " + e.message);
												}

												if (window.console && console.error)
													console.error(e.message + "\n" + t);
											});
									} else {
										//debugger;
										Xrm.Utility.closeProgressIndicator();
									}
								} else {
									//Empty Get Filelink Field
									Endeavor.Skanetrafiken.Email.tempFormContext.getAttribute("cgi_getfilelink").setValue(false);
								}
							}
						},
						function (error) {
							debugger;
							Xrm.Utility.closeProgressIndicator();
							alert("Fel i Endeavor.Skanetrafiken.Email.populateAttachmentArray\n\n" + error.message);
							console.log(error.message);
						}
					);
				}
			} catch (e) {
				Xrm.Utility.closeProgressIndicator();
				alert("Fel i Endeavor.Skanetrafiken.Email.populateAttachmentArray\n\n" + e.message);
			}
		},

		downloadAttachmentFiles: function (formContext) {


		},

		callGlobalAction: function (actionName, inputParameters, sucessCallback, errorCallback) {

			var req = {};

			var parameterTypes = {};
			if (inputParameters != null)
				for (var i = 0; i < inputParameters.length; i++) {
					var parameter = inputParameters[i];

					req[parameter.Field] = parameter.Value;
					parameterTypes[parameter.Field] = {
						"typeName": parameter.TypeName,
						"structuralProperty": parameter.StructuralProperty
					};
				}

			req.getMetadata = function () {

				return {
					boundParameter: null,
					parameterTypes: parameterTypes,
					operationType: 0,
					operationName: actionName
				};
			};

			if (typeof (Xrm) == "undefined")
				Xrm = parent.Xrm;

			Xrm.WebApi.online.execute(req).then(sucessCallback, errorCallback);
		}
	}
}