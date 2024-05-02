using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace hallodoc_mvc_Repository.DataModels;

[Table("invoice")]
public partial class Invoice
{
    [Key]
    [Column("invoice_id")]
    public int InvoiceId { get; set; }

    [Column("physician_id")]
    public int PhysicianId { get; set; }

    [Column("start_date", TypeName = "timestamp without time zone")]
    public DateTime StartDate { get; set; }

    [Column("end_date", TypeName = "timestamp without time zone")]
    public DateTime EndDate { get; set; }

    [Column("status")]
    public int Status { get; set; }

    [Column("created_by")]
    public int CreatedBy { get; set; }

    [Column("created_date", TypeName = "timestamp without time zone")]
    public DateTime CreatedDate { get; set; }

    [Column("modified_by")]
    public int? ModifiedBy { get; set; }

    [Column("modified_date", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedDate { get; set; }

    [Column("approved_by")]
    public int? ApprovedBy { get; set; }

    [Column("approved_date", TypeName = "timestamp without time zone")]
    public DateTime? ApprovedDate { get; set; }

    [Column("is_finalized")]
    public bool? IsFinalized { get; set; }

    [Column("bonus")]
    public int? Bonus { get; set; }

    [Column("description")]
    [StringLength(200)]
    public string? Description { get; set; }

    [ForeignKey("ApprovedBy")]
    [InverseProperty("InvoiceApprovedByNavigations")]
    public virtual AspNetUser? ApprovedByNavigation { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("InvoiceCreatedByNavigations")]
    public virtual AspNetUser CreatedByNavigation { get; set; } = null!;

    [ForeignKey("ModifiedBy")]
    [InverseProperty("InvoiceModifiedByNavigations")]
    public virtual AspNetUser? ModifiedByNavigation { get; set; }

    [ForeignKey("PhysicianId")]
    [InverseProperty("Invoices")]
    public virtual Physician Physician { get; set; } = null!;

    [InverseProperty("Invoice")]
    public virtual ICollection<Reimbursement> Reimbursements { get; set; } = new List<Reimbursement>();

    [InverseProperty("Invoice")]
    public virtual ICollection<Timesheet> Timesheets { get; set; } = new List<Timesheet>();
}
