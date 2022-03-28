CREATE TABLE [Corresponsales].[Transaccion] (
    [Id]                    UNIQUEIDENTIFIER NOT NULL,
    [AgenteId]              UNIQUEIDENTIFIER NULL,
    [EstadoId]              UNIQUEIDENTIFIER NULL,
    [CanalId]               NVARCHAR (MAX)   NULL,
    [FechaSistema]          DATETIME2 (7)    NOT NULL,
    [FechaDispositivo]      DATETIME2 (7)    NOT NULL,
    [HoraDispositivo]       TIME (7)         NOT NULL,
    [Criptografia]          NVARCHAR (MAX)   NULL,
    [Tipo]                  NVARCHAR (MAX)   NULL,
    [JsonDatos]             NVARCHAR (MAX)   NULL,
    [Valor]                 FLOAT (53)       NOT NULL,
    [SaldoDisponible]       FLOAT (53)       NOT NULL,
    [Comisiones]            NVARCHAR (MAX)   NULL,
    [ReposicionRealizada]   BIT              NOT NULL,
    [Descripcion]           NVARCHAR (MAX)   NULL,
    [NombreCliente]         NVARCHAR (MAX)   NULL,
    [IdentificacionCliente] NVARCHAR (MAX)   NULL,
    [SecuencialCuenta]      NVARCHAR (MAX)   NULL,
    [EstaActivo]            BIT              NOT NULL,
    [Concurrencia]          ROWVERSION       NULL,
    [SaldoCuenta]           FLOAT (53)       NULL,
    CONSTRAINT [PK_Transaccion] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Transaccion_Agente_AgenteId] FOREIGN KEY ([AgenteId]) REFERENCES [Corresponsales].[Agente] ([Id]),
    CONSTRAINT [FK_Transaccion_Catalogo_EstadoId] FOREIGN KEY ([EstadoId]) REFERENCES [Nomenclador].[Catalogo] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Transaccion_AgenteId]
    ON [Corresponsales].[Transaccion]([AgenteId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Transaccion_EstadoId]
    ON [Corresponsales].[Transaccion]([EstadoId] ASC);

