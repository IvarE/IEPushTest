

create function dbo.fn_BeginOfDay ( 
  @DayUTC         datetime
)
returns datetime
as
begin
  declare @DayUserLocal datetime
  declare @BDay datetime
  declare @BDayUTC datetime
  set @DayUserLocal = dbo.fn_UTCToLocalTime(@DayUTC)
  set @BDay = convert(datetime, convert(nvarchar, @DayUserLocal, 112))
  set @BDayUTC = dbo.fn_LocalTimeToUTC(@BDay) 
  return @BDayUTC
end
