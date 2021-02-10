


--
-- base view for cgi_applicationlog
--
create view dbo.[cgi_applicationlog]
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
    [cgi_applicationlogId],
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
    [cgi_applicationname],
    [cgi_LogType],
    [cgi_Method],
    [cgi_LogMessage],
    [cgi_SystemException]
) with view_metadata as
select
    -- logical attributes
    [lk_cgi_applicationlog_createdby].[FullName],
    [lk_cgi_applicationlog_createdby].[YomiFullName],
    [lk_cgi_applicationlog_createdonbehalfby].[FullName],
    [lk_cgi_applicationlog_createdonbehalfby].[YomiFullName],
    [lk_cgi_applicationlog_modifiedby].[FullName],
    [lk_cgi_applicationlog_modifiedby].[YomiFullName],
    [lk_cgi_applicationlog_modifiedonbehalfby].[FullName],
    [lk_cgi_applicationlog_modifiedonbehalfby].[YomiFullName],
    [organization_cgi_applicationlog].[Name],

    -- physical attribute
    [cgi_applicationlogBase].[cgi_applicationlogId],
    [cgi_applicationlogBase].[CreatedOn],
    [cgi_applicationlogBase].[CreatedBy],
    [cgi_applicationlogBase].[ModifiedOn],
    [cgi_applicationlogBase].[ModifiedBy],
    [cgi_applicationlogBase].[CreatedOnBehalfBy],
    [cgi_applicationlogBase].[ModifiedOnBehalfBy],
    [cgi_applicationlogBase].[OrganizationId],
    [cgi_applicationlogBase].[statecode],
    [cgi_applicationlogBase].[statuscode],
    [cgi_applicationlogBase].[VersionNumber],
    [cgi_applicationlogBase].[ImportSequenceNumber],
    [cgi_applicationlogBase].[OverriddenCreatedOn],
    [cgi_applicationlogBase].[TimeZoneRuleVersionNumber],
    [cgi_applicationlogBase].[UTCConversionTimeZoneCode],
    [cgi_applicationlogBase].[cgi_applicationname],
    [cgi_applicationlogBase].[cgi_LogType],
    [cgi_applicationlogBase].[cgi_Method],
    [cgi_applicationlogBase].[cgi_LogMessage],
    [cgi_applicationlogBase].[cgi_SystemException]
from [cgi_applicationlogBase] 
    left join [SystemUserBase] [lk_cgi_applicationlog_createdby] with(nolock) on ([cgi_applicationlogBase].[CreatedBy] = [lk_cgi_applicationlog_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_applicationlog_createdonbehalfby] with(nolock) on ([cgi_applicationlogBase].[CreatedOnBehalfBy] = [lk_cgi_applicationlog_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_applicationlog_modifiedby] with(nolock) on ([cgi_applicationlogBase].[ModifiedBy] = [lk_cgi_applicationlog_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_applicationlog_modifiedonbehalfby] with(nolock) on ([cgi_applicationlogBase].[ModifiedOnBehalfBy] = [lk_cgi_applicationlog_modifiedonbehalfby].[SystemUserId])
    left join [OrganizationBase] [organization_cgi_applicationlog] with(nolock) on ([cgi_applicationlogBase].[OrganizationId] = [organization_cgi_applicationlog].[OrganizationId])
