using System;
using Npgsql;

namespace RotationAlterBase
{
    static class WorkDB
    {
        static public void ChangeTablespace(string host, string port, string user, string pass, string db, string tablespace)
        {
            try
            {
                string str = "Server=" + host + ";port=" + port + ";User Id=" + user + ";Password=" + pass + ";Database=postgres;Timeout=300;CommandTimeout=3000";
            NpgsqlConnection conn = new NpgsqlConnection(str);
            conn.Open();
            // Define a query returning a single row result set
            NpgsqlCommand command = new NpgsqlCommand("ALTER DATABASE \""+db+"\" SET TABLESPACE \""+tablespace+"\"", conn);
            // Execute the query and obtain the value of the first column of the first row
            Int64 count = (Int64)command.ExecuteNonQuery();
            conn.Close();
            WriteLog.Write("База данных была перенесена в " + tablespace + " tablespace");
            }
            catch(Exception ex)
            {
                WriteLog.Write(ex.ToString());
            }
        }
        static public void CloseConnection(string host, string port, string user, string pass, string db)
        {
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection("Server=" + host + ";port=" + port + ";User Id=" + user + ";Password=" + pass + ";Database=" + db + ";"); 
                conn.Open();
                // Define a query returning a single row result set
                NpgsqlCommand command = new NpgsqlCommand("SELECT pg_terminate_backend(pg_stat_activity.pid) FROM pg_stat_activity", conn);
                // Execute the query and obtain the value of the first column of the first row
                Int64 count = (Int64)command.ExecuteNonQuery();
                WriteLog.Write("Все соединения с базой данных были сброшенны.");
            }
            catch
            {
                WriteLog.Write("Все соединения с базой данных были сброшенны.");
            }

        }

        static public string GetTablespaceName(string host, string port, string user, string pass, string db)
        {
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection("Server=" + host + ";port=" + port + ";User Id=" + user + ";Password=" + pass + ";Database=" + db + ";");
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
