CREATE TABLE [dbo].[TBDespesa] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [Descricao]      NVARCHAR (100)   NOT NULL,
    [DataOcorrencia] DATE             NOT NULL,
    [Valor]          DECIMAL (18, 2)  NOT NULL,
    [FormaPagamento] INT              NOT NULL
);
GO

ALTER TABLE [dbo].[TBDespesa]
    ADD CONSTRAINT [PK_TBDespesa] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

