
MERGE INTO [Nomenclador].[Catalogo] AS Target
USING(VALUES
	(N'd822d8ed-f6ce-40ad-b32b-08d7564c0cd1', N'84c279c0-63d2-45f1-b3e3-3d70522afe43', N'Movistar', N'Movistar', 1),
	(N'723215c3-b9ab-46a3-5561-08d75d69c940', N'84c279c0-63d2-45f1-b3e3-3d70522afe72', N'A001', N'El agente no se encuentra en el sistema.', 1),
	(N'1d465356-395f-4ce2-5562-08d75d69c940', N'84c279c0-63d2-45f1-b3e3-3d70522afe72', N'A002', N'Los datos del dispositivos no son correctos.', 1),
	(N'f7dffb8d-06fc-4b90-5563-08d75d69c940', N'84c279c0-63d2-45f1-b3e3-3d70522afe72', N'A003', N'Error en los datos de autenticación del usuario.', 1),
	(N'0fb3a7dc-8cf5-448a-5564-08d75d69c940', N'84c279c0-63d2-45f1-b3e3-3d70522afe72', N'A004', N'El agente no se encuentra activo.', 1),
	(N'41c16e7b-17be-4f16-5565-08d75d69c940', N'84c279c0-63d2-45f1-b3e3-3d70522afe72', N'A005', N'El agente no tiene datos de geolocalización registrados.', 1),
	(N'448f0230-c90c-4095-5566-08d75d69c940', N'84c279c0-63d2-45f1-b3e3-3d70522afe72', N'A006', N'El agente esta intentando acceder de una geolocalización no autorizada.', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe42', N'84c279c0-63d2-45f1-b3e3-3d70522afe43', N'Digitel', N'Digitel', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe43', N'84c279c0-63d2-45f1-b3e3-3d70522afe43', N'Movitel', N'Movitel', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe54', N'84c279c0-63d2-45f1-b3e3-3d70522afe53', N'Iphone', N'Iphone', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe56', N'84c279c0-63d2-45f1-b3e3-3d70522afe53', N'Samsung', N'Samsung', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe64', N'84c279c0-63d2-45f1-b3e3-3d70522afe63', N'Android', N'Android', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe65', N'84c279c0-63d2-45f1-b3e3-3d70522afe63', N'IOS', N'IOS', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe66', N'84c279c0-63d2-45f1-b3e3-3d70522afe63', N'Windows', N'Windows', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe67', N'84c279c0-63d2-45f1-b3e3-3d70522afe63', N'Ubuntu', N'Ubuntu', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe68', N'84c279c0-63d2-45f1-b3e3-3d70522afe64', N'Registrado', N'Registrado', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe69', N'84c279c0-63d2-45f1-b3e3-3d70522afe64', N'Activo', N'Activo', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe70', N'84c279c0-63d2-45f1-b3e3-3d70522afe64', N'Ubicado', N'Ubicado', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe71', N'84c279c0-63d2-45f1-b3e3-3d70522afe65', N'Cobro de servicios', N'Cobro de servicios', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe72', N'84c279c0-63d2-45f1-b3e3-3d70522afe65', N'Depósito', N'Depósito', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe73', N'84c279c0-63d2-45f1-b3e3-3d70522afe65', N'Retiro', N'Retiro', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe74', N'84c279c0-63d2-45f1-b3e3-3d70522afe71', N'Autenticación', N'Autenticación', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe75', N'84c279c0-63d2-45f1-b3e3-3d70522afe71', N'Activación', N'Activación', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe76', N'84c279c0-63d2-45f1-b3e3-3d70522afe66', N'Solicitado', N'Solicitado', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe77', N'84c279c0-63d2-45f1-b3e3-3d70522afe66', N'Enviado', N'Enviado', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe78', N'84c279c0-63d2-45f1-b3e3-3d70522afe66', N'Recibido', N'Recibido', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe79', N'84c279c0-63d2-45f1-b3e3-3d70522afe66', N'Terminado', N'Terminado', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe80', N'84c279c0-63d2-45f1-b3e3-3d70522afe71', N'Solicitar Otp', N'Solicitar Otp', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe81', N'84c279c0-63d2-45f1-b3e3-3d70522afe71', N'Verificar Otp', N'Verificar OTP', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe82', N'84c279c0-63d2-45f1-b3e3-3d70522afe71', N'Crear Cuenta', N'Crear Cuenta', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe83', N'84c279c0-63d2-45f1-b3e3-3d70522afe67', N'Nueva', N'Nueva', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe84', N'84c279c0-63d2-45f1-b3e3-3d70522afe67', N'Recibida', N'Recibida', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe85', N'84c279c0-63d2-45f1-b3e3-3d70522afe67', N'Resulta', N'Resuelta', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe86', N'84c279c0-63d2-45f1-b3e3-3d70522afe71', N'Crear Cliente', N'Crear Cliente', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe87', N'84c279c0-63d2-45f1-b3e3-3d70522afe64', N'Cobrando', N'Cobrando', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe88', N'84c279c0-63d2-45f1-b3e3-3d70522afe71', N'Cerrar Día', N'Cerrar Día', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe89', N'84c279c0-63d2-45f1-b3e3-3d70522afe71', N'Abrir Día', N'Abrir Día', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe90', N'84c279c0-63d2-45f1-b3e3-3d70522afe68', N'Cédula', N'Cédula', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe91', N'84c279c0-63d2-45f1-b3e3-3d70522afe68', N'RUC', N'RUC', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe92', N'84c279c0-63d2-45f1-b3e3-3d70522afe68', N'Pasaporte', N'Pasaporte', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe93', N'84c279c0-63d2-45f1-b3e3-3d70522afe69', N'Recibida', N'Recibida', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe94', N'84c279c0-63d2-45f1-b3e3-3d70522afe69', N'Procesada', N'Procesada', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe95', N'84c279c0-63d2-45f1-b3e3-3d70522afe70', N'Falta de Fondos', N'Falta de Fondos', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe96', N'84c279c0-63d2-45f1-b3e3-3d70522afe70', N'Falta de Papel', N'Falta de Papel', 1),
	(N'84c279c0-63d2-45f1-b3e3-3d70522afe97', N'84c279c0-63d2-45f1-b3e3-3d70522afe64', N'Inactivo', N'Agente Inactivo', 1)
) 
AS Source([Id], [TipoCatalogoId], [Nombre], [Descripcion], [EstaActivo])
ON Target.Id = Source.Id
    WHEN MATCHED
    THEN UPDATE SET 
                    [Nombre] = Source.[Nombre], 
                    [TipoCatalogoId] = Source.[TipoCatalogoId], 
                    [Descripcion] = Source.[Descripcion],
                    [EstaActivo] = Source.[EstaActivo]
    WHEN NOT MATCHED BY TARGET
    THEN
      INSERT ([Id], [TipoCatalogoId], [Nombre], [Descripcion], [EstaActivo])
      VALUES ([Id], [TipoCatalogoId], [Nombre], [Descripcion], [EstaActivo])
    WHEN NOT MATCHED BY SOURCE
    THEN DELETE;


