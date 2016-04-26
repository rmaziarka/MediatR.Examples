@pushd %~dp0
@powershell -Command "&{.\Deploy.ps1 -dropexistingdatabase}"
@pause