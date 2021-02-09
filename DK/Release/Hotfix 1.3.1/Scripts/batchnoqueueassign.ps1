[void][System.Reflection.Assembly]::LoadFile("D:\CGI\SDK_2013\microsoft.xrm.sdk.dll")
[void][System.Reflection.Assembly]::LoadFile("D:\CGI\SDK_2013\microsoft.xrm.Client.dll")
[void][System.Reflection.Assembly]::LoadFile("D:\CGI\SDK_2013\microsoft.crm.sdk.proxy.dll")
[void][System.Reflection.Assembly]::LoadWithPartialName("system.servicemodel")

#$crmServiceUrl = "https://sekunduat.skanetrafiken.se/DKCRMUAT/XRMServices/2011/Organization.svc"
$crmServiceUrl = "https://sekund.skanetrafiken.se/DKCRM/XRMServices/2011/Organization.svc"
$clientCredentials = new-object System.ServiceModel.Description.ClientCredentials
$clientCredentials.UserName.UserName = 'd1\crmadmin'
$clientCredentials.UserName.Password = 'uSEme2!nstal1'
$service = new-object Microsoft.Xrm.Sdk.Client.OrganizationServiceProxy($crmServiceUrl, $null, $clientCredentials, $null)
$service.Timeout = new-object System.Timespan(0, 10, 0)

$userlist = "D1\cth";

$query = New-Object -TypeName Microsoft.Xrm.Sdk.Query.QueryExpression -ArgumentList "systemuser";
$query.ColumnSet.AddColumn("systemuserid");
$query.ColumnSet.AddColumn("fullname");
$query.ColumnSet.AddColumn("domainname");
$query.ColumnSet.AddColumn("cgi_noqueueassign");
$query.Criteria.AddCondition("domainname", [Microsoft.Xrm.Sdk.Query.ConditionOperator]::In, $userlist);
$results = $service.RetrieveMultiple($query);
$contacts = $results.Entities;

#$contacts

$objs = $null

foreach ($contact in $contacts)
{
    Write-Host $contact.Id "-" $contact.Attributes["fullname"] "-" $contact.Attributes["domainname"] $contact.Attributes["cgi_noqueueassign"]

    $contact.Attributes["cgi_noqueueassign"] = "1"
    $service.Update($contact);
}

