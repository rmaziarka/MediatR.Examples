@echo off
SETLOCAL ENABLEDELAYEDEXPANSION
@echo off

:: set the working dir (default to current dir)
set wdir=%cd%
if not (%1)==() set wdir=%1

:: transform all the templates
TextTransform.exe -out "../app/common/models/enums/enums.ts" %wdir%"/enumTypeItems.tt"

echo transformation complete