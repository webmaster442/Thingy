@echo off
Title Thingy compiler script

"c:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\msbuild.exe" /m Thingy.sln /p:Configuration=Release
echo compile done

echo deploying apps
xcopy Build-tools\Apps bin\Release\Apps /s /r /i /y

cd Setup
echo running setup
"c:\Program Files (x86)\Inno Setup 5\ISCC.exe" setup.iss