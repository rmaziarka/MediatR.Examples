@pushd %~dp0
@powershell -Command "&{.\Deploy.ps1 -DropExistingDatabase}"
@pause