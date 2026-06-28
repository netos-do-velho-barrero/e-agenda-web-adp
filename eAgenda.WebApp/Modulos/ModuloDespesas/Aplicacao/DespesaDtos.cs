using eAgenda.WebApp.Modulos.ModuloDespesas.Dominio;

namespace eAgenda.WebApp.Modulos.ModuloDespesas.Aplicacao;


public record ListarDespesaDto(Guid Id, string Descricao, DateTime DataOcorrencia, decimal Valor, FormaPagamento FormaPagamento);

public record CadastrarDespesaDto(string Descricao, DateTime DataOcorrencia, decimal Valor, FormaPagamento FormaPagamento, List<Guid> CategoriaIds);

public record EditarDespesaDto(Guid Id, string Descricao, DateTime DataOcorrencia, decimal Valor, FormaPagamento FormaPagamento, List<Guid> CategoriaIds);

public record DetalhesDespesaDto(Guid Id, string Descricao, DateTime DataOcorrencia, decimal Valor, FormaPagamento FormaPagamento, List<Guid> CategoriaIds);