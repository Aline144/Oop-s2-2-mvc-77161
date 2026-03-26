using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodSafetyInspectionTracker.Constants;
using FoodSafetyInspectionTracker.Data;
using FoodSafetyInspectionTracker.Models;

namespace FoodSafetyInspectionTracker.Controllers;

[Authorize]
public class FollowUpController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<FollowUpController> _logger;

    public FollowUpController(ApplicationDbContext context, ILogger<FollowUpController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [Authorize(Roles = $"{Roles.Admin},{Roles.Inspector},{Roles.Viewer}")]
    public async Task<IActionResult> Index()
    {
        _logger.LogInformation("FollowUp list viewed by user {UserName}", User.Identity?.Name);

        var followUps = _context.FollowUps
            .Include(f => f.Inspection)
            .ThenInclude(i => i.Premises);

        return View(await followUps.ToListAsync());
    }

    [Authorize(Roles = $"{Roles.Admin},{Roles.Inspector},{Roles.Viewer}")]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            _logger.LogWarning("FollowUp details requested with null id by user {UserName}", User.Identity?.Name);
            return NotFound();
        }

        var followUp = await _context.FollowUps
            .Include(f => f.Inspection)
            .ThenInclude(i => i.Premises)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (followUp == null)
        {
            _logger.LogWarning("FollowUp details not found for id {FollowUpId} requested by user {UserName}", id, User.Identity?.Name);
            return NotFound();
        }

        _logger.LogInformation("FollowUp details viewed for id {FollowUpId} by user {UserName}", id, User.Identity?.Name);
        return View(followUp);
    }

    [Authorize(Roles = $"{Roles.Admin},{Roles.Inspector}")]
    public IActionResult Create()
    {
        _logger.LogInformation("FollowUp create page opened by user {UserName}", User.Identity?.Name);

        ViewData["InspectionId"] = new SelectList(
            _context.Inspections.Include(i => i.Premises).ToList()
                .Select(i => new
                {
                    i.Id,
                    Display = $"{i.Id} - {i.Premises!.Name} ({i.InspectionDate:dd/MM/yyyy})"
                }),
            "Id",
            "Display");

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Inspector}")]
    public async Task<IActionResult> Create(FollowUp followUp)
    {
        if (ModelState.IsValid)
        {
            _context.Add(followUp);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "FollowUp created with id {FollowUpId} for inspection id {InspectionId} by user {UserName}",
                followUp.Id, followUp.InspectionId, User.Identity?.Name);

            return RedirectToAction(nameof(Index));
        }

        _logger.LogWarning("Invalid FollowUp create attempt by user {UserName}", User.Identity?.Name);

        ViewData["InspectionId"] = new SelectList(
            _context.Inspections.Include(i => i.Premises).ToList()
                .Select(i => new
                {
                    i.Id,
                    Display = $"{i.Id} - {i.Premises!.Name} ({i.InspectionDate:dd/MM/yyyy})"
                }),
            "Id",
            "Display",
            followUp.InspectionId);

        return View(followUp);
    }

    [Authorize(Roles = $"{Roles.Admin},{Roles.Inspector}")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            _logger.LogWarning("FollowUp edit requested with null id by user {UserName}", User.Identity?.Name);
            return NotFound();
        }

        var followUp = await _context.FollowUps.FindAsync(id);
        if (followUp == null)
        {
            _logger.LogWarning("FollowUp edit not found for id {FollowUpId} requested by user {UserName}", id, User.Identity?.Name);
            return NotFound();
        }

        _logger.LogInformation("FollowUp edit page opened for id {FollowUpId} by user {UserName}", id, User.Identity?.Name);

        ViewData["InspectionId"] = new SelectList(
            _context.Inspections.Include(i => i.Premises).ToList()
                .Select(i => new
                {
                    i.Id,
                    Display = $"{i.Id} - {i.Premises!.Name} ({i.InspectionDate:dd/MM/yyyy})"
                }),
            "Id",
            "Display",
            followUp.InspectionId);

        return View(followUp);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Inspector}")]
    public async Task<IActionResult> Edit(int id, FollowUp followUp)
    {
        if (id != followUp.Id)
        {
            _logger.LogWarning(
                "FollowUp edit id mismatch. Route id {RouteId}, model id {ModelId}, user {UserName}",
                id, followUp.Id, User.Identity?.Name);
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(followUp);
                await _context.SaveChangesAsync();

                _logger.LogInformation("FollowUp updated for id {FollowUpId} by user {UserName}",
                    followUp.Id, User.Identity?.Name);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!FollowUpExists(followUp.Id))
                {
                    _logger.LogWarning("FollowUp update failed because id {FollowUpId} was not found for user {UserName}",
                        followUp.Id, User.Identity?.Name);
                    return NotFound();
                }

                _logger.LogError(ex, "Concurrency error updating FollowUp id {FollowUpId} by user {UserName}",
                    followUp.Id, User.Identity?.Name);
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        _logger.LogWarning("Invalid FollowUp edit attempt for id {FollowUpId} by user {UserName}",
            followUp.Id, User.Identity?.Name);

        ViewData["InspectionId"] = new SelectList(
            _context.Inspections.Include(i => i.Premises).ToList()
                .Select(i => new
                {
                    i.Id,
                    Display = $"{i.Id} - {i.Premises!.Name} ({i.InspectionDate:dd/MM/yyyy})"
                }),
            "Id",
            "Display",
            followUp.InspectionId);

        return View(followUp);
    }

    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            _logger.LogWarning("FollowUp delete requested with null id by user {UserName}", User.Identity?.Name);
            return NotFound();
        }

        var followUp = await _context.FollowUps
            .Include(f => f.Inspection)
            .ThenInclude(i => i.Premises)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (followUp == null)
        {
            _logger.LogWarning("FollowUp delete not found for id {FollowUpId} requested by user {UserName}", id, User.Identity?.Name);
            return NotFound();
        }

        _logger.LogInformation("FollowUp delete page opened for id {FollowUpId} by user {UserName}", id, User.Identity?.Name);
        return View(followUp);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var followUp = await _context.FollowUps.FindAsync(id);
        if (followUp != null)
        {
            _context.FollowUps.Remove(followUp);
            await _context.SaveChangesAsync();

            _logger.LogInformation("FollowUp deleted for id {FollowUpId} by user {UserName}",
                followUp.Id, User.Identity?.Name);
        }
        else
        {
            _logger.LogWarning("FollowUp delete confirmed but id {FollowUpId} was not found for user {UserName}",
                id, User.Identity?.Name);
        }

        return RedirectToAction(nameof(Index));
    }

    private bool FollowUpExists(int id)
    {
        return _context.FollowUps.Any(e => e.Id == id);
    }
}   