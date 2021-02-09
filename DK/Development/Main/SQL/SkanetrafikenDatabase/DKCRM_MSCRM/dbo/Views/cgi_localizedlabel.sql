


--
-- base view for cgi_localizedlabel
--
create view dbo.[cgi_localizedlabel]
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
    [cgi_LocalizationLanguageidName],
    [OrganizationIdName],
    [cgi_LocalizedLabelGroupidName],

    -- physical attributes
    [cgi_localizedlabelId],
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
    [cgi_localizedlabelname],
    [cgi_LocalizedControlId],
    [cgi_LocalizationLanguageid],
    [cgi_LocalizedLabelGroupid]
) with view_metadata as
select
    -- logical attributes
    [lk_cgi_localizedlabel_createdby].[FullName],
    [lk_cgi_localizedlabel_createdby].[YomiFullName],
    [lk_cgi_localizedlabel_createdonbehalfby].[FullName],
    [lk_cgi_localizedlabel_createdonbehalfby].[YomiFullName],
    [lk_cgi_localizedlabel_modifiedby].[FullName],
    [lk_cgi_localizedlabel_modifiedby].[YomiFullName],
    [lk_cgi_localizedlabel_modifiedonbehalfby].[FullName],
    [lk_cgi_localizedlabel_modifiedonbehalfby].[YomiFullName],
    [cgi_cgi_localizationlanguage_cgi_localizedlabel_LocalizationLanguageid].[cgi_localizationlanguagename],
    [organization_cgi_localizedlabel].[Name],
    [cgi_cgi_localizedlabelgroup_cgi_localizedlabel_LocalizedLabelGroupid].[cgi_localizedlabelgroupname],

    -- physical attribute
    [cgi_localizedlabelBase].[cgi_localizedlabelId],
    [cgi_localizedlabelBase].[CreatedOn],
    [cgi_localizedlabelBase].[CreatedBy],
    [cgi_localizedlabelBase].[ModifiedOn],
    [cgi_localizedlabelBase].[ModifiedBy],
    [cgi_localizedlabelBase].[CreatedOnBehalfBy],
    [cgi_localizedlabelBase].[ModifiedOnBehalfBy],
    [cgi_localizedlabelBase].[OrganizationId],
    [cgi_localizedlabelBase].[statecode],
    [cgi_localizedlabelBase].[statuscode],
    [cgi_localizedlabelBase].[VersionNumber],
    [cgi_localizedlabelBase].[ImportSequenceNumber],
    [cgi_localizedlabelBase].[OverriddenCreatedOn],
    [cgi_localizedlabelBase].[TimeZoneRuleVersionNumber],
    [cgi_localizedlabelBase].[UTCConversionTimeZoneCode],
    [cgi_localizedlabelBase].[cgi_localizedlabelname],
    [cgi_localizedlabelBase].[cgi_LocalizedControlId],
    [cgi_localizedlabelBase].[cgi_LocalizationLanguageid],
    [cgi_localizedlabelBase].[cgi_LocalizedLabelGroupid]
from [cgi_localizedlabelBase] 
    left join [cgi_localizationlanguageBase] [cgi_cgi_localizationlanguage_cgi_localizedlabel_LocalizationLanguageid] on ([cgi_localizedlabelBase].[cgi_LocalizationLanguageid] = [cgi_cgi_localizationlanguage_cgi_localizedlabel_LocalizationLanguageid].[cgi_localizationlanguageId])
    left join [cgi_localizedlabelgroupBase] [cgi_cgi_localizedlabelgroup_cgi_localizedlabel_LocalizedLabelGroupid] on ([cgi_localizedlabelBase].[cgi_LocalizedLabelGroupid] = [cgi_cgi_localizedlabelgroup_cgi_localizedlabel_LocalizedLabelGroupid].[cgi_localizedlabelgroupId])
    left join [SystemUserBase] [lk_cgi_localizedlabel_createdby] with(nolock) on ([cgi_localizedlabelBase].[CreatedBy] = [lk_cgi_localizedlabel_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_localizedlabel_createdonbehalfby] with(nolock) on ([cgi_localizedlabelBase].[CreatedOnBehalfBy] = [lk_cgi_localizedlabel_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_localizedlabel_modifiedby] with(nolock) on ([cgi_localizedlabelBase].[ModifiedBy] = [lk_cgi_localizedlabel_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_localizedlabel_modifiedonbehalfby] with(nolock) on ([cgi_localizedlabelBase].[ModifiedOnBehalfBy] = [lk_cgi_localizedlabel_modifiedonbehalfby].[SystemUserId])
    left join [OrganizationBase] [organization_cgi_localizedlabel] with(nolock) on ([cgi_localizedlabelBase].[OrganizationId] = [organization_cgi_localizedlabel].[OrganizationId])
