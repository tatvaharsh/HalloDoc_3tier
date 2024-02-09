using System;
using System.Collections.Generic;

namespace HalloDoc.Models;

public partial class FamilyReqModel
{
    public string? FamFirstName { get; set; }

    public string? Password { get; set; }

    public string? FamLastName { get; set; }

    public string FamEmail { get; set; } = null!;

    public string? FamMobile { get; set; }

    public string? Relation { get; set; }

    public string? Symptoms { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime DOB { get; set; }

    public string Email { get; set; } = null!;

    public string? Mobile { get; set; }

    public string? Street { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? ZipCode { get; set; }

    public string? Room { get; set; }
}