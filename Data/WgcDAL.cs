using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using GoWMS.Server.Models.Wgc;
using System.Data.OleDb;
using GoWMS.Server.Controllers;
using GoWMS.Server.Models;
using GoWMS.Server.Models.Erp;
using GoWMS.Server.Models.Api;
using GoWMS.Server.Models.Inb;


namespace GoWMS.Server.Data
{
    public class WgcDAL
    {
        readonly private string connectionString = ConnGlobals.GetConnErpDBWCG();

        public async Task<IEnumerable<BOOKING_NOTE_ITEMS>> GetAllApiBooking_note()
        {
            Environment.SetEnvironmentVariable("NLS_LANG",
                    "AMERICAN_AMERICA.WE8MSWIN1252",
                    EnvironmentVariableTarget.Process);

            List<BOOKING_NOTE_ITEMS> lstobj = new List<BOOKING_NOTE_ITEMS>();
            StringBuilder sql = new StringBuilder();

            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                //StringBuilder sql = new StringBuilder();
                sql.Clear();
                sql.AppendLine("SELECT t1.*, t2.ITEM_NAME ");
                sql.AppendLine("FROM WGRB.BOOKING_NOTE_ITEMS t1");
                sql.AppendLine("LEFT JOIN WGRB.ITEMS t2 ON t1.ITEM_CODE = t2.ITEM_CODE");
                sql.AppendLine("WHERE t1.PALLET_GO is not null");
                sql.AppendLine("AND t1.PALLET_GO like 'WGC-%'");
                //sql.AppendLine("AND STATUS_GO is not null");
                sql.AppendLine("ORDER BY t1.SEQ_NO");
                
                try
                {
                    OleDbCommand cmd = new OleDbCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    //await con.OpenAsync();
                    con.Open();

                    
                    OleDbDataReader rdr = cmd.ExecuteReader();
                    while ( await rdr.ReadAsync())
                    {
                    
                        BOOKING_NOTE_ITEMS objrd = new BOOKING_NOTE_ITEMS
                        {
                            BK_NO = rdr["BK_NO"].ToString(),
                            SEQ_NO = rdr["SEQ_NO"] == DBNull.Value ? null : (Decimal?)rdr["SEQ_NO"],
                            DN_NO = rdr["DN_NO"].ToString(),
                            DN_SEQ = rdr["DN_SEQ"] == DBNull.Value ? null : (Decimal?)rdr["DN_SEQ"],
                            SO_NO = rdr["SO_NO"].ToString(),
                            SO_SEQ = rdr["SO_SEQ"] == DBNull.Value ? null : (Decimal?)rdr["SO_SEQ"],
                            JOB_NO = rdr["JOB_NO"].ToString(),
                            ITEM_CODE = rdr["ITEM_CODE"].ToString(),
                            DN_QTY = rdr["DN_QTY"] == DBNull.Value ? null : (Decimal?)rdr["DN_QTY"],
                            DN_UM = rdr["DN_UM"].ToString(),
                            DN_DATE = rdr["DN_DATE"] == DBNull.Value ? null : (DateTime?)rdr["DN_DATE"],
                            STATUS = rdr["STATUS"].ToString(),
                            CUSTOMER_CODE = rdr["CUSTOMER_CODE"].ToString(),
                            BATCH_NO = rdr["BATCH_NO"].ToString(),
                            FACTORY_ROOM = rdr["FACTORY_ROOM"].ToString(),
                            PALETTE_NO = rdr["PALETTE_NO"].ToString(),
                            PCK_NO = rdr["PCK_NO"] == DBNull.Value ? null : (Decimal?)rdr["PCK_NO"],
                            BOX_NO = rdr["BOX_NO"] == DBNull.Value ? null : (Decimal?)rdr["BOX_NO"],
                            CLOSE_FLAG = rdr["CLOSE_FLAG"].ToString(),
                            PACKETING_CODE = rdr["PACKETING_CODE"].ToString(),
                            DN_WEIGHT = rdr["DN_WEIGHT"] == DBNull.Value ? null : (Decimal?)rdr["DN_WEIGHT"],
                            DN_WEIGHT_UM = rdr["DN_WEIGHT_UM"].ToString(),
                            LOT_NO = rdr["LOT_NO"].ToString(),
                            STORE_ID = rdr["STORE_ID"].ToString(),
                            SCREEN_NAME = rdr["SCREEN_NAME"].ToString(),
                            CUST_ITEM_CODE = rdr["CUST_ITEM_CODE"].ToString(),
                            VEHICLE_CODE = rdr["VEHICLE_CODE"].ToString(),
                            VEHICLE_LINK_CODE = rdr["VEHICLE_LINK_CODE"].ToString(),
                            PACKAGE_WEIGHT = rdr["PACKAGE_WEIGHT"] == DBNull.Value ? null : (Decimal?)rdr["PACKAGE_WEIGHT"],
                            PK_NO = rdr["PK_NO"].ToString(),
                            PK_SEQ = rdr["PK_SEQ"] == DBNull.Value ? null : (Decimal?)rdr["PK_SEQ"],
                            STICKER_CODE = rdr["STICKER_CODE"].ToString(),
                            SUB_CUSTOMER_CODE = rdr["SUB_CUSTOMER_CODE"].ToString(),
                            TO_PCK_NO = rdr["TO_PCK_NO"].ToString(),
                            EFFECT_TYPE = rdr["EFFECT_TYPE"].ToString(),
                            REMARKS = rdr["REMARKS"].ToString(),
                            BOOKING_MODEL_NO = rdr["BOOKING_MODEL_NO"].ToString(),
                            REQUEST_DATE = rdr["REQUEST_DATE"] == DBNull.Value ? null : (DateTime?)rdr["REQUEST_DATE"],
                            SEQ_TR = rdr["SEQ_TR"] == DBNull.Value ? null : (Decimal?)rdr["SEQ_TR"],
                            TANK_SIZE = rdr["TANK_SIZE"].ToString(),
                            HUMAN_DRIVE_CODE = rdr["HUMAN_DRIVE_CODE"].ToString(),
                            TIME_LOAD_PRODUCT = rdr["TIME_LOAD_PRODUCT"].ToString(),
                            QTY_REMARKS = rdr["QTY_REMARKS"].ToString(),
                            PACKING_DATE = rdr["PACKING_DATE"] == DBNull.Value ? null : (DateTime?)rdr["PACKING_DATE"],
                            SHELF_LIFE_DATE = rdr["SHELF_LIFE_DATE"] == DBNull.Value ? null : (DateTime?)rdr["SHELF_LIFE_DATE"],
                            SHELF_LIFE_DAY = rdr["SHELF_LIFE_DAY"] == DBNull.Value ? null : (Decimal?)rdr["SHELF_LIFE_DAY"],
                            SHELF_LIFE_KEEP_IN = rdr["SHELF_LIFE_KEEP_IN"] == DBNull.Value ? null : (Decimal?)rdr["SHELF_LIFE_KEEP_IN"],
                            SHELF_LIFE_KEEP_IN_DATE = rdr["SHELF_LIFE_KEEP_IN_DATE"] == DBNull.Value ? null : (DateTime?)rdr["SHELF_LIFE_KEEP_IN_DATE"],
                            LOT_CORN = rdr["LOT_CORN"].ToString(),
                            HIT_LET = rdr["HIT_LET"].ToString(),
                            PALLET_GO = rdr["PALLET_GO"].ToString(),
                            STATUS_GO = rdr["STATUS_GO"].ToString(),
                            UPDATE_DATE_GO = rdr["UPDATE_DATE_GO"] == DBNull.Value ? null : (DateTime?)rdr["UPDATE_DATE_GO"],
                            CREATED_DATE = rdr["CREATED_DATE"] == DBNull.Value ? null : (DateTime?)rdr["CREATED_DATE"],
                            ITEM_NAME = rdr["ITEM_NAME"].ToString()

                        };
                        lstobj.Add(objrd);
                    }
                }
                catch (Exception e)
                {
                    throw;
                }
                con.Close();
            }
            return lstobj;
        }

