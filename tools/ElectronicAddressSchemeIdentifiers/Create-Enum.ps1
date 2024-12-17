function ConvertTo-UpperCamelCase
{
    param([string]$text)

    # Split text into words and clean
    $words = $text -split '\s+'

    # Capitalize first letter of each word
    $camelCase = foreach ($word in $words)
    {
        if ($word)
        {
            $word.Substring(0, 1).ToUpper() + $word.Substring(1).ToLower()
        }
    }

    return [string]::Join("", $camelCase)
}

function Get-CleanSlug
{
    param([string]$text)
    # Convert German umlauts to ASCII representation
    $cleaned = $text -replace 'ä', 'ae' `
        -replace 'ö', 'oe' `
        -replace 'ü', 'ue' `
        -replace 'Ä', 'Ae' `
        -replace 'Ö', 'Oe' `
        -replace 'Ü', 'Ue' `
        -replace 'ß', 'ss'

    # Remove special characters but keep spaces for word boundaries
    # Also handle special case for VAT abbreviation
    $cleaned = $cleaned -replace '[^a-zA-Z0-9\s]', ' ' `
        -replace '\s+', ' '`
        -replace 'VAT', 'Vat'

    # Convert to UpperCamelCase
    $cleaned = ConvertTo-UpperCamelCase -text $cleaned
    return $cleaned
}

