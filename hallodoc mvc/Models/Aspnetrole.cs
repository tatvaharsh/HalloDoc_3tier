﻿using System;
using System.Collections.Generic;

namespace hallodoc_mvc.Models;

public partial class AspNetRole
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<AspNetUser> Users { get; set; } = new List<AspNetUser>();
}
