using eAgenda.WebApp.Compartilhado.Dominio;

namespace eAgenda.WebApp.Modulos.ModuloTarefas.Dominio;

public class Tarefa : EntidadeBase<Tarefa>
{
    public string Titulo { get; set; } = string.Empty;
    public PrioridadeTarefa Prioridade { get; set; }
    public DateOnly DataCriacao { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public DateOnly? DataConclusao { get; set; }
    public bool Concluida { get; set; }
    public decimal PercentualConcluido { get; set; }
    public List<ItemTarefa> Itens { get; set; } = [];

    public Tarefa() { }

    public Tarefa(string titulo, PrioridadeTarefa prioridade)
    {
        Titulo = titulo;
        Prioridade = prioridade;
    }

    public override List<string> Validar()
    {
        List<string> erros = [];

        if (string.IsNullOrWhiteSpace(Titulo) || Titulo.Length < 2 || Titulo.Length > 100)
            erros.Add("O campo \"Titulo\" deve conter entre 2 e 100 caracteres.");

        foreach (ItemTarefa item in Itens)
        {
            if (string.IsNullOrWhiteSpace(item.Titulo) || item.Titulo.Length < 2 || item.Titulo.Length > 100)
                erros.Add("O campo \"Titulo\" do item deve conter entre 2 e 100 caracteres.");
        }

        return erros;
    }

    public void AtualizarPercentual()
    {
        if (Itens.Count == 0)
        {
            PercentualConcluido = Concluida ? 100 : 0;
            DataConclusao = Concluida ? DateOnly.FromDateTime(DateTime.Today) : null;
            return;
        }

        int quantidadeConcluida = Itens.Count(i => i.Concluido);

        PercentualConcluido = quantidadeConcluida * 100m / Itens.Count;
        Concluida = PercentualConcluido == 100;
        DataConclusao = Concluida ? DateOnly.FromDateTime(DateTime.Today) : null;
    }

    public override void Atualizar(Tarefa entidadeAtualizada)
    {
        Titulo = entidadeAtualizada.Titulo;
        Prioridade = entidadeAtualizada.Prioridade;
        DataConclusao = entidadeAtualizada.DataConclusao;
        Concluida = entidadeAtualizada.Concluida;
        PercentualConcluido = entidadeAtualizada.PercentualConcluido;
        Itens = entidadeAtualizada.Itens;
    }
}