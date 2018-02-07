using System;
using System.IO;

namespace RotationAlterBase
{
    class FileCopy
    {
        static public void Copy(string sourcePath, string destinationPath)
        {
            DirectoryInfo source = new DirectoryInfo(sourcePath);
            DirectoryInfo dest = new DirectoryInfo(destinationPath);
            if (!System.IO.Directory.Exists(dest.ToString()))
            {
                System.IO.Directory.CreateDirectory(dest.ToString());
            }
            try
            {
                // Recursively call 
                foreach (DirectoryInfo dir in source.GetDirectories())
                    Copy(dir.FullName, dest.CreateSubdirectory(dir.Name).FullName);

                // Go ahead and copy each file in "source" to the "target" directory
                foreach (FileInfo file in source.GetFiles())
                    file.MoveTo(Path.Combine(dest.FullName, file.Name));
            }
            catch(Exception ex)
            {
                WriteLog.Write(ex.ToString()); 
            }
        }

        internal static void Delete(string v)
        {
            if (System.IO.Directory.Exists(v.ToString()))
            {
                Directory.Delete(v, true);
                WriteLog.Write("Directory " + v + " has been deleted");
            }
            else
            {
                WriteLog.Write("Directory " + v + " not exist");
            }
        }
    }
}
