CREATE TABLE [Canales].[Geolocalizacion] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [Latitud]      FLOAT (53)       NOT NULL,
    [Longitud]     FLOAT (53)       NOT NULL,
    [FechaAlta]    DATETIME2 (7)    NOT NULL,
    [FechaBaja]    DATETIME2 (7)    NOT NULL,
    [EstaActivo]   BIT              NOT NULL,
    [Concurrencia] ROWVERSION       NULL,
    CONSTRAINT [PK_Geolocalizacion] PRIMARY KEY CLUSTERED ([Id] ASC)
);

