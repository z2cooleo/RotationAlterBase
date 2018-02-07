namespace RotationAlterBase
{
    class YamlWork
    {
        internal static void YamlFix(string fgStConfigYaml, string currDbID, string fastTableSpacePath, string slowTableSpacePath)
        {
            var lines = System.IO.File.ReadAllLines(fgStConfigYaml);
            for(int i=0; i<lines.Length;i++)
            { 
                string str = "id: " + currDbID;
                if (lines[i].Trim() == str.Trim())
                {
                    for (int k = i; i < lines.Length; k++)
                    {
                        if (lines[k].Contains("index_path:"))
                        {
                            lines[k] = lines[k].Replace(slowTableSpacePath, fastTableSpacePath);
                            System.IO.File.WriteAllLines(fgStConfigYaml, lines);
                            break;
                        }
                    }
                    break;
                }
            }
                /*var deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());
                Dictionary<object, object> yamlObject = (Dictionary<object, object>)deserializer.Deserialize(reader);
                Dictionary<object, object> r = (Dictionary<object,object>)yamlObject["data"];
                List<object> odd = (List<object>)r["databases"];
                Dictionary<object, object> tmp = (Dictionary<object, object>)odd[0];
                List<object> t = (List<object>)tmp["databases"];
                for(int i = 0; i<t.Count;i++)
                {
                    Dictionary<object, object> z = (Dictionary<object, object>)t[i];
                    if (z["id"].ToString() == currDbID)
                    {
                        z["index_path"] = fastTableSpacePath;
                        break;
                    }
                    else continue;
                }*/
            }
    }
}
