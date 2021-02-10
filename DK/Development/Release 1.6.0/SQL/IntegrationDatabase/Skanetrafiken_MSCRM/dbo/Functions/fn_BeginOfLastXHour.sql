

create function dbo.fn_BeginOfLastXHour ( 
  @TodayUTC     datetime,
  @X		int
)
returns datetime
as
begin
  return dbo.fn_BeginOfHour(dateadd(hh, -@X, @TodayUTC))
end
