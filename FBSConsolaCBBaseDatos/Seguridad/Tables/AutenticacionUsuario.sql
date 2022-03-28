CREATE TABLE [Seguridad].[AutenticacionUsuario] (
    [Proveedor]       NVARCHAR (450) NOT NULL,
    [LlaveProveedor]  NVARCHAR (450) NOT NULL,
    [NombreProveedor] NVARCHAR (MAX) NULL,
    [UsuarioId]       NVARCHAR (450) NOT NULL,
    CONSTRAINT [PK_AutenticacionUsuario] PRIMARY KEY CLUSTERED ([Proveedor] ASC, [LlaveProveedor] ASC),
    CONSTRAINT [FK_AutenticacionUsuario_Usuario_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [Seguridad].[Usuario] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_AutenticacionUsuario_UsuarioId]
    ON [Seguridad].[AutenticacionUsuario]([UsuarioId] ASC);

