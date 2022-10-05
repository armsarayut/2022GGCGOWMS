using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoWMS.Server.Models.Wcs;
using GoWMS.Server.Data;

namespace GoWMS.Server.Controllers
{
    public class WcsService
    {
        readonly WcsDAL objDAL = new WcsDAL();

        public List<Vmachine> GetAllMachine()
        {
            List<Vmachine> retlist = objDAL.GetAllMachine().ToList();
            return retlist;
        }

        public List<Vmachine_command> GetCommandMachine(string mccode)
        {
            List<Vmachine_command> retlist = objDAL.GetCommandMachine(mccode).ToList();
            return retlist;
        }
        public Boolean CreateCommandMC(string mccode, Int32 command)
        {
            Boolean bRet = false;
            bRet = objDAL.CreateCommandMC(mccode, command);
            return bRet;
        }

        public Boolean CreatePotocalMC(string mccode, Int32 startpos, Int32 stoppos, Int32 unittyp, string palletid, Int32 weight, Int32 command)
        {
            Boolean bRet = false;
            bRet = objDAL.CreatePotocalMC(mccode, startpos, stoppos, unittyp, palletid, weight, command);
            return bRet;
        }


        public List<Vset_gate_rgv> GetGateRgv()
        {
            List<Vset_gate_rgv> retlist = objDAL.GetGateRgv().ToList();
            return retlist;
        }

        public List<Tas_WorksInfo> GetASRSWork()
        {
            List<Tas_WorksInfo> retlist = objDAL.GetASRSWork().ToList();
            return retlist;
        }

        public List<Tas_Mcworks> GetASRSWorks()
        {
            List<Tas_Mcworks> retlist = objDAL.GetASRSWorks().ToList();
            return retlist;
        }

        public List<Tas_Rgvworks> GetRGVWorks()
        {
            List<Tas_Rgvworks> retlist = objDAL.GetRGVWorks().ToList();
            return retlist;
        }

        public List<AsrsPerformance> GetAsrsPerformance(DateTime Fmtime, DateTime Totime)
        {
            List<AsrsPerformance> retlist = objDAL.GetAsrsPerformance(Fmtime, Totime).ToList();
            return retlist;
        }

        public List<SetConstance> GetConstance()
        {
            List<SetConstance> retlist = objDAL.GetConstance().ToList();
            return retlist;
        }

        public bool SetConstance(string setcode, Int32 setval)
        {
            bool bret = objDAL.SetConstance(setcode, setval);

            return bret;
        }

        public List<Set_Operate> GetOperate()
        {
            List<Set_Operate> retlist = objDAL.GetOperate().ToList();
            return retlist;
        }

        public bool SetOperate(Int64 setcode, bool setval)
        {
            bool bret = objDAL.SetOperate(setcode, setval);
            return bret;
        }

        public bool SetEnableConstance(String setcode, bool setval)
        {
            bool bret = objDAL.SetEnableConstance(setcode, setval);
            return bret;
        }

        public Boolean SetRelocation(string stcode, bool valbit, ref string msgret)
        {
            bool bret = objDAL.SetRelocation(stcode, valbit, ref msgret);
            return bret;
        }



        public List<Set_Srm_Operate> GetSRMOperate()
        {
            List<Set_Srm_Operate> retlist = objDAL.GetSRMOperate().ToList();
            return retlist;
        }

        public bool SetSRMOperate(Int64 setcode, bool setvalint, bool setvalout)
        {
            bool bret = objDAL.SetSRMOperate(setcode, setvalint, setvalout);
            return bret;
        }

        public List<AsrsLoadtime> GetAsrsloadtime(DateTime stime, DateTime etime)
        {
            List<AsrsLoadtime> retlist = objDAL.GetAsrsloadtime(stime, etime).ToList();
            return retlist;
        }

        public List<Rpt_Ejectgate> GetReportEject(DateTime stime, DateTime etime)
        {
            List<Rpt_Ejectgate> retlist = objDAL.GetReportEject(stime, etime).ToList();
            return retlist;
        }
        public bool SetUrgentAsrsQueueByPallet(string spallet)
        {
            bool bret = objDAL.SetUrgentAsrsQueueByPallet(spallet);

            return bret;
        }

    }
}
