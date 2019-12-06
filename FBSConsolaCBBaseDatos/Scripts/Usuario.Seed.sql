
MERGE INTO [Seguridad].[Usuario] AS Target
USING(VALUES
	(N'f0d637ba-06fe-4071-9790-6810f34ef227', N'84c279c0-63d2-45f1-b3e3-3d70522afe43', N'root', N'Administrador del sistema', 
	N'Administrador', N'AQAAAAEAACcQAAAAEHyw03+qxBG7a4x0nGfLt1HyC6Qo8hayVCe68lAQLfSp0JrIBFSlq1pMZf5K1NUM7w==', 
	CAST(N'2019-09-05T10:22:31.0000000' AS DateTime2), N'pepito.png', N'root@gmail.com', N'+53525222222', 0, N'ROOT',
	N'ROOT@GMAIL.COM', 1, 1, N'MWT4AZHLZL2H2H6HLDEOWFCPIFCRJ2BB', 0, 0, NULL, 0, 1, N'ea0dba0e-ef5b-4f83-a34c-869c7fba260a')
) 
AS Source([Id], [OperadoraId], [Codigo], [NombreCompleto], [NombreMostrar], [Contrasenia], [FechaCreacion], [Imagen], 
[CorreoElectronico], [TelefonoCelular], [CambioContrasenia], [CodigoNormalizado], [CorreoElectronicoNormalizado], 
[CorreoElectronicoConfirmado], [TelefonoConfirmado], [MarcaSeguridad], [DobleVerificacion], [AccesosFallidos], [FinBloqueo], 
[BloqueoActivo], [EstaActivo], [Concurrencia])
ON Target.Id = Source.Id
    WHEN MATCHED
    THEN UPDATE SET 
                    [OperadoraId] = Source.[OperadoraId], 
                    [Codigo] = Source.[Codigo],
                    [NombreCompleto] = Source.[NombreCompleto],
                    [NombreMostrar] = Source.[NombreMostrar],
                    [Contrasenia] = Source.[Contrasenia],
                    [FechaCreacion] = Source.[FechaCreacion],
                    [Imagen] = Source.[Imagen],
                    [CorreoElectronico] = Source.[CorreoElectronico],
                    [TelefonoCelular] = Source.[TelefonoCelular],
                    [CambioContrasenia] = Source.[CambioContrasenia],
                    [CodigoNormalizado] = Source.[CodigoNormalizado],
                    [CorreoElectronicoNormalizado] = Source.[CorreoElectronicoNormalizado],
                    [CorreoElectronicoConfirmado] = Source.[CorreoElectronicoConfirmado],
                    [TelefonoConfirmado] = Source.[TelefonoConfirmado],
                    [MarcaSeguridad] = Source.[MarcaSeguridad],
                    [DobleVerificacion] = Source.[DobleVerificacion],
                    [AccesosFallidos] = Source.[AccesosFallidos],
                    [FinBloqueo] = Source.[FinBloqueo],
                    [BloqueoActivo] = Source.[BloqueoActivo],
                    [EstaActivo] = Source.[EstaActivo],
                    [Concurrencia] = Source.[Concurrencia]
    WHEN NOT MATCHED BY TARGET
    THEN
      INSERT ([Id], [OperadoraId], [Codigo], [NombreCompleto], [NombreMostrar], [Contrasenia], [FechaCreacion], [Imagen], 
	  [CorreoElectronico], [TelefonoCelular], [CambioContrasenia], [CodigoNormalizado], [CorreoElectronicoNormalizado],
	  [CorreoElectronicoConfirmado], [TelefonoConfirmado], [MarcaSeguridad], [DobleVerificacion], [AccesosFallidos], [FinBloqueo], 
	  [BloqueoActivo], [EstaActivo], [Concurrencia])
      VALUES ([Id], [OperadoraId], [Codigo], [NombreCompleto], [NombreMostrar], [Contrasenia], [FechaCreacion], [Imagen], 
	  [CorreoElectronico], [TelefonoCelular], [CambioContrasenia], [CodigoNormalizado], [CorreoElectronicoNormalizado], 
	  [CorreoElectronicoConfirmado], [TelefonoConfirmado], [MarcaSeguridad], [DobleVerificacion], [AccesosFallidos], [FinBloqueo],
	  [BloqueoActivo], [EstaActivo], [Concurrencia])
    WHEN NOT MATCHED BY SOURCE
    THEN DELETE;


