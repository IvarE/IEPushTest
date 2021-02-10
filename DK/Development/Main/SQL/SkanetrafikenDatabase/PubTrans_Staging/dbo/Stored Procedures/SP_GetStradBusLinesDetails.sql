





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
		PurposeOfGroupingCode = 'TRAFFIC' --'PRODUCT'
	)

--select * from #StradBus

Delete LD From LineDetails LD where LD.CreatedOn=Convert(varchar(10),GetDate(),110) AND LD.LineType='STRADBUS'
       
Insert into LineDetails(LineDetails,LineType,CreatedOn)

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
				--And
				----(convert(datetime, convert(varchar(10), D.ExistsFromDate, 102))>=convert(datetime,convert(varchar(10), GetDate(), 102))or D.ExistsUptoDate is null)
				----And
				----(convert(datetime, convert(varchar(10), D.ExistsFromDate, 102))<=convert(datetime,convert(varchar(10), GetDate(), 102)))
				--(convert(datetime, convert(varchar(10), D.ExistsFromDate, 102)) <= convert(datetime,'06-12-2014') or D.ExistsUptoDate is null)
				--And
				--(convert(datetime, convert(varchar(10), D.ExistsFromDate, 102)) <= convert(datetime,'06-12-2014'))
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
		--And
			----(convert(datetime, convert(varchar(10), C.ExistsFromDate, 102))>=convert(datetime,convert(varchar(10), GetDate(), 102))or C.ExistsUptoDate is null)
			----And
			----(convert(datetime, convert(varchar(10), C.ExistsFromDate, 102))<=convert(datetime,convert(varchar(10), GetDate(), 102)))
			--(convert(datetime, convert(varchar(10), C.ExistsFromDate, 102)) <= convert(datetime,'06-12-2014') or C.ExistsUptoDate is null)
			--	And
			--(convert(datetime, convert(varchar(10), C.ExistsFromDate, 102)) <= convert(datetime,'06-12-2014'))
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
			--And
			--(B.ExistsUptoDate>=Convert(varchar(10),GETDATE(),110) or B.ExistsUptoDate is null)
			----(convert(datetime, convert(varchar(10), B.ExistsFromDate, 102))>=convert(datetime,convert(varchar(10), GetDate(), 102))or B.ExistsUptoDate is null)
			----And
			----(convert(datetime, convert(varchar(10), B.ExistsFromDate, 102))<=convert(datetime,convert(varchar(10), GetDate(), 102)))
			--(convert(datetime, convert(varchar(10), B.ExistsFromDate, 102)) <= convert(datetime,'06-12-2014') or B.ExistsUptoDate is null)
			--	And
			--(convert(datetime, convert(varchar(10), B.ExistsFromDate, 102)) <= convert(datetime,'06-12-2014'))
		)		
	order by ZoneName
	for xml path ('Zone'), root('Zones'), TYPE 
	)
 as LineDetails,
'STRADBUS' as LineType,
Convert(varchar(10),GetDate(),110) as CreatedOn

END

--Exec GetRegionBusLinesDetails
--drop PROCEDURE  GetRegionBusLinesDetails

--select * from LineDetails







