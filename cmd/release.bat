set dst="C:\bin\SyncFavor\sync2.0.0"
echo %dst%

mkdir %dst%

del /Q /S %dst%

xcopy "C:\prj\SyncFavor\bin" %dst% /e /y /v /exclude:exclude.txt

pause