using AutoMapper;
using eAgenda.WebApp.Modulos.ModuloTarefas.Aplicacao;
using eAgenda.WebApp.Modulos.ModuloTarefas.Dominio;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApp.Modulos.ModuloTarefas.Apresentacao;

public class TarefasController : Controller
{
    private readonly ServicoTarefa servicoTarefa;
    private readonly IMapper mapeador;

    public TarefasController(ServicoTarefa servicoTarefa, IMapper mapeador)
    {
        this.servicoTarefa = servicoTarefa;
        this.mapeador = mapeador;
    }

    [HttpGet]
    public IActionResult Listar(string? filtro, PrioridadeTarefa? prioridade)
    {
        List<ListarTarefasDto> tarefasDto = filtro switch
        {
            "pendentes" => servicoTarefa.SelecionarPendentes(),
            "concluidas" => servicoTarefa.SelecionarConcluidas(),
            "prioridade" when prioridade.HasValue => servicoTarefa.SelecionarPorPrioridade(prioridade.Value),
            _ => servicoTarefa.SelecionarTodos()
        };

        ViewBag.Filtro = filtro;
        ViewBag.Prioridade = prioridade;

        List<ListarTarefasViewModel> tarefasVm =
            mapeador.Map<List<ListarTarefasViewModel>>(tarefasDto);

        return View(tarefasVm);
    }

    [HttpGet]
    public IActionResult Cadastrar()
    {
        CadastrarTarefaViewModel viewModel = new(
            string.Empty,
            PrioridadeTarefa.Normal,
            []
        );

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Cadastrar(CadastrarTarefaViewModel viewModel)
    {
        List<string> itens = viewModel.Itens
            .Where(i => !string.IsNullOrWhiteSpace(i))
            .ToList();

        CadastrarTarefaViewModel viewModelTratado = viewModel with { Itens = itens };

        if (!ModelState.IsValid)
            return View(viewModelTratado);

        CadastrarTarefaDto dto = mapeador.Map<CadastrarTarefaDto>(viewModelTratado);

        Result resultado = servicoTarefa.Cadastrar(dto);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(string.Empty, resultado.Errors[0].Message);

            return View(viewModelTratado);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public IActionResult Editar(Guid id)
    {
        Result<DetalhesTarefaDto> resultado = servicoTarefa.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            TempData["MensagemErro"] = resultado.Errors[0].Message;

            return RedirectToAction(nameof(Listar));
        }

        EditarTarefaViewModel viewModel =
            mapeador.Map<EditarTarefaViewModel>(resultado.Value);

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Editar(EditarTarefaViewModel viewModel)
    {
        List<ItemTarefaViewModel> itens = viewModel.Itens
            .Where(i => !string.IsNullOrWhiteSpace(i.Titulo))
            .ToList();

        EditarTarefaViewModel viewModelTratado = viewModel with { Itens = itens };

        if (!ModelState.IsValid)
            return View(viewModelTratado);

        EditarTarefaDto dto = mapeador.Map<EditarTarefaDto>(viewModelTratado);

        Result resultado = servicoTarefa.Editar(dto);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(string.Empty, resultado.Errors[0].Message);

            return View(viewModelTratado);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public IActionResult Excluir(Guid id)
    {
        Result<DetalhesTarefaDto> resultado = servicoTarefa.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            TempData["MensagemErro"] = resultado.Errors[0].Message;

            return RedirectToAction(nameof(Listar));
        }

        ExcluirTarefaViewModel viewModel =
            mapeador.Map<ExcluirTarefaViewModel>(resultado.Value);

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Excluir(ExcluirTarefaViewModel viewModel)
    {
        Result resultado = servicoTarefa.Excluir(viewModel.Id);

        if (resultado.IsFailed)
            TempData["MensagemErro"] = resultado.Errors[0].Message;

        return RedirectToAction(nameof(Listar));
    }
}