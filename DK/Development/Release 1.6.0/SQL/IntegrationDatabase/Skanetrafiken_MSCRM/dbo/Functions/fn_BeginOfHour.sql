

create function dbo.fn_BeginOfHour ( 
  @DayUTC         datetime
)
returns datetime
as
begin
  declare @DayBeginUTC datetime
  set @DayBeginUTC = convert(datetime, convert(nvarchar, @DayUTC, 112))
  return dateadd(hh, datepart(hh, @DayUTC), @DayBeginUTC)
end
