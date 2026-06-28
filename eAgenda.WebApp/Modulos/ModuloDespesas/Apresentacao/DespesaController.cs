using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using FluentResults;
using AutoMapper;
using eAgenda.WebApp.Compartilhado.Apresentacao.Extensions;
using eAgenda.WebApp.Modulos.ModuloDespesas.Aplicacao;
using eAgenda.WebApp.Modulos.ModuloDespesas.Dominio;
using eAgenda.WebApp.Modulos.ModuloCategoria.Aplicacao;

namespace eAgenda.WebApp.Modulos.ModuloDespesas.Apresentacao;

public class DespesaController(
    ServicoDespesa servicoDespesa, 
    ServicoCategoria servicoCategoria, 
    IMapper mapeador) : Controller
{
    [HttpGet]
    public ActionResult Listar()
    {
        List<ListarDespesaDto> dtos = servicoDespesa.SelecionarTodos();

        List<ListarDespesaViewModel> listarVms = mapeador.Map<List<ListarDespesaViewModel>>(dtos);

        return View(listarVms);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        CarregarCategoriasEFormasPagamento();
        
        CadastrarDespesaViewModel cadastrarVm = new CadastrarDespesaViewModel();
        return View(cadastrarVm);
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarDespesaViewModel cadastrarVm)
    {
        if (!ModelState.IsValid)
        {
            CarregarCategoriasEFormasPagamento();
            return View(cadastrarVm);
        }

        CadastrarDespesaDto dto = mapeador.Map<CadastrarDespesaDto>(cadastrarVm);
        Result resultado = servicoDespesa.Cadastrar(dto);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            CarregarCategoriasEFormasPagamento();
            return View(cadastrarVm);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Editar(Guid id)
    {
        Result<DetalhesDespesaDto> resultado = servicoDespesa.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            TempData.AddErrorMessage(resultado);
            return RedirectToAction(nameof(Listar));
        }

        DetalhesDespesaDto dto = resultado.Value;
        EditarDespesaViewModel editarVm = mapeador.Map<EditarDespesaViewModel>(dto);

        CarregarCategoriasEFormasPagamento();
        return View(editarVm);
    }

    [HttpPost]
    public ActionResult Editar(EditarDespesaViewModel editarVm)
    {
        if (!ModelState.IsValid)
        {
            CarregarCategoriasEFormasPagamento();
            return View(editarVm);
        }

        EditarDespesaDto dto = mapeador.Map<EditarDespesaDto>(editarVm);
        Result resultado = servicoDespesa.Editar(dto);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            CarregarCategoriasEFormasPagamento();
            return View(editarVm);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Excluir(Guid id)
    {
        Result<DetalhesDespesaDto> resultado = servicoDespesa.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            TempData.AddErrorMessage(resultado);
            return RedirectToAction(nameof(Listar));
        }

        DetalhesDespesaDto dto = resultado.Value;
        ExcluirDespesaViewModel excluirVm = mapeador.Map<ExcluirDespesaViewModel>(dto);

        return View(excluirVm);
    }

    [HttpPost]
    public ActionResult Excluir(ExcluirDespesaViewModel excluirVm)
    {
        Result resultado = servicoDespesa.Excluir(excluirVm.Id);

        if (resultado.IsFailed)
            TempData.AddErrorMessage(resultado);

        return RedirectToAction(nameof(Listar));
    }

    private void CarregarCategoriasEFormasPagamento()
    {
       
        var categorias = servicoCategoria.SelecionarTodos();
        ViewBag.Categorias = categorias.Select(c => new SelectListItem(c.Titulo, c.Id.ToString()));

       
        ViewBag.FormasPagamento = Enum.GetValues<FormaPagamento>()
            .Select(f => new SelectListItem(f.ToString(), ((int)f).ToString()));
    }
    
}