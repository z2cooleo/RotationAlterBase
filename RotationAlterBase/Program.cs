using System;
using System.Diagnostics;
using System.Reflection;
using System.Deployment;

namespace RotationAlterBase
{
    class Program
    {
        static string ver = "23";
        static string currDbID;
        static string currDbType;
        static string currDbPort;
        static string currDbHost;
        static string currDbUser;
        static string currDbPass;
        static string currDbname;
        static string prevDbID;
        static string prevDbType;
        static string prevDbPort;
        static string prevDbHost;
        static string prevDbUser;
        static string prevDbPass;
        static string prevDbname;
        static string fastTableSpace;
        static string fastTableSpacePath;
        static string slowTableSpace;
        static string slowTableSpacePath;
        static string fgStConfigXML;
        static string fgStConfigYaml;
        static string maintenanceDB;

        static void Main(string[] args)
        {
            ArgsParser(args);
            if (args.Length == 0) { Console.WriteLine("Bye!!!!"); }
            else if (currDbType == "pgsql" && prevDbType == "pgsql")
            {
                Services.StopService(new string[] {"FgStStorageServer", "FgStSearchSvc" });
                WorkDB.MaintenanceModeDB(currDbHost, currDbPort, currDbUser, currDbPass, maintenanceDB, currDbname, prevDbname, "TurnOn");
                WorkDB.CloseConnection(currDbHost, currDbPort, currDbUser, currDbPass, currDbname, maintenanceDB);
                WorkDB.CloseConnection(currDbHost, currDbPort, currDbUser, currDbPass, prevDbname, maintenanceDB);
                WorkDB.ChangeTablespace(prevDbHost, prevDbPort, prevDbUser, prevDbPass, maintenanceDB, prevDbname, slowTableSpace);
                WorkDB.ChangeTablespace(currDbHost, currDbPort, currDbUser, currDbPass, maintenanceDB, currDbname, fastTableSpace);
                WorkDB.MaintenanceModeDB(currDbHost, currDbPort, currDbUser, currDbPass, maintenanceDB, currDbname, prevDbname, "TurnOff");
                XMLWork.XMLFix(fgStConfigXML, currDbID, fastTableSpacePath);
                XMLWork.XMLFix(fgStConfigXML, prevDbID, slowTableSpacePath);
                YamlWork.YamlFix(fgStConfigYaml, currDbID, fastTableSpacePath, slowTableSpacePath);
                YamlWork.YamlFix(fgStConfigYaml, prevDbID, slowTableSpacePath, fastTableSpacePath);
                FileCopy.Delete(slowTableSpacePath + @"\" + currDbID);
                FileCopy.Copy(fastTableSpacePath + @"\" + prevDbID, slowTableSpacePath + @"\"+prevDbID);
                FileCopy.Delete(fastTableSpacePath + @"\" + prevDbID);
                Services.StartService(new string[] { "FgStStorageServer", "FgStSearchSvc" });
            }
            else
            {
                WriteLog.Write("Скрипт поддерживает только PostgreSQL DB");
            }
        }

        private static void ArgsParser(string[] args)
        {

            foreach (string item in args)
            {
                switch (item.Split('=')[0].ToString())
                {
                    case "logPath": WriteLog.SetPath(GetVar(item), ver); break;
                    case "currDbID": currDbID = WriteLog.Write("currDbId", GetVar(item)); break;
                    case "currdbType": currDbType = WriteLog.Write("currDbType", GetVar(item)); break;
                    case "cPort": currDbPort = WriteLog.Write("currDbPort", GetVar(item)); break;
                    case "port": prevDbPort = WriteLog.Write("prevPort", GetVar(item)); break;
                    case "cHost": currDbHost = WriteLog.Write("currDbHost", GetVar(item)); break;
                    case "host": prevDbHost = WriteLog.Write("prevDbHost", GetVar(item)); break;
                    case "cUser": currDbUser = WriteLog.Write("currDbUser", GetVar(item)); break;
                    case "user": prevDbUser = WriteLog.Write("prevDbUser", GetVar(item)); break;
                    case "cPassword": currDbPass = WriteLog.Write("currDbPass", GetVar(item)); break;
                    case "password": prevDbPass = WriteLog.Write("prevDbPass", GetVar(item)); break;
                    case "cDbName": currDbname = WriteLog.Write("currDbName", GetVar(item)); break;
                    case "dbname": prevDbname = WriteLog.Write("prevDbName", GetVar(item)); break;
                    case "prevDbID": prevDbID = WriteLog.Write("prevDbId", GetVar(item)); break;
                    case "prevDbType": prevDbType = WriteLog.Write("prevDbType", GetVar(item)); break;
                    case "prevDbname": prevDbname = WriteLog.Write("prevDbName", GetVar(item)); break;
                    case "fastTableSpace":
                        if (GetVar(item) == "auto")
                        {
                            string str = WorkDB.GetTablespaceName(prevDbHost, prevDbPort, prevDbUser, prevDbPass, prevDbname, maintenanceDB);
                            WriteLog.Write("fastTableSpacePath", str);
                            fastTableSpace = WriteLog.Write("fastTablespace", str);
                        }
                        else
                        {
                            fastTableSpace = WriteLog.Write("fastTablespace", GetVar(item));
                        }
                        break;
                    case "slowTableSpace":
                        if (GetVar(item) == "auto")
                        {
                            string str = WorkDB.GetTablespaceName(currDbHost, currDbPort, currDbUser, currDbPass, currDbname, maintenanceDB);
                            WriteLog.Write("fastTableSpacePath", str);
                            slowTableSpace = WriteLog.Write("fastTablespace", str);
                        }
                        else
                        {
                            slowTableSpace = WriteLog.Write("fastTablespace", GetVar(item));
                        }
                        break;
                    case "fastTableSpacePath":
                        fastTableSpacePath = GetVar(item) == "auto" ? WriteLog.Write("fastTableSpacePath", XMLWork.GetPath(fgStConfigXML, prevDbID)) : item;  break;
                    case "slowTableSpacePath":
                        slowTableSpacePath = GetVar(item) == "auto" ? WriteLog.Write("slowTableSpacePath", XMLWork.GetPath(fgStConfigXML, currDbID)) : item; break;
                    case "fgStConfigXML": fgStConfigXML = WriteLog.Write("XMLconfigPath", GetVar(item)); break;
                    case "fgStConfigYaml": fgStConfigYaml = WriteLog.Write("YamlConfigPath", GetVar(item)); break;
                    case "maintenanceDB": maintenanceDB = WriteLog.Write("maintenanceDB", GetVar(item)); break;
                }
            }
        }

        private static string GetVar(string item)
        {
            item = item.Split('=')[1].ToString().Replace("cHost", "host");
            item = item.Replace('?', ' ');
            return item;
        }
    }
}

