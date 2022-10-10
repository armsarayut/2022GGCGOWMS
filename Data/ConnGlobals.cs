using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace GoWMS.Server.Data
{
    public static class ConnGlobals
    {
        public static string GetConnLocalDB()
        {
            return "server=DESKTOP-NQ62BHU\\MSSQL; database=GoSQLCOMM;Trusted_Connection=True;";
        }

        #region SQL Database

    
        private static readonly string SqlServer = " 10.251.11.3"; // Develop ,Local : DESKTOP-NQ62BHU\MSSQL , Host : 203.159.93.86, GGC QAS : 10.251.11.3
        private static readonly string SqlDB = "GOSQL"; // Develop , Local : GOSQLGGC , Host : GOSQL
        private static readonly string SqlPort = "1433";
        private static readonly string SqlUser = "sa";
        private static readonly string SqlPass = "@ei0u2020";
        //private static readonly string NpgPass = "@ei0u";
        private static readonly string SqlContime = "120";


        /// <summary>
        /// GetConnLocalDBSQL
        /// </summary>
        /// <remarks>Get Database Connection string for GoWMS</remarks>
        /// <returns></returns>
        public static string GetConnDBSQL()
        {
            return "Server=" + SqlServer + " ," + SqlPort + ";Database=" + SqlDB + ";User Id=" + SqlUser + ";Password=" + SqlPass + ";Timeout=" + SqlContime + ";";
            //return "server=DESKTOP-NQ62BHU\\MSSQL; database=GoSQL-GGCQAS;Trusted_Connection=True;";

        }
        #endregion

        #region Local Database
        private static readonly string NpgServer = "localhost"; // Develop
        //private static readonly string NpgServer = "192.168.1.14"; // Production

        private static readonly string NpgDB = "WGCQASxx"; // Develop
        //private static readonly string NpgDB = "Gowes"; // Production

        private static readonly string NpgPort = "5432";
        
        private static readonly string NpgUser = "postgres";
        private static readonly string NpgPass = "@ei0u2020";
        //private static readonly string NpgPass = "@ei0u";
        private static readonly string NpgContime = "120";

        /// <summary>
        /// GetConnLocalDBPG
        /// </summary>
        /// <remarks>Get Database Connection string for GoWMS</remarks>
        /// <returns></returns>
        public static string GetConnLocalDBPG()
        {
            return "Server=" + NpgServer + " ;Port=" + NpgPort + ";Database=" + NpgDB + ";User Id=" + NpgUser + ";Password=" + NpgPass + ";Timeout=" + NpgContime + ";";
        }
        #endregion

        /// <summary>
        /// GetConnApiDB
        /// </summary>
        /// <remarks>Get Database Connection string for API GoWMS</remarks>
        /// <returns></returns>
        public static string GetConnApiDB()
        {
            return "Server=" + NpgServer + " ;Port=" + NpgPort + ";Database=" + NpgDB + ";User Id=" + NpgUser + ";Password=" + NpgPass + ";Timeout=" + NpgContime + ";";
        }

        #region ERP Darabase
        private static readonly string ErpServer = "203.150.202.44";
        private static readonly string ErpDB = "Staging";
        private static readonly string ErpUser = "ASRS";
        private static readonly string ErpPass = "ASRS";
        private static readonly string ErpContime = "60";
        /// <summary>
        /// GetConnErpDB
        /// </summary>
        /// <remarks>Get Database Connection String For SQL Server. This project is Prepack</remarks>
        /// <returns></returns>
        public static string GetConnErpDB()
        {
            return "Data Source=" + ErpServer + ";Initial Catalog=" + ErpDB + ";Persist Security Info=True;User ID=" + ErpUser + ";Password=" + ErpPass + ";Connect Timeout=" + ErpContime + ";";
            //return "server=DESKTOP-NQ62BHU\\MSSQL; database=GoSQL;Trusted_Connection=True;";
        }
        #endregion

        #region ERP WGC Site [Oracle 11.2G]
        private static readonly string oraServer = "192.168.1.143";
        private static readonly string oraPort = "1521";
        //private static readonly string oraSource = "ASRS_WMS";
        private static readonly string oraUser = "WGRB";
        private static readonly string oraPass = "WGRB2021LINK";
        private static readonly string oraDB = "orcl";

        /// <summary>
        /// GetConnErpDBWCG
        /// </summary>
        /// <remarks>Get Database Connection String For Oracle. This project is WGC</remarks>
        /// <returns></returns>
        public static string GetConnErpDBWCG()
        {
            //Using Oracle.ManagedDataAccess.Core without tnsnames.ora
            //return "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=" + oraServer + ")(PORT=" + oraPort + ")))(CONNECT_DATA=(SERVER=DEDICATED)(SID = " + oraDB + ")));User Id=" + oraUser + ";Password=" + oraPass + ";";
            return "Provider = OraOLEDB.Oracle; Data Source = " + oraUser +"; User Id = " + oraUser +"; Password = "+ oraPass+"; OLEDB.NET = True;";
        }
        #endregion
    }
}
