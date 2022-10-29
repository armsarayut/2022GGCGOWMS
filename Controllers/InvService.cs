using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoWMS.Server.Data;
using GoWMS.Server.Models;
using GoWMS.Server.Models.Inv;


namespace GoWMS.Server.Controllers
{
    public class InvService
    {
        readonly InvDAL objDAL = new InvDAL();

        public Task<IEnumerable<InvStockList>> GetStckList()
        {
            return objDAL.GetStockList();
        }
        /*
        public List<InvStockList> GetStckList()
        {
            List<InvStockList> ListRet = objDAL.GetStockList().ToList();
            return ListRet;
        }
        */
        public List<InvStockSum> GetStockSum()
        {
            List<InvStockSum> ListRet = objDAL.GetStockSum().ToList();
            return ListRet;
        }

        public List<InvStockSumByLot> GetStockSumByLot()
        {
            List<InvStockSumByLot> ListRet = objDAL.GetStockSumByLot().ToList();
            return ListRet;
        }

        public List<Vrpt_shelf_listInfo> GetShelfList()
        {
            List<Vrpt_shelf_listInfo> ListRet = objDAL.GetShelfLocation().ToList();
            return ListRet;
        }
        public Task<IEnumerable<Inv_Stock_GoInfo>> GetStockListInfo()
        {
            return objDAL.GetStockListInfo();
        }

        public List<InvStockSumByCus> GetStockSumByCustomer()
        {
            List<InvStockSumByCus> ListRet = objDAL.GetStockSumByCustomer().ToList();
            return ListRet;
        }
    }
}
