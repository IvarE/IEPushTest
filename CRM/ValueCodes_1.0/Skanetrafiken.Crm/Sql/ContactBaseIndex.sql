CREATE NONCLUSTERED INDEX IX_ContactBase_Composite1 ON ContactBase
(
	ContactId ASC,
	st_SSN_2 ASC,
	cgi_socialsecuritynumber ASC,
	ed_MklId ASC,
	LastName ASC,
	Telephone2 ASC
)
INCLUDE (FirstName,
	FullName,
	EMailAddress1,
	EMailAddress2,
	Telephone1,
	cgi_ContactNumber)


create index ed_contactid_include_fullname on contactbase (contactid) include(fullname, YomiFullName)
