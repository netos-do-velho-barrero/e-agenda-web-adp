CREATE TABLE [dbo].[TBCompromisso] (
    [Id]              UNIQUEIDENTIFIER NOT NULL,
    [Assunto]         NVARCHAR (100)   NOT NULL,
    [DataOcorrencia]  DATE             NOT NULL,
    [HoraInicio]      TIME (0)         NOT NULL,
    [HoraTermino]     TIME (0)         NOT NULL,
    [TipoCompromisso] INT              NULL,
    [Local]           NVARCHAR (200)   NOT NULL,
    [Link]            NVARCHAR (500)   NOT NULL,
    [ContatoId]       UNIQUEIDENTIFIER NULL
);
GO

ALTER TABLE [dbo].[TBCompromisso]
    ADD CONSTRAINT [FK_TBCompromisso_TBContato] FOREIGN KEY ([ContatoId]) REFERENCES [dbo].[TBContato] ([Id]);
GO

ALTER TABLE [dbo].[TBCompromisso]
    ADD CONSTRAINT [PK_TBCompromisso] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

