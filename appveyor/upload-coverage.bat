echo "Uploading coverage"
IF "%COVERALLS_TOKEN%"=="" echo "Coveralls token variable is NOT defined"
.\coveralls\tools\csmacnz.Coveralls.exe ^
 --opencover -i .\coverage\coverage.xml ^
 --repoToken %COVERALLS_TOKEN% ^
 --commitId %APPVEYOR_REPO_COMMIT% --commitBranch %APPVEYOR_REPO_BRANCH% ^
 --commitAuthor %APPVEYOR_REPO_COMMIT_AUTHOR% --commitEmail %APPVEYOR_REPO_COMMIT_AUTHOR_EMAIL% ^
  --commitMessage %APPVEYOR_REPO_COMMIT_MESSAGE% --jobId %APPVEYOR_JOB_ID%
