using FoodSafetyInspectionTracker.Models;
using FoodSafetyInspectionTracker.Models.Enums;

namespace FoodSafetyInspectionTracker.Data;

public static class SeedData
{
    public static List<Premises> GetPremises() => new()
    {
        new Premises { Id = 1, Name = "Green Farm Cafe", Address = "12 Market Street", Town = "Dublin", RiskRating = RiskRating.Low },
        new Premises { Id = 2, Name = "River Bistro", Address = "45 River Road", Town = "Cork", RiskRating = RiskRating.Medium },
        new Premises { Id = 3, Name = "Ocean View Restaurant", Address = "8 Harbour Lane", Town = "Galway", RiskRating = RiskRating.High },
        new Premises { Id = 4, Name = "Sunny Bakery", Address = "21 Main Street", Town = "Limerick", RiskRating = RiskRating.Low },
        new Premises { Id = 5, Name = "Hilltop Diner", Address = "9 Mountain Road", Town = "Waterford", RiskRating = RiskRating.Medium },
        new Premises { Id = 6, Name = "Fresh Bites", Address = "30 King Street", Town = "Dublin", RiskRating = RiskRating.High },
        new Premises { Id = 7, Name = "City Grill", Address = "14 Queen Avenue", Town = "Cork", RiskRating = RiskRating.Low },
        new Premises { Id = 8, Name = "Golden Spoon", Address = "67 Oak Drive", Town = "Galway", RiskRating = RiskRating.Medium },
        new Premises { Id = 9, Name = "Happy Meals Cafe", Address = "3 Pine Street", Town = "Limerick", RiskRating = RiskRating.High },
        new Premises { Id = 10, Name = "Urban Kitchen", Address = "19 Central Road", Town = "Waterford", RiskRating = RiskRating.Low },
        new Premises { Id = 11, Name = "Taste Corner", Address = "50 Bridge Street", Town = "Dublin", RiskRating = RiskRating.Medium },
        new Premises { Id = 12, Name = "Family Table", Address = "88 Lake View", Town = "Cork", RiskRating = RiskRating.High }
    };

