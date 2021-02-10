


--
-- base view for cgi_letter_template
--
create view dbo.[cgi_letter_template]
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
    [cgi_letter_templateId],
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
    [cgi_title],
    [cgi_template_body]
) with view_metadata as
select
    -- logical attributes
    [lk_cgi_letter_template_createdby].[FullName],
    [lk_cgi_letter_template_createdby].[YomiFullName],
    [lk_cgi_letter_template_createdonbehalfby].[FullName],
    [lk_cgi_letter_template_createdonbehalfby].[YomiFullName],
    [lk_cgi_letter_template_modifiedby].[FullName],
    [lk_cgi_letter_template_modifiedby].[YomiFullName],
    [lk_cgi_letter_template_modifiedonbehalfby].[FullName],
    [lk_cgi_letter_template_modifiedonbehalfby].[YomiFullName],

    -- ownership entries
    OwnerId = [cgi_letter_templateBase].OwnerId,
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
    [cgi_letter_templateBase].[cgi_letter_templateId],
    [cgi_letter_templateBase].[CreatedOn],
    [cgi_letter_templateBase].[CreatedBy],
    [cgi_letter_templateBase].[ModifiedOn],
    [cgi_letter_templateBase].[ModifiedBy],
    [cgi_letter_templateBase].[CreatedOnBehalfBy],
    [cgi_letter_templateBase].[ModifiedOnBehalfBy],
    [cgi_letter_templateBase].[OwningBusinessUnit],
    [cgi_letter_templateBase].[statecode],
    [cgi_letter_templateBase].[statuscode],
    [cgi_letter_templateBase].[VersionNumber],
    [cgi_letter_templateBase].[ImportSequenceNumber],
    [cgi_letter_templateBase].[OverriddenCreatedOn],
    [cgi_letter_templateBase].[TimeZoneRuleVersionNumber],
    [cgi_letter_templateBase].[UTCConversionTimeZoneCode],
    [cgi_letter_templateBase].[cgi_name],
    [cgi_letter_templateBase].[cgi_title],
    [cgi_letter_templateBase].[cgi_template_body]
from [cgi_letter_templateBase] 
    left join [SystemUserBase] [lk_cgi_letter_template_createdby] with(nolock) on ([cgi_letter_templateBase].[CreatedBy] = [lk_cgi_letter_template_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_letter_template_createdonbehalfby] with(nolock) on ([cgi_letter_templateBase].[CreatedOnBehalfBy] = [lk_cgi_letter_template_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_letter_template_modifiedby] with(nolock) on ([cgi_letter_templateBase].[ModifiedBy] = [lk_cgi_letter_template_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_letter_template_modifiedonbehalfby] with(nolock) on ([cgi_letter_templateBase].[ModifiedOnBehalfBy] = [lk_cgi_letter_template_modifiedonbehalfby].[SystemUserId])
    left join OwnerBase XXowner with(nolock) on ([cgi_letter_templateBase].OwnerId = XXowner.OwnerId)
