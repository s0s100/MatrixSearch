using UnityEngine;

public class JSONReaderService
{
    public string ReadJSON(string path)
    {
        var filePath = path.Replace(".json", "");
        var targetFile = Resources.Load<TextAsset>(filePath);
        return targetFile.text;
    }
}