


--
-- base view for cgi_localizationlanguage
--
create view dbo.[cgi_localizationlanguage]
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
    [OrganizationIdName],

    -- physical attributes
    [cgi_localizationlanguageId],
    [CreatedOn],
    [CreatedBy],
    [ModifiedOn],
    [ModifiedBy],
    [CreatedOnBehalfBy],
    [ModifiedOnBehalfBy],
    [OrganizationId],
    [statecode],
    [statuscode],
    [VersionNumber],
    [ImportSequenceNumber],
    [OverriddenCreatedOn],
    [TimeZoneRuleVersionNumber],
    [UTCConversionTimeZoneCode],
    [cgi_localizationlanguagename],
    [cgi_LocalizationLanguageNumber]
) with view_metadata as
select
    -- logical attributes
    [lk_cgi_localizationlanguage_createdby].[FullName],
    [lk_cgi_localizationlanguage_createdby].[YomiFullName],
    [lk_cgi_localizationlanguage_createdonbehalfby].[FullName],
    [lk_cgi_localizationlanguage_createdonbehalfby].[YomiFullName],
    [lk_cgi_localizationlanguage_modifiedby].[FullName],
    [lk_cgi_localizationlanguage_modifiedby].[YomiFullName],
    [lk_cgi_localizationlanguage_modifiedonbehalfby].[FullName],
    [lk_cgi_localizationlanguage_modifiedonbehalfby].[YomiFullName],
    [organization_cgi_localizationlanguage].[Name],

    -- physical attribute
    [cgi_localizationlanguageBase].[cgi_localizationlanguageId],
    [cgi_localizationlanguageBase].[CreatedOn],
    [cgi_localizationlanguageBase].[CreatedBy],
    [cgi_localizationlanguageBase].[ModifiedOn],
    [cgi_localizationlanguageBase].[ModifiedBy],
    [cgi_localizationlanguageBase].[CreatedOnBehalfBy],
    [cgi_localizationlanguageBase].[ModifiedOnBehalfBy],
    [cgi_localizationlanguageBase].[OrganizationId],
    [cgi_localizationlanguageBase].[statecode],
    [cgi_localizationlanguageBase].[statuscode],
    [cgi_localizationlanguageBase].[VersionNumber],
    [cgi_localizationlanguageBase].[ImportSequenceNumber],
    [cgi_localizationlanguageBase].[OverriddenCreatedOn],
    [cgi_localizationlanguageBase].[TimeZoneRuleVersionNumber],
    [cgi_localizationlanguageBase].[UTCConversionTimeZoneCode],
    [cgi_localizationlanguageBase].[cgi_localizationlanguagename],
    [cgi_localizationlanguageBase].[cgi_LocalizationLanguageNumber]
from [cgi_localizationlanguageBase] 
    left join [SystemUserBase] [lk_cgi_localizationlanguage_createdby] with(nolock) on ([cgi_localizationlanguageBase].[CreatedBy] = [lk_cgi_localizationlanguage_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_localizationlanguage_createdonbehalfby] with(nolock) on ([cgi_localizationlanguageBase].[CreatedOnBehalfBy] = [lk_cgi_localizationlanguage_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_localizationlanguage_modifiedby] with(nolock) on ([cgi_localizationlanguageBase].[ModifiedBy] = [lk_cgi_localizationlanguage_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_localizationlanguage_modifiedonbehalfby] with(nolock) on ([cgi_localizationlanguageBase].[ModifiedOnBehalfBy] = [lk_cgi_localizationlanguage_modifiedonbehalfby].[SystemUserId])
    left join [OrganizationBase] [organization_cgi_localizationlanguage] with(nolock) on ([cgi_localizationlanguageBase].[OrganizationId] = [organization_cgi_localizationlanguage].[OrganizationId])
