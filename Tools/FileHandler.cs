using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Tools
{
    public class FileHandler
    {
        public static void WriteToFile(string fileLocation, JsonSerializedDataObject o)
        {
            using (TextWriter file = new StreamWriter(fileLocation))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, o);
            }
        }
        public static JsonSerializedDataObject ReadFromFile(string fileLocation)
        {
            try
            {
                using (StreamReader file = File.OpenText(fileLocation))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    var obj = (JsonSerializedDataObject)serializer.Deserialize(file, typeof(JsonSerializedDataObject));
                    return obj.Packages == null ? null : obj;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
