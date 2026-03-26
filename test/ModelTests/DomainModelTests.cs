using FoodSafetyInspectionTracker.Models;
using FoodSafetyInspectionTracker.Models.Enums;
using Xunit;

namespace FoodSafetyInspectionTracker.Tests.ModelTests;

public class DomainModelTests
{
    [Fact]
    public void Premises_CanStore_Name_Address_Town_And_RiskRating()
    {
        var premises = new Premises
        {
            Name = "Test Cafe",
            Address = "123 Main Street",
            Town = "Dublin",
            RiskRating = RiskRating.High
        };

        Assert.Equal("Test Cafe", premises.Name);
        Assert.Equal("123 Main Street", premises.Address);
        Assert.Equal("Dublin", premises.Town);
        Assert.Equal(RiskRating.High, premises.RiskRating);
    }

    [Fact]
    public void Inspection_CanBeLinked_ToPremises()
    {
        var premises = new Premises
        {
            Id = 1,
            Name = "River Bistro",
            Address = "45 River Road",
            Town = "Cork",
            RiskRating = RiskRating.Medium
        };

        var inspection = new Inspection
        {
            Id = 1,
            PremisesId = 1,
            Premises = premises,
            InspectionDate = new DateTime(2026, 1, 12),
            Score = 75,
            Outcome = InspectionOutcome.ImprovementRequired,
            Notes = "Minor hygiene issues."
        };

        Assert.Equal(1, inspection.PremisesId);
        Assert.NotNull(inspection.Premises);
        Assert.Equal("River Bistro", inspection.Premises!.Name);
    }

    [Fact]
    public void FollowUp_CanBeClosed_WithClosedDate()
    {
        var followUp = new FollowUp
        {
            InspectionId = 3,
            DueDate = new DateTime(2026, 1, 25),
            Status = FollowUpStatus.Closed,
            ClosedDate = new DateTime(2026, 1, 24)
        };

        Assert.Equal(FollowUpStatus.Closed, followUp.Status);
        Assert.NotNull(followUp.ClosedDate);
    }

    [Fact]
    public void FollowUp_OpenStatus_CanHaveNullClosedDate()
    {
        var followUp = new FollowUp
        {
            InspectionId = 3,
            DueDate = new DateTime(2026, 1, 25),
            Status = FollowUpStatus.Open,
            ClosedDate = null
        };

        Assert.Equal(FollowUpStatus.Open, followUp.Status);
        Assert.Null(followUp.ClosedDate);
    }
}