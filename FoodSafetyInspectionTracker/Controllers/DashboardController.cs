using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FoodSafetyInspectionTracker.Constants;

namespace FoodSafetyInspectionTracker.Controllers;

[Authorize(Roles = $"{Roles.Admin},{Roles.Inspector},{Roles.Viewer}")]
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}