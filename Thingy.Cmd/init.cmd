@echo off
title Thingy Shell
rem ------------------------------------------------------------
rem Thingy.cmd init script
rem ------------------------------------------------------------

set current=%cd%
set apps=%current%\Native\x64
SET PATH=%PATH%;%current%;%apps%

doskey ipy=Thingy.Cmd.exe ipy %*
doskey brainfuck=Thingy.Cmd.exe brainfuck %*
doskey drvlist=wmic logicaldisk get deviceid, volumename, description
doskey cd=PUSHD $*
doskey edit=notepad2 $*

PROMPT $P$_$G

cls
Thingy.Cmd.exe

echo new commands: ipy brainfuck drvlist