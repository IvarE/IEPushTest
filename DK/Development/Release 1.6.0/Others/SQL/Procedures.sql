USE [IntegrationDB]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetIncidentFromIncidentId]    Script Date: 2014-10-20 12:32:03 ******/
DROP PROCEDURE [dbo].[sp_GetIncidentFromIncidentId]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetDefaultCustomer]    Script Date: 2014-10-20 12:32:03 ******/
DROP PROCEDURE [dbo].[sp_GetDefaultCustomer]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetAccountFromEmail]    Script Date: 2014-10-20 12:32:03 ******/
DROP PROCEDURE [dbo].[sp_GetAccountFromEmail]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetAccountFromAccountNumber]    Script Date: 2014-10-20 12:32:03 ******/
DROP PROCEDURE [dbo].[sp_GetAccountFromAccountNumber]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetAccountFromAccountId]    Script Date: 2014-10-20 12:32:03 ******/
DROP PROCEDURE [dbo].[sp_GetAccountFromAccountId]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetAccountFromAccountId]    Script Date: 2014-10-20 12:32:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Reine Rosqvist
-- Create date: 2014-10-16
-- Description:	Search for account based on accountid
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetAccountFromAccountId] 
	@accountid	uniqueidentifier,
	@type			int
AS
BEGIN
	SET NOCOUNT ON;

    EXECUTE AS LOGIN = 'D1\CRMAdmin'

	if (@type = 0)
	begin
		select
		contact.contactid as accountid,
		contact.cgi_contactnumber as accountnumber,
		(contact.firstname + ' ' + contact.lastname) as name,
		contact.emailaddress1 as emailaddress
		from Skanetrafiken_MSCRM.dbo.FilteredContact contact
		where contact.contactid = @accountid
		and contact.statecode = 0
		for xml path ('account'), root ('accounts'), type
	end

	if (@type = 1)
	begin
		select
		account.accountid as accountid,
		account.accountnumber as accountnumber,
		account.name as name,
		account.emailaddress1 as emailaddress
		from Skanetrafiken_MSCRM.dbo.FilteredAccount account
		where account.accountid = @accountid
		and account.statecode = 0
		for xml path ('account'), root ('accounts'), type
	end

	REVERT

END



GO

/****** Object:  StoredProcedure [dbo].[sp_GetAccountFromAccountNumber]    Script Date: 2014-10-20 12:32:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		Reine Rosqvist
-- Create date: 2014-10-16
-- Description:	Search for account based on accountnumber
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetAccountFromAccountNumber] 
	@accountnumber	varchar(255),
	@type			int
AS
BEGIN
	SET NOCOUNT ON;

    EXECUTE AS LOGIN = 'D1\CRMAdmin'
	
	if (@type = 0)
	begin
		select
		contact.contactid as accountid,
		contact.cgi_contactnumber as accountnumber,
		(contact.firstname + ' ' + contact.lastname) as name,
		contact.emailaddress1 as emailaddress
		from Skanetrafiken_MSCRM.dbo.FilteredContact contact
			where contact.cgi_contactnumber = @accountnumber
			and contact.statecode = 0
		for xml path ('account'), root ('accounts'), type
	end

	if (@type = 1)
	begin
		select
			account.accountid as accountid,
			account.accountnumber as accountnumber,
			account.name as name,
			account.emailaddress1 as emailaddress
		from Skanetrafiken_MSCRM.dbo.FilteredAccount account
			where account.accountnumber = @accountnumber
			and account.statecode = 0
		for xml path ('account'), root ('accounts'), type
	end

	REVERT

END




GO

/****** Object:  StoredProcedure [dbo].[sp_GetAccountFromEmail]    Script Date: 2014-10-20 12:32:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Reine Rosqvist
-- Create date: 2014-10-16
-- Description:	Search for account based on emailaddress
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetAccountFromEmail] 
	@emailaddress	varchar(255),
	@type			int
AS
BEGIN
	SET NOCOUNT ON;

    EXECUTE AS LOGIN = 'D1\CRMAdmin'

	if (@type = 0)
	begin
		select
		contact.contactid as accountid,
		contact.cgi_contactnumber as accountnumber,
		(contact.firstname + ' ' + contact.lastname) as name,
		contact.emailaddress1 as emailaddress
		from Skanetrafiken_MSCRM.dbo.FilteredContact contact
		where contact.emailaddress1 = @emailaddress
		and contact.statecode = 0
		for xml path ('account'), root ('accounts'), type
	end

	if (@type = 1)
	begin
		select
		account.accountid as accountid,
		account.accountnumber as accountnumber,
		account.name as name,
		account.emailaddress1 as emailaddress
		from Skanetrafiken_MSCRM.dbo.FilteredAccount account
		where account.emailaddress1 = @emailaddress
		and account.statecode = 0
		for xml path ('account'), root ('accounts'), type
	end

	REVERT

END



GO

/****** Object:  StoredProcedure [dbo].[sp_GetDefaultCustomer]    Script Date: 2014-10-20 12:32:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Reine Rosqvist
-- Create date: 2014-10-20
-- Description:	Get default customer from setting
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetDefaultCustomer]
AS
BEGIN
	SET NOCOUNT ON;

    EXECUTE AS LOGIN = 'D1\CRMAdmin'

	declare @now datetime
	select @now = DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0)

	select setting.cgi_defaultcustomeroncase as defaultcustomeroncase
	from Skanetrafiken_MSCRM.dbo.Filteredcgi_setting setting
	where setting.cgi_validfrom <= @now
	and (setting.cgi_validto is null or setting.cgi_validto >= @now)
	and setting.statecode = 0
	for xml path ('setting'), root ('settings'), type

	REVERT

END

GO

/****** Object:  StoredProcedure [dbo].[sp_GetIncidentFromIncidentId]    Script Date: 2014-10-20 12:32:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Reine Rosqvist
-- Create date: 2014-10-16
-- Description:	get incident from incidentid
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetIncidentFromIncidentId] 
	@incidentid		uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

    EXECUTE AS LOGIN = 'D1\CRMAdmin'

	select
	incident.ticketnumber as ticketnumber,
	incident.title as title
	from Skanetrafiken_MSCRM.dbo.FilteredIncident incident
	where incident.incidentid = @incidentid
	for xml path ('incident'), root ('incidents'), type
	
	REVERT

END

GO


