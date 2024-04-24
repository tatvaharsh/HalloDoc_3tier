using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace hallodoc_mvc_Repository.DataModels;

[Table("User")]
public partial class User
{
    [Column(TypeName = "character varying")]
    public string? FirstName { get; set; }

    [Column(TypeName = "character varying")]
    public string? LastName { get; set; }

    public int? CityId { get; set; }

    public int? Age { get; set; }

    [Column(TypeName = "character varying")]
    public string? Email { get; set; }

    [Column(TypeName = "character varying")]
    public string? PhoneNo { get; set; }

    [Column(TypeName = "character varying")]
    public string? Gender { get; set; }

    [Column(TypeName = "character varying")]
    public string? City { get; set; }

    [Column(TypeName = "character varying")]
    public string? Country { get; set; }

    [Key]
    public int Id { get; set; }

    public bool? IsDeleted { get; set; }

    [ForeignKey("CityId")]
    [InverseProperty("Users")]
    public virtual City? CityNavigation { get; set; }
}
