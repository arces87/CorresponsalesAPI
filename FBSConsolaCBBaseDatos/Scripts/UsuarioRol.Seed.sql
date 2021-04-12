
MERGE INTO [Seguridad].[UsuarioRol] AS Target
USING(VALUES
	(N'f0d637ba-06fe-4071-9790-6810f34ef227', N'f0d637ba-06fe-4071-9790-6810f34ef227')
) 
AS Source([UsuarioId], [RolId])
ON Target.[UsuarioId] = Source.[UsuarioId] and Target.[RolId] = Source.[RolId] 
    WHEN MATCHED
    THEN UPDATE SET 
                    [RolId] = Source.[RolId], 
                    [UsuarioId] = Source.[UsuarioId]
    WHEN NOT MATCHED BY TARGET
    THEN
      INSERT ([UsuarioId], [RolId])
      VALUES ([UsuarioId], [RolId])
    WHEN NOT MATCHED BY SOURCE
    THEN DELETE;


