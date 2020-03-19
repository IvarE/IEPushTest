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

namespace Endeavor.Crm.DeltabatchService
{
    [DisallowConcurrentExecution, PersistJobDataAfterExecution]
    public class DownloadJob : DeltabatchJob
    {
        public const string JobName = "DownloadJob";
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

        new private ILog _log = LogManager.GetLogger(typeof(DownloadJob));

        new public void Execute(IJobExecutionContext context)
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
            try
            {
                Plugin.LocalPluginContext localContext = GenerateLocalContext();
                
                RetrieveFile();

                UpdateContactsWithNewInfo(localContext);
            }
            catch (Exception e)
            {
                _log.Error($"Exception caught in ExecuteJob():\n{e.Message}\n\n{e}");
                throw e;
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
                StreamReader file = new StreamReader(outputFilePath);
                string outputLine;
                _fieldNames = file.ReadLine().Split(';');
                VerifyPresenceOfAllFields();
                while ((outputLine = file.ReadLine()) != null)
                {
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

                            bool update = false, rejected = false;

                            ContactEntity updateEntity = ParseChangedFieldsToAlteredContactEntity(localContext, lineParams, ref update, ref rejected);
                            if (updateEntity == null)
                                continue;

                            if (rejected || update)
                            {
                                if (rejected)
                                {

                                }
                                else if (update)
                                {
                                    updateEntity.ed_InformationSource = Generated.ed_informationsource.Folkbokforing;
                                }
                                _log.Info($"Updating Contact.");
                                XrmHelper.Update(localContext, updateEntity);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        _errors.Add(new Tuple<string[], Exception>(new string[] {outputLine}, e));
                    }
                }
                
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
                    string errorMess = tuple.Item2?.Message != null ? tuple.Item2.Message : "Error Message missing";
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
                return false;
            }
            if (Array.IndexOf(_fieldNames, _guidFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_guidFieldKeyword}")));
                return false;
            }
            if (Array.IndexOf(_fieldNames, _freetextFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_freetextFieldKeyword}")));
                return false;
            }
            if (Array.IndexOf(_fieldNames, _changeCodesFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_changeCodesFieldKeyword}")));
                return false;
            }
            if (Array.IndexOf(_fieldNames, _firstNameFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_firstNameFieldKeyword}")));
                return false;
            }
            if (Array.IndexOf(_fieldNames, _givenNameFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_givenNameFieldKeyword}")));
                return false;
            }
            if (Array.IndexOf(_fieldNames, _lastNameFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_lastNameFieldKeyword}")));
                return false;
            }
            if (Array.IndexOf(_fieldNames, _coAddressFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_coAddressFieldKeyword}")));
                return false;
            }
            if (Array.IndexOf(_fieldNames, _registeredAddressFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_registeredAddressFieldKeyword}")));
                return false;
            }
            if (Array.IndexOf(_fieldNames, _postalCodeFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_postalCodeFieldKeyword}")));
                return false;
            }
            if (Array.IndexOf(_fieldNames, _cityFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_cityFieldKeyword}")));
                return false;
            }
            if (Array.IndexOf(_fieldNames, _communityFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_communityFieldKeyword}")));
                return false;
            }
            if (Array.IndexOf(_fieldNames, _specCoAddressFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_specCoAddressFieldKeyword}")));
                return false;
            }
            if (Array.IndexOf(_fieldNames, _specAddressFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_specAddressFieldKeyword}")));
                return false;
            }
            if (Array.IndexOf(_fieldNames, _specPostalCodeFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_specPostalCodeFieldKeyword}")));
                return false;
            }
            if (Array.IndexOf(_fieldNames, _specCountryFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_specCountryFieldKeyword}")));
                return false;
            }
            if (Array.IndexOf(_fieldNames, _specCityFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_specCityFieldKeyword}")));
                return false;
            }
            if (Array.IndexOf(_fieldNames, _specRegisteredAddressFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_specRegisteredAddressFieldKeyword}")));
                return false;
            }
            if (Array.IndexOf(_fieldNames, _searchDateFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_searchDateFieldKeyword}")));
                return false;
            }
            if (Array.IndexOf(_fieldNames, _countyFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_countyFieldKeyword}")));
                return false;
            }
            if (Array.IndexOf(_fieldNames, _countyNumberFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_countyNumberFieldKeyword}")));
                return false;
            }
            if (Array.IndexOf(_fieldNames, _communityNumberFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_communityNumberFieldKeyword}")));
                return false;
            }
            if (Array.IndexOf(_fieldNames, _rejectCodeFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_rejectCodeFieldKeyword}")));
                return false;
            }
            if (Array.IndexOf(_fieldNames, _rejectTextFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_rejectTextFieldKeyword}")));
                return false;
            }
            if (Array.IndexOf(_fieldNames, _rejectCommentFieldKeyword) < 0)
            {
                _errors.Add(new Tuple<string[], Exception>(_fieldNames, new Exception($"Missing parameter: {_rejectCommentFieldKeyword}")));
                return false;
            }
            return true;
        }
        
