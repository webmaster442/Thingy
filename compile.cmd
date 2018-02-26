@echo off
Title Thingy compiler script
call :compile
goto exit

:compile
"c:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\msbuild.exe" /m Thingy.sln /p:Configuration=Release
echo compile done

:exit