﻿

--
-- report view for recurringappointmentmaster
--
create view dbo.[FilteredRecurringAppointmentMaster] (
    [activityid],
    [activitytypecode],
    [activitytypecodename],
    [category],
    [createdby],
    [createdbyname],
    [createdbyyominame],
    [createdon],
    [createdonutc],
    [createdonbehalfby],
    [createdonbehalfbydsc],
    [createdonbehalfbyname],
    [createdonbehalfbyyominame],
    [dayofmonth],
    [daysofweekmask],
    [deletedexceptionslist],
    [description],
    [duration],
    [effectiveenddate],
    [effectiveenddateutc],
    [effectivestartdate],
    [effectivestartdateutc],
    [endtime],
    [endtimeutc],
    [exchangerate],
    [expansionstatecode],
    [expansionstatecodename],
    [firstdayofweek],
    [globalobjectid],
    [groupid],
    [importsequencenumber],
    [instance],
    [instancename],
    [instancetypecode],
    [instancetypecodename],
    [interval],
    [isalldayevent],
    [isalldayeventname],
    [isbilled],
    [isbilledname],
    [ismapiprivate],
    [ismapiprivatename],
    [isnthmonthly],
    [isnthmonthlyname],
    [isnthyearly],
    [isnthyearlyname],
    [isregenerate],
    [isregeneratename],
    [isregularactivity],
    [isregularactivityname],
    [isweekdaypattern],
    [isweekdaypatternname],
    [isworkflowcreated],
    [isworkflowcreatedname],
    [lastexpandedinstancedate],
    [lastexpandedinstancedateutc],
    [location],
    [modifiedby],
    [modifiedbyname],
    [modifiedbyyominame],
    [modifiedon],
    [modifiedonutc],
    [modifiedonbehalfby],
    [modifiedonbehalfbydsc],
    [modifiedonbehalfbyname],
    [modifiedonbehalfbyyominame],
    [monthofyear],
    [monthofyearname],
    [nextexpansioninstancedate],
    [nextexpansioninstancedateutc],
    [occurrences],
    [outlookownerapptid],
    [overriddencreatedon],
    [overriddencreatedonutc],
    [ownerid],
    [owneridname],
    [owneridtype],
    [owneridyominame],
    [owningbusinessunit],
    [owningteam],
    [owninguser],
    [patternenddate],
    [patternenddateutc],
    [patternendtype],
    [patternendtypename],
    [patternstartdate],
    [patternstartdateutc],
    [prioritycode],
    [prioritycodename],
    [processid],
    [recurrencepatterntype],
    [recurrencepatterntypename],
    [regardingobjectid],
    [regardingobjectidname],
    [regardingobjectidyominame],
    [regardingobjecttypecode],
    [ruleid],
    [scheduledend],
    [scheduledendutc],
    [scheduledstart],
    [scheduledstartutc],
    [seriesstatus],
    [seriesstatusname],
    [serviceid],
    [stageid],
    [starttime],
    [starttimeutc],
    [statecode],
    [statecodename],
    [statuscode],
    [statuscodename],
    [subcategory],
    [subject],
    [timezoneruleversionnumber],
    [transactioncurrencyid],
    [transactioncurrencyidname],
    [utcconversiontimezonecode],
    [versionnumber]
) with view_metadata as
select
    [RecurringAppointmentMaster].[ActivityId],
    [RecurringAppointmentMaster].[ActivityTypeCode],
    ActivityTypeCodePLTable.Value,
    [RecurringAppointmentMaster].[Category],
    [RecurringAppointmentMaster].[CreatedBy],
    [RecurringAppointmentMaster].[CreatedByName],
    [RecurringAppointmentMaster].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([RecurringAppointmentMaster].[CreatedOn], 
			us.TimeZoneBias,
			us.TimeZoneDaylightBias,
			us.TimeZoneDaylightYear,
			us.TimeZoneDaylightMonth,
			us.TimeZoneDaylightDay,
			us.TimeZoneDaylightHour,
			us.TimeZoneDaylightMinute,
			us.TimeZoneDaylightSecond,
			0,
			us.TimeZoneDaylightDayOfWeek,
			us.TimeZoneStandardBias,
			us.TimeZoneStandardYear,
			us.TimeZoneStandardMonth,
			us.TimeZoneStandardDay,
			us.TimeZoneStandardHour,
			us.TimeZoneStandardMinute,
			us.TimeZoneStandardSecond,
			0,
			us.TimeZoneStandardDayOfWeek),
        [RecurringAppointmentMaster].[CreatedOn],
    [RecurringAppointmentMaster].[CreatedOnBehalfBy],
    --[RecurringAppointmentMaster].[CreatedOnBehalfByDsc]
    0,
    [RecurringAppointmentMaster].[CreatedOnBehalfByName],
    [RecurringAppointmentMaster].[CreatedOnBehalfByYomiName],
    [RecurringAppointmentMaster].[DayOfMonth],
    [RecurringAppointmentMaster].[DaysOfWeekMask],
    [RecurringAppointmentMaster].[DeletedExceptionsList],
    [RecurringAppointmentMaster].[Description],
    [RecurringAppointmentMaster].[Duration],
    dbo.fn_UTCToTzSpecificLocalTime([RecurringAppointmentMaster].[EffectiveEndDate], 
			us.TimeZoneBias,
			us.TimeZoneDaylightBias,
			us.TimeZoneDaylightYear,
			us.TimeZoneDaylightMonth,
			us.TimeZoneDaylightDay,
			us.TimeZoneDaylightHour,
			us.TimeZoneDaylightMinute,
			us.TimeZoneDaylightSecond,
			0,
			us.TimeZoneDaylightDayOfWeek,
			us.TimeZoneStandardBias,
			us.TimeZoneStandardYear,
			us.TimeZoneStandardMonth,
			us.TimeZoneStandardDay,
			us.TimeZoneStandardHour,
			us.TimeZoneStandardMinute,
			us.TimeZoneStandardSecond,
			0,
			us.TimeZoneStandardDayOfWeek),
        [RecurringAppointmentMaster].[EffectiveEndDate],
    dbo.fn_UTCToTzSpecificLocalTime([RecurringAppointmentMaster].[EffectiveStartDate], 
			us.TimeZoneBias,
			us.TimeZoneDaylightBias,
			us.TimeZoneDaylightYear,
			us.TimeZoneDaylightMonth,
			us.TimeZoneDaylightDay,
			us.TimeZoneDaylightHour,
			us.TimeZoneDaylightMinute,
			us.TimeZoneDaylightSecond,
			0,
			us.TimeZoneDaylightDayOfWeek,
			us.TimeZoneStandardBias,
			us.TimeZoneStandardYear,
			us.TimeZoneStandardMonth,
			us.TimeZoneStandardDay,
			us.TimeZoneStandardHour,
			us.TimeZoneStandardMinute,
			us.TimeZoneStandardSecond,
			0,
			us.TimeZoneStandardDayOfWeek),
        [RecurringAppointmentMaster].[EffectiveStartDate],
    dbo.fn_UTCToTzSpecificLocalTime([RecurringAppointmentMaster].[EndTime], 
			us.TimeZoneBias,
			us.TimeZoneDaylightBias,
			us.TimeZoneDaylightYear,
			us.TimeZoneDaylightMonth,
			us.TimeZoneDaylightDay,
			us.TimeZoneDaylightHour,
			us.TimeZoneDaylightMinute,
			us.TimeZoneDaylightSecond,
			0,
			us.TimeZoneDaylightDayOfWeek,
			us.TimeZoneStandardBias,
			us.TimeZoneStandardYear,
			us.TimeZoneStandardMonth,
			us.TimeZoneStandardDay,
			us.TimeZoneStandardHour,
			us.TimeZoneStandardMinute,
			us.TimeZoneStandardSecond,
			0,
			us.TimeZoneStandardDayOfWeek),
        [RecurringAppointmentMaster].[EndTime],
    [RecurringAppointmentMaster].[ExchangeRate],
    [RecurringAppointmentMaster].[ExpansionStateCode],
    ExpansionStateCodePLTable.Value,
    [RecurringAppointmentMaster].[FirstDayOfWeek],
    [RecurringAppointmentMaster].[GlobalObjectId],
    [RecurringAppointmentMaster].[GroupId],
    [RecurringAppointmentMaster].[ImportSequenceNumber],
    [RecurringAppointmentMaster].[Instance],
    InstancePLTable.Value,
    [RecurringAppointmentMaster].[InstanceTypeCode],
    InstanceTypeCodePLTable.Value,
    [RecurringAppointmentMaster].[Interval],
    [RecurringAppointmentMaster].[IsAllDayEvent],
    IsAllDayEventPLTable.Value,
    [RecurringAppointmentMaster].[IsBilled],
    IsBilledPLTable.Value,
    [RecurringAppointmentMaster].[IsMapiPrivate],
    IsMapiPrivatePLTable.Value,
    [RecurringAppointmentMaster].[IsNthMonthly],
    IsNthMonthlyPLTable.Value,
    [RecurringAppointmentMaster].[IsNthYearly],
    IsNthYearlyPLTable.Value,
    [RecurringAppointmentMaster].[IsRegenerate],
    IsRegeneratePLTable.Value,
    [RecurringAppointmentMaster].[IsRegularActivity],
    IsRegularActivityPLTable.Value,
    [RecurringAppointmentMaster].[IsWeekDayPattern],
    IsWeekDayPatternPLTable.Value,
    [RecurringAppointmentMaster].[IsWorkflowCreated],
    IsWorkflowCreatedPLTable.Value,
    dbo.fn_UTCToTzSpecificLocalTime([RecurringAppointmentMaster].[LastExpandedInstanceDate], 
			us.TimeZoneBias,
			us.TimeZoneDaylightBias,
			us.TimeZoneDaylightYear,
			us.TimeZoneDaylightMonth,
			us.TimeZoneDaylightDay,
			us.TimeZoneDaylightHour,
			us.TimeZoneDaylightMinute,
			us.TimeZoneDaylightSecond,
			0,
			us.TimeZoneDaylightDayOfWeek,
			us.TimeZoneStandardBias,
			us.TimeZoneStandardYear,
			us.TimeZoneStandardMonth,
			us.TimeZoneStandardDay,
			us.TimeZoneStandardHour,
			us.TimeZoneStandardMinute,
			us.TimeZoneStandardSecond,
			0,
			us.TimeZoneStandardDayOfWeek),
        [RecurringAppointmentMaster].[LastExpandedInstanceDate],
    [RecurringAppointmentMaster].[Location],
    [RecurringAppointmentMaster].[ModifiedBy],
    [RecurringAppointmentMaster].[ModifiedByName],
    [RecurringAppointmentMaster].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([RecurringAppointmentMaster].[ModifiedOn], 
			us.TimeZoneBias,
			us.TimeZoneDaylightBias,
			us.TimeZoneDaylightYear,
			us.TimeZoneDaylightMonth,
			us.TimeZoneDaylightDay,
			us.TimeZoneDaylightHour,
			us.TimeZoneDaylightMinute,
			us.TimeZoneDaylightSecond,
			0,
			us.TimeZoneDaylightDayOfWeek,
			us.TimeZoneStandardBias,
			us.TimeZoneStandardYear,
			us.TimeZoneStandardMonth,
			us.TimeZoneStandardDay,
			us.TimeZoneStandardHour,
			us.TimeZoneStandardMinute,
			us.TimeZoneStandardSecond,
			0,
			us.TimeZoneStandardDayOfWeek),
        [RecurringAppointmentMaster].[ModifiedOn],
    [RecurringAppointmentMaster].[ModifiedOnBehalfBy],
    --[RecurringAppointmentMaster].[ModifiedOnBehalfByDsc]
    0,
    [RecurringAppointmentMaster].[ModifiedOnBehalfByName],
    [RecurringAppointmentMaster].[ModifiedOnBehalfByYomiName],
    [RecurringAppointmentMaster].[MonthOfYear],
    MonthOfYearPLTable.Value,
    dbo.fn_UTCToTzSpecificLocalTime([RecurringAppointmentMaster].[NextExpansionInstanceDate], 
			us.TimeZoneBias,
			us.TimeZoneDaylightBias,
			us.TimeZoneDaylightYear,
			us.TimeZoneDaylightMonth,
			us.TimeZoneDaylightDay,
			us.TimeZoneDaylightHour,
			us.TimeZoneDaylightMinute,
			us.TimeZoneDaylightSecond,
			0,
			us.TimeZoneDaylightDayOfWeek,
			us.TimeZoneStandardBias,
			us.TimeZoneStandardYear,
			us.TimeZoneStandardMonth,
			us.TimeZoneStandardDay,
			us.TimeZoneStandardHour,
			us.TimeZoneStandardMinute,
			us.TimeZoneStandardSecond,
			0,
			us.TimeZoneStandardDayOfWeek),
        [RecurringAppointmentMaster].[NextExpansionInstanceDate],
    [RecurringAppointmentMaster].[Occurrences],
    [RecurringAppointmentMaster].[OutlookOwnerApptId],
    dbo.fn_UTCToTzSpecificLocalTime([RecurringAppointmentMaster].[OverriddenCreatedOn], 
			us.TimeZoneBias,
			us.TimeZoneDaylightBias,
			us.TimeZoneDaylightYear,
			us.TimeZoneDaylightMonth,
			us.TimeZoneDaylightDay,
			us.TimeZoneDaylightHour,
			us.TimeZoneDaylightMinute,
			us.TimeZoneDaylightSecond,
			0,
			us.TimeZoneDaylightDayOfWeek,
			us.TimeZoneStandardBias,
			us.TimeZoneStandardYear,
			us.TimeZoneStandardMonth,
			us.TimeZoneStandardDay,
			us.TimeZoneStandardHour,
			us.TimeZoneStandardMinute,
			us.TimeZoneStandardSecond,
			0,
			us.TimeZoneStandardDayOfWeek),
        [RecurringAppointmentMaster].[OverriddenCreatedOn],
    [RecurringAppointmentMaster].[OwnerId],
    [RecurringAppointmentMaster].[OwnerIdName],
    [RecurringAppointmentMaster].[OwnerIdType],
    [RecurringAppointmentMaster].[OwnerIdYomiName],
    [RecurringAppointmentMaster].[OwningBusinessUnit],
    [RecurringAppointmentMaster].[OwningTeam],
    [RecurringAppointmentMaster].[OwningUser],
    dbo.fn_UTCToTzSpecificLocalTime([RecurringAppointmentMaster].[PatternEndDate], 
			us.TimeZoneBias,
			us.TimeZoneDaylightBias,
			us.TimeZoneDaylightYear,
			us.TimeZoneDaylightMonth,
			us.TimeZoneDaylightDay,
			us.TimeZoneDaylightHour,
			us.TimeZoneDaylightMinute,
			us.TimeZoneDaylightSecond,
			0,
			us.TimeZoneDaylightDayOfWeek,
			us.TimeZoneStandardBias,
			us.TimeZoneStandardYear,
			us.TimeZoneStandardMonth,
			us.TimeZoneStandardDay,
			us.TimeZoneStandardHour,
			us.TimeZoneStandardMinute,
			us.TimeZoneStandardSecond,
			0,
			us.TimeZoneStandardDayOfWeek),
        [RecurringAppointmentMaster].[PatternEndDate],
    [RecurringAppointmentMaster].[PatternEndType],
    PatternEndTypePLTable.Value,
    dbo.fn_UTCToTzSpecificLocalTime([RecurringAppointmentMaster].[PatternStartDate], 
			us.TimeZoneBias,
			us.TimeZoneDaylightBias,
			us.TimeZoneDaylightYear,
			us.TimeZoneDaylightMonth,
			us.TimeZoneDaylightDay,
			us.TimeZoneDaylightHour,
			us.TimeZoneDaylightMinute,
			us.TimeZoneDaylightSecond,
			0,
			us.TimeZoneDaylightDayOfWeek,
			us.TimeZoneStandardBias,
			us.TimeZoneStandardYear,
			us.TimeZoneStandardMonth,
			us.TimeZoneStandardDay,
			us.TimeZoneStandardHour,
			us.TimeZoneStandardMinute,
			us.TimeZoneStandardSecond,
			0,
			us.TimeZoneStandardDayOfWeek),
        [RecurringAppointmentMaster].[PatternStartDate],
    [RecurringAppointmentMaster].[PriorityCode],
    PriorityCodePLTable.Value,
    [RecurringAppointmentMaster].[ProcessId],
    [RecurringAppointmentMaster].[RecurrencePatternType],
    RecurrencePatternTypePLTable.Value,
    [RecurringAppointmentMaster].[RegardingObjectId],
    [RecurringAppointmentMaster].[RegardingObjectIdName],
    [RecurringAppointmentMaster].[RegardingObjectIdYomiName],
    [RecurringAppointmentMaster].[RegardingObjectTypeCode],
    [RecurringAppointmentMaster].[RuleId],
    dbo.fn_UTCToTzSpecificLocalTime([RecurringAppointmentMaster].[ScheduledEnd], 
			us.TimeZoneBias,
			us.TimeZoneDaylightBias,
			us.TimeZoneDaylightYear,
			us.TimeZoneDaylightMonth,
			us.TimeZoneDaylightDay,
			us.TimeZoneDaylightHour,
			us.TimeZoneDaylightMinute,
			us.TimeZoneDaylightSecond,
			0,
			us.TimeZoneDaylightDayOfWeek,
			us.TimeZoneStandardBias,
			us.TimeZoneStandardYear,
			us.TimeZoneStandardMonth,
			us.TimeZoneStandardDay,
			us.TimeZoneStandardHour,
			us.TimeZoneStandardMinute,
			us.TimeZoneStandardSecond,
			0,
			us.TimeZoneStandardDayOfWeek),
        [RecurringAppointmentMaster].[ScheduledEnd],
    dbo.fn_UTCToTzSpecificLocalTime([RecurringAppointmentMaster].[ScheduledStart], 
			us.TimeZoneBias,
			us.TimeZoneDaylightBias,
			us.TimeZoneDaylightYear,
			us.TimeZoneDaylightMonth,
			us.TimeZoneDaylightDay,
			us.TimeZoneDaylightHour,
			us.TimeZoneDaylightMinute,
			us.TimeZoneDaylightSecond,
			0,
			us.TimeZoneDaylightDayOfWeek,
			us.TimeZoneStandardBias,
			us.TimeZoneStandardYear,
			us.TimeZoneStandardMonth,
			us.TimeZoneStandardDay,
			us.TimeZoneStandardHour,
			us.TimeZoneStandardMinute,
			us.TimeZoneStandardSecond,
			0,
			us.TimeZoneStandardDayOfWeek),
        [RecurringAppointmentMaster].[ScheduledStart],
    [RecurringAppointmentMaster].[SeriesStatus],
    SeriesStatusPLTable.Value,
    [RecurringAppointmentMaster].[ServiceId],
    [RecurringAppointmentMaster].[StageId],
    dbo.fn_UTCToTzSpecificLocalTime([RecurringAppointmentMaster].[StartTime], 
			us.TimeZoneBias,
			us.TimeZoneDaylightBias,
			us.TimeZoneDaylightYear,
			us.TimeZoneDaylightMonth,
			us.TimeZoneDaylightDay,
			us.TimeZoneDaylightHour,
			us.TimeZoneDaylightMinute,
			us.TimeZoneDaylightSecond,
			0,
			us.TimeZoneDaylightDayOfWeek,
			us.TimeZoneStandardBias,
			us.TimeZoneStandardYear,
			us.TimeZoneStandardMonth,
			us.TimeZoneStandardDay,
			us.TimeZoneStandardHour,
			us.TimeZoneStandardMinute,
			us.TimeZoneStandardSecond,
			0,
			us.TimeZoneStandardDayOfWeek),
        [RecurringAppointmentMaster].[StartTime],
    [RecurringAppointmentMaster].[StateCode],
    StateCodePLTable.Value,
    [RecurringAppointmentMaster].[StatusCode],
    StatusCodePLTable.Value,
    [RecurringAppointmentMaster].[Subcategory],
    [RecurringAppointmentMaster].[Subject],
    [RecurringAppointmentMaster].[TimeZoneRuleVersionNumber],
    [RecurringAppointmentMaster].[TransactionCurrencyId],
    [RecurringAppointmentMaster].[TransactionCurrencyIdName],
    [RecurringAppointmentMaster].[UTCConversionTimeZoneCode],
    [RecurringAppointmentMaster].[VersionNumber]
