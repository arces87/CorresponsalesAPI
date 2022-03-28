CREATE TABLE [Seguridad].[RolPermiso] (
    [Id]    INT            IDENTITY (1, 1) NOT NULL,
    [RolId] NVARCHAR (450) NOT NULL,
    [Tipo]  NVARCHAR (MAX) NULL,
    [Valor] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_RolPermiso] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_RolPermiso_Rol_RolId] FOREIGN KEY ([RolId]) REFERENCES [Seguridad].[Rol] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_RolPermiso_RolId]
    ON [Seguridad].[RolPermiso]([RolId] ASC);

