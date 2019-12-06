CREATE TABLE [Nomenclador].[Catalogo] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [TipoCatalogoId] UNIQUEIDENTIFIER NULL,
    [Nombre]         NVARCHAR (MAX)   NULL,
    [Descripcion]    NVARCHAR (MAX)   NULL,
    [EstaActivo]     BIT              NOT NULL,
    [Concurrencia]   ROWVERSION       NULL,
    CONSTRAINT [PK_Catalogo] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Catalogo_TipoCatalogo_TipoCatalogoId] FOREIGN KEY ([TipoCatalogoId]) REFERENCES [Nomenclador].[TipoCatalogo] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Catalogo_TipoCatalogoId]
    ON [Nomenclador].[Catalogo]([TipoCatalogoId] ASC);

