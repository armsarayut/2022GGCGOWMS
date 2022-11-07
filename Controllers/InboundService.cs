using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoWMS.Server.Data;
using GoWMS.Server.Models;
using GoWMS.Server.Models.Inb;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Emit;

namespace GoWMS.Server.Controllers
{
    public class InboundService
    {
        readonly InbDAL objDAL = new InbDAL();

        public List<Inb_Goodreceipt_Go> GetAllInbGoodreceiptGos()
        {
            List<Inb_Goodreceipt_Go> retlist = objDAL.GetAllInbGoodreceiptGo().ToList();
            return retlist;
        }

        public List<Inb_Goodreceive_Go> GetAllInbGoodreceiveGos()
        {
            List<Inb_Goodreceive_Go> retlist = objDAL.GetAllInbGoodreceiveGo().ToList();
            return retlist;
        }

        public List<Inb_Goodreceive_Go> GetAllInbGoodreceiveGobyTag(string sTag)
        {
            List<Inb_Goodreceive_Go> retlist = objDAL.GetAllInbGoodreceiveGobyTag(sTag).ToList();
            return retlist;
        }


        

        public List<Inb_Putaway_Go> GetAllInbPutawayGos()
        {
            List<Inb_Putaway_Go> retlist = objDAL.GetAllInbPutawayGo().ToList();
            return retlist;
        }

        public List<Inb_Putaway_Go> GetAllInbPutawayGosByPallet(string pallet)
        {
            List<Inb_Putaway_Go> retlist = objDAL.GetAllInbPutawayGoBypallet(pallet).ToList();
            return retlist;
        }

        public string SetStorageComplete(string pallet, string bin)
        {
            objDAL.SetStorageComplete(pallet, bin);
            return "Map Successfully";
        }

        public Task<Int64> GetSumOrderAllInbGoodreceiptGo()
        {
            return objDAL.GetSumOrderAllInbGoodreceiptGo();
        }

        public Task<Int64> GetSumPalletAllInbGoodreceiptGo()
        {
            return objDAL.GetSumPalletAllInbGoodreceiptGo();
        }


        public Task<Int64> GetSumPalletAllOubGoodPickingGo()
        {
            return objDAL.GetSumPalletAllOubGoodPickingGo();
        }

        public Task<Int64> GetSumOrderAllOubGoodPickingGo()
        {
            return objDAL.GetSumOrderAllOubGoodPickingGo();
        }

        public bool CancelReceivingOrdersBypack(string pallet, string pack)
        {
            bool bret = objDAL.CancelReceivingOrdersBypack(pallet, pack);

            return bret;
        }

        public bool CancelReceivingOrdersBypackId(long transId, decimal tramsQty, string tranDoc, string tranItem , string tranTag)
        {
            bool bret = objDAL.CancelReceivingOrdersBypackId(transId, tramsQty, tranDoc, tranItem, tranTag);

            return bret;
        }



        public bool CancelReceivingOrdersByPallet(string pallet)
        {
            bool bret = objDAL.CancelReceivingOrdersByPallet(pallet);

            return bret;
        }

        public bool CreateTagno(Int64 valapiref, Int32 valpalletfrom, Int32 valpalletto, string valtranref, string valtrancreateby, DataTable valTransData , string valuuid)
        {
            bool bret = objDAL.CreateTagno(valapiref, valpalletfrom, valpalletto, valtranref, valtrancreateby, valTransData, valuuid);

            return bret;
        }

        public bool CreateTagnodup(Int64 valapiref, Int32 valpalletfrom, Int32 valpalletto, string valtranref, string valtrancreateby, DataTable valTransData, string valuuid, ref string strReturn)
        {
            bool bret = objDAL.CreateTagnodup(valapiref, valpalletfrom, valpalletto, valtranref, valtrancreateby, valTransData, valuuid, ref strReturn);

            return bret;
        }


        

        public bool CreateTagnoACC(Int64 valapiref, Int32 valpalletfrom, Int32 valpalletto, string valtranref, string valtrancreateby, DataTable valTransData, string valuuid)
        {
            bool bret = objDAL.CreateTagnoACC(valapiref, valpalletfrom, valpalletto, valtranref, valtrancreateby, valTransData, valuuid);

            return bret;
        }



        public List<ggcTag4x4> GetTagGGCByIndex(long apiid)
        {
            List<ggcTag4x4> retlist = objDAL.GetTagGGCByIndex(apiid).ToList();
            return retlist;
        }

        public List<ggcTag4x4> GetTagGGCByIndexKey(long apiid, string sKey)
        {
            List<ggcTag4x4> retlist = objDAL.GetTagGGCByIndexKey(apiid, sKey).ToList();
            return retlist;
        }

        

        public bool CreatePutawayWms(string lpncode, string tagcode, string gatecode, string worktype, ref string retstring)
        {
            bool bret = objDAL.CreatePutawayWms(lpncode, tagcode, gatecode, worktype, ref retstring);

            return bret;
        }

        public bool CreatePutawayEmpty(string lpncode, string tagcode, string gatecode, string worktype, ref string retstring)
        {
            bool bret = objDAL.CreatePutawayEmpty(lpncode, tagcode, gatecode, worktype, ref retstring);

            return bret;
        }

        public bool ManageGRAccessory(long idx, string itemcode, double totalqty, string createby, string batchno, DateTime recivedate, string remark, String docref, Int32 managecase, ref string retmessage)
        {
            bool bret = objDAL.ManageGRAccessory(idx, itemcode, totalqty, createby, batchno, recivedate, remark, docref, managecase, ref retmessage);

            return bret;
        }

    }
}
