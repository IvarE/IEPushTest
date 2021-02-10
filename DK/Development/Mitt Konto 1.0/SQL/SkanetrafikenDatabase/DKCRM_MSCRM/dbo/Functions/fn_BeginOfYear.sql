

create function dbo.fn_BeginOfYear ( 
  @DayUTC         datetime
)
returns datetime
as
begin
  declare @DayUserLocal datetime
  declare @DayFirst datetime
  set @DayUserLocal = dbo.fn_UTCToLocalTime(@DayUTC)
  set @DayFirst = dbo.fn_FirstDayOfMonth(@DayUserLocal, 1)  
  return dbo.fn_LocalTimeToUTC(@DayFirst)
end
