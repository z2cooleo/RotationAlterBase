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
            string s = DateTime.Now + "    " + str;
            System.IO.File.AppendAllText(path, s);
            return str;
        }
        static public string Write(string firstStr, string secondStr)
        {
            string str = DateTime.Now + "    " + firstStr + ": " + secondStr + Environment.NewLine;
            System.IO.File.AppendAllText(path, str);
            return secondStr;
        }
    }
}
