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

    public FollowUpController(ApplicationDbContext context)
    {
        _context = context;
    }

    [Authorize(Roles = $"{Roles.Admin},{Roles.Inspector},{Roles.Viewer}")]
    public async Task<IActionResult> Index()
    {
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
            return NotFound();
        }

        var followUp = await _context.FollowUps
            .Include(f => f.Inspection)
            .ThenInclude(i => i.Premises)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (followUp == null)
        {
            return NotFound();
        }

        return View(followUp);
    }

    [Authorize(Roles = $"{Roles.Admin},{Roles.Inspector}")]
    public IActionResult Create()
    {
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
            return RedirectToAction(nameof(Index));
        }

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
            return NotFound();
        }

        var followUp = await _context.FollowUps.FindAsync(id);
        if (followUp == null)
        {
            return NotFound();
        }

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
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(followUp);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FollowUpExists(followUp.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

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
            return NotFound();
        }

        var followUp = await _context.FollowUps
            .Include(f => f.Inspection)
            .ThenInclude(i => i.Premises)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (followUp == null)
        {
            return NotFound();
        }

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
        }

        return RedirectToAction(nameof(Index));
    }

    private bool FollowUpExists(int id)
    {
        return _context.FollowUps.Any(e => e.Id == id);
    }
}