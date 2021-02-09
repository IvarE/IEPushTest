


--
-- base view for cgi_representative
--
create view dbo.[cgi_representative]
 (
    -- logical attributes
    [CreatedByName],
    [CreatedByYomiName],
    [CreatedOnBehalfByName],
    [CreatedOnBehalfByYomiName],
    [ModifiedByName],
    [ModifiedByYomiName],
    [ModifiedOnBehalfByName],
    [ModifiedOnBehalfByYomiName],

    -- ownership entries
    OwnerId,
    OwnerIdName,
    OwnerIdYomiName,
    OwnerIdDsc,
    OwnerIdType,
    OwningUser,
    OwningTeam,

    -- physical attributes
    [cgi_representativeId],
    [CreatedOn],
    [CreatedBy],
    [ModifiedOn],
    [ModifiedBy],
    [CreatedOnBehalfBy],
    [ModifiedOnBehalfBy],
    [OwningBusinessUnit],
    [statecode],
    [statuscode],
    [VersionNumber],
    [ImportSequenceNumber],
    [OverriddenCreatedOn],
    [TimeZoneRuleVersionNumber],
    [UTCConversionTimeZoneCode],
    [cgi_name],
    [cgi_Mainphone],
    [cgi_FirstName],
    [cgi_LastName],
    [cgi_OtherPhone],
    [cgi_Telephone3],
    [cgi_Email],
    [cgi_StreetAddress],
    [cgi_COaddress],
    [cgi_ZIPPostalCode],
    [cgi_City],
    [EmailAddress],
    [cgi_country]
) with view_metadata as
select
    -- logical attributes
    [lk_cgi_representative_createdby].[FullName],
    [lk_cgi_representative_createdby].[YomiFullName],
    [lk_cgi_representative_createdonbehalfby].[FullName],
    [lk_cgi_representative_createdonbehalfby].[YomiFullName],
    [lk_cgi_representative_modifiedby].[FullName],
    [lk_cgi_representative_modifiedby].[YomiFullName],
    [lk_cgi_representative_modifiedonbehalfby].[FullName],
    [lk_cgi_representative_modifiedonbehalfby].[YomiFullName],

    -- ownership entries
    OwnerId = [cgi_representativeBase].OwnerId,
    OwnerName = XXowner.Name,
    OwnerYomiName =  XXowner.YomiName,
    OwnerDsc = 0, -- DSC is removed, stub it to 0
    OwnerIdType = XXowner.OwnerIdType,
    OwningUser = case 
 		when XXowner.OwnerIdType= 8 then XXowner.OwnerId
		else null
		end,
    OwningTeam = case 
 		when XXowner.OwnerIdType= 9 then XXowner.OwnerId
		else null
		end,

    -- physical attribute
    [cgi_representativeBase].[cgi_representativeId],
    [cgi_representativeBase].[CreatedOn],
    [cgi_representativeBase].[CreatedBy],
    [cgi_representativeBase].[ModifiedOn],
    [cgi_representativeBase].[ModifiedBy],
    [cgi_representativeBase].[CreatedOnBehalfBy],
    [cgi_representativeBase].[ModifiedOnBehalfBy],
    [cgi_representativeBase].[OwningBusinessUnit],
    [cgi_representativeBase].[statecode],
    [cgi_representativeBase].[statuscode],
    [cgi_representativeBase].[VersionNumber],
    [cgi_representativeBase].[ImportSequenceNumber],
    [cgi_representativeBase].[OverriddenCreatedOn],
    [cgi_representativeBase].[TimeZoneRuleVersionNumber],
    [cgi_representativeBase].[UTCConversionTimeZoneCode],
    [cgi_representativeBase].[cgi_name],
    [cgi_representativeBase].[cgi_Mainphone],
    [cgi_representativeBase].[cgi_FirstName],
    [cgi_representativeBase].[cgi_LastName],
    [cgi_representativeBase].[cgi_OtherPhone],
    [cgi_representativeBase].[cgi_Telephone3],
    [cgi_representativeBase].[cgi_Email],
    [cgi_representativeBase].[cgi_StreetAddress],
    [cgi_representativeBase].[cgi_COaddress],
    [cgi_representativeBase].[cgi_ZIPPostalCode],
    [cgi_representativeBase].[cgi_City],
    [cgi_representativeBase].[EmailAddress],
    [cgi_representativeBase].[cgi_country]
from [cgi_representativeBase] 
    left join [SystemUserBase] [lk_cgi_representative_createdby] with(nolock) on ([cgi_representativeBase].[CreatedBy] = [lk_cgi_representative_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_representative_createdonbehalfby] with(nolock) on ([cgi_representativeBase].[CreatedOnBehalfBy] = [lk_cgi_representative_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_representative_modifiedby] with(nolock) on ([cgi_representativeBase].[ModifiedBy] = [lk_cgi_representative_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_representative_modifiedonbehalfby] with(nolock) on ([cgi_representativeBase].[ModifiedOnBehalfBy] = [lk_cgi_representative_modifiedonbehalfby].[SystemUserId])
    left join OwnerBase XXowner with(nolock) on ([cgi_representativeBase].OwnerId = XXowner.OwnerId)
