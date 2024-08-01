using UnityEngine;

public class JSONReaderService
{
    public string ReadJSON(string path)
    {
        string filePath = path.Replace(".json", "");
        var targetFile = Resources.Load<TextAsset>(filePath);
        return targetFile.text;
    }
}