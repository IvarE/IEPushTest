param (
	[string]
	$environment
)

function Get-ScriptDirectory
{
	if ($script:MyInvocation.MyCommand.Path) { Split-Path $script:MyInvocation.MyCommand.Path } else { $pwd }
}

$scriptPath = Get-ScriptDirectory
& $scriptPath\ImportAlmModule.ps1

$crmenvs = @(
	[PSCustomObject]@{ Environment = "UTV"; Url = "http://v-dkcrm-utv/DKCRM" },
	[PSCustomObject]@{ Environment = "TEST"; Url = "http://v-dkcrm-tst/DKCRM" },
	[PSCustomObject]@{ Environment = "UAT"; Url = "https://sekunduat.skanetrafiken.se/DKCRM" },
	[PSCustomObject]@{ Environment = "PROD"; Url = "https://sekund.skanetrafiken.se/DKCRM" }
)

if (-not $environment)
{
    $crmenvs | Format-Table

    Write-Host "Usage: " $script:MyInvocation.MyCommand.Name "-environment Environment"
}
else 
{

    $ImportSolutions = @(
        #"$scriptPath\Customizations\clickdimensions.zip",
		#"$scriptPath\Customizations\EndeavorForSkanetrafiken_$Env:BUILD_BUILDNUMBER.zip",
        #"$scriptPath\Customizations\ST_External_List_$Env:BUILD_BUILDNUMBER.zip",
		#"$scriptPath\Customizations\Skanetrafiken_$Env:BUILD_BUILDNUMBER.zip",
        #"$scriptPath\Customizations\SkanetrafikenSilverlight_$Env:BUILD_BUILDNUMBER.zip",
        #"$scriptPath\Customizations\SkanetrafikenPlugins_$Env:BUILD_BUILDNUMBER.zip"
		"$scriptPath\Customizations\Skanetrafiken_170_$Env:BUILD_BUILDNUMBER.zip"
		#,"$scriptPath\Customizations\Skanetrafiken_180_$Env:BUILD_BUILDNUMBER.zip"
		
    )
    #"$scriptPath\Customizations\EndeavorForSkanetrafiken.zip"

    $crmenv = $crmenvs.Where{ $_.Environment -like $environment}

    Try 
	{
        $credential = New-Object System.Management.Automation.PSCredential -ArgumentList "D1\CrmAdmin", (ConvertTo-SecureString -String “uSEme2!nstal1” -AsPlainText -Force)

        $srcconn = Get-CrmConnection -Url "http://v-dkcrm-utv/DKCRM" -Credential $credential
        $dstconn = Get-CrmConnection -Url $crmenv.Url -Credential $credential

        Write-Host "Exporting solutions:"
		#Never export ClickDimensions it is a vendor supplied package
		#$res = Export-CrmSolution -Connection $srcconn -SolutionName "clickdimensions" -OutputPath "$scriptPath\Customizations\clickdimensions_$Env:BUILD_BUILDNUMBER.zip" -Managed -TargetVersion "6.1.0.0" -Verbose

        #$res = Export-CrmSolution -Connection $srcconn -SolutionName "Skanetrafiken" -OutputPath "$scriptPath\Customizations\Skanetrafiken_$Env:BUILD_BUILDNUMBER.zip" -Managed -TargetVersion "6.1.0.0" -Verbose

        #$res = Export-CrmSolution -Connection $srcconn -SolutionName "EndeavorForSkanetrafiken" -OutputPath "$scriptPath\Customizations\EndeavorForSkanetrafiken_$Env:BUILD_BUILDNUMBER.zip" -Managed -TargetVersion "6.1.0.0" -Verbose

        #$res = Export-CrmSolution -Connection $srcconn -SolutionName "SkanetrafikenSilverlight" -OutputPath "$scriptPath\Customizations\SkanetrafikenSilverlight_$Env:BUILD_BUILDNUMBER.zip" -Managed -TargetVersion "6.1.0.0" -Verbose

        #$res = Export-CrmSolution -Connection $srcconn -SolutionName "SkanetrafikenPlugins" -OutputPath "$scriptPath\Customizations\SkanetrafikenPlugins_$Env:BUILD_BUILDNUMBER.zip" -Managed -TargetVersion "6.1.0.0" -Verbose

		#$res = Export-CrmSolution -Connection $srcconn -SolutionName "ST_External_List" -OutputPath "$scriptPath\Customizations\ST_External_List_$Env:BUILD_BUILDNUMBER.zip" -TargetVersion "6.1.0.0" -Verbose

		$res = Export-CrmSolution -Connection $srcconn -SolutionName "Skanetrafiken_170" -OutputPath "$scriptPath\Customizations\Skanetrafiken_170_$Env:BUILD_BUILDNUMBER.zip" -TargetVersion "6.1.0.0" -Verbose
		
		#$res = Export-CrmSolution -Connection $srcconn -SolutionName "Skanetrafiken_180" -OutputPath "$scriptPath\Customizations\Skanetrafiken_180_$Env:BUILD_BUILDNUMBER.zip" -TargetVersion "6.1.0.0" -Verbose

        Write-Host "Importing solution:"
        Import-CrmSolution -Connection $dstconn -CustomizationPath $ImportSolutions -OverwriteUnmanagedCustomizations -PublishWorkflows -Verbose
    }
    Catch
    {
        $errmsg = "Error: " + $_.Exception.Message # $_.Exception.ItemName " Message: "
        Write-Error -Message $errmsg
        Break
    }
}