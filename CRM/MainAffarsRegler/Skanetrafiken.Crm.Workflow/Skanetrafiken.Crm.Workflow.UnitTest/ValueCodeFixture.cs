using Microsoft.Crm.Sdk.Samples;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using NUnit.Framework;
using Skanetrafiken.Crm;
using Skanetrafiken.Crm.Entities;
using Skanetrafiken.Crm.ValueCodes;
using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using static Skanetrafiken.Crm.ValueCodes.ValueCodeHandler;
using Generated = Skanetrafiken.Crm.Schema.Generated;

namespace Endeavor.Crm.UnitTest
{
    public class WFReturnObject
    {
        public EntityReference valueCodeRef { get; set; }
        public string returnMessage { get; set; }
    }

    [TestClass]
    public class ValueCodeFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;



        [Test, Category("Debug")]
        public void FullFlowValueCodes()
        {

            try
            {

                // Connect to the Organization service. 
                // The using statement assures that the service proxy will be properly disposed.
                using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
                {
                    string testInstanceName = GetUnitTestID();

                    // This statement is required to enable early-bound type support.
                    _serviceProxy.EnableProxyTypes();

                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                    string travelCardNumber = "1136632691";
                    //ContactEntity contact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, new Guid("7FDA72A0-8689-E811-80EF-005056B61FFF"), new ColumnSet(true));

                    ContactEntity contact = CreateOrRetrieveRGOLTestContact(localContext, "marcus.stenswed@endeavor.se", "+46735198846", testInstanceName);

                    IncidentEntity incident = CreateRGOLTestCase(localContext, contact, testInstanceName);

                    RefundEntity refund = CreateRGOLTestRefund(localContext, incident, contact, testInstanceName);

                    incident.CloseCase(localContext, testInstanceName);

                    #region Förseningsersättning (Value Code)

                    #region Should Create and Send a Value Code without errors (email and sms)
                    WFReturnObject valueCodeForseningsersattningEmail1 = CallWorkflowCreateValueCodeAction(
                        localContext,
                        (int)Generated.ed_valuecodetypeglobal.Forseningsersattning,
                        refund.cgi_Amount.Value,
                        contact.Telephone2,
                        (contact.EMailAddress1 != null) ? contact.EMailAddress1 : contact.EMailAddress2,
                        refund.ToEntityReference(),
                        null,
                        contact.ToEntityReference(),
                        null,
                        (int)Generated.ed_valuecodedeliverytypeglobal.Email
                        );

                    NUnit.Framework.Assert.IsNotNull(valueCodeForseningsersattningEmail1.returnMessage);
                    NUnit.Framework.StringAssert.Contains("OK", valueCodeForseningsersattningEmail1.returnMessage);
                    NUnit.Framework.Assert.IsNotNull(valueCodeForseningsersattningEmail1.valueCodeRef);
                    NUnit.Framework.Assert.IsNotNull(XrmRetrieveHelper.Retrieve<ValueCodeEntity>(localContext, valueCodeForseningsersattningEmail1.valueCodeRef.Id, new ColumnSet(false)));

                    // Send Value Code (Förseningsersättning)
                    string resultSendEmail1 = CallWorkflowSendValueCode(localContext, valueCodeForseningsersattningEmail1.valueCodeRef);

                    NUnit.Framework.Assert.AreEqual("Value code was sent.", resultSendEmail1);


                    WFReturnObject valueCodeForseningsersattningSMS1 = CallWorkflowCreateValueCodeAction(
                        localContext,
                        (int)Generated.ed_valuecodetypeglobal.Forseningsersattning,
                        refund.cgi_Amount.Value,
                        contact.Telephone2,
                        (contact.EMailAddress1 != null) ? contact.EMailAddress1 : contact.EMailAddress2,
                        refund.ToEntityReference(),
                        null,
                        contact.ToEntityReference(),
                        null,
                        (int)Generated.ed_valuecodedeliverytypeglobal.SMS
                        );

                    NUnit.Framework.Assert.IsNotNull(valueCodeForseningsersattningSMS1.returnMessage);
                    NUnit.Framework.StringAssert.Contains("OK", valueCodeForseningsersattningSMS1.returnMessage);
                    NUnit.Framework.Assert.IsNotNull(valueCodeForseningsersattningSMS1.valueCodeRef);
                    NUnit.Framework.Assert.IsNotNull(XrmRetrieveHelper.Retrieve<ValueCodeEntity>(localContext, valueCodeForseningsersattningSMS1.valueCodeRef.Id, new ColumnSet(false)));

                    // Send Value Code (Förseningsersättning)
                    string resultSendSMS1 = CallWorkflowSendValueCode(localContext, valueCodeForseningsersattningSMS1.valueCodeRef);

                    NUnit.Framework.Assert.AreEqual("Value code was sent.", resultSendSMS1);
                    #endregion Should Create and Send a Value Code without errors (email and sms)

                    #region Bad Mobile Format. Should not Create Value Code and should reopen Case and insert error message on Refund (cgi_errormessage)
                    ContactEntity updateContact = new ContactEntity()
                    {
                        Telephone2 = "043563265",
                        Id = contact.Id,
                        ed_InformationSource = Generated.ed_informationsource.AdmAndraKund
                    };
                    XrmHelper.Update(localContext, updateContact);
                    contact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, contact.Id, new ColumnSet(true));

                    IncidentEntity incident2 = CreateRGOLTestCase(localContext, contact, testInstanceName);

                    RefundEntity refund2 = CreateRGOLTestRefund(localContext, incident2, contact, testInstanceName);

                    incident2.CloseCase(localContext, testInstanceName);

                    WFReturnObject valueCodeForseningsersattning2 = CallWorkflowCreateValueCodeAction(
                        localContext,
                        (int)Generated.ed_valuecodetypeglobal.Forseningsersattning,
                        refund.cgi_Amount.Value,
                        contact.Telephone2,
                        (contact.EMailAddress1 != null) ? contact.EMailAddress1 : contact.EMailAddress2,
                        refund2.ToEntityReference(),
                        null,
                        contact.ToEntityReference(),
                        null,
                        (int)Generated.ed_valuecodedeliverytypeglobal.SMS
                        );

                    // TODO (Marcus) - Why does not the correct error messagee returns and make sure case i reopened
                    NUnit.Framework.StringAssert.Contains("Bad format on phone number.", valueCodeForseningsersattning2.returnMessage);
                    NUnit.Framework.Assert.IsNull(valueCodeForseningsersattning2.valueCodeRef);

                    incident2 = XrmRetrieveHelper.Retrieve<IncidentEntity>(localContext, incident2.Id, new ColumnSet(IncidentEntity.Fields.StateCode));
                    NUnit.Framework.Assert.AreEqual(incident2.StateCode, Generated.IncidentState.Active);

                    refund2 = XrmRetrieveHelper.Retrieve<RefundEntity>(localContext, refund2.Id, new ColumnSet(RefundEntity.Fields.cgi_errormessage));
                    NUnit.Framework.Assert.IsNotNull(refund2.cgi_errormessage);
                    #endregion Bad Mobile Format. Should not Create Value Code and should reopen Case and insert error message on Refund (cgi_errormessage)

                    #region Bad Email Format. Should not Create Value Code and should reopen Case and insert error message on Refund (cgi_errormessage)
                    //updateContact = new ContactEntity()
                    //{
                    //    Telephone2 = "0735198846",
                    //    EMailAddress2 = "test-email" + testInstanceName,
                    //    EMailAddress1 = null,
                    //    Id = contact.Id,
                    //    ed_InformationSource = Generated.ed_informationsource.AdmAndraKund
                    //};
                    //XrmHelper.Update(localContext, updateContact);
                    //contact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, contact.Id, new ColumnSet(true));

                    //incident2.CloseCase(localContext, testInstanceName);

                    //WFReturnObject valueCodeForseningsersattning3 = CallWorkflowCreateValueCodeAction(
                    //    localContext,
                    //    (int)Generated.ed_valuecodetypeglobal.Forseningsersattning,
                    //    refund.cgi_Amount.Value,
                    //    contact.Telephone2,
                    //    (contact.EMailAddress1 != null) ? contact.EMailAddress1 : contact.EMailAddress2,
                    //    refund2.ToEntityReference(),
                    //    null,
                    //    contact.ToEntityReference(),
                    //    null,
                    //    (int)Generated.ed_valuecodedeliverytypeglobal.Email
                    //    );

                    //// TODO (Marcus) - Why does not the correct error messagee returns and make sure case i reopened
                    //NUnit.Framework.StringAssert.Contains("Bad format on email.", valueCodeForseningsersattning3.returnMessage);
                    //NUnit.Framework.Assert.IsNull(valueCodeForseningsersattning3.valueCodeRef);

                    //incident2 = XrmRetrieveHelper.Retrieve<IncidentEntity>(localContext, incident2.Id, new ColumnSet(IncidentEntity.Fields.StateCode));
                    //NUnit.Framework.Assert.AreEqual(incident2.StateCode, Generated.IncidentState.Active);

                    //refund2 = XrmRetrieveHelper.Retrieve<RefundEntity>(localContext, refund2.Id, new ColumnSet(RefundEntity.Fields.cgi_errormessage));
                    //NUnit.Framework.Assert.IsNotNull(refund2.cgi_errormessage);
                    #endregion Bad Email Format. Should not Create Value Code and should reopen Case and insert error message on Refund (cgi_errormessage)

                    #region Change to bad format on ValueCode in CRM before sending
                    updateContact = new ContactEntity()
                    {
                        Telephone2 = "0735198846",
                        EMailAddress2 = "marcus.stenswed@endeavor.se",
                        EMailAddress1 = null,
                        Id = contact.Id,
                        ed_InformationSource = Generated.ed_informationsource.AdmAndraKund
                    };
                    XrmHelper.Update(localContext, updateContact);
                    contact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, contact.Id, new ColumnSet(true));

                    incident2.CloseCase(localContext, testInstanceName);

                    #region SMS
                    WFReturnObject valueCodeForseningsersattningSMS2 = CallWorkflowCreateValueCodeAction(
                        localContext,
                        (int)Generated.ed_valuecodetypeglobal.Forseningsersattning,
                        refund.cgi_Amount.Value,
                        contact.Telephone2,
                        (contact.EMailAddress1 != null) ? contact.EMailAddress1 : contact.EMailAddress2,
                        refund.ToEntityReference(),
                        null,
                        contact.ToEntityReference(),
                        null,
                        (int)Generated.ed_valuecodedeliverytypeglobal.SMS
                        );

                    // Should not work (not a valid mobile)
                    ValueCodeEntity valueCodeSMS = XrmRetrieveHelper.Retrieve<ValueCodeEntity>(localContext, valueCodeForseningsersattningSMS2.valueCodeRef.Id,
                        new ColumnSet(
                            true
                        ));
                    valueCodeSMS.ed_MobileNumber = "0633694422";
                    XrmHelper.Update(localContext, valueCodeSMS);

                    valueCodeSMS.SendValueCode(localContext);

                    string resultSendSMS2 = CallWorkflowSendValueCode(localContext, valueCodeForseningsersattningSMS2.valueCodeRef);

                    RefundEntity refundSMS2 = XrmRetrieveHelper.Retrieve<RefundEntity>(localContext, valueCodeSMS.ed_Refund.Id,
                        new ColumnSet(
                            RefundEntity.Fields.cgi_errormessage
                        ));
                    //NUnit.Framework.Assert.AreNotEqual("Value code was sent.", resultSendSMS2);
                    //NUnit.Framework.Assert.IsNotNull(refundSMS2.cgi_errormessage);
                    refundSMS2.cgi_errormessage = null;
                    XrmHelper.Update(localContext, refundSMS2);


                    // Should not work (not a valid mobile)
                    valueCodeSMS = XrmRetrieveHelper.Retrieve<ValueCodeEntity>(localContext, valueCodeForseningsersattningSMS2.valueCodeRef.Id, new ColumnSet(false));
                    valueCodeSMS.ed_MobileNumber = "0316478645";
                    XrmHelper.Update(localContext, valueCodeSMS);

                    string resultSendSMS3 = CallWorkflowSendValueCode(localContext, valueCodeForseningsersattningSMS2.valueCodeRef);

                    RefundEntity refundSMS3 = XrmRetrieveHelper.Retrieve<RefundEntity>(localContext, valueCodeSMS.ed_Refund.Id,
                        new ColumnSet(
                            RefundEntity.Fields.cgi_errormessage
                        ));
                    //NUnit.Framework.Assert.AreNotEqual("Value code was sent.", resultSendSMS3);
                    //NUnit.Framework.Assert.IsNotNull(refundSMS3.cgi_errormessage);
                    refundSMS3.cgi_errormessage = null;
                    XrmHelper.Update(localContext, refundSMS3);


                    // Should work (valid mobile)
                    valueCodeSMS = XrmRetrieveHelper.Retrieve<ValueCodeEntity>(localContext, valueCodeForseningsersattningSMS2.valueCodeRef.Id, new ColumnSet(ValueCodeEntity.Fields.ed_Refund));
                    valueCodeSMS.ed_MobileNumber = "0735198846";
                    XrmHelper.Update(localContext, valueCodeSMS);

                    string resultSendSMS4 = CallWorkflowSendValueCode(localContext, valueCodeForseningsersattningSMS2.valueCodeRef);

                    RefundEntity refundSMS4 = XrmRetrieveHelper.Retrieve<RefundEntity>(localContext, valueCodeSMS.ed_Refund.Id,
                        new ColumnSet(
                            RefundEntity.Fields.cgi_errormessage
                        ));
                    NUnit.Framework.Assert.AreEqual("Value code was sent.", resultSendSMS4);
                    NUnit.Framework.Assert.IsNull(refundSMS4.cgi_errormessage);


                    // Should work (valid mobile)
                    valueCodeSMS = XrmRetrieveHelper.Retrieve<ValueCodeEntity>(localContext, valueCodeForseningsersattningSMS2.valueCodeRef.Id, new ColumnSet(ValueCodeEntity.Fields.ed_Refund));
                    valueCodeSMS.ed_MobileNumber = "0046735198846";
                    XrmHelper.Update(localContext, valueCodeSMS);

                    string resultSendSMS5 = CallWorkflowSendValueCode(localContext, valueCodeForseningsersattningSMS2.valueCodeRef);

                    RefundEntity refundSMS5 = XrmRetrieveHelper.Retrieve<RefundEntity>(localContext, valueCodeSMS.ed_Refund.Id,
                        new ColumnSet(
                            RefundEntity.Fields.cgi_errormessage
                        ));
                    NUnit.Framework.Assert.AreEqual("Value code was sent.", resultSendSMS5);
                    NUnit.Framework.Assert.IsNull(refundSMS5.cgi_errormessage);
                    #endregion SMS

                    #region Email
                    WFReturnObject valueCodeForseningsersattningEmail2 = CallWorkflowCreateValueCodeAction(
                       localContext,
                       (int)Generated.ed_valuecodetypeglobal.Forseningsersattning,
                       refund.cgi_Amount.Value,
                       contact.Telephone2,
                       (contact.EMailAddress1 != null) ? contact.EMailAddress1 : contact.EMailAddress2,
                       refund.ToEntityReference(),
                       null,
                       contact.ToEntityReference(),
                       null,
                       (int)Generated.ed_valuecodedeliverytypeglobal.Email
                       );

                    // Should not work (not a valid email)
                    ValueCodeEntity valueCodeEmail = XrmRetrieveHelper.Retrieve<ValueCodeEntity>(localContext, valueCodeForseningsersattningEmail2.valueCodeRef.Id,
                        new ColumnSet(
                            ValueCodeEntity.Fields.ed_Refund
                        ));
                    valueCodeEmail.ed_Email = "marcus.stenswed@test";
                    XrmHelper.Update(localContext, valueCodeEmail);

                    string resultSendEmail2 = CallWorkflowSendValueCode(localContext, valueCodeForseningsersattningEmail2.valueCodeRef);

                    RefundEntity refundEmail2 = XrmRetrieveHelper.Retrieve<RefundEntity>(localContext, valueCodeEmail.ed_Refund.Id,
                        new ColumnSet(
                            RefundEntity.Fields.cgi_errormessage
                        ));
                    NUnit.Framework.Assert.AreNotEqual("Value code was sent.", resultSendSMS2);
                    NUnit.Framework.Assert.IsNotNull(refundEmail2.cgi_errormessage);
                    refundEmail2.cgi_errormessage = null;
                    XrmHelper.Update(localContext, refundEmail2);


                    // Should work (valid email)
                    valueCodeEmail = XrmRetrieveHelper.Retrieve<ValueCodeEntity>(localContext, valueCodeForseningsersattningEmail2.valueCodeRef.Id,
                        new ColumnSet(
                            ValueCodeEntity.Fields.ed_Refund
                        ));
                    valueCodeEmail.ed_Email = "marcus.stenswed@endeavor.se";
                    XrmHelper.Update(localContext, valueCodeEmail);

                    string resultSendEmail3 = CallWorkflowSendValueCode(localContext, valueCodeForseningsersattningEmail2.valueCodeRef);

                    RefundEntity refundEmail3 = XrmRetrieveHelper.Retrieve<RefundEntity>(localContext, valueCodeEmail.ed_Refund.Id,
                        new ColumnSet(
                            RefundEntity.Fields.cgi_errormessage
                        ));
                    NUnit.Framework.Assert.AreEqual("Value code was sent.", resultSendSMS3);
                    NUnit.Framework.Assert.IsNull(refundEmail3.cgi_errormessage);
                    #endregion Email

                    #endregion Change to bad format on ValueCode in CRM before sending

                    #endregion Förseningsersättning (Value Code)

                    #region Ersättning (Value Code)
                    WFReturnObject valueCodeErsattning = CallWorkflowCreateValueCodeAction(
                        localContext,
                        (int)Generated.ed_valuecodetypeglobal.Ersattningsarende,
                        refund.cgi_Amount.Value,
                        contact.Telephone2,
                        (contact.EMailAddress1 != null) ? contact.EMailAddress1 : contact.EMailAddress2,
                        refund.ToEntityReference(),
                        null,
                        contact.ToEntityReference(),
                        null,
                        (int)Generated.ed_valuecodedeliverytypeglobal.Email
                        );

                    NUnit.Framework.Assert.IsNull(valueCodeErsattning.returnMessage);
                    NUnit.Framework.Assert.IsNotNull(valueCodeErsattning.valueCodeRef);
                    NUnit.Framework.Assert.IsNotNull(XrmRetrieveHelper.Retrieve<ValueCodeEntity>(localContext, valueCodeErsattning.valueCodeRef.Id, new ColumnSet(false)));
                    #endregion Ersättning (Value Code)

                    #region Förlustgaranti (Value Code)
                    WFReturnObject valueCodeForlustgaranti = CallWorkflowCreateValueCodeAction(
                        localContext,
                        (int)Generated.ed_valuecodetypeglobal.Forlustgaranti,
                        //refund.cgi_Amount.Value,
                        0,
                        contact.Telephone2,
                        (contact.EMailAddress1 != null) ? contact.EMailAddress1 : contact.EMailAddress2,
                        refund.ToEntityReference(),
                        null,
                        contact.ToEntityReference(),
                        null,
                        (int)Generated.ed_valuecodedeliverytypeglobal.Email
                        );

                    NUnit.Framework.Assert.IsNull(valueCodeForlustgaranti.returnMessage);
                    NUnit.Framework.Assert.IsNotNull(valueCodeForlustgaranti.valueCodeRef);
                    NUnit.Framework.Assert.IsNotNull(XrmRetrieveHelper.Retrieve<ValueCodeEntity>(localContext, valueCodeForlustgaranti.valueCodeRef.Id, new ColumnSet(false)));

                    // TODO - Marcus
                    // Create more tests for blocking card and creating valuecode etc.

                    #endregion Förlustgaranti (Value Code)

                    #region Presentkort - Spärra kort och få värdekod (Value Code)
                    WFReturnObject valueCodePresentKort = CallWorkflowCreateValueCodeAction(
                        localContext,
                        (int)Generated.ed_valuecodetypeglobal.InlostReskassa,
                        refund.cgi_Amount.Value,
                        contact.Telephone2,
                        (contact.EMailAddress1 != null) ? contact.EMailAddress1 : contact.EMailAddress2,
                        refund.ToEntityReference(),
                        null,
                        contact.ToEntityReference(),
                        null,
                        (int)Generated.ed_valuecodedeliverytypeglobal.Email
                        );

                    NUnit.Framework.Assert.IsNull(valueCodePresentKort.returnMessage);
                    NUnit.Framework.Assert.IsNotNull(valueCodePresentKort.valueCodeRef);
                    NUnit.Framework.Assert.IsNotNull(XrmRetrieveHelper.Retrieve<ValueCodeEntity>(localContext, valueCodePresentKort.valueCodeRef.Id, new ColumnSet(false)));

                    // TODO - Marcus
                    // Create more tests for blocking card and creating valuecode etc.

                    // Get Card Details
                    string getCardDetailsResponse = CallWorkflowGetCardDetails(localContext, travelCardNumber);

                    getCardDetailsResponse = "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"><s:Body><ns1:GetCardDetails2Response xmlns:ns1=\"http://www.skanetrafiken.com/DK/INTSTDK004/GetCardDetails2Response/20141216\" xmlns:ns0=\"http://www.skanetrafiken.com/DK/INTSTDK004/CardDetails2/20141216\"><ns1:GetCardDetails2Result><ns0:CardDetails2><ns0:CardInformation><ns0:CardNumber>1136632691</ns0:CardNumber><ns0:CardIssuer>Skånetrafiken CSC</ns0:CardIssuer><ns0:CardKind>4</ns0:CardKind><ns0:CardHotlisted>false</ns0:CardHotlisted><ns0:CardTypePeriod>0</ns0:CardTypePeriod><ns0:CardTypeValue>3161</ns0:CardTypeValue><ns0:CardValueProductType>RESKASSA SKÅNE</ns0:CardValueProductType></ns0:CardInformation><ns0:PurseDetails><ns0:CardCategory>4</ns0:CardCategory><ns0:Balance>200</ns0:Balance><ns0:Currency>SEK</ns0:Currency><ns0:OutstandingDirectedAutoload>false</ns0:OutstandingDirectedAutoload><ns0:OutstandingEnableThresholdAutoload>false</ns0:OutstandingEnableThresholdAutoload><ns0:Hotlisted>false</ns0:Hotlisted></ns0:PurseDetails></ns0:CardDetails2></ns1:GetCardDetails2Result></ns1:GetCardDetails2Response></s:Body></s:Envelope>";
                    // Parse Card Details
                    BiztalkParseCardDetailsMessage cardDetailsParsed = CallWorkflowParseCardDetails(localContext, getCardDetailsResponse);

                    // Block Card
                    string blockCardDetailsResponse = CallWorkflowBlockCardDetails(localContext, cardDetailsParsed.CardNumberField, 5);

                    // Parse Block Card
                    string blockResult = CallWorkflowParseBlockCardDetails(localContext, blockCardDetailsResponse);

                    // Create Value Code (Presentkort)
                    WFReturnObject valueCodePresentkort = CallWorkflowCreateValueCodeAction(localContext, (int)Generated.ed_valuecodetypeglobal.InlostReskassa, (decimal)cardDetailsParsed.BalanceField,
                        "+46735198846", "marcus.stenswed@endeavor.se", null, null, contact.ToEntityReference(), null, (int)Generated.ed_valuecodedeliverytypeglobal.SMS);

                    // Send Value Code (Presentkort)
                    string resultSend2 = CallWorkflowSendValueCode(localContext, valueCodePresentkort.valueCodeRef);

                    // Create Value Code Approval (Presentkort)
                    EntityReference approval = CallWorkflowCreateValueCodeApproval(localContext, cardDetailsParsed.BalanceField, cardDetailsParsed.CardNumberField, contact.ToEntityReference(), (int)Generated.ed_valuecodedeliverytypeglobal.SMS,
                        "marcus.stenswed@endeavor.se", "Marcus", "Stenswed", "+46735198846", true, DateTime.Now.AddYears(1), (int)Generated.ed_valuecodetypeglobal.InlostReskassa);

                    // Approval Value Code Approval (Presentkort)
                    EntityReference valueCodeFromApproval = CallApproveValueCodeApprovalAction(localContext, approval);

                    // Send Value Code (Presentkort)
                    string resultSend3 = CallWorkflowSendValueCode(localContext, valueCodeFromApproval);

                    #endregion

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"FullFlowValueCodes test failed. Ex: {ex.Message}");
            }
        }


        [Test, Category("Debug")]
        public void TestConnectionVoucherService()
        {
            try
            {
                bool manualTest = true;

                // Connect to the Organization service. 
                // The using statement assures that the service proxy will be properly disposed.
                using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
                {
                    // This statement is required to enable early-bound type support.
                    _serviceProxy.EnableProxyTypes();

                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                    //string apiUrl = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_CreateValueCodeAPI);
                    string apiUrl = string.Empty;
                    //string apiToken = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_CreateValueCodeToken);
                    string InputJSON = string.Empty;

                    //Convert string value to enum
                    var tovcEnum = (Generated.ed_valuecodetype)Enum.Parse(typeof(Generated.ed_valuecodetype), "2");

                    if (manualTest == true)
                    {
                        apiUrl = "https://stvoucherservicecertacc.azurewebsites.net/vouchers";
                    }
                    else
                    {
                        //Fetch different api depending on TypeOfValueCode
                        if (tovcEnum == Generated.ed_valuecodetype.Utansaldo)
                            apiUrl = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_CreateValueCodeCoupons);
                        else
                            apiUrl = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_VoucherService);
                    }

                    apiUrl = "https://stvoucherservicecert.azurewebsites.net/vouchers";

                    Uri url = new Uri(apiUrl);
                    HttpWebRequest httpWebRequest = CreateRequest(url);
                    httpWebRequest.Method = "POST";
                    //Changed with VoucherService 2.0
                    ApiHelper.CreateTokenForVoucherService(localContext, httpWebRequest);

                    // TODO - Which template
                    //template = GetValueCodeTemplate(localContext, template, templateNumber);
                    ValueCodeTemplateEntity template = new ValueCodeTemplateEntity();
                    template.ed_TemplateId = 3;

                    // Call to Voucher Service
                    InputJSON = CreateInputJSONVoucherServiceGeneric(localContext, tovcEnum, 365, (int)template.ed_TemplateId, "mobile", (float)0, "api", "0735198846", "marcus.stenswed@endeavor.se",
                        "", "", "", "", "", "", "", "", "", "", "");


                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    //var response = GetAPIResponse<ValueCodeVoucherServiceResponseMessage>(httpWebRequest);

                    //Guid valueCodeGuid = CreateValueCodeFromVoucherServiceResponseGeneric(localContext, response, template, contact, lead, type, email, phoneNumber, refund, valueCodeApproval);

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                    try
                    {
                        //Get response
                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            //Read response
                            var response = streamReader.ReadToEnd();

                            ValueCodeVoucherServiceResponseMessage deserializedVoucher = new ValueCodeVoucherServiceResponseMessage();
                            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(response));
                            DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedVoucher.GetType());
                            deserializedVoucher = ser.ReadObject(ms) as ValueCodeVoucherServiceResponseMessage;
                            ms.Close();

                        }
                    }
                    catch (WebException we)
                    {
                        string resultFromService;

                        HttpWebResponse response = (HttpWebResponse)we.Response;

                        using (var streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            resultFromService = streamReader.ReadToEnd();
                            localContext.TracingService.Trace($"got http error: {response.StatusCode} Content: {resultFromService}");
                        }

                        throw new WebException($"Error when trying to create Value Code. Ex:{we.Message}, message:{resultFromService}");
                    }
                    catch (Exception ex1)
                    {

                    }
                }
            }
            catch (WebException we)
            {
                HttpWebResponse response = (HttpWebResponse)we.Response;
                if (response == null)
                    throw we;

                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    throw new Exception(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception caught when creating value code. Ex: {ex.Message}");
            }
        }

        private static string CreateInputJSONVoucherServiceGeneric(Plugin.LocalPluginContext localContext, Generated.ed_valuecodetype valueCodeType, int validDays, int clearOnTemplate, string type, float amount, string deliverType, string clientsPhoneNumber, string clientsEmailAddress, string apiToken, string label1, string value1, string label2, string value2, string label3, string value3, string label4, string value4, string label5, string value5)
        {
            string customText = "";

            if (label1 != "" && value1 != "")
                customText += $"{label1}: {value1}\n";
            if (label2 != "" && value2 != "")
                customText += $"{label2}: {value2}\n";
            if (label3 != "" && value3 != "")
                customText += $"{label3}: {value3}\n";
            if (label4 != "" && value4 != "")
                customText += $"{label4}: {value4}\n";
            if (label5 != "" && value5 != "")
                customText += $"{label5}: {value5}";

            DataContractJsonSerializer js = null;
            MemoryStream msObj = new MemoryStream();

            localContext.TracingService.Trace($"Value Code Type: {valueCodeType}");

            if (valueCodeType == Generated.ed_valuecodetype.Utansaldo)
            {
                localContext.TracingService.Trace($"Setting up ValueCodeCouponRequest.------\nclearon:{clearOnTemplate}\tapi_token:{apiToken}\ttype:{type}\namount:{amount}\tdelivery_type:{deliverType}\t" +
                    $"email:{clientsEmailAddress}:\tmobile:{clientsPhoneNumber}\nvaliddays:{validDays}\tcustom_Text:{customText}\n-------");

                ValueCodeCouponVoucherServiceRequest valueCode = new ValueCodeCouponVoucherServiceRequest();

                // Amount
                valueCode.amount = Convert.ToInt32(amount);

                if (clientsEmailAddress != null && clientsEmailAddress != "")
                {
                    valueCode.contactAddress = clientsEmailAddress;
                    // contactType 2 = Email
                    valueCode.contactType = 2;
                }
                else if (clientsPhoneNumber != null && clientsPhoneNumber != "")
                {
                    valueCode.contactAddress = clientsPhoneNumber;
                    // contactType 1 = SMS
                    valueCode.contactType = 1;
                }

                // TODO - Set MKL-id from Contact (not necessary)
                //valueCode.travellerId = "";

                // Valid From
                valueCode.validFromDate = DateTime.Now;

                // Valid To
                valueCode.validToDate = DateTime.Now.AddDays(validDays);

                // 1 = förseningsersätting, 2 = presentkort (med saldo), 3 = förlustgaranti
                valueCode.voucherType = clearOnTemplate;


                js = new DataContractJsonSerializer(typeof(ValueCodeCouponRequest));
                js.WriteObject(msObj, valueCode);
            }

            else
            {
                localContext.TracingService.Trace($"Setting up ValueCodeVoucherRequest.");

                ValueCodeCouponVoucherServiceRequest valueCode = new ValueCodeCouponVoucherServiceRequest();

                // Amount
                valueCode.amount = Convert.ToInt32(amount);

                if (clientsEmailAddress != null && clientsEmailAddress != "")
                {
                    valueCode.contactAddress = clientsEmailAddress;
                    // contactType 2 = Email
                    valueCode.contactType = 2;
                }
                else if (clientsPhoneNumber != null && clientsPhoneNumber != "")
                {
                    valueCode.contactAddress = clientsPhoneNumber;
                    // contactType 1 = SMS
                    valueCode.contactType = 1;
                }

                // TODO - Set MKL-id from Contact (not necessary)
                //valueCode.travellerId = "";

                // Valid From
                valueCode.validFromDate = DateTime.Now;

                // Valid To
                valueCode.validToDate = DateTime.Now.AddDays(validDays);

                // 1 = förseningsersätting, 2 = presentkort (med saldo), 3 = förlustgaranti
                valueCode.voucherType = clearOnTemplate;

                js = new DataContractJsonSerializer(typeof(ValueCodeVoucherRequest));
                js.WriteObject(msObj, valueCode);
            }


            msObj.Position = 0;
            StreamReader sr = new StreamReader(msObj);
            string json = sr.ReadToEnd();
            sr.Close();
            msObj.Close();

            return json;

        }

        private static HttpWebRequest CreateRequest(Uri uri)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(uri);
            httpWebRequest.ContentType = "application/json";
            return httpWebRequest;
        }


        private string CallWorkflowGetCardDetails(Plugin.LocalPluginContext localContext, string travelCardNumber)
        {

            OrganizationRequest request = new OrganizationRequest("ed_GetCardDetails");
            request["TravelCardNumber"] = travelCardNumber;

            OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);

            string soapresponse = (string)response["CardDetailsResponse"];
            return soapresponse;
        }

        public WFReturnObject CallWorkflowCreateValueCodeAction(Plugin.LocalPluginContext localContext, int voucherType, decimal amount, string mobile, string email, EntityReference refundId, EntityReference leadId, EntityReference contactId, EntityReference valueCodeApprovalId, int deliveryType)
        {
            try
            {
                OrganizationRequest request = new OrganizationRequest("ed_CreateValueCodeGeneric");
                request["DeliveryType"] = deliveryType;
                request["VoucherType"] = voucherType;
                request["Amount"] = amount;
                request["Mobile"] = mobile;
                request["Email"] = email;
                request["RefundId"] = refundId;
                request["LeadId"] = leadId;
                request["ContactId"] = contactId;
                request["ValueCodeApprovalId"] = valueCodeApprovalId;

                OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);

                WFReturnObject returnObj = new WFReturnObject();

                returnObj.returnMessage = (string)response["ResultCreateValueCodeGeneric"];
                returnObj.valueCodeRef = (EntityReference)response["ValueCodeId"];

                return returnObj;
            }
            catch (Exception ex)
            {
                return new WFReturnObject()
                {
                    returnMessage = ex.Message,
                    valueCodeRef = null
                };
            }
        }

        private BiztalkParseCardDetailsMessage CallWorkflowParseCardDetails(Plugin.LocalPluginContext localContext, string biztalkResponse)
        {
            OrganizationRequest request = new OrganizationRequest("ed_ParseBiztalkResponse");
            request["BiztalkResponse"] = biztalkResponse;

            OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);

            BiztalkParseCardDetailsMessage biztalkResponseObject = new BiztalkParseCardDetailsMessage();

            biztalkResponseObject.BalanceField = (decimal)response["BalanceField"];
            biztalkResponseObject.CardCategoryField = (string)response["CardCategoryField"];
            biztalkResponseObject.CardHotlistedField = (bool)response["CardHotlistedField"];
            biztalkResponseObject.CardIssuerField = (string)response["CardIssuerField"];
            biztalkResponseObject.CardNumberField = (string)response["CardNumberField"];
            biztalkResponseObject.CardTypePeriodField = (int)response["CardTypePeriodField"];
            biztalkResponseObject.CardTypeValueField = (int)response["CardTypeValueField"];
            biztalkResponseObject.CardValueProductTypeField = (string)response["CardValueProductTypeField"];
            biztalkResponseObject.ContractSerialNumberField = (string)response["ContractSerialNumberField"];
            biztalkResponseObject.CurrencyField = (string)response["CurrencyField"];
            //biztalkResponseObject.HotlistedField = (bool)response["HotlistedField"];
            biztalkResponseObject.OutstandingDirectedAutoloadField = (bool)response["OutstandingDirectedAutoloadField"];
            biztalkResponseObject.OutstandingEnableThresholdAutoloadField = (bool)response["OutstandingEnableThresholdAutoloadField"];
            biztalkResponseObject.PeriodCardCategoryField = (string)response["PeriodCardCategoryField"];
            biztalkResponseObject.PeriodCurrencyField = (string)response["PeriodCurrencyField"];
            biztalkResponseObject.PeriodEndField = (DateTime)response["PeriodEndField"];
            biztalkResponseObject.PeriodHotlistedField = (bool)response["PeriodHotlistedField"];
            biztalkResponseObject.PeriodOutstandingDirectedAutoloadField = (bool)response["PeriodOutstandingDirectedAutoloadField"];
            biztalkResponseObject.PeriodOutstandingEnableThresholdAutoload = (bool)response["PeriodOutstandingEnableThresholdAutoload"];
            biztalkResponseObject.PeriodStartField = (DateTime)response["PeriodStartField"];
            biztalkResponseObject.PricePaidField = (int)response["PricePaidField"];
            biztalkResponseObject.ProductTypeField = (string)response["ProductTypeField"];
            biztalkResponseObject.WaitingPeriodsField = (string)response["WaitingPeriodsField"];
            biztalkResponseObject.ZoneListIDField = (string)response["ZoneListIDField"];

            return biztalkResponseObject;
        }

        private string CallWorkflowBlockCardDetails(Plugin.LocalPluginContext localContext, string cardNumber, int reasonCode)
        {
            OrganizationRequest request = new OrganizationRequest("ed_BlockCardBiztalk");
            request["CardNumber"] = cardNumber;
            request["ReasonCode"] = reasonCode;

            OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);

            string cardBlockResponse = (string)response["CardBlockResponse"];
            return cardBlockResponse;
        }

        private string CallWorkflowParseBlockCardDetails(Plugin.LocalPluginContext localContext, string biztalkResponse)
        {
            OrganizationRequest request = new OrganizationRequest("ed_ParseBlockCardResponseFromBiztalk");
            request["BiztalkResponse"] = biztalkResponse;

            OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);

            string parsedCardBlockResponse = (string)response["RequestCardBlockResult"];
            return parsedCardBlockResponse;
        }

        private string CallWorkflowSendValueCode(Plugin.LocalPluginContext localContext, EntityReference valueCodeId)
        {
            OrganizationRequest request = new OrganizationRequest("ed_SendValueCode");
            request["Target"] = valueCodeId;

            OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);

            return (string)response["Result"];
        }

        private EntityReference CallWorkflowCreateValueCodeApproval(Plugin.LocalPluginContext localContext, decimal amount, string cardNumber, EntityReference contact, int deliveryMethod, string email, string firstname, string lastname, string mobile, bool needsManualApproval, DateTime validTo, int typeOfValueCode)
        {
            OrganizationRequest request = new OrganizationRequest("ed_CreateValueCodeApproval");
            request["Amount"] = amount;
            request["CardNumber"] = cardNumber;
            request["Contact"] = contact;
            request["DeliveryMethod"] = deliveryMethod;
            request["Email"] = email;
            request["Firstname"] = firstname;
            request["Lastname"] = lastname;
            request["Mobile"] = mobile;
            request["NeedsManualApproval"] = needsManualApproval;
            request["ValidTo"] = validTo;
            request["TypeOfValueCode"] = typeOfValueCode;

            OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);

            return (EntityReference)response["ValueCodeApprovalId"];
        }

        private EntityReference CallWorkflowApproveValueCodeApproval(Plugin.LocalPluginContext localContext, EntityReference approval)
        {
            OrganizationRequest request = new OrganizationRequest("ed_ApproveValueCodeApproval");
            request["ValueCodeApprovalId"] = approval;

            OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);

            return (EntityReference)response["ValueCodeId"];
        }

        private void CallWorkflowDeclineValueCodeApproval(Plugin.LocalPluginContext localContext, EntityReference approval)
        {
            OrganizationRequest request = new OrganizationRequest("ed_DeclineValueCode");
            request["ValueCodeApprovalId"] = approval;

            OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);
        }

        private ContactEntity CreateOrRetrieveRGOLTestContact(Plugin.LocalPluginContext localContext, string email, string mobile, string testInstanceName)
        {
            ContactEntity contact = new ContactEntity();

            FilterExpression emailFilter = new FilterExpression(LogicalOperator.Or)
            {
                Conditions =
                {
                    new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, email),
                    new ConditionExpression(ContactEntity.Fields.EMailAddress2, ConditionOperator.Equal, email)
                }
            };

            contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, new ColumnSet(true),
                    new FilterExpression()
                    {
                        Conditions =
                        {
                                new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                                new ConditionExpression(ContactEntity.Fields.Telephone2, ConditionOperator.Equal, mobile)
                        },
                        Filters =
                        {
                            emailFilter
                        }
                    });

            if (contact == null)
            {
                contact = new ContactEntity
                {
                    FirstName = "TestRGOL" + testInstanceName,
                    LastName = "TestRGOL" + testInstanceName,
                    EMailAddress2 = email,
                    MobilePhone = mobile,
                    ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund
                };

                Guid id = XrmHelper.Create(localContext, contact);
                contact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, id, new ColumnSet(true));
            }

            return contact;
        }

        private IncidentEntity CreateRGOLTestCase(Plugin.LocalPluginContext localContext, ContactEntity contact, string testInstanceName)
        {
            IncidentEntity incident = new IncidentEntity
            {
                Title = "2019-07-19: Öresundståg Malmö C -> Kristianstad C  [1082]" + testInstanceName,
                Description = "Description" + testInstanceName,
                CaseTypeCode = Generated.incident_casetypecode.Travelwarranty,
                CaseOriginCode = Generated.incident_caseorigincode.ResegarantiOnline,
                PriorityCode = Generated.incident_prioritycode._2,
                cgi_ActionDate = DateTime.Now.AddDays(-6),
                cgi_arrival_date = DateTime.Now,
                cgi_Contactid = contact.ToEntityReference(),
                cgi_customer_email = (contact.EMailAddress1 != null) ? contact.EMailAddress1 : contact.EMailAddress2,
                cgi_TelephoneNumber = contact.Telephone2,
                cgi_BOMBMobileNumber = contact.Telephone2,
                CustomerId = contact.ToEntityReference()
            };

            Guid id = XrmHelper.Create(localContext, incident);
            return XrmRetrieveHelper.Retrieve<IncidentEntity>(localContext, id, new ColumnSet(true));
        }

        private RefundEntity CreateRGOLTestRefund(Plugin.LocalPluginContext localContext, IncidentEntity incident, ContactEntity contact, string testInstanceName)
        {
            RefundEntity refund = new RefundEntity()
            {
                cgi_RefundTypeid = GetPrisavdrag(localContext).ToEntityReference(),
                cgi_Amount = new Money(1),
                cgi_ReimbursementFormid = GetReimbursementByName(localContext, "Värdekod - SMS").ToEntityReference(),
                cgi_Caseid = incident.ToEntityReference(),
                cgi_MobileNumber = contact.Telephone2,
                cgi_email = (contact.EMailAddress1 != null) ? contact.EMailAddress1 : contact.EMailAddress2,
                cgi_refundnumber = incident.TicketNumber
            };

            Guid id = XrmHelper.Create(localContext, refund);

            return XrmRetrieveHelper.Retrieve<RefundEntity>(localContext, id, new ColumnSet(true));
        }

        public ReimbursementFormEntity GetReimbursementByName(Plugin.LocalPluginContext localContext, string name)
        {
            var reimbursementSmsQuery = new QueryExpression()
            {
                EntityName = ReimbursementFormEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(),
                Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(ReimbursementFormEntity.Fields.cgi_reimbursementname, ConditionOperator.Equal, name)
                        }
                    }
            };

            return XrmRetrieveHelper.RetrieveFirst<ReimbursementFormEntity>(localContext, reimbursementSmsQuery);

        }

        public RefundTypeEntity GetPrisavdrag(Plugin.LocalPluginContext localContext)
        {
            var refundTypeQuery = new QueryExpression()
            {
                EntityName = RefundTypeEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(false),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(RefundTypeEntity.Fields.cgi_refundtypename, ConditionOperator.Equal, "Prisavdrag")
                    }
                }
            };
            return XrmRetrieveHelper.RetrieveFirst<RefundTypeEntity>(localContext, refundTypeQuery);
        }

        /// <summary>
        /// Get unique string YYMMDD.HHSS
        /// </summary>
        /// <returns></returns>
        public static string GetUnitTestID()
        {
            DateTime today = DateTime.Now;

            return today.ToString("yyyyMMdd.hhmm");
        }

        internal ServerConnection ServerConnection
        {
            get
            {
                if (_serverConnection == null)
                {
                    _serverConnection = new ServerConnection();
                }
                return _serverConnection;
            }
        }

        internal ServerConnection.Configuration Config
        {
            get
            {
                return TestSetup.Config;
            }
        }
    }
}
