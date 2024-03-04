set dst="C:\bin\SyncFavor\0"
echo %dst%

xcopy "C:\prj\SyncFavor\bin" %dst% /e /y /exclude:exclude.txt

pause