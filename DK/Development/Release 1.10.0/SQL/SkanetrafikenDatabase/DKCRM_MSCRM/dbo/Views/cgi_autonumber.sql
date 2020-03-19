


--
-- base view for cgi_autonumber
--
create view dbo.[cgi_autonumber]
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
    [cgi_autonumberId],
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
    [cgi_entity],
    [cgi_LastUsed],
    [cgi_LockField]
) with view_metadata as
select
    -- logical attributes
    [lk_cgi_autonumber_createdby].[FullName],
    [lk_cgi_autonumber_createdby].[YomiFullName],
    [lk_cgi_autonumber_createdonbehalfby].[FullName],
    [lk_cgi_autonumber_createdonbehalfby].[YomiFullName],
    [lk_cgi_autonumber_modifiedby].[FullName],
    [lk_cgi_autonumber_modifiedby].[YomiFullName],
    [lk_cgi_autonumber_modifiedonbehalfby].[FullName],
    [lk_cgi_autonumber_modifiedonbehalfby].[YomiFullName],
    [organization_cgi_autonumber].[Name],

    -- physical attribute
    [cgi_autonumberBase].[cgi_autonumberId],
    [cgi_autonumberBase].[CreatedOn],
    [cgi_autonumberBase].[CreatedBy],
    [cgi_autonumberBase].[ModifiedOn],
    [cgi_autonumberBase].[ModifiedBy],
    [cgi_autonumberBase].[CreatedOnBehalfBy],
    [cgi_autonumberBase].[ModifiedOnBehalfBy],
    [cgi_autonumberBase].[OrganizationId],
    [cgi_autonumberBase].[statecode],
    [cgi_autonumberBase].[statuscode],
    [cgi_autonumberBase].[VersionNumber],
    [cgi_autonumberBase].[ImportSequenceNumber],
    [cgi_autonumberBase].[OverriddenCreatedOn],
    [cgi_autonumberBase].[TimeZoneRuleVersionNumber],
    [cgi_autonumberBase].[UTCConversionTimeZoneCode],
    [cgi_autonumberBase].[cgi_entity],
    [cgi_autonumberBase].[cgi_LastUsed],
    [cgi_autonumberBase].[cgi_LockField]
from [cgi_autonumberBase] 
    left join [SystemUserBase] [lk_cgi_autonumber_createdby] with(nolock) on ([cgi_autonumberBase].[CreatedBy] = [lk_cgi_autonumber_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_autonumber_createdonbehalfby] with(nolock) on ([cgi_autonumberBase].[CreatedOnBehalfBy] = [lk_cgi_autonumber_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_autonumber_modifiedby] with(nolock) on ([cgi_autonumberBase].[ModifiedBy] = [lk_cgi_autonumber_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_autonumber_modifiedonbehalfby] with(nolock) on ([cgi_autonumberBase].[ModifiedOnBehalfBy] = [lk_cgi_autonumber_modifiedonbehalfby].[SystemUserId])
    left join [OrganizationBase] [organization_cgi_autonumber] with(nolock) on ([cgi_autonumberBase].[OrganizationId] = [organization_cgi_autonumber].[OrganizationId])
