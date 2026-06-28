using eAgenda.WebApp.Compartilhado.Dominio;

namespace eAgenda.WebApp.Modulos.ModuloDespesas.Dominio;

public class Despesa : EntidadeBase<Despesa>
{
    public string Descricao { get; set; } = string.Empty;
    public DateTime DataOcorrencia { get; set; } = DateTime.Today;
    public decimal Valor { get; set; }
    public FormaPagamento FormaPagamento { get; set; }
    public List<Guid> CategoriaIds { get; set; } = [];

    public Despesa() { }

    public Despesa(string descricao, DateTime dataOcorrencia, decimal valor, FormaPagamento formaPagamento, List<Guid> categoriaIds)
    {
        Descricao = descricao;
        DataOcorrencia = dataOcorrencia;
        Valor = valor;
        FormaPagamento = formaPagamento;
        CategoriaIds = categoriaIds;
    }

    public override List<string> Validar()
    {
        List<string> erros = [];

        if (string.IsNullOrWhiteSpace(Descricao) || Descricao.Length < 2 || Descricao.Length > 100)
            erros.Add("O campo \"Descricao\" deve conter entre 2 e 100 caracteres.");

        if (Valor <= 0)
            erros.Add("O campo \"Valor\" deve ser maior que zero.");

        if (CategoriaIds.Count == 0)
            erros.Add("A despesa deve possuir ao menos uma categoria.");

        return erros;
    }

    public override void Atualizar(Despesa entidadeAtualizada)
    {
        Descricao = entidadeAtualizada.Descricao;
        DataOcorrencia = entidadeAtualizada.DataOcorrencia;
        Valor = entidadeAtualizada.Valor;
        FormaPagamento = entidadeAtualizada.FormaPagamento;
        CategoriaIds = entidadeAtualizada.CategoriaIds;
    }
}