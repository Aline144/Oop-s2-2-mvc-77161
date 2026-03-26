using FoodSafetyInspectionTracker.Models.Enums;

namespace FoodSafetyInspectionTracker.ViewModels;

public class DashboardViewModel
{
    public int InspectionsThisMonth { get; set; }
    public int FailedInspectionsThisMonth { get; set; }
    public int OpenFollowUps { get; set; }
    public int OverdueFollowUps { get; set; }

    public string? SelectedTown { get; set; }
    public RiskRating? SelectedRiskRating { get; set; }

    public List<string> Towns { get; set; } = new();
}