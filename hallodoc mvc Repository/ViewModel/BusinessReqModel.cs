﻿using System;
namespace hallodoc_mvc_Repository.ViewModel
{


    [Required(ErrorMessage = "Business Name is Required")]

    public string? Password { get; set; }
    [Required(ErrorMessage = "Business Name is Required")]
    [Required(ErrorMessage = "Business Email is Required")]
    [Required(ErrorMessage = "Business Mobile is Required")]
    [Required(ErrorMessage = "First Name is Required")]
    [Required(ErrorMessage = "Last Name is Required")]
    [Required(ErrorMessage = "Date of Birth is Required")]
    [Required(ErrorMessage = "Email is Required")]