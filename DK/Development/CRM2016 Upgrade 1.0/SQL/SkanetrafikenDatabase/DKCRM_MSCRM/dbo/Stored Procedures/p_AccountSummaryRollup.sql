﻿

CREATE   PROC [dbo].[p_AccountSummaryRollup](
	@accountId UNIQUEIDENTIFIER,
        @subAccount BIT,
	@acctDepth INT,
	@subContact BIT,
	@contactDepth INT,
	@displayEntities BIT,
	@startDate DATETIME,
	@endDate DATETIME,
	@displayNotes BIT
        ) AS
BEGIN
	SET NOCOUNT ON

Declare @rsize int
Declare @constr nvarchar(3)

-- size of regarding column in temp table #TempRollupIds created in .rdl file
SET @rsize = 300
SET @constr = ' > '

INSERT INTO #TempRollupIds(rollupid,type,regardingid,ownerid,owneridname,subject,regarding) 
(
  SELECT accountid,1,parentaccountid,ownerid,owneridname,[name],'' FROM FilteredAccount WHERE accountid = @accountId
)

IF( @subAccount <> 0 )
BEGIN
 WHILE (@acctDepth <> 0)
	BEGIN
		INSERT INTO #TempRollupIds(rollupid,type,regardingid,ownerid,owneridname,subject,regarding)
		(
			SELECT a.accountid,1,a.parentaccountid,a.ownerid,a.owneridname,a.[name],
			-- a.parentaccountidname is same as ri.subject as join is on the ids
			( CASE WHEN ri.regarding = '' THEN a.parentaccountidname ELSE RIGHT( ri.regarding + @constr + ri.subject , @rsize )  END )
			FROM FilteredAccount a (NOLOCK)
			join #TempRollupIds ri ON
			(
			a.parentaccountid = ri.rollupid
			)
			where
				not exists(
					select rollupid from #TempRollupIds where a.accountid = rollupid
				)
		)

		IF ( @@rowcount = 0 )
		BEGIN
			BREAK	
		END 
	
		SET @acctDepth = @acctDepth - 1
	END
End

IF( @subContact <> 0 )
BEGIN
	Declare @tempdepth int
	SET @tempdepth = @contactDepth
		
	INSERT INTO #TempRollupIds(rollupid,type,regardingid,source,ownerid,owneridname,createdon,subject,statuscodename,regarding)
	(
		SELECT c.contactid,2,c.parentcustomerid,1,c.ownerid,c.owneridname,c.createdon,c.fullname,c.statuscodename,
		-- c.parentcustomeridname is same as ri.subject as join is on the ids
		( CASE WHEN ri.regarding = '' THEN c.parentcustomeridname ELSE RIGHT( ri.regarding + @constr+ ri.subject , @rsize )  END )
		FROM FilteredContact c (NOLOCK)
		join #TempRollupIds ri ON
		(
			c.parentcustomerid = ri.rollupid
		)
		WHERE
			c.parentcustomeridtype = 1 and
			not exists(
				SELECT rollupid FROM #TempRollupIds WHERE c.contactid = rollupid
			)
	)

	IF ( @@rowcount = 0 )
	BEGIN
		SET @contactDepth = 0
	END 
	
	
  	SET @tempdepth = @contactDepth -1
	WHILE (@tempdepth > 0)
		BEGIN
			INSERT INTO #TempRollupIds(rollupid,type,regardingid,source,ownerid,owneridname,createdon,subject,statuscodename,regarding)
			(
				SELECT c.contactid,2,c.parentcustomerid,2,c.ownerid,c.owneridname,c.createdon,c.fullname,c.statuscodename,
				RIGHT( ri.regarding + @constr+ ri.subject , @rsize )
				FROM FilteredContact c (NOLOCK)
				join #TempRollupIds ri ON
				(
					c.parentcustomerid = ri.rollupid
				)
				WHERE
					c.parentcustomeridtype = 2 and
					not exists(
						SELECT rollupid FROM #TempRollupIds WHERE c.contactid = rollupid
					)
			)
	
			IF ( @@rowcount = 0 )
			BEGIN
				BREAK	
			END 
		
			SET @tempdepth = @tempdepth - 1
		END
End

