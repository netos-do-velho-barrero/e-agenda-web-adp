using eAgenda.WebApp.Modulos.ModuloContatos.Aplicacao;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApp.Modulos.ModuloContatos.Apresentacao;

public class ContatoController : Controller
{
    private readonly ServicoContato servicoContato;

    public ContatoController(ServicoContato servicoContato)
    {
        this.servicoContato = servicoContato;
    }

    [HttpGet]
    public IActionResult Listar()
    {
        List<ListarContatosViewModel> viewModel = servicoContato
            .SelecionarTodos()
            .Select(c => new ListarContatosViewModel(c.Id, c.Nome, c.Email, c.Telefone))
            .ToList();

        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Cadastrar()
    {
        return View(new CadastrarContatoViewModel(string.Empty, string.Empty, string.Empty, null, null));
    }

    [HttpPost]
    public IActionResult Cadastrar(CadastrarContatoViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        CadastrarContatoDto dto = new(
            viewModel.Nome,
            viewModel.Email,
            viewModel.Telefone,
            viewModel.Cargo,
            viewModel.Empresa
        );

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
            return RedirectToAction(nameof(Listar));

        DetalhesContatoDto contato = resultado.Value;

        EditarContatoViewModel viewModel = new(
            contato.Id,
            contato.Nome,
            contato.Email,
            contato.Telefone,
            contato.Cargo,
            contato.Empresa
        );

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Editar(EditarContatoViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        EditarContatoDto dto = new(
            viewModel.Id,
            viewModel.Nome,
            viewModel.Email,
            viewModel.Telefone,
            viewModel.Cargo,
            viewModel.Empresa
        );

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
            return RedirectToAction(nameof(Listar));

        DetalhesContatoDto contato = resultado.Value;

        ExcluirContatoViewModel viewModel = new(
            contato.Id,
            contato.Nome,
            contato.Email,
            contato.Telefone,
            contato.Cargo,
            contato.Empresa
        );

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