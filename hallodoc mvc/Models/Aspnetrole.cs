using System;
using System.Collections.Generic;

namespace hallodoc_mvc.Models;

public partial class Aspnetrole
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<Aspnetuser> Users { get; set; } = new List<Aspnetuser>();
}
