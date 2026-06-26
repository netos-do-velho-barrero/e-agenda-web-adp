using eAgenda.WebApp.Compartilhado.Dominio;

namespace eAgenda.WebApp.Modulos.ModuloTarefas.Dominio;

public interface IRepositorioTarefa : IRepositorio<Tarefa>
{
    List<Tarefa> SelecionarPendentes();
    List<Tarefa> SelecionarConcluidas();
    List<Tarefa> SelecionarPorPrioridade(PrioridadeTarefa prioridade);
}