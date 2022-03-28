CREATE TABLE [Seguridad].[CanalUsuario] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [CanalId]      UNIQUEIDENTIFIER NULL,
    [UsuarioId]    NVARCHAR (450)   NULL,
    [EstaActivo]   BIT              NOT NULL,
    [Concurrencia] ROWVERSION       NULL,
    CONSTRAINT [PK_CanalUsuario] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CanalUsuario_Canal_CanalId] FOREIGN KEY ([CanalId]) REFERENCES [Seguridad].[Canal] ([Id]),
    CONSTRAINT [FK_CanalUsuario_Usuario_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [Seguridad].[Usuario] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_CanalUsuario_CanalId]
    ON [Seguridad].[CanalUsuario]([CanalId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CanalUsuario_UsuarioId]
    ON [Seguridad].[CanalUsuario]([UsuarioId] ASC);

