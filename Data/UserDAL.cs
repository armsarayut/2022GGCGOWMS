using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using GoWMS.Server.Models;
using Npgsql;
using NpgsqlTypes;
using System.Text;

using Serilog;


namespace GoWMS.Server.Data
{
    public class UserDAL
    {
        readonly private string connectionString = ConnGlobals.GetConnDBSQL();

        public IEnumerable<Userinfo> GetUserAll()
        {
            List<Userinfo> lstobj = new List<Userinfo>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();

                    sql.AppendLine("select u.idx, u.usid, u.usfirstname, u.uspass");
                    sql.AppendLine(", u.ugid, g.ugdesc");
                    sql.AppendLine("from dbo.set_users u");
                    sql.AppendLine("inner join dbo.set_usergroups g");
                    sql.AppendLine("on u.ugid=g.idx");
                    sql.AppendLine(";");

                    SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };

                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Userinfo objrd = new Userinfo
                        {
                            Usid = rdr["idx"] == DBNull.Value ? null : (long?)rdr["idx"],
                            UserId = rdr["usid"].ToString(),
                            Usname = rdr["usfirstname"].ToString(),
                            Uspass = rdr["uspass"].ToString(),
                            Usgid = rdr["ugid"] == DBNull.Value ? null : (long?)rdr["ugid"],
                            Usgrp = rdr["ugdesc"].ToString(),
                        };
                        lstobj.Add(objrd);
                    }
                }
                catch (SqlException ex)
                {
                    Log.Error(ex.ToString());
                }
                finally
                {
                    con.Close();
                }
            }
            return lstobj;
        }


        public IEnumerable<Usergroups> GetUsergroups()
        {
            List<Usergroups> lstobj = new List<Usergroups>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();

                    sql.AppendLine("select idx, created, entity_lock, modified, client_id, client_ip, ugdesc");
                    sql.AppendLine("from dbo.set_usergroups");
                    sql.AppendLine(";");

                    SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };

                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Usergroups objrd = new Usergroups
                        {
                            Idx = rdr["idx"] == DBNull.Value ? null : (long?)rdr["idx"],
                            Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                            Entity_lock = rdr["entity_lock"] == DBNull.Value ? null : (Int32?)rdr["entity_lock"],
                            Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                            Client_id = rdr["client_id"] == DBNull.Value ? null : (long?)rdr["client_id"],
                            Client_ip = rdr["client_ip"].ToString(),
                            Ugdesc = rdr["ugdesc"].ToString(),
                        };
                        lstobj.Add(objrd);
                    }
                }
                catch (SqlException ex)
                {
                    Log.Error(ex.ToString());
                }
                finally
                {
                    con.Close();
                }
            }
            return lstobj;
        }


        public IEnumerable<Userinfo> GetUser(string uusname, string uspass)
        {
            List<Userinfo> lstobj = new List<Userinfo>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();

                    sql.AppendLine("select top(1) u.idx, u.usid, u.usfirstname, u.uspass");
                    sql.AppendLine(", u.ugid");
                    sql.AppendLine("from dbo.set_users u");
                    sql.AppendLine("inner join dbo.set_usergroups g");
                    sql.AppendLine("on u.ugid=g.idx");
                    sql.AppendLine("where");
                    sql.AppendLine("u.usid=@usid");
                    sql.AppendLine("and");
                    sql.AppendLine("u.uspass=@uspass");
                    //sql.AppendLine("Limit 1");
                    sql.AppendLine(";");

                    SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@usid", uusname);
                    cmd.Parameters.AddWithValue("@uspass", uspass);

                    //cmd.Parameters.AddWithValue("@usid", NpgsqlDbType.Varchar, uusname);
                    //cmd.Parameters.AddWithValue("@uspass", NpgsqlDbType.Varchar, uspass);

                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Userinfo objrd = new Userinfo
                        {
                            Usid = rdr["idx"] == DBNull.Value ? null : (long?)rdr["idx"],
                            Usname = rdr["usfirstname"].ToString(),
                            Uspass = rdr["uspass"].ToString(),
                            Usgid = rdr["ugid"] == DBNull.Value ? null : (long?)rdr["ugid"],
                        };
                        lstobj.Add(objrd);
                    }
                }
                catch (NpgsqlException ex)
                {
                    Log.Error(ex.ToString());
                }
                finally
                {
                    con.Close();
                }
            }
            return lstobj;
        }

        public IEnumerable<Userroleinfo> GetUserRole(string menu_desc, long group_id)
        {
            List<Userroleinfo> lstobj = new List<Userroleinfo>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();

                    sql.AppendLine("SELECT  role_acc, role_add, role_edit, role_del, role_rpt, role_apv");
                    sql.AppendLine("FROM dbo.vrole");
                    sql.AppendLine("WHERE menu_id = @menu_desc");
                    sql.AppendLine("AND group_id = @group_id");
                    sql.AppendLine(";");


                    SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };

                    cmd.Parameters.AddWithValue("@menu_desc", menu_desc);
                    cmd.Parameters.AddWithValue("@group_id", group_id);

                    //cmd.Parameters.AddWithValue("@menu_desc", NpgsqlDbType.Varchar, menu_desc);
                    //cmd.Parameters.AddWithValue("@group_id", NpgsqlDbType.Bigint, group_id);

                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Userroleinfo objrd = new Userroleinfo
                        {
                            Role_acc = rdr["role_acc"] == DBNull.Value ? false : (bool?)rdr["role_acc"],
                            Role_add = rdr["role_add"] == DBNull.Value ? false : (bool?)rdr["role_add"],
                            Role_edit = rdr["role_edit"] == DBNull.Value ? false : (bool?)rdr["role_edit"],
                            Role_del = rdr["role_del"] == DBNull.Value ? false : (bool?)rdr["role_del"],
                            Role_rpt = rdr["role_rpt"] == DBNull.Value ? false : (bool?)rdr["role_rpt"],
                            Role_apv = rdr["role_apv"] == DBNull.Value ? false : (bool?)rdr["role_apv"],

                        };
                        lstobj.Add(objrd);
                    }
                }
                catch (SqlException ex)
                {
                    Log.Error(ex.ToString());
                }
                finally
                {
                    con.Close();
                }
            }
            return lstobj;
        }


        public IEnumerable<UserPrivilege> GetPrivilegeAll()
        {
            List<UserPrivilege> lstobj = new List<UserPrivilege>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();


                    sql.AppendLine("select ");
                    sql.AppendLine("r.idx, r.created, r.entity_lock, r.modified, r.client_id, r.client_ip, ");
                    sql.AppendLine("r.group_id, u.ugdesc, r.menu_id, m.menu_desc, ");
                    sql.AppendLine("r.role_acc, r.role_add, r.role_edit, r.role_del, r.role_rpt, r.role_apv");
                    sql.AppendLine("from dbo.set_privilege r");
                    sql.AppendLine("inner join dbo.set_usergroups u");
                    sql.AppendLine("on r.group_id=u.idx");
                    sql.AppendLine("inner join dbo.set_menu m");
                    sql.AppendLine("on r.menu_id=m.menu_code");
                    sql.AppendLine("where (1=1)");
                    sql.AppendLine("order by r.group_id asc, m.menu_desc asc");
                    sql.AppendLine(";");




                    SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };


                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        UserPrivilege objrd = new UserPrivilege
                        {
                            Idx = rdr["idx"] == DBNull.Value ? 0 : (long?)rdr["idx"],
                            Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                            Entity_lock = rdr["entity_lock"] == DBNull.Value ? 0 : (Int32?)rdr["entity_lock"],
                            Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                            Client_id = rdr["client_id"] == DBNull.Value ? 0 : (long?)rdr["client_id"],
                            Group_id = rdr["group_id"] == DBNull.Value ? 0 : (long?)rdr["group_id"],
                            Ugdesc = rdr["ugdesc"].ToString(),
                            Menu_id = rdr["menu_id"].ToString(),
                            Menu_desc = rdr["menu_desc"].ToString(),
                            Role_acc = rdr["role_acc"] == DBNull.Value ? false : (bool?)rdr["role_acc"],
                            Role_add = rdr["role_add"] == DBNull.Value ? false : (bool?)rdr["role_add"],
                            Role_edit = rdr["role_edit"] == DBNull.Value ? false : (bool?)rdr["role_edit"],
                            Role_del = rdr["role_del"] == DBNull.Value ? false : (bool?)rdr["role_del"],
                            Role_rpt = rdr["role_rpt"] == DBNull.Value ? false : (bool?)rdr["role_rpt"],
                            Role_apv = rdr["role_apv"] == DBNull.Value ? false : (bool?)rdr["role_apv"],

                        };
                        lstobj.Add(objrd);
                    }
                }
                catch (SqlException ex)
                {
                    Log.Error(ex.ToString());
                }
                finally
                {
                    con.Close();
                }
            }
            return lstobj;
        }




        public async Task<ResultReturn> InsertPrivilege(string mnuName)
        {

            ResultReturn listRet = new ResultReturn
            {
                Bret = false,
                Iret = 0,
                Sret = "Error:Intarial"
            };

            using SqlConnection con = new SqlConnection(connectionString);
            try
            {
                StringBuilder sql = new StringBuilder();
                //sql.AppendLine("insert into dbo.set_privilege");
                //sql.AppendLine("(group_id,menu_id)");
                //sql.AppendLine("SELECT idx as group_id , menu_code as menu_id  FROM dbo.vmenu_group ");
                //sql.AppendLine("WHERE menu_code = @menu_code");
                //sql.AppendLine(" on conflict (group_id, menu_id)");
                //sql.AppendLine(" do ");
                //sql.AppendLine(" update");
                //sql.AppendLine("     Set menu_id=@menu_id");
                //sql.AppendLine(";");


                sql.AppendLine("MERGE dbo.set_privilege WITH (HOLDLOCK) AS T");
                sql.Append("USING (");
                sql.Append("SELECT idx as group_id , menu_code as menu_id  FROM dbo.vmenu_group ");
                sql.Append("WHERE menu_code = @menu_code");
                sql.AppendLine(") AS U (group_id, menu_id)");
                sql.AppendLine("ON U.menu_id = T.menu_id");
                sql.AppendLine("AND U.group_id = T.group_id");
                sql.AppendLine("WHEN MATCHED THEN ");
                sql.AppendLine("UPDATE SET T.menu_id =U.menu_id");
                sql.AppendLine("WHEN NOT MATCHED THEN");
                sql.AppendLine("INSERT(group_id, menu_id)");
                sql.AppendLine("VALUES (U.group_id, U.menu_id)");
                sql.AppendLine(";");



                using var cmd = new SqlCommand(connection: con, cmdText: null);


                string sParammenu_code = "@menu_code";
                //string sParammenu_id = "@menu_id";

                cmd.Parameters.AddWithValue(sParammenu_code, mnuName);
                //cmd.Parameters.AddWithValue(sParammenu_id, mnuName);

                //cmd.Parameters.Add(new SqlParameter<string>(sParammenu_code, mnuName));
                //cmd.Parameters.Add(new SqlParameter<string>(sParammenu_id, mnuName));

                con.Open();
                cmd.CommandText = sql.ToString();
                await cmd.ExecuteNonQueryAsync();
                listRet.Bret = true;
                listRet.Iret = 1;
                listRet.Sret = "OK";

            }
            catch (NpgsqlException ex)
            {
                Log.Error(ex.ToString());
                listRet.Bret = false;
                listRet.Iret = 0;
                listRet.Sret = "Error :" + ex.ToString();
            }
            finally
            {
                con.Close();
            }

            return listRet;
        }

        public async Task<ResultReturn> InsertMenu(string mnuName, string mundesc)
        {

            ResultReturn listRet = new ResultReturn
            {
                Bret = false,
                Iret = 0,
                Sret = "Error:Intarial"
            };

            using SqlConnection con = new SqlConnection(connectionString);
            try
            {
                StringBuilder sql = new StringBuilder();


                sql.AppendLine("MERGE dbo.set_menu WITH (HOLDLOCK) AS T");
                sql.AppendLine("USING (VALUES (@menu_code, @menu_desc)) AS U (menu_code, menu_desc)");
                sql.AppendLine("ON U.menu_code = T.menu_code");
                sql.AppendLine("WHEN MATCHED THEN ");
                sql.AppendLine("UPDATE SET T.menu_desc =U.menu_desc");
                sql.AppendLine("WHEN NOT MATCHED THEN");
                sql.AppendLine("INSERT(menu_code, menu_desc)");
                sql.AppendLine("VALUES (U.menu_code, U.menu_desc)");
                sql.AppendLine(";");


                //sql.Append("insert into public.set_menu(");
                //sql.Append("menu_code, menu_desc, modified");
                //sql.AppendLine(")");
                //sql.Append("values (");
                //sql.Append("@menu_code,");
                //sql.Append("@menu_desc,");
                //sql.Append("current_timestamp");
                //sql.AppendLine(")");
                //sql.AppendLine("on conflict (menu_code)");
                //sql.AppendLine("do ");
                //sql.AppendLine(" update");
                //sql.AppendLine("     Set menu_desc= @menu_descup");
                //sql.AppendLine("     , modified = current_timestamp");
                //sql.AppendLine(";");



                using var cmd = new SqlCommand(connection: con, cmdText: null);


                string sParammenu_code = "@menu_code";
                string sParammenu_desc = "@menu_desc";
                //string sParammenu_descup = "@menu_descup";

                cmd.Parameters.AddWithValue(sParammenu_code, mnuName);
                cmd.Parameters.AddWithValue(sParammenu_desc, mundesc);
                //cmd.Parameters.AddWithValue(sParammenu_descup, mundesc);

                //cmd.Parameters.Add(new SqlParameter<string>(sParammenu_code, mnuName));
                //cmd.Parameters.Add(new NpgsqlParameter<string>(sParammenu_desc, mundesc));
                //cmd.Parameters.Add(new NpgsqlParameter<string>(sParammenu_descup, mundesc));

                con.Open();
                cmd.CommandText = sql.ToString();
                await cmd.ExecuteNonQueryAsync();
                listRet.Bret = true;
                listRet.Iret = 1;
                listRet.Sret = "OK";

            }
            catch (NpgsqlException ex)
            {
                Log.Error(ex.ToString());
                listRet.Bret = false;
                listRet.Iret = 0;
                listRet.Sret = "Error :" + ex.ToString();
            }
            finally
            {
                con.Close();
            }

            return listRet;
        }

        public async Task<ResultReturn> UpsertRegister(long ugid, string usid, string uspass, string usfirstname)
        {

            ResultReturn listRet = new ResultReturn
            {
                Bret = false,
                Iret = 0,
                Sret = "Error:Intarial"
            };

            using SqlConnection con = new SqlConnection(connectionString);
            try
            {
                StringBuilder sql = new StringBuilder();

                //WhiteSmoke

                sql.AppendLine("UPDATE dbo.set_users");
                sql.AppendLine("SET usfirstname = @usfirstname");
                sql.AppendLine(", usgridcolor = @usgridcolor");
                sql.AppendLine(", ugid = @ugid");
                sql.AppendLine(", uspass = @uspass");
                sql.AppendLine("WHERE usid = @usid");
                sql.AppendLine(";");
                sql.AppendLine("insert into dbo.set_users");
                sql.AppendLine("(ugid, usid, uspass, usfirstname, uslastname, usgridcolor)");
                sql.AppendLine("SELECT ");
                sql.AppendLine("@ugidins, @usidins, @uspassins, @usfirstnameins, @uslastnameins, @usgridcolorins");
                sql.AppendLine("WHERE NOT EXISTS (SELECT 1 FROM dbo.set_users WHERE usid = @usidinsw)");
                sql.AppendLine(";");

                using var cmd = new SqlCommand(connection: con, cmdText: null);

                string pusfirstname = "@usfirstname";
                string pusgridcolor = "@usgridcolor";
                string pugid = "@ugid";
                string puspass = "@uspass";
                string pusid = "@usid";
                string pugidins = "@ugidins";
                string pusidins = "@usidins";
                string puspassins = "@uspassins";
                string pusfirstnameins = "@usfirstnameins";
                string puslastnameins = "@uslastnameins";
                string pusgridcolorins = "@usgridcolorins";
                string pusidinsw = "@usidinsw";


                cmd.Parameters.AddWithValue(pusfirstname, usfirstname);
                cmd.Parameters.AddWithValue(pusgridcolor, "WhiteSmoke");
                cmd.Parameters.AddWithValue(pugid, ugid);
                cmd.Parameters.AddWithValue(puspass, uspass);
                cmd.Parameters.AddWithValue(pusid, usid);
                cmd.Parameters.AddWithValue(pugidins, ugid);
                cmd.Parameters.AddWithValue(pusidins, usid);
                cmd.Parameters.AddWithValue(puspassins, uspass);
                cmd.Parameters.AddWithValue(pusfirstnameins, usfirstname);
                cmd.Parameters.AddWithValue(puslastnameins, usfirstname);
                cmd.Parameters.AddWithValue(pusgridcolorins, "WhiteSmoke");
                cmd.Parameters.AddWithValue(pusidinsw, usid);


                //cmd.Parameters.Add(new SqlParameter<string>(pusfirstname, usfirstname));
                //cmd.Parameters.Add(new SqlParameter<string>(pusgridcolor, "WhiteSmoke"));
                //cmd.Parameters.Add(new SqlParameter<long>(pugid, ugid));
                //cmd.Parameters.Add(new SqlParameter<string>(puspass, uspass));
                //cmd.Parameters.Add(new SqlParameter<string>(pusid, usid));
                //cmd.Parameters.Add(new SqlParameter<long>(pugidins, ugid));
                //cmd.Parameters.Add(new SqlParameter<string>(pusidins, usid));
                //cmd.Parameters.Add(new SqlParameter<string>(puspassins, uspass));
                //cmd.Parameters.Add(new SqlParameter<string>(pusfirstnameins, usfirstname));
                //cmd.Parameters.Add(new SqlParameter<string>(puslastnameins, usfirstname));
                //cmd.Parameters.Add(new SqlParameter<string>(pusgridcolorins, "WhiteSmoke"));
                //cmd.Parameters.Add(new SqlParameter<string>(pusidinsw, usid));

                con.Open();
                cmd.CommandText = sql.ToString();
                await cmd.ExecuteNonQueryAsync();
                listRet.Bret = true;
                listRet.Iret = 1;
                listRet.Sret = "OK";

            }
            catch (SqlException ex)
            {
                Log.Error(ex.ToString());
                listRet.Bret = false;
                listRet.Iret = 0;
                listRet.Sret = "Error :" + ex.ToString();
            }
            finally
            {
                con.Close();
            }

            return listRet;
        }

        public bool SetPrivilege(long idx, bool acc, bool add, bool edi, bool del, bool rpt, bool apv)
        {
            bool bRet = false;


            using SqlConnection con = new SqlConnection(connectionString);
            try
            {

                StringBuilder sql = new StringBuilder();

                using var cmd = new SqlCommand(connection: con, cmdText: null);
                sql.AppendLine("UPDATE dbo.set_privilege");
                sql.AppendLine("SET role_acc = @role_acc");
                sql.AppendLine(", role_add = @role_add");
                sql.AppendLine(", role_edit = @role_edit");
                sql.AppendLine(", role_del = @role_del");
                sql.AppendLine(", role_rpt = @role_rpt");
                sql.AppendLine(", role_apv = @role_apv");
                sql.AppendLine("WHERE idx =  @idx");
                sql.AppendLine(";");

                cmd.Parameters.AddWithValue("@idx", idx);
                cmd.Parameters.AddWithValue("@role_acc", acc);
                cmd.Parameters.AddWithValue("@role_add", add);
                cmd.Parameters.AddWithValue("@role_edit", edi);
                cmd.Parameters.AddWithValue("@role_del", del);
                cmd.Parameters.AddWithValue("@role_rpt", rpt);
                cmd.Parameters.AddWithValue("@role_apv", apv);

                con.Open();
                cmd.CommandText = sql.ToString();
                cmd.ExecuteNonQuery();

                bRet = true;

            }
            catch (SqlException ex)
            {
                Log.Error(ex.ToString());
                bRet = false;
            }
            finally
            {
                con.Close();
            }


            return bRet;
        }

    }
}
