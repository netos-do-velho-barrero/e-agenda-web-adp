using eAgenda.WebApp.Compartilhado.Dominio;

namespace eAgenda.WebApp.Modulos.ModuloCompromissos.Dominio;

public class Compromisso : EntidadeBase<Compromisso>
{
    public string Assunto { get; set; } = string.Empty;
    public DateOnly DataOcorrencia { get; set; }
    public TimeOnly HoraInicio { get; set; }
    public TimeOnly HoraTermino { get; set; }
    public TipoCompromisso Tipo { get; set; }
    public string? Local { get; set; }
    public string? Link { get; set; }
    public Guid? ContatoId { get; set; }

    public Compromisso() { }

    public Compromisso(
        string assunto,
        DateOnly dataOcorrencia,
        TimeOnly horaInicio,
        TimeOnly horaTermino,
        TipoCompromisso tipo,
        string? local,
        string? link,
        Guid? contatoId
    )
    {
        Assunto = assunto;
        DataOcorrencia = dataOcorrencia;
        HoraInicio = horaInicio;
        HoraTermino = horaTermino;
        Tipo = tipo;
        Local = local;
        Link = link;
        ContatoId = contatoId;
    }

    public override List<string> Validar()
    {
        List<string> erros = [];

        if (string.IsNullOrWhiteSpace(Assunto) || Assunto.Length < 2 || Assunto.Length > 100)
            erros.Add("O campo \"Assunto\" deve conter entre 2 e 100 caracteres.");

        if (HoraTermino <= HoraInicio)
            erros.Add("A hora de termino deve ser maior que a hora de inicio.");

        if (Tipo == TipoCompromisso.Presencial && string.IsNullOrWhiteSpace(Local))
            erros.Add("O campo \"Local\" deve ser preenchido para compromissos presenciais.");

        if (Tipo == TipoCompromisso.Remoto && string.IsNullOrWhiteSpace(Link))
            erros.Add("O campo \"Link\" deve ser preenchido para compromissos remotos.");

        return erros;
    }

    public override void Atualizar(Compromisso entidadeAtualizada)
    {
        Assunto = entidadeAtualizada.Assunto;
        DataOcorrencia = entidadeAtualizada.DataOcorrencia;
        HoraInicio = entidadeAtualizada.HoraInicio;
        HoraTermino = entidadeAtualizada.HoraTermino;
        Tipo = entidadeAtualizada.Tipo;
        Local = entidadeAtualizada.Local;
        Link = entidadeAtualizada.Link;
        ContatoId = entidadeAtualizada.ContatoId;
    }
}