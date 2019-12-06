
MERGE INTO [Nomenclador].[TipoCatalogo] AS Target
USING(VALUES
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe43', N'Operadoras', N'Operadoreas de Telecomunicaciones', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe53', N'Marca Dispositivos', N'Marcas de Dispositivos', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe63', N'Sistema Operativo', N'Sistemas Operativos', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe64', N'Estado de Agente', N'Estados de los Agentes', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe65', N'Tipos de Transacción', N'Tipos de transacciones', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe66', N'Estados de los Log', N'Estados de los Logs', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe67', N'Estado de las Alertas', N'Estados de las Alertas', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe68', N'Tipos de Identificación', N'Tipos de Identificación', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe69', N'Estados Transacciones', N'Estados Transacciones', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe70', N'Tipos de Alerta', N'Tipos de Alerta', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe71', N'Tipos de Operaciones', N'Tipos de Operaciones', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe72', N'Mensajeria de Error Api Móvil', N'Mensajeria de Error en el API Móvil', 1)
) 
AS Source([Id], [Nombre], [Descripcion], [EstaActivo])
ON Target.Id = Source.Id
    WHEN MATCHED
    THEN UPDATE SET 
                    [Nombre] = Source.[Nombre], 
                    [Descripcion] = Source.[Descripcion],
                    [EstaActivo] = Source.[EstaActivo]
    WHEN NOT MATCHED BY TARGET
    THEN
      INSERT ([Id], [Nombre], [Descripcion], [EstaActivo])
      VALUES ([Id], [Nombre], [Descripcion], [EstaActivo])
    WHEN NOT MATCHED BY SOURCE
    THEN DELETE;


