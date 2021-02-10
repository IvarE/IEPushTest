
-- =============================================
-- Author:		Per Lindblom
-- Create date: 2012-08-10
-- Description:	Adds 1 to the last customernumber and returns the new value
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetNextCustomerNumber]
	@NextCustomerNumber as int output,
	@CounterName as nvarchar(50)
AS
BEGIN
	 begin transaction
 
            
 
 
		    update [Counter] with(rowlock)
            set @NextCustomerNumber = LastValue + 1, LastValue = @NextCustomerNumber 
			where CounterName = @CounterName
            commit transaction
END

GO