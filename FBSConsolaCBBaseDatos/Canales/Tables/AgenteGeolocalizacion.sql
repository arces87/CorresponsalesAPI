CREATE TABLE [Canales].[AgenteGeolocalizacion] (
    [Id]                UNIQUEIDENTIFIER NOT NULL,
    [AgenteId]          UNIQUEIDENTIFIER NULL,
    [GeolocalizacionId] UNIQUEIDENTIFIER NULL,
    [EstaActivo]        BIT              NOT NULL,
    [Concurrencia]      ROWVERSION       NULL,
    CONSTRAINT [PK_AgenteGeolocalizacion] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AgenteGeolocalizacion_Agente_AgenteId] FOREIGN KEY ([AgenteId]) REFERENCES [Corresponsales].[Agente] ([Id]),
    CONSTRAINT [FK_AgenteGeolocalizacion_Geolocalizacion_GeolocalizacionId] FOREIGN KEY ([GeolocalizacionId]) REFERENCES [Canales].[Geolocalizacion] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_AgenteGeolocalizacion_AgenteId]
    ON [Canales].[AgenteGeolocalizacion]([AgenteId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AgenteGeolocalizacion_GeolocalizacionId]
    ON [Canales].[AgenteGeolocalizacion]([GeolocalizacionId] ASC);

