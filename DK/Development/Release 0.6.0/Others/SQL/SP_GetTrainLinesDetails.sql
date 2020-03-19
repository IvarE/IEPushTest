USE [PubTrans_Staging]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetTrainLinesDetails]    Script Date: 2014-11-27 15:57:43 ******/
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

