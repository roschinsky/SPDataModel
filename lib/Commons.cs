using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace TRoschinsky.SPDataModel.Lib
{
    public class Commons
    {
        public static Dictionary<string, string> LoadCustomResources(string name, CultureInfo cultureToUse)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            try 
            {
                string path = System.Reflection.Assembly.GetAssembly(typeof(Commons)).Location;
                path = path.Substring(0, path.LastIndexOf('\\'));

                string customResourcePath = String.Format("{0}\\Resources\\{1}.{2}.reson", path, name, cultureToUse.IetfLanguageTag);

                if(!File.Exists(customResourcePath))
                {
                    customResourcePath = String.Format("{0}\\Resources\\{1}.{2}.reson", path, name, cultureToUse.IetfLanguageTag);
                }

                if(!File.Exists(customResourcePath))
                {
                    customResourcePath = String.Format("{0}\\Resources\\{1}.reson", path, name);
                }

                var customResourceJsonString = File.ReadAllText(customResourcePath);
                var customResourceJsonObject = JsonDocument.Parse(customResourceJsonString);
                foreach(var sub in customResourceJsonObject.RootElement.EnumerateObject())
                {
                    result.Add(sub.Name, sub.Value.ToString());
                }
            }
            catch (System.Exception ex)
            {
                result.Add("_ERROR", ex.Message);
            }
            return result;
        }
    }

    
}