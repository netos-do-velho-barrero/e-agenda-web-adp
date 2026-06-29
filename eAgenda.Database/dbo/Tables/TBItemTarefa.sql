CREATE TABLE [dbo].[TBItemTarefa] (
    [Id]        UNIQUEIDENTIFIER NOT NULL,
    [Titulo]    NVARCHAR (100)   NULL,
    [Concluido] BIT              NOT NULL,
    [TarefaId]  UNIQUEIDENTIFIER NOT NULL
);
GO

ALTER TABLE [dbo].[TBItemTarefa]
    ADD CONSTRAINT [FK_TBItemTarefa.TarefaId_TBTarefa.Id] FOREIGN KEY ([TarefaId]) REFERENCES [dbo].[TBTarefa] ([Id]);
GO

ALTER TABLE [dbo].[TBItemTarefa]
    ADD CONSTRAINT [PK_TBItemTarefa] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

