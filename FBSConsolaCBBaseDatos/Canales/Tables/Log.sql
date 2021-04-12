CREATE TABLE [Canales].[Log] (
    [Id]              UNIQUEIDENTIFIER NOT NULL,
    [TipoAccionId]    UNIQUEIDENTIFIER NULL,
    [EstadoId]        UNIQUEIDENTIFIER NULL,
    [UsuarioId]       NVARCHAR (450)   NULL,
    [Fecha]           DATETIME2 (7)    NOT NULL,
    [Hora]            TIME (7)         NOT NULL,
    [Criptografia]    NVARCHAR (MAX)   NULL,
    [JsonDispositivo] NVARCHAR (MAX)   NULL,
    [JsonLog]         NVARCHAR (MAX)   NULL,
    [RelacionadoId]   NVARCHAR (MAX)   NULL,
    [EstaActivo]      BIT              NOT NULL,
    [Concurrencia]    ROWVERSION       NULL,
    CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Log_Catalogo_EstadoId] FOREIGN KEY ([EstadoId]) REFERENCES [Nomenclador].[Catalogo] ([Id]),
    CONSTRAINT [FK_Log_Catalogo_TipoAccionId] FOREIGN KEY ([TipoAccionId]) REFERENCES [Nomenclador].[Catalogo] ([Id]),
    CONSTRAINT [FK_Log_Usuario_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [Seguridad].[Usuario] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Log_EstadoId]
    ON [Canales].[Log]([EstadoId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Log_TipoAccionId]
    ON [Canales].[Log]([TipoAccionId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Log_UsuarioId]
    ON [Canales].[Log]([UsuarioId] ASC);

