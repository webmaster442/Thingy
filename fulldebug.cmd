@echo off
Title Debug app deploy
echo this will copy apps to debug folder
pause

echo deploying apps
xcopy Build-tools\Apps bin\Debug\Apps /s /r /i /y

pause