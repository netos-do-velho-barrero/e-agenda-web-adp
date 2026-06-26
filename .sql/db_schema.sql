CREATE TABLE [dbo].[TBContato]
(
    [Id] uniqueidentifier NOT NULL,
    [Nome] nvarchar(100) NOT NULL,
    [Email] nvarchar(100) NOT NULL,
    [Telefone] nvarchar(15) NOT NULL,
    [Cargo] nvarchar(100),
    [Empresa] nvarchar(100),
    PRIMARY KEY ([Id])
);

CREATE TABLE [dbo].[TBCompromisso]
(
    [Id] uniqueidentifier NOT NULL,
    [Assunto] nvarchar(100) NOT NULL,
    [DataOcorrencia] date NOT NULL,
    [HoraInicio] time(0) NOT NULL,
    [HoraTermino] time(0) NOT NULL,
    [TipoCompromisso] int,
    [Local] nvarchar(200) NOT NULL,
    [Link] nvarchar(500) NOT NULL,
    [ContatoId] uniqueidentifier,
    PRIMARY KEY ([Id])
);

CREATE TABLE [dbo].[TBCategoria]
(
    [Id] uniqueidentifier NOT NULL,
    [Titulo] nvarchar(100) NOT NULL,
    PRIMARY KEY ([Id])
);

CREATE TABLE [dbo].[TBDespesa]
(
    [Id] uniqueidentifier NOT NULL,
    [Descricao] nvarchar(100) NOT NULL,
    [DataOcorrencia] date NOT NULL,
    [Valor] decimal(18,2) NOT NULL,
    [FormaPagamento] int NOT NULL,
    PRIMARY KEY ([Id])
);

CREATE TABLE [dbo].[TBDespesaCategoria]
(
    [DespesaId     ] uniqueidentifier NOT NULL,
    [CategoriaId] uniqueidentifier NOT NULL,
    PRIMARY KEY ([DespesaId     ], [CategoriaId])
);

CREATE TABLE [dbo].[TBTarefa]
(
    [Id] uniqueidentifier NOT NULL,
    [Titulo] nvarchar(100) NOT NULL,
    [Prioridade] int NOT NULL,
    [DataCriacao] date NOT NULL,
    [DataConclusao] date,
    [Concluida] bit NOT NULL,
    [PercentualConcluido] decimal(5,2) NOT NULL,
    PRIMARY KEY ([Id])
);

CREATE TABLE [dbo].[TBItemTarefa]
(
    [Id] uniqueidentifier NOT NULL,
    [Titulo] nvarchar(100),
    [Concluido] bit NOT NULL,
    [TarefaId] uniqueidentifier NOT NULL,
    PRIMARY KEY ([Id])
);


ALTER TABLE [dbo].[TBCompromisso]
ADD CONSTRAINT [FK_TBCompromisso_TBContato]
FOREIGN KEY ([ContatoId]) 
REFERENCES [dbo].[TBContato]([Id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[TBDespesaCategoria]
ADD CONSTRAINT [FK_TBDespesaCategoria.DespesaId_TBDespesa.Id]
FOREIGN KEY ([DespesaId     ]) 
REFERENCES [dbo].[TBDespesa]([Id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[TBDespesaCategoria]
ADD CONSTRAINT [FK_TBDespesaCategoria.CategoriaId_TBCategoria.Id]
FOREIGN KEY ([CategoriaId]) 
REFERENCES [dbo].[TBCategoria]([Id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



ALTER TABLE [dbo].[TBItemTarefa]
ADD CONSTRAINT [FK_TBItemTarefa.TarefaId_TBTarefa.Id]
FOREIGN KEY ([TarefaId]) 
REFERENCES [dbo].[TBTarefa]([Id])
ON DELETE NO ACTION
ON UPDATE NO ACTION;



