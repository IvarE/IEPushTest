


--
-- base view for cgi_categorydetail
--
create view dbo.[cgi_categorydetail]
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
    [cgi_parentid2Name],
    [cgi_ParentidName],

    -- ownership entries
    OwnerId,
    OwnerIdName,
    OwnerIdYomiName,
    OwnerIdDsc,
    OwnerIdType,
    OwningUser,
    OwningTeam,

    -- physical attributes
    [cgi_categorydetailId],
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
    [cgi_categorydetailname],
    [cgi_CallguideCategory],
    [cgi_Color],
    [cgi_Sortorder],
    [cgi_Parentid],
    [cgi_Level],
    [cgi_parentid2],
    [cgi_requirestravelinfo]
) with view_metadata as
select
    -- logical attributes
    [lk_cgi_categorydetail_createdby].[FullName],
    [lk_cgi_categorydetail_createdby].[YomiFullName],
    [lk_cgi_categorydetail_createdonbehalfby].[FullName],
    [lk_cgi_categorydetail_createdonbehalfby].[YomiFullName],
    [lk_cgi_categorydetail_modifiedby].[FullName],
    [lk_cgi_categorydetail_modifiedby].[YomiFullName],
    [lk_cgi_categorydetail_modifiedonbehalfby].[FullName],
    [lk_cgi_categorydetail_modifiedonbehalfby].[YomiFullName],
    [cgi_categorydetail_cgi_categorydetail_parentid2].[cgi_categorydetailname],
    [cgi_cgi_categorydetail_cgi_categorydetail_Parentid].[cgi_categorydetailname],

    -- ownership entries
    OwnerId = [cgi_categorydetailBase].OwnerId,
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
    [cgi_categorydetailBase].[cgi_categorydetailId],
    [cgi_categorydetailBase].[CreatedOn],
    [cgi_categorydetailBase].[CreatedBy],
    [cgi_categorydetailBase].[ModifiedOn],
    [cgi_categorydetailBase].[ModifiedBy],
    [cgi_categorydetailBase].[CreatedOnBehalfBy],
    [cgi_categorydetailBase].[ModifiedOnBehalfBy],
    [cgi_categorydetailBase].[OwningBusinessUnit],
    [cgi_categorydetailBase].[statecode],
    [cgi_categorydetailBase].[statuscode],
    [cgi_categorydetailBase].[VersionNumber],
    [cgi_categorydetailBase].[ImportSequenceNumber],
    [cgi_categorydetailBase].[OverriddenCreatedOn],
    [cgi_categorydetailBase].[TimeZoneRuleVersionNumber],
    [cgi_categorydetailBase].[UTCConversionTimeZoneCode],
    [cgi_categorydetailBase].[cgi_categorydetailname],
    [cgi_categorydetailBase].[cgi_CallguideCategory],
    [cgi_categorydetailBase].[cgi_Color],
    [cgi_categorydetailBase].[cgi_Sortorder],
    [cgi_categorydetailBase].[cgi_Parentid],
    [cgi_categorydetailBase].[cgi_Level],
    [cgi_categorydetailBase].[cgi_parentid2],
    [cgi_categorydetailBase].[cgi_requirestravelinfo]
from [cgi_categorydetailBase] 
    left join [cgi_categorydetailBase] [cgi_categorydetail_cgi_categorydetail_parentid2] on ([cgi_categorydetailBase].[cgi_parentid2] = [cgi_categorydetail_cgi_categorydetail_parentid2].[cgi_categorydetailId])
    left join [cgi_categorydetailBase] [cgi_cgi_categorydetail_cgi_categorydetail_Parentid] on ([cgi_categorydetailBase].[cgi_Parentid] = [cgi_cgi_categorydetail_cgi_categorydetail_Parentid].[cgi_categorydetailId])
    left join [SystemUserBase] [lk_cgi_categorydetail_createdby] with(nolock) on ([cgi_categorydetailBase].[CreatedBy] = [lk_cgi_categorydetail_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_categorydetail_createdonbehalfby] with(nolock) on ([cgi_categorydetailBase].[CreatedOnBehalfBy] = [lk_cgi_categorydetail_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_categorydetail_modifiedby] with(nolock) on ([cgi_categorydetailBase].[ModifiedBy] = [lk_cgi_categorydetail_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_categorydetail_modifiedonbehalfby] with(nolock) on ([cgi_categorydetailBase].[ModifiedOnBehalfBy] = [lk_cgi_categorydetail_modifiedonbehalfby].[SystemUserId])
    left join OwnerBase XXowner with(nolock) on ([cgi_categorydetailBase].OwnerId = XXowner.OwnerId)
