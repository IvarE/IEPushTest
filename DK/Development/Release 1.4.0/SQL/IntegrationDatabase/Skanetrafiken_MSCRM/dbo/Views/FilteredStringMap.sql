

create view dbo.FilteredStringMap(
	FilteredViewName,
	AttributeName,
	AttributeValue,
	Value,
	DisplayOrder,
	LangId
)
as
select
	e.ReportViewName,
	sm.AttributeName,
	sm.AttributeValue,
	sm.Value,
	sm.DisplayOrder,
	sm.LangId
from
	StringMap sm
	join EntityView e on (e.ObjectTypeCode = sm.ObjectTypeCode)