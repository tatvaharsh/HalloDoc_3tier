using hallocdoc_mvc_Service.Interface;
using hallodoc_mvc_Repository.ViewModel;
using hallodoc_mvc_Repository.DataModels;
using hallodoc_mvc_Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Runtime.Intrinsics.X86;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;
using System.Net;

namespace hallocdoc_mvc_Service.Implementation
{
    public class Patient_Service : IPatient_Service
    {
        private readonly IPatient_Repository _Repository;
        private readonly IJwtService _JwtService;
        public Patient_Service(IPatient_Repository Repository, IJwtService jwtService)
        {
            _Repository = Repository;
            _JwtService = jwtService;

        }

        public void editprofile(PatientProfile model,int id)
        {

            var req = _Repository.getUser(id);
            var asp = _Repository.getAspuserTable(id);
            //var asp = _context.AspNetUsers.FirstOrDefault(u => u.Id == req.AspNetUserId);

            req.FirstName = model.FirstName;
            req.LastName = model.LastName;
            req.Email = model.Email;
            req.Mobile = model.PhoneNumber;
            req.StrMonth = model.BirthDate.ToString("MMM");
            req.IntYear = model.BirthDate.Year;
            req.IntDate = model.BirthDate.Day;
            req.Street = model.Street;
            req.City = model.City;
            req.State = model.State;
            req.ZipCode = model.ZipCode;
            _Repository.UpdateUserTable(req);
            _Repository.Save();
            //_context.Users.Update(req);
            //_context.SaveChanges();

            asp.UserName = model.FirstName;
            asp.Email = model.Email;
            asp.PhoneNumber = model.PhoneNumber;
            _Repository.updateAspnetuserTable(asp);
            _Repository.Save();
            //_context.AspNetUsers.Update(asp);
            //_context.SaveChanges();
            
            

        }

        public User getUser(int? user1)
        {
            return _Repository.getUser(user1);
        }

        public bool ValidateUser(LoginViewModel model)
        {
            AspNetUser asp = _Repository.ValidateUser(model);
            if (asp.Id > 0)
            {
                model.Id = asp.Id;
                return true;
            }
            return false;
           
        }

        List<Request> IPatient_Service.getRequest(int? id)
        {
            return _Repository.getRequest(id);
        }

        User IPatient_Service.getUser(string email)
        {
            return _Repository.getUser(email);
        }

     
        List<RequestWiseFile> IPatient_Service.getRequestWiseFile(int id)
        {
            return _Repository.getRequestWiseFile(id);
        }

        public RequestWiseFile getRequestWiseFileById(int id)
        {
           return _Repository.getRequestWiseFileById(id);
        }

        public void FileUpload(int id, List<IFormFile> file)
        {
            foreach (IFormFile files in file)
            {
                string filename = files.FileName;
                string path = Path.Combine("D:\\Projects\\.net learning\\hallo_doc\\HalloDoc_MVC\\hallodoc mvc\\wwwroot\\uplodedfiles\\", filename);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    files.CopyToAsync(stream).Wait();
                }


                RequestWiseFile requestWiseFile = new RequestWiseFile();
                requestWiseFile.FileName = filename;
                requestWiseFile.RequestId = id;
                requestWiseFile.DocType = 1;
                _Repository.AddRequestWiseFiles(requestWiseFile);
                
            }
        }

        List<RequestWiseFile> IPatient_Service.getFiles()
        {
            return _Repository.getFiles();
        }

        public void SubmitForMe(patient_form pf,int userid)
        {
            

            var req = _Repository.getUser(userid);

            pf.FirstName = req.FirstName;
            pf.LastName = req.LastName;
            pf.Email = req.Email;
            pf.City = req.City;
            pf.Street = req.Street;
            pf.City = req.City;
            pf.State = req.State;
            pf.ZipCode = req.ZipCode;

        }

        public void ForMe(patient_form req,int user1)
        {
            Request request = new Request
            {
                RequestTypeId = 2,
                UserId = user1,
                FirstName = req.FirstName,
                LastName = req.LastName,
                PhoneNumber = req.PhoneNumber,
                Email = req.Email,
                CreatedDate = DateTime.Now,
                Status = 1,

            };
            _Repository.AddRequest(request);
          

            RequestClient requestclient = new RequestClient
            {

                RequestId = request.RequestId,
                FirstName = req.FirstName,
                LastName = req.LastName,
                PhoneNumber = req.PhoneNumber,
                Email = req.Email,
                Location = req.City,
                Address = req.Street,

                IntDate = req.BirthDate.Day,
                StrMonth = req.BirthDate.Month.ToString("MMM"),
                IntYear = req.BirthDate.Year,
                Street = req.Street,
                City = req.City,
                State = req.State,
                ZipCode = req.ZipCode,

            };
            _Repository.AddRequestClients(requestclient);
           

        }

