﻿using Xunit.Sdk;

namespace hallodoc_mvc.Models
{
    public class SubmitForMe
    {

        public string? Email { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? ZipCode { get; set; }

        public string? State { get; set; }

        public string? City { get; set; }

        public string? Street { get; set; }

        public string? Room { get; set; }

        public DateOnly BirthDate { get; set; }

        public User user { get; set; }
    }
}
