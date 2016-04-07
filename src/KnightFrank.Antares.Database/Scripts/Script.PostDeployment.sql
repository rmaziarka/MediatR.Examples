/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

--:r .\Locale.sql
--:r .\Country.sql
--:r .\Department.sql
--:r .\Business.sql
:r .\Role.sql
--:r .\EnumType.sql
--:r .\EnumTypeItem.sql
--:r .\PropertyType.sql
--:r .\PropertyTypeDefinition.sql