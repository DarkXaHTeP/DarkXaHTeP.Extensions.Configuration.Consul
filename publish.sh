#!/usr/bin/env bash

dotnet pack ./ConsulConfiguration.Test/ConsulConfiguration.csproj -o ../release

sed -i '' "s%{NUGET_SOURCE}%$NUGET_SOURCE%g" NuGet.config
sed -i '' "s%{NUGET_USERNAME}%$NUGET_USERNAME%g" NuGet.config
sed -i '' "s%{NUGET_API_KEY}%$NUGET_API_KEY%g" NuGet.config


dotnet nuget push ./release/*.nupkg -k $NUGET_API_KEY -s NuGetFeed