        public  void PatientForm(patient_form model)
        {
            var aspnetuser1 =  _Repository.AspEmail(model.Email);
            var user1 = _Repository.getUser(model.Email);
            //var aspnetuser1 = await _context.AspNetUsers.FirstOrDefaultAsync(u => u.Email == model.Email);
            //var user1 = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

            /* Debug.WriteLine(aspnetuser);*/

            if (aspnetuser1 == null)
            {
                AspNetUser aspnetuser2 = new AspNetUser
                {

                    UserName = model.FirstName + "_" + model.LastName,
                    Email = model.Email,
                    //PasswordHash = model.Password,
                    PhoneNumber = model.PhoneNumber,
                    CreatedDate = DateTime.Now,
                    PasswordHash = model.Password,
                    Roles = _Repository.RolePatient(),
                };
                _Repository.AddAspnetuser(aspnetuser2);
                 aspnetuser1 = aspnetuser2;

            }

            Region region = new Region
            {
                Name = model.State,
                Abbreviation = model.State.Substring(0, 3),
            };
            Region isRegion = _Repository.isRegion(region.Abbreviation);

            if (isRegion == null)
            {
                isRegion = region;
                _Repository.AddRegion(region);
            }

            if (user1 == null)
            {
                User user = new User
                {
                    AspNetUserId = aspnetuser1.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Mobile = model.PhoneNumber,
                    ZipCode = model.ZipCode,
                    State = model.State,
                    City = model.City,
                    RegionId = isRegion.RegionId,
                    Street = model.Street,
                    IntDate = model.BirthDate.Day,
                    IntYear = model.BirthDate.Year,
                    StrMonth = model.BirthDate.ToString("MMM"),
                    CreatedDate = DateTime.Now,
                   
                    CreatedBy = aspnetuser1.Id
                };

                _Repository.AddUser(user);

                user1 = user;
            }

            Request request = new Request
            {
                RequestTypeId = 2,
                UserId = user1.UserId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                CreatedDate = DateTime.Now,
                Status = 1,
            };

            _Repository.AddRequest(request);

            RequestClient requestclient = new RequestClient
            {

                RequestId = request.RequestId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                Location = model.City,
                Address = model.Street,
                RegionId=isRegion.RegionId,
                IntDate = model.BirthDate.Day,
                StrMonth = model.BirthDate.ToString("MMM"),
                IntYear = model.BirthDate.Year,
                Street = model.Street,
                City = model.City,
                State = model.State,
                ZipCode = model.ZipCode,

            };

            _Repository.AddRequestClients(requestclient);
            if(model.File != null)
            {

            foreach (IFormFile files in model.File)
            {
                string filename = model.FirstName + model.LastName + files.FileName;
                string path = Path.Combine("D:\\Projects\\.net learning\\hallo_doc\\HalloDoc_MVC\\hallodoc mvc\\wwwroot\\uplodedfiles\\", filename);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    files.CopyToAsync(stream).Wait();
                }


                RequestWiseFile requestWiseFile = new RequestWiseFile();
                requestWiseFile.FileName = filename;
                requestWiseFile.RequestId = request.RequestId;
                requestWiseFile.DocType = 1;
                _Repository.AddRequestWiseFiles(requestWiseFile);
              
            }
            }
        }

        public void FamilyForm(FamilyReqModel req)
        {
            AspNetUser aspuser = _Repository.AspEmail(req.Email);
            //AspNetUser aspuser = _context.AspNetUsers.FirstOrDefault(m => m.Email == req.Email);
            //User usertbl = _context.Users.FirstOrDefault(m => m.Email == req.Email);
            User usertbl = _Repository.getUser(req.Email);


            if (aspuser == null)
            {
                var receiver = req.Email;
                var subject = "Create Account";
                var message = "Tap on link for Create Account: http://localhost:5198/Home/create_patient?token="+_JwtService.GenerateJwtTokenByEmail(receiver);


                var mail = "tatva.dotnet.binalmalaviya@outlook.com";
                var password = "binal@2002";

                var client = new SmtpClient("smtp.office365.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, password)
                };

                client.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message));

               

            }


            Region region = new Region
            {
                Name = req.State,
                Abbreviation = req.State.Substring(0, 3),
            };
            Region isRegion = _Repository.isRegion(region.Abbreviation);

            if (isRegion == null)
            {
                isRegion = region;
                _Repository.AddRegion(region);
            }

            //if (aspuser == null)
            //{
            //    AspNetUser aspNetUser = new AspNetUser
            //    {
            //        UserName = req.FirstName,
            //        PasswordHash = req.Password,
            //        Email = req.Email,
            //        CreatedDate = DateTime.Now,
            //        PhoneNumber = req.Mobile,
            //    };
            //    _Repository.AddAspnetuser(aspNetUser);

            //    aspuser = aspNetUser;
            //}

            //if (usertbl == null)
            //{
            //    User user = new()
            //    {
            //        AspNetUserId = aspuser.Id,
            //        FirstName = req.FirstName,
            //        LastName = req.LastName,
            //        Email = req.Email,
            //        Mobile = req.Mobile,
            //        ZipCode = req.ZipCode,
            //        State = req.State,
            //        City = req.City,
            //        Street = req.Street,
            //        Status = 1,
            //        RegionId = isRegion.RegionId,
            //        CreatedBy = aspuser.Id,
            //        IntDate = req.DOB.Day,
            //        IntYear = req.DOB.Year,
            //        StrMonth = req.DOB.ToString("MMM"),
            //        CreatedDate = DateTime.Now,
            //    };
            //    _Repository.AddUser(user);

            //    usertbl = user;
            //}

            Request reqobj = new()
            {
                RequestTypeId = 3,
               
                FirstName = req.FamFirstName,
                LastName = req.FamLastName,
                Email = req.FamEmail,
                PhoneNumber = req.FamMobile,
                Status = 1,
                CreatedDate = DateTime.Now,
            };
            _Repository.AddRequest(reqobj);



            RequestClient rc = new RequestClient
            {
                RequestId = reqobj.RequestId,
                FirstName = req.FirstName,
                LastName = req.LastName,
                PhoneNumber = req.Mobile,
                Location = req.Room,
                Address = req.Street + ", " + req.City + ", " + req.State,
                Notes = req.Symptoms,
                Email = req.Email,
                RegionId = isRegion.RegionId,
                IntDate = req.DOB.Day,
                IntYear = req.DOB.Year,
                StrMonth = req.DOB.ToString("MMM"),
                Street = req.Street,
                City = req.City,
                State = req.State,
                ZipCode = req.ZipCode,
            };
           
            _Repository.AddRequestClients(rc);
        }

        public void ConciergeForm(ConciergeReqModel req)
        {
            AspNetUser aspuser = _Repository.AspEmail(req.Email);
            User usertbl = _Repository.getUser(req.Email);


            if (aspuser == null)
            {
                var receiver = req.Email;
                var subject = "Create Account";
                var message = "Tap on link for Create Account: http://localhost:5198/Home/create_patient?token=" + _JwtService.GenerateJwtTokenByEmail(receiver);


                var mail = "tatva.dotnet.binalmalaviya@outlook.com";
                var password = "binal@2002";

                var client = new SmtpClient("smtp.office365.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, password)
                };

                client.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message));



            }
            Region region = new Region
            {
                Name = req.State,
                Abbreviation = req.State.Substring(0, 3),
            };
            Region isRegion = _Repository.isRegion(region.Abbreviation);

            if (isRegion == null)
            {
                isRegion = _Repository.AddRegion(region);
            }



            //if (aspuser == null)
            //{
            //    AspNetUser aspNetUser = new AspNetUser
            //    {
            //        UserName = req.FirstName,
            //        PasswordHash = req.Password,
            //        Email = req.Email,
            //        PhoneNumber = req.Mobile,
            //        CreatedDate = DateTime.Now,
            //    };
            //    _Repository.AddAspnetuser(aspNetUser);
            //    aspuser = aspNetUser;
            //}

            //if (usertbl == null)
            //{
            //    User user = new User
            //    {
            //        AspNetUserId = aspuser.Id,
            //        FirstName = req.FirstName,
            //        LastName = req.LastName,
            //        Email = req.Email,
            //        Mobile = req.Mobile,
            //        Status = 1,
            //        IntDate = req.DOB.Day,
            //        IntYear = req.DOB.Year,
            //        StrMonth = req.DOB.ToString("MMM"),
            //        CreatedBy = aspuser.Id,
            //        CreatedDate = DateTime.Now,
            //        RegionId=isRegion.RegionId
            //    };
            //    _Repository.AddUser(user);
            
            //    usertbl = user;
            //}


            Request reqobj = new Request
            {
                RequestTypeId = 4,
                UserId = usertbl.UserId,
                FirstName = req.ConFirstName,
                LastName = req.ConLastName,
                Email = req.ConEmail,
                PhoneNumber = req.ConMobile,
                Status = 1,
                CreatedDate = DateTime.Now,
            };
            _Repository.AddRequest(reqobj);


         



            RequestClient rc = new RequestClient
            {
                RequestId = reqobj.RequestId,
                FirstName = req.FirstName,
                LastName = req.LastName,
                PhoneNumber = req.Mobile,
                Location = req.Room,
                Address = req.Street + ", " + req.City + ", " + req.State,
                Notes = req.Symptoms,
                Email = req.Email,
                RegionId = isRegion.RegionId,
                IntDate = req.DOB.Day,
                IntYear = req.DOB.Year,
                StrMonth = req.DOB.ToString("MMM"),
                Street = req.Street,
                City = req.City,
                State = req.State,
                ZipCode = req.ZipCode,
            };
            _Repository.AddRequestClients(rc);
         


            Concierge con = new Concierge
            {
                ConciergeName = req.ConFirstName + " " + req.ConLastName,
                Address = req.Property,
                Street = req.Street,
                City = req.City,
                ZipCode = req.ZipCode,
                State = req.State,
                RegionId = isRegion.RegionId,
                CreatedDate = DateTime.Now,
            };
            _Repository.AddConcierge(con);
          


            RequestConcierge reqCon = new RequestConcierge
            {
                RequestId = reqobj.RequestId,
                ConciergeId = con.ConciergeId,
            };
            _Repository.AddRequestConcierge(reqCon);
            
        }

        public void Business(BusinessReqModel req)
        {
            AspNetUser aspuser = _Repository.AspEmail(req.Email);
            //AspNetUser aspuser = _context.AspNetUsers.FirstOrDefault(m => m.Email == req.Email);
            //User usertbl = _context.Users.FirstOrDefault(m => m.Email == req.Email);
            User usertbl = _Repository.getUser(req.Email);

            if (aspuser == null)
            {
                var receiver = req.Email;
                var subject = "Create Account";
                var message = "Tap on link for Create Account: http://localhost:5198/Home/create_patient?token=" + _JwtService.GenerateJwtTokenByEmail(receiver);


                var mail = "tatva.dotnet.binalmalaviya@outlook.com";
                var password = "binal@2002";

                var client = new SmtpClient("smtp.office365.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, password)
                };

                client.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message));



            }
            Region region = new Region
            {
                Name = req.State,
                Abbreviation = req.State.Substring(0, 3),
            };
            Region isRegion = _Repository.isRegion(region.Abbreviation);

            if (isRegion == null)
            {
                isRegion = region;
                _Repository.AddRegion(region);
            }


            //if (aspuser == null)
            //{
            //    AspNetUser aspNetUser = new AspNetUser
            //    {
            //        UserName = req.FirstName,
            //        PasswordHash = req.Password,
            //        Email = req.Email,
            //        PhoneNumber = req.Mobile,
            //        CreatedDate = DateTime.Now,
            //    };
            //    _Repository.AddAspnetuser(aspNetUser);
              
            //    aspuser = aspNetUser;
            //}

            //if (usertbl == null)
            //{
            //    User user = new User
            //    {
            //        AspNetUserId = aspuser.Id,
            //        FirstName = req.FirstName,
            //        LastName = req.LastName,
            //        Email = req.Email,
            //        Mobile = req.Mobile,
            //        ZipCode = req.ZipCode,
            //        State = req.State,
            //        City = req.City,
            //        Street = req.Street,
            //        Status = 1,
            //        CreatedBy = aspuser.Id,
            //        IntDate = req.DOB.Day,
            //        IntYear = req.DOB.Year,
            //        StrMonth = req.DOB.ToString("MMM"),
            //        CreatedDate = DateTime.Now,
            //        RegionId=isRegion.RegionId
                    
            //    };
            //    _Repository.AddUser(user);
               
            //    usertbl = user;
            //}

            Request reqobj = new Request
            {
                RequestTypeId = 1,
                UserId = usertbl.UserId,
                FirstName = req.BusFirstName,
                LastName = req.BusLastName,
                Email = req.BusEmail,
                PhoneNumber = req.BusMobile,
                Status = 1,
                CreatedDate = DateTime.Now,
            };
            _Repository.AddRequest(reqobj);



          



            RequestClient rc = new RequestClient
            {
                RequestId = reqobj.RequestId,
                FirstName = req.FirstName,
                LastName = req.LastName,
                PhoneNumber = req.Mobile,
                Location = req.Room,
                Address = req.Street + ", " + req.City + ", " + req.State,
                Notes = req.Symptoms,
                Email = req.Email,
                RegionId = isRegion.RegionId,
                IntDate = req.DOB.Day,
                IntYear = req.DOB.Year,
                StrMonth = req.DOB.ToString("MMM"),
                Street = req.Street,
                City = req.City,
                State = req.State,
                ZipCode = req.ZipCode,
            };
            _Repository.AddRequestClients(rc);
            

            Business business = new Business
            {
                Name = req.BusFirstName,
                Address1 = req.Property,
                PhoneNumber = req.BusMobile,
                RegionId = isRegion.RegionId,
                CreatedDate = DateTime.Now,
            };
            _Repository.AddBusiness(business);
          


            RequestBusiness reqBus = new RequestBusiness
            {
                RequestId = reqobj.RequestId,
                BusinessId = business.BusinessId,
            };
            _Repository.AddRequestBusiness(reqBus);
          

        }

        public void Update(Create req)
        {

            AspNetUser aspuser = _Repository.AspEmail(req.UserName);
            //AspNetUser aspuser = _context.AspNetUsers.FirstOrDefault(m => m.Email == req.Email);
            //User usertbl = _context.Users.FirstOrDefault(m => m.Email == req.Email);
            User usertbl = _Repository.getUser(req.UserName);
            RequestClient rc = _Repository.getRcbyemail(req.UserName);

            /* _context.AspNetUsers.FirstOrDefault(u => u.Email == model.UserName);*/
            if (aspuser == null)
            {
                AspNetUser aspNetUser = new AspNetUser
                {
                    UserName = rc.FirstName,
                    PasswordHash = req.PasswordHash,
                    Email = req.UserName,
                    PhoneNumber = rc.PhoneNumber,
                    CreatedDate = DateTime.Now,
                    Roles = _Repository.AddRole(),
                };
                _Repository.AddAspnetuser(aspNetUser);

                aspuser = aspNetUser;
            }

    

            if (usertbl == null)
            {
                User user = new User
                {
                    AspNetUserId = aspuser.Id,
                    FirstName = rc.FirstName,
                    LastName = rc.LastName,
                    Email = req.UserName,
                    Mobile = rc.PhoneNumber,
                    ZipCode = rc.ZipCode,
                    State = rc.State,
                    City = rc.City,
                    Street = rc.Street,
                    Status = 1,
                    CreatedBy = aspuser.Id,
                    IntDate = rc.IntDate,
                    IntYear = rc.IntYear,
                    StrMonth = rc.StrMonth,
                    CreatedDate = DateTime.Now,
                    RegionId = rc.RegionId

                };
                _Repository.AddUser(user);

        
            }

        


        }

        public void Forgot(Create model)
        {
            AspNetUser aspuser = _Repository.AspEmail(model.UserName);

           
        }

        public void Forgotmail(Create model)
        {
            AspNetUser aspuser = _Repository.AspEmail(model.UserName);
            if (aspuser != null)
            {
                var receiver = model.UserName;
                var subject = "Reset Password";
                var message = "Tap on link for Reset Account: http://localhost:5198/Home/ResetPassword?token=" + _JwtService.GenerateJwtTokenByEmail(receiver);


                var mail = "tatva.dotnet.binalmalaviya@outlook.com";
                var password = "binal@2002";

                var client = new SmtpClient("smtp.office365.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, password)
                };

                client.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message));
               



            }
        }

        public List<Request> getRequestcon(int id)
        {
           return _Repository.getcon(id);
        }

        public void Resetpass(Create model, string email)
        {
            AspNetUser aspuser = _Repository.AspEmail(email);
            if (aspuser != null)
            {
                aspuser.PasswordHash = model.PasswordHash;
                _Repository.updateAspnetuserTable(aspuser);
                _Repository.Save();
            }
        }
    }
}
