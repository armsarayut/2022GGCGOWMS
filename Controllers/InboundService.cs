using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoWMS.Server.Data;
using GoWMS.Server.Models;
using GoWMS.Server.Models.Inb;
using Microsoft.AspNetCore.Mvc;

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

        public bool CancelReceivingOrdersByPallet(string pallet)
        {
            bool bret = objDAL.CancelReceivingOrdersByPallet(pallet);

            return bret;
        }

    }
}
