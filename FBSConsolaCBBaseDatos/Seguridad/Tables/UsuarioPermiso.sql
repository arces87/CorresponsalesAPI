CREATE TABLE [Seguridad].[UsuarioPermiso] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [UsuarioId] NVARCHAR (450) NOT NULL,
    [Tipo]      NVARCHAR (MAX) NULL,
    [Valor]     NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_UsuarioPermiso] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UsuarioPermiso_Usuario_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [Seguridad].[Usuario] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_UsuarioPermiso_UsuarioId]
    ON [Seguridad].[UsuarioPermiso]([UsuarioId] ASC);

