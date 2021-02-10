

--
-- report view for systemuserprofiles
--
create view dbo.[FilteredSystemUserProfiles] (
    [fieldsecurityprofileid],
    [systemuserid],
    [systemuserprofileid],
    [versionnumber]
) with view_metadata as
select
    [SystemUserProfiles].[FieldSecurityProfileId],
    [SystemUserProfiles].[SystemUserId],
    [SystemUserProfiles].[SystemUserProfileId],
    [SystemUserProfiles].[VersionNumber]
from SystemUserProfiles
