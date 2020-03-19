/*

Sökbara kolumner för Case (IncidentBase) i Quick Find-vyn per 2017-05-23 18:00:

Action Date and Time: cgi_actiondate
casdet_row1_cat3: cgi_casdet_row1_cat3id
Case Number: ticketnumber
Case Title: title
Customer_Email: cgi_customer_email
Customer_Telephonenumber: cgi_customer_telephonenumber
Customer_Telephonenumber_Mobile: cgi_customer_telephonenumber_mobile
Customer_Telephonenumber_work: cgi_customer_telephonenumber_work
RGOL ärendeid: cgi_rgolissueid
rgol_fullname: cgi_rgol_fullname
soc_sec_number: cgi_soc_sec_number

*/

create nonclustered index IX_IncidentBase_cgi_actiondate on IncidentBase (cgi_actiondate)
create nonclustered index IX_IncidentBase_cgi_casdet_row1cat3 on IncidentBase (cgi_casdet_row1_cat3id)
create nonclustered index ix_IncidentBase_cgi_cust_email on incidentbase (cgi_customer_email)
create nonclustered index ix_IncidentBase_cgi_cust_phone on incidentbase (cgi_customer_telephonenumber)
create nonclustered index ix_IncidentBase_cgi_cust_phone_mobile on incidentbase (cgi_customer_telephonenumber_mobile)
create nonclustered index ix_IncidentBase_cgi_cust_phone_work on incidentbase (cgi_customer_telephonenumber_work)
create nonclustered index ix_IncidentBase_cgi_RGOLIssueId on incidentbase (cgi_RGOLIssueId)
create nonclustered index ix_IncidentBase_cgi_rgol_fname on incidentbase (cgi_rgol_fullname)
create nonclustered index ix_IncidentBase_cgi_soc_sec_number on incidentbase (cgi_soc_sec_number)
