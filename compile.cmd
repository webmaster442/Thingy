@echo off
Title Thingy compiler script
call :compile
call :deploy
goto exit

:compile
"c:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\msbuild.exe" /m Thingy.sln /p:Configuration=Release
echo compile done

:deploy
xcopy deploy_source\ffmpeg.exe bin\release\Native\x64\ffmpeg.exe
xcopy deploy_source\mpv.exe bin\release\Native\x64\mpv.exe
xcopy deploy_source\youtube-dl.exe bin\release\Native\x64\youtube-dl.exe

:exit
pause