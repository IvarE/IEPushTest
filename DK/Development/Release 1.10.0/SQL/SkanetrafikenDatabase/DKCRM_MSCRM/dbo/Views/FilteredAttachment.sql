

--
-- report view for attachment
--
create view dbo.[FilteredAttachment] (
    [attachmentid],
    [body],
    [filename],
    [filesize],
    [mimetype],
    [subject],
    [versionnumber]
) with view_metadata as
select
    [Attachment].[AttachmentId],
    [Attachment].[Body],
    [Attachment].[FileName],
    [Attachment].[FileSize],
    [Attachment].[MimeType],
    [Attachment].[Subject],
    [Attachment].[VersionNumber]
from Attachment
