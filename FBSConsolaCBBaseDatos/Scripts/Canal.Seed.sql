
MERGE INTO [Seguridad].[Canal] AS Target
USING(VALUES
	(N'f0d637ba-06fe-4071-9790-6810f34ef227', N'Corresponsales', N'{"Parametrizaciones":[{"Llave":"AgenteIdEstadoRegistrado","Valor":"84C279C0-63D2-45F1-B3E3-3D70522AFE68"},{"Llave":"AgenteIdEstadoUbicado","Valor":"84C279C0-63D2-45F1-B3E3-3D70522AFE70"},{"Llave":"AgenteIdEstadoActivo","Valor":"84C279C0-63D2-45F1-B3E3-3D70522AFE69"},{"Llave":"AgenteIdEstadoCobrando","Valor":"84c279c0-63d2-45f1-b3e3-3d70522afe87"},{"Llave":"AgenteIdEstadoEliminado","Valor":"84C279C0-63D2-45F1-B3E3-3D70522AFE97"},{"Llave":"IdRolSupervisor","Valor":"c3399529-96f3-4014-94ce-bf36ee695546"},{"Llave":"IdRolAgente","Valor":"e0aee3b1-57c9-4d4e-8f35-4bf3bc38236e"},{"Llave":"IdDeposito","Valor":"84C279C0-63D2-45F1-B3E3-3D70522AFE72"},{"Llave":"IdRetiro","Valor":"84C279C0-63D2-45F1-B3E3-3D70522AFE73"},{"Llave":"IdCobroServicio","Valor":"84C279C0-63D2-45F1-B3E3-3D70522AFE71"},{"Llave":"IdAutenticacion","Valor":"84C279C0-63D2-45F1-B3E3-3D70522AFE74"},{"Llave":"IdActivacion","Valor":"84C279C0-63D2-45F1-B3E3-3D70522AFE75"},{"Llave":"IdLogSolicitado","Valor":"84C279C0-63D2-45F1-B3E3-3D70522AFE76"},{"Llave":"IdLogEnviado","Valor":"84C279C0-63D2-45F1-B3E3-3D70522AFE77"},{"Llave":"IdLogRecibido","Valor":"84C279C0-63D2-45F1-B3E3-3D70522AFE78"},{"Llave":"IdLogTerminado","Valor":"84C279C0-63D2-45F1-B3E3-3D70522AFE79"},{"Llave":"IdSolicitarOtp","Valor":"84C279C0-63D2-45F1-B3E3-3D70522AFE80"},{"Llave":"IdVerificarOtp","Valor":"84C279C0-63D2-45F1-B3E3-3D70522AFE81"},{"Llave":"UrlFinancial","Valor":"http://186.5.29.68:9001"},{"Llave":"IdCrearCuenta","Valor":"84c279c0-63d2-45f1-b3e3-3d70522afe82"},{"Llave":"IdAlertaNueva","Valor":"84c279c0-63d2-45f1-b3e3-3d70522afe83"},{"Llave":"IdCrearCliente","Valor":"84c279c0-63d2-45f1-b3e3-3d70522afe86"},{"Llave":"IdCerrarDia","Valor":"84c279c0-63d2-45f1-b3e3-3d70522afe88"},{"Llave":"IdAbrirDia","Valor":"84c279c0-63d2-45f1-b3e3-3d70522afe89"},{"Llave":"IdTipoIdentificacion","Valor":"84c279c0-63d2-45f1-b3e3-3d70522afe68"},{"Llave":"IdTipoAlerta","Valor":"84c279c0-63d2-45f1-b3e3-3d70522afe70"},{"Llave":"IdTransferenciaRecibida","Valor":"84c279c0-63d2-45f1-b3e3-3d70522afe93"},{"Llave":"IdTransferenciaProcesada","Valor":"84c279c0-63d2-45f1-b3e3-3d70522afe94"},{"Llave":"UrlFacilito","Valor":"http://186.5.29.68:501"}],"TiempoVidaOtp":600}', N'{"limites":{"numeroMaximoDiarioDeTransacciones":50,"montoMaximoDiarioDeTransacciones":1300,"saldoMaximoCuentaAsociada":3456,"saldoMaximoAgente":300,"existenciaCaja":100},"deposito":{"activo":true,"ValidarOtpAgente":true,"ValidarOtpCliente":false,"limites":{"numeroMaximoDiarioDeTransacciones":10,"montoMaximoDiarioDeTransacciones":100,"montoMinimoPorTransaccion":20,"montoMaximoPorTransaccion":100},"comisiones":{"agente":2,"administracionCanal":1,"cooperativa":2,"gravaIva":true},"operacion":"Depósito"},"retiro":{"activo":false,"ValidarOtpAgente":true,"ValidarOtpCliente":false,"limites":{"numeroMaximoDiarioDeTransacciones":11,"montoMaximoDiarioDeTransacciones":101,"montoMinimoPorTransaccion":21,"montoMaximoPorTransaccion":101},"comisiones":{"agente":3,"administracionCanal":2,"cooperativa":3,"gravaIva":false},"operacion":"Retiro"},"cobroServicios":{"activo":true,"ValidarOtpAgente":true,"ValidarOtpCliente":false,"limites":{"numeroMaximoDiarioDeTransacciones":12,"montoMaximoDiarioDeTransacciones":102,"montoMinimoPorTransaccion":22,"montoMaximoPorTransaccion":100},"comisiones":{"agente":2,"administracionCanal":1,"cooperativa":2,"gravaIva":true},"operacion":"Cobro de Servicios"}}', 1)
) 
AS Source([Id], [Nombre], [JsonConfiguracion], [JsonNegocio], [EstaActivo])
ON Target.Id = Source.Id
    WHEN MATCHED
    THEN UPDATE SET 
                    [Nombre] = Source.[Nombre], 
                    [JsonConfiguracion] = Source.[JsonConfiguracion],
                    [JsonNegocio] = Source.[JsonNegocio],
                    [EstaActivo] = Source.[EstaActivo]
    WHEN NOT MATCHED BY TARGET
    THEN
      INSERT ([Id], [Nombre], [JsonConfiguracion], [JsonNegocio], [EstaActivo])
      VALUES ([Id], [Nombre], [JsonConfiguracion], [JsonNegocio], [EstaActivo])
    WHEN NOT MATCHED BY SOURCE
    THEN DELETE;


