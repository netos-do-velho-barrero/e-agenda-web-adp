using FluentResults;
using eAgenda.WebApp.Modulos.ModuloDespesas.Dominio;

namespace eAgenda.WebApp.Modulos.ModuloDespesas.Aplicacao;

public class ServicoDespesa
{
    private readonly IRepositorioDespesa repositorioDespesa;

    public ServicoDespesa(IRepositorioDespesa repositorioDespesa)
    {
        this.repositorioDespesa = repositorioDespesa;
    }

    public Result Cadastrar(CadastrarDespesaDto dto)
    {
        Despesa despesa = new(dto.Descricao, dto.DataOcorrencia, dto.Valor, dto.FormaPagamento, dto.CategoriaIds);

        Result resultadoValidacao = ValidarEntidade(despesa);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        repositorioDespesa.Cadastrar(despesa);

        return Result.Ok();
    }

    public Result Editar(EditarDespesaDto dto)
    {
        Despesa despesa = new(dto.Descricao, dto.DataOcorrencia, dto.Valor, dto.FormaPagamento, dto.CategoriaIds);

        Result resultadoValidacao = ValidarEntidade(despesa);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        bool conseguiuEditar = repositorioDespesa.Editar(dto.Id, despesa);

        if (!conseguiuEditar)
            return Result.Fail("Despesa nao encontrada.");

        return Result.Ok();
    }

    public Result Excluir(Guid id)
    {
        bool conseguiuExcluir = repositorioDespesa.Excluir(id);

        if (!conseguiuExcluir)
            return Result.Fail("Despesa nao encontrada.");

        return Result.Ok();
    }

    public List<ListarDespesaDto> SelecionarTodos()
    {
        return repositorioDespesa.SelecionarTodos()
            .Select(d => new ListarDespesaDto(d.Id, d.Descricao, d.DataOcorrencia, d.Valor, d.FormaPagamento))
            .ToList();
    }

    public Result<DetalhesDespesaDto> SelecionarPorId(Guid id)
    {
        Despesa? despesa = repositorioDespesa.SelecionarPorId(id);

        if (despesa == null)
            return Result.Fail("Despesa nao encontrada.");

        return Result.Ok(new DetalhesDespesaDto(despesa.Id, despesa.Descricao, despesa.DataOcorrencia, despesa.Valor, despesa.FormaPagamento, despesa.CategoriaIds));
    }

    private static Result ValidarEntidade(Despesa despesa)
    {
        List<string> erros = despesa.Validar();

        if (erros.Count == 0)
            return Result.Ok();

        return Result.Fail(new Error(erros.First()).WithMetadata("Campo", string.Empty));
    }
}