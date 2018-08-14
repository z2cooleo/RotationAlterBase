using System;
using Npgsql;

namespace RotationAlterBase
{
    static class WorkDB
    {
        static public void ChangeTablespace(string host, string port, string user, string pass, string maintenanceDB, string db, string tablespace)
        {
            try
            {
                string str = "Server=" + host + ";port=" + port + ";User Id=" + user + ";Password=" + pass + ";Database=" + maintenanceDB + ";Timeout=300;CommandTimeout=3000";
                NpgsqlConnection conn = new NpgsqlConnection(str);
                conn.Open();
                // Define a query returning a single row result set
                NpgsqlCommand command = new NpgsqlCommand("ALTER DATABASE \""+db+"\" SET TABLESPACE \""+tablespace+"\"", conn);
                // Execute the query and obtain the value of the first column of the first row
                Int64 count = (Int64)command.ExecuteNonQuery();
                conn.Close();
                WriteLog.Write("База данных " + db + " была перенесена в " + tablespace + " tablespace");
            }
            catch(Exception ex)
            {
                WriteLog.Write(ex.ToString());
            }
        }
        static public void CloseConnection(string host, string port, string user, string pass, string db, string maintananceDB)
        {
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection("Server=" + host + ";port=" + port + ";User Id=" + user + ";Password=" + pass + ";Database=" + maintananceDB + ";"); 
                conn.Open();
                // Define a query returning a single row result set
                NpgsqlCommand command = new NpgsqlCommand("SELECT pg_terminate_backend(pg_stat_activity.pid) FROM pg_stat_activity WHERE pg_stat_activity.datname = '"+db+"';", conn);
                // Execute the query and obtain the value of the first column of the first row
                NpgsqlDataReader count = command.ExecuteReader();
                conn.Close();
                WriteLog.Write("Все соединения с базой данных "+ db + " были сброшенны.");                
            }
            catch(Exception ex)
            {
                WriteLog.Write(ex.ToString());
            }
            finally
            {
                System.Threading.Thread.Sleep(5000);
            }

        }

        static public void MaintenanceModeDB(string host, string port, string user, string pass, string maintenanceDB, string currDbname, string prevDbname, string maintenanceMode)
        {
            string mMode = maintenanceMode == "TurnOn" ? "false" : "true";
            string mStr = maintenanceMode == "TurnOn" ? "включен" : "выключен";
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection("Server=" + host + ";port=" + port + ";User Id=" + user + ";Password=" + pass + ";Database=" + maintenanceDB + ";");
                conn.Open();

                NpgsqlCommand commandCurrDB = new NpgsqlCommand("ALTER DATABASE \"" + currDbname + "\" WITH ALLOW_CONNECTIONS " + mMode + ";", conn);
                // Execute the query and obtain the value of the first column of the first row
                Int64 count = (Int64)commandCurrDB.ExecuteNonQuery();
                conn.Close();
                conn.Open();
                WriteLog.Write("БД "+ currDbname + " " + mStr + " режим обслуживания");
                NpgsqlCommand commandprevDB = new NpgsqlCommand("ALTER DATABASE \"" + prevDbname + "\" WITH ALLOW_CONNECTIONS " + mMode + ";", conn);
                // Execute the query and obtain the value of the first column of the first row
                count = (Int64)commandprevDB.ExecuteNonQuery();
                WriteLog.Write("БД " + prevDbname + " " + mStr + " режим обслуживания");
                conn.Close();
                WriteLog.Write("Режим обслуживания " + mStr + "");
            }
            catch(Exception ex)
            {
                WriteLog.Write("не удалось включить режим обслуживания");
                WriteLog.Write(ex.ToString());
            }
        }

        static public string GetTablespaceName(string host, string port, string user, string pass, string db, string maintenanceDB)
        {
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection("Server=" + host + ";port=" + port + ";User Id=" + user + ";Password=" + pass + ";Database=" + maintenanceDB + ";");
                string sql = "SELECT d.datname, t.spcname FROM pg_catalog.pg_database d JOIN pg_catalog.pg_tablespace t on d.dattablespace = t.oid WHERE d.datname = '" + db + "'";
                conn.Open();
                // Define a query returning a single row result set
                NpgsqlCommand command = new NpgsqlCommand(sql, conn);
                // Execute the query and obtain the value of the first column of the first row
                NpgsqlDataReader dr = command.ExecuteReader();
                string str = null;
                while (dr.Read())
                {
                    str = dr[1].ToString();
                }
                conn.Close();
                return str;
            }
            catch (Exception ex)
            {
                WriteLog.Write(ex.ToString());
            }
            return "Get Tablespace not successfull";
        }
    }
}
