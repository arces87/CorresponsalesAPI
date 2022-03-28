CREATE TABLE [Corresponsales].[Agente] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [EstadoId]       UNIQUEIDENTIFIER NULL,
    [UsuarioId]      NVARCHAR (450)   NULL,
    [SupervisorId]   NVARCHAR (450)   NULL,
    [DispositivoId]  UNIQUEIDENTIFIER NULL,
    [NombreAgente]   NVARCHAR (MAX)   NULL,
    [JsonAgente]     NVARCHAR (MAX)   NULL,
    [Identificacion] NVARCHAR (MAX)   NULL,
    [Ubicacion]      NVARCHAR (MAX)   NULL,
    [EstaActivo]     BIT              NOT NULL,
    [Concurrencia]   ROWVERSION       NULL,
    CONSTRAINT [PK_Agente] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Agente_Catalogo_EstadoId] FOREIGN KEY ([EstadoId]) REFERENCES [Nomenclador].[Catalogo] ([Id]),
    CONSTRAINT [FK_Agente_Dispositivo_DispositivoId] FOREIGN KEY ([DispositivoId]) REFERENCES [Canales].[Dispositivo] ([Id]),
    CONSTRAINT [FK_Agente_Usuario_SupervisorId] FOREIGN KEY ([SupervisorId]) REFERENCES [Seguridad].[Usuario] ([Id]),
    CONSTRAINT [FK_Agente_Usuario_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [Seguridad].[Usuario] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Agente_DispositivoId]
    ON [Corresponsales].[Agente]([DispositivoId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Agente_EstadoId]
    ON [Corresponsales].[Agente]([EstadoId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Agente_SupervisorId]
    ON [Corresponsales].[Agente]([SupervisorId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Agente_UsuarioId]
    ON [Corresponsales].[Agente]([UsuarioId] ASC);

