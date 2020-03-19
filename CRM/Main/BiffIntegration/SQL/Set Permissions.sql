

-- TESTMILJÖ --
-- **********************************

/****** Object:  Login [D1\dbDKReadDWTST]    Script Date: 2017-08-25 14:52:45 ******/
CREATE LOGIN [D1\dbDKReadDWTST] FROM WINDOWS WITH DEFAULT_DATABASE=[CrmTravelCards]
, DEFAULT_LANGUAGE=[us_english]



-- ACCMILJÖ --
-- **********************************
CREATE LOGIN [D1\dbDKReadDWACC] FROM WINDOWS WITH DEFAULT_DATABASE=[CrmTravelCards]
, DEFAULT_LANGUAGE=[us_english]
GO

USE [CrmTravelCards]
GO
CREATE USER [D1\dbDKReadDWACC] FOR LOGIN [D1\dbDKReadDWACC]

ALTER ROLE [db_owner] ADD MEMBER [D1\dbDKReadDWACC]

use DKCRM_MSCRM
CREATE USER [D1\dbDKReadDWACC] FOR LOGIN [D1\dbDKReadDWACC]
ALTER ROLE [db_owner] ADD MEMBER [D1\dbDKReadDWACC]




-- ProdMILJÖ --
-- **********************************
CREATE LOGIN [D1\dbDKReadDW] FROM WINDOWS WITH DEFAULT_DATABASE=[CrmTravelCards]
, DEFAULT_LANGUAGE=[us_english]
GO

USE [CrmTravelCards]
GO
CREATE USER [D1\dbDKReadDW] FOR LOGIN [D1\dbDKReadDW]
GO
ALTER ROLE [db_owner] ADD MEMBER [D1\dbDKReadDW]
GO


use DKCRM_MSCRM
CREATE USER [D1\dbDKReadDW] FOR LOGIN [D1\dbDKReadDW]
ALTER ROLE [db_owner] ADD MEMBER [D1\dbDKReadDW]



