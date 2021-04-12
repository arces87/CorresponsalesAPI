CREATE TABLE [Canales].[ImagenGeolocalizacion] (
    [Id]                UNIQUEIDENTIFIER NOT NULL,
    [GeolocalizacionId] UNIQUEIDENTIFIER NULL,
    [DireccionImagen]   NVARCHAR (MAX)   NULL,
    [EstaActivo]        BIT              NOT NULL,
    [Concurrencia]      ROWVERSION       NULL,
    CONSTRAINT [PK_ImagenGeolocalizacion] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ImagenGeolocalizacion_Geolocalizacion_GeolocalizacionId] FOREIGN KEY ([GeolocalizacionId]) REFERENCES [Canales].[Geolocalizacion] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_ImagenGeolocalizacion_GeolocalizacionId]
    ON [Canales].[ImagenGeolocalizacion]([GeolocalizacionId] ASC);

