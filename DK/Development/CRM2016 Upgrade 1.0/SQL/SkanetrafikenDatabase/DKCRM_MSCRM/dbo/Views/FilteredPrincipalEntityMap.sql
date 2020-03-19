

--
-- report view for principalentitymap
--
create view dbo.[FilteredPrincipalEntityMap] (
    [objecttypecode],
    [principalentitymapid],
    [principalid],
    [versionnumber]
) with view_metadata as
select
    [PrincipalEntityMap].[ObjectTypeCode],
    [PrincipalEntityMap].[PrincipalEntityMapId],
    [PrincipalEntityMap].[PrincipalId],
    [PrincipalEntityMap].[VersionNumber]
from PrincipalEntityMap
