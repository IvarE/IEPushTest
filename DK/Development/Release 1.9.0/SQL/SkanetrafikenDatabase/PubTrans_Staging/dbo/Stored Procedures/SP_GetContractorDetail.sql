


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



