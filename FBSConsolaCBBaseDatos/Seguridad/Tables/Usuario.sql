CREATE TABLE [Seguridad].[Usuario] (
    [Id]                           NVARCHAR (450)     NOT NULL,
    [OperadoraId]                  UNIQUEIDENTIFIER   NULL,
    [Codigo]                       NVARCHAR (256)     NULL,
    [NombreCompleto]               NVARCHAR (256)     NULL,
    [NombreMostrar]                NVARCHAR (256)     NULL,
    [Contrasenia]                  NVARCHAR (MAX)     NULL,
    [FechaCreacion]                DATETIME2 (7)      NOT NULL,
    [Imagen]                       NVARCHAR (256)     NULL,
    [CorreoElectronico]            NVARCHAR (256)     NULL,
    [TelefonoCelular]              NVARCHAR (MAX)     NULL,
    [CambioContrasenia]            BIT                NOT NULL,
    [CodigoNormalizado]            NVARCHAR (256)     NULL,
    [CorreoElectronicoNormalizado] NVARCHAR (256)     NULL,
    [CorreoElectronicoConfirmado]  BIT                NOT NULL,
    [TelefonoConfirmado]           BIT                NOT NULL,
    [MarcaSeguridad]               NVARCHAR (MAX)     NULL,
    [DobleVerificacion]            BIT                NOT NULL,
    [AccesosFallidos]              INT                NOT NULL,
    [FinBloqueo]                   DATETIMEOFFSET (7) NULL,
    [BloqueoActivo]                BIT                NOT NULL,
    [EstaActivo]                   BIT                DEFAULT ((1)) NOT NULL,
    [Concurrencia]                 NVARCHAR (MAX)     NULL,
    CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Usuario_Catalogo_OperadoraId] FOREIGN KEY ([OperadoraId]) REFERENCES [Nomenclador].[Catalogo] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [EmailIndex]
    ON [Seguridad].[Usuario]([CorreoElectronicoNormalizado] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Usuario_OperadoraId]
    ON [Seguridad].[Usuario]([OperadoraId] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex]
    ON [Seguridad].[Usuario]([CodigoNormalizado] ASC) WHERE ([CodigoNormalizado] IS NOT NULL);

