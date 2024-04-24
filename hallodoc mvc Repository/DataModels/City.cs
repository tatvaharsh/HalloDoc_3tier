using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace hallodoc_mvc_Repository.DataModels;

[Table("City")]
public partial class City
{
    [Column(TypeName = "character varying")]
    public string? Name { get; set; }

    [Key]
    public int Id { get; set; }

    [InverseProperty("CityNavigation")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
