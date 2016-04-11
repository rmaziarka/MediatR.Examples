BEGIN
EXEC sp_addsrvrolemember @loginame= '$(Username)', @rolename = '$(Role)';
END