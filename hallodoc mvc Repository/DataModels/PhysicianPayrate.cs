using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace hallodoc_mvc_Repository.DataModels;

[Table("physician_payrate")]
public partial class PhysicianPayrate
{
    [Key]
    [Column("payrate_id")]
    public int PayrateId { get; set; }

    [Column("physician_id")]
    public int PhysicianId { get; set; }

    [Column("nigthshift_weekend")]
    public int? NigthshiftWeekend { get; set; }

    [Column("shift")]
    public int? Shift { get; set; }

    [Column("housecalls_nigths_weekend")]
    public int? HousecallsNigthsWeekend { get; set; }

    [Column("phone_consults")]
    public int? PhoneConsults { get; set; }

    [Column("phone_consults_nigths_weekend")]
    public int? PhoneConsultsNigthsWeekend { get; set; }

    [Column("batch_testing")]
    public int? BatchTesting { get; set; }

    [Column("housecall")]
    public int? Housecall { get; set; }

    [Column("created_by")]
    public int CreatedBy { get; set; }

    [Column("modified_by")]
    public int? ModifiedBy { get; set; }

    [Column("created_date", TypeName = "timestamp without time zone")]
    public DateTime CreatedDate { get; set; }

    [Column("modified_date")]
    public TimeOnly? ModifiedDate { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("PhysicianPayrateCreatedByNavigations")]
    public virtual AspNetUser CreatedByNavigation { get; set; } = null!;

    [ForeignKey("ModifiedBy")]
    [InverseProperty("PhysicianPayrateModifiedByNavigations")]
    public virtual AspNetUser? ModifiedByNavigation { get; set; }

    [ForeignKey("PhysicianId")]
    [InverseProperty("PhysicianPayrates")]
    public virtual Physician Physician { get; set; } = null!;
}
