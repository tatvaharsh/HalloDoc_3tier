using System;
using System.Collections.Generic;

namespace HalloDoc.Models;

public partial class ConciergeReqModel
{
    public string? ConFirstName { get; set; }

    public string? Password { get; set; }

    public string? ConLastName { get; set; }

    public string ConEmail { get; set; } = null!;

    public string? ConMobile { get; set; }

    public string? Property { get; set; }

    public string? Street { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? ZipCode { get; set; }

    public string? Symptoms { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime DOB { get; set; }

    public string Email { get; set; } = null!;

    public string? Mobile { get; set; }

    public string? Room { get; set; }
}