set dst="C:\bin\syncfs\syncfs1.0.0"
echo %dst%

mkdir %dst%

del /Q /S %dst%

xcopy "C:\prj\syncfs\bin" %dst% /e /y /v /exclude:exclude.txt

pause