from RecurringAppointmentMaster
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [ActivityTypeCodePLTable] on 
		([ActivityTypeCodePLTable].AttributeName = 'activitytypecode'
		and [ActivityTypeCodePLTable].ObjectTypeCode = 4251
		and [ActivityTypeCodePLTable].AttributeValue = [RecurringAppointmentMaster].[ActivityTypeCode]
		and [ActivityTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [ExpansionStateCodePLTable] on 
		([ExpansionStateCodePLTable].AttributeName = 'expansionstatecode'
		and [ExpansionStateCodePLTable].ObjectTypeCode = 4251
		and [ExpansionStateCodePLTable].AttributeValue = [RecurringAppointmentMaster].[ExpansionStateCode]
		and [ExpansionStateCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [InstancePLTable] on 
		([InstancePLTable].AttributeName = 'instance'
		and [InstancePLTable].ObjectTypeCode = 4251
		and [InstancePLTable].AttributeValue = [RecurringAppointmentMaster].[Instance]
		and [InstancePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [InstanceTypeCodePLTable] on 
		([InstanceTypeCodePLTable].AttributeName = 'instancetypecode'
		and [InstanceTypeCodePLTable].ObjectTypeCode = 4251
		and [InstanceTypeCodePLTable].AttributeValue = [RecurringAppointmentMaster].[InstanceTypeCode]
		and [InstanceTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsAllDayEventPLTable] on 
		([IsAllDayEventPLTable].AttributeName = 'isalldayevent'
		and [IsAllDayEventPLTable].ObjectTypeCode = 4251
		and [IsAllDayEventPLTable].AttributeValue = [RecurringAppointmentMaster].[IsAllDayEvent]
		and [IsAllDayEventPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsBilledPLTable] on 
		([IsBilledPLTable].AttributeName = 'isbilled'
		and [IsBilledPLTable].ObjectTypeCode = 4251
		and [IsBilledPLTable].AttributeValue = [RecurringAppointmentMaster].[IsBilled]
		and [IsBilledPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsMapiPrivatePLTable] on 
		([IsMapiPrivatePLTable].AttributeName = 'ismapiprivate'
		and [IsMapiPrivatePLTable].ObjectTypeCode = 4251
		and [IsMapiPrivatePLTable].AttributeValue = [RecurringAppointmentMaster].[IsMapiPrivate]
		and [IsMapiPrivatePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsNthMonthlyPLTable] on 
		([IsNthMonthlyPLTable].AttributeName = 'isnthmonthly'
		and [IsNthMonthlyPLTable].ObjectTypeCode = 4251
		and [IsNthMonthlyPLTable].AttributeValue = [RecurringAppointmentMaster].[IsNthMonthly]
		and [IsNthMonthlyPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsNthYearlyPLTable] on 
		([IsNthYearlyPLTable].AttributeName = 'isnthyearly'
		and [IsNthYearlyPLTable].ObjectTypeCode = 4251
		and [IsNthYearlyPLTable].AttributeValue = [RecurringAppointmentMaster].[IsNthYearly]
		and [IsNthYearlyPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsRegeneratePLTable] on 
		([IsRegeneratePLTable].AttributeName = 'isregenerate'
		and [IsRegeneratePLTable].ObjectTypeCode = 4251
		and [IsRegeneratePLTable].AttributeValue = [RecurringAppointmentMaster].[IsRegenerate]
		and [IsRegeneratePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsRegularActivityPLTable] on 
		([IsRegularActivityPLTable].AttributeName = 'isregularactivity'
		and [IsRegularActivityPLTable].ObjectTypeCode = 4251
		and [IsRegularActivityPLTable].AttributeValue = [RecurringAppointmentMaster].[IsRegularActivity]
		and [IsRegularActivityPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsWeekDayPatternPLTable] on 
		([IsWeekDayPatternPLTable].AttributeName = 'isweekdaypattern'
		and [IsWeekDayPatternPLTable].ObjectTypeCode = 4251
		and [IsWeekDayPatternPLTable].AttributeValue = [RecurringAppointmentMaster].[IsWeekDayPattern]
		and [IsWeekDayPatternPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [IsWorkflowCreatedPLTable] on 
		([IsWorkflowCreatedPLTable].AttributeName = 'isworkflowcreated'
		and [IsWorkflowCreatedPLTable].ObjectTypeCode = 4251
		and [IsWorkflowCreatedPLTable].AttributeValue = [RecurringAppointmentMaster].[IsWorkflowCreated]
		and [IsWorkflowCreatedPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [MonthOfYearPLTable] on 
		([MonthOfYearPLTable].AttributeName = 'monthofyear'
		and [MonthOfYearPLTable].ObjectTypeCode = 4251
		and [MonthOfYearPLTable].AttributeValue = [RecurringAppointmentMaster].[MonthOfYear]
		and [MonthOfYearPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [PatternEndTypePLTable] on 
		([PatternEndTypePLTable].AttributeName = 'patternendtype'
		and [PatternEndTypePLTable].ObjectTypeCode = 4251
		and [PatternEndTypePLTable].AttributeValue = [RecurringAppointmentMaster].[PatternEndType]
		and [PatternEndTypePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [PriorityCodePLTable] on 
		([PriorityCodePLTable].AttributeName = 'prioritycode'
		and [PriorityCodePLTable].ObjectTypeCode = 4251
		and [PriorityCodePLTable].AttributeValue = [RecurringAppointmentMaster].[PriorityCode]
		and [PriorityCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [RecurrencePatternTypePLTable] on 
		([RecurrencePatternTypePLTable].AttributeName = 'recurrencepatterntype'
		and [RecurrencePatternTypePLTable].ObjectTypeCode = 4251
		and [RecurrencePatternTypePLTable].AttributeValue = [RecurringAppointmentMaster].[RecurrencePatternType]
		and [RecurrencePatternTypePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [SeriesStatusPLTable] on 
		([SeriesStatusPLTable].AttributeName = 'seriesstatus'
		and [SeriesStatusPLTable].ObjectTypeCode = 4251
		and [SeriesStatusPLTable].AttributeValue = [RecurringAppointmentMaster].[SeriesStatus]
		and [SeriesStatusPLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StateCodePLTable] on 
		([StateCodePLTable].AttributeName = 'statecode'
		and [StateCodePLTable].ObjectTypeCode = 4251
		and [StateCodePLTable].AttributeValue = [RecurringAppointmentMaster].[StateCode]
		and [StateCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [StatusCodePLTable] on 
		([StatusCodePLTable].AttributeName = 'statuscode'
		and [StatusCodePLTable].ObjectTypeCode = 4251
		and [StatusCodePLTable].AttributeValue = [RecurringAppointmentMaster].[StatusCode]
		and [StatusCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    cross join dbo.fn_GetMaxPrivilegeDepthMask(4200) pdm
where
(
	-- privilege check
	pdm.PrivilegeDepthMask is not null and
	(
	
	-- Owner check
	--
	[RecurringAppointmentMaster].OwnerId in 
	( 	-- returns only principals with Basic Read privilege for entity
		select pem.PrincipalId from PrincipalEntityMap pem (NOLOCK)
			join SystemUserPrincipals sup (NOLOCK) on pem.PrincipalId = sup.PrincipalId 
			where sup.SystemUserId = u.SystemUserId 
				and pem.ObjectTypeCode = 4200
	)	

		
	-- role based access
	or 
	
exists
(
	select 
	1
	where
	(
		-- deep/local security
		(((pdm.PrivilegeDepthMask & 0x4) != 0) or ((pdm.PrivilegeDepthMask & 0x2) != 0)) and 
		[RecurringAppointmentMaster].[OwningBusinessUnit] in (select BusinessUnitId from SystemUserBusinessUnitEntityMap (NOLOCK) where SystemUserId = u.SystemUserId and ObjectTypeCode = 4200)
	) 
	or
	(
		-- global security
		((pdm.PrivilegeDepthMask & 0x8) != 0) and 
		[RecurringAppointmentMaster].[OwningBusinessUnit] is not null 
	) 
)

	
	-- object shared to the user 
	or 
	[RecurringAppointmentMaster].[ActivityId] in 
		(
		select  POA.ObjectId from PrincipalObjectAccess POA 
		join SystemUserPrincipals sup (NOLOCK) on POA.PrincipalId = sup.PrincipalId
			where sup.SystemUserId = u.SystemUserId and
				POA.ObjectTypeCode = 4200 and
				((POA.AccessRightsMask | POA.InheritedAccessRightsMask) & 1)=1
		)
	)
)
