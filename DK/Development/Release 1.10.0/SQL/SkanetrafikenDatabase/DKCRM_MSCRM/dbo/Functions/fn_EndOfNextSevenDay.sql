


create function dbo.fn_EndOfNextSevenDay(
  @TodayUTC	datetime
)
returns datetime
as
begin
  return dbo.fn_BeginOfDay(dateadd(dd, 8, @TodayUTC))
end
