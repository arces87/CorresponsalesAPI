CREATE TABLE [Corresponsales].[Cuenta] (
    [Id]               UNIQUEIDENTIFIER NOT NULL,
    [AgenteId]         UNIQUEIDENTIFIER NULL,
    [Tipo]             NVARCHAR (MAX)   NULL,
    [NumeroCuenta]     NVARCHAR (MAX)   NULL,
    [EstaActivo]       BIT              NOT NULL,
    [Concurrencia]     ROWVERSION       NULL,
    [SecuencialCuenta] NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_Cuenta] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Cuenta_Agente_AgenteId] FOREIGN KEY ([AgenteId]) REFERENCES [Corresponsales].[Agente] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Cuenta_AgenteId]
    ON [Corresponsales].[Cuenta]([AgenteId] ASC);

