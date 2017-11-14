echo "Uploading coverage"
IF "%COVERALLS_TOKEN%"=="" echo "Coveralls token variable is NOT defined"
.\coveralls\tools\csmacnz.Coveralls.exe ^
 --opencover -i .\coverage\coverage.xml ^
 --repoToken %COVERALLS_TOKEN% ^
 --useRelativePaths
