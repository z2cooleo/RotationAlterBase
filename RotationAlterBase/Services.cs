using System;
using System.ServiceProcess;

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
                    serviceFG.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(120));
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
