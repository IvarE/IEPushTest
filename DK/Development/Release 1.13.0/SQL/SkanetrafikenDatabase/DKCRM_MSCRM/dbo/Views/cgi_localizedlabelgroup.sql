


--
-- base view for cgi_localizedlabelgroup
--
create view dbo.[cgi_localizedlabelgroup]
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
    [cgi_localizedlabelgroupId],
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
    [cgi_localizedlabelgroupname]
) with view_metadata as
select
    -- logical attributes
    [lk_cgi_localizedlabelgroup_createdby].[FullName],
    [lk_cgi_localizedlabelgroup_createdby].[YomiFullName],
    [lk_cgi_localizedlabelgroup_createdonbehalfby].[FullName],
    [lk_cgi_localizedlabelgroup_createdonbehalfby].[YomiFullName],
    [lk_cgi_localizedlabelgroup_modifiedby].[FullName],
    [lk_cgi_localizedlabelgroup_modifiedby].[YomiFullName],
    [lk_cgi_localizedlabelgroup_modifiedonbehalfby].[FullName],
    [lk_cgi_localizedlabelgroup_modifiedonbehalfby].[YomiFullName],
    [organization_cgi_localizedlabelgroup].[Name],

    -- physical attribute
    [cgi_localizedlabelgroupBase].[cgi_localizedlabelgroupId],
    [cgi_localizedlabelgroupBase].[CreatedOn],
    [cgi_localizedlabelgroupBase].[CreatedBy],
    [cgi_localizedlabelgroupBase].[ModifiedOn],
    [cgi_localizedlabelgroupBase].[ModifiedBy],
    [cgi_localizedlabelgroupBase].[CreatedOnBehalfBy],
    [cgi_localizedlabelgroupBase].[ModifiedOnBehalfBy],
    [cgi_localizedlabelgroupBase].[OrganizationId],
    [cgi_localizedlabelgroupBase].[statecode],
    [cgi_localizedlabelgroupBase].[statuscode],
    [cgi_localizedlabelgroupBase].[VersionNumber],
    [cgi_localizedlabelgroupBase].[ImportSequenceNumber],
    [cgi_localizedlabelgroupBase].[OverriddenCreatedOn],
    [cgi_localizedlabelgroupBase].[TimeZoneRuleVersionNumber],
    [cgi_localizedlabelgroupBase].[UTCConversionTimeZoneCode],
    [cgi_localizedlabelgroupBase].[cgi_localizedlabelgroupname]
from [cgi_localizedlabelgroupBase] 
    left join [SystemUserBase] [lk_cgi_localizedlabelgroup_createdby] with(nolock) on ([cgi_localizedlabelgroupBase].[CreatedBy] = [lk_cgi_localizedlabelgroup_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_localizedlabelgroup_createdonbehalfby] with(nolock) on ([cgi_localizedlabelgroupBase].[CreatedOnBehalfBy] = [lk_cgi_localizedlabelgroup_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_localizedlabelgroup_modifiedby] with(nolock) on ([cgi_localizedlabelgroupBase].[ModifiedBy] = [lk_cgi_localizedlabelgroup_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_localizedlabelgroup_modifiedonbehalfby] with(nolock) on ([cgi_localizedlabelgroupBase].[ModifiedOnBehalfBy] = [lk_cgi_localizedlabelgroup_modifiedonbehalfby].[SystemUserId])
    left join [OrganizationBase] [organization_cgi_localizedlabelgroup] with(nolock) on ([cgi_localizedlabelgroupBase].[OrganizationId] = [organization_cgi_localizedlabelgroup].[OrganizationId])
