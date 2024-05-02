using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace hallodoc_mvc_Repository.DataModels;

[Table("reimbursement")]
public partial class Reimbursement
{
    [Key]
    [Column("reimbursement_id")]
    public int ReimbursementId { get; set; }

    [Column("physician_id")]
    public int PhysicianId { get; set; }

    [Column("invoice_id")]
    public int InvoiceId { get; set; }

    [Column("reimbursement_date", TypeName = "timestamp without time zone")]
    public DateTime ReimbursementDate { get; set; }

    [Column("item")]
    public string Item { get; set; } = null!;

    [Column("amount")]
    public int Amount { get; set; }

    [Column("filename")]
    public string? Filename { get; set; }

    [Column("created_by")]
    public int CreatedBy { get; set; }

    [Column("created_date", TypeName = "timestamp without time zone")]
    public DateTime CreatedDate { get; set; }

    [Column("modified_by")]
    public int? ModifiedBy { get; set; }

    [Column("modified_date", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedDate { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ReimbursementCreatedByNavigations")]
    public virtual AspNetUser CreatedByNavigation { get; set; } = null!;

    [ForeignKey("InvoiceId")]
    [InverseProperty("Reimbursements")]
    public virtual Invoice Invoice { get; set; } = null!;

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ReimbursementModifiedByNavigations")]
    public virtual AspNetUser? ModifiedByNavigation { get; set; }

    [ForeignKey("PhysicianId")]
    [InverseProperty("Reimbursements")]
    public virtual Physician Physician { get; set; } = null!;
}
