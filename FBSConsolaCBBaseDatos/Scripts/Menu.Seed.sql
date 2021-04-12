
MERGE INTO [Seguridad].[Menu] AS Target
USING(VALUES
	(N'f0d637ba-06fe-4071-9790-6810f34ef227', NULL, N'Administración', 1, N'3d_rotation', N'', 1),
	(N'f0d637ba-06fe-4071-9790-6810f34ef228', NULL, N'Consola', 1, N'account_balance', N'sst', 1),
	(N'361c7810-659b-4750-d15e-08d754ceb622', N'f0d637ba-06fe-4071-9790-6810f34ef227', N'Mantenimientos', 1, N'fa fa-list', N'admin/mantenimientos', 1),
	(N'd8786dc8-8d89-408a-d15f-08d754ceb622', N'f0d637ba-06fe-4071-9790-6810f34ef227', N'Dispositivos', 4, N'fa fa-mobile', N'admin/dispositivos', 1),
	(N'e8432df4-0084-49f0-d160-08d754ceb622', N'f0d637ba-06fe-4071-9790-6810f34ef228', N'Agente', 1, N'fa fa-street-view', N'agentes', 1),
	(N'6f992c56-e65c-4e6e-d161-08d754ceb622', N'f0d637ba-06fe-4071-9790-6810f34ef228', N'Agente Consola', 2, N'fa  fa-vcard', N'consola', 1),
	(N'8a53faa7-3b44-4655-d162-08d754ceb622', N'f0d637ba-06fe-4071-9790-6810f34ef228', N'Agente Activación', 2, N'fa  fa-check', N'agente-activacion', 1),
	(N'b125056c-1681-4b49-d163-08d754ceb622', N'f0d637ba-06fe-4071-9790-6810f34ef228', N'Alertas', 4, N'fa fa-bell', N'alertas', 1),
	(N'0a6d79dd-21a4-438e-cded-08d76eae6d8d', N'f0d637ba-06fe-4071-9790-6810f34ef227', N'Canales', 6, N'fa fa-sellsy', N'admin/canales', 1),
	(N'f0d637ba-06fe-4071-9790-6810f34ef257', N'f0d637ba-06fe-4071-9790-6810f34ef227', N'Usuario', 4, N'fa fa-users', N'admin/usuarios', 1),
	(N'f0d637ba-06fe-4071-9790-6810f34ef327', N'f0d637ba-06fe-4071-9790-6810f34ef227', N'Roles', 2, N'fa fa-user-circle', N'admin/rol', 1),
	(N'f0d637ba-06fe-4071-9790-6810f34ef347', N'f0d637ba-06fe-4071-9790-6810f34ef227', N'Menu', 3, N'fa fa-list', N'admin/menu', 1)
) 
AS Source([Id], [MenuPadreId], [Nombre], [Orden], [Icono], [Ruta], [EstaActivo])
ON Target.Id = Source.Id
    WHEN MATCHED
    THEN UPDATE SET 
                    [MenuPadreId] = Source.[MenuPadreId], 
                    [Nombre] = Source.[Nombre],
                    [Orden] = Source.[Orden],
                    [Icono] = Source.[Icono],
                    [Ruta] = Source.[Ruta],
                    [EstaActivo] = Source.[EstaActivo]
    WHEN NOT MATCHED BY TARGET
    THEN
      INSERT ([Id], [MenuPadreId], [Nombre], [Orden], [Icono], [Ruta], [EstaActivo])
      VALUES ([Id], [MenuPadreId], [Nombre], [Orden], [Icono], [Ruta], [EstaActivo])
    WHEN NOT MATCHED BY SOURCE
    THEN DELETE;


