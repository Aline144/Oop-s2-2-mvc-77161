using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodSafetyInspectionTracker.Constants;
using FoodSafetyInspectionTracker.Data;
using FoodSafetyInspectionTracker.Models;

namespace FoodSafetyInspectionTracker.Controllers;

[Authorize]
public class PremisesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PremisesController> _logger;

    public PremisesController(ApplicationDbContext context, ILogger<PremisesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [Authorize(Roles = $"{Roles.Admin},{Roles.Inspector},{Roles.Viewer}")]
    public async Task<IActionResult> Index()
    {
        _logger.LogInformation("Premises list viewed by user {UserName}", User.Identity?.Name);

        var premises = await _context.Premises.ToListAsync();
        return View(premises);
    }

    [Authorize(Roles = $"{Roles.Admin},{Roles.Inspector},{Roles.Viewer}")]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            _logger.LogWarning("Premises details requested with null id by user {UserName}", User.Identity?.Name);
            return NotFound();
        }

        var premises = await _context.Premises
            .FirstOrDefaultAsync(m => m.Id == id);

        if (premises == null)
        {
            _logger.LogWarning("Premises details not found for id {PremisesId} requested by user {UserName}", id, User.Identity?.Name);
            return NotFound();
        }

        _logger.LogInformation("Premises details viewed for id {PremisesId} by user {UserName}", id, User.Identity?.Name);
        return View(premises);
    }

    [Authorize(Roles = $"{Roles.Admin},{Roles.Inspector}")]
    public IActionResult Create()
    {
        _logger.LogInformation("Premises create page opened by user {UserName}", User.Identity?.Name);
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Inspector}")]
    public async Task<IActionResult> Create(Premises premises)
    {
        if (ModelState.IsValid)
        {
            _context.Add(premises);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Premises created with id {PremisesId}, name {PremisesName} by user {UserName}",
                premises.Id, premises.Name, User.Identity?.Name);

            return RedirectToAction(nameof(Index));
        }

        _logger.LogWarning("Invalid premises create attempt by user {UserName}", User.Identity?.Name);
        return View(premises);
    }

    [Authorize(Roles = $"{Roles.Admin},{Roles.Inspector}")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            _logger.LogWarning("Premises edit requested with null id by user {UserName}", User.Identity?.Name);
            return NotFound();
        }

        var premises = await _context.Premises.FindAsync(id);
        if (premises == null)
        {
            _logger.LogWarning("Premises edit not found for id {PremisesId} requested by user {UserName}", id, User.Identity?.Name);
            return NotFound();
        }

        _logger.LogInformation("Premises edit page opened for id {PremisesId} by user {UserName}", id, User.Identity?.Name);
        return View(premises);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Inspector}")]
    public async Task<IActionResult> Edit(int id, Premises premises)
    {
        if (id != premises.Id)
        {
            _logger.LogWarning("Premises edit id mismatch. Route id {RouteId}, model id {ModelId}, user {UserName}",
                id, premises.Id, User.Identity?.Name);
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(premises);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Premises updated for id {PremisesId} by user {UserName}",
                    premises.Id, User.Identity?.Name);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!PremisesExists(premises.Id))
                {
                    _logger.LogWarning("Premises update failed because id {PremisesId} was not found for user {UserName}",
                        premises.Id, User.Identity?.Name);
                    return NotFound();
                }

                _logger.LogError(ex, "Concurrency error updating premises id {PremisesId} by user {UserName}",
                    premises.Id, User.Identity?.Name);
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        _logger.LogWarning("Invalid premises edit attempt for id {PremisesId} by user {UserName}",
            premises.Id, User.Identity?.Name);

        return View(premises);
    }

    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            _logger.LogWarning("Premises delete requested with null id by user {UserName}", User.Identity?.Name);
            return NotFound();
        }

        var premises = await _context.Premises
            .FirstOrDefaultAsync(m => m.Id == id);

        if (premises == null)
        {
            _logger.LogWarning("Premises delete not found for id {PremisesId} requested by user {UserName}", id, User.Identity?.Name);
            return NotFound();
        }

        _logger.LogInformation("Premises delete page opened for id {PremisesId} by user {UserName}", id, User.Identity?.Name);
        return View(premises);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var premises = await _context.Premises.FindAsync(id);
        if (premises != null)
        {
            _context.Premises.Remove(premises);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Premises deleted for id {PremisesId}, name {PremisesName} by user {UserName}",
                premises.Id, premises.Name, User.Identity?.Name);
        }
        else
        {
            _logger.LogWarning("Premises delete confirmed but id {PremisesId} was not found for user {UserName}",
                id, User.Identity?.Name);
        }

        return RedirectToAction(nameof(Index));
    }

    private bool PremisesExists(int id)
    {
        return _context.Premises.Any(e => e.Id == id);
    }
}