        private ContactEntity ParseChangedFieldsToAlteredContactEntity(Plugin.LocalPluginContext localContext, string[] lineParams, ref bool update, ref bool rejected)
        {
            _log.Info("Entered ParseChangedFieldsToAlteredContactEntity()");
            if (string.IsNullOrWhiteSpace(lineParams[Array.IndexOf(_fieldNames, _guidFieldKeyword)]))
            {
                _errors.Add(new Tuple<string[], Exception>(lineParams, new Exception("Could not update this Contact due to lack of TransactionId(Guid)")));
                return null;
            }
            
            Guid guid = Guid.Empty;
            if (!Guid.TryParse(lineParams[Array.IndexOf(_fieldNames, _guidFieldKeyword)], out guid))
            {
                _errors.Add(new Tuple<string[], Exception>(lineParams, new Exception("Could not update this Contact because TransactionId was not a valid Guid")));
                return null;
            }

            ColumnSet contactColumnsWithAddress2 = ContactEntity.ContactInfoBlock;
            contactColumnsWithAddress2.AddColumns(
                ContactEntity.Fields.Address2_Line1,
                ContactEntity.Fields.Address2_Line2,
                ContactEntity.Fields.Address2_PostalCode,
                ContactEntity.Fields.Address2_City,
                ContactEntity.Fields.Address2_Country);
            ContactEntity existingContact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, contactColumnsWithAddress2,
                new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression(ContactEntity.Fields.ContactId, ConditionOperator.Equal, guid)
                    }
                });

            if (existingContact == null)
            {
                _errors.Add(new Tuple<string[], Exception>(lineParams, new Exception($"Could not find a contact with id {guid}. Please handle manually.")));
                return null;
            }

            ContactEntity incomingContact = new ContactEntity
            {
                ContactId = existingContact.ContactId,
                Id = existingContact.Id
            };

            if (!string.IsNullOrWhiteSpace(lineParams[Array.IndexOf(_fieldNames, _rejectCodeFieldKeyword)]))
            {
                string rejectCode = lineParams[Array.IndexOf(_fieldNames, _rejectCodeFieldKeyword)];
                string rejectText = lineParams[Array.IndexOf(_fieldNames, _rejectTextFieldKeyword)];
                string rejectComment = lineParams[Array.IndexOf(_fieldNames, _rejectCommentFieldKeyword)];

                string rejectErrorMess;
                Generated.ed_creditsaferejectcodes? rejectOptionSetValue = TranslateIntToRejectionOptionSetValue(localContext, rejectCode, out rejectErrorMess);
                if (rejectOptionSetValue == null)
                {
                    _errors.Add(new Tuple<string[], Exception>(lineParams, new Exception($"Reject code {rejectCode} could not be translated to OptionSetValue. Please consult a developer.\nInner Error message: {rejectErrorMess}")));
                    return null;
                }
                incomingContact.ed_CreditsafeRejectionCode = rejectOptionSetValue;
                incomingContact.ed_CreditsafeRejectionText = rejectText;
                incomingContact.ed_CreditsafeRejectionComment = rejectComment;

                rejected = true;
                return incomingContact;
            }

            #region SocialSecurityNumber
            if (existingContact.cgi_socialsecuritynumber != lineParams[Array.IndexOf(_fieldNames, _socSecFieldKeyword)])
            {
                incomingContact.cgi_socialsecuritynumber = lineParams[Array.IndexOf(_fieldNames, _socSecFieldKeyword)];
                update = true;
            }
            #endregion
            #region FirstName
            string firstNameToUse = string.IsNullOrWhiteSpace(lineParams[Array.IndexOf(_fieldNames, _givenNameFieldKeyword)]) ? lineParams[Array.IndexOf(_fieldNames, _firstNameFieldKeyword)] : lineParams[Array.IndexOf(_fieldNames, _givenNameFieldKeyword)];
            if (existingContact.FirstName != firstNameToUse)
            {
                incomingContact.FirstName = firstNameToUse;
                update = true;
            }
            #endregion
            #region LastName
            if (existingContact.LastName != lineParams[Array.IndexOf(_fieldNames, _lastNameFieldKeyword)])
            {
                incomingContact.LastName = lineParams[Array.IndexOf(_fieldNames, _lastNameFieldKeyword)];
                update = true;
            }
            #endregion
            #region C/O Address
            if (existingContact.Address1_Line1 != lineParams[Array.IndexOf(_fieldNames, _coAddressFieldKeyword)])
            {
                incomingContact.Address1_Line1 = lineParams[Array.IndexOf(_fieldNames, _coAddressFieldKeyword)];
                update = true;
            }
            #endregion
            #region AddressField
            if (existingContact.Address1_Line2 != lineParams[Array.IndexOf(_fieldNames, _registeredAddressFieldKeyword)])
            {
                incomingContact.Address1_Line2 = lineParams[Array.IndexOf(_fieldNames, _registeredAddressFieldKeyword)];
                update = true;
            }
            #endregion
            #region PostalCode
            if (existingContact.Address1_PostalCode != lineParams[Array.IndexOf(_fieldNames, _postalCodeFieldKeyword)])
            {
                incomingContact.Address1_PostalCode = lineParams[Array.IndexOf(_fieldNames, _postalCodeFieldKeyword)];
                update = true;
            }
            #endregion
            #region City
            if (existingContact.Address1_City != lineParams[Array.IndexOf(_fieldNames, _cityFieldKeyword)])
            {
                incomingContact.Address1_City = lineParams[Array.IndexOf(_fieldNames, _cityFieldKeyword)];
                update = true;
            }
            #endregion
            #region Community
            if (existingContact.ed_Address1_Community != lineParams[Array.IndexOf(_fieldNames, _communityFieldKeyword)])
            {
                incomingContact.ed_Address1_Community = lineParams[Array.IndexOf(_fieldNames, _communityFieldKeyword)];
                update = true;
            }
            #endregion
            #region C/O Address - Special
            if (existingContact.Address2_Line1 != lineParams[Array.IndexOf(_fieldNames, _specCoAddressFieldKeyword)])
            {
                incomingContact.Address2_Line1 = lineParams[Array.IndexOf(_fieldNames, _specCoAddressFieldKeyword)];
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
            if (existingContact.Address2_PostalCode != lineParams[Array.IndexOf(_fieldNames, _specPostalCodeFieldKeyword)])
            {
                incomingContact.Address2_PostalCode = lineParams[Array.IndexOf(_fieldNames, _specPostalCodeFieldKeyword)];
                update = true;
            }
            #endregion
            #region Country - Special
            if (existingContact.Address2_Country != lineParams[Array.IndexOf(_fieldNames, _specCountryFieldKeyword)])
            {
                incomingContact.Address2_Country = lineParams[Array.IndexOf(_fieldNames, _specCountryFieldKeyword)];
                update = true;
            }
            #endregion
            #region City - Special
            if (existingContact.Address2_City != lineParams[Array.IndexOf(_fieldNames, _specCityFieldKeyword)])
            {
                incomingContact.Address2_City = lineParams[Array.IndexOf(_fieldNames, _specCityFieldKeyword)];
                update = true;
            }
            #endregion
            #region AddressField - Special
            if (existingContact.Address2_Line2 != lineParams[Array.IndexOf(_fieldNames, _specRegisteredAddressFieldKeyword)])
            {
                incomingContact.Address2_Line2 = lineParams[Array.IndexOf(_fieldNames, _specRegisteredAddressFieldKeyword)];
                update = true;
            }
            #endregion
            #region SearchDate
            string dateString = lineParams[Array.IndexOf(_fieldNames, _searchDateFieldKeyword)];
            if (dateString.Length != 8)
            {
                _errors.Add(new Tuple<string[], Exception>(lineParams, new Exception($"Invalid parameter SEARCH_DATE: {dateString} was expected to be 8 integers")));
                return null;
            }
            string yearString = dateString.Substring(0, 4);
            string monthString = dateString.Substring(4, 2);
            string dayString = dateString.Substring(6, 2);
            int year, month, day;
            if (!int.TryParse(yearString, out year) || !int.TryParse(monthString, out month) || !int.TryParse(dayString, out day))
            {
                _errors.Add(new Tuple<string[], Exception>(lineParams, new Exception($"Invalid parameter SEARCH_DATE: {dateString} was expected to be 8 integers")));
                return null;
            }
            if (1 > month || month > 12)
            {
                _errors.Add(new Tuple<string[], Exception>(lineParams, new Exception($"Invalid parameter SEARCH_DATE({dateString}), the month-part({month}) needs to be between 1 and 12.")));
                return null;
            }
            if (1 > day || day > DateTime.DaysInMonth(year, month))
            {
                _errors.Add(new Tuple<string[], Exception>(lineParams, new Exception($"Invalid parameter SEARCH_DATE({dateString}), the day-part({day}) needs to be between 1 and {DateTime.DaysInMonth(year, month)} for month number {month}, year {year}.")));
                return null;
            }
            DateTime parsedSearchDate = new DateTime(year, month, day);
            if (existingContact.ed_UpdatedFBDate != parsedSearchDate)
            {
                incomingContact.ed_UpdatedFBDate = parsedSearchDate;
                update = true;
            }
            #endregion
            #region County
            if (existingContact.Address1_County != lineParams[Array.IndexOf(_fieldNames, _countyFieldKeyword)])
            {
                incomingContact.Address1_County = lineParams[Array.IndexOf(_fieldNames, _countyFieldKeyword)];
                update = true;
            }
            #endregion
            #region CountyNumber
            int countyNumber;
            if (int.TryParse(lineParams[Array.IndexOf(_fieldNames, _countyNumberFieldKeyword)], out countyNumber) || "".Equals(lineParams[Array.IndexOf(_fieldNames, _countyNumberFieldKeyword)]))
            {
                if ("".Equals(lineParams[Array.IndexOf(_fieldNames, _countyNumberFieldKeyword)]))
                {
                    if (existingContact.ed_Address1_CountyNumber != null)
                    {
                        incomingContact.ed_Address1_CountyNumber = null;
                        update = true;
                    }
                }
                else if (existingContact.ed_Address1_CountyNumber != countyNumber)
                {
                    incomingContact.ed_Address1_CountyNumber = countyNumber;
                    update = true;
                }
            }
            else
            {
                _log.Error($"Parameter {_countyNumberFieldKeyword} with value {lineParams[Array.IndexOf(_fieldNames, _countyNumberFieldKeyword)]} could not be parsed to an integer. Field ignored");
            }
            #endregion
            #region CommunityNumber
            int communityNumber;
            if (int.TryParse(lineParams[Array.IndexOf(_fieldNames, _communityNumberFieldKeyword)], out communityNumber) || "".Equals(lineParams[Array.IndexOf(_fieldNames, _communityNumberFieldKeyword)]))
            {
                if ("".Equals(lineParams[Array.IndexOf(_fieldNames, _communityNumberFieldKeyword)]))
                {
                    if (existingContact.ed_Address1_CommunityNumber != null)
                    {
                        incomingContact.ed_Address1_CommunityNumber = null;
                        update = true;
                    }
                }
                else if (existingContact.ed_Address1_CommunityNumber != communityNumber)
                {
                    incomingContact.ed_Address1_CommunityNumber = communityNumber;
                    update = true;
                }
            }
            else
            {
                _log.Error($"Parameter {_communityNumberFieldKeyword} with value {lineParams[Array.IndexOf(_fieldNames, _communityNumberFieldKeyword)]} could not be parsed to an integer. Field ignored");
            }
            #endregion
            if (update && existingContact.ed_UpdatedFB != true)
                incomingContact.ed_UpdatedFB = true;

            return incomingContact;
        }

        private Generated.ed_creditsaferejectcodes? TranslateIntToRejectionOptionSetValue(Plugin.LocalPluginContext localContext, string rejectCode, out string rejectErrorMess)
        {
            rejectCode.Replace("S", "10");
            int rejectOptionSetCode;
            if (!int.TryParse(rejectCode, out rejectOptionSetCode))
            {
                rejectErrorMess= $"Could not parse {rejectOptionSetCode} to integer.";
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

        private static bool StatusChanged(ref ContactEntity updateEntity ,IEnumerable<KeyValuePair<string, string>> orderedFields)
        {
            string rejectCode = orderedFields.FirstOrDefault(kvp => "REJECT_CODE".Equals(kvp.Key)).Value;
            string rejectText = orderedFields.FirstOrDefault(kvp => "REJECT_TEXT".Equals(kvp.Key)).Value;
            string rejectComment = orderedFields.FirstOrDefault(kvp => "REJECT_COMMENT".Equals(kvp.Key)).Value;

            if ("S4".Equals(rejectCode))
            {
                // Avliden
                string[] commentDateFields = rejectComment?.Split("-".ToCharArray());
                int year, month, day;
                DateTime deathDate;
                if (commentDateFields.Length == 3 && int.TryParse(commentDateFields[0], out year) && int.TryParse(commentDateFields[1], out month) && int.TryParse(commentDateFields[2], out day))
                {
                    deathDate = new DateTime(year, month, day);
                }
                else
                {
                    throw new Exception($"Unrecognised format in REJECT-COMMENT: {rejectComment}, expected 'yyyy-mm-dd'");
                }

                updateEntity.StatusCode = Generated.contact_statuscode.Inactive;
                return true;
            }
            return false;
        }

        public void RetrieveFile()
        {
            _log.Info("Entered RetrieveFile()");
            sftp = null;
            try
            {
                sftp = CreateSftpConnectionToCreditsafe();
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
