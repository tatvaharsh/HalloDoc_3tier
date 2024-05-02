using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace hallodoc_mvc_Repository.DataModels;

[Table("timesheet")]
public partial class Timesheet
{
    [Key]
    [Column("timesheet_id")]
    public int TimesheetId { get; set; }

    [Column("physician_id")]
    public int PhysicianId { get; set; }

    [Column("invoice_id")]
    public int InvoiceId { get; set; }

    [Column("sheet_date", TypeName = "timestamp without time zone")]
    public DateTime SheetDate { get; set; }

    [Column("total_hours")]
    public int? TotalHours { get; set; }

    [Column("weekend_holiday")]
    public bool? WeekendHoliday { get; set; }

    [Column("no_housecalls")]
    public int? NoHousecalls { get; set; }

    [Column("no_housecalls_night")]
    public int? NoHousecallsNight { get; set; }

    [Column("no_phone_consult")]
    public int? NoPhoneConsult { get; set; }

    [Column("no_phone_consult_night")]
    public int? NoPhoneConsultNight { get; set; }

    [Column("created_by")]
    public int CreatedBy { get; set; }

    [Column("created_date", TypeName = "timestamp without time zone")]
    public DateTime CreatedDate { get; set; }

    [Column("modified_by")]
    public int? ModifiedBy { get; set; }

    [Column("modified_date", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedDate { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("TimesheetCreatedByNavigations")]
    public virtual AspNetUser CreatedByNavigation { get; set; } = null!;

    [ForeignKey("InvoiceId")]
    [InverseProperty("Timesheets")]
    public virtual Invoice Invoice { get; set; } = null!;

    [ForeignKey("ModifiedBy")]
    [InverseProperty("TimesheetModifiedByNavigations")]
    public virtual AspNetUser? ModifiedByNavigation { get; set; }

    [ForeignKey("PhysicianId")]
    [InverseProperty("Timesheets")]
    public virtual Physician Physician { get; set; } = null!;
}
