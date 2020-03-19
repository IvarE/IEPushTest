


create function dbo.fn_EndOfTomorrow(
  @TodayUTC	datetime
)
returns datetime
as
begin
  return dbo.fn_BeginOfDay(dateadd(dd, 2, @TodayUTC))
end
