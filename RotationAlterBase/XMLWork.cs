using System;
using System.Xml;

namespace RotationAlterBase
{
    static class XMLWork
    {
        static public void XMLFix(string pathToXML, string idDb, string str)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(pathToXML);
            XmlNode node = doc.DocumentElement.SelectSingleNode("indexes");
            XmlNodeList lst = node.ChildNodes;
            foreach(XmlNode i in lst)
            {
               if( i.Attributes["name"].Value == idDb)
                {
                    i.Attributes["path"].Value = str;
                    break;
                }
            }
            doc.Save(pathToXML);
        }

        internal static string GetPath(string pathToXML, string idDb)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(pathToXML);
            XmlNode node = doc.DocumentElement.SelectSingleNode("indexes");
            XmlNodeList lst = node.ChildNodes;
            foreach (XmlNode i in lst)
            {
                if (i.Attributes["name"].Value == idDb)
                {
                    return i.Attributes["path"].Value;
                }
            }
            return null;
        }

        internal static string GetPath(int v)
        {
            throw new NotImplementedException();
        }
    }
}
