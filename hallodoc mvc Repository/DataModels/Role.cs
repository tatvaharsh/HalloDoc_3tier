﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace hallodoc_mvc_Repository.DataModels;

[Table("Role")]
public partial class Role
{
    [Key]
    public int RoleId { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    public short AccountType { get; set; }

    [StringLength(128)]
    public string CreatedBy { get; set; } = null!;

    [Column(TypeName = "timestamp without time zone")]
    public DateTime CreatedDate { get; set; }

    [StringLength(128)]
    public string? ModifiedBy { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? ModifiedDate { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray IsDeleted { get; set; } = null!;

    [Column("IP")]
    [StringLength(20)]
    public string? Ip { get; set; }

    [InverseProperty("Role")]
    public virtual ICollection<Admin> Admins { get; set; } = new List<Admin>();

    [InverseProperty("Role")]
    public virtual ICollection<EmailLog> EmailLogs { get; set; } = new List<EmailLog>();

    [InverseProperty("Role")]
    public virtual ICollection<RoleMenu> RoleMenus { get; set; } = new List<RoleMenu>();

    [InverseProperty("Role")]
    public virtual ICollection<Smslog> Smslogs { get; set; } = new List<Smslog>();
}
