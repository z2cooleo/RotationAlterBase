using System;
using System.IO;

namespace RotationAlterBase
{
    static class WriteLog
    {
        static private string path = @"C:\ProgramData\Falcongaze SecureTower\Logs";
        static public void SetPath(string str, string ver)
        {
            DateTime localDate = DateTime.Now;
            path = Path.Combine(str, "log" + localDate.ToString("yyyyMMdd_HHmmss") + ".txt");
            Write(ver);
        }
        static public string Write(string str)
        {
            System.IO.File.AppendAllText(path, str + Environment.NewLine);
            return str;
        }
        static public string Write(string firstStr, string secondStr)
        {
            System.IO.File.AppendAllText(path, firstStr+": "+ secondStr + Environment.NewLine);
            return secondStr;
        }
    }
}
