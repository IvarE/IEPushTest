CREATE NONCLUSTERED INDEX IX_ED_AuditBase_Composite1 ON AuditBase
(
	ObjectTypeCode ASC,
	ObjectId ASC,
	CreatedOn ASC,
	AuditId ASC,
	[Action] ASC,
	CallingUserId ASC,
	UserId ASC,
	ObjectIdName ASC,
	Operation ASC
)
INCLUDE (AttributeMask, ChangeData)