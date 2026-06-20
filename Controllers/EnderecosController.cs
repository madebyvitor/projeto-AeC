using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projetoAeC.Data;
using projetoAeC.Models;
using projetoAeC.ViewModels.Enderecos;
using System.Security.Claims;
using System.Text;

namespace projetoAeC.Controllers;

[Authorize]
public class EnderecosController : Controller
{
    private readonly ApplicationDbContext _context;

    public EnderecosController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var usuarioId = GetUsuarioId();
        var enderecos = await _context.Enderecos
            .Where(endereco => endereco.UsuarioId == usuarioId)
            .OrderBy(endereco => endereco.Cidade)
            .ThenBy(endereco => endereco.Logradouro)
            .ToListAsync();

        return View(enderecos);
    }

    public async Task<IActionResult> Details(int id)
    {
        var endereco = await BuscarEnderecoDoUsuarioAsync(id);

        if (endereco is null)
        {
            return NotFound();
        }

        return View(endereco);
    }

    public IActionResult Create()
    {
        return View(new EnderecoViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EnderecoViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var endereco = new Endereco
        {
            UsuarioId = GetUsuarioId()
        };

        AtualizarEndereco(endereco, model);

        _context.Enderecos.Add(endereco);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Endereço cadastrado com sucesso.";

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var endereco = await BuscarEnderecoDoUsuarioAsync(id);

        if (endereco is null)
        {
            return NotFound();
        }

        return View(MapearViewModel(endereco));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EnderecoViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        var endereco = await BuscarEnderecoDoUsuarioAsync(id);

        if (endereco is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        AtualizarEndereco(endereco, model);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Endereço atualizado com sucesso.";

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var endereco = await BuscarEnderecoDoUsuarioAsync(id);

        if (endereco is null)
        {
            return NotFound();
        }

        return View(endereco);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var endereco = await BuscarEnderecoDoUsuarioAsync(id);

        if (endereco is null)
        {
            return NotFound();
        }

        _context.Enderecos.Remove(endereco);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Endereço excluído com sucesso.";

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> ExportCsv()
    {
        var usuarioId = GetUsuarioId();
        var enderecos = await _context.Enderecos
            .Where(endereco => endereco.UsuarioId == usuarioId)
            .OrderBy(endereco => endereco.Cidade)
            .ThenBy(endereco => endereco.Logradouro)
            .ToListAsync();

        var csv = new StringBuilder();
        csv.AppendLine("CEP,Logradouro,Numero,Complemento,Bairro,Cidade,UF,IBGE");

        foreach (var endereco in enderecos)
        {
            csv.AppendLine(string.Join(",", new[]
            {
                FormatarCampoCsv(endereco.Cep),
                FormatarCampoCsv(endereco.Logradouro),
                FormatarCampoCsv(endereco.Numero),
                FormatarCampoCsv(endereco.Complemento),
                FormatarCampoCsv(endereco.Bairro),
                FormatarCampoCsv(endereco.Cidade),
                FormatarCampoCsv(endereco.Uf),
                FormatarCampoCsv(endereco.Ibge)
            }));
        }

        var conteudo = Encoding.UTF8.GetPreamble()
            .Concat(Encoding.UTF8.GetBytes(csv.ToString()))
            .ToArray();

        return File(conteudo, "text/csv; charset=utf-8", $"enderecos-{DateTime.UtcNow:yyyyMMddHHmmss}.csv");
    }

    private int GetUsuarioId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    private Task<Endereco?> BuscarEnderecoDoUsuarioAsync(int id)
    {
        var usuarioId = GetUsuarioId();

        return _context.Enderecos
            .SingleOrDefaultAsync(endereco => endereco.Id == id && endereco.UsuarioId == usuarioId);
    }

    private static EnderecoViewModel MapearViewModel(Endereco endereco)
    {
        return new EnderecoViewModel
        {
            Id = endereco.Id,
            Cep = endereco.Cep,
            Logradouro = endereco.Logradouro,
            Numero = endereco.Numero,
            Complemento = endereco.Complemento,
            Bairro = endereco.Bairro,
            Cidade = endereco.Cidade,
            Uf = endereco.Uf,
            Ibge = endereco.Ibge
        };
    }

    private static void AtualizarEndereco(Endereco endereco, EnderecoViewModel model)
    {
        endereco.Cep = model.Cep.Trim();
        endereco.Logradouro = model.Logradouro.Trim();
        endereco.Numero = model.Numero.Trim();
        endereco.Complemento = string.IsNullOrWhiteSpace(model.Complemento) ? null : model.Complemento.Trim();
        endereco.Bairro = model.Bairro.Trim();
        endereco.Cidade = model.Cidade.Trim();
        endereco.Uf = model.Uf.Trim().ToUpperInvariant();
        endereco.Ibge = string.IsNullOrWhiteSpace(model.Ibge) ? null : model.Ibge.Trim();
    }

    private static string FormatarCampoCsv(string? valor)
    {
        return $"\"{(valor ?? string.Empty).Replace("\"", "\"\"")}\"";
    }
}
