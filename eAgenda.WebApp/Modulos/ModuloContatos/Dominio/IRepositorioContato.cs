using eAgenda.WebApp.Compartilhado.Dominio;

namespace eAgenda.WebApp.Modulos.ModuloContatos.Dominio;

public interface IRepositorioContato : IRepositorio<Contato>
{
    bool ExisteEmail(string email, Guid? idIgnorado = null);
    bool ExisteTelefone(string telefone, Guid? idIgnorado = null);
    bool PossuiCompromissosVinculados(Guid contatoId);
}