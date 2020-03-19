


--
-- base view for cgi_casecategory
--
create view dbo.[cgi_casecategory]
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
    [cgi_CaseidName],
    [cgi_Category2idName],
    [cgi_Category3idName],
    [cgi_Category1idName],

    -- ownership entries
    OwnerId,
    OwnerIdName,
    OwnerIdYomiName,
    OwnerIdDsc,
    OwnerIdType,
    OwningUser,
    OwningTeam,

    -- physical attributes
    [cgi_casecategoryId],
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
    [cgi_casecategoryname],
    [cgi_Category1id],
    [cgi_Category2id],
    [cgi_Category3id],
    [cgi_Caseid]
) with view_metadata as
select
    -- logical attributes
    [lk_cgi_casecategory_createdby].[FullName],
    [lk_cgi_casecategory_createdby].[YomiFullName],
    [lk_cgi_casecategory_createdonbehalfby].[FullName],
    [lk_cgi_casecategory_createdonbehalfby].[YomiFullName],
    [lk_cgi_casecategory_modifiedby].[FullName],
    [lk_cgi_casecategory_modifiedby].[YomiFullName],
    [lk_cgi_casecategory_modifiedonbehalfby].[FullName],
    [lk_cgi_casecategory_modifiedonbehalfby].[YomiFullName],
    [cgi_incident_cgi_casecategory_Caseid].[Title],
    [cgi_cgi_categorydetail_cgi_casecategory_Category2id].[cgi_categorydetailname],
    [cgi_cgi_categorydetail_cgi_casecategory_Category3id].[cgi_categorydetailname],
    [cgi_cgi_categorydetail_cgi_casecategory_Category1id].[cgi_categorydetailname],

    -- ownership entries
    OwnerId = [cgi_casecategoryBase].OwnerId,
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
    [cgi_casecategoryBase].[cgi_casecategoryId],
    [cgi_casecategoryBase].[CreatedOn],
    [cgi_casecategoryBase].[CreatedBy],
    [cgi_casecategoryBase].[ModifiedOn],
    [cgi_casecategoryBase].[ModifiedBy],
    [cgi_casecategoryBase].[CreatedOnBehalfBy],
    [cgi_casecategoryBase].[ModifiedOnBehalfBy],
    [cgi_casecategoryBase].[OwningBusinessUnit],
    [cgi_casecategoryBase].[statecode],
    [cgi_casecategoryBase].[statuscode],
    [cgi_casecategoryBase].[VersionNumber],
    [cgi_casecategoryBase].[ImportSequenceNumber],
    [cgi_casecategoryBase].[OverriddenCreatedOn],
    [cgi_casecategoryBase].[TimeZoneRuleVersionNumber],
    [cgi_casecategoryBase].[UTCConversionTimeZoneCode],
    [cgi_casecategoryBase].[cgi_casecategoryname],
    [cgi_casecategoryBase].[cgi_Category1id],
    [cgi_casecategoryBase].[cgi_Category2id],
    [cgi_casecategoryBase].[cgi_Category3id],
    [cgi_casecategoryBase].[cgi_Caseid]
from [cgi_casecategoryBase] 
    left join [cgi_categorydetailBase] [cgi_cgi_categorydetail_cgi_casecategory_Category1id] on ([cgi_casecategoryBase].[cgi_Category1id] = [cgi_cgi_categorydetail_cgi_casecategory_Category1id].[cgi_categorydetailId])
    left join [cgi_categorydetailBase] [cgi_cgi_categorydetail_cgi_casecategory_Category2id] on ([cgi_casecategoryBase].[cgi_Category2id] = [cgi_cgi_categorydetail_cgi_casecategory_Category2id].[cgi_categorydetailId])
    left join [cgi_categorydetailBase] [cgi_cgi_categorydetail_cgi_casecategory_Category3id] on ([cgi_casecategoryBase].[cgi_Category3id] = [cgi_cgi_categorydetail_cgi_casecategory_Category3id].[cgi_categorydetailId])
    left join [IncidentBase] [cgi_incident_cgi_casecategory_Caseid] on ([cgi_casecategoryBase].[cgi_Caseid] = [cgi_incident_cgi_casecategory_Caseid].[IncidentId])
    left join [SystemUserBase] [lk_cgi_casecategory_createdby] with(nolock) on ([cgi_casecategoryBase].[CreatedBy] = [lk_cgi_casecategory_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_casecategory_createdonbehalfby] with(nolock) on ([cgi_casecategoryBase].[CreatedOnBehalfBy] = [lk_cgi_casecategory_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_casecategory_modifiedby] with(nolock) on ([cgi_casecategoryBase].[ModifiedBy] = [lk_cgi_casecategory_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_casecategory_modifiedonbehalfby] with(nolock) on ([cgi_casecategoryBase].[ModifiedOnBehalfBy] = [lk_cgi_casecategory_modifiedonbehalfby].[SystemUserId])
    left join OwnerBase XXowner with(nolock) on ([cgi_casecategoryBase].OwnerId = XXowner.OwnerId)
