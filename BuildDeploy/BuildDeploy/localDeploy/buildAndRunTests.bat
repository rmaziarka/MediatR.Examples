@pushd %~dp0
@powershell -Command "&{..\build.ps1 -Tasks Build-Local,Run-Tests}"
@pause