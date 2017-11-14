mkdir .\coverage

.\opencover\OpenCover.Console.exe ^
  -target:".\dotnet\dotnet.exe" ^
  -targetargs:"test /p:DebugType=full -f netcoreapp2.0 -c Release ..\ConsulConfiguration.Test\ConsulConfiguration.Test.csproj" ^
  -mergeoutput ^
  -hideskipped:File ^
  -output:coverage/coverage.xml ^
  -oldStyle ^
  -filter:"+[DarkXaHTeP.Extensions.Configuration.Consul*]* -[DarkXaHTeP.Extensions.Configuration.Consul.Test*]*" ^
  -searchdirs:"../ConsulConfiguration.Test/bin/Release/netcoreapp2.0" ^
  -register:user
