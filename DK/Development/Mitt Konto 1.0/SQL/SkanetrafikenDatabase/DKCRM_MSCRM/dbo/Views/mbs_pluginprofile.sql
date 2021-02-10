


--
-- base view for mbs_pluginprofile
--
create view dbo.[mbs_pluginprofile]
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
    [mbs_BusinessUnitIdName],
    [mbs_InitiatingUserIdYomiName],
    [mbs_InitiatingUserIdName],

    -- physical attributes
    [mbs_pluginprofileId],
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
    [mbs_label],
    [mbs_Configuration],
    [mbs_CorrelationId],
    [mbs_Depth],
    [mbs_MessageName],
    [mbs_mode],
    [mbs_OperationType],
    [mbs_PerformanceConstructorDuration],
    [mbs_PerformanceConstructorStartTime],
    [mbs_PerformanceExecutionDuration],
    [mbs_PerformanceExecutionStartTime],
    [mbs_PersistenceKey],
    [mbs_PrimaryEntity],
    [mbs_Profile],
    [mbs_RequestId],
    [mbs_SecureConfiguration],
    [mbs_WorkflowStepId],
    [mbs_BusinessUnitId],
    [mbs_InitiatingUserId]
) with view_metadata as
select
    -- logical attributes
    [lk_mbs_pluginprofile_createdby].[FullName],
    [lk_mbs_pluginprofile_createdby].[YomiFullName],
    [lk_mbs_pluginprofile_createdonbehalfby].[FullName],
    [lk_mbs_pluginprofile_createdonbehalfby].[YomiFullName],
    [lk_mbs_pluginprofile_modifiedby].[FullName],
    [lk_mbs_pluginprofile_modifiedby].[YomiFullName],
    [lk_mbs_pluginprofile_modifiedonbehalfby].[FullName],
    [lk_mbs_pluginprofile_modifiedonbehalfby].[YomiFullName],
    [organization_mbs_pluginprofile].[Name],
    [mbs_businessunit_mbs_pluginprofile].[Name],
    [mbs_systemuser_mbs_pluginprofile].[YomiFullName],
    [mbs_systemuser_mbs_pluginprofile].[FullName],

    -- physical attribute
    [mbs_pluginprofileBase].[mbs_pluginprofileId],
    [mbs_pluginprofileBase].[CreatedOn],
    [mbs_pluginprofileBase].[CreatedBy],
    [mbs_pluginprofileBase].[ModifiedOn],
    [mbs_pluginprofileBase].[ModifiedBy],
    [mbs_pluginprofileBase].[CreatedOnBehalfBy],
    [mbs_pluginprofileBase].[ModifiedOnBehalfBy],
    [mbs_pluginprofileBase].[OrganizationId],
    [mbs_pluginprofileBase].[statecode],
    [mbs_pluginprofileBase].[statuscode],
    [mbs_pluginprofileBase].[VersionNumber],
    [mbs_pluginprofileBase].[ImportSequenceNumber],
    [mbs_pluginprofileBase].[OverriddenCreatedOn],
    [mbs_pluginprofileBase].[TimeZoneRuleVersionNumber],
    [mbs_pluginprofileBase].[UTCConversionTimeZoneCode],
    [mbs_pluginprofileBase].[mbs_label],
    [mbs_pluginprofileBase].[mbs_Configuration],
    [mbs_pluginprofileBase].[mbs_CorrelationId],
    [mbs_pluginprofileBase].[mbs_Depth],
    [mbs_pluginprofileBase].[mbs_MessageName],
    [mbs_pluginprofileBase].[mbs_mode],
    [mbs_pluginprofileBase].[mbs_OperationType],
    [mbs_pluginprofileBase].[mbs_PerformanceConstructorDuration],
    [mbs_pluginprofileBase].[mbs_PerformanceConstructorStartTime],
    [mbs_pluginprofileBase].[mbs_PerformanceExecutionDuration],
    [mbs_pluginprofileBase].[mbs_PerformanceExecutionStartTime],
    [mbs_pluginprofileBase].[mbs_PersistenceKey],
    [mbs_pluginprofileBase].[mbs_PrimaryEntity],
    [mbs_pluginprofileBase].[mbs_Profile],
    [mbs_pluginprofileBase].[mbs_RequestId],
    [mbs_pluginprofileBase].[mbs_SecureConfiguration],
    [mbs_pluginprofileBase].[mbs_WorkflowStepId],
    [mbs_pluginprofileBase].[mbs_BusinessUnitId],
    [mbs_pluginprofileBase].[mbs_InitiatingUserId]
from [mbs_pluginprofileBase] 
    left join [SystemUserBase] [lk_mbs_pluginprofile_createdby] with(nolock) on ([mbs_pluginprofileBase].[CreatedBy] = [lk_mbs_pluginprofile_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_mbs_pluginprofile_createdonbehalfby] with(nolock) on ([mbs_pluginprofileBase].[CreatedOnBehalfBy] = [lk_mbs_pluginprofile_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_mbs_pluginprofile_modifiedby] with(nolock) on ([mbs_pluginprofileBase].[ModifiedBy] = [lk_mbs_pluginprofile_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_mbs_pluginprofile_modifiedonbehalfby] with(nolock) on ([mbs_pluginprofileBase].[ModifiedOnBehalfBy] = [lk_mbs_pluginprofile_modifiedonbehalfby].[SystemUserId])
    left join [BusinessUnitBase] [mbs_businessunit_mbs_pluginprofile] on ([mbs_pluginprofileBase].[mbs_BusinessUnitId] = [mbs_businessunit_mbs_pluginprofile].[BusinessUnitId])
    left join [SystemUserBase] [mbs_systemuser_mbs_pluginprofile] with(nolock) on ([mbs_pluginprofileBase].[mbs_InitiatingUserId] = [mbs_systemuser_mbs_pluginprofile].[SystemUserId])
    left join [OrganizationBase] [organization_mbs_pluginprofile] with(nolock) on ([mbs_pluginprofileBase].[OrganizationId] = [organization_mbs_pluginprofile].[OrganizationId])
