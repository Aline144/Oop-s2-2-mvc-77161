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
    private readonly ILogger<InspectionController> _logger;

    public InspectionController(ApplicationDbContext context, ILogger<InspectionController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [Authorize(Roles = $"{Roles.Admin},{Roles.Inspector},{Roles.Viewer}")]
    public async Task<IActionResult> Index()
    {
        _logger.LogInformation("Inspection list viewed by user {UserName}", User.Identity?.Name);

        var inspections = _context.Inspections.Include(i => i.Premises);
        return View(await inspections.ToListAsync());
    }

    [Authorize(Roles = $"{Roles.Admin},{Roles.Inspector},{Roles.Viewer}")]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            _logger.LogWarning("Inspection details requested with null id by user {UserName}", User.Identity?.Name);
            return NotFound();
        }

        var inspection = await _context.Inspections
            .Include(i => i.Premises)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (inspection == null)
        {
            _logger.LogWarning("Inspection details not found for id {InspectionId} requested by user {UserName}", id, User.Identity?.Name);
            return NotFound();
        }

        _logger.LogInformation("Inspection details viewed for id {InspectionId} by user {UserName}", id, User.Identity?.Name);
        return View(inspection);
    }

    [Authorize(Roles = $"{Roles.Admin},{Roles.Inspector}")]
    public IActionResult Create()
    {
        _logger.LogInformation("Inspection create page opened by user {UserName}", User.Identity?.Name);

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

            _logger.LogInformation(
                "Inspection created with id {InspectionId} for premises id {PremisesId} by user {UserName}",
                inspection.Id, inspection.PremisesId, User.Identity?.Name);

            return RedirectToAction(nameof(Index));
        }

        _logger.LogWarning("Invalid inspection create attempt by user {UserName}", User.Identity?.Name);

        ViewData["PremisesId"] = new SelectList(_context.Premises, "Id", "Name", inspection.PremisesId);
        return View(inspection);
    }

    [Authorize(Roles = $"{Roles.Admin},{Roles.Inspector}")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            _logger.LogWarning("Inspection edit requested with null id by user {UserName}", User.Identity?.Name);
            return NotFound();
        }

        var inspection = await _context.Inspections.FindAsync(id);
        if (inspection == null)
        {
            _logger.LogWarning("Inspection edit not found for id {InspectionId} requested by user {UserName}", id, User.Identity?.Name);
            return NotFound();
        }

        _logger.LogInformation("Inspection edit page opened for id {InspectionId} by user {UserName}", id, User.Identity?.Name);

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
            _logger.LogWarning(
                "Inspection edit id mismatch. Route id {RouteId}, model id {ModelId}, user {UserName}",
                id, inspection.Id, User.Identity?.Name);
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(inspection);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Inspection updated for id {InspectionId} by user {UserName}",
                    inspection.Id, User.Identity?.Name);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!InspectionExists(inspection.Id))
                {
                    _logger.LogWarning("Inspection update failed because id {InspectionId} was not found for user {UserName}",
                        inspection.Id, User.Identity?.Name);
                    return NotFound();
                }

                _logger.LogError(ex, "Concurrency error updating inspection id {InspectionId} by user {UserName}",
                    inspection.Id, User.Identity?.Name);
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        _logger.LogWarning("Invalid inspection edit attempt for id {InspectionId} by user {UserName}",
            inspection.Id, User.Identity?.Name);

        ViewData["PremisesId"] = new SelectList(_context.Premises, "Id", "Name", inspection.PremisesId);
        return View(inspection);
    }

    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            _logger.LogWarning("Inspection delete requested with null id by user {UserName}", User.Identity?.Name);
            return NotFound();
        }

        var inspection = await _context.Inspections
            .Include(i => i.Premises)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (inspection == null)
        {
            _logger.LogWarning("Inspection delete not found for id {InspectionId} requested by user {UserName}", id, User.Identity?.Name);
            return NotFound();
        }

        _logger.LogInformation("Inspection delete page opened for id {InspectionId} by user {UserName}", id, User.Identity?.Name);
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

            _logger.LogInformation("Inspection deleted for id {InspectionId} by user {UserName}",
                inspection.Id, User.Identity?.Name);
        }
        else
        {
            _logger.LogWarning("Inspection delete confirmed but id {InspectionId} was not found for user {UserName}",
                id, User.Identity?.Name);
        }

        return RedirectToAction(nameof(Index));
    }

    private bool InspectionExists(int id)
    {
        return _context.Inspections.Any(e => e.Id == id);
    }
}