using AutoMapper;
using eAgenda.WebApp.Modulos.ModuloCompromissos.Aplicacao;
using eAgenda.WebApp.Modulos.ModuloCompromissos.Dominio;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApp.Modulos.ModuloCompromissos.Apresentacao;

public class CompromissoController : Controller
{
    private readonly ServicoCompromisso servicoCompromisso;
    private readonly IMapper mapeador;

    public CompromissoController(ServicoCompromisso servicoCompromisso, IMapper mapeador)
    {
        this.servicoCompromisso = servicoCompromisso;
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
            return View(viewModel);

        CadastrarCompromissoDto dto = mapeador.Map<CadastrarCompromissoDto>(viewModel);

        Result resultado = servicoCompromisso.Cadastrar(dto);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(string.Empty, resultado.Errors[0].Message);

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

        EditarCompromissoViewModel viewModel =
            mapeador.Map<EditarCompromissoViewModel>(resultado.Value);

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Editar(EditarCompromissoViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        EditarCompromissoDto dto = mapeador.Map<EditarCompromissoDto>(viewModel);

        Result resultado = servicoCompromisso.Editar(dto);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(string.Empty, resultado.Errors[0].Message);

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
}