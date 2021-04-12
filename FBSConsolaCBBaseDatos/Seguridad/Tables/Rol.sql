CREATE TABLE [Seguridad].[Rol] (
    [Id]                NVARCHAR (450) NOT NULL,
    [Nombre]            NVARCHAR (256) NULL,
    [NombreNormalizado] NVARCHAR (256) NULL,
    [Descripcion]       NVARCHAR (256) NULL,
    [Tipo]              BIT            NOT NULL,
    [EstaActivo]        BIT            DEFAULT ((1)) NOT NULL,
    [Concurrencia]      NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Rol] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex]
    ON [Seguridad].[Rol]([NombreNormalizado] ASC) WHERE ([NombreNormalizado] IS NOT NULL);