        public async Task<IEnumerable<BOOKING_NOTE_ITEMS>> GetAllApiNewBooking_note()
        {
            Environment.SetEnvironmentVariable("NLS_LANG",
                    "AMERICAN_AMERICA.WE8MSWIN1252",
                    EnvironmentVariableTarget.Process);

            List<BOOKING_NOTE_ITEMS> lstobj = new List<BOOKING_NOTE_ITEMS>();
            StringBuilder sql = new StringBuilder();

            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                //StringBuilder sql = new StringBuilder();
                sql.Clear();
                sql.AppendLine("SELECT t1.*, t2.ITEM_NAME ");
                sql.AppendLine("FROM WGRB.BOOKING_NOTE_ITEMS t1");
                sql.AppendLine("LEFT JOIN WGRB.ITEMS t2 ON t1.ITEM_CODE = t2.ITEM_CODE");
                sql.AppendLine("WHERE t1.PALLET_GO is not null");
                sql.AppendLine("AND t1.STATUS_GO is null");
                sql.AppendLine("AND t1.REMARKS is null");
                sql.AppendLine("AND t1.PALLET_GO like 'WGC-%'");
                sql.AppendLine("ORDER BY t1.SEQ_NO");
                try
                {
                    OleDbCommand cmd = new OleDbCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    //await con.OpenAsync();
                    con.Open();

                    OleDbDataReader rdr = cmd.ExecuteReader();
                    while (await rdr.ReadAsync())
                    {

                        BOOKING_NOTE_ITEMS objrd = new BOOKING_NOTE_ITEMS
                        {
                            BK_NO = rdr["BK_NO"].ToString(),
                            SEQ_NO = rdr["SEQ_NO"] == DBNull.Value ? null : (Decimal?)rdr["SEQ_NO"],
                            DN_NO = rdr["DN_NO"].ToString(),
                            DN_SEQ = rdr["DN_SEQ"] == DBNull.Value ? null : (Decimal?)rdr["DN_SEQ"],
                            SO_NO = rdr["SO_NO"].ToString(),
                            SO_SEQ = rdr["SO_SEQ"] == DBNull.Value ? null : (Decimal?)rdr["SO_SEQ"],
                            JOB_NO = rdr["JOB_NO"].ToString(),
                            ITEM_CODE = rdr["ITEM_CODE"].ToString(),
                            DN_QTY = rdr["DN_QTY"] == DBNull.Value ? null : (Decimal?)rdr["DN_QTY"],
                            DN_UM = rdr["DN_UM"].ToString(),
                            DN_DATE = rdr["DN_DATE"] == DBNull.Value ? null : (DateTime?)rdr["DN_DATE"],
                            STATUS = rdr["STATUS"].ToString(),
                            CUSTOMER_CODE = rdr["CUSTOMER_CODE"].ToString(),
                            BATCH_NO = rdr["BATCH_NO"].ToString(),
                            FACTORY_ROOM = rdr["FACTORY_ROOM"].ToString(),
                            PALETTE_NO = rdr["PALETTE_NO"].ToString(),
                            PCK_NO = rdr["PCK_NO"] == DBNull.Value ? null : (Decimal?)rdr["PCK_NO"],
                            BOX_NO = rdr["BOX_NO"] == DBNull.Value ? null : (Decimal?)rdr["BOX_NO"],
                            CLOSE_FLAG = rdr["CLOSE_FLAG"].ToString(),
                            PACKETING_CODE = rdr["PACKETING_CODE"].ToString(),
                            DN_WEIGHT = rdr["DN_WEIGHT"] == DBNull.Value ? null : (Decimal?)rdr["DN_WEIGHT"],
                            DN_WEIGHT_UM = rdr["DN_WEIGHT_UM"].ToString(),
                            LOT_NO = rdr["LOT_NO"].ToString(),
                            STORE_ID = rdr["STORE_ID"].ToString(),
                            SCREEN_NAME = rdr["SCREEN_NAME"].ToString(),
                            CUST_ITEM_CODE = rdr["CUST_ITEM_CODE"].ToString(),
                            VEHICLE_CODE = rdr["VEHICLE_CODE"].ToString(),
                            VEHICLE_LINK_CODE = rdr["VEHICLE_LINK_CODE"].ToString(),
                            PACKAGE_WEIGHT = rdr["PACKAGE_WEIGHT"] == DBNull.Value ? null : (Decimal?)rdr["PACKAGE_WEIGHT"],
                            PK_NO = rdr["PK_NO"].ToString(),
                            PK_SEQ = rdr["PK_SEQ"] == DBNull.Value ? null : (Decimal?)rdr["PK_SEQ"],
                            STICKER_CODE = rdr["STICKER_CODE"].ToString(),
                            SUB_CUSTOMER_CODE = rdr["SUB_CUSTOMER_CODE"].ToString(),
                            TO_PCK_NO = rdr["TO_PCK_NO"].ToString(),
                            EFFECT_TYPE = rdr["EFFECT_TYPE"].ToString(),
                            REMARKS = rdr["REMARKS"].ToString(),
                            BOOKING_MODEL_NO = rdr["BOOKING_MODEL_NO"].ToString(),
                            REQUEST_DATE = rdr["REQUEST_DATE"] == DBNull.Value ? null : (DateTime?)rdr["REQUEST_DATE"],
                            SEQ_TR = rdr["SEQ_TR"] == DBNull.Value ? null : (Decimal?)rdr["SEQ_TR"],
                            TANK_SIZE = rdr["TANK_SIZE"].ToString(),
                            HUMAN_DRIVE_CODE = rdr["HUMAN_DRIVE_CODE"].ToString(),
                            TIME_LOAD_PRODUCT = rdr["TIME_LOAD_PRODUCT"].ToString(),
                            QTY_REMARKS = rdr["QTY_REMARKS"].ToString(),
                            PACKING_DATE = rdr["PACKING_DATE"] == DBNull.Value ? null : (DateTime?)rdr["PACKING_DATE"],
                            SHELF_LIFE_DATE = rdr["SHELF_LIFE_DATE"] == DBNull.Value ? null : (DateTime?)rdr["SHELF_LIFE_DATE"],
                            SHELF_LIFE_DAY = rdr["SHELF_LIFE_DAY"] == DBNull.Value ? null : (Decimal?)rdr["SHELF_LIFE_DAY"],
                            SHELF_LIFE_KEEP_IN = rdr["SHELF_LIFE_KEEP_IN"] == DBNull.Value ? null : (Decimal?)rdr["SHELF_LIFE_KEEP_IN"],
                            SHELF_LIFE_KEEP_IN_DATE = rdr["SHELF_LIFE_KEEP_IN_DATE"] == DBNull.Value ? null : (DateTime?)rdr["SHELF_LIFE_KEEP_IN_DATE"],
                            LOT_CORN = rdr["LOT_CORN"].ToString(),
                            HIT_LET = rdr["HIT_LET"].ToString(),
                            PALLET_GO = rdr["PALLET_GO"].ToString(),
                            STATUS_GO = rdr["STATUS_GO"].ToString(),
                            UPDATE_DATE_GO = rdr["UPDATE_DATE_GO"] == DBNull.Value ? null : (DateTime?)rdr["UPDATE_DATE_GO"],
                            CREATED_DATE = rdr["CREATED_DATE"] == DBNull.Value ? null : (DateTime?)rdr["CREATED_DATE"],
                            ITEM_NAME = rdr["ITEM_NAME"].ToString()
                        };
                        lstobj.Add(objrd);
                    }
                }
                catch (Exception e)
                {
                    throw;
                }
                con.Close();
            }
            return lstobj;
        }

        public async Task<IEnumerable<BOOKING_NOTE_ITEMS>> GetAllApiNewBooking_noteFilter()
        {
            Environment.SetEnvironmentVariable("NLS_LANG",
                    "AMERICAN_AMERICA.WE8MSWIN1252",
                    EnvironmentVariableTarget.Process);

            List<BOOKING_NOTE_ITEMS> lstobj = new List<BOOKING_NOTE_ITEMS>();
            StringBuilder sql = new StringBuilder();

            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                //StringBuilder sql = new StringBuilder();
                sql.Clear();
                sql.AppendLine("SELECT t1.*, t2.ITEM_NAME ");
                sql.AppendLine("FROM WGRB.BOOKING_NOTE_ITEMS t1");
                sql.AppendLine("LEFT JOIN WGRB.ITEMS t2 ON t1.ITEM_CODE = t2.ITEM_CODE");
                sql.AppendLine("WHERE t1.PALLET_GO is not null");
                sql.AppendLine("AND t1.STATUS_GO is null");
                sql.AppendLine("AND t1.REMARKS is null");
                sql.AppendLine("ORDER BY t1.SEQ_NO");
                try
                {
                    OleDbCommand cmd = new OleDbCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    //await con.OpenAsync();
                    con.Open();

                    OleDbDataReader rdr = cmd.ExecuteReader();
                    while (await rdr.ReadAsync())
                    {

                        BOOKING_NOTE_ITEMS objrd = new BOOKING_NOTE_ITEMS
                        {
                            BK_NO = rdr["BK_NO"].ToString(),
                            SEQ_NO = rdr["SEQ_NO"] == DBNull.Value ? null : (Decimal?)rdr["SEQ_NO"],
                            DN_NO = rdr["DN_NO"].ToString(),
                            DN_SEQ = rdr["DN_SEQ"] == DBNull.Value ? null : (Decimal?)rdr["DN_SEQ"],
                            SO_NO = rdr["SO_NO"].ToString(),
                            SO_SEQ = rdr["SO_SEQ"] == DBNull.Value ? null : (Decimal?)rdr["SO_SEQ"],
                            JOB_NO = rdr["JOB_NO"].ToString(),
                            ITEM_CODE = rdr["ITEM_CODE"].ToString(),
                            DN_QTY = rdr["DN_QTY"] == DBNull.Value ? null : (Decimal?)rdr["DN_QTY"],
                            DN_UM = rdr["DN_UM"].ToString(),
                            DN_DATE = rdr["DN_DATE"] == DBNull.Value ? null : (DateTime?)rdr["DN_DATE"],
                            STATUS = rdr["STATUS"].ToString(),
                            CUSTOMER_CODE = rdr["CUSTOMER_CODE"].ToString(),
                            BATCH_NO = rdr["BATCH_NO"].ToString(),
                            FACTORY_ROOM = rdr["FACTORY_ROOM"].ToString(),
                            PALETTE_NO = rdr["PALETTE_NO"].ToString(),
                            PCK_NO = rdr["PCK_NO"] == DBNull.Value ? null : (Decimal?)rdr["PCK_NO"],
                            BOX_NO = rdr["BOX_NO"] == DBNull.Value ? null : (Decimal?)rdr["BOX_NO"],
                            CLOSE_FLAG = rdr["CLOSE_FLAG"].ToString(),
                            PACKETING_CODE = rdr["PACKETING_CODE"].ToString(),
                            DN_WEIGHT = rdr["DN_WEIGHT"] == DBNull.Value ? null : (Decimal?)rdr["DN_WEIGHT"],
                            DN_WEIGHT_UM = rdr["DN_WEIGHT_UM"].ToString(),
                            LOT_NO = rdr["LOT_NO"].ToString(),
                            STORE_ID = rdr["STORE_ID"].ToString(),
                            SCREEN_NAME = rdr["SCREEN_NAME"].ToString(),
                            CUST_ITEM_CODE = rdr["CUST_ITEM_CODE"].ToString(),
                            VEHICLE_CODE = rdr["VEHICLE_CODE"].ToString(),
                            VEHICLE_LINK_CODE = rdr["VEHICLE_LINK_CODE"].ToString(),
                            PACKAGE_WEIGHT = rdr["PACKAGE_WEIGHT"] == DBNull.Value ? null : (Decimal?)rdr["PACKAGE_WEIGHT"],
                            PK_NO = rdr["PK_NO"].ToString(),
                            PK_SEQ = rdr["PK_SEQ"] == DBNull.Value ? null : (Decimal?)rdr["PK_SEQ"],
                            STICKER_CODE = rdr["STICKER_CODE"].ToString(),
                            SUB_CUSTOMER_CODE = rdr["SUB_CUSTOMER_CODE"].ToString(),
                            TO_PCK_NO = rdr["TO_PCK_NO"].ToString(),
                            EFFECT_TYPE = rdr["EFFECT_TYPE"].ToString(),
                            REMARKS = rdr["REMARKS"].ToString(),
                            BOOKING_MODEL_NO = rdr["BOOKING_MODEL_NO"].ToString(),
                            REQUEST_DATE = rdr["REQUEST_DATE"] == DBNull.Value ? null : (DateTime?)rdr["REQUEST_DATE"],
                            SEQ_TR = rdr["SEQ_TR"] == DBNull.Value ? null : (Decimal?)rdr["SEQ_TR"],
                            TANK_SIZE = rdr["TANK_SIZE"].ToString(),
                            HUMAN_DRIVE_CODE = rdr["HUMAN_DRIVE_CODE"].ToString(),
                            TIME_LOAD_PRODUCT = rdr["TIME_LOAD_PRODUCT"].ToString(),
                            QTY_REMARKS = rdr["QTY_REMARKS"].ToString(),
                            PACKING_DATE = rdr["PACKING_DATE"] == DBNull.Value ? null : (DateTime?)rdr["PACKING_DATE"],
                            SHELF_LIFE_DATE = rdr["SHELF_LIFE_DATE"] == DBNull.Value ? null : (DateTime?)rdr["SHELF_LIFE_DATE"],
                            SHELF_LIFE_DAY = rdr["SHELF_LIFE_DAY"] == DBNull.Value ? null : (Decimal?)rdr["SHELF_LIFE_DAY"],
                            SHELF_LIFE_KEEP_IN = rdr["SHELF_LIFE_KEEP_IN"] == DBNull.Value ? null : (Decimal?)rdr["SHELF_LIFE_KEEP_IN"],
                            SHELF_LIFE_KEEP_IN_DATE = rdr["SHELF_LIFE_KEEP_IN_DATE"] == DBNull.Value ? null : (DateTime?)rdr["SHELF_LIFE_KEEP_IN_DATE"],
                            LOT_CORN = rdr["LOT_CORN"].ToString(),
                            HIT_LET = rdr["HIT_LET"].ToString(),
                            PALLET_GO = rdr["PALLET_GO"].ToString(),
                            STATUS_GO = rdr["STATUS_GO"].ToString(),
                            UPDATE_DATE_GO = rdr["UPDATE_DATE_GO"] == DBNull.Value ? null : (DateTime?)rdr["UPDATE_DATE_GO"],
                            CREATED_DATE = rdr["CREATED_DATE"] == DBNull.Value ? null : (DateTime?)rdr["CREATED_DATE"],
                            ITEM_NAME = rdr["ITEM_NAME"].ToString()
                        };
                        lstobj.Add(objrd);
                    }
                }
                catch (Exception e)
                {
                    throw;
                }
                con.Close();
            }
            return lstobj;
        }

        public async Task<IEnumerable<BOOKING_NOTE_ITEMS>> GetPalletApiNewBooking_note(string pallet)
        {
            Environment.SetEnvironmentVariable("NLS_LANG",
                    "AMERICAN_AMERICA.WE8MSWIN1252",
                    EnvironmentVariableTarget.Process);

            List<BOOKING_NOTE_ITEMS> lstobj = new List<BOOKING_NOTE_ITEMS>();
            StringBuilder sql = new StringBuilder();

            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                //StringBuilder sql = new StringBuilder();
                sql.Clear();
                //sql.AppendLine("SELECT * ");
                //sql.AppendLine("FROM WGRB.BOOKING_NOTE_ITEMS");
                //sql.AppendLine("WHERE PALLET_GO is not null");
                //sql.AppendLine("AND STATUS_GO is not null");
                //sql.AppendLine("AND PALLET_GO = :Pallet");
                //sql.AppendLine("ORDER BY SEQ_NO");

                sql.AppendLine("SELECT t1.*, t2.ITEM_NAME ");
                sql.AppendLine("FROM WGRB.BOOKING_NOTE_ITEMS t1");
                sql.AppendLine("LEFT JOIN WGRB.ITEMS t2 ON t1.ITEM_CODE = t2.ITEM_CODE");
                sql.AppendLine("WHERE t1.PALLET_GO is not null");
                sql.AppendLine("AND t1.STATUS_GO is null");
                sql.AppendLine("AND t1.PALLET_GO = :Pallet");
                sql.AppendLine("AND t1.REMARKS is null");
                sql.AppendLine("ORDER BY t1.SEQ_NO");

                try
                {
                    OleDbCommand cmd = new OleDbCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.Add(new OleDbParameter("Pallet", pallet));
                    //await con.OpenAsync();
                    con.Open();

                    OleDbDataReader rdr = cmd.ExecuteReader();
                    while (await rdr.ReadAsync())
                    {
                        BOOKING_NOTE_ITEMS objrd = new BOOKING_NOTE_ITEMS
                        {
                            BK_NO = rdr["BK_NO"].ToString(),
                            SEQ_NO = rdr["SEQ_NO"] == DBNull.Value ? null : (Decimal?)rdr["SEQ_NO"],
                            DN_NO = rdr["DN_NO"].ToString(),
                            DN_SEQ = rdr["DN_SEQ"] == DBNull.Value ? null : (Decimal?)rdr["DN_SEQ"],
                            SO_NO = rdr["SO_NO"].ToString(),
                            SO_SEQ = rdr["SO_SEQ"] == DBNull.Value ? null : (Decimal?)rdr["SO_SEQ"],
                            JOB_NO = rdr["JOB_NO"].ToString(),
                            ITEM_CODE = rdr["ITEM_CODE"].ToString(),
                            DN_QTY = rdr["DN_QTY"] == DBNull.Value ? null : (Decimal?)rdr["DN_QTY"],
                            DN_UM = rdr["DN_UM"].ToString(),
                            DN_DATE = rdr["DN_DATE"] == DBNull.Value ? null : (DateTime?)rdr["DN_DATE"],
                            STATUS = rdr["STATUS"].ToString(),
                            CUSTOMER_CODE = rdr["CUSTOMER_CODE"].ToString(),
                            BATCH_NO = rdr["BATCH_NO"].ToString(),
                            FACTORY_ROOM = rdr["FACTORY_ROOM"].ToString(),
                            PALETTE_NO = rdr["PALETTE_NO"].ToString(),
                            PCK_NO = rdr["PCK_NO"] == DBNull.Value ? null : (Decimal?)rdr["PCK_NO"],
                            BOX_NO = rdr["BOX_NO"] == DBNull.Value ? null : (Decimal?)rdr["BOX_NO"],
                            CLOSE_FLAG = rdr["CLOSE_FLAG"].ToString(),
                            PACKETING_CODE = rdr["PACKETING_CODE"].ToString(),
                            DN_WEIGHT = rdr["DN_WEIGHT"] == DBNull.Value ? null : (Decimal?)rdr["DN_WEIGHT"],
                            DN_WEIGHT_UM = rdr["DN_WEIGHT_UM"].ToString(),
                            LOT_NO = rdr["LOT_NO"].ToString(),
                            STORE_ID = rdr["STORE_ID"].ToString(),
                            SCREEN_NAME = rdr["SCREEN_NAME"].ToString(),
                            CUST_ITEM_CODE = rdr["CUST_ITEM_CODE"].ToString(),
                            VEHICLE_CODE = rdr["VEHICLE_CODE"].ToString(),
                            VEHICLE_LINK_CODE = rdr["VEHICLE_LINK_CODE"].ToString(),
                            PACKAGE_WEIGHT = rdr["PACKAGE_WEIGHT"] == DBNull.Value ? null : (Decimal?)rdr["PACKAGE_WEIGHT"],
                            PK_NO = rdr["PK_NO"].ToString(),
                            PK_SEQ = rdr["PK_SEQ"] == DBNull.Value ? null : (Decimal?)rdr["PK_SEQ"],
                            STICKER_CODE = rdr["STICKER_CODE"].ToString(),
                            SUB_CUSTOMER_CODE = rdr["SUB_CUSTOMER_CODE"].ToString(),
                            TO_PCK_NO = rdr["TO_PCK_NO"].ToString(),
                            EFFECT_TYPE = rdr["EFFECT_TYPE"].ToString(),
                            REMARKS = rdr["REMARKS"].ToString(),
                            BOOKING_MODEL_NO = rdr["BOOKING_MODEL_NO"].ToString(),
                            REQUEST_DATE = rdr["REQUEST_DATE"] == DBNull.Value ? null : (DateTime?)rdr["REQUEST_DATE"],
                            SEQ_TR = rdr["SEQ_TR"] == DBNull.Value ? null : (Decimal?)rdr["SEQ_TR"],
                            TANK_SIZE = rdr["TANK_SIZE"].ToString(),
                            HUMAN_DRIVE_CODE = rdr["HUMAN_DRIVE_CODE"].ToString(),
                            TIME_LOAD_PRODUCT = rdr["TIME_LOAD_PRODUCT"].ToString(),
                            QTY_REMARKS = rdr["QTY_REMARKS"].ToString(),
                            PACKING_DATE = rdr["PACKING_DATE"] == DBNull.Value ? null : (DateTime?)rdr["PACKING_DATE"],
                            SHELF_LIFE_DATE = rdr["SHELF_LIFE_DATE"] == DBNull.Value ? null : (DateTime?)rdr["SHELF_LIFE_DATE"],
                            SHELF_LIFE_DAY = rdr["SHELF_LIFE_DAY"] == DBNull.Value ? null : (Decimal?)rdr["SHELF_LIFE_DAY"],
                            SHELF_LIFE_KEEP_IN = rdr["SHELF_LIFE_KEEP_IN"] == DBNull.Value ? null : (Decimal?)rdr["SHELF_LIFE_KEEP_IN"],
                            SHELF_LIFE_KEEP_IN_DATE = rdr["SHELF_LIFE_KEEP_IN_DATE"] == DBNull.Value ? null : (DateTime?)rdr["SHELF_LIFE_KEEP_IN_DATE"],
                            LOT_CORN = rdr["LOT_CORN"].ToString(),
                            HIT_LET = rdr["HIT_LET"].ToString(),
                            PALLET_GO = rdr["PALLET_GO"].ToString(),
                            STATUS_GO = rdr["STATUS_GO"].ToString(),
                            UPDATE_DATE_GO = rdr["UPDATE_DATE_GO"] == DBNull.Value ? null : (DateTime?)rdr["UPDATE_DATE_GO"],
                            CREATED_DATE = rdr["CREATED_DATE"] == DBNull.Value ? null : (DateTime?)rdr["CREATED_DATE"],
                            ITEM_NAME = rdr["ITEM_NAME"].ToString()
                        };
                        lstobj.Add(objrd);
                    }
                }
                catch (Exception e)
                {
                    throw;
                }
                con.Close();
            }
            return lstobj;
        }

        public void UpdateNewBooking_note(string pallet)
        {
            Environment.SetEnvironmentVariable("NLS_LANG",
                    "AMERICAN_AMERICA.WE8MSWIN1252",
                    EnvironmentVariableTarget.Process);

            using OleDbConnection con = new OleDbConnection(connectionString);
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("Update WGRB.BOOKING_NOTE_ITEMS");
            sql.AppendLine("Set STATUS_GO = ?");
            sql.AppendLine(", UPDATE_DATE_GO = CURRENT_TIMESTAMP");
            sql.AppendLine("Where PALLET_GO = ?");
            sql.AppendLine("And  STATUS_GO = is null");
            OleDbCommand cmd = new OleDbCommand(sql.ToString(), con)
            {
                CommandType = CommandType.Text
            };

            cmd.Parameters.Add(new OleDbParameter("STATUS_GO", "Y"));
            cmd.Parameters.Add(new OleDbParameter("PALLET_GO", pallet));

            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                string emsg = e.Message;
            }
            finally
            {
                con.Close();
            }

        }

        public async Task UpdateNewBooking_notebylist(List<BOOKING_NOTE_ITEMS> listOrder)
        {
            Environment.SetEnvironmentVariable("NLS_LANG",
                 "AMERICAN_AMERICA.WE8MSWIN1252",
                 EnvironmentVariableTarget.Process);

            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                con.Open();

                try
                {
                    OleDbCommand command = new OleDbCommand();
                    command.Connection = con;
                    command.CommandType = CommandType.Text;
                    string statusgo = "Y";
                    foreach (var s in listOrder)
                    {
                        string cmdStr = string.Format("Update WGRB.BOOKING_NOTE_ITEMS " +
                            "SET STATUS_GO ='{0}' " +
                            ",UPDATE_DATE_GO=CURRENT_TIMESTAMP " +
                            "WHERE PALLET_GO='{1}' " +
                            "AND SO_NO='{2}' " +
                            "AND SEQ_NO={3} " +
                            "AND DN_SEQ={4} " +
                            "AND STATUS_GO is null", statusgo, s.PALLET_GO.ToString(), s.SO_NO.ToString(), s.SEQ_NO, s.DN_SEQ);
                        command.CommandText = cmdStr;
                        command.ExecuteNonQuery();

                    }
                }
                catch (Exception e)
                {
                    string emsg = e.Message;
                }
                finally
                {
                    con.Close();
                }
               

                
            }


            /*
                StringBuilder sql = new StringBuilder();
            sql.AppendLine("Update WGRB.BOOKING_NOTE_ITEMS");
            sql.AppendLine("Set STATUS_GO = ?");
            sql.AppendLine(", UPDATE_DATE_GO = CURRENT_TIMESTAMP");
            sql.AppendLine("Where PALLET_GO = ?");
            sql.AppendLine("And  STATUS_GO = is null");
            OleDbCommand cmd = new OleDbCommand(sql.ToString(), con)
            {
                CommandType = CommandType.Text
            };

            cmd.Parameters.Add(new OleDbParameter("STATUS_GO", "Y"));
            cmd.Parameters.Add(new OleDbParameter("PALLET_GO", pallet));

            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                string emsg = e.Message;
            }
            finally
            {
                con.Close();
            }
            */

        }

        public async Task<IEnumerable<DELIVERY_NOTE_ITEMS>> GetAllApiDelivery_note()
        {
            Environment.SetEnvironmentVariable("NLS_LANG",
                    "AMERICAN_AMERICA.WE8MSWIN1252",
                    EnvironmentVariableTarget.Process);

            List<DELIVERY_NOTE_ITEMS> lstobj = new List<DELIVERY_NOTE_ITEMS>();
            StringBuilder sql = new StringBuilder();
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                //StringBuilder sql = new StringBuilder();
                sql.Clear();
                sql.AppendLine("SELECT t1.*, t2.ITEM_NAME ");
                sql.AppendLine("FROM WGRB.DELIVERY_NOTE_ITEMS t1");
                sql.AppendLine("LEFT JOIN WGRB.ITEMS t2 ON t1.ITEM_CODE = t2.ITEM_CODE");
                sql.AppendLine("WHERE t1.PALLET_GO is not null");
                sql.AppendLine("AND t1.PALLET_GO like 'WGC-%'");
                //sql.AppendLine("AND STATUS_GO is not null");
                sql.AppendLine("ORDER BY t1.SEQ_NO");

                try
                {
                    OleDbCommand cmd = new OleDbCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    //await con.OpenAsync();
                    con.Open();
                    OleDbDataReader rdr = cmd.ExecuteReader();
                    while (await rdr.ReadAsync())
                    {

                        DELIVERY_NOTE_ITEMS objrd = new DELIVERY_NOTE_ITEMS
                        {
                            DN_NO = rdr["DN_NO"].ToString(),
                            SEQ_NO = rdr["SEQ_NO"] == DBNull.Value ? null : (Decimal?)rdr["SEQ_NO"],
                            JOB_NO = rdr["JOB_NO"].ToString(),
                            ITEM_CODE = rdr["ITEM_CODE"].ToString(),
                            DN_QTY = rdr["DN_QTY"] == DBNull.Value ? null : (Decimal?)rdr["DN_QTY"],
                            DN_UM = rdr["DN_UM"].ToString(),
                            DN_DATE = rdr["DN_DATE"] == DBNull.Value ? null : (DateTime?)rdr["DN_DATE"],
                            STATUS = rdr["STATUS"].ToString(),
                            CUSTOMER_CODE = rdr["CUSTOMER_CODE"].ToString(),
                            BATCH_NO = rdr["BATCH_NO"].ToString(),
                            FACTORY_ROOM = rdr["FACTORY_ROOM"].ToString(),
                            PALETTE_NO = rdr["PALETTE_NO"].ToString(),
                            BOX_NO = rdr["BOX_NO"] == DBNull.Value ? null : (Decimal?)rdr["BOX_NO"],
                            CLOSE_FLAG = rdr["CLOSE_FLAG"].ToString(),
                            PACKETING_CODE = rdr["PACKETING_CODE"].ToString(),
                            DN_WEIGHT = rdr["DN_WEIGHT"] == DBNull.Value ? null : (Decimal?)rdr["DN_WEIGHT"],
                            DN_WEIGHT_UM = rdr["DN_WEIGHT_UM"].ToString(),
                            LOT_NO = rdr["LOT_NO"].ToString(),
                            STORE_ID = rdr["STORE_ID"].ToString(),
                            SCREEN_NAME = rdr["SCREEN_NAME"].ToString(),
                            CUST_ITEM_CODE = rdr["CUST_ITEM_CODE"].ToString(),
                            VEHICLE_CODE = rdr["VEHICLE_CODE"].ToString(),
                            VEHICLE_LINK_CODE = rdr["VEHICLE_LINK_CODE"].ToString(),
                            PACKAGE_WEIGHT = rdr["PACKAGE_WEIGHT"] == DBNull.Value ? null : (Decimal?)rdr["PACKAGE_WEIGHT"],
                            APP1_FLAG = rdr["APP1_FLAG"].ToString(),
                            APP2_FLAG = rdr["APP2_FLAG"].ToString(),
                            APP3_FLAG = rdr["APP3_FLAG"].ToString(),
                            APP_AG_FLAG = rdr["APP_AG_FLAG"].ToString(),
                            NET_IRON_DATE = rdr["NET_IRON_DATE"] == DBNull.Value ? null : (DateTime?)rdr["NET_IRON_DATE"],
                            NET_IRON_NUMBER= rdr["NET_IRON_NUMBER"] == DBNull.Value ? null : (Decimal?)rdr["NET_IRON_NUMBER"],
                            CREATED_T1_BY  = rdr["CREATED_T1_BY"].ToString(),
                            CREATED_T1_ON = rdr["CREATED_T1_ON"] == DBNull.Value ? null : (DateTime?)rdr["CREATED_T1_ON"],
                            APPROVED_T1_BY= rdr["APPROVED_T1_BY"].ToString(),
                            APPROVED_T1_ON = rdr["APPROVED_T1_ON"] == DBNull.Value ? null : (DateTime?)rdr["APPROVED_T1_ON"],
                            NET_IRON_NC = rdr["NET_IRON_NC"].ToString(),
                            INFECTION_DATE = rdr["INFECTION_DATE"] == DBNull.Value ? null : (DateTime?)rdr["INFECTION_DATE"],
                            TPC = rdr["TPC"].ToString(),
                            YM = rdr["YM"].ToString(),
                            SAUREUS = rdr["SAUREUS"].ToString(),
                            ECOLI = rdr["ECOLI"].ToString(),
                            COLIFORM = rdr["COLIFORM"].ToString(),
                            BCEREUS = rdr["BCEREUS"].ToString(),
                            SALLMONELLA = rdr["SALLMONELLA"].ToString(),
                            AW = rdr["AW"].ToString(),
                            REMARKS_INFECTION = rdr["REMARKS_INFECTION"].ToString(),
                            INFECTION_NC = rdr["INFECTION_NC"].ToString(),
                            APPROVED_T2_BY = rdr["APPROVED_T2_BY"].ToString(),
                            APPROVED_T2_ON = rdr["APPROVED_T2_ON"] == DBNull.Value ? null : (DateTime?)rdr["APPROVED_T2_ON"],
                            APPROVED_T3_BY = rdr["APPROVED_T3_BY"].ToString(),
                            APPROVED_T3_ON = rdr["APPROVED_T3_ON"] == DBNull.Value ? null : (DateTime?)rdr["APPROVED_T3_ON"],
                            APPROVED_AG_BY = rdr["APPROVED_AG_BY"].ToString(),
                            APPROVED_AG_ON = rdr["APPROVED_AG_ON"] == DBNull.Value ? null : (DateTime?)rdr["APPROVED_AG_ON"],
                            PK_NO = rdr["PK_NO"].ToString(),
                            PK_SEQ = rdr["PK_SEQ"] == DBNull.Value ? null : (Decimal?)rdr["PK_SEQ"],
                            STICKER_CODE = rdr["STICKER_CODE"].ToString(),
                            SUB_CUSTOMER_CODE = rdr["SUB_CUSTOMER_CODE"].ToString(),
                            TO_PCK_NO = rdr["TO_PCK_NO"].ToString(),
                            EFFECT_TYPE = rdr["EFFECT_TYPE"].ToString(),
                            REMARKS = rdr["REMARKS"].ToString(),
                            FLAG_BOOK = rdr["FLAG_BOOK"].ToString(),
                            SEQ_GROUP = rdr["SEQ_GROUP"] == DBNull.Value ? null : (Decimal?)rdr["SEQ_GROUP"],
                            TANK = rdr["TANK"].ToString(),
                            BRIX = rdr["BRIX"].ToString(),
                            PH = rdr["PH"].ToString(),
                            DE = rdr["DE"].ToString(),
                            SOII = rdr["SOII"].ToString(),
                            BSTAR = rdr["BSTAR"].ToString(),
                            AWQA3 = rdr["AWQA3"].ToString(),
                            REMARKS_QUALITY = rdr["REMARKS_QUALITY"].ToString(),
                            QUALITY_NC = rdr["QUALITY_NC"].ToString(),
                            DATE_QUALITY = rdr["DATE_QUALITY"] == DBNull.Value ? null : (DateTime?)rdr["DATE_QUALITY"],
                            CREATED_T2_BY = rdr["CREATED_T2_BY"].ToString(),
                            CREATED_T2_ON = rdr["CREATED_T2_ON"] == DBNull.Value ? null : (DateTime?)rdr["CREATED_T2_ON"],
                            CREATED_T3_BY = rdr["CREATED_T3_BY"].ToString(),
                            CREATED_T3_ON = rdr["CREATED_T3_ON"] == DBNull.Value ? null : (DateTime?)rdr["CREATED_T3_ON"],
                            APPROVED_INV_BY = rdr["APPROVED_INV_BY"].ToString(),
                            APPROVED_INV_ON = rdr["APPROVED_INV_ON"] == DBNull.Value ? null : (DateTime?)rdr["APPROVED_INV_ON"],
                            CREATED_INV_BY = rdr["CREATED_INV_BY"].ToString(),
                            CREATED_INV_ON = rdr["CREATED_INV_ON"] == DBNull.Value ? null : (DateTime?)rdr["CREATED_INV_ON"],
                            APP_INV_FLAG = rdr["APP_INV_FLAG"].ToString(),
                            REMARKS_INV = rdr["REMARKS_INV"].ToString(),
                            ITEM_STATUS = rdr["ITEM_STATUS"].ToString(),
                            MJ3 = rdr["MJ3"].ToString(),
                            BULKD3 = rdr["BULKD3"].ToString(),
                            PACKBD3 = rdr["PACKBD3"].ToString(),
                            NC_BOOK = rdr["NC_BOOK"].ToString(),
                            RACK_NO = rdr["RACK_NO"].ToString(),
                            BLACK_CARBON = rdr["BLACK_CARBON"].ToString(),
                            DUP_PCK_NO = rdr["DUP_PCK_NO"] == DBNull.Value ? null : (Decimal?)rdr["DUP_PCK_NO"],
                            PCK_NO = rdr["PCK_NO"] == DBNull.Value ? null : (Decimal?)rdr["PCK_NO"],
                            STATUS_LOAD = rdr["STATUS_LOAD"].ToString(),
                            PACKING_DATE = rdr["PACKING_DATE"] == DBNull.Value ? null : (DateTime?)rdr["PACKING_DATE"],
                            YEAST = rdr["YEAST"] == DBNull.Value ? null : (Decimal?)rdr["YEAST"],
                            MOLD = rdr["MOLD"] == DBNull.Value ? null : (Decimal?)rdr["MOLD"],
                            SHELF_LIFE_DAY = rdr["SHELF_LIFE_DAY"] == DBNull.Value ? null : (Decimal?)rdr["SHELF_LIFE_DAY"],
                            SHELF_LIFE_KEEP_IN = rdr["SHELF_LIFE_KEEP_IN"] == DBNull.Value ? null : (Decimal?)rdr["SHELF_LIFE_KEEP_IN"],
                            SHELF_LIFE_DATE = rdr["SHELF_LIFE_DATE"] == DBNull.Value ? null : (DateTime?)rdr["SHELF_LIFE_DATE"],
                            SHELF_LIFE_KEEP_IN_DATE = rdr["SHELF_LIFE_KEEP_IN_DATE"] == DBNull.Value ? null : (DateTime?)rdr["SHELF_LIFE_KEEP_IN_DATE"],
                            PALET_SORT = rdr["PALET_SORT"] == DBNull.Value ? null : (Decimal?)rdr["PALET_SORT"],
                            HIT_LET = rdr["HIT_LET"].ToString(),
                            LOT_CORN = rdr["LOT_CORN"].ToString(),
                            TANK_EF = rdr["TANK_EF"].ToString(),
                            REF_DATE = rdr["REF_DATE"] == DBNull.Value ? null : (DateTime?)rdr["REF_DATE"],
                            LSTAR = rdr["LSTAR"].ToString(),
                            ASTAR = rdr["ASTAR"].ToString(),
                            REMARKS_ISS = rdr["REMARKS_ISS"].ToString(),
                            INJECTION_DATE = rdr["INJECTION_DATE"] == DBNull.Value ? null : (DateTime?)rdr["INJECTION_DATE"],
                            SPRAY_DATE = rdr["SPRAY_DATE"] == DBNull.Value ? null : (DateTime?)rdr["SPRAY_DATE"],
                            BATCH_NO_CUST = rdr["BATCH_NO_CUST"].ToString(),
                            PALLET_GO = rdr["PALLET_GO"].ToString(),
                            STATUS_GO = rdr["STATUS_GO"].ToString(),
                            UPDATE_DATE_GO = rdr["UPDATE_DATE_GO"] == DBNull.Value ? null : (DateTime?)rdr["UPDATE_DATE_GO"],
                            CREATED_BY = rdr["CREATED_BY"].ToString(),
                            CREATED_DATE = rdr["CREATED_DATE"] == DBNull.Value ? null : (DateTime?)rdr["CREATED_DATE"],
                            ITEM_NAME = rdr["ITEM_NAME"].ToString()
                        };
                        lstobj.Add(objrd);
                    }
                }
                catch (Exception e)
                {

                    throw;
                }
                con.Close();
            }
            return lstobj;
        }

        public async Task<Int64> GetSumOrderAllApiNewDelivery_note()
        {
            Int64 lRet =0;
            Environment.SetEnvironmentVariable("NLS_LANG",
                    "AMERICAN_AMERICA.WE8MSWIN1252",
                    EnvironmentVariableTarget.Process);
            StringBuilder sql = new StringBuilder();
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                sql.Clear();
                 sql.AppendLine("SELECT COUNT(*) FROM (");
                sql.AppendLine("SELECT t1.*");
                sql.AppendLine("FROM WGRB.DELIVERY_NOTE_ITEMS t1");
                sql.AppendLine("WHERE t1.STATUS = 'A'");
                sql.AppendLine("AND (t1.STATUS_GO <> 'Y' or t1.STATUS_GO is null)");
                sql.AppendLine("AND t1.REMARKS is null");
                sql.AppendLine("AND t1.REMARKS_INV is null");
                sql.AppendLine("AND t1.PALLET_GO is not null");
                sql.AppendLine(")");

                //sql.AppendLine("ORDER BY t1.SEQ_NO");
                try
                {
                    OleDbCommand cmd = new OleDbCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    //await con.OpenAsync();
                    con.Open();
                    //var iret = cmd.ExecuteScalar();
                    lRet = Convert.ToInt64(cmd.ExecuteScalar());
                    cmd.Dispose();
                }
                catch (Exception e)
                {
                    throw;
                }
                con.Close();
            }

            return lRet;
        }

        public async Task<Int64> GetSumPalletAllApiNewDelivery_note()
        {
            Int64 lRet = 0;
            Environment.SetEnvironmentVariable("NLS_LANG",
                    "AMERICAN_AMERICA.WE8MSWIN1252",
                    EnvironmentVariableTarget.Process);
            StringBuilder sql = new StringBuilder();
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                sql.Clear();
                sql.AppendLine("SELECT COUNT(*) FROM (");
                sql.AppendLine("SELECT t1.PALLET_GO");
                sql.AppendLine("FROM WGRB.DELIVERY_NOTE_ITEMS t1");
                sql.AppendLine("WHERE t1.STATUS = 'A'");
                sql.AppendLine("AND (t1.STATUS_GO <> 'Y' or t1.STATUS_GO is null)");
                sql.AppendLine("AND t1.REMARKS is null");
                sql.AppendLine("AND t1.REMARKS_INV is null");
                sql.AppendLine("AND t1.PALLET_GO is not null");
                sql.AppendLine("GROUP BY t1.PALLET_GO");
                sql.AppendLine(")");

                try
                {
                    OleDbCommand cmd = new OleDbCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    //await con.OpenAsync();
                    con.Open();
                    lRet = Convert.ToInt64(cmd.ExecuteScalar());
                    cmd.Dispose();
                }
                catch (Exception e)
                {
                    throw;
                }
                con.Close();
            }

            return lRet;
        }


        public async Task<IEnumerable<DELIVERY_NOTE_ITEMS>> GetAllApiNewDelivery_note()
        {
            Environment.SetEnvironmentVariable("NLS_LANG",
                    "AMERICAN_AMERICA.WE8MSWIN1252",
                    EnvironmentVariableTarget.Process);

            List<DELIVERY_NOTE_ITEMS> lstobj = new List<DELIVERY_NOTE_ITEMS>();
            StringBuilder sql = new StringBuilder();
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                //StringBuilder sql = new StringBuilder();
                sql.Clear();
                //sql.AppendLine("SELECT * ");
                //sql.AppendLine("FROM WGRB.DELIVERY_NOTE_ITEMS");
                //sql.AppendLine("WHERE PALLET_GO is not null");
                //sql.AppendLine("AND STATUS_GO is not null");
                //sql.AppendLine("ORDER BY SEQ_NO");

                sql.AppendLine("SELECT t1.*, t2.ITEM_NAME ");
                sql.AppendLine("FROM WGRB.DELIVERY_NOTE_ITEMS t1");
                sql.AppendLine("LEFT JOIN WGRB.ITEMS t2 ON t1.ITEM_CODE = t2.ITEM_CODE");
                sql.AppendLine("WHERE t1.PALLET_GO is not null");
                sql.AppendLine("AND t1.STATUS_GO is not null");
                sql.AppendLine("ORDER BY t1.SEQ_NO");



                try
                {
                    OleDbCommand cmd = new OleDbCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    //await con.OpenAsync();
                    con.Open();
                    OleDbDataReader rdr = cmd.ExecuteReader();
                    while (await rdr.ReadAsync())
                    {

                        DELIVERY_NOTE_ITEMS objrd = new DELIVERY_NOTE_ITEMS
                        {
                            DN_NO = rdr["DN_NO"].ToString(),
                            SEQ_NO = rdr["SEQ_NO"] == DBNull.Value ? null : (Decimal?)rdr["SEQ_NO"],
                            JOB_NO = rdr["JOB_NO"].ToString(),
                            ITEM_CODE = rdr["ITEM_CODE"].ToString(),
                            DN_QTY = rdr["DN_QTY"] == DBNull.Value ? null : (Decimal?)rdr["DN_QTY"],
                            DN_UM = rdr["DN_UM"].ToString(),
                            DN_DATE = rdr["DN_DATE"] == DBNull.Value ? null : (DateTime?)rdr["DN_DATE"],
                            STATUS = rdr["STATUS"].ToString(),
                            CUSTOMER_CODE = rdr["CUSTOMER_CODE"].ToString(),
                            BATCH_NO = rdr["BATCH_NO"].ToString(),
                            FACTORY_ROOM = rdr["FACTORY_ROOM"].ToString(),
                            PALETTE_NO = rdr["PALETTE_NO"].ToString(),
                            BOX_NO = rdr["BOX_NO"] == DBNull.Value ? null : (Decimal?)rdr["BOX_NO"],
                            CLOSE_FLAG = rdr["CLOSE_FLAG"].ToString(),
                            PACKETING_CODE = rdr["PACKETING_CODE"].ToString(),
                            DN_WEIGHT = rdr["DN_WEIGHT"] == DBNull.Value ? null : (Decimal?)rdr["DN_WEIGHT"],
                            DN_WEIGHT_UM = rdr["DN_WEIGHT_UM"].ToString(),
                            LOT_NO = rdr["LOT_NO"].ToString(),
                            STORE_ID = rdr["STORE_ID"].ToString(),
                            SCREEN_NAME = rdr["SCREEN_NAME"].ToString(),
                            CUST_ITEM_CODE = rdr["CUST_ITEM_CODE"].ToString(),
                            VEHICLE_CODE = rdr["VEHICLE_CODE"].ToString(),
                            VEHICLE_LINK_CODE = rdr["VEHICLE_LINK_CODE"].ToString(),
                            PACKAGE_WEIGHT = rdr["PACKAGE_WEIGHT"] == DBNull.Value ? null : (Decimal?)rdr["PACKAGE_WEIGHT"],
                            APP1_FLAG = rdr["APP1_FLAG"].ToString(),
                            APP2_FLAG = rdr["APP2_FLAG"].ToString(),
                            APP3_FLAG = rdr["APP3_FLAG"].ToString(),
                            APP_AG_FLAG = rdr["APP_AG_FLAG"].ToString(),
                            NET_IRON_DATE = rdr["NET_IRON_DATE"] == DBNull.Value ? null : (DateTime?)rdr["NET_IRON_DATE"],
                            NET_IRON_NUMBER = rdr["NET_IRON_NUMBER"] == DBNull.Value ? null : (Decimal?)rdr["NET_IRON_NUMBER"],
                            CREATED_T1_BY = rdr["CREATED_T1_BY"].ToString(),
                            CREATED_T1_ON = rdr["CREATED_T1_ON"] == DBNull.Value ? null : (DateTime?)rdr["CREATED_T1_ON"],
                            APPROVED_T1_BY = rdr["APPROVED_T1_BY"].ToString(),
                            APPROVED_T1_ON = rdr["APPROVED_T1_ON"] == DBNull.Value ? null : (DateTime?)rdr["APPROVED_T1_ON"],
                            NET_IRON_NC = rdr["NET_IRON_NC"].ToString(),
                            INFECTION_DATE = rdr["INFECTION_DATE"] == DBNull.Value ? null : (DateTime?)rdr["INFECTION_DATE"],
                            TPC = rdr["TPC"].ToString(),
                            YM = rdr["YM"].ToString(),
                            SAUREUS = rdr["SAUREUS"].ToString(),
                            ECOLI = rdr["ECOLI"].ToString(),
                            COLIFORM = rdr["COLIFORM"].ToString(),
                            BCEREUS = rdr["BCEREUS"].ToString(),
                            SALLMONELLA = rdr["SALLMONELLA"].ToString(),
                            AW = rdr["AW"].ToString(),
                            REMARKS_INFECTION = rdr["REMARKS_INFECTION"].ToString(),
                            INFECTION_NC = rdr["INFECTION_NC"].ToString(),
                            APPROVED_T2_BY = rdr["APPROVED_T2_BY"].ToString(),
                            APPROVED_T2_ON = rdr["APPROVED_T2_ON"] == DBNull.Value ? null : (DateTime?)rdr["APPROVED_T2_ON"],
                            APPROVED_T3_BY = rdr["APPROVED_T3_BY"].ToString(),
                            APPROVED_T3_ON = rdr["APPROVED_T3_ON"] == DBNull.Value ? null : (DateTime?)rdr["APPROVED_T3_ON"],
                            APPROVED_AG_BY = rdr["APPROVED_AG_BY"].ToString(),
                            APPROVED_AG_ON = rdr["APPROVED_AG_ON"] == DBNull.Value ? null : (DateTime?)rdr["APPROVED_AG_ON"],
                            PK_NO = rdr["PK_NO"].ToString(),
                            PK_SEQ = rdr["PK_SEQ"] == DBNull.Value ? null : (Decimal?)rdr["PK_SEQ"],
                            STICKER_CODE = rdr["STICKER_CODE"].ToString(),
                            SUB_CUSTOMER_CODE = rdr["SUB_CUSTOMER_CODE"].ToString(),
                            TO_PCK_NO = rdr["TO_PCK_NO"].ToString(),
                            EFFECT_TYPE = rdr["EFFECT_TYPE"].ToString(),
                            REMARKS = rdr["REMARKS"].ToString(),
                            FLAG_BOOK = rdr["FLAG_BOOK"].ToString(),
                            SEQ_GROUP = rdr["SEQ_GROUP"] == DBNull.Value ? null : (Decimal?)rdr["SEQ_GROUP"],
                            TANK = rdr["TANK"].ToString(),
                            BRIX = rdr["BRIX"].ToString(),
                            PH = rdr["PH"].ToString(),
                            DE = rdr["DE"].ToString(),
                            SOII = rdr["SOII"].ToString(),
                            BSTAR = rdr["BSTAR"].ToString(),
                            AWQA3 = rdr["AWQA3"].ToString(),
                            REMARKS_QUALITY = rdr["REMARKS_QUALITY"].ToString(),
                            QUALITY_NC = rdr["QUALITY_NC"].ToString(),
                            DATE_QUALITY = rdr["DATE_QUALITY"] == DBNull.Value ? null : (DateTime?)rdr["DATE_QUALITY"],
                            CREATED_T2_BY = rdr["CREATED_T2_BY"].ToString(),
                            CREATED_T2_ON = rdr["CREATED_T2_ON"] == DBNull.Value ? null : (DateTime?)rdr["CREATED_T2_ON"],
                            CREATED_T3_BY = rdr["CREATED_T3_BY"].ToString(),
                            CREATED_T3_ON = rdr["CREATED_T3_ON"] == DBNull.Value ? null : (DateTime?)rdr["CREATED_T3_ON"],
                            APPROVED_INV_BY = rdr["APPROVED_INV_BY"].ToString(),
                            APPROVED_INV_ON = rdr["APPROVED_INV_ON"] == DBNull.Value ? null : (DateTime?)rdr["APPROVED_INV_ON"],
                            CREATED_INV_BY = rdr["CREATED_INV_BY"].ToString(),
                            CREATED_INV_ON = rdr["CREATED_INV_ON"] == DBNull.Value ? null : (DateTime?)rdr["CREATED_INV_ON"],
                            APP_INV_FLAG = rdr["APP_INV_FLAG"].ToString(),
                            REMARKS_INV = rdr["REMARKS_INV"].ToString(),
                            ITEM_STATUS = rdr["ITEM_STATUS"].ToString(),
                            MJ3 = rdr["MJ3"].ToString(),
                            BULKD3 = rdr["BULKD3"].ToString(),
                            PACKBD3 = rdr["PACKBD3"].ToString(),
                            NC_BOOK = rdr["NC_BOOK"].ToString(),
                            RACK_NO = rdr["RACK_NO"].ToString(),
                            BLACK_CARBON = rdr["BLACK_CARBON"].ToString(),
                            DUP_PCK_NO = rdr["DUP_PCK_NO"] == DBNull.Value ? null : (Decimal?)rdr["DUP_PCK_NO"],
                            PCK_NO = rdr["PCK_NO"] == DBNull.Value ? null : (Decimal?)rdr["PCK_NO"],
                            STATUS_LOAD = rdr["STATUS_LOAD"].ToString(),
                            PACKING_DATE = rdr["PACKING_DATE"] == DBNull.Value ? null : (DateTime?)rdr["PACKING_DATE"],
                            YEAST = rdr["YEAST"] == DBNull.Value ? null : (Decimal?)rdr["YEAST"],
                            MOLD = rdr["MOLD"] == DBNull.Value ? null : (Decimal?)rdr["MOLD"],
                            SHELF_LIFE_DAY = rdr["SHELF_LIFE_DAY"] == DBNull.Value ? null : (Decimal?)rdr["SHELF_LIFE_DAY"],
                            SHELF_LIFE_KEEP_IN = rdr["SHELF_LIFE_KEEP_IN"] == DBNull.Value ? null : (Decimal?)rdr["SHELF_LIFE_KEEP_IN"],
                            SHELF_LIFE_DATE = rdr["SHELF_LIFE_DATE"] == DBNull.Value ? null : (DateTime?)rdr["SHELF_LIFE_DATE"],
                            SHELF_LIFE_KEEP_IN_DATE = rdr["SHELF_LIFE_KEEP_IN_DATE"] == DBNull.Value ? null : (DateTime?)rdr["SHELF_LIFE_KEEP_IN_DATE"],
                            PALET_SORT = rdr["PALET_SORT"] == DBNull.Value ? null : (Decimal?)rdr["PALET_SORT"],
                            HIT_LET = rdr["HIT_LET"].ToString(),
                            LOT_CORN = rdr["LOT_CORN"].ToString(),
                            TANK_EF = rdr["TANK_EF"].ToString(),
                            REF_DATE = rdr["REF_DATE"] == DBNull.Value ? null : (DateTime?)rdr["REF_DATE"],
                            LSTAR = rdr["LSTAR"].ToString(),
                            ASTAR = rdr["ASTAR"].ToString(),
                            REMARKS_ISS = rdr["REMARKS_ISS"].ToString(),
                            INJECTION_DATE = rdr["INJECTION_DATE"] == DBNull.Value ? null : (DateTime?)rdr["INJECTION_DATE"],
                            SPRAY_DATE = rdr["SPRAY_DATE"] == DBNull.Value ? null : (DateTime?)rdr["SPRAY_DATE"],
                            BATCH_NO_CUST = rdr["BATCH_NO_CUST"].ToString(),
                            PALLET_GO = rdr["PALLET_GO"].ToString(),
                            STATUS_GO = rdr["STATUS_GO"].ToString(),
                            UPDATE_DATE_GO = rdr["UPDATE_DATE_GO"] == DBNull.Value ? null : (DateTime?)rdr["UPDATE_DATE_GO"],
                            CREATED_BY = rdr["CREATED_BY"].ToString(),
                            CREATED_DATE = rdr["CREATED_DATE"] == DBNull.Value ? null : (DateTime?)rdr["CREATED_DATE"],
                            ITEM_NAME = rdr["ITEM_NAME"].ToString()
                        };
                        lstobj.Add(objrd);
                    }
                }
                catch (Exception e)
                {

                    throw;
                }
                con.Close();
            }
            return lstobj;
        }

        public async Task<IEnumerable<DELIVERY_NOTE_ITEMS>> GetPalletApiNewDelivery_note(string pallet)
        {
            Environment.SetEnvironmentVariable("NLS_LANG",
                    "AMERICAN_AMERICA.WE8MSWIN1252",
                    EnvironmentVariableTarget.Process);

            List<DELIVERY_NOTE_ITEMS> lstobj = new List<DELIVERY_NOTE_ITEMS>();
            StringBuilder sql = new StringBuilder();
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                //StringBuilder sql = new StringBuilder();
                sql.Clear();
                //sql.AppendLine("SELECT * ");
                //sql.AppendLine("FROM WGRB.DELIVERY_NOTE_ITEMS");
                //sql.AppendLine("WHERE PALLET_GO is not null");
                //sql.AppendLine("AND STATUS_GO is not null");
                //sql.AppendLine("AND PALLET_GO = :Pallet");
                //sql.AppendLine("ORDER BY SEQ_NO");

                sql.AppendLine("SELECT t1.*, t2.ITEM_NAME ");
                sql.AppendLine("FROM WGRB.DELIVERY_NOTE_ITEMS t1");
                sql.AppendLine("LEFT JOIN WGRB.ITEMS t2 ON t1.ITEM_CODE = t2.ITEM_CODE");
                sql.AppendLine("WHERE t1.PALLET_GO is not null");
                sql.AppendLine("AND t1.STATUS_GO is not null");
                sql.AppendLine("AND t1.PALLET_GO = :Pallet");
                sql.AppendLine("AND t1.REMARKS is null");
                sql.AppendLine("AND t1.REMARKS_INV is null");
                sql.AppendLine("ORDER BY t1.SEQ_NO");

                try
                {
                    OleDbCommand cmd = new OleDbCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.Add(new OleDbParameter("Pallet", pallet));

                    //await con.OpenAsync();
                    con.Open();
                    OleDbDataReader rdr = cmd.ExecuteReader();
                    while (await rdr.ReadAsync())
                    {

                        DELIVERY_NOTE_ITEMS objrd = new DELIVERY_NOTE_ITEMS
                        {
                            DN_NO = rdr["DN_NO"].ToString(),
                            SEQ_NO = rdr["SEQ_NO"] == DBNull.Value ? null : (Decimal?)rdr["SEQ_NO"],
                            JOB_NO = rdr["JOB_NO"].ToString(),
                            ITEM_CODE = rdr["ITEM_CODE"].ToString(),
                            DN_QTY = rdr["DN_QTY"] == DBNull.Value ? null : (Decimal?)rdr["DN_QTY"],
                            DN_UM = rdr["DN_UM"].ToString(),
                            DN_DATE = rdr["DN_DATE"] == DBNull.Value ? null : (DateTime?)rdr["DN_DATE"],
                            STATUS = rdr["STATUS"].ToString(),
                            CUSTOMER_CODE = rdr["CUSTOMER_CODE"].ToString(),
                            BATCH_NO = rdr["BATCH_NO"].ToString(),
                            FACTORY_ROOM = rdr["FACTORY_ROOM"].ToString(),
                            PALETTE_NO = rdr["PALETTE_NO"].ToString(),
                            BOX_NO = rdr["BOX_NO"] == DBNull.Value ? null : (Decimal?)rdr["BOX_NO"],
                            CLOSE_FLAG = rdr["CLOSE_FLAG"].ToString(),
                            PACKETING_CODE = rdr["PACKETING_CODE"].ToString(),
                            DN_WEIGHT = rdr["DN_WEIGHT"] == DBNull.Value ? null : (Decimal?)rdr["DN_WEIGHT"],
                            DN_WEIGHT_UM = rdr["DN_WEIGHT_UM"].ToString(),
                            LOT_NO = rdr["LOT_NO"].ToString(),
                            STORE_ID = rdr["STORE_ID"].ToString(),
                            SCREEN_NAME = rdr["SCREEN_NAME"].ToString(),
                            CUST_ITEM_CODE = rdr["CUST_ITEM_CODE"].ToString(),
                            VEHICLE_CODE = rdr["VEHICLE_CODE"].ToString(),
                            VEHICLE_LINK_CODE = rdr["VEHICLE_LINK_CODE"].ToString(),
                            PACKAGE_WEIGHT = rdr["PACKAGE_WEIGHT"] == DBNull.Value ? null : (Decimal?)rdr["PACKAGE_WEIGHT"],
                            APP1_FLAG = rdr["APP1_FLAG"].ToString(),
                            APP2_FLAG = rdr["APP2_FLAG"].ToString(),
                            APP3_FLAG = rdr["APP3_FLAG"].ToString(),
                            APP_AG_FLAG = rdr["APP_AG_FLAG"].ToString(),
                            NET_IRON_DATE = rdr["NET_IRON_DATE"] == DBNull.Value ? null : (DateTime?)rdr["NET_IRON_DATE"],
                            NET_IRON_NUMBER = rdr["NET_IRON_NUMBER"] == DBNull.Value ? null : (Decimal?)rdr["NET_IRON_NUMBER"],
                            CREATED_T1_BY = rdr["CREATED_T1_BY"].ToString(),
                            CREATED_T1_ON = rdr["CREATED_T1_ON"] == DBNull.Value ? null : (DateTime?)rdr["CREATED_T1_ON"],
                            APPROVED_T1_BY = rdr["APPROVED_T1_BY"].ToString(),
                            APPROVED_T1_ON = rdr["APPROVED_T1_ON"] == DBNull.Value ? null : (DateTime?)rdr["APPROVED_T1_ON"],
                            NET_IRON_NC = rdr["NET_IRON_NC"].ToString(),
                            INFECTION_DATE = rdr["INFECTION_DATE"] == DBNull.Value ? null : (DateTime?)rdr["INFECTION_DATE"],
                            TPC = rdr["TPC"].ToString(),
                            YM = rdr["YM"].ToString(),
                            SAUREUS = rdr["SAUREUS"].ToString(),
                            ECOLI = rdr["ECOLI"].ToString(),
                            COLIFORM = rdr["COLIFORM"].ToString(),
                            BCEREUS = rdr["BCEREUS"].ToString(),
                            SALLMONELLA = rdr["SALLMONELLA"].ToString(),
                            AW = rdr["AW"].ToString(),
                            REMARKS_INFECTION = rdr["REMARKS_INFECTION"].ToString(),
                            INFECTION_NC = rdr["INFECTION_NC"].ToString(),
                            APPROVED_T2_BY = rdr["APPROVED_T2_BY"].ToString(),
                            APPROVED_T2_ON = rdr["APPROVED_T2_ON"] == DBNull.Value ? null : (DateTime?)rdr["APPROVED_T2_ON"],
                            APPROVED_T3_BY = rdr["APPROVED_T3_BY"].ToString(),
                            APPROVED_T3_ON = rdr["APPROVED_T3_ON"] == DBNull.Value ? null : (DateTime?)rdr["APPROVED_T3_ON"],
                            APPROVED_AG_BY = rdr["APPROVED_AG_BY"].ToString(),
                            APPROVED_AG_ON = rdr["APPROVED_AG_ON"] == DBNull.Value ? null : (DateTime?)rdr["APPROVED_AG_ON"],
                            PK_NO = rdr["PK_NO"].ToString(),
                            PK_SEQ = rdr["PK_SEQ"] == DBNull.Value ? null : (Decimal?)rdr["PK_SEQ"],
                            STICKER_CODE = rdr["STICKER_CODE"].ToString(),
                            SUB_CUSTOMER_CODE = rdr["SUB_CUSTOMER_CODE"].ToString(),
                            TO_PCK_NO = rdr["TO_PCK_NO"].ToString(),
                            EFFECT_TYPE = rdr["EFFECT_TYPE"].ToString(),
                            REMARKS = rdr["REMARKS"].ToString(),
                            FLAG_BOOK = rdr["FLAG_BOOK"].ToString(),
                            SEQ_GROUP = rdr["SEQ_GROUP"] == DBNull.Value ? null : (Decimal?)rdr["SEQ_GROUP"],
                            TANK = rdr["TANK"].ToString(),
                            BRIX = rdr["BRIX"].ToString(),
                            PH = rdr["PH"].ToString(),
                            DE = rdr["DE"].ToString(),
                            SOII = rdr["SOII"].ToString(),
                            BSTAR = rdr["BSTAR"].ToString(),
                            AWQA3 = rdr["AWQA3"].ToString(),
                            REMARKS_QUALITY = rdr["REMARKS_QUALITY"].ToString(),
                            QUALITY_NC = rdr["QUALITY_NC"].ToString(),
                            DATE_QUALITY = rdr["DATE_QUALITY"] == DBNull.Value ? null : (DateTime?)rdr["DATE_QUALITY"],
                            CREATED_T2_BY = rdr["CREATED_T2_BY"].ToString(),
                            CREATED_T2_ON = rdr["CREATED_T2_ON"] == DBNull.Value ? null : (DateTime?)rdr["CREATED_T2_ON"],
                            CREATED_T3_BY = rdr["CREATED_T3_BY"].ToString(),
                            CREATED_T3_ON = rdr["CREATED_T3_ON"] == DBNull.Value ? null : (DateTime?)rdr["CREATED_T3_ON"],
                            APPROVED_INV_BY = rdr["APPROVED_INV_BY"].ToString(),
                            APPROVED_INV_ON = rdr["APPROVED_INV_ON"] == DBNull.Value ? null : (DateTime?)rdr["APPROVED_INV_ON"],
                            CREATED_INV_BY = rdr["CREATED_INV_BY"].ToString(),
                            CREATED_INV_ON = rdr["CREATED_INV_ON"] == DBNull.Value ? null : (DateTime?)rdr["CREATED_INV_ON"],
                            APP_INV_FLAG = rdr["APP_INV_FLAG"].ToString(),
                            REMARKS_INV = rdr["REMARKS_INV"].ToString(),
                            ITEM_STATUS = rdr["ITEM_STATUS"].ToString(),
                            MJ3 = rdr["MJ3"].ToString(),
                            BULKD3 = rdr["BULKD3"].ToString(),
                            PACKBD3 = rdr["PACKBD3"].ToString(),
                            NC_BOOK = rdr["NC_BOOK"].ToString(),
                            RACK_NO = rdr["RACK_NO"].ToString(),
                            BLACK_CARBON = rdr["BLACK_CARBON"].ToString(),
                            DUP_PCK_NO = rdr["DUP_PCK_NO"] == DBNull.Value ? null : (Decimal?)rdr["DUP_PCK_NO"],
                            PCK_NO = rdr["PCK_NO"] == DBNull.Value ? null : (Decimal?)rdr["PCK_NO"],
                            STATUS_LOAD = rdr["STATUS_LOAD"].ToString(),
                            PACKING_DATE = rdr["PACKING_DATE"] == DBNull.Value ? null : (DateTime?)rdr["PACKING_DATE"],
                            YEAST = rdr["YEAST"] == DBNull.Value ? null : (Decimal?)rdr["YEAST"],
                            MOLD = rdr["MOLD"] == DBNull.Value ? null : (Decimal?)rdr["MOLD"],
                            SHELF_LIFE_DAY = rdr["SHELF_LIFE_DAY"] == DBNull.Value ? null : (Decimal?)rdr["SHELF_LIFE_DAY"],
                            SHELF_LIFE_KEEP_IN = rdr["SHELF_LIFE_KEEP_IN"] == DBNull.Value ? null : (Decimal?)rdr["SHELF_LIFE_KEEP_IN"],
                            SHELF_LIFE_DATE = rdr["SHELF_LIFE_DATE"] == DBNull.Value ? null : (DateTime?)rdr["SHELF_LIFE_DATE"],
                            SHELF_LIFE_KEEP_IN_DATE = rdr["SHELF_LIFE_KEEP_IN_DATE"] == DBNull.Value ? null : (DateTime?)rdr["SHELF_LIFE_KEEP_IN_DATE"],
                            PALET_SORT = rdr["PALET_SORT"] == DBNull.Value ? null : (Decimal?)rdr["PALET_SORT"],
                            HIT_LET = rdr["HIT_LET"].ToString(),
                            LOT_CORN = rdr["LOT_CORN"].ToString(),
                            TANK_EF = rdr["TANK_EF"].ToString(),
                            REF_DATE = rdr["REF_DATE"] == DBNull.Value ? null : (DateTime?)rdr["REF_DATE"],
                            LSTAR = rdr["LSTAR"].ToString(),
                            ASTAR = rdr["ASTAR"].ToString(),
                            REMARKS_ISS = rdr["REMARKS_ISS"].ToString(),
                            INJECTION_DATE = rdr["INJECTION_DATE"] == DBNull.Value ? null : (DateTime?)rdr["INJECTION_DATE"],
                            SPRAY_DATE = rdr["SPRAY_DATE"] == DBNull.Value ? null : (DateTime?)rdr["SPRAY_DATE"],
                            BATCH_NO_CUST = rdr["BATCH_NO_CUST"].ToString(),
                            PALLET_GO = rdr["PALLET_GO"].ToString(),
                            STATUS_GO = rdr["STATUS_GO"].ToString(),
                            UPDATE_DATE_GO = rdr["UPDATE_DATE_GO"] == DBNull.Value ? null : (DateTime?)rdr["UPDATE_DATE_GO"],
                            CREATED_BY = rdr["CREATED_BY"].ToString(),
                            CREATED_DATE = rdr["CREATED_DATE"] == DBNull.Value ? null : (DateTime?)rdr["CREATED_DATE"],
                            ITEM_NAME = rdr["ITEM_NAME"].ToString()
                        };
                        lstobj.Add(objrd);
                    }
                }
                catch (Exception e)
                {

                    throw;
                }
                con.Close();
            }
            return lstobj;
        }

        public  IEnumerable<Api_Receivingorders_Go> GetMapPalletApiNewDelivery_note(string pallet)
        {
            Environment.SetEnvironmentVariable("NLS_LANG",
                    "AMERICAN_AMERICA.WE8MSWIN1252",
                    EnvironmentVariableTarget.Process);

            List<Api_Receivingorders_Go> lstobj = new List<Api_Receivingorders_Go>();
            StringBuilder sql = new StringBuilder();
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                //StringBuilder sql = new StringBuilder();
                sql.Clear();
                //sql.AppendLine("SELECT * ");
                //sql.AppendLine("FROM WGRB.DELIVERY_NOTE_ITEMS");
                //sql.AppendLine("WHERE PALLET_GO is not null");
                //sql.AppendLine("AND STATUS_GO is not null");
                //sql.AppendLine("AND PALLET_GO = :Pallet");
                //sql.AppendLine("ORDER BY SEQ_NO");

                sql.AppendLine("SELECT t1.*, t2.ITEM_NAME, t2.ITEM_TYPE ");
                sql.AppendLine("FROM WGRB.DELIVERY_NOTE_ITEMS t1");
                sql.AppendLine("LEFT JOIN WGRB.ITEMS t2 ON t1.ITEM_CODE = t2.ITEM_CODE");
                //sql.AppendLine("WHERE t1.PALLET_GO is not null");
                sql.AppendLine("WHERE t1.PALLET_GO = :Pallet");
                //sql.AppendLine("AND t1.STATUS_GO is null");
               
                sql.AppendLine("AND t1.STATUS = :Status");
                sql.AppendLine("AND t1.REMARKS is null");
                sql.AppendLine("AND t1.REMARKS_INV is null");
                sql.AppendLine("ORDER BY t1.SEQ_NO");

                try
                {
                    OleDbCommand cmd = new OleDbCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.Add(new OleDbParameter("Pallet", pallet));
                    cmd.Parameters.Add(new OleDbParameter("Status", "A"));

                    //await con.OpenAsync();
                    con.Open();
                    OleDbDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        string pack = rdr["SEQ_NO"].ToString();

                        Api_Receivingorders_Go objrd = new Api_Receivingorders_Go
                        {
                            //DN_NO = rdr["DN_NO"].ToString(),
                            //SEQ_NO = rdr["SEQ_NO"] == DBNull.Value ? null : (Decimal?)rdr["SEQ_NO"],
                            //JOB_NO = rdr["JOB_NO"].ToString(),
                            //ITEM_CODE = rdr["ITEM_CODE"].ToString(),
                            //DN_QTY = rdr["DN_QTY"] == DBNull.Value ? null : (Decimal?)rdr["DN_QTY"],
                            //DN_UM = rdr["DN_UM"].ToString(),
                            //DN_DATE = rdr["DN_DATE"] == DBNull.Value ? null : (DateTime?)rdr["DN_DATE"],
                            //STATUS = rdr["STATUS"].ToString(),
                            //CUSTOMER_CODE = rdr["CUSTOMER_CODE"].ToString(),
                            //BATCH_NO = rdr["BATCH_NO"].ToString(),
                            //FACTORY_ROOM = rdr["FACTORY_ROOM"].ToString(),
                            //PALETTE_NO = rdr["PALETTE_NO"].ToString(),
                            //BOX_NO = rdr["BOX_NO"] == DBNull.Value ? null : (Decimal?)rdr["BOX_NO"],
                            //CLOSE_FLAG = rdr["CLOSE_FLAG"].ToString(),
                            //PACKETING_CODE = rdr["PACKETING_CODE"].ToString(),
                            //DN_WEIGHT = rdr["DN_WEIGHT"] == DBNull.Value ? null : (Decimal?)rdr["DN_WEIGHT"],
                            //DN_WEIGHT_UM = rdr["DN_WEIGHT_UM"].ToString(),
                            //LOT_NO = rdr["LOT_NO"].ToString(),
                            //STORE_ID = rdr["STORE_ID"].ToString(),
                            //SCREEN_NAME = rdr["SCREEN_NAME"].ToString(),
                            //CUST_ITEM_CODE = rdr["CUST_ITEM_CODE"].ToString(),
                            //VEHICLE_CODE = rdr["VEHICLE_CODE"].ToString(),
                            //VEHICLE_LINK_CODE = rdr["VEHICLE_LINK_CODE"].ToString(),
                            //PACKAGE_WEIGHT = rdr["PACKAGE_WEIGHT"] == DBNull.Value ? null : (Decimal?)rdr["PACKAGE_WEIGHT"],
                            //APP1_FLAG = rdr["APP1_FLAG"].ToString(),
                            //APP2_FLAG = rdr["APP2_FLAG"].ToString(),
                            //APP3_FLAG = rdr["APP3_FLAG"].ToString(),
                            //APP_AG_FLAG = rdr["APP_AG_FLAG"].ToString(),
                            //NET_IRON_DATE = rdr["NET_IRON_DATE"] == DBNull.Value ? null : (DateTime?)rdr["NET_IRON_DATE"],
                            //NET_IRON_NUMBER = rdr["NET_IRON_NUMBER"] == DBNull.Value ? null : (Decimal?)rdr["NET_IRON_NUMBER"],
                            //CREATED_T1_BY = rdr["CREATED_T1_BY"].ToString(),
                            //CREATED_T1_ON = rdr["CREATED_T1_ON"] == DBNull.Value ? null : (DateTime?)rdr["CREATED_T1_ON"],
                            //APPROVED_T1_BY = rdr["APPROVED_T1_BY"].ToString(),
                            //APPROVED_T1_ON = rdr["APPROVED_T1_ON"] == DBNull.Value ? null : (DateTime?)rdr["APPROVED_T1_ON"],
                            //NET_IRON_NC = rdr["NET_IRON_NC"].ToString(),
                            //INFECTION_DATE = rdr["INFECTION_DATE"] == DBNull.Value ? null : (DateTime?)rdr["INFECTION_DATE"],
                            //TPC = rdr["TPC"].ToString(),
                            //YM = rdr["YM"].ToString(),
                            //SAUREUS = rdr["SAUREUS"].ToString(),
                            //ECOLI = rdr["ECOLI"].ToString(),
                            //COLIFORM = rdr["COLIFORM"].ToString(),
                            //BCEREUS = rdr["BCEREUS"].ToString(),
                            //SALLMONELLA = rdr["SALLMONELLA"].ToString(),
                            //AW = rdr["AW"].ToString(),
                            //REMARKS_INFECTION = rdr["REMARKS_INFECTION"].ToString(),
                            //INFECTION_NC = rdr["INFECTION_NC"].ToString(),
                            //APPROVED_T2_BY = rdr["APPROVED_T2_BY"].ToString(),
                            //APPROVED_T2_ON = rdr["APPROVED_T2_ON"] == DBNull.Value ? null : (DateTime?)rdr["APPROVED_T2_ON"],
                            //APPROVED_T3_BY = rdr["APPROVED_T3_BY"].ToString(),
                            //APPROVED_T3_ON = rdr["APPROVED_T3_ON"] == DBNull.Value ? null : (DateTime?)rdr["APPROVED_T3_ON"],
                            //APPROVED_AG_BY = rdr["APPROVED_AG_BY"].ToString(),
                            //APPROVED_AG_ON = rdr["APPROVED_AG_ON"] == DBNull.Value ? null : (DateTime?)rdr["APPROVED_AG_ON"],
                            //PK_NO = rdr["PK_NO"].ToString(),
                            //PK_SEQ = rdr["PK_SEQ"] == DBNull.Value ? null : (Decimal?)rdr["PK_SEQ"],
                            //STICKER_CODE = rdr["STICKER_CODE"].ToString(),
                            //SUB_CUSTOMER_CODE = rdr["SUB_CUSTOMER_CODE"].ToString(),
                            //TO_PCK_NO = rdr["TO_PCK_NO"].ToString(),
                            //EFFECT_TYPE = rdr["EFFECT_TYPE"].ToString(),
                            //REMARKS = rdr["REMARKS"].ToString(),
                            //FLAG_BOOK = rdr["FLAG_BOOK"].ToString(),
                            //SEQ_GROUP = rdr["SEQ_GROUP"] == DBNull.Value ? null : (Decimal?)rdr["SEQ_GROUP"],
                            //TANK = rdr["TANK"].ToString(),
                            //BRIX = rdr["BRIX"].ToString(),
                            //PH = rdr["PH"].ToString(),
                            //DE = rdr["DE"].ToString(),
                            //SOII = rdr["SOII"].ToString(),
                            //BSTAR = rdr["BSTAR"].ToString(),
                            //AWQA3 = rdr["AWQA3"].ToString(),
                            //REMARKS_QUALITY = rdr["REMARKS_QUALITY"].ToString(),
                            //QUALITY_NC = rdr["QUALITY_NC"].ToString(),
                            //DATE_QUALITY = rdr["DATE_QUALITY"] == DBNull.Value ? null : (DateTime?)rdr["DATE_QUALITY"],
                            //CREATED_T2_BY = rdr["CREATED_T2_BY"].ToString(),
                            //CREATED_T2_ON = rdr["CREATED_T2_ON"] == DBNull.Value ? null : (DateTime?)rdr["CREATED_T2_ON"],
                            //CREATED_T3_BY = rdr["CREATED_T3_BY"].ToString(),
                            //CREATED_T3_ON = rdr["CREATED_T3_ON"] == DBNull.Value ? null : (DateTime?)rdr["CREATED_T3_ON"],
                            //APPROVED_INV_BY = rdr["APPROVED_INV_BY"].ToString(),
                            //APPROVED_INV_ON = rdr["APPROVED_INV_ON"] == DBNull.Value ? null : (DateTime?)rdr["APPROVED_INV_ON"],
                            //CREATED_INV_BY = rdr["CREATED_INV_BY"].ToString(),
                            //CREATED_INV_ON = rdr["CREATED_INV_ON"] == DBNull.Value ? null : (DateTime?)rdr["CREATED_INV_ON"],
                            //APP_INV_FLAG = rdr["APP_INV_FLAG"].ToString(),
                            //REMARKS_INV = rdr["REMARKS_INV"].ToString(),
                            //ITEM_STATUS = rdr["ITEM_STATUS"].ToString(),
                            //MJ3 = rdr["MJ3"].ToString(),
                            //BULKD3 = rdr["BULKD3"].ToString(),
                            //PACKBD3 = rdr["PACKBD3"].ToString(),
                            //NC_BOOK = rdr["NC_BOOK"].ToString(),
                            //RACK_NO = rdr["RACK_NO"].ToString(),
                            //BLACK_CARBON = rdr["BLACK_CARBON"].ToString(),
                            //DUP_PCK_NO = rdr["DUP_PCK_NO"] == DBNull.Value ? null : (Decimal?)rdr["DUP_PCK_NO"],
                            //PCK_NO = rdr["PCK_NO"] == DBNull.Value ? null : (Decimal?)rdr["PCK_NO"],
                            //STATUS_LOAD = rdr["STATUS_LOAD"].ToString(),
                            //PACKING_DATE = rdr["PACKING_DATE"] == DBNull.Value ? null : (DateTime?)rdr["PACKING_DATE"],
                            //YEAST = rdr["YEAST"] == DBNull.Value ? null : (Decimal?)rdr["YEAST"],
                            //MOLD = rdr["MOLD"] == DBNull.Value ? null : (Decimal?)rdr["MOLD"],
                            //SHELF_LIFE_DAY = rdr["SHELF_LIFE_DAY"] == DBNull.Value ? null : (Decimal?)rdr["SHELF_LIFE_DAY"],
                            //SHELF_LIFE_KEEP_IN = rdr["SHELF_LIFE_KEEP_IN"] == DBNull.Value ? null : (Decimal?)rdr["SHELF_LIFE_KEEP_IN"],
                            //SHELF_LIFE_DATE = rdr["SHELF_LIFE_DATE"] == DBNull.Value ? null : (DateTime?)rdr["SHELF_LIFE_DATE"],
                            //SHELF_LIFE_KEEP_IN_DATE = rdr["SHELF_LIFE_KEEP_IN_DATE"] == DBNull.Value ? null : (DateTime?)rdr["SHELF_LIFE_KEEP_IN_DATE"],
                            //PALET_SORT = rdr["PALET_SORT"] == DBNull.Value ? null : (Decimal?)rdr["PALET_SORT"],
                            //HIT_LET = rdr["HIT_LET"].ToString(),
                            //LOT_CORN = rdr["LOT_CORN"].ToString(),
                            //TANK_EF = rdr["TANK_EF"].ToString(),
                            //REF_DATE = rdr["REF_DATE"] == DBNull.Value ? null : (DateTime?)rdr["REF_DATE"],
                            //LSTAR = rdr["LSTAR"].ToString(),
                            //ASTAR = rdr["ASTAR"].ToString(),
                            //REMARKS_ISS = rdr["REMARKS_ISS"].ToString(),
                            //INJECTION_DATE = rdr["INJECTION_DATE"] == DBNull.Value ? null : (DateTime?)rdr["INJECTION_DATE"],
                            //SPRAY_DATE = rdr["SPRAY_DATE"] == DBNull.Value ? null : (DateTime?)rdr["SPRAY_DATE"],
                            //BATCH_NO_CUST = rdr["BATCH_NO_CUST"].ToString(),
                            //PALLET_GO = rdr["PALLET_GO"].ToString(),
                            //STATUS_GO = rdr["STATUS_GO"].ToString(),
                            //UPDATE_DATE_GO = rdr["UPDATE_DATE_GO"] == DBNull.Value ? null : (DateTime?)rdr["UPDATE_DATE_GO"],
                            //CREATED_BY = rdr["CREATED_BY"].ToString(),
                            //CREATED_DATE = rdr["CREATED_DATE"] == DBNull.Value ? null : (DateTime?)rdr["CREATED_DATE"],
                            //ITEM_NAME = rdr["ITEM_NAME"].ToString(),

                            Package_Id = pack,

                            //Package_Id =  rdr["DN_NO"].ToString()  + "." + rdr["PALLET_GO"].ToString() + "." + pack,
                            Roll_Id = rdr["DN_NO"].ToString()  + "." + pack,
                            Material_Code = rdr["ITEM_CODE"].ToString(),
                            Material_Description = rdr["ITEM_NAME"].ToString(),
                            Receiving_Date = rdr["CREATED_DATE"] == DBNull.Value ? null : (DateTime?)rdr["CREATED_DATE"],
                            Gr_Quantity = rdr["DN_QTY"] == DBNull.Value ? null : (decimal?)rdr["DN_QTY"],
                            Unit = rdr["DN_UM"].ToString(),
                            Gr_Quantity_Kg = rdr["DN_WEIGHT"] == DBNull.Value ? null : (decimal?)rdr["DN_WEIGHT"],
                            Wh_Code = rdr["STORE_ID"].ToString(),
                            Warehouse = rdr["CUSTOMER_CODE"].ToString(),
                            Locationno = rdr["FACTORY_ROOM"].ToString(),
                            Document_Number = rdr["DN_NO"].ToString(),
                            Job = rdr["LOT_NO"].ToString(),
                            Job_Code = rdr["BATCH_NO"].ToString(),
                            Matcategory = rdr["ITEM_TYPE"].ToString()
                        };
                        lstobj.Add(objrd);
                    }
                }
                catch (Exception e)
                {

                    throw;
                }
                con.Close();
            }
            return lstobj;
        }

        public void UpdateNewDelivery_note(string pallet)
        {
            Environment.SetEnvironmentVariable("NLS_LANG",
                    "AMERICAN_AMERICA.WE8MSWIN1252",
                    EnvironmentVariableTarget.Process);

            using OleDbConnection con = new OleDbConnection(connectionString);
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("Update WGRB.DELIVERY_NOTE_ITEMS");
            sql.AppendLine("Set STATUS_GO = ?");
            sql.AppendLine(", UPDATE_DATE_GO = CURRENT_TIMESTAMP");
            sql.AppendLine("Where PALLET_GO = ?");
            sql.AppendLine("And  STATUS = ?");

            //sql.AppendLine(";");

            OleDbCommand cmd = new OleDbCommand(sql.ToString(), con)
            {
                CommandType = CommandType.Text
            };

            cmd.Parameters.AddWithValue("@STATUS_GO", "Y");
            cmd.Parameters.AddWithValue("@PALLET_GO", pallet);
            cmd.Parameters.AddWithValue("@STATUS", "A");

            //cmd.Parameters.Add(new OleDbParameter("@STATUS_GO", OleDbType.VarChar, 1, "Y"));
            //cmd.Parameters.Add(new OleDbParameter("@PALLET_GO", OleDbType.VarChar, 10, pallet));
            //cmd.Parameters.Add(new OleDbParameter("@STATUS", OleDbType.VarChar, 1, "A"));

            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                string emsg = e.Message;
            }
            finally
            {
                con.Close();
            }
           
           
        }

        public async Task<IEnumerable<CUSTOMERS>>GetAllApiAllCustomer()
        {
            Environment.SetEnvironmentVariable("NLS_LANG",
                    "AMERICAN_AMERICA.WE8MSWIN1252",
                    EnvironmentVariableTarget.Process);

            List<CUSTOMERS> lstobj = new List<CUSTOMERS>();
            StringBuilder sql = new StringBuilder();

            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                //StringBuilder sql = new StringBuilder();
                sql.Clear();
                sql.AppendLine("SELECT * ");
                sql.AppendLine("FROM WGRB.CUSTOMERS");
                //sql.AppendLine("WHERE PALLET_GO is not null");
                //sql.AppendLine("AND STATUS_GO is not null");
                //sql.AppendLine("ORDER BY SEQ_NO");

                try
                {
                    OleDbCommand cmd = new OleDbCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };

                     con.Open();


                    // con.Open();
                    OleDbDataReader rdr = cmd.ExecuteReader();
                    while (await rdr.ReadAsync())
                    {
                        CUSTOMERS objrd = new CUSTOMERS
                        {
                            CUSTOMER_CODE = rdr["CUSTOMER_CODE"].ToString(),
                            CUST_NAME_THAI = rdr["CUST_NAME_THAI"].ToString(),
                            STATUS = rdr["STATUS"].ToString()           
                        };
                        lstobj.Add(objrd);
                    }
                }
                catch (Exception e)
                {
                    throw;
                }
                con.Close();
            }
            return lstobj;
        }

        public async Task<IEnumerable<ITEMS>> GetAllApiAllItem()
        {
            Environment.SetEnvironmentVariable("NLS_LANG",
                    "AMERICAN_AMERICA.WE8MSWIN1252",
                    EnvironmentVariableTarget.Process);

            List<ITEMS> lstobj = new List<ITEMS>();
            StringBuilder sql = new StringBuilder();
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                sql.Clear();
                sql.AppendLine("SELECT * ");
                sql.AppendLine("FROM WGRB.ITEMS");
                //sql.AppendLine("WHERE PALLET_GO is not null");
                //sql.AppendLine("AND STATUS_GO is not null");
                //sql.AppendLine("ORDER BY SEQ_NO");

                try
                {
                    OleDbCommand cmd = new OleDbCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    //await con.OpenAsync();
                    con.Open();
                    OleDbDataReader rdr = cmd.ExecuteReader();
                    while (await rdr.ReadAsync())
                    {
                        ITEMS objrd = new ITEMS
                        {
                            ITEM_CODE = rdr["ITEM_CODE"].ToString(),
                            ITEM_NAME = rdr["ITEM_NAME"].ToString(),
                            ITEM_TYPE = rdr["ITEM_TYPE"].ToString(),
                            RECORD_TYPE = rdr["RECORD_TYPE"].ToString(),
                            ITEM_UM = rdr["ITEM_UM"].ToString(),
                            STATUS = rdr["STATUS"].ToString()
                        };
                        lstobj.Add(objrd);
                    }
                }
                catch (Exception e)
                {
                    throw;
                }
                con.Close();
            }
            return lstobj;
        }

        public async Task<IEnumerable<PACKETINGS>> GetAllApiAllPackeing()
        {
            Environment.SetEnvironmentVariable("NLS_LANG",
                    "AMERICAN_AMERICA.WE8MSWIN1252",
                    EnvironmentVariableTarget.Process);

            List<PACKETINGS> lstobj = new List<PACKETINGS>();
            StringBuilder sql = new StringBuilder();

            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                sql.Clear();
                sql.AppendLine("SELECT * ");
                sql.AppendLine("FROM WGRB.PACKETINGS");
                //sql.AppendLine("WHERE PALLET_GO is not null");
                //sql.AppendLine("AND STATUS_GO is not null");
                //sql.AppendLine("ORDER BY SEQ_NO");

                try
                {
                    OleDbCommand cmd = new OleDbCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    //await con.OpenAsync();
                    con.Open();
                    OleDbDataReader rdr = cmd.ExecuteReader();
                    while (await rdr.ReadAsync())
                    {

                        PACKETINGS objrd = new PACKETINGS
                        {
                            PACKETING_CODE = rdr["PACKETING_CODE"].ToString(),
                            PACKETING_NAME = rdr["PACKETING_NAME"].ToString(),
                            PACKAGE_WEIGHT = rdr["PACKAGE_WEIGHT"] == DBNull.Value ? null : (Decimal?)rdr["PACKAGE_WEIGHT"],
                            ITEM_CODE = rdr["ITEM_CODE"].ToString(),
                            PACKAGE_TPYE = rdr[6].ToString(), //PACKAGE_TPYE
                            ITEM_UM = rdr["ITEM_UM"].ToString(),
                            TARE_WEIGHT = rdr["TARE_WEIGHT"] == DBNull.Value ? null : (Decimal?)rdr["TARE_WEIGHT"],
                            GROSS_WEIGHT = rdr["GROSS_WEIGHT"] == DBNull.Value ? null : (Decimal?)rdr["GROSS_WEIGHT"]
                        };
                        lstobj.Add(objrd);
                    }
                }
                catch (Exception e)
                {

                    throw;
                }
                con.Close();
            }
            return lstobj;
        }

    }
}
