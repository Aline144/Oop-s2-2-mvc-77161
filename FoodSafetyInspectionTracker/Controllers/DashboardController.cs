using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodSafetyInspectionTracker.Constants;
using FoodSafetyInspectionTracker.Data;
using FoodSafetyInspectionTracker.Models.Enums;
using FoodSafetyInspectionTracker.ViewModels;

namespace FoodSafetyInspectionTracker.Controllers;

[Authorize(Roles = $"{Roles.Admin},{Roles.Inspector},{Roles.Viewer}")]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? selectedTown, RiskRating? selectedRiskRating)
    {
        var now = DateTime.Now;

        var inspectionsQuery = _context.Inspections
            .Include(i => i.Premises)
            .AsQueryable();

        var followUpsQuery = _context.FollowUps
            .Include(f => f.Inspection)
            .ThenInclude(i => i.Premises)
            .AsQueryable();

        if (!string.IsNullOrEmpty(selectedTown))
        {
            inspectionsQuery = inspectionsQuery.Where(i => i.Premises != null && i.Premises.Town == selectedTown);
            followUpsQuery = followUpsQuery.Where(f => f.Inspection != null &&
                                                       f.Inspection.Premises != null &&
                                                       f.Inspection.Premises.Town == selectedTown);
        }

        if (selectedRiskRating.HasValue)
        {
            inspectionsQuery = inspectionsQuery.Where(i => i.Premises != null && i.Premises.RiskRating == selectedRiskRating.Value);
            followUpsQuery = followUpsQuery.Where(f => f.Inspection != null &&
                                                       f.Inspection.Premises != null &&
                                                       f.Inspection.Premises.RiskRating == selectedRiskRating.Value);
        }

        var inspectionsThisMonth = await inspectionsQuery
            .CountAsync(i => i.InspectionDate.Month == now.Month && i.InspectionDate.Year == now.Year);

        var failedInspectionsThisMonth = await inspectionsQuery
            .CountAsync(i => i.InspectionDate.Month == now.Month &&
                             i.InspectionDate.Year == now.Year &&
                             i.Outcome == InspectionOutcome.Fail);

        var openFollowUps = await followUpsQuery
            .CountAsync(f => f.Status == FollowUpStatus.Open);

        var overdueFollowUps = await followUpsQuery
            .CountAsync(f => f.Status == FollowUpStatus.Overdue);

        var towns = await _context.Premises
            .Select(p => p.Town)
            .Distinct()
            .OrderBy(t => t)
            .ToListAsync();

        var viewModel = new DashboardViewModel
        {
            InspectionsThisMonth = inspectionsThisMonth,
            FailedInspectionsThisMonth = failedInspectionsThisMonth,
            OpenFollowUps = openFollowUps,
            OverdueFollowUps = overdueFollowUps,
            SelectedTown = selectedTown,
            SelectedRiskRating = selectedRiskRating,
            Towns = towns
        };

        return View(viewModel);
    }
}