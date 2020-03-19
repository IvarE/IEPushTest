CREATE INDEX IX_CustomerAddressBase_Line2_City ON CustomerAddressBase (Line2, City) 
INCLUDE (ParentId, PostalCode, AddressNumber, ObjectTypeCode)