using System.ComponentModel.DataAnnotations;

namespace UKParliament.CodeTest.Web.ViewModels;

public class DepartmentViewModel
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;
} 