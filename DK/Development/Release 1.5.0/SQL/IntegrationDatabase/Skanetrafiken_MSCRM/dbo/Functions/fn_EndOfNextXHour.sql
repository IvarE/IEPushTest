

create function dbo.fn_EndOfNextXHour ( 
  @TodayUTC     datetime,
  @X		int
)
returns datetime
as
begin
  return dbo.fn_BeginOfHour(dateadd(hh, @X+1, @TodayUTC))
end
