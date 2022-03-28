
MERGE INTO [Seguridad].[Rol] AS Target
USING(VALUES
	(N'c3399529-96f3-4014-94ce-bf36ee695546', N'Supervisor', N'SUPERVISOR', N'Supervisor de Agente', 1, 1, N'960794ea-a481-49c2-a772-1707577288ba'),
	(N'e0aee3b1-57c9-4d4e-8f35-4bf3bc38236e', N'Agente', N'AGENTE', N'Agente', 1, 1, N'4ac7fbe2-1037-48a0-a093-4d8d9dd88cff'),
	(N'f0d637ba-06fe-4071-9790-6810f34ef227', N'Administrador', N'ADMINISTRADOR', N'Administrador', 1, 1, N'b751f5cf-a408-41ea-ac65-bc1ac15c5abb')
) 
AS Source([Id], [Nombre], [NombreNormalizado], [Descripcion], [Tipo], [EstaActivo], [Concurrencia])
ON Target.Id = Source.Id
    WHEN MATCHED
    THEN UPDATE SET 
                    [Nombre] = Source.[Nombre], 
                    [NombreNormalizado] = Source.[NombreNormalizado],
                    [Descripcion] = Source.[Descripcion],
                    [Tipo] = Source.[Tipo],
                    [EstaActivo] = Source.[EstaActivo],
                    [Concurrencia] = Source.[Concurrencia]
    WHEN NOT MATCHED BY TARGET
    THEN
      INSERT ([Id], [Nombre], [NombreNormalizado], [Descripcion], [Tipo], [EstaActivo], [Concurrencia])
      VALUES ([Id], [Nombre], [NombreNormalizado], [Descripcion], [Tipo], [EstaActivo], [Concurrencia])
    WHEN NOT MATCHED BY SOURCE
    THEN DELETE;


