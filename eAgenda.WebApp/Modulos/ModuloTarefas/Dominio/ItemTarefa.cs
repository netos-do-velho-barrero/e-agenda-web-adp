namespace eAgenda.WebApp.Modulos.ModuloTarefas.Dominio;

public class ItemTarefa
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Titulo { get; set; } = string.Empty;
    public bool Concluido { get; set; }

    public ItemTarefa() { }

    public ItemTarefa(string titulo)
    {
        Titulo = titulo;
    }
}