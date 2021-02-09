


create function dbo.fn_BeginOfLastSevenDay(
  @TodayUTC	datetime
)
returns datetime
as
begin
  return dbo.fn_BeginOfDay(dateadd(dd, -7, @TodayUTC))
end
