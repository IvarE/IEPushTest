USE [PubTrans_Staging]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetContractorDetail]    Script Date: 2015-01-30 14:53:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Suman S
-- Create date: 02-Sep-2014
-- Description:	This procedure will return the contractor organisational detail
-- based on the contractor Gid supplied.
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetContractorDetail] 
		@ContractorId varchar(max) 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
SET NOCOUNT ON;

select 
	B.Gid as ContractorGid,
	A.id as ContractorId,
	A.Name as ContractorName,
	A.Code as ContractorCode
from 
	OrganisationalUnit A
	join Contractor B on A.id = B.IsOrganisationId
where 
	B.Gid = @ContractorId
FOR XML PATH('ContactorDetail'), TYPE
END



GO

/****** Object:  StoredProcedure [dbo].[SP_GetContractors]    Script Date: 2015-01-30 14:53:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Reine Rosqvist
-- Create date: 2015-01-14
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetContractors]
	
AS
BEGIN
	SET NOCOUNT ON;

	select 
		c.Id,
		c.Gid,
		c.IsOrganisationId
	from Contractor c
	order by c.Id
	for xml path ('Contractor'), root('Contractors'), TYPE

END

GO

/****** Object:  StoredProcedure [dbo].[SP_GetLineDetails]    Script Date: 2015-01-30 14:53:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Suman Subramanian
-- Create date: 06-29-2014
-- Description: Procedure for getting the line Details.
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetLineDetails] @LineType varchar(50) 
	
AS
BEGIN
	
	SET NOCOUNT ON;
	
	select 
		LD.LineDetails LineDetails 
	from 
		LineDetails LD
	Where
		LD.LineType=@LineType
	And
		LD.CreatedOn = CONVERT(varchar(10),GETDATE(),110)
   
END

GO

/****** Object:  StoredProcedure [dbo].[SP_GetOrganisationalUnits]    Script Date: 2015-01-30 14:53:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetOrganisationalUnits]
	
AS
BEGIN
	SET NOCOUNT ON;

    select
		o.Id,
		o.Code,
		o.Name
	from OrganisationalUnit o
	order by o.Id
	for xml path ('OrganisationalUnit'), root('OrganisationalUnits'), TYPE

END

GO

/****** Object:  StoredProcedure [dbo].[SP_GetRegionBusLinesDetails]    Script Date: 2015-01-30 14:53:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO







-- =============================================
-- Author:           Suman Subramanian
-- Create date:		 2014-02-17
-- Description:      Procedure for getting Region Bus Line Details
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetRegionBusLinesDetails]
AS
BEGIN

IF OBJECT_ID('tempdb..#RegionBus') IS NOT NULL
    DROP TABLE #RegionBus

select	 	
	distinct(A.Gid) LineGid,
	A.Name LineName,
	A.Number LineNumber,
	A.Designation LineDesignation,
	A.DisplayOrder LineDisplayOrder,
	A.TransportAuthorityCode LineTransportAuthorityCode		
	into #RegionBus
	from 
		LineInZone A		
	where 
	A.gid in
	(
		select 
			B.IsLineGid
		from 
			[dbo].[LineInGroupOfLines] B
			Left  join 
			LineInZone A on A.gid=b.IsLineGid
		where 
			IsInGroupOfLineId in
			(
				select 
				id 
				from 
				GroupOfLines 
				where 
				code not in ('SB','TÅG','NB')
				and 
				PurposeOfGroupingCode='PRODUCT'
			)
	)
	And
	----(convert(datetime, convert(varchar(10), A.ExistsFromDate, 102))>=convert(datetime,convert(varchar(10), GetDate(), 102))or A.ExistsUptoDate is null)
	----And
	----(convert(datetime, convert(varchar(10), A.ExistsFromDate, 102))<=convert(datetime,convert(varchar(10), GetDate(), 102)))
	(convert(datetime, convert(varchar(10), A.ExistsFromDate, 102)) <= convert(datetime,'06-12-2014') or A.ExistsUptoDate is null)
		And
	(convert(datetime, convert(varchar(10), A.ExistsFromDate, 102)) <= convert(datetime,'06-12-2014'))
	order by A.Designation

	
	
-- Delete the Existing Record on the current Date  
Delete LD From LineDetails LD where LD.CreatedOn=Convert(varchar(10),GetDate(),110) AND LD.LineType='REGIONBUS'

-- set statistics time on
set nocount on                    
Insert into LineDetails(LineDetails,LineType)

Select 
( 
	select	 	
	A.LineGid LineGid,
	A.LineName LineName,
	A.LineNumber LineNumber,
	A.LineDesignation LineDesignation,
	A.LineDisplayOrder LineDisplayOrder,
	A.LineTransportAuthorityCode LineTransportAuthorityCode,	
	(
		Select
			Distinct(B.StopAreaGid) StopAreaGid,
			B.StopAreaName StopAreaName,
			B.StopAreaShortName StopAreaShortName,
			B.ExistsFromDate StopExistsFromDate,
			B.ExistsUptoDate StopExistsUptoDate
		From
			StopOnLine B
		where
			B.IsOnLineGid = A.LineGid
		And
		   (B.ExistsUptoDate>Convert(varchar(10),GETDATE(),110) or B.ExistsUptoDate is null)
		for xml path ('StopArea'), root('StopAreas'), TYPE 
	)	
	from 
		#RegionBus A	
	
	for xml path ('Line'), root('Lines'), TYPE 
) as LineDetails,
'REGIONBUS' as LineType

END

--Exec GetRegionBusLinesDetails
--drop PROCEDURE  GetRegionBusLinesDetails




GO

/****** Object:  StoredProcedure [dbo].[SP_GetStradBusLinesDetails]    Script Date: 2015-01-30 14:53:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






-- Query For SB


-- =============================================
-- Author:           Suman Subramanian
-- Create date:		 2014-02-17
-- Description:      Procedure for getting StradBus Line Details
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetStradBusLinesDetails]
AS
BEGIN
       
       
-- set statistics time on
set nocount on             

IF OBJECT_ID('tempdb..#StradBus') IS NOT NULL
    DROP TABLE #StradBus  

select 
	IsLineGid LineGid
into 
	#StradBus
from 
	LineInGroupOfLines  
where 
	IsInGroupOfLineId in
	(
		select 
		id 
		from 
		GroupOfLines 
		where 
		code ='SB'
		and 
		PurposeOfGroupingCode='PRODUCT'
	)

--select * from #StradBus

Delete LD From LineDetails LD where LD.CreatedOn=Convert(varchar(10),GetDate(),110) AND LD.LineType='STRADBUS'
       
Insert into LineDetails(LineDetails,LineType)

Select 
(
select 
	A.Id ZoneId,
	A.Gid ZoneGid,
	A.Number ZoneNumber,
	A.Name ZoneName,
	A.ShortName ZoneShortName,
	A.TypeCode ZoneType,
	A.TransportAuthorityCode ZoneTransportAuthorityCode,
	A.ExistsFromDate ZoneExistsFromDate,
	(
		select 
			C.Gid LineGid,
			C.Name LineName,
			C.Number LineNumber,
			C.Designation LineDesignation,
			C.DisplayOrder LineDisplayOrder,
			C.TransportAuthorityCode LineTransportAuthorityCode,
			C.OperatorCode LineOperatorCode,
			C.ExistsFromDate LineExistsFromDate,
			C.ExistsUptoDate LineExistsUpToDate,	
			(
				select 	
					distinct(D.stopareagid) StopAreaGid,	
					D.stopareaname StopAreaName,
					D.StopAreaShortName StopAreaShortName,
					D.ExistsFromDate StopExistsFromDate,
					D.ExistsUptoDate StopExistsUptoDate		
				from 
					StopOnLine D
				Where
					D.IsOnLineGid=C.Gid
				And
				----(convert(datetime, convert(varchar(10), D.ExistsFromDate, 102))>=convert(datetime,convert(varchar(10), GetDate(), 102))or D.ExistsUptoDate is null)
				----And
				----(convert(datetime, convert(varchar(10), D.ExistsFromDate, 102))<=convert(datetime,convert(varchar(10), GetDate(), 102)))
				(convert(datetime, convert(varchar(10), D.ExistsFromDate, 102)) <= convert(datetime,'06-12-2014') or D.ExistsUptoDate is null)
				And
				(convert(datetime, convert(varchar(10), D.ExistsFromDate, 102)) <= convert(datetime,'06-12-2014'))
				order by StopAreaName
				for xml path ('StopArea'), root('StopAreas'), TYPE 
			)
		from
			LineInZone C 
		where 
			C.Gid in 
			(
				select 
					LineGid
				from 
					#StradBus
			)
		And
			C.OperatesZoneId =A.Id
		And
			----(convert(datetime, convert(varchar(10), C.ExistsFromDate, 102))>=convert(datetime,convert(varchar(10), GetDate(), 102))or C.ExistsUptoDate is null)
			----And
			----(convert(datetime, convert(varchar(10), C.ExistsFromDate, 102))<=convert(datetime,convert(varchar(10), GetDate(), 102)))
			(convert(datetime, convert(varchar(10), C.ExistsFromDate, 102)) <= convert(datetime,'06-12-2014') or C.ExistsUptoDate is null)
				And
			(convert(datetime, convert(varchar(10), C.ExistsFromDate, 102)) <= convert(datetime,'06-12-2014'))
		for xml path ('Line'), root('Lines'), TYPE 
	)	
from 
	Zone A	
	where
		A.ID in 
		(
		 Select 
			B.OperatesZoneId  
		 from
			LineInZone B
		 Where			
			B.Gid in 
			(
				select 
					LineGid
				from 
					#StradBus
			)
			And
			--(B.ExistsUptoDate>=Convert(varchar(10),GETDATE(),110) or B.ExistsUptoDate is null)
			----(convert(datetime, convert(varchar(10), B.ExistsFromDate, 102))>=convert(datetime,convert(varchar(10), GetDate(), 102))or B.ExistsUptoDate is null)
			----And
			----(convert(datetime, convert(varchar(10), B.ExistsFromDate, 102))<=convert(datetime,convert(varchar(10), GetDate(), 102)))
			(convert(datetime, convert(varchar(10), B.ExistsFromDate, 102)) <= convert(datetime,'06-12-2014') or B.ExistsUptoDate is null)
				And
			(convert(datetime, convert(varchar(10), B.ExistsFromDate, 102)) <= convert(datetime,'06-12-2014'))
		)		
	order by ZoneName
	for xml path ('Zone'), root('Zones'), TYPE 
	)
 as LineDetails,
'STRADBUS' as LineType

END

--Exec GetRegionBusLinesDetails
--drop PROCEDURE  GetRegionBusLinesDetails

--select * from LineDetails







GO

/****** Object:  StoredProcedure [dbo].[SP_GetTrainLinesDetails]    Script Date: 2015-01-30 14:53:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





-- =============================================
-- Author:           Suman Subramanian
-- Create date:		 2014-02-17
-- Description:      Procedure for getting Train Line Details
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetTrainLinesDetails]
AS
BEGIN
       
-- set statistics time on
set nocount on   

IF OBJECT_ID('tempdb..#TempLinesWithStops') IS NOT NULL
    DROP TABLE #TempLinesWithStops  
	
select distinct(A.StopAreaGid),
	A.StopAreaName,
	A.StopAreaShortName,
	A.ExistsFromDate,
	A.ExistsUptoDate
	into #TempLinesWithStops
	from 
	stoponline A 
	join 
	line B 
	on A.IsOnLineGid = B.Gid
	where B.Gid in 
	(
		select 
			IsLineGid 
		from 
			[dbo].[LineInGroupOfLines] 
		where 
			IsInGroupOfLineId in
			(
				select 
				id 
				from 
				GroupOfLines 
				where 
				code ='TÅG'
				and 
				PurposeOfGroupingCode='PRODUCT'
			)
	)
	and 
	--(B.ExistsUptoDate!=Convert(varchar(10),GETDATE(),110) or B.ExistsUptoDate is null)
	----(convert(datetime, convert(varchar(10), B.ExistsFromDate, 102))>=convert(datetime,convert(varchar(10), GetDate(), 102))or B.ExistsUptoDate is null)
	----And
	----(convert(datetime, convert(varchar(10), B.ExistsFromDate, 102))<=convert(datetime,convert(varchar(10), GetDate(), 102)))
	(convert(datetime, convert(varchar(10), B.ExistsFromDate, 102)) <= convert(datetime,'06-12-2014') or B.ExistsUptoDate is null)
		And
	(convert(datetime, convert(varchar(10), B.ExistsFromDate, 102)) <= convert(datetime,'06-12-2014'))
	order by StopAreaName

--Delete the exisitng record on the current date
Delete LD From LineDetails LD where LD.CreatedOn=Convert(varchar(10),GetDate(),110) AND LD.LineType='TRAIN'


Insert into LineDetails(LineDetails,LineType)
Select
(
	
		select 
		A.StopAreaGid ,
		A.StopAreaName,
		A.StopAreaShortName,
		A.ExistsFromDate,
		A.ExistsUptoDate,
		(
			select 	
				Distinct(B.StopAreaGid),
				B.StopAreaName,
				B.StopAreaShortName,	
				B.ExistsFromDate,
				B.ExistsUptoDate				
			from 
				StopOnLine B 
			where 
			B.IsOnLineGid in
			(
				select 
					C.IsOnLineGid 
				from 
					stoponline C 
					join 
					line D 
					on 
					C.IsOnLineGid = D.Gid
				where 
					C.StopAreaGid =A.StopAreaGid 
				and 
					D.Gid in
					(
						select 
							E.IsLineGid 
						from 
							LineInGroupOfLines E
						where 
							E.IsInGroupOfLineId in
							(
								select 
									F.id 
								from 
									GroupOfLines F
								where 
									F.code ='TÅG'
								and 
									F.PurposeOfGroupingCode='PRODUCT'
							)
					)
				
			) 
			and B.StopAreaGid !=A.StopAreaGid
			order by B.StopAreaName
			for xml path ('StopArea'), root('UptoStopAreas'), TYPE 
		)
		from #TempLinesWithStops A
		for xml path ('StopArea'), root('StopAreas'), TYPE 
	
) as LineDetails,
'TRAIN' as LineType

END







GO


