using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace hallodoc_mvc_Repository.ViewModel
{



public partial class FamilyReqModel
{

    [Required(ErrorMessage = "Family Member Name is required")]
    public string? FamFirstName { get; set; }

    public string? Password { get; set; }
    [Required(ErrorMessage = "Family Member Name is required")]
    public string? FamLastName { get; set; }
    [Required(ErrorMessage = "Family Email is required")]
    public string FamEmail { get; set; } = null!;
    [Required(ErrorMessage = "Family Mobile Number is required")]
    public string? FamMobile { get; set; }
    [Required(ErrorMessage = "Relation is required")]
    public string? Relation { get; set; }

    public string? Symptoms { get; set; }
    [Required(ErrorMessage = "Firstname is required")]
    public string? FirstName { get; set; }
    [Required(ErrorMessage = "Lastname is required")]
    public string? LastName { get; set; }
    [Required(ErrorMessage = "Date of Birth is required")]
    public DateTime DOB { get; set; }
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = null!;
    [Required(ErrorMessage = "Mobile Number is required")]
    public string? Mobile { get; set; }

    public string? Street { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? ZipCode { get; set; }

    public string? Room { get; set; }
}
}