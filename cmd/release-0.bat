set dst="C:\bin\syncfs\0"
echo %dst%

xcopy "C:\prj\syncfs\bin" %dst% /e /y /exclude:exclude.txt

pause