using eAgenda.WebApp.Compartilhado.Dominio;

namespace eAgenda.WebApp.Modulos.ModuloCategoria.Dominio;

public interface IRepositorioCategoria : IRepositorio<Categoria>
{
    bool ExisteTitulo(string titulo, Guid? idIgnorado = null);
    bool PossuiDespesasVinculadas(Guid categoriaId);
}