using eAgenda.WebApp.Modulos.ModuloDespesas.Dominio;

namespace eAgenda.WebApp.Modulos.ModuloDespesas.Aplicacao;

public record ListarDespesasDto(Guid Id, string Descricao, DateOnly DataOcorrencia, decimal Valor, FormaPagamento FormaPagamento);

public record CadastrarDespesaDto(string Descricao, DateOnly DataOcorrencia, decimal Valor, FormaPagamento FormaPagamento, List<Guid> CategoriaIds);

public record EditarDespesaDto(Guid Id, string Descricao, DateOnly DataOcorrencia, decimal Valor, FormaPagamento FormaPagamento, List<Guid> CategoriaIds);

public record DetalhesDespesaDto(Guid Id, string Descricao, DateOnly DataOcorrencia, decimal Valor, FormaPagamento FormaPagamento, List<Guid> CategoriaIds);