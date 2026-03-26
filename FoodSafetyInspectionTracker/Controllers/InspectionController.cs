using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodSafetyInspectionTracker.Constants;
using FoodSafetyInspectionTracker.Data;
using FoodSafetyInspectionTracker.Models;

namespace FoodSafetyInspectionTracker.Controllers;

[Authorize]
public class InspectionController : Controller
{
    private readonly ApplicationDbContext _context;

    public InspectionController(ApplicationDbContext context)
    {
        _context = context;
    }

    [Authorize(Roles = $"{Roles.Admin},{Roles.Inspector},{Roles.Viewer}")]
    public async Task<IActionResult> Index()
    {
        var inspections = _context.Inspections.Include(i => i.Premises);
        return View(await inspections.ToListAsync());
    }

    [Authorize(Roles = $"{Roles.Admin},{Roles.Inspector},{Roles.Viewer}")]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var inspection = await _context.Inspections
            .Include(i => i.Premises)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (inspection == null)
        {
            return NotFound();
        }

        return View(inspection);
    }

    [Authorize(Roles = $"{Roles.Admin},{Roles.Inspector}")]
    public IActionResult Create()
    {
        ViewData["PremisesId"] = new SelectList(_context.Premises, "Id", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Inspector}")]
    public async Task<IActionResult> Create(Inspection inspection)
    {
        if (ModelState.IsValid)
        {
            _context.Add(inspection);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["PremisesId"] = new SelectList(_context.Premises, "Id", "Name", inspection.PremisesId);
        return View(inspection);
    }

    [Authorize(Roles = $"{Roles.Admin},{Roles.Inspector}")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var inspection = await _context.Inspections.FindAsync(id);
        if (inspection == null)
        {
            return NotFound();
        }

        ViewData["PremisesId"] = new SelectList(_context.Premises, "Id", "Name", inspection.PremisesId);
        return View(inspection);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Inspector}")]
    public async Task<IActionResult> Edit(int id, Inspection inspection)
    {
        if (id != inspection.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(inspection);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InspectionExists(inspection.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["PremisesId"] = new SelectList(_context.Premises, "Id", "Name", inspection.PremisesId);
        return View(inspection);
    }

    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var inspection = await _context.Inspections
            .Include(i => i.Premises)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (inspection == null)
        {
            return NotFound();
        }

        return View(inspection);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var inspection = await _context.Inspections.FindAsync(id);
        if (inspection != null)
        {
            _context.Inspections.Remove(inspection);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool InspectionExists(int id)
    {
        return _context.Inspections.Any(e => e.Id == id);
    }
}