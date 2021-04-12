CREATE TABLE [Nomenclador].[TipoCatalogo] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [Nombre]       NVARCHAR (MAX)   NULL,
    [Descripcion]  NVARCHAR (MAX)   NULL,
    [EstaActivo]   BIT              NOT NULL,
    [Concurrencia] ROWVERSION       NULL,
    CONSTRAINT [PK_TipoCatalogo] PRIMARY KEY CLUSTERED ([Id] ASC)
);

