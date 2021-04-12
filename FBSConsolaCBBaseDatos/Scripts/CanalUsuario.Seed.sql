
MERGE INTO [Seguridad].[CanalUsuario] AS Target
USING(VALUES
	(N'83572964-7a43-4c67-0058-08d73de1465d', N'f0d637ba-06fe-4071-9790-6810f34ef227', N'f0d637ba-06fe-4071-9790-6810f34ef227', 1)
) 
AS Source([Id], [CanalId], [UsuarioId], [EstaActivo])
ON Target.Id = Source.Id
    WHEN MATCHED
    THEN UPDATE SET 
                    [CanalId] = Source.[CanalId], 
                    [UsuarioId] = Source.[UsuarioId],
                    [EstaActivo] = Source.[EstaActivo]
    WHEN NOT MATCHED BY TARGET
    THEN
      INSERT ([Id], [CanalId], [UsuarioId], [EstaActivo])
      VALUES ([Id], [CanalId], [UsuarioId], [EstaActivo])
    WHEN NOT MATCHED BY SOURCE
    THEN DELETE;


