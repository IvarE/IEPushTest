

create function dbo.fn_EndOfToday(
  @TodayUTC	datetime
)
returns datetime
as
begin
  return dbo.fn_BeginOfDay(dateadd(dd, 1, @TodayUTC))
end
