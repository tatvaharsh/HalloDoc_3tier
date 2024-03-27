﻿using hallodoc_mvc_Repository.DataModels;
using hallodoc_mvc_Repository.ViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallocdoc_mvc_Service.Interface
{
    public interface IPatient_Service
    {
        void Business(BusinessReqModel req);
        void ConciergeForm(ConciergeReqModel req);
        public void editprofile(PatientProfile model,int id);
        void FamilyForm(FamilyReqModel req);
        public void FileUpload(int id, List<IFormFile> file);
        void Forgot(Create model);
        void Forgotmail(Create model);
        void ForMe(patient_form req,int user1);
       
        public List<RequestWiseFile>  getFiles();
        public List<Request> getRequest(int? id);
        List<Request> getRequestcon(int id);
        public List<RequestWiseFile> getRequestWiseFile(int id);
       public RequestWiseFile getRequestWiseFileById(int id);
        public User getUser(string email);
       public User getUser(int? user1);
        void PatientForm(patient_form model);
        void SubmitForMe(patient_form pf,int userid);
        void Update(Create model);
        bool ValidateUser(LoginViewModel model);
    }
}