CREATE TABLE [Corresponsales].[Alerta] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [EstadoId]     UNIQUEIDENTIFIER NULL,
    [AgenteId]     UNIQUEIDENTIFIER NULL,
    [TipoId]       UNIQUEIDENTIFIER NULL,
    [Fecha]        DATETIME2 (7)    NOT NULL,
    [Hora]         TIME (7)         NOT NULL,
    [Descripcion]  NVARCHAR (MAX)   NULL,
    [Comentario]   NVARCHAR (MAX)   NULL,
    [EstaActivo]   BIT              NOT NULL,
    [Concurrencia] ROWVERSION       NULL,
    CONSTRAINT [PK_Alerta] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Alerta_Agente_AgenteId] FOREIGN KEY ([AgenteId]) REFERENCES [Corresponsales].[Agente] ([Id]),
    CONSTRAINT [FK_Alerta_Catalogo_EstadoId] FOREIGN KEY ([EstadoId]) REFERENCES [Nomenclador].[Catalogo] ([Id]),
    CONSTRAINT [FK_Alerta_Catalogo_TipoId] FOREIGN KEY ([TipoId]) REFERENCES [Nomenclador].[Catalogo] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Alerta_AgenteId]
    ON [Corresponsales].[Alerta]([AgenteId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Alerta_EstadoId]
    ON [Corresponsales].[Alerta]([EstadoId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Alerta_TipoId]
    ON [Corresponsales].[Alerta]([TipoId] ASC);

