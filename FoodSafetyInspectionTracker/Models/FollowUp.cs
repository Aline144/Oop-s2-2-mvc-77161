using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FoodSafetyInspectionTracker.Models.Enums;

namespace FoodSafetyInspectionTracker.Models;

public class FollowUp
{
    public int Id { get; set; }

    [Required]
    public int InspectionId { get; set; }

    [ForeignKey(nameof(InspectionId))]
    public Inspection? Inspection { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    [Required]
    public FollowUpStatus Status { get; set; }

    public DateTime? ClosedDate { get; set; }
}