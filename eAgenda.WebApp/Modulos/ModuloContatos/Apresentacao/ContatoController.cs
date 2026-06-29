using AutoMapper;
using eAgenda.WebApp.Modulos.ModuloContatos.Aplicacao;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApp.Modulos.ModuloContatos.Apresentacao;

public class ContatoController : Controller
{
    private readonly ServicoContato servicoContato;
    private readonly IMapper mapeador;

    public ContatoController(ServicoContato servicoContato, IMapper mapeador)
    {
        this.servicoContato = servicoContato;
        this.mapeador = mapeador;
    }

    [HttpGet]
    public IActionResult Listar()
    {
        List<ListarContatosDto> contatosDto = servicoContato.SelecionarTodos();

        List<ListarContatosViewModel> contatosVm =
            mapeador.Map<List<ListarContatosViewModel>>(contatosDto);

        return View(contatosVm);
    }

    [HttpGet]
    public IActionResult Cadastrar()
    {
        CadastrarContatoViewModel viewModel = new(string.Empty, string.Empty, string.Empty, null, null);

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Cadastrar(CadastrarContatoViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        CadastrarContatoDto dto = mapeador.Map<CadastrarContatoDto>(viewModel);

        Result resultado = servicoContato.Cadastrar(dto);

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
        Result<DetalhesContatoDto> resultado = servicoContato.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            TempData["MensagemErro"] = resultado.Errors[0].Message;
            return RedirectToAction(nameof(Listar));
        }

        EditarContatoViewModel viewModel = mapeador.Map<EditarContatoViewModel>(resultado.Value);

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Editar(EditarContatoViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        EditarContatoDto dto = mapeador.Map<EditarContatoDto>(viewModel);

        Result resultado = servicoContato.Editar(dto);

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
        Result<DetalhesContatoDto> resultado = servicoContato.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            TempData["MensagemErro"] = resultado.Errors[0].Message;
            return RedirectToAction(nameof(Listar));
        }

        ExcluirContatoViewModel viewModel = mapeador.Map<ExcluirContatoViewModel>(resultado.Value);

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Excluir(ExcluirContatoViewModel viewModel)
    {
        Result resultado = servicoContato.Excluir(viewModel.Id);

        if (resultado.IsFailed)
            TempData["MensagemErro"] = resultado.Errors[0].Message;

        return RedirectToAction(nameof(Listar));
    }
}