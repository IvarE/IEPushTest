

declare @CardNumber as bigint
, @CardSection as smallint

-- scramle data 200 records
declare cursor_rev cursor for
SELECT TOP 200 
      [CardNumber]
      ,[CardSection]
  FROM [CrmTravelCardsDW].[dbo].[GetCardView]

open cursor_rev;
fetch next from cursor_rev into @CardNumber, @CardSection;


while @@fetch_status = 0
begin
	update [CrmTravelCardsDW].[dbo].[GetCardView]
	   set BlockDescription = 'Blocked On.:'+Cast(getdate() as varchar(12))
	where [CardNumber] = @CardNumber
	  and [CardSection] = @CardSection


fetch next from cursor_rev into @CardNumber, @CardSection;
end

close cursor_rev
deallocate cursor_rev
