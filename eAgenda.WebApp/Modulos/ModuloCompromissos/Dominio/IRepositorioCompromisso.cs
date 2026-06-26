using eAgenda.WebApp.Compartilhado.Dominio;

namespace eAgenda.WebApp.Modulos.ModuloCompromissos.Dominio;

public interface IRepositorioCompromisso : IRepositorio<Compromisso>
{
    bool ExisteConflitoDeHorario(
        DateOnly data,
        TimeOnly horaInicio,
        TimeOnly horaTermino,
        Guid? idIgnorado = null
    );

    List<Compromisso> SelecionarCompromissosFuturos();
    List<Compromisso> SelecionarCompromissosPassados();
}