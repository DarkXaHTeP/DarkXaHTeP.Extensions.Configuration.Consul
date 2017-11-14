echo "Uploading coverage"
IF "%COVERALLS_TOKEN%"=="" echo "Coveralls token variable is NOT defined"
.\coveralls\tools\csmacnz.Coveralls.exe ^
 --opencover -i .\coverage\coverage.xml ^
 --repoToken %COVERALLS_TOKEN% ^
 --commitId $env:APPVEYOR_REPO_COMMIT --commitBranch $env:APPVEYOR_REPO_BRANCH ^
 --commitAuthor $env:APPVEYOR_REPO_COMMIT_AUTHOR --commitEmail $env:APPVEYOR_REPO_COMMIT_AUTHOR_EMAIL ^
  --commitMessage $env:APPVEYOR_REPO_COMMIT_MESSAGE --jobId $env:APPVEYOR_JOB_ID
