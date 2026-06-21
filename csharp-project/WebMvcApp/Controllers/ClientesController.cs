using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMvcApp.Data;
using WebMvcApp.Models;

namespace WebMvcApp.Controllers;

// ======================================================================
// CRUD de Clientes contra el contexto SQL Server.
// (Equivalente al codigo que generaria el scaffolding automatico de
//  controladores y vistas de ASP.NET Core MVC.)
// ======================================================================
public class ClientesController : Controller
{
    private readonly SqlServerContext _context;

    public ClientesController(SqlServerContext context)
    {
        _context = context;
    }

    // GET: Clientes
    public async Task<IActionResult> Index()
    {
        return View(await _context.Clientes.ToListAsync());
    }

    // GET: Clientes/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var cliente = await _context.Clientes
            .FirstOrDefaultAsync(m => m.ClienteId == id);
        if (cliente == null) return NotFound();
        return View(cliente);
    }

    // GET: Clientes/Create
    public IActionResult Create() => View();

    // POST: Clientes/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Nombre,Email,Telefono,Ciudad,FechaRegistro")] Cliente cliente)
    {
        if (ModelState.IsValid)
        {
            _context.Add(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(cliente);
    }

    // GET: Clientes/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente == null) return NotFound();
        return View(cliente);
    }

    // POST: Clientes/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind("ClienteId,Nombre,Email,Telefono,Ciudad,FechaRegistro")] Cliente cliente)
    {
        if (id != cliente.ClienteId) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(cliente);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Clientes.Any(e => e.ClienteId == cliente.ClienteId))
                    return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(cliente);
    }

    // GET: Clientes/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var cliente = await _context.Clientes
            .FirstOrDefaultAsync(m => m.ClienteId == id);
        if (cliente == null) return NotFound();
        return View(cliente);
    }

    // POST: Clientes/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente != null)
        {
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
