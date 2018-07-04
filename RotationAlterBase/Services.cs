using System;
using System.ServiceProcess;
using System.Diagnostics;

namespace RotationAlterBase
{
    static class Services
    {
        static public string StopService(string serviceName)
        {
            Stop(serviceName);
            return "Service " + serviceName.ToString() + " disabled cuccess!";
        }
        
        static public string StopService(string[] serviceName)
        {
            foreach (string item in serviceName)
            {
                Stop(item);
            }
            return "Service " + serviceName + " disabled cuccess!";            
        }
        static public string StartService(string serviceName)
        {
            Start(serviceName);
            return "Service " + serviceName.ToString() + " disabled cuccess!";
        }
        static public string StartService(string[] serviceName)
        {
            foreach (string item in serviceName)
            {
                Start(item);
            }
            return "Service " + serviceName + " disabled cuccess!";
        }

        private static void Start(string item)
        {
            try
            {
                ServiceController serviceFG = new ServiceController(item);
                if (!serviceFG.ServiceHandle.IsInvalid)
                {
                    serviceFG.Start();
                }
                WriteLog.Write("Service " + item + " enabled cuccess!");
            }
            catch(Exception ex)
            {
                WriteLog.Write("Service " + item + " not started");
                WriteLog.Write(ex.ToString());
            }
        }

        private static void Stop(string serviceName)
        {
            try
            {
                ServiceController serviceFG = new ServiceController(serviceName);
                if (!serviceFG.ServiceHandle.IsInvalid && serviceFG.Status == ServiceControllerStatus.Running)
                {
                    serviceFG.Stop();
                    try
                    {
                        serviceFG.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(120));
                    }
                    catch
                    {
                        if (serviceFG.Status != ServiceControllerStatus.Stopped)
                        {
                            Process[] processlist = Process.GetProcesses();
                            foreach (Process theprocess in processlist)
                            {
                                if (theprocess.ProcessName == "FgIndexerProc")
                                {
                                    try
                                    {
                                        theprocess.Kill();
                                        WriteLog.Write("Process is stuck but was kill successful");
                                    }
                                    catch (Exception ex)
                                    {
                                        WriteLog.Write(ex.ToString());
                                    }
                                }
                            }
                        }
                    }
                }
                WriteLog.Write("Service " + serviceName + " disabled cuccess!");
            }
            catch (Exception ex)
            {
                WriteLog.Write("Service " + serviceName + " not disabled");
                WriteLog.Write(ex.ToString());
            }
        }
    }
}
