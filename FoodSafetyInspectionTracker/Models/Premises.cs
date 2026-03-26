using System.ComponentModel.DataAnnotations;
using FoodSafetyInspectionTracker.Models.Enums;

namespace FoodSafetyInspectionTracker.Models;

public class Premises
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Address { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Town { get; set; } = string.Empty;

    [Required]
    public RiskRating RiskRating { get; set; }

    public ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();
}