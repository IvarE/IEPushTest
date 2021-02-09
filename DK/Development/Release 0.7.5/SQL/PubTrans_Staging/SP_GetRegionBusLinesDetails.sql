USE [PubTrans_Staging]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetRegionBusLinesDetails]    Script Date: 2015-04-24 12:54:27 ******/
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