    public static List<Inspection> GetInspections() => new()
    {
        new Inspection { Id = 1, PremisesId = 1, InspectionDate = new DateTime(2026, 1, 10), Score = 92, Outcome = InspectionOutcome.Pass, Notes = "Clean and well maintained." },
        new Inspection { Id = 2, PremisesId = 2, InspectionDate = new DateTime(2026, 1, 12), Score = 75, Outcome = InspectionOutcome.ImprovementRequired, Notes = "Minor hygiene issues." },
        new Inspection { Id = 3, PremisesId = 3, InspectionDate = new DateTime(2026, 1, 15), Score = 48, Outcome = InspectionOutcome.Fail, Notes = "Serious food storage issue." },
        new Inspection { Id = 4, PremisesId = 4, InspectionDate = new DateTime(2026, 1, 18), Score = 88, Outcome = InspectionOutcome.Pass, Notes = "Good record keeping." },
        new Inspection { Id = 5, PremisesId = 5, InspectionDate = new DateTime(2026, 1, 20), Score = 68, Outcome = InspectionOutcome.ImprovementRequired, Notes = "Cleaning schedule incomplete." },
        new Inspection { Id = 6, PremisesId = 6, InspectionDate = new DateTime(2026, 1, 22), Score = 40, Outcome = InspectionOutcome.Fail, Notes = "Cross-contamination risk found." },
        new Inspection { Id = 7, PremisesId = 7, InspectionDate = new DateTime(2026, 1, 24), Score = 95, Outcome = InspectionOutcome.Pass, Notes = "Excellent compliance." },
        new Inspection { Id = 8, PremisesId = 8, InspectionDate = new DateTime(2026, 1, 25), Score = 71, Outcome = InspectionOutcome.ImprovementRequired, Notes = "Temperature log missing." },
        new Inspection { Id = 9, PremisesId = 9, InspectionDate = new DateTime(2026, 1, 26), Score = 52, Outcome = InspectionOutcome.Fail, Notes = "Poor pest control evidence." },
        new Inspection { Id = 10, PremisesId = 10, InspectionDate = new DateTime(2026, 1, 27), Score = 89, Outcome = InspectionOutcome.Pass, Notes = "No major issues." },
        new Inspection { Id = 11, PremisesId = 11, InspectionDate = new DateTime(2026, 2, 1), Score = 73, Outcome = InspectionOutcome.ImprovementRequired, Notes = "Staff training records incomplete." },
        new Inspection { Id = 12, PremisesId = 12, InspectionDate = new DateTime(2026, 2, 2), Score = 45, Outcome = InspectionOutcome.Fail, Notes = "Unsanitary preparation area." },
        new Inspection { Id = 13, PremisesId = 1, InspectionDate = new DateTime(2026, 2, 5), Score = 94, Outcome = InspectionOutcome.Pass, Notes = "High standard maintained." },
        new Inspection { Id = 14, PremisesId = 2, InspectionDate = new DateTime(2026, 2, 7), Score = 78, Outcome = InspectionOutcome.ImprovementRequired, Notes = "Storage labels missing." },
        new Inspection { Id = 15, PremisesId = 3, InspectionDate = new DateTime(2026, 2, 10), Score = 50, Outcome = InspectionOutcome.Fail, Notes = "Repeat critical issue." },
        new Inspection { Id = 16, PremisesId = 4, InspectionDate = new DateTime(2026, 2, 12), Score = 91, Outcome = InspectionOutcome.Pass, Notes = "Very good hygiene." },
        new Inspection { Id = 17, PremisesId = 5, InspectionDate = new DateTime(2026, 2, 14), Score = 69, Outcome = InspectionOutcome.ImprovementRequired, Notes = "Follow cleaning checklist." },
        new Inspection { Id = 18, PremisesId = 6, InspectionDate = new DateTime(2026, 2, 15), Score = 43, Outcome = InspectionOutcome.Fail, Notes = "Improper chilled storage." },
        new Inspection { Id = 19, PremisesId = 7, InspectionDate = new DateTime(2026, 2, 18), Score = 96, Outcome = InspectionOutcome.Pass, Notes = "Excellent documentation." },
        new Inspection { Id = 20, PremisesId = 8, InspectionDate = new DateTime(2026, 2, 20), Score = 74, Outcome = InspectionOutcome.ImprovementRequired, Notes = "Corrective action needed." },
        new Inspection { Id = 21, PremisesId = 9, InspectionDate = new DateTime(2026, 2, 22), Score = 49, Outcome = InspectionOutcome.Fail, Notes = "Multiple non-compliances." },
        new Inspection { Id = 22, PremisesId = 10, InspectionDate = new DateTime(2026, 2, 24), Score = 90, Outcome = InspectionOutcome.Pass, Notes = "Compliant." },
        new Inspection { Id = 23, PremisesId = 11, InspectionDate = new DateTime(2026, 2, 25), Score = 76, Outcome = InspectionOutcome.ImprovementRequired, Notes = "Food label update required." },
        new Inspection { Id = 24, PremisesId = 12, InspectionDate = new DateTime(2026, 2, 27), Score = 47, Outcome = InspectionOutcome.Fail, Notes = "Cleaning failures continue." },
        new Inspection { Id = 25, PremisesId = 6, InspectionDate = new DateTime(2026, 3, 1), Score = 82, Outcome = InspectionOutcome.Pass, Notes = "Improvement observed." }
    };

    public static List<FollowUp> GetFollowUps() => new()
    {
        new FollowUp { Id = 1, InspectionId = 3, DueDate = new DateTime(2026, 1, 25), Status = FollowUpStatus.Open, ClosedDate = null },
        new FollowUp { Id = 2, InspectionId = 6, DueDate = new DateTime(2026, 2, 1), Status = FollowUpStatus.Closed, ClosedDate = new DateTime(2026, 1, 30) },
        new FollowUp { Id = 3, InspectionId = 9, DueDate = new DateTime(2026, 2, 5), Status = FollowUpStatus.Overdue, ClosedDate = null },
        new FollowUp { Id = 4, InspectionId = 12, DueDate = new DateTime(2026, 2, 12), Status = FollowUpStatus.Open, ClosedDate = null },
        new FollowUp { Id = 5, InspectionId = 15, DueDate = new DateTime(2026, 2, 18), Status = FollowUpStatus.Closed, ClosedDate = new DateTime(2026, 2, 17) },
        new FollowUp { Id = 6, InspectionId = 18, DueDate = new DateTime(2026, 2, 22), Status = FollowUpStatus.Open, ClosedDate = null },
        new FollowUp { Id = 7, InspectionId = 21, DueDate = new DateTime(2026, 2, 28), Status = FollowUpStatus.Overdue, ClosedDate = null },
        new FollowUp { Id = 8, InspectionId = 24, DueDate = new DateTime(2026, 3, 5), Status = FollowUpStatus.Open, ClosedDate = null },
        new FollowUp { Id = 9, InspectionId = 5, DueDate = new DateTime(2026, 1, 28), Status = FollowUpStatus.Closed, ClosedDate = new DateTime(2026, 1, 27) },
        new FollowUp { Id = 10, InspectionId = 8, DueDate = new DateTime(2026, 2, 3), Status = FollowUpStatus.Open, ClosedDate = null }
    };
}