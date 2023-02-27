using Common.Logging;
using Quartz;
using Skanetrafiken.Crm.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using System.Linq;
using Microsoft.Xrm.Sdk.Query;
using Renci.SshNet;
using Endeavor.Crm.DeltabatchService.CancellationCodes.CancellationCodeLogic;
using System.Globalization;
using Skanetrafiken.Crm.Schema.Generated;

namespace Endeavor.Crm.DeltabatchService
{
    [DisallowConcurrentExecution, PersistJobDataAfterExecution]
    public partial class DownloadJob : IJob
    {
        public const string DataMapModifiedAfter = "ModifiedAfterDownload";
        public const string GroupName = "Download Schedule";
        public const string TriggerDescription = "Download Schedule Trigger";
        public const string JobDescription = "Download Schedule Job";
        public const string TriggerName = "DownloadTrigger";
        public const string JobName = "DownloadJob";

        protected SftpClient sftpClient;

        private string _currentOutputFileName;
        private string[] _fieldNames = new string[0];
        private List<Tuple<string[], Exception>> _errors = new List<Tuple<string[], Exception>>();

        #region Deltabatch Field Names
        private const string _socSecFieldKeyword = "PNR";
        private const string _guidFieldKeyword = "TRANSACTION_ID";
        private const string _freetextFieldKeyword = "FREETEXT";
        private const string _changeCodesFieldKeyword = "CHANGE_CODES";
        private const string _firstNameFieldKeyword = "FIRST_NAME";
        private const string _givenNameFieldKeyword = "GIVEN_NAME";
        private const string _lastNameFieldKeyword = "LAST_NAME";
        private const string _coAddressFieldKeyword = "CO_ADDRESS";
        private const string _registeredAddressFieldKeyword = "REGISTERED_ADDRESS";
        private const string _postalCodeFieldKeyword = "ZIPCODE";
        private const string _cityFieldKeyword = "TOWN";
        private const string _communityFieldKeyword = "COMMUNITY";
        private const string _specCoAddressFieldKeyword = "SPEC_CO_ADDRESS";
        private const string _specAddressFieldKeyword = "SPEC_ADDRESS";
        private const string _specPostalCodeFieldKeyword = "SPEC_ZIPCODE";
        private const string _specCountryFieldKeyword = "SPEC_COUNTRY";
        private const string _specCityFieldKeyword = "SPEC_TOWN";
        private const string _specRegisteredAddressFieldKeyword = "SPEC_REGISTERED_ADDRESS";
        private const string _searchDateFieldKeyword = "SEARCH_DATE";
        private const string _countyFieldKeyword = "COUNTY";
        private const string _countyNumberFieldKeyword = "COUNTYNO";
        private const string _communityNumberFieldKeyword = "COMMUNITYNO";
        private const string _rejectCodeFieldKeyword = "REJECT_CODE";
        private const string _rejectTextFieldKeyword = "REJECT_TEXT";
        private const string _rejectCommentFieldKeyword = "REJECT_COMMENT";
        #endregion

        private ILog _log = LogManager.GetLogger(typeof(DownloadJob));

        private int numberOfOperations;

        private ICancellationCodeLogicFactory _cancellationCodeLogicFactory;


        public DownloadJob()
        {
            _cancellationCodeLogicFactory = new CancellationCodeLogicFactory(new DateTimeProvider());
        }

        public DownloadJob(ICancellationCodeLogicFactory cancellationCodeLogicFactory)
        {
            _cancellationCodeLogicFactory = cancellationCodeLogicFactory;
        }

        public void Execute(IJobExecutionContext context)
        {
            _log.Debug(string.Format(Properties.Resources.TriggerExecuting, context.Trigger.Description ?? context.Trigger.Key.Name));

            JobDataMap dataMap = context.JobDetail.JobDataMap;

            DateTime modifiedAfter = dataMap.GetDateTime(DataMapModifiedAfter);

            _log.Debug(string.Format(Properties.Resources.ScheduleJobExecuting, context.JobDetail.Description ?? context.JobDetail.Key.Name ?? "NULL", modifiedAfter.ToString() ?? "NULL"));

            ExecuteJob();

            _log.Debug(string.Format(Properties.Resources.ScheduleJobExecuted, context.JobDetail.Description ?? context.JobDetail.Key.Name, modifiedAfter.ToString()));
        }

