using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoWMS.Server.Data;
using GoWMS.Server.Models;
using GoWMS.Server.Models.Mas;

namespace GoWMS.Server.Controllers
{
    public class MasService
    {
        readonly MasDAL objDAL = new MasDAL();

        public List<Mas_Pallet_Go> GetAllMasterPallets()
        {
            List<Mas_Pallet_Go> retlist = objDAL.GetAllMasterpalletGo().ToList();
            return retlist;
        }

        public List<Mas_Storagebin_Go> GetAllMasterStorageBins()
        {
            List<Mas_Storagebin_Go> retlist = objDAL.GetAllStorageBinGo().ToList();
            return retlist;
        }

        public List<Mas_Item_Go> GetAllMasterItems()
        {
            List<Mas_Item_Go> retlist = objDAL.GetAllMasteritemGo().ToList();
            return retlist;
        }

        public List<Mas_Storage_Go> GetAllMasterStorages()
        {
            List<Mas_Storage_Go> retlist = objDAL.GetAllMasterstorageGo().ToList();
            return retlist;
        }

        public List<Mas_Worktype_Go> GetAllMasterWorktypes()
        {
            List<Mas_Worktype_Go> retlist = objDAL.GetAllMasterworktypeGo().ToList();
            return retlist;
        }

        public List<Mas_Status_Go> GetAllMasterStatus()
        {
            List<Mas_Status_Go> retlist = objDAL.GetAllMasterstatusGo().ToList();
            return retlist;
        }

        public List<Mas_Reason_Go> GetAllMasterReasons()
        {
            List<Mas_Reason_Go> retlist = objDAL.GetAllMasterreasonGo().ToList();
            return retlist;
        }

        public Boolean ValidateMasterpallet(string spallet)
        {
            Boolean bret = false;
            bret = objDAL.ValidateMasterpallet(spallet);
            return bret;
        }

        public Boolean ManageMasterAccessory(long idx, string itemcode, string itemname, double packqty, string packuon, string uom, double gosweight, double netweight, bool batchmanage, Int32 managecase, ref string retmessage)
        {
            Boolean bret = false;
            bret = objDAL.ManageMasterAccessory(idx, itemcode, itemname, packqty, packuon, uom, gosweight, netweight, batchmanage, managecase , ref retmessage);
            return bret;
        }

        public bool UpdateItemMasterPalletqty(decimal? Palqty, string Itemcode)
        {
            Boolean bret = false;
            bret = objDAL.UpdateItemMasterPalletqty(Palqty, Itemcode);
            return bret;
        }

    }
}
