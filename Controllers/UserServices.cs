using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoWMS.Server.Data;
using GoWMS.Server.Models;


namespace GoWMS.Server.Controllers
{
    public class UserServices
    {
        readonly UserDAL objDAL = new UserDAL();

        public List<Userinfo> GetUserAll()
        {
            List<Userinfo> retlist = objDAL.GetUserAll().ToList();
            return retlist;
        }
        public List<Usergroups> GetUsergroups()
        {
            List<Usergroups> retlist = objDAL.GetUsergroups().ToList();
            return retlist;
        }

        public List<Userinfo> GetUser(string uusname, string uspass)
        {
            List<Userinfo> retlist = objDAL.GetUser(uusname, uspass).ToList();
            return retlist;
        }

        public List<Userinfo> GetUserAD(string uusname, string uspass)
        {
            List<Userinfo> retlist = objDAL.GetUserAD(uusname, uspass).ToList();
            return retlist;
        }

        

        public List<Userroleinfo> GetUserRole(string menu_desc, long group_id)
        {
            List<Userroleinfo> retlist = objDAL.GetUserRole(menu_desc, group_id).ToList();
            return retlist;
        }

        //public async Task<ResultReturn> InsertMenu(string mnuName, string mundesc)

        public async Task<ResultReturn> InsertMenu(string mnuName, string mundesc)
        {
            ResultReturn retlist = await Task.Run(() => objDAL.InsertMenu(mnuName, mundesc));
            return retlist;
        }

        public async Task<ResultReturn> InsertPrivilege(string mnuName)
        {
            ResultReturn retlist = await Task.Run(() => objDAL.InsertPrivilege(mnuName));
            return retlist;
        }

        public List<UserPrivilege> GetPrivilegeAll()
        {
            List<UserPrivilege> retlist = objDAL.GetPrivilegeAll().ToList();
            return retlist;
        }

        public bool SetPrivilege(long idx, bool acc, bool add, bool edi, bool del, bool rpt, bool apv)
        {
            bool bret = objDAL.SetPrivilege(idx, acc, add, edi, del, rpt, apv);

            return bret;
        }

        public async Task<ResultReturn> UpsertRegister(long ugid, string usid, string uspass, string usfirstname)
        {
            ResultReturn retlist = await Task.Run(() => objDAL.UpsertRegister(ugid, usid, uspass, usfirstname));
            return retlist;
        }

        public ResultReturn UpsertRegisterAD(long ugid, string usid, string uspass, string usfirstname)
        {
            ResultReturn retlist = objDAL.UpsertRegisterAD(ugid, usid, uspass, usfirstname);
            
            return retlist;
        }


        


    }
}
