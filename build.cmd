@echo off
Title Thingy compiler script

"c:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\msbuild.exe" /m Thingy.sln /p:Configuration=Release
echo compile done

xcopy Build-tools\Apps bin\Release\Apps /s /r /i /y
echo deploying apps

cd Setup
"c:\Program Files (x86)\Inno Setup 5\ISCC.exe" setup.iss