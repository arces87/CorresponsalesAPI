CREATE TABLE [Seguridad].[UsuarioToken] (
    [UsuarioId] NVARCHAR (450) NOT NULL,
    [Proveedor] NVARCHAR (450) NOT NULL,
    [Nombre]    NVARCHAR (450) NOT NULL,
    [Valor]     NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_UsuarioToken] PRIMARY KEY CLUSTERED ([UsuarioId] ASC, [Proveedor] ASC, [Nombre] ASC),
    CONSTRAINT [FK_UsuarioToken_Usuario_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [Seguridad].[Usuario] ([Id]) ON DELETE CASCADE
);

