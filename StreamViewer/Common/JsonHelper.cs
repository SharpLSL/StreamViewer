using System.IO;

using Newtonsoft.Json;

namespace StreamViewer.Common;

public static class JsonHelper
{
    public static void SerializeToFile<T>(T obj, string filePath)
    {
        var jsonContent = JsonConvert.SerializeObject(obj, Formatting.Indented);
        File.WriteAllText(filePath, jsonContent);
    }

    public static T? DeserializeFromFile<T>(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return default;
        }

        var jsonContent = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<T>(jsonContent);
    }

    public static T DeepClone<T>(T obj)
    {
        var jsonContent = JsonConvert.SerializeObject(obj);
        return JsonConvert.DeserializeObject<T>(jsonContent)!;
    }
}
