CREATE TABLE [Seguridad].[RolMenu] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [RolId]        NVARCHAR (450)   NULL,
    [MenuId]       UNIQUEIDENTIFIER NULL,
    [EstaActivo]   BIT              NOT NULL,
    [Concurrencia] ROWVERSION       NULL,
    CONSTRAINT [PK_RolMenu] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_RolMenu_Menu_MenuId] FOREIGN KEY ([MenuId]) REFERENCES [Seguridad].[Menu] ([Id]),
    CONSTRAINT [FK_RolMenu_Rol_RolId] FOREIGN KEY ([RolId]) REFERENCES [Seguridad].[Rol] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_RolMenu_MenuId]
    ON [Seguridad].[RolMenu]([MenuId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RolMenu_RolId]
    ON [Seguridad].[RolMenu]([RolId] ASC);

