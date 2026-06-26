using eAgenda.WebApp.Compartilhado.Dominio;

namespace eAgenda.WebApp.Modulos.ModuloDespesas.Dominio;

public interface IRepositorioDespesa : IRepositorio<Despesa>
{
    List<Despesa> SelecionarPorCategoria(Guid categoriaId);
}