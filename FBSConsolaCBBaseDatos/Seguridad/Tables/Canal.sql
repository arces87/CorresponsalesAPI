CREATE TABLE [Seguridad].[Canal] (
    [Id]                UNIQUEIDENTIFIER NOT NULL,
    [Nombre]            NVARCHAR (MAX)   NULL,
    [JsonConfiguracion] NVARCHAR (MAX)   NULL,
    [JsonNegocio]       NVARCHAR (MAX)   NULL,
    [EstaActivo]        BIT              NOT NULL,
    [Concurrencia]      ROWVERSION       NULL,
    CONSTRAINT [PK_Canal] PRIMARY KEY CLUSTERED ([Id] ASC)
);

