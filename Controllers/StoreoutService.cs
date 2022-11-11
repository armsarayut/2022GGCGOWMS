using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoWMS.Server.Data;
using GoWMS.Server.Models;
using GoWMS.Server.Models.Oub;
using Microsoft.AspNetCore.Mvc;

namespace GoWMS.Server.Controllers
{
    public class StoreoutService
    {
        readonly OubDAL objDAL = new OubDAL();


        public List<Sap_Storeout> GetAllSapStoreout()
        {
            List<Sap_Storeout> retlist = objDAL.GetAllSapStoreout().ToList();
            return retlist;
        }

        public List<Sap_Storeout> GetSapStoreoutSetBatch()
        {
            List<Sap_Storeout> retlist = objDAL.GetSapStoreoutSetBatch().ToList();
            return retlist;
        }
        public string GetRunnning(string sSeq, Int32 iPad)
        {
            string sRunning = "";
            sRunning = objDAL.GetRunnning(sSeq, iPad);
            return sRunning;
        }
        public Boolean CreateBatchOrder(DateTime deliverydate, Int32 deliveryprio, string orderno, string shiptocode, string sSeq)
        {
            Boolean bRet = false;
            bRet = objDAL.CreateBatchOrder(deliverydate, deliveryprio, orderno, shiptocode, sSeq);
            return bRet;
        }

        public Boolean CreateBatchSetting(string sSeq, Int32 istation)
        {
            Boolean bRet = false;
            bRet = objDAL.CreateBatchSetting(sSeq, istation);
            return bRet;
        }

        public Boolean RollbackBatch(string sSeq)
        {
            Boolean bRet = false;
            bRet = objDAL.RollbackBatch(sSeq);
            return bRet;
        }

        public Boolean StartBatchsetting(string sSeq, Int32 istation)
        {
            Boolean bRet = false;
            bRet = objDAL.StartBatchsetting(sSeq, istation);
            return bRet;
        }

        public List<Oub_CustomerReturn> GetAllCustomerReturn()
        {
            List<Oub_CustomerReturn> retlist = objDAL.GetAllCustomerReturn().ToList();
            return retlist;
        }

        public Boolean UpdateStockOut(string lpncode, ref string msgret)
        {
            Boolean bRet = false;
            bRet = objDAL.UpdateStockOut(lpncode, ref msgret);
            return bRet;
        }





    }
}