function ConvertFrom-HtmlEntities
{
    param([string]$text)

    # Replace HTML entities with their corresponding characters
    $text = $text -replace '&quot;', '"' `
        -replace '&amp;', '&' `
        -replace '&lt;', '<' `
        -replace '&gt;', '>' `
        -replace '&apos;', "'" `
        -replace '&#39;', "'" `
        -replace '&#34;', '"'`
        -replace '\s+', ' '

    return $text
}

function Get-PeppolEasItems
{
    [CmdletBinding()]
    [OutputType([System.Collections.Generic.List[PSObject]])]
    param(
        [Parameter(Mandatory = $true)]
        [string]$HtmlFilePath
    )

    try
    {
        # Load HTML file
        $html = Get-Content -Path $HtmlFilePath -Raw

        # Parse HTML using regex pattern to extract code, summary
        $pattern = '<div id="(\d+)"[^>]*>.*?<code>(\d+)</code>.*?<strong>(.*?)</strong>'
        $matches = [regex]::Matches($html, $pattern, [System.Text.RegularExpressions.RegexOptions]::Singleline)

        # Create result list
        $results = New-Object System.Collections.Generic.List[PSObject]

        foreach ($match in $matches)
        {
            $codeInt = [int]$match.Groups[1].Value
            [string]$code = '{0:D4}' -f ($codeInt)

            # Check for manual override entries
            $override = Get-ManualItemOverride -code $code
            if ($override)
            {
                $results.Add($override)
                continue
            }

            $summary = $match.Groups[3].Value.Trim()
            $summary = ConvertFrom-HtmlEntities -text $summary

            # Generate slug: clean special characters and format
            $slug = Get-CleanSlug -text $summary

            # Create and add object to results
            $results.Add([PSCustomObject]@{
                    code    = $code
                    summary = $summary
                    slug    = $slug
                })
        }

        # Add manual items
        $manualItems = Get-ManualItems
        foreach ($manualItem in $manualItems)
        {
            $results.Add($manualItem)
        }

        # Order results by code
        $results = $results | Sort-Object -Property code
        return $results
    }
    catch
    {
        Write-Error "Error processing HTML file: $_"
        return $null
    }
}

function Get-ManualItems
{
    [OutputType([System.Collections.Generic.List[PSObject]])]
    $result = New-Object System.Collections.Generic.List[PSObject]
    $result.Add([PSCustomObject]@{
        code    = "9955"
        summary = "Swedish VAT number"
        slug    = "SwedishVatNumber"
    })
    $result.Add([PSCustomObject]@{
        code    = "9956"
        summary = "Belgian Crossroad Bank of Enterprises"
        slug    = "BelgianCrossroad"
    })
    $result.Add([PSCustomObject]@{
        code    = "9958"
        summary = "German Leitweg ID"
        slug    = "GermanLeitwegID"
    })

    return $result
}
function Get-ManualItemOverride
{
    [OutputType([PSObject])]
    param (
        [string]$code,
        [string]$slug
    )

    $manualItems = New-Object System.Collections.Generic.List[PSObject]
    $manualItems.Add([PSCustomObject]@{
            code    = "0088"
            summary = "EAN Location Code"
            slug    = "EanLocationCode"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "0204"
            summary = "Leitweg-ID"
            slug    = "LeitwegID"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9910"
            summary = "Hungary VAT number"
            slug    = "HungaryVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9914"
            summary = "Austria VAT number"
            slug    = "AustriaVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9922"
            summary = "Andorra VAT number"
            slug    = "AndorraVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9923"
            summary = "Albania VAT number"
            slug    = "AlbaniaVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9924"
            summary = "Bosnia and Herzegovina VAT number"
            slug    = "BosniaAndHerzegovinaVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9925"
            summary = "Belgium VAT number"
            slug    = "BelgiumVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9926"
            summary = "Bulgaria VAT number"
            slug    = "BulgariaVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9927"
            summary = "Switzerland VAT number"
            slug    = "SwitzerlandVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9928"
            summary = "Cyprus VAT number"
            slug    = "CyprusVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9929"
            summary = "Czech Republic VAT number"
            slug    = "CzechRepublicVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9930"
            summary = "Germany VAT number"
            slug    = "GermanyVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9931"
            summary = "Estonia VAT number"
            slug    = "EstoniaVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9932"
            summary = "United Kingdom VAT number"
            slug    = "UnitedKingdomVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9933"
            summary = "Greece VAT number"
            slug    = "GreeceVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9934"
            summary = "Croatia VAT number"
            slug    = "CroatiaVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9935"
            summary = "Ireland VAT number"
            slug    = "IrelandVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9936"
            summary = "Liechtenstein VAT number"
            slug    = "LiechtensteinVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9937"
            summary = "Lithuania VAT number"
            slug    = "LithuaniaVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9938"
            summary = "Luxemburg VAT number"
            slug    = "LuxemburgVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9939"
            summary = "Latvia VAT number"
            slug    = "LatviaVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9940"
            summary = "Monaco VAT number"
            slug    = "MonacoVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9941"
            summary = "Montenegro VAT number"
            slug    = "MontenegroVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9942"
            summary = "Macedonia, of the former Yugoslav Republic VAT number"
            slug    = "MacedoniaVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9943"
            summary = "Malta VAT number"
            slug    = "MaltaVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9944"
            summary = "Netherlands VAT number"
            slug    = "NetherlandsVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9945"
            summary = "Poland VAT number"
            slug    = "PolandVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9946"
            summary = "Portugal VAT number"
            slug    = "PortugalVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9947"
            summary = "Romania VAT number"
            slug    = "RomaniaVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9948"
            summary = "Serbia VAT number"
            slug    = "SerbiaVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9949"
            summary = "Slovenia VAT number"
            slug    = "SloveniaVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9950"
            summary = "Slovakia VAT number"
            slug    = "SlovakiaVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9951"
            summary = "San Marino VAT number"
            slug    = "SanMarinoVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9952"
            summary = "Turkey VAT number"
            slug    = "TurkeyVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9953"
            summary = "Holy See (Vatican City State) VAT number"
            slug    = "HolySeeVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9955"
            summary = "Swedish VAT number"
            slug    = "SwedishVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9956"
            summary = "Belgian Crossroad Bank of Enterprises"
            slug    = "BelgianCrossroad"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9957"
            summary = "French VAT number"
            slug    = "FrenchVatNumber"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9958"
            summary = "German Leitweg ID"
            slug    = "GermanLeitwegID"
        })
    $manualItems.Add([PSCustomObject]@{
            code    = "9959"
            summary = "Employer Identification Number (EIN, USA)"
            slug    = "EmployerIdentificationNumber"
        })

    return $manualItems | Where-Object { $_.code -eq $code }
}

function Get-PeppolEasCodelist
{
    [CmdletBinding()]
    [OutputType([string])]
    param()

    try
    {
        # Create temporary file for download
        $tempFile = [System.IO.Path]::GetTempFileName()
        $tempFileHtml = [System.IO.Path]::ChangeExtension($tempFile, "html")
        Rename-Item -Path $tempFile -NewName $tempFileHtml

        # Download PEPPOL EAS codelist
        $url = "https://docs.peppol.eu/poacc/billing/3.0/codelist/eas/"
        Invoke-WebRequest -Uri $url -OutFile $tempFileHtml

        Write-Verbose "Downloaded to: $tempFileHtml"
        return $tempFileHtml
    }
    catch
    {
        Write-Error "Failed to download PEPPOL EAS codelist: $_"
        return $null
    }
}

function Create-EnumFile
{
    param (
        [Parameter(Mandatory = $true)]
        [string]$FileName,

        [Parameter(Mandatory = $true)]
        [array]$Items,

        [Parameter(Mandatory = $false)]
        [string]$Namespace = "s2industries.ZUGFeRD"
    )

    # Create C# enum file content
    $enumContent = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace $Namespace
{
    /// <summary>
    /// For a reference see:
    /// https://docs.peppol.eu/poacc/billing/3.0/codelist/eas/
    /// </summary>
    public enum ElectronicAddressSchemeIdentifiers
    {
"@

    # Add enum entries
    foreach ($item in $Items)
    {
        $enumContent += @"

        /// <summary>
        /// $($item.summary)
        /// </summary>
        $($item.slug) = $($item.code),

"@
    }
    $enumContent += @"
    }
}
"@
    # Save enum content to file
    Set-Content -Path $FileName -Value $enumContent -Encoding UTF8

    Write-Output "Enum file created: $FileName"
}

# Main execution
$tempFilePath = Get-PeppolEasCodelist
if ($tempFilePath)
{
    try
    {
        $items = Get-PeppolEasItems -HtmlFilePath $tempFilePath
        $items | Format-Table -AutoSize
        Create-EnumFile -FileName "ElectronicAddressSchemeIdentifiers.cs" -Items $items -Namespace "s2industries.ZUGFeRD"
    }
    finally
    {
        # Cleanup temporary files
        if (Test-Path $tempFilePath)
        {
            Remove-Item -Path $tempFilePath -Force
        }
    }
}
else
{
    Write-Error "Failed to download PEPPOL EAS codelist"
}