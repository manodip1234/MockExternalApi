param(
  [string]$BaseUrl = "http://localhost:3001",
  [string]$OutRoot = (Join-Path $PSScriptRoot "..\\Datasets"),
  [string]$AppSettingsPath = (Join-Path $PSScriptRoot "..\\appsettings.json")
)

$ErrorActionPreference = "Stop"

function Ensure-Directory([string]$Path) {
  if (-not (Test-Path -LiteralPath $Path)) {
    New-Item -ItemType Directory -Path $Path | Out-Null
  }
}

function Pretty-Json([string]$Json) {
  try {
    return ($Json | ConvertFrom-Json -Depth 100 | ConvertTo-Json -Depth 100)
  } catch {
    return $Json
  }
}

function Get-HmacSignature([string]$Secret, [string]$PathAndQuery, [string]$Timestamp) {
  $raw = "GET|$PathAndQuery|$Timestamp|"
  $keyBytes = [System.Text.Encoding]::UTF8.GetBytes($Secret)
  $msgBytes = [System.Text.Encoding]::UTF8.GetBytes($raw)
  $hmac = [System.Security.Cryptography.HMACSHA256]::new($keyBytes)
  try {
    $hashBytes = $hmac.ComputeHash($msgBytes)
  } finally {
    $hmac.Dispose()
  }
  return [Convert]::ToBase64String($hashBytes)
}

function Save-Endpoint([string]$Url, [string]$OutFile, [string]$ApiKey, [string]$HmacSecret) {
  Write-Host "GET $Url -> $OutFile"
  $u = [System.Uri]::new($Url)
  $pathAndQuery = $u.PathAndQuery
  $timestamp = [DateTime]::UtcNow.ToString("o")
  $sig = Get-HmacSignature -Secret $HmacSecret -PathAndQuery $pathAndQuery -Timestamp $timestamp

  $headers = @{
    "X-API-KEY"        = $ApiKey
    "X-TIMESTAMP"      = $timestamp
    "X-HMAC-SIGNATURE" = $sig
  }

  $raw = Invoke-RestMethod -Method Get -Uri $Url -Headers $headers | ConvertTo-Json -Depth 100
  Ensure-Directory (Split-Path -Parent $OutFile)
  (Pretty-Json $raw) | Set-Content -LiteralPath $OutFile -Encoding UTF8
}

Ensure-Directory $OutRoot

$currentDir = Join-Path $OutRoot "current"
$archiveDir = Join-Path $OutRoot "archive"
Ensure-Directory $archiveDir

if (Test-Path -LiteralPath $currentDir) {
  $stamp = Get-Date -Format "yyyyMMdd_HHmmss"
  $dest = Join-Path $archiveDir $stamp
  Write-Host "Archiving existing current dataset -> $dest"
  Move-Item -LiteralPath $currentDir -Destination $dest
}

Ensure-Directory $currentDir

$pageAll = "page=1&page_size=5000"

$cfg = Get-Content -LiteralPath $AppSettingsPath -Raw | ConvertFrom-Json
$keys = @{
  bcw   = @{ key = $cfg.BcwApi.ApiKey;  secret = $cfg.BcwApi.HmacSecret }
  food  = @{ key = $cfg.FoodApi.ApiKey; secret = $cfg.FoodApi.HmacSecret }
  trans = @{ key = $cfg.TransApi.ApiKey; secret = $cfg.TransApi.HmacSecret }
  wrd   = @{ key = $cfg.HmacAuth.ApiKey; secret = $cfg.HmacAuth.Secret }
}

$endpoints = @(
  @{ dept = "bcw";  name = "offices";           path = "/bcw/offices?$pageAll" },
  @{ dept = "bcw";  name = "services";          path = "/bcw/services?$pageAll" },
  @{ dept = "bcw";  name = "officers";          path = "/bcw/officers?$pageAll" },
  @{ dept = "bcw";  name = "acknowledgements";  path = "/bcw/acknowledgements?$pageAll" },

  @{ dept = "food"; name = "offices";           path = "/food/offices?$pageAll" },
  @{ dept = "food"; name = "services";          path = "/food/services?$pageAll" },
  @{ dept = "food"; name = "officers";          path = "/food/officers?$pageAll" },
  @{ dept = "food"; name = "acknowledgements";  path = "/food/acknowledgements?$pageAll" },

  @{ dept = "trans"; name = "offices";          path = "/trans/office?$pageAll" },
  @{ dept = "trans"; name = "services";         path = "/trans/service?$pageAll" },
  @{ dept = "trans"; name = "officers";         path = "/trans/officer?$pageAll" },
  @{ dept = "trans"; name = "ack";              path = "/trans/ack?$pageAll" },

  @{ dept = "wrd"; name = "offices";            path = "/wrd/office?$pageAll" },
  @{ dept = "wrd"; name = "services";           path = "/wrd/service?$pageAll" },
  @{ dept = "wrd"; name = "officers";           path = "/wrd/user?$pageAll" },
  @{ dept = "wrd"; name = "ack";                path = "/wrd/ack?$pageAll" }
)

foreach ($e in $endpoints) {
  $url = ($BaseUrl.TrimEnd("/") + $e.path)
  $out = Join-Path $currentDir (Join-Path $e.dept ($e.name + ".json"))
  $k = $keys[$e.dept]
  Save-Endpoint -Url $url -OutFile $out -ApiKey $k.key -HmacSecret $k.secret
}

Write-Host "Done. New datasets written to: $currentDir"
