CREATE TABLE [Canales].[Dispositivo] (
    [Id]                 UNIQUEIDENTIFIER NOT NULL,
    [MarcaId]            UNIQUEIDENTIFIER NULL,
    [SistemaOperativoId] UNIQUEIDENTIFIER NULL,
    [MacAddress]         NVARCHAR (MAX)   NULL,
    [Modelo]             NVARCHAR (MAX)   NULL,
    [NumeroSerie]        NVARCHAR (MAX)   NULL,
    [TieneImpresora]     BIT              NOT NULL,
    [DireccionImpresora] NVARCHAR (MAX)   NULL,
    [Observaciones]      NVARCHAR (MAX)   NULL,
    [Ubicacion]          NVARCHAR (MAX)   NULL,
    [Imei]               NVARCHAR (MAX)   NULL,
    [EstaActivo]         BIT              NOT NULL,
    [Concurrencia]       ROWVERSION       NULL,
    CONSTRAINT [PK_Dispositivo] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Dispositivo_Catalogo_MarcaId] FOREIGN KEY ([MarcaId]) REFERENCES [Nomenclador].[Catalogo] ([Id]),
    CONSTRAINT [FK_Dispositivo_Catalogo_SistemaOperativoId] FOREIGN KEY ([SistemaOperativoId]) REFERENCES [Nomenclador].[Catalogo] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Dispositivo_MarcaId]
    ON [Canales].[Dispositivo]([MarcaId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Dispositivo_SistemaOperativoId]
    ON [Canales].[Dispositivo]([SistemaOperativoId] ASC);

