$client = New-Object System.Net.WebClient

Write-Output "Downloading dotnet SDK..."
$client.DownloadFile("https://download.microsoft.com/download/7/3/A/73A3E4DC-F019-47D1-9951-0453676E059B/dotnet-sdk-2.0.2-win-x64.zip", ".\dotnet.zip")
Write-Output "Expanding into .\dotnet folder..."
Expand-Archive .\dotnet.zip -DestinationPath .\dotnet

Write-Output "Downloading opencover..."
$client.DownloadFile("https://github.com/OpenCover/opencover/releases/download/4.6.519/opencover.4.6.519.zip", ".\opencover.zip")
Write-Output "Expanding into .\opencover folder..."
Expand-Archive .\opencover.zip -DestinationPath .\opencover
