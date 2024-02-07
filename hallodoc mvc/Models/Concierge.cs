﻿using System;
using System.Collections.Generic;

namespace hallodoc_mvc.Models;

public partial class Concierge
{
    public int Conciergeid { get; set; }

    public string Conciergename { get; set; } = null!;

    public string? Address { get; set; }

    public string Street { get; set; } = null!;

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;

    public string Zipcode { get; set; } = null!;

    public DateTime Createddate { get; set; }

    public int? Regionid { get; set; }

    public string? Roleid { get; set; }

    public virtual Region? Region { get; set; }

    public virtual ICollection<Requestconcierge> Requestconcierges { get; set; } = new List<Requestconcierge>();
}
