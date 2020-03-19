


--
-- base view for cgi_traffic_contract
--
create view dbo.[cgi_traffic_contract]
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
    [cgi_traffic_contractId],
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
    [cgi_contract_no],
    [cgi_contract_note],
    [cgi_contract_startdate],
    [cgi_contract_enddate]
) with view_metadata as
select
    -- logical attributes
    [lk_cgi_traffic_contract_createdby].[FullName],
    [lk_cgi_traffic_contract_createdby].[YomiFullName],
    [lk_cgi_traffic_contract_createdonbehalfby].[FullName],
    [lk_cgi_traffic_contract_createdonbehalfby].[YomiFullName],
    [lk_cgi_traffic_contract_modifiedby].[FullName],
    [lk_cgi_traffic_contract_modifiedby].[YomiFullName],
    [lk_cgi_traffic_contract_modifiedonbehalfby].[FullName],
    [lk_cgi_traffic_contract_modifiedonbehalfby].[YomiFullName],

    -- ownership entries
    OwnerId = [cgi_traffic_contractBase].OwnerId,
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
    [cgi_traffic_contractBase].[cgi_traffic_contractId],
    [cgi_traffic_contractBase].[CreatedOn],
    [cgi_traffic_contractBase].[CreatedBy],
    [cgi_traffic_contractBase].[ModifiedOn],
    [cgi_traffic_contractBase].[ModifiedBy],
    [cgi_traffic_contractBase].[CreatedOnBehalfBy],
    [cgi_traffic_contractBase].[ModifiedOnBehalfBy],
    [cgi_traffic_contractBase].[OwningBusinessUnit],
    [cgi_traffic_contractBase].[statecode],
    [cgi_traffic_contractBase].[statuscode],
    [cgi_traffic_contractBase].[VersionNumber],
    [cgi_traffic_contractBase].[ImportSequenceNumber],
    [cgi_traffic_contractBase].[OverriddenCreatedOn],
    [cgi_traffic_contractBase].[TimeZoneRuleVersionNumber],
    [cgi_traffic_contractBase].[UTCConversionTimeZoneCode],
    [cgi_traffic_contractBase].[cgi_name],
    [cgi_traffic_contractBase].[cgi_contract_no],
    [cgi_traffic_contractBase].[cgi_contract_note],
    [cgi_traffic_contractBase].[cgi_contract_startdate],
    [cgi_traffic_contractBase].[cgi_contract_enddate]
from [cgi_traffic_contractBase] 
    left join [SystemUserBase] [lk_cgi_traffic_contract_createdby] with(nolock) on ([cgi_traffic_contractBase].[CreatedBy] = [lk_cgi_traffic_contract_createdby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_traffic_contract_createdonbehalfby] with(nolock) on ([cgi_traffic_contractBase].[CreatedOnBehalfBy] = [lk_cgi_traffic_contract_createdonbehalfby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_traffic_contract_modifiedby] with(nolock) on ([cgi_traffic_contractBase].[ModifiedBy] = [lk_cgi_traffic_contract_modifiedby].[SystemUserId])
    left join [SystemUserBase] [lk_cgi_traffic_contract_modifiedonbehalfby] with(nolock) on ([cgi_traffic_contractBase].[ModifiedOnBehalfBy] = [lk_cgi_traffic_contract_modifiedonbehalfby].[SystemUserId])
    left join OwnerBase XXowner with(nolock) on ([cgi_traffic_contractBase].OwnerId = XXowner.OwnerId)
