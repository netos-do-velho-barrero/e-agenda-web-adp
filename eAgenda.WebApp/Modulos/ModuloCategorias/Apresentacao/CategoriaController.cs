using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using eAgenda.WebApp.Compartilhado.Apresentacao.Extensions;
using eAgenda.WebApp.Modulos.ModuloCategoria.Aplicacao;
using eAgenda.WebApp.Modulos.ModuloDespesas.Aplicacao;
using eAgenda.WebApp.Modulos.ModuloDespesas.Apresentacao;

namespace eAgenda.WebApp.Modulos.ModuloCategoria.Apresentacao;

public class CategoriaController(
    ServicoCategoria servicoCategoria,
    ServicoDespesa servicoDespesa,
    IMapper mapeador
) : Controller
{
    [HttpGet]
    public ActionResult Listar()
    {
        List<ListarCategoriaDto> dtos = servicoCategoria.SelecionarTodos();

        List<ListarCategoriaViewModel> listarVms = mapeador.Map<List<ListarCategoriaViewModel>>(dtos);

        return View(listarVms);
    }

    [HttpGet]
    public ActionResult Despesas(Guid id)
    {
        Result<DetalhesCategoriaDto> resultadoCategoria = servicoCategoria.SelecionarPorId(id);

        if (resultadoCategoria.IsFailed)
        {
            TempData.AddErrorMessage(resultadoCategoria);
            return RedirectToAction(nameof(Listar));
        }

        List<ListarDespesaDto> despesasDto = servicoDespesa.SelecionarPorCategoria(id);
        List<ListarDespesaViewModel> despesasVm = mapeador.Map<List<ListarDespesaViewModel>>(despesasDto);

        ViewBag.CategoriaTitulo = resultadoCategoria.Value.Titulo;

        return View(despesasVm);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        CadastrarCategoriaViewModel cadastrarVm = new CadastrarCategoriaViewModel(string.Empty);
        return View(cadastrarVm);
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarCategoriaViewModel cadastrarVm)
    {
        if (!ModelState.IsValid)
            return View(cadastrarVm);

        CadastrarCategoriaDto dto = mapeador.Map<CadastrarCategoriaDto>(cadastrarVm);
        Result resultado = servicoCategoria.Cadastrar(dto);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            return View(cadastrarVm);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Editar(Guid id)
    {
        Result<DetalhesCategoriaDto> resultado = servicoCategoria.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            TempData.AddErrorMessage(resultado);
            return RedirectToAction(nameof(Listar));
        }

        EditarCategoriaViewModel editarVm = mapeador.Map<EditarCategoriaViewModel>(resultado.Value);

        return View(editarVm);
    }

    [HttpPost]
    public ActionResult Editar(EditarCategoriaViewModel editarVm)
    {
        if (!ModelState.IsValid)
            return View(editarVm);

        EditarCategoriaDto dto = mapeador.Map<EditarCategoriaDto>(editarVm);
        Result resultado = servicoCategoria.Editar(dto);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            return View(editarVm);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Excluir(Guid id)
    {
        Result<DetalhesCategoriaDto> resultado = servicoCategoria.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            TempData.AddErrorMessage(resultado);
            return RedirectToAction(nameof(Listar));
        }

        ExcluirCategoriaViewModel excluirVm = mapeador.Map<ExcluirCategoriaViewModel>(resultado.Value);

        return View(excluirVm);
    }

    [HttpPost]
    public ActionResult Excluir(ExcluirCategoriaViewModel excluirVm)
    {
        Result resultado = servicoCategoria.Excluir(excluirVm.Id);

        if (resultado.IsFailed)
            TempData.AddErrorMessage(resultado);

        return RedirectToAction(nameof(Listar));
    }
}