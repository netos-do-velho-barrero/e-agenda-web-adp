CREATE TABLE [dbo].[TBTarefa] (
    [Id]                  UNIQUEIDENTIFIER NOT NULL,
    [Titulo]              NVARCHAR (100)   NOT NULL,
    [Prioridade]          INT              NOT NULL,
    [DataCriacao]         DATE             NOT NULL,
    [DataConclusao]       DATE             NULL,
    [Concluida]           BIT              NOT NULL,
    [PercentualConcluido] DECIMAL (5, 2)   NOT NULL
);
GO

ALTER TABLE [dbo].[TBTarefa]
    ADD CONSTRAINT [PK_TBTarefa] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

