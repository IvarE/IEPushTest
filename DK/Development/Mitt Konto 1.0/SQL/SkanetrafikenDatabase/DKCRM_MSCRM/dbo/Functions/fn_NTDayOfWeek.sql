

create function dbo.fn_NTDayOfWeek(@Date datetime)
returns int
as
begin
	return (DATEPART(dw, @Date) + @@DATEFIRST -1) % 7
	
end