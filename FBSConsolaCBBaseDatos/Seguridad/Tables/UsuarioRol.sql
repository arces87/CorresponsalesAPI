CREATE TABLE [Seguridad].[UsuarioRol] (
    [UsuarioId] NVARCHAR (450) NOT NULL,
    [RolId]     NVARCHAR (450) NOT NULL,
    CONSTRAINT [PK_UsuarioRol] PRIMARY KEY CLUSTERED ([UsuarioId] ASC, [RolId] ASC),
    CONSTRAINT [FK_UsuarioRol_Rol_RolId] FOREIGN KEY ([RolId]) REFERENCES [Seguridad].[Rol] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UsuarioRol_Usuario_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [Seguridad].[Usuario] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_UsuarioRol_RolId]
    ON [Seguridad].[UsuarioRol]([RolId] ASC);

