

CREATE  FUNCTION [dbo].[fn_RptBracket]
   (@MyDiff int, @NDays int )
RETURNS nvarchar(10)
AS
BEGIN
   if(@MyDiff >= 5*@NDays)
   begin
	RETURN ( Cast(5 * @NDays as nvarchar(5)) + N'+')
   end

   RETURN ( Cast(Floor(@MyDiff / @NDays) * @NDays as nvarchar(5)) + N' - ' + Cast(Floor(@MyDiff / @NDays + 1) * @NDays - 1 as nvarchar(5)))
END