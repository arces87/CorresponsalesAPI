CREATE TABLE [Canales].[Imagen] (
    [Id]              UNIQUEIDENTIFIER NOT NULL,
    [DispositivoId]   UNIQUEIDENTIFIER NULL,
    [DireccionImagen] NVARCHAR (MAX)   NULL,
    [EstaActivo]      BIT              NOT NULL,
    [Concurrencia]    ROWVERSION       NULL,
    CONSTRAINT [PK_Imagen] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Imagen_Dispositivo_DispositivoId] FOREIGN KEY ([DispositivoId]) REFERENCES [Canales].[Dispositivo] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Imagen_DispositivoId]
    ON [Canales].[Imagen]([DispositivoId] ASC);

