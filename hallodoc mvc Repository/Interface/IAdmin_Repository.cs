﻿using hallodoc_mvc_Repository.DataModels;
using hallodoc_mvc_Repository.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallodoc_mvc_Repository.Interface
{
    public interface IAdmin_Repository
    {
        IQueryable<Request> GetAdminCode();
        IQueryable<Request> GetAdminConclude();
        IQueryable<Request> GetAdminToclose();
        IQueryable<Request> GetAdminUnpaid();
        IQueryable<Request> GetAdminPending();
        IQueryable<Request> GetAdminStatus();
        List<Request> GetRequest();
        AspNetUser Validate(LoginViewModel model);
        Admin getaspuser(string email);
        List<Region> GetRegion();
        RequestNote? setnotes(int id);
        List<RequestStatusLog?> GetStatusLogsByRequest(int id);
        void save();
        void AddRequestNotes(RequestNote newnotedata);
        //List<Region> GetPhyByRegion(int id, ModalData md);
        List<Physician> GetPhysiciansByRegion(int regionid);
        RequestClient? Cancelname(int id);
        List<CaseTag> GetCases();
        Request GetRequestById(int id);
        void UpdateRequesttbl(Request req);
        void AddRequestStatuslog(RequestStatusLog reqlog);
        Physician GetPhysician(int? transToPhysicianId);
        RequestClient? Blockname(int id);
        RequestClient? GetRequestclient(int id);
        void AddBlockRequest(BlockRequest blreq);
        List<RequestWiseFile> getfile(int id);
        Request? getdetail(int id);
        void AddRequestWiseFiles(RequestWiseFile requestWiseFile);
        RequestWiseFile GetDocumentFile(int id);
        void update_RequestWiseTable(RequestWiseFile df);
        List<RequestWiseFile> GetDocumentList(int id);
        List<HealthProfessionalType> GetHealthprofessionalByType();
        List<HealthProfessional> getvendorbyprofessiontype(int Profession);
        List<HealthProfessional> getdatabyvendorid(int id);
        OrderDetail GetOredrDetail(int id);
        void AddOrderdetails(OrderDetail oredr);

        List<string> GetAspNetRole(int? id);
        RequestClient getagreement(int id);
        void updaterequestclient(RequestClient REQ);
        List<Request> GetCountData();
        void AddRequestclosed(RequestClosed requestClosed);
        Request getreqid(int id);
        RequestClient getclient(int id);
        EncounterForm getencounterbyid(int id);
        void updateEncounterForm(EncounterForm ef);
        Admin getadminbyadminid(int admin);
        List<AdminRegion> getadminreg(int admin);
        string GetRegionname(int? regionId);
        AspNetUser getuserbyaspid(int aspNetUserId);
        List<Request> getINlist();
        AspNetUser getAsp(string email);
        void AddAspnetUser(AspNetUser aspnetuser2);
        Region isRegion(string abbreviation);
        Region AddRegion(Region region);
        void AddRequesttbl(Request request);
        void AddRequestClient(RequestClient requestclient);
        void updateadmintbl(Admin a);
        int? GetRegionid(string state);
        User getUser(string email);
        void AddUser(User user);
        bool IsAdminRegion(int ritem, int admin);
        void deletereg(int admin);
        void AddRegionbyid(int ritem, int admin);
        string Adminname(int admin1);
        void UpdateAsp(AspNetUser asp);
        List<Physician> getphysician();
        List<PhysicianLocation> GetPhyLocation();
        string Getrolebyroleid(int? roleId);
        PhysicianNotification phynoti(int physicianId);
        void updatephynoti(PhysicianNotification pl);
        void addPhynoti(PhysicianNotification p);
        List<Region> GetReg();
        List<Role> getrole();
        void AddPhysician(Physician phy);
        void AddPhysicianRegion(PhysicianRegion reg);
        ICollection<AspNetRole> PhycianRoles();
        List<Role> getroletbl();
        List<Menu> getmenutbl(int value);
        void AddRoletbl(Role role);
        void AddRoleMenutbl(RoleMenu rolemenu);
        void RemoveRoleMenu(int roleId);
        void AddRoleMenu(RoleMenu rolemenu);
        Role GetDataFromRoles(int? id);
        List<RoleMenu> GetDataFromRoleMenu(int id);
        List<Menu> GetMenuDataWithCheckwise(short accountType);
    }
}
