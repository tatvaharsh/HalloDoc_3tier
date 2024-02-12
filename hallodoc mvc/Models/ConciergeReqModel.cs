using System;
using System.Collections.Generic;
using Xunit.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace HalloDoc.Models;

public partial class ConciergeReqModel
{
    [Required(ErrorMessage = "Concierge Name is Required")]
    public string? ConFirstName { get; set; }

    public string? Password { get; set; }
    [Required(ErrorMessage = "Concierge Name is Required")]
    public string? ConLastName { get; set; }
    [Required(ErrorMessage = "Concierge Email is Required")]
    public string ConEmail { get; set; } = null!;
    [Required(ErrorMessage = "Concierge Mobile is Required")]
    public string? ConMobile { get; set; }

    public string? Property { get; set; }

    public string? Street { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? ZipCode { get; set; }

    public string? Symptoms { get; set; }
    [Required(ErrorMessage = "First Name is Required")]
    public string? FirstName { get; set; }
    [Required(ErrorMessage = "First Name is Required")]
    public string? LastName { get; set; }
    [Required(ErrorMessage = "DOB is Required")]
    public DateTime DOB { get; set; }
    [Required(ErrorMessage = "Email is Required")]
    public string Email { get; set; } = null!;
    [Required(ErrorMessage = "Mobile is Required")]
    public string? Mobile { get; set; }

    public string? Room { get; set; }
}