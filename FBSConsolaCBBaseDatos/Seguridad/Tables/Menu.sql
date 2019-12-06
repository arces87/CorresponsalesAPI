CREATE TABLE [Seguridad].[Menu] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [MenuPadreId]  UNIQUEIDENTIFIER NULL,
    [Nombre]       NVARCHAR (MAX)   NULL,
    [Orden]        INT              NOT NULL,
    [Icono]        NVARCHAR (MAX)   NULL,
    [Ruta]         NVARCHAR (MAX)   NULL,
    [EstaActivo]   BIT              NOT NULL,
    [Concurrencia] ROWVERSION       NULL,
    CONSTRAINT [PK_Menu] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Menu_Menu_MenuPadreId] FOREIGN KEY ([MenuPadreId]) REFERENCES [Seguridad].[Menu] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Menu_MenuPadreId]
    ON [Seguridad].[Menu]([MenuPadreId] ASC);

