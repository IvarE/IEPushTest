﻿

--
-- report view for competitoraddress
--
create view dbo.[FilteredCompetitorAddress] (
    [addressnumber],
    [addresstypecode],
    [addresstypecodename],
    [city],
    [competitoraddressid],
    [composite],
    [country],
    [county],
    [createdby],
    [createdbydsc],
    [createdbyname],
    [createdbyyominame],
    [createdon],
    [createdonutc],
    [createdonbehalfby],
    [createdonbehalfbydsc],
    [createdonbehalfbyname],
    [createdonbehalfbyyominame],
    [fax],
    [latitude],
    [line1],
    [line2],
    [line3],
    [longitude],
    [modifiedby],
    [modifiedbydsc],
    [modifiedbyname],
    [modifiedbyyominame],
    [modifiedon],
    [modifiedonutc],
    [modifiedonbehalfby],
    [modifiedonbehalfbydsc],
    [modifiedonbehalfbyname],
    [modifiedonbehalfbyyominame],
    [name],
    [parentid],
    [parentiddsc],
    [parentidname],
    [parentidyominame],
    [postalcode],
    [postofficebox],
    [shippingmethodcode],
    [shippingmethodcodename],
    [stateorprovince],
    [telephone1],
    [telephone2],
    [telephone3],
    [upszone],
    [utcoffset],
    [versionnumber]
) with view_metadata as
select
    [CompetitorAddress].[AddressNumber],
    [CompetitorAddress].[AddressTypeCode],
    AddressTypeCodePLTable.Value,
    [CompetitorAddress].[City],
    [CompetitorAddress].[CompetitorAddressId],
    [CompetitorAddress].[Composite],
    [CompetitorAddress].[Country],
    [CompetitorAddress].[County],
    [CompetitorAddress].[CreatedBy],
    --[CompetitorAddress].[CreatedByDsc]
    0,
    [CompetitorAddress].[CreatedByName],
    [CompetitorAddress].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([CompetitorAddress].[CreatedOn], 
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
        [CompetitorAddress].[CreatedOn],
    [CompetitorAddress].[CreatedOnBehalfBy],
    --[CompetitorAddress].[CreatedOnBehalfByDsc]
    0,
    [CompetitorAddress].[CreatedOnBehalfByName],
    [CompetitorAddress].[CreatedOnBehalfByYomiName],
    [CompetitorAddress].[Fax],
    [CompetitorAddress].[Latitude],
    [CompetitorAddress].[Line1],
    [CompetitorAddress].[Line2],
    [CompetitorAddress].[Line3],
    [CompetitorAddress].[Longitude],
    [CompetitorAddress].[ModifiedBy],
    --[CompetitorAddress].[ModifiedByDsc]
    0,
    [CompetitorAddress].[ModifiedByName],
    [CompetitorAddress].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([CompetitorAddress].[ModifiedOn], 
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
        [CompetitorAddress].[ModifiedOn],
    [CompetitorAddress].[ModifiedOnBehalfBy],
    --[CompetitorAddress].[ModifiedOnBehalfByDsc]
    0,
    [CompetitorAddress].[ModifiedOnBehalfByName],
    [CompetitorAddress].[ModifiedOnBehalfByYomiName],
    [CompetitorAddress].[Name],
    [CompetitorAddress].[ParentId],
    --[CompetitorAddress].[ParentIdDsc]
    0,
    [CompetitorAddress].[ParentIdName],
    [CompetitorAddress].[ParentIdYomiName],
    [CompetitorAddress].[PostalCode],
    [CompetitorAddress].[PostOfficeBox],
    [CompetitorAddress].[ShippingMethodCode],
    ShippingMethodCodePLTable.Value,
    [CompetitorAddress].[StateOrProvince],
    [CompetitorAddress].[Telephone1],
    [CompetitorAddress].[Telephone2],
    [CompetitorAddress].[Telephone3],
    [CompetitorAddress].[UPSZone],
    [CompetitorAddress].[UTCOffset],
    [CompetitorAddress].[VersionNumber]
from CompetitorAddress
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [AddressTypeCodePLTable] on 
		([AddressTypeCodePLTable].AttributeName = 'addresstypecode'
		and [AddressTypeCodePLTable].ObjectTypeCode = 1004
		and [AddressTypeCodePLTable].AttributeValue = [CompetitorAddress].[AddressTypeCode]
		and [AddressTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [ShippingMethodCodePLTable] on 
		([ShippingMethodCodePLTable].AttributeName = 'shippingmethodcode'
		and [ShippingMethodCodePLTable].ObjectTypeCode = 1004
		and [ShippingMethodCodePLTable].AttributeValue = [CompetitorAddress].[ShippingMethodCode]
		and [ShippingMethodCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
