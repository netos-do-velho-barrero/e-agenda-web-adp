using AutoMapper;
using eAgenda.WebApp.Modulos.ModuloCompromissos.Aplicacao;
using eAgenda.WebApp.Modulos.ModuloCompromissos.Dominio;
using eAgenda.WebApp.Modulos.ModuloContatos.Aplicacao;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eAgenda.WebApp.Modulos.ModuloCompromissos.Apresentacao;

public class CompromissoController : Controller
{
    private readonly ServicoCompromisso servicoCompromisso;
    private readonly ServicoContato servicoContato;
    private readonly IMapper mapeador;

    public CompromissoController(
        ServicoCompromisso servicoCompromisso,
        ServicoContato servicoContato,
        IMapper mapeador
    )
    {
        this.servicoCompromisso = servicoCompromisso;
        this.servicoContato = servicoContato;
        this.mapeador = mapeador;
    }

    [HttpGet]
    public IActionResult Listar()
    {
        List<ListarCompromissosDto> compromissosDto = servicoCompromisso.SelecionarTodos();

        List<ListarCompromissosViewModel> compromissosVm =
            mapeador.Map<List<ListarCompromissosViewModel>>(compromissosDto);

        return View(compromissosVm);
    }

    [HttpGet]
    public IActionResult Cadastrar()
    {
        CarregarContatos();

        CadastrarCompromissoViewModel viewModel = new(
            string.Empty,
            DateOnly.FromDateTime(DateTime.Today),
            new TimeOnly(8, 0),
            new TimeOnly(9, 0),
            TipoCompromisso.Presencial,
            null,
            null,
            null
        );

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Cadastrar(CadastrarCompromissoViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            CarregarContatos();
            return View(viewModel);
        }

        CadastrarCompromissoDto dto = mapeador.Map<CadastrarCompromissoDto>(viewModel);

        Result resultado = servicoCompromisso.Cadastrar(dto);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(string.Empty, resultado.Errors[0].Message);
            CarregarContatos();
            return View(viewModel);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public IActionResult Editar(Guid id)
    {
        Result<DetalhesCompromissoDto> resultado = servicoCompromisso.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            TempData["MensagemErro"] = resultado.Errors[0].Message;
            return RedirectToAction(nameof(Listar));
        }

        CarregarContatos();

        EditarCompromissoViewModel viewModel =
            mapeador.Map<EditarCompromissoViewModel>(resultado.Value);

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Editar(EditarCompromissoViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            CarregarContatos();
            return View(viewModel);
        }

        EditarCompromissoDto dto = mapeador.Map<EditarCompromissoDto>(viewModel);

        Result resultado = servicoCompromisso.Editar(dto);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(string.Empty, resultado.Errors[0].Message);
            CarregarContatos();
            return View(viewModel);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public IActionResult Excluir(Guid id)
    {
        Result<DetalhesCompromissoDto> resultado = servicoCompromisso.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            TempData["MensagemErro"] = resultado.Errors[0].Message;
            return RedirectToAction(nameof(Listar));
        }

        ExcluirCompromissoViewModel viewModel =
            mapeador.Map<ExcluirCompromissoViewModel>(resultado.Value);

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Excluir(ExcluirCompromissoViewModel viewModel)
    {
        Result resultado = servicoCompromisso.Excluir(viewModel.Id);

        if (resultado.IsFailed)
            TempData["MensagemErro"] = resultado.Errors[0].Message;

        return RedirectToAction(nameof(Listar));
    }

    private void CarregarContatos()
    {
        List<SelectListItem> contatos = servicoContato.SelecionarTodos()
            .Select(c => new SelectListItem($"{c.Nome} - {c.Email}", c.Id.ToString()))
            .ToList();

        contatos.Insert(0, new SelectListItem("Nenhum contato", string.Empty));

        ViewBag.Contatos = contatos;
    }
}