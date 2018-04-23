@echo off
Title Thingy compiler script
call :compile
call :apps
call :setup
goto exit

:compile
"c:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\msbuild.exe" /m Thingy.sln /p:Configuration=Release
echo compile done

:apps
xcopy Build-tools\Apps bin\Release\Apps /s /r /i /y
echo deploying apps

:setup
cd Setup
"c:\Program Files (x86)\Inno Setup 5\ISCC.exe" setup.iss

:exit