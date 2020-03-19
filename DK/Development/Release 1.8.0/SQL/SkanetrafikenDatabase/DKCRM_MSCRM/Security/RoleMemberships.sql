ALTER ROLE [db_owner] ADD MEMBER [D1\SQLAccessGroup {a75c8b9a-e617-4daa-8593-5a4231fe7cb5}];


GO
ALTER ROLE [db_owner] ADD MEMBER [StagingCRMSqlUser];


GO
ALTER ROLE [db_backupoperator] ADD MEMBER [backupadmin];


GO
ALTER ROLE [db_datareader] ADD MEMBER [StagingCRMSqlUser];

