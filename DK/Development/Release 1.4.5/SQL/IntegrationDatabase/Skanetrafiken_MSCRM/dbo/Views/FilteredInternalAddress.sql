

--
-- report view for internaladdress
--
create view dbo.[FilteredInternalAddress] (
    [addressnumber],
    [addresstypecode],
    [addresstypecodename],
    [city],
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
    [internaladdressid],
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
    [objecttypecode],
    [objecttypecodename],
    [parentid],
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
    [InternalAddress].[AddressNumber],
    [InternalAddress].[AddressTypeCode],
    AddressTypeCodePLTable.Value,
    [InternalAddress].[City],
    [InternalAddress].[Composite],
    [InternalAddress].[Country],
    [InternalAddress].[County],
    [InternalAddress].[CreatedBy],
    --[InternalAddress].[CreatedByDsc]
    0,
    [InternalAddress].[CreatedByName],
    [InternalAddress].[CreatedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([InternalAddress].[CreatedOn], 
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
        [InternalAddress].[CreatedOn],
    [InternalAddress].[CreatedOnBehalfBy],
    --[InternalAddress].[CreatedOnBehalfByDsc]
    0,
    [InternalAddress].[CreatedOnBehalfByName],
    [InternalAddress].[CreatedOnBehalfByYomiName],
    [InternalAddress].[Fax],
    [InternalAddress].[InternalAddressId],
    [InternalAddress].[Latitude],
    [InternalAddress].[Line1],
    [InternalAddress].[Line2],
    [InternalAddress].[Line3],
    [InternalAddress].[Longitude],
    [InternalAddress].[ModifiedBy],
    --[InternalAddress].[ModifiedByDsc]
    0,
    [InternalAddress].[ModifiedByName],
    [InternalAddress].[ModifiedByYomiName],
    dbo.fn_UTCToTzSpecificLocalTime([InternalAddress].[ModifiedOn], 
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
        [InternalAddress].[ModifiedOn],
    [InternalAddress].[ModifiedOnBehalfBy],
    --[InternalAddress].[ModifiedOnBehalfByDsc]
    0,
    [InternalAddress].[ModifiedOnBehalfByName],
    [InternalAddress].[ModifiedOnBehalfByYomiName],
    [InternalAddress].[Name],
    [InternalAddress].[ObjectTypeCode],
    ObjectTypeCodePLTable.Value,
    [InternalAddress].[ParentId],
    [InternalAddress].[PostalCode],
    [InternalAddress].[PostOfficeBox],
    [InternalAddress].[ShippingMethodCode],
    ShippingMethodCodePLTable.Value,
    [InternalAddress].[StateOrProvince],
    [InternalAddress].[Telephone1],
    [InternalAddress].[Telephone2],
    [InternalAddress].[Telephone3],
    [InternalAddress].[UPSZone],
    [InternalAddress].[UTCOffset],
    [InternalAddress].[VersionNumber]
from InternalAddress
    join SystemUserBase u on (u.SystemUserId = dbo.fn_FindUserGuid() and u.IsDisabled = 0)
    left join UserSettingsBase us on us.SystemUserId = u.SystemUserId
    left join OrganizationBase o on u.OrganizationId = o.OrganizationId
    left outer join StringMap [AddressTypeCodePLTable] on 
		([AddressTypeCodePLTable].AttributeName = 'addresstypecode'
		and [AddressTypeCodePLTable].ObjectTypeCode = 1003
		and [AddressTypeCodePLTable].AttributeValue = [InternalAddress].[AddressTypeCode]
		and [AddressTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [ObjectTypeCodePLTable] on 
		([ObjectTypeCodePLTable].AttributeName = 'objecttypecode'
		and [ObjectTypeCodePLTable].ObjectTypeCode = 1003
		and [ObjectTypeCodePLTable].AttributeValue = [InternalAddress].[ObjectTypeCode]
		and [ObjectTypeCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
    left outer join StringMap [ShippingMethodCodePLTable] on 
		([ShippingMethodCodePLTable].AttributeName = 'shippingmethodcode'
		and [ShippingMethodCodePLTable].ObjectTypeCode = 1003
		and [ShippingMethodCodePLTable].AttributeValue = [InternalAddress].[ShippingMethodCode]
		and [ShippingMethodCodePLTable].LangId = 
			case us.UILanguageId 
				when 0 then o.LanguageCode
				else us.UILanguageId
			end)
