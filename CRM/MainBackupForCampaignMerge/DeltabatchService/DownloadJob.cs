using Common.Logging;
using Quartz;
using Skanetrafiken.Crm.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using System.Linq;
using System.Collections;
using Microsoft.Xrm.Sdk.Query;
using Tamir.SharpSsh;
using System.Reflection;
using Tamir.SharpSsh.jsch;
using Microsoft.Xrm.Sdk;
using System.Globalization;

namespace Endeavor.Crm.DeltabatchService
{
    [DisallowConcurrentExecution, PersistJobDataAfterExecution]
    public class DownloadJob : IJob
    {
        public const string DataMapModifiedAfter = "ModifiedAfterDownload";
        public const string GroupName = "Download Schedule";
        public const string TriggerDescription = "Download Schedule Trigger";
        public const string JobDescription = "Download Schedule Job";
        public const string TriggerName = "DownloadTrigger";
        public const string JobName = "DownloadJob";

        protected Sftp sftp;

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
                
                RetrieveFile();

                UpdateContactsWithNewInfo(localContext);

                ArchiveFile();

                _log.Info("DownloadJob Done!");
            }
            catch (Exception e)
            {
                _log.Error($"Exception caught in ExecuteJob():\n{e.Message}\n\n{e}");
                //if (localContext != null)
                //    DeltabatchJobHelper.SendErrorMailToDev(localContext, e);
            }
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
            for (int i=0; i < retrievedFilesInfo.Length; i++)
            {
                File.Move($"{retrievedFileLocation.FullName}\\{retrievedFilesInfo[i]}", $"{retrievedFileHistoryLocation.FullName}\\{retrievedFilesInfo[i]}");
            }
        }

        public void UpdateContactsWithNewInfo(Plugin.LocalPluginContext localContext)
        {
            _log.Info("Entered UpdateContactsWithNewInfo()");
            try
            {
                string outputFilePath = $"{Properties.Settings.Default.DeltabatchRetrievedFileLocation}/{_currentOutputFileName}";
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
                                if (!string.IsNullOrWhiteSpace(outputLine))
                                {
                                    string[] lineParams = outputLine.Split(';');
                                    if (lineParams.Length != _fieldNames.Length)
                                    {
                                        //throw new Exception($"Incomplete responseRow: {outputLine}, expected number of parameters: {_fieldNames.Length}");
                                        _errors.Add(new Tuple<string[], Exception>(lineParams, new Exception($"Incomplete responseRow: {outputLine}, expected number of parameters: {_fieldNames.Length}")));
                                        continue;
                                    }
                                    //add params to log entity
                                    DeltabatchLogEntity credLog = new DeltabatchLogEntity
                                    {
                                        ed_CreditsafeParameters = outputLine
                                    };

                                    bool update = false, rejected = false;
                                    //if (numberOfOperations % 5000 < 3)
                                    //{
                                    //    _log.Info($"Iteration {numberOfOperations}, starting to parse to updateContact");
                                    //}
                                    ContactEntity updateEntity = ParseChangedFieldsToAlteredContactEntity(localContext, lineParams, ref credLog, ref update, ref rejected);

                                    //if (numberOfOperations % 5000 < 3)
                                    //{
                                    //    _log.Info($"Creating credLog");
                                    //}
                                    XrmHelper.Create(localContext, credLog);
                                    if (updateEntity == null)
                                        continue;

                                    if (rejected || update)
                                    {
                                        updateEntity.ed_InformationSource = Generated.ed_informationsource.Folkbokforing;

                                        //if (numberOfOperations % 5000 < 3)
                                        //{
                                        //    _log.Info($"Updating Contact");
                                        //}
                                        //_log.Debug($"Updating Contact.");
                                        XrmHelper.Update(localContext, updateEntity);
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
                                //localContext = DeltabatchJobHelper.GenerateLocalContext();
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
        
        private ContactEntity ParseChangedFieldsToAlteredContactEntity(Plugin.LocalPluginContext localContext, string[] lineParams, ref DeltabatchLogEntity deltaLog, ref bool update, ref bool rejected)
        {
            _log.Debug("Entered ParseChangedFieldsToAlteredContactEntity()");
            if (numberOfOperations % 5000 < 3)
            {
                _log.Info($"Iteration {numberOfOperations}, Entered ParseChangedFieldsToAlteredContactEntity");
            }
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

            string socSec = lineParams[Array.IndexOf(_fieldNames, _socSecFieldKeyword)];
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
            deltaLog.ed_SpecialRegisteredAddress = specialRegAddr.Length < 100 ? specialRegAddr : specialRegAddr.Substring(0,99); // field is limited to 100 characters
            string dateString = lineParams[Array.IndexOf(_fieldNames, _searchDateFieldKeyword)];
            deltaLog.ed_SearchDate = dateString;
            string county = lineParams[Array.IndexOf(_fieldNames, _countyFieldKeyword)];
            deltaLog.ed_County = county;
            string countyNumberString = lineParams[Array.IndexOf(_fieldNames, _countyNumberFieldKeyword)];
            deltaLog.ed_CountyNumber = countyNumberString;
            string communityNumberString = lineParams[Array.IndexOf(_fieldNames, _communityNumberFieldKeyword)];
            deltaLog.ed_CommunityNumber = communityNumberString;
            #endregion


            if (string.IsNullOrWhiteSpace(transactionGuid))
            {
                _errors.Add(new Tuple<string[], Exception>(lineParams, new Exception("Could not update this Contact due to lack of TransactionId(Guid)")));
                return null;
            }

            Guid guid = Guid.Empty;
            if (!Guid.TryParse(transactionGuid, out guid))
            {
                _errors.Add(new Tuple<string[], Exception>(lineParams, new Exception($"Could not update this Contact because TransactionId was not a valid Guid, {transactionGuid}")));
                return null;
            }
            if (guid == Guid.Empty)
            {
                _errors.Add(new Tuple<string[], Exception>(lineParams, new Exception($"{transactionGuid} was parsed to an empty Guid")));
                return null;
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
                ContactEntity.Fields.Address2_Country);
            ContactEntity existingContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, guid, contactColumnsWithAddress2);
            //ContactEntity existingContact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, contactColumnsWithAddress2,
            //    new FilterExpression
            //    {
            //        Conditions =
            //        {
            //            new ConditionExpression(ContactEntity.Fields.ContactId, ConditionOperator.Equal, guid)
            //        }
            //    });
            if (numberOfOperations % 5000 < 3)
            {
                _log.Info($"Retrieve Done");
            }

            if (existingContact == null)
            {
                _errors.Add(new Tuple<string[], Exception>(lineParams, new Exception($"Could not find a contact with id {guid}. Please handle manually. SocSecNr = {socSec}")));
                return null;
            }
            deltaLog.ed_RegardingContact = existingContact.ToEntityReference();

            ContactEntity updateContact = new ContactEntity
            {
                ContactId = existingContact.ContactId,
                Id = existingContact.Id,
                ed_UpdatedFBDate = DateTime.Now
            };

            if (!string.IsNullOrWhiteSpace(rejectCode))
            {
                string rejectErrorMess;
                Generated.ed_creditsaferejectcodes? rejectOptionSetValue = TranslateIntToRejectionOptionSetValue(localContext, rejectCode, out rejectErrorMess);
                if (rejectOptionSetValue == null)
                {
                    _errors.Add(new Tuple<string[], Exception>(lineParams, new Exception($"Reject code {rejectCode} could not be translated to OptionSetValue. Please consult a developer.\nInner Error message: {rejectErrorMess}")));
                    return null;
                }
                if (existingContact.ed_CreditsafeRejectionCode != rejectOptionSetValue)
                    updateContact.ed_CreditsafeRejectionCode = rejectOptionSetValue;
                if (existingContact.ed_CreditsafeRejectionText != rejectText)
                    updateContact.ed_CreditsafeRejectionText = rejectText;
                if (existingContact.ed_CreditsafeRejectionComment != rejectComment)
                    updateContact.ed_CreditsafeRejectionComment = rejectComment;
                rejected = true;

                if (rejectOptionSetValue == Generated.ed_creditsaferejectcodes.Deceased)
                {
                    updateContact.ed_Deceased = true;
                    DateTime parsedDate;
                    if (DateTime.TryParse(rejectComment, new CultureInfo("sv-SE"), DateTimeStyles.AssumeLocal, out parsedDate))
                        updateContact.ed_DeceasedDate = parsedDate;
                    else
                        updateContact.ed_DeceasedDate = DateTime.Now;
                }
                else if (rejectOptionSetValue == Generated.ed_creditsaferejectcodes.Emigrated)
                {
                    #region C/O Address - Special
                    if (existingContact.Address2_Line1 != specialCo)
                    {
                        updateContact.Address2_Line1 = specialCo;
                    }
                    #endregion
                    #region PostalCode - Special
                    if (existingContact.Address2_PostalCode != specialPostalCode)
                    {
                        updateContact.Address2_PostalCode = specialPostalCode;
                    }
                    #endregion
                    #region Country - Special
                    if (existingContact.Address2_Country != specialCountry)
                    {
                        updateContact.Address2_Country = specialCountry;
                    }
                    #endregion
                    #region City - Special
                    if (existingContact.Address2_City != specialCity)
                    {
                        updateContact.Address2_City = specialCity;
                    }
                    #endregion
                    #region AddressField - Special
                    if (existingContact.Address2_Line2 != specialRegAddr)
                    {
                        updateContact.Address2_Line2 = specialRegAddr;
                    }
                    #endregion
                }
                if (existingContact.ed_UpdatedFB != true)
                    updateContact.ed_UpdatedFB = true;

                if (numberOfOperations % 5000 < 3)
                {
                    _log.Info($"Rejected, returning rejected Contact");
                }
                return updateContact;
            }

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
                update = true;
            }
            #endregion
            #region FirstName
            string firstNameToUse = !string.IsNullOrWhiteSpace(givenName) ? givenName : firstName;
            if (existingContact.FirstName != firstNameToUse)
            {
                updateContact.FirstName = firstNameToUse;
                update = true;
            }
            #endregion
            #region LastName
            if (existingContact.LastName != lastName)
            {
                updateContact.LastName = lastName;
                update = true;
            }
            #endregion
            #region C/O Address
            if (existingContact.Address1_Line1 != coAddress)
            {
                updateContact.Address1_Line1 = coAddress;
                update = true;
            }
            #endregion
            #region AddressField
            if (existingContact.Address1_Line2 != addressLine)
            {
                updateContact.Address1_Line2 = addressLine;
                update = true;
            }
            #endregion
            #region PostalCode
            if (existingContact.Address1_PostalCode != postalCode)
            {
                updateContact.Address1_PostalCode = postalCode;
                update = true;
            }
            #endregion
            #region City
            if (existingContact.Address1_City != city)
            {
                updateContact.Address1_City = city;
                update = true;
            }
            #endregion
            #region Community
            if (existingContact.ed_Address1_Community != community)
            {
                updateContact.ed_Address1_Community = community;
                update = true;
            }
            #endregion
            #region C/O Address - Special
            if (existingContact.Address2_Line1 != specialCo)
            {
                updateContact.Address2_Line1 = specialCo;
                update = true;
            }
            #endregion
            // INFO - Should not be used. Is always a combination of co and registered
            //#region CompositeAddressField - Special
            //if (existingContact.Address2_Composite != lineParams[Array.IndexOf(_fieldNames, _specAddressFieldKeyword)])
            //{
            //    incomingContact.Address2_Composite = lineParams[Array.IndexOf(_fieldNames, _specAddressFieldKeyword)];
            //    update = true;
            //}
            //#endregion
            #region PostalCode - Special
            if (existingContact.Address2_PostalCode != specialPostalCode)
            {
                updateContact.Address2_PostalCode = specialPostalCode;
                update = true;
            }
            #endregion
            #region Country - Special
            if (existingContact.Address2_Country != specialCountry)
            {
                updateContact.Address2_Country = specialCountry;
                update = true;
            }
            #endregion
            #region City - Special
            if (existingContact.Address2_City != specialCity)
            {
                updateContact.Address2_City = specialCity;
                update = true;
            }
            #endregion
            #region AddressField - Special
            if (existingContact.Address2_Line2 != specialRegAddr)
            {
                updateContact.Address2_Line2 = specialRegAddr;
                update = true;
            }
            #endregion
            //#region SearchDate
            //if (dateString.Length != 8)
            //{
            //    _errors.Add(new Tuple<string[], Exception>(lineParams, new Exception($"Invalid parameter SEARCH_DATE: {dateString} was expected to be 8 integers")));
            //    return null;
            //}
            //string yearString = dateString.Substring(0, 4);
            //string monthString = dateString.Substring(4, 2);
            //string dayString = dateString.Substring(6, 2);
            //int year, month, day;
            //if (!int.TryParse(yearString, out year) || !int.TryParse(monthString, out month) || !int.TryParse(dayString, out day))
            //{
            //    _errors.Add(new Tuple<string[], Exception>(lineParams, new Exception($"Invalid parameter SEARCH_DATE: {dateString} was expected to be 8 integers")));
            //    return null;
            //}
            //if (1 > month || month > 12)
            //{
            //    _errors.Add(new Tuple<string[], Exception>(lineParams, new Exception($"Invalid parameter SEARCH_DATE({dateString}), the month-part({month}) needs to be between 1 and 12.")));
            //    return null;
            //}
            //if (1 > day || day > DateTime.DaysInMonth(year, month))
            //{
            //    _errors.Add(new Tuple<string[], Exception>(lineParams, new Exception($"Invalid parameter SEARCH_DATE({dateString}), the day-part({day}) needs to be between 1 and {DateTime.DaysInMonth(year, month)} for month number {month}, year {year}.")));
            //    return null;
            //}
            //DateTime parsedSearchDate = new DateTime(year, month, day);
            //if (existingContact.ed_UpdatedFBDate != parsedSearchDate)
            //{
            //    updateContact.ed_UpdatedFBDate = parsedSearchDate;
            //}
            //#endregion
            #region County
            if (existingContact.Address1_County != county)
            {
                updateContact.Address1_County = county;
                update = true;
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
                        update = true;
                    }
                }
                else if (existingContact.ed_Address1_CountyNumber != countyNumber)
                {
                    updateContact.ed_Address1_CountyNumber = countyNumber;
                    update = true;
                }
            }
            else
            {
                _log.Error($"Parameter {_countyNumberFieldKeyword} with value {countyNumberString} could not be parsed to an integer. Field ignored");
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
                        update = true;
                    }
                }
                else if (existingContact.ed_Address1_CommunityNumber != communityNumber)
                {
                    updateContact.ed_Address1_CommunityNumber = communityNumber;
                    update = true;
                }
            }
            else
            {
                _log.Error($"Parameter {_communityNumberFieldKeyword} with value {communityNumberString} could not be parsed to an integer. Field ignored");
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
            if (update && existingContact.ed_UpdatedFB != true)
                updateContact.ed_UpdatedFB = true;

            if (numberOfOperations % 5000 < 3)
            {
                _log.Info($"Returning updated Contact, update = {update}");
            }
            return updateContact;
        }

        private Generated.ed_creditsaferejectcodes? TranslateIntToRejectionOptionSetValue(Plugin.LocalPluginContext localContext, string rejectCode, out string rejectErrorMess)
        {
            rejectCode = rejectCode.Replace("S", "10");
            int rejectOptionSetCode;
            if (!int.TryParse(rejectCode, out rejectOptionSetCode))
            {
                rejectErrorMess= $"Could not parse {rejectCode} to integer.";
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

        //private static bool StatusChanged(ref ContactEntity updateEntity ,IEnumerable<KeyValuePair<string, string>> orderedFields)
        //{
        //    string rejectCode = orderedFields.FirstOrDefault(kvp => "REJECT_CODE".Equals(kvp.Key)).Value;
        //    string rejectText = orderedFields.FirstOrDefault(kvp => "REJECT_TEXT".Equals(kvp.Key)).Value;
        //    string rejectComment = orderedFields.FirstOrDefault(kvp => "REJECT_COMMENT".Equals(kvp.Key)).Value;

        //    if ("S4".Equals(rejectCode))
        //    {
        //        // Avliden
        //        string[] commentDateFields = rejectComment?.Split("-".ToCharArray());
        //        int year, month, day;
        //        DateTime deathDate;
        //        if (commentDateFields.Length == 3 && int.TryParse(commentDateFields[0], out year) && int.TryParse(commentDateFields[1], out month) && int.TryParse(commentDateFields[2], out day))
        //        {
        //            deathDate = new DateTime(year, month, day);
        //        }
        //        else
        //        {
        //            throw new Exception($"Unrecognised format in REJECT-COMMENT: {rejectComment}, expected 'yyyy-mm-dd'");
        //        }

        //        updateEntity.StatusCode = Generated.contact_statuscode.Inactive;
        //        return true;
        //    }
        //    return false;
        //}

        public void RetrieveFile()
        {
            _log.Info("Entered RetrieveFile()");
            sftp = null;
            try
            {
                sftp = DeltabatchJobHelper.CreateSftpConnectionToCreditsafe();
                sftp.Connect(Properties.Settings.Default.CreditsafeFtpPort);
                if (!sftp.Connected)
                    throw new Exception($"Unable to connect to sftp-server");
                ArrayList fileList = sftp.GetFileList("/Outfile");
                if (fileList.Count == 2)
                {
                    if ("History".Equals(fileList[0].ToString()))
                    {
                        if (fileList[1].ToString().StartsWith(Properties.Settings.Default.OutputFileNameStart) && fileList[1].ToString().EndsWith(".txt"))
                        {
                            sftp.Get($"/Outfile/{fileList[1].ToString()}", Properties.Settings.Default.DeltabatchRetrievedFileLocation);
                            _currentOutputFileName = fileList[1].ToString();
                            sftp.Put($"{Properties.Settings.Default.DeltabatchRetrievedFileLocation}/{fileList[1].ToString()}", $"/Outfile/History/{fileList[1].ToString()}");

                            var prop = sftp.GetType().GetProperty("SftpChannel", BindingFlags.NonPublic | BindingFlags.Instance);
                            var methodInfo = prop.GetGetMethod(true);
                            var sftpChannel = methodInfo.Invoke(sftp, null);
                            ((ChannelSftp)sftpChannel).rm($"/Outfile/{fileList[1].ToString()}");
                        }
                        else
                            throw new Exception($"Invalid file/folder structure in /Output folder in Creditsafe ftp-server.\nSecond item expected to be '{Properties.Settings.Default.OutputFileNameStart}*...*.txt', but was in fact '{fileList[1].ToString()}'");
                    }
                    else
                        throw new Exception($"Invalid file/folder structure in /Output folder in Creditsafe ftp-server.\nFirst item expected to be 'History', but was in fact '{fileList[0].ToString()}'");
                }
                else
                {
                    switch (fileList.Count)
                    {
                        case 0:
                            throw new Exception("No Content found in /Outfile folder in Creditsafe ftp-server");
                        case 1:
                            throw new Exception($"Found only {fileList[0].ToString()} in /Outfile folder in Creditsafe ftp-server");
                        default:
                            throw new Exception($"Found too many items in /Outfile folder in Creditsafe ftp-server");
                    }
                }
                sftp.Close();
            }
            catch (Exception e)
            {
                _log.Error($"Exception caught in RetrieveFiles():\n{e.Message}\n\n{e}");
                throw e;
            }
            finally
            {
                //Close sftp
                try { sftp.Close(); }
                catch { }

                try { sftp = null; }
                catch { }

                try { GC.Collect(); }
                catch { }
            }
        }
    }
}