-- Now the Account + (sub-accounts and sub-contacts are included in temp table)
-- Check if display entities option is selected
IF ( @displayEntities <> 0)
	BEGIN
		-- Start with opportunities for Accounts
		INSERT INTO #TempRollupIds (rollupid,type,regardingid,source,ownerid,owneridname,createdon,
				[description],subject,statuscodename,regarding)
		(
		  SELECT opp.opportunityid,3,opp.customerid,1,opp.ownerid,opp.owneridname,opp.createdon,
				opp.[description],opp.[name],opp.statuscodename,
				-- opp.accountidname is same as ri.subject as join is on the ids
		  		( CASE WHEN ri.regarding = '' THEN opp.accountidname ELSE RIGHT( ri.regarding + @constr + ri.subject , @rsize )  END )
		  FROM FilteredOpportunity opp
		  join #TempRollupIds ri
		  ON opp.customerid = ri.rollupid
		  WHERE  opp.customeridtype = 1 and

		      not exists (
			SELECT rollupid FROM #TempRollupIds WHERE opp.opportunityid = rollupid 
			)
			and
			opp.createdonutc >= @startDate and opp.createdonutc <= @endDate
		)
		
		-- opportunities for Contacts
		INSERT INTO #TempRollupIds (rollupid,type,regardingid,source,ownerid,owneridname,createdon,
				[description],subject,statuscodename,regarding)
		(
		  SELECT opp.opportunityid,3,opp.customerid,2,opp.ownerid,opp.owneridname,opp.createdon,
				opp.[description],opp.[name],opp.statuscodename,RIGHT( ri.regarding + @constr+ ri.subject , @rsize )
		  FROM FilteredOpportunity opp
		  join #TempRollupIds ri
		  ON opp.customerid = ri.rollupid
		  WHERE opp.customeridtype = 2 and
		      not exists (
			SELECT rollupid FROM #TempRollupIds WHERE opp.opportunityid =rollupid 
			)
			and
			opp.createdonutc >= @startDate and opp.createdonutc <= @endDate
		)
		
		-- Quotes for Accounts
		INSERT INTO #TempRollupIds (rollupid,type,regardingid,source,ownerid,owneridname,createdon,
				[description],subject,statuscodename,regarding)
		(
		  SELECT q.quoteid,1084,q.customerid,1,q.ownerid,q.owneridname,q.createdon,
				q.[description],q.[name],q.statecodename,
				-- q.accountidname is same as ri.subject as join is on the ids
				( CASE WHEN ri.regarding = '' THEN q.accountidname ELSE RIGHT( ri.regarding + @constr+ ri.subject , @rsize )  END )
		  FROM FilteredQuote q
		  join #TempRollupIds ri
		  ON q.customerid = ri.rollupid
		  WHERE q.customeridtype =1 and
		       not exists (
			SELECT rollupid FROM #TempRollupIds WHERE q.quoteid =rollupid 
			)
			and
			q.createdonutc >= @startDate and q.createdonutc <= @endDate
		)
		
		-- Quotes for Contacts
		INSERT INTO #TempRollupIds (rollupid,type,regardingid,source,ownerid,owneridname,createdon,
				[description],subject,statuscodename,regarding)
		(
		  SELECT q.quoteid,1084,q.customerid,2,q.ownerid,q.owneridname,q.createdon,
				q.[description],q.[name],q.statecodename,RIGHT( ri.regarding + @constr+ ri.subject , @rsize )
		  FROM FilteredQuote q
		  join #TempRollupIds ri
		  ON q.customerid = ri.rollupid
		  WHERE q.customeridtype =2 and
		       not exists (
			SELECT rollupid FROM #TempRollupIds WHERE q.quoteid =rollupid 
			)
			and
			q.createdonutc >= @startDate and q.createdonutc <= @endDate
		)
		
		-- Order for account
		INSERT INTO #TempRollupIds (rollupid,type,regardingid,source,ownerid,owneridname,createdon,
				[description],subject,statuscodename,regarding)
		(
		  SELECT o.salesorderid,1088,o.customerid,1,o.ownerid,o.owneridname,o.createdon,
				o.[description],o.[name],o.statuscodename,
				-- o.accountidname is same as ri.subject as join is on the ids
				( CASE WHEN ri.regarding = '' THEN o.accountidname ELSE RIGHT( ri.regarding + @constr+ ri.subject , @rsize )  END )
		  from FilteredSalesOrder o
		  join #TempRollupIds ri
		  on o.customerid = ri.rollupid
		  where o.customeridtype =1 and
		       not exists (
			select rollupid from #TempRollupIds where o.salesorderid =rollupid 
			)
			and
			o.createdonutc >= @startDate and o.createdonutc <= @endDate
		)
		
		-- Order for contact
		INSERT INTO #TempRollupIds (rollupid,type,regardingid,source,ownerid,owneridname,createdon,
				[description],subject,statuscodename,regarding)
		(
		  SELECT o.salesorderid,1088,o.customerid,2,o.ownerid,o.owneridname,o.createdon,
				o.[description],o.[name],o.statuscodename,RIGHT( ri.regarding + @constr + ri.subject , @rsize )
		  FROM FilteredSalesOrder o
		  join #TempRollupIds ri
		  ON o.customerid = ri.rollupid
		  WHERE o.customeridtype =2 and
		       not exists (
			SELECT rollupid FROM #TempRollupIds WHERE o.salesorderid =rollupid 
			)
			and
			o.createdonutc >= @startDate and o.createdonutc <= @endDate
		)
		
		-- Invoice for account
		INSERT INTO #TempRollupIds (rollupid,type,regardingid,source,ownerid,owneridname,createdon,
				[description],subject,statuscodename,regarding)
		(
		  SELECT inv.invoiceid,1090,inv.customerid,1,inv.ownerid,inv.owneridname,inv.createdon,
				inv.[description],inv.[name],inv.statuscodename,
				-- inv.accountidname is same as ri.subject as join is on the ids
				( CASE WHEN ri.regarding = '' THEN inv.accountidname ELSE RIGHT( ri.regarding + @constr+ ri.subject , @rsize )  END )
		  FROM FilteredInvoice inv
		  JOIN #TempRollupIds ri
		  ON inv.customerid = ri.rollupid
		  WHERE inv.customeridtype =1 and
		       NOT EXISTS (
			SELECT rollupid FROM #TempRollupIds WHERE inv.invoiceid =rollupid 
			)
			AND
			inv.createdonutc >= @startDate AND inv.createdonutc <= @endDate
		)
		
		-- Invoice for contact
		INSERT INTO #TempRollupIds (rollupid,type,regardingid,source,ownerid,owneridname,createdon,
				[description],subject,statuscodename,regarding)
		(
		  SELECT inv.invoiceid,1090,inv.customerid,2,inv.ownerid,inv.owneridname,inv.createdon,
				inv.[description],inv.[name],inv.statuscodename,RIGHT( ri.regarding + @constr+ ri.subject , @rsize )
		  FROM FilteredInvoice inv
		  JOIN #TempRollupIds ri
		  ON inv.customerid = ri.rollupid
		  WHERE inv.customeridtype =2 and
		       NOT EXISTS (
			SELECT rollupid FROM #TempRollupIds WHERE inv.invoiceid =rollupid 
			)
			AND
			inv.createdonutc >= @startDate AND inv.createdonutc <= @endDate
		)
		
		-- Contract for accounts
		INSERT INTO #TempRollupIds (rollupid,type,regardingid,source,ownerid,owneridname,createdon,
				[description],subject,statuscodename,regarding)
		(
		  SELECT con.contractid,1010,con.customerid,1,con.ownerid,con.owneridname,con.createdon,
				null,con.title,con.statecodename,
				-- con.accountidname is same as ri.subject as join is on the ids
				( CASE WHEN ri.regarding = '' THEN con.accountidname ELSE RIGHT( ri.regarding + @constr + ri.subject , @rsize )  END )
		  FROM FilteredContract con
		  JOIN #TempRollupIds ri
		  ON con.customerid = ri.rollupid
		  WHERE con.customeridtype =1 and
		       NOT EXISTS (
			SELECT rollupid FROM #TempRollupIds WHERE con.contractid =rollupid 
			)
			and
			con.createdonutc >= @startDate and con.createdonutc <= @endDate
		)
		
		-- Contract for contacts
		INSERT INTO #TempRollupIds (rollupid,type,regardingid,source,ownerid,owneridname,createdon,
				[description],subject,statuscodename,regarding)
		(
		  SELECT con.contractid,1010,con.customerid,2,con.ownerid,con.owneridname,con.createdon,
				null,con.title,con.statecodename,RIGHT( ri.regarding + @constr + ri.subject , @rsize )
		  FROM FilteredContract con
		  JOIN #TempRollupIds ri
		  ON con.customerid = ri.rollupid
		  WHERE con.customeridtype =2 and
		       NOT EXISTS (
			SELECT rollupid FROM #TempRollupIds WHERE con.contractid =rollupid 
			)
			and
			con.createdonutc >= @startDate and con.createdonutc <= @endDate
		)
		
		-- Cases for account
		INSERT INTO #TempRollupIds (rollupid,type,regardingid,source,ownerid,owneridname,createdon,
				[description],subject,statuscodename,regarding)
		(
		  SELECT cas.incidentid,112,cas.customerid,1,cas.ownerid,cas.owneridname,cas.createdon,
				cas.[description],cas.title,cas.statuscodename,
				-- cas.accountidname is same as ri.subject as join is on the ids
				( CASE WHEN ri.regarding = '' THEN cas.accountidname ELSE RIGHT( ri.regarding + @constr + ri.subject , @rsize )  END )
		  FROM FilteredIncident cas
		  JOIN #TempRollupIds ri
		  ON cas.customerid = ri.rollupid
		  WHERE cas.customeridtype =1 and
		       NOT EXISTS (
			SELECT rollupid FROM #TempRollupIds WHERE cas.incidentid =rollupid 
			)
			AND
			cas.createdonutc >= @startDate and cas.createdonutc <= @endDate 
		)
		
		-- Cases for contacts
		INSERT INTO #TempRollupIds (rollupid,type,regardingid,source,ownerid,owneridname,createdon,
				[description],subject,statuscodename,regarding)
		(
		  SELECT cas.incidentid,112,cas.customerid,2,cas.ownerid,cas.owneridname,cas.createdon,
				cas.[description],cas.title,cas.statuscodename,RIGHT( ri.regarding + @constr + ri.subject , @rsize )
		  FROM FilteredIncident cas
		  JOIN #TempRollupIds ri
		  ON cas.customerid = ri.rollupid
		  WHERE cas.customeridtype =2 and
		       NOT EXISTS (
			select rollupid from #TempRollupIds where cas.incidentid =rollupid 
			)
			AND
			cas.createdonutc >= @startDate and cas.createdonutc <= @endDate
		)
	END -- @displayEntities
	
