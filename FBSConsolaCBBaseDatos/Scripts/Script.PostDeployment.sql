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

:r .\TipoCatalogo.Seed.sql
:r .\Catalogo.Seed.sql
:r .\Menu.Seed.sql
:r .\Rol.Seed.sql
:r .\RolMenu.Seed.sql
:r .\Usuario.Seed.sql
:r .\Canal.Seed.sql
:r .\CanalUsuario.Seed.sql
:r .\UsuarioRol.Seed.sql
