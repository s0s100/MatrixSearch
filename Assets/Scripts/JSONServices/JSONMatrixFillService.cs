using UnityEngine;

public class JSONMatrixFillService
{
    public MatrixData GetMatricesFromJSON(string json)
    {
        string wrappedJson = "{ \"matrices\": " + json + " }";
        return JsonUtility.FromJson<MatrixData>(wrappedJson);
    }
}