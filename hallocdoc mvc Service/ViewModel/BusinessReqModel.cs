using System;using System.Collections.Generic;using System.ComponentModel.DataAnnotations;
namespace hallocdoc_mvc_Service.ViewModel
{

public partial class BusinessReqModel{
    [Required(ErrorMessage = "Business Name is Required")]    public string? BusFirstName { get; set; }

    public string? Password { get; set; }
    [Required(ErrorMessage = "Business Name is Required")]    public string? BusLastName { get; set; }
    [Required(ErrorMessage = "Business Email is Required")]    public string BusEmail { get; set; } = null!;
    [Required(ErrorMessage = "Business Mobile is Required")]    public string? BusMobile { get; set; }    public string? Property { get; set; }    public int? CaseNum { get; set; }    public string? Symptoms { get; set; }
    [Required(ErrorMessage = "First Name is Required")]    public string? FirstName { get; set; }
    [Required(ErrorMessage = "Last Name is Required")]    public string? LastName { get; set; }
    [Required(ErrorMessage = "Date of Birth is Required")]    public DateTime DOB { get; set; }
    [Required(ErrorMessage = "Email is Required")]    public string Email { get; set; } = null!;    [Required(ErrorMessage = "Mobile is Required")]    public string? Mobile { get; set; }    public string? Street { get; set; }    public string? City { get; set; }    public string? State { get; set; }    [Required(ErrorMessage = "Concierge Name is Required")]    public string? ZipCode { get; set; }    public string? Room { get; set; }}}