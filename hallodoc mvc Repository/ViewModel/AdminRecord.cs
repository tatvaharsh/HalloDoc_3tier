﻿using hallodoc_mvc_Repository.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.ViewModel
{
    public class AdminRecord
    {
        public string? Status { get; set; }
        public string? PhysicianNote { get; set; }
        public string? AdminNote { get; set; }
        public string? PatientNote { get; set; }
        public string? Physician { get; set; }
        public string? zip { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? CloseCaseDate { get; set; }
        public string? DateOfService { get; set; }
        public string? Requestor { get; set; }
        public string? PatientName { get; set; }
        public int? ReqCliId { get; set; }
        public int? RequestId { get; set; }
        public List<RequestType>? ReqType { get; set; }
        public List<RequestClient> Data { get; set; }
        public List<RequestNote> ReqNotes { get; set; }
        public List<Physician> phy { get; set; }
        public List<BlockRequest> blockRequests { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? reason { get; set; }
        public string? note { get; set; }
        public List<Region>? region { get; set; }
        public List<Physician>? Physicians { get; set; }
        public string? SelectedRegion { get; set; }
        public int? SelectedPhysicianId { get; set; }
    }
}