        public void ExecuteJob()
        {
            Plugin.LocalPluginContext localContext = null;
            try
            {
                localContext = DeltabatchJobHelper.GenerateLocalContext();

#if !DEBUG
                RetrieveFile();
#endif

                UpdateContactsWithNewInfo(localContext);

                ArchiveFile();

                _log.Info("DownloadJob Done!");
            }
            catch (Exception e)
            {
                _log.Error($"Exception caught in ExecuteJob():\n{e.Message}\n\n{e}");
                //if (localContext != null)
                DeltabatchJobHelper.SendErrorMailToDev(localContext, e);
            }
        }

        /// <summary>
        /// Checks if the given contact should be updated by deltabatch. The contact should be updated if it has a MklId or if the service travels flag is set.
        /// </summary>
        /// <returns></returns>
        private bool ShouldUpdateContact(ContactEntity contact)
        {
            return !string.IsNullOrEmpty(contact.ed_MklId) || contact.ed_Serviceresor == true;
        }

        public void ArchiveFile()
        {
            _log.Info("Archiving files");
            DirectoryInfo retrievedFileLocation = new DirectoryInfo(Properties.Settings.Default.DeltabatchRetrievedFileLocation);
            DirectoryInfo retrievedFileHistoryLocation = new DirectoryInfo(Properties.Settings.Default.DeltabatchRetrievedFileLocation + "\\History");
            if (!retrievedFileLocation.Exists)
                retrievedFileLocation.Create();
            if (!retrievedFileHistoryLocation.Exists)
                retrievedFileHistoryLocation.Create();

            FileInfo[] retrievedFilesInfo = retrievedFileLocation.GetFiles();
            if (retrievedFilesInfo.Length < 1)
                return;
            for (int i = 0; i < retrievedFilesInfo.Length; i++)
            {
                File.Move($"{retrievedFileLocation.FullName}\\{retrievedFilesInfo[i]}", $"{retrievedFileHistoryLocation.FullName}\\{retrievedFilesInfo[i]}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localContext"></param>
        public void UpdateContactsWithNewInfo(Plugin.LocalPluginContext localContext)
        {
            _log.Info("Entered UpdateContactsWithNewInfo()");
            try
            {

                string outputFilePath = $"{Properties.Settings.Default.DeltabatchRetrievedFileLocation}{_currentOutputFileName}";

                _log.Info($"outputFilePath {outputFilePath}");

                if (!File.Exists(outputFilePath))
                {
                    throw new Exception($"No file found at {outputFilePath}");
                }
                using (StreamReader file = new StreamReader(outputFilePath))
                {
                    string outputLine;
                    _fieldNames = file.ReadLine().Split(';');
                    if (VerifyPresenceOfAllFields())
                    {
                        _log.Info($"Commencing update loop");
                        numberOfOperations = 0;
                        while ((outputLine = file.ReadLine()) != null)
                        {
                            numberOfOperations++;

                            try
                            {
                                if (string.IsNullOrWhiteSpace(outputLine))
                                {
                                    continue;
                                }

                                string[] lineParams = outputLine.Split(';');
                                if (lineParams.Length != _fieldNames.Length)
                                {
                                    _errors.Add(new Tuple<string[], Exception>(lineParams, new Exception($"Incomplete responseRow: {outputLine}, expected number of parameters: {_fieldNames.Length}")));
                                    continue;
                                }

                                string socialSecurityNumber = lineParams[Array.IndexOf(_fieldNames, _socSecFieldKeyword)];
                                if (string.IsNullOrEmpty(socialSecurityNumber))
                                {
                                    _errors.Add(new Tuple<string[], Exception>(lineParams, new Exception($"Socialsecuritynumber is null or empty. Please handle manually.")));
                                    continue;
                                }

                                if (numberOfOperations % 5000 < 3)
                                {
                                    _log.Info($"Retrieving Current information");
                                }

                                ColumnSet contactColumnsWithAddress2 = ContactEntity.ContactInfoBlock;
                                contactColumnsWithAddress2.AddColumns(
                                                                ContactEntity.Fields.Address2_Line1,
                                                                ContactEntity.Fields.Address2_Line2,
                                                                ContactEntity.Fields.Address2_PostalCode,
                                                                ContactEntity.Fields.Address2_City,
                                                                ContactEntity.Fields.Address2_Country,
                                                                ContactEntity.Fields.ed_MklId,
                                                                ContactEntity.Fields.ed_Serviceresor);

                                FilterExpression filterContacts = new FilterExpression
                                {
                                    Conditions =
                                        {
                                            new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.Equal, socialSecurityNumber),
                                            new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active)
                                        }
                                };

                                List<ContactEntity> lExistingContact = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, ContactEntity.EntityLogicalName, contactColumnsWithAddress2, filterContacts).ToList();

                                if (numberOfOperations % 5000 < 3)
                                {
                                    _log.Info($"Retrieve Done");
                                }

                                foreach (ContactEntity contact in lExistingContact)
                                {
                                    //add params to log entity
                                    DeltabatchLogEntity credLog = new DeltabatchLogEntity
                                    {
                                        ed_CreditsafeParameters = outputLine
                                    };

                                    ContactEntity updateEntity = ParseChangedFieldsToAlteredContactEntity(localContext, lineParams, credLog, out bool fbUpdated, out bool isRejected, contact, socialSecurityNumber);
                                    if (ShouldUpdateContact(updateEntity))
                                    {
                                        XrmHelper.Create(localContext, credLog);

                                        if (updateEntity == null)
                                        {
                                            continue;
                                        }

                                        if (fbUpdated || isRejected)
                                        {
                                            updateEntity.ed_InformationSource = ed_informationsource.Folkbokforing;
                                            XrmHelper.Update(localContext, updateEntity);
                                        }
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                _errors.Add(new Tuple<string[], Exception>(new string[] { outputLine }, e));
                            }
                            if (numberOfOperations % 10000 == 0)
                            {
                                _log.Info($"Iterated {numberOfOperations} updates, regenerating context");
                            }
                        }
                    }
                    else
                    {
                        _log.Info($"Incorrect definition of Fields");
                    }
                }

                _log.Info($"Creating Error log for {_errors.Count} errors");
                DeltabatchErrorLogEntity errorLog = new DeltabatchErrorLogEntity()
                {
                    ed_name = "Deltabatch Error Log " + DateTime.Now.ToString("yyyy/MM/dd - hh:mm")
                };
                errorLog.Id = XrmHelper.Create(localContext, errorLog);
                errorLog.ed_DeltabatchErrorLogId = errorLog.Id;
                foreach (Tuple<string[], Exception> tuple in _errors)
                {
                    string errorSource = "";
                    if (tuple.Item1 != null && tuple.Item1.Length > 0)
                    {
                        for (int i = 0; i < tuple.Item1.Length; i++)
                        {
                            if (i > 0)
                                errorSource += ";";
                            errorSource += tuple.Item1[i];
                        }
                    }
                    string errorMess = tuple.Item2?.Message != null ? (tuple.Item2.Message.Length > 199 ? tuple.Item2.Message.Substring(0, 199) : tuple.Item2.Message) : "Error Message missing";
                    DeltabatchErrorLogRowEntity errorRow = new DeltabatchErrorLogRowEntity
                    {
                        ed_DeltabatchErrorLog = errorLog.ToEntityReference(),
                        ed_ErrorMessage = errorMess,
                        ed_ParameterString = errorSource,
                        ed_name = errorMess.Substring(0, 15) + "... " + errorSource.Substring(0, 12)
                    };
                    errorRow.Id = XrmHelper.Create(localContext, errorRow);
                }
            }
            catch (Exception e)
            {
                _log.Error($"Exception caught in UpdateContactsWithNewInfo():\n{e.Message}\n\n{e}");
                throw e;
            }
        }

        private bool VerifyPresenceOfAllFields()
        {
            if (Array.IndexOf(_fieldNames, _socSecFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_socSecFieldKeyword}")));
            }
            if (Array.IndexOf(_fieldNames, _guidFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_guidFieldKeyword}")));
            }
            if (Array.IndexOf(_fieldNames, _freetextFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_freetextFieldKeyword}")));
            }
            if (Array.IndexOf(_fieldNames, _changeCodesFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_changeCodesFieldKeyword}")));
            }
            if (Array.IndexOf(_fieldNames, _firstNameFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_firstNameFieldKeyword}")));
            }
            if (Array.IndexOf(_fieldNames, _givenNameFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_givenNameFieldKeyword}")));
            }
            if (Array.IndexOf(_fieldNames, _lastNameFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_lastNameFieldKeyword}")));
            }
            if (Array.IndexOf(_fieldNames, _coAddressFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_coAddressFieldKeyword}")));
            }
            if (Array.IndexOf(_fieldNames, _registeredAddressFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_registeredAddressFieldKeyword}")));
            }
            if (Array.IndexOf(_fieldNames, _postalCodeFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_postalCodeFieldKeyword}")));
            }
            if (Array.IndexOf(_fieldNames, _cityFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_cityFieldKeyword}")));
            }
            if (Array.IndexOf(_fieldNames, _communityFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_communityFieldKeyword}")));
            }
            if (Array.IndexOf(_fieldNames, _specCoAddressFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_specCoAddressFieldKeyword}")));
            }
            if (Array.IndexOf(_fieldNames, _specAddressFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_specAddressFieldKeyword}")));
            }
            if (Array.IndexOf(_fieldNames, _specPostalCodeFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_specPostalCodeFieldKeyword}")));
            }
            if (Array.IndexOf(_fieldNames, _specCountryFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_specCountryFieldKeyword}")));
            }
            if (Array.IndexOf(_fieldNames, _specCityFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_specCityFieldKeyword}")));
            }
            if (Array.IndexOf(_fieldNames, _specRegisteredAddressFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_specRegisteredAddressFieldKeyword}")));
            }
            if (Array.IndexOf(_fieldNames, _searchDateFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_searchDateFieldKeyword}")));
            }
            if (Array.IndexOf(_fieldNames, _countyFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_countyFieldKeyword}")));
            }
            if (Array.IndexOf(_fieldNames, _countyNumberFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_countyNumberFieldKeyword}")));
            }
            if (Array.IndexOf(_fieldNames, _communityNumberFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_communityNumberFieldKeyword}")));
            }
            if (Array.IndexOf(_fieldNames, _rejectCodeFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_rejectCodeFieldKeyword}")));
            }
            if (Array.IndexOf(_fieldNames, _rejectTextFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_rejectTextFieldKeyword}")));
            }
            if (Array.IndexOf(_fieldNames, _rejectCommentFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_rejectCommentFieldKeyword}")));
            }
            if (_errors.Count > 0)
                return false;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="lineParams"></param>
        /// <param name="deltaLog"></param>
        /// <param name="fbUpdated"></param>
        /// <param name="rejected"></param>
        /// <returns></returns>
        private ContactEntity ParseChangedFieldsToAlteredContactEntity(Plugin.LocalPluginContext localContext, string[] lineParams, DeltabatchLogEntity deltaLog, out bool fbUpdated, out bool rejected, ContactEntity existingContact, string socSec)
        {
            _log.Debug("Entered ParseChangedFieldsToAlteredContactEntity()");
            if (numberOfOperations % 5000 < 3)
            {
                _log.Info($"Iteration {numberOfOperations}, Entered ParseChangedFieldsToAlteredContactEntity");
            }

            fbUpdated = false;
            rejected = false;

            // TODO: Refactor to use DeltaBatchUpdateRow
            #region extract params to strings and build log
            string transactionGuid = lineParams[Array.IndexOf(_fieldNames, _guidFieldKeyword)];
            deltaLog.ed_TransactionId = transactionGuid;
            string updateName = lineParams[Array.IndexOf(_fieldNames, _freetextFieldKeyword)];
            deltaLog.ed_UpdateName = updateName;
            deltaLog.ed_name = updateName;
            string changeCodesString = lineParams[Array.IndexOf(_fieldNames, _changeCodesFieldKeyword)];
            deltaLog.ed_ChangeCodes = changeCodesString;

            string rejectCode = lineParams[Array.IndexOf(_fieldNames, _rejectCodeFieldKeyword)];
            string rejectText = lineParams[Array.IndexOf(_fieldNames, _rejectTextFieldKeyword)];
            string rejectComment = lineParams[Array.IndexOf(_fieldNames, _rejectCommentFieldKeyword)];
            deltaLog.ed_RejectCode = rejectCode;
            deltaLog.ed_RejectText = rejectText;
            deltaLog.ed_RejectComment = rejectComment;

            deltaLog.ed_SocialSecurityNumber = socSec;
            string givenName = lineParams[Array.IndexOf(_fieldNames, _givenNameFieldKeyword)];
            deltaLog.ed_GivenName = givenName;
            string firstName = lineParams[Array.IndexOf(_fieldNames, _firstNameFieldKeyword)];
            deltaLog.ed_FirstName = firstName;
            string lastName = lineParams[Array.IndexOf(_fieldNames, _lastNameFieldKeyword)];
            deltaLog.ed_LastName = lastName;
            string coAddress = lineParams[Array.IndexOf(_fieldNames, _coAddressFieldKeyword)];
            deltaLog.ed_AddressCo = coAddress;
            string addressLine = lineParams[Array.IndexOf(_fieldNames, _registeredAddressFieldKeyword)];
            deltaLog.ed_RegisteredAddress = addressLine;
            string postalCode = lineParams[Array.IndexOf(_fieldNames, _postalCodeFieldKeyword)];
            deltaLog.ed_PostalCode = postalCode;
            string city = lineParams[Array.IndexOf(_fieldNames, _cityFieldKeyword)];
            deltaLog.ed_City = city;
            string community = lineParams[Array.IndexOf(_fieldNames, _communityFieldKeyword)];
            deltaLog.ed_Community = community;
            string specialCo = lineParams[Array.IndexOf(_fieldNames, _specCoAddressFieldKeyword)];
            deltaLog.ed_SpecialCoAddress = specialCo;
            string specialPostalCode = lineParams[Array.IndexOf(_fieldNames, _specPostalCodeFieldKeyword)];
            deltaLog.ed_SpecialPostalCode = specialPostalCode;
            string specialCountry = lineParams[Array.IndexOf(_fieldNames, _specCountryFieldKeyword)];
            deltaLog.ed_SpecialCountry = specialCountry;
            string specialCity = lineParams[Array.IndexOf(_fieldNames, _specCityFieldKeyword)];
            deltaLog.ed_SpecialCity = specialCity;
            string specialRegAddr = lineParams[Array.IndexOf(_fieldNames, _specRegisteredAddressFieldKeyword)];
            deltaLog.ed_SpecialRegisteredAddress = specialRegAddr.Length < 100 ? specialRegAddr : specialRegAddr.Substring(0, 99); // field is limited to 100 characters
            string dateString = lineParams[Array.IndexOf(_fieldNames, _searchDateFieldKeyword)];
            deltaLog.ed_SearchDate = dateString;
            string county = lineParams[Array.IndexOf(_fieldNames, _countyFieldKeyword)];
            deltaLog.ed_County = county;
            string countyNumberString = lineParams[Array.IndexOf(_fieldNames, _countyNumberFieldKeyword)];
            deltaLog.ed_CountyNumber = countyNumberString;
            string communityNumberString = lineParams[Array.IndexOf(_fieldNames, _communityNumberFieldKeyword)];
            deltaLog.ed_CommunityNumber = communityNumberString;
            #endregion

            if (existingContact == null)
            {
                _errors.Add(new Tuple<string[], Exception>(lineParams, new Exception($"Could not find a contact with socialsecuritynumber {socSec}. Please handle manually. SocSecNr = {socSec}")));
                return null;
            }
            deltaLog.ed_RegardingContact = existingContact.ToEntityReference();

            ContactEntity updateContact = new ContactEntity
            {
                ContactId = existingContact.ContactId,
                Id = existingContact.Id,
                ed_MklId = existingContact.ed_MklId,
                ed_Serviceresor = existingContact.ed_Serviceresor
            };
            //rejectCode
            if (!string.IsNullOrWhiteSpace(rejectCode))
            {
                string rejectErrorMessage;
                Generated.ed_creditsaferejectcodes? rejectOptionSetValue = TranslateIntToRejectionOptionSetValue(localContext, rejectCode, out rejectErrorMessage);
                if (rejectOptionSetValue == null)
                {
                    _errors.Add(new Tuple<string[], Exception>(lineParams, new Exception($"Reject code {rejectCode} could not be translated to OptionSetValue. Please consult a developer.\nInner Error message: {rejectErrorMessage}")));
                    return null;
                }

                updateContact.ed_CreditsafeRejectionCode = rejectOptionSetValue;
                updateContact.ed_CreditsafeRejectionText = rejectText;
                updateContact.ed_CreditsafeRejectionComment = rejectComment;
                rejected = true;

                DeltaBatchUpdateRow updatedContactData = new DeltaBatchUpdateRow
                {
                    TransactionGuid = transactionGuid,
                    UpdateName = updateName,
                    ChangeCodesString = changeCodesString,
                    RejectCode = rejectCode,
                    RejectText = rejectText,
                    RejectComment = rejectComment,
                    GivenName = givenName,
                    FirstName = firstName,
                    LastName = lastName,
                    CoAddress = coAddress,
                    AddressLine = addressLine,
                    PostalCode = postalCode,
                    City = city,
                    Community = community,
                    SpecialCo = specialCo,
                    SpecialCountry = specialCountry,
                    SpecialCity = specialCity,
                    SpecialPostalCode = specialPostalCode,
                    SpecialRegAddr = specialRegAddr,
                    DateString = dateString,
                    County = county,
                    CountyNumberString = countyNumberString,
                    CommunityNumberString = communityNumberString
                };

                ICancellationCodeLogic cancellationCodeLogic = _cancellationCodeLogicFactory.GetCancellationCodeHandler(rejectOptionSetValue.Value);
                if (cancellationCodeLogic != null)
                {
                    cancellationCodeLogic.HandleStatusCode(updateContact, updatedContactData);
                }
                else
                {
                    _log.Warn($"Recieved reject code {rejectCode} but no handler is implemented for this code yet.");
                }

                updateContact.ed_UpdatedFB = true;

                if (numberOfOperations % 5000 < 3)
                {
                    _log.Info($"Rejected, returning rejected Contact");
                }

                return updateContact;
            }

            // INFO - Address2_Composite should not be used. Is always a combination of co and registered
            #region Change Codes
            //if (!string.IsNullOrWhiteSpace(changeCodesString))
            //{
            //    string[] separatedChangeCodes = changeCodesString.Split('&');
            //}
            #endregion
            #region SocialSecurityNumber
            if (existingContact.cgi_socialsecuritynumber != socSec)
            {
                updateContact.cgi_socialsecuritynumber = socSec;
                updateContact.BirthDate = ContactEntity.UpdateBirthDateOnContact(socSec); //DevOps 9168
                fbUpdated = true;
            }
            #endregion
            #region FirstName
            string firstNameToUse = !string.IsNullOrWhiteSpace(givenName) ? givenName : firstName;
            if (existingContact.FirstName != firstNameToUse)
            {
                updateContact.FirstName = firstNameToUse;
                fbUpdated = true;
            }
            #endregion
            #region LastName
            if (existingContact.LastName != lastName)
            {
                updateContact.LastName = lastName;
                fbUpdated = true;
            }
            #endregion
            #region C/O Address
            if (existingContact.Address1_Line1 != coAddress)
            {
                updateContact.Address1_Line1 = coAddress;
                fbUpdated = true;
            }
            #endregion
            #region AddressField
            if (existingContact.Address1_Line2 != addressLine)
            {
                updateContact.Address1_Line2 = addressLine;
                fbUpdated = true;
            }
            #endregion
            #region PostalCode
            // 2018-11-22 - Marcus Stenswed
            // If Postal Code is missing, set value 00000
            if (String.IsNullOrEmpty(postalCode) && !String.IsNullOrEmpty(existingContact.Address1_PostalCode) && existingContact.Address1_PostalCode != "00000")
            {
                updateContact.Address1_PostalCode = "00000";
                fbUpdated = true;
            }
            else if (String.IsNullOrEmpty(postalCode) && String.IsNullOrEmpty(existingContact.Address1_PostalCode))
            {
                updateContact.Address1_PostalCode = "00000";
                fbUpdated = true;
            }
            else if (existingContact.Address1_PostalCode != postalCode)
            {
                updateContact.Address1_PostalCode = postalCode;
                fbUpdated = true;
            }
            #endregion
            #region City
            {
                updateContact.Address1_City = city;
                fbUpdated = true;
            }
            #endregion
            #region Community
            if (existingContact.ed_Address1_Community != community)
            {
                updateContact.ed_Address1_Community = community;
                fbUpdated = true;
            }
            #endregion
            #region C/O Address - Special
            if (existingContact.Address2_Line1 != specialCo)
            {
                updateContact.Address2_Line1 = specialCo;
                fbUpdated = true;
            }
            #endregion
            #region PostalCode - Special
            if (existingContact.Address2_PostalCode != specialPostalCode)
            {
                updateContact.Address2_PostalCode = specialPostalCode;
                fbUpdated = true;
            }
            #endregion
            #region Country - Special
            if (existingContact.Address2_Country != specialCountry)
            {
                updateContact.Address2_Country = specialCountry;
                fbUpdated = true;
            }
            #endregion
            #region City - Special
            if (existingContact.Address2_City != specialCity)
            {
                updateContact.Address2_City = specialCity;
                fbUpdated = true;
            }
            #endregion
            #region AddressField - Special
            if (existingContact.Address2_Line2 != specialRegAddr)
            {
                updateContact.Address2_Line2 = specialRegAddr;
                fbUpdated = true;
            }
            #endregion

            #region SearchDate
            if (dateString.Length != 8)
            {
                _errors.Add(new Tuple<string[], Exception>(lineParams, new Exception($"Invalid parameter SEARCH_DATE: {dateString} was expected to be 8 integers")));
                return null;
            }

            if(DateTime.TryParseExact(dateString, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedSearchDate))
            {
                updateContact.ed_UpdatedFBDate = parsedSearchDate;
            }
            else
            {
                _errors.Add(new Tuple<string[], Exception>(lineParams, new Exception($"Invalid parameter SEARCH_DATE: {dateString} does not conform to the pattern 'yyyyMMdd'.")));
                return null;
            }

            #endregion
            #region County
            if (existingContact.Address1_County != county)
            {
                updateContact.Address1_County = county;
                fbUpdated = true;
            }
            #endregion
            #region CountyNumber
            int countyNumber;
            if (int.TryParse(countyNumberString, out countyNumber) || "".Equals(countyNumberString))
            {
                if ("".Equals(countyNumberString))
                {
                    if (existingContact.ed_Address1_CountyNumber != null)
                    {
                        updateContact.ed_Address1_CountyNumber = null;
                        fbUpdated = true;
                    }
                }
                else if (existingContact.ed_Address1_CountyNumber != countyNumber)
                {
                    updateContact.ed_Address1_CountyNumber = countyNumber;
                    fbUpdated = true;
                }
            }
            else
            {
                _log.Info($"Parameter {_countyNumberFieldKeyword} with value {countyNumberString} could not be parsed to an integer. Field ignored");
            }
            #endregion
            #region CommunityNumber
            int communityNumber;
            if (int.TryParse(communityNumberString, out communityNumber) || "".Equals(communityNumberString))
            {
                if ("".Equals(communityNumberString))
                {
                    if (existingContact.ed_Address1_CommunityNumber != null)
                    {
                        updateContact.ed_Address1_CommunityNumber = null;
                        fbUpdated = true;
                    }
                }
                else if (existingContact.ed_Address1_CommunityNumber != communityNumber)
                {
                    updateContact.ed_Address1_CommunityNumber = communityNumber;
                    fbUpdated = true;
                }
            }
            else
            {
                _log.Info($"Parameter {_communityNumberFieldKeyword} with value {communityNumberString} could not be parsed to an integer. Field ignored");
            }
            #endregion
            #region Country if needed
            if (!string.IsNullOrWhiteSpace(updateContact.Address1_Line1) ||
                !string.IsNullOrWhiteSpace(updateContact.Address1_Line2) ||
                !string.IsNullOrWhiteSpace(updateContact.Address1_PostalCode) ||
                !string.IsNullOrWhiteSpace(updateContact.Address1_City) ||
                !string.IsNullOrWhiteSpace(updateContact.ed_Address1_Community) ||
                !string.IsNullOrWhiteSpace(updateContact.Address1_County)
                )
            {
                updateContact.ed_Address1_Country = CountryEntity.GetEntityRefForCountryCode(localContext, "SE");
            }
            #endregion
            if (fbUpdated && existingContact.ed_UpdatedFB != true)
                updateContact.ed_UpdatedFB = true;

            if (numberOfOperations % 5000 < 3)
            {
                _log.Info($"Returning updated Contact, update = {fbUpdated}");
            }
            return updateContact;
        }

        private Generated.ed_creditsaferejectcodes? TranslateIntToRejectionOptionSetValue(Plugin.LocalPluginContext localContext, string rejectCode, out string rejectErrorMess)
        {
            rejectCode = rejectCode.Replace("S", "10");
            int rejectOptionSetCode;
            if (!int.TryParse(rejectCode, out rejectOptionSetCode))
            {
                rejectErrorMess = $"Could not parse {rejectCode} to integer.";
                return null;
            }
            IEnumerable<Generated.ed_creditsaferejectcodes> values = Enum.GetValues(typeof(Generated.ed_creditsaferejectcodes)).Cast<Generated.ed_creditsaferejectcodes>();

            foreach (Generated.ed_creditsaferejectcodes code in values)
            {
                if ((int)code == rejectOptionSetCode)
                {
                    rejectErrorMess = null;
                    return code;
                }
            }
            rejectErrorMess = "Could not find a valid OptionSetValue in Generated.ed_creditsaferejectcodes";
            return null;
        }

        public void RetrieveFile()
        {
            _log.Info("Entered RetrieveFile()");
            _log.InfoFormat($"Retrieved File Path is: " + Properties.Settings.Default.DeltabatchRetrievedFileLocation);
            sftpClient = null;
            try
            {
                sftpClient = DeltabatchJobHelper.CreateSftpConnectionToCreditsafe(_log);
                sftpClient.Connect();
                if (!sftpClient.IsConnected)
                    throw new Exception($"Unable to connect to sftp-server");
                sftpClient.ChangeDirectory("/");

                var fileList = sftpClient.ListDirectory("OUTFILE").ToList();

                _log.Info($"Files in OUTFILE: {fileList.Count}");

                if (fileList.Count > 1)
                {
                    _log.Info($"More than 0 files in OUTFILE");
                    //if ("History".Equals(fileList[0].ToString()))
                    //{
                    var x = 0;

                    if (fileList[0].Name.ToString() == "History")
                    {
                        x = 1;
                    }
                    else
                    {
                        x = 0;
                    }

                    if (fileList[x].Name.ToString().StartsWith(Properties.Settings.Default.OutputFileNameStart) && fileList[x].Name.ToString().EndsWith(".txt")) //was fileList[1] when there is a history map
                    {
                        _log.Info($"Found a file  {fileList[x].Name.ToString()}");

                        using (Stream RetrivedFile = File.OpenWrite(Properties.Settings.Default.DeltabatchRetrievedFileLocation + fileList[x].Name.ToString()))
                        {
                            sftpClient.DownloadFile($"OUTFILE/{fileList[x].Name.ToString()}", RetrivedFile);
                        }

                        _log.Info($"File downloaded");
                        _currentOutputFileName = fileList[x].Name.ToString();

                        //sftpClient.ChangeDirectory($"OUTFILE/History/");

                        //using (var uplfileStream = System.IO.File.OpenRead(fileList[1].ToString()))
                        //{
                        //    sftpClient.UploadFile(uplfileStream, fileList[1].ToString(), true);
                        //}

                        //using (var fileStream = new FileStream(plusFileName, FileMode.Open))
                        //{
                        //    sftpClient.UploadFile(fileStream, Path.GetFileName(plusFileName));
                        //}

                        sftpClient.Delete($"OUTFILE/{fileList[x].Name.ToString()}");

                        _log.Info($"File deleted from server");
                        //var prop = sftpClient.GetType().GetProperty("SftpChannel", BindingFlags.NonPublic | BindingFlags.Instance);
                        //var methodInfo = prop.GetGetMethod(true);
                        //var sftpChannel = methodInfo.Invoke(sftpClient, null);
                        //((ChannelSftp)sftpChannel).rm($"/OUTFILE/{fileList[1].ToString()}");
                    }
                    else
                        throw new Exception($"Invalid file/folder structure in /Output folder in Creditsafe ftp-server.\nSecond item expected to be '{Properties.Settings.Default.OutputFileNameStart}*...*.txt', but was in fact '{fileList[1].ToString()}'");
                    //}
                    //else
                    //  throw new Exception($"Invalid file/folder structure in /Output folder in Creditsafe ftp-server.\nFirst item expected to be 'History', but was in fact '{fileList[0].ToString()}'");
                }
                else
                {
                    switch (fileList.Count)
                    {
                        case 0:
                            throw new Exception("No Content found in /OUTFILE folder in Creditsafe ftp-server");
                        default:
                            throw new Exception($"Found only {fileList[0].Name.ToString()} in /OUTFILE folder in Creditsafe ftp-server");
                    }
                }
                sftpClient.Disconnect();
            }
            catch (Exception e)
            {
                _log.Error($"Exception caught in RetrieveFiles():\n{e.Message}\n\n{e}");
                throw e;
            }
            finally
            {
                //Close sftp
                try { sftpClient.Disconnect(); }
                catch { }

                try { sftpClient = null; }
                catch { }

                try { GC.Collect(); }
                catch { }
            }
        }
    }
}
