﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace hallodoc_mvc_Repository.DataModels;

[Table("Physician")]
public partial class Physician
{
    [Key]
    public int PhysicianId { get; set; }

    public int? AspNetUserId { get; set; }

    [StringLength(100)]
    public string FirstName { get; set; } = null!;

    [StringLength(100)]
    public string? LastName { get; set; }

    [StringLength(50)]
    public string Email { get; set; } = null!;

    [StringLength(20)]
    public string? Mobile { get; set; }

    [StringLength(500)]
    public string? MedicalLicense { get; set; }

    [StringLength(100)]
    public string? Photo { get; set; }

    [StringLength(500)]
    public string? AdminNotes { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? IsAgreementDoc { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? IsBackgroundDoc { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? IsTrainingDoc { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? IsNonDisclosureDoc { get; set; }

    [StringLength(500)]
    public string? Address1 { get; set; }

    [StringLength(500)]
    public string? Address2 { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    public int? RegionId { get; set; }

    [StringLength(10)]
    public string? Zip { get; set; }

    [StringLength(20)]
    public string? AltPhone { get; set; }

    public int? CreatedBy { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? ModifiedDate { get; set; }

    public short? Status { get; set; }

    [StringLength(100)]
    public string BusinessName { get; set; } = null!;

    [StringLength(200)]
    public string BusinessWebsite { get; set; } = null!;

    [Column(TypeName = "bit(1)")]
    public BitArray? IsDeleted { get; set; }

    public int? RoleId { get; set; }

    [Column("NPINumber")]
    [StringLength(500)]
    public string? Npinumber { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? IsLicenseDoc { get; set; }

    [StringLength(100)]
    public string? Signature { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? IsCredentialDoc { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? IsTokenGenerate { get; set; }

    [StringLength(50)]
    public string? SyncEmailAddress { get; set; }

    [ForeignKey("AspNetUserId")]
    [InverseProperty("PhysicianAspNetUsers")]
    public virtual AspNetUser? AspNetUser { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("PhysicianCreatedByNavigations")]
    public virtual AspNetUser? CreatedByNavigation { get; set; }

    [InverseProperty("Physician")]
    public virtual ICollection<EmailLog> EmailLogs { get; set; } = new List<EmailLog>();

    [InverseProperty("Physician")]
    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    [ForeignKey("ModifiedBy")]
    [InverseProperty("PhysicianModifiedByNavigations")]
    public virtual AspNetUser? ModifiedByNavigation { get; set; }

    [InverseProperty("Physician")]
    public virtual ICollection<PhysicianNotification> PhysicianNotifications { get; set; } = new List<PhysicianNotification>();

    [InverseProperty("Physician")]
    public virtual ICollection<PhysicianPayrate> PhysicianPayrates { get; set; } = new List<PhysicianPayrate>();

    [InverseProperty("Physician")]
    public virtual ICollection<PhysicianRegion> PhysicianRegions { get; set; } = new List<PhysicianRegion>();

    [ForeignKey("RegionId")]
    [InverseProperty("Physicians")]
    public virtual Region? Region { get; set; }

    [InverseProperty("Physician")]
    public virtual ICollection<Reimbursement> Reimbursements { get; set; } = new List<Reimbursement>();

    [InverseProperty("Physician")]
    public virtual ICollection<RequestStatusLog> RequestStatusLogPhysicians { get; set; } = new List<RequestStatusLog>();

    [InverseProperty("TransToPhysician")]
    public virtual ICollection<RequestStatusLog> RequestStatusLogTransToPhysicians { get; set; } = new List<RequestStatusLog>();

    [InverseProperty("Physician")]
    public virtual ICollection<RequestWiseFile> RequestWiseFiles { get; set; } = new List<RequestWiseFile>();

    [InverseProperty("Physician")]
    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    [InverseProperty("Physician")]
    public virtual ICollection<Shift> Shifts { get; set; } = new List<Shift>();

    [InverseProperty("Physician")]
    public virtual ICollection<Smslog> Smslogs { get; set; } = new List<Smslog>();

    [InverseProperty("Physician")]
    public virtual ICollection<Timesheet> Timesheets { get; set; } = new List<Timesheet>();
}
