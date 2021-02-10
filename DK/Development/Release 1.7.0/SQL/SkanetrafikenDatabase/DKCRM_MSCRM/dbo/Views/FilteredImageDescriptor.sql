

--
-- report view for imagedescriptor
--
create view dbo.[FilteredImageDescriptor] (
    [filetype],
    [imagedata],
    [imagedescriptorid],
    [imagetimestamp],
    [imageurl],
    [objectid],
    [objecttypecode],
    [size],
    [title]
) with view_metadata as
select
    [ImageDescriptor].[FileType],
    [ImageDescriptor].[ImageData],
    [ImageDescriptor].[ImageDescriptorId],
    [ImageDescriptor].[ImageTimestamp],
    [ImageDescriptor].[ImageURL],
    [ImageDescriptor].[ObjectId],
    [ImageDescriptor].[ObjectTypeCode],
    [ImageDescriptor].[Size],
    [ImageDescriptor].[Title]
from ImageDescriptor
