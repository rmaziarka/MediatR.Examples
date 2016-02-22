@pushd %~dp0
@powershell -Command "&{..\build.ps1 -Tasks Build-Local,Run-Tests,Build-Database,Build-Frontend}"
@powershell -File ..\deploy.ps1 -Environment Local
@pause