-- Include all the activities associated with all the ids in temprollupids
INSERT INTO #TempRollupIds (rollupid,type,regardingid,source,ownerid,owneridname,createdon,
		[description],subject,statuscodename,regarding)
(
  SELECT ap.activityid,ap.activitytypecode,ap.regardingobjectid,ap.regardingobjecttypecode,
         ap.ownerid,ap.owneridname,ap.createdon,ap.[description],ap.subject,
	ap.statecodename as statuscodename, 
	-- ap.regardingobjectidname is same as ri.subject as join is on the ids
	( CASE WHEN ri.regarding = '' THEN ap.regardingobjectidname ELSE RIGHT( ri.regarding + @constr + ri.subject , @rsize )  END )
  FROM FilteredActivityPointer ap
  JOIN #TempRollupIds ri
  ON ap.regardingobjectid = ri.rollupid
  WHERE
	NOT EXISTS (
	SELECT rollupid FROM #TempRollupIds WHERE ap.activityid =rollupid 
	)
	AND
	(
	 ap.activitytypecode NOT IN (4206,4208,4209,4211) 
	)
	AND
	(
		( ap.scheduledendutc >= @startDate AND ap.scheduledendutc <= @endDate )
		OR 
		( ap.createdonutc >= @startDate AND ap.createdonutc <= @endDate)
	)
--	(IsNull(ap.scheduledendutc,ap.createdonutc) >= @startDate 
--         and IsNull(ap.scheduledendutc,ap.createdonutc)  <= @endDate)

)


-- If include notes option is selected
IF( @displayNotes <> 0)
	BEGIN
		INSERT INTO #TempRollupIds (rollupid,type,regardingid,source,ownerid,owneridname,createdon,
		[description],subject,regarding)
		(
		  SELECT fa.annotationid,5,fa.objectid,fa.objecttypecode,
		         fa.ownerid,fa.owneridname,fa.createdon,fa.notetext,fa.subject,
				( CASE WHEN ri.regarding = '' THEN ri.subject ELSE RIGHT( ri.regarding + @constr + ri.subject , @rsize )  END )
		  FROM FilteredAnnotation fa
		  JOIN #TempRollupIds ri
		  ON fa.objectid = ri.rollupid
		  WHERE
		       NOT EXISTS (
			SELECT rollupid FROM #TempRollupIds WHERE fa.annotationid =rollupid 
			)
			AND
			(
			fa.createdon >= @startDate AND fa.createdon <= @endDate
			)
		)
	
	END

	
END

