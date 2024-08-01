using UnityEngine;

public class JSONMatrixFillService
{
    public string GetJSONFromMatrices(MatrixData matrixData)
    {
        var result = JsonUtility.ToJson(matrixData);
        Debug.Log(result);
        return result;
    }

    public MatrixData GetMatricesFromJSON(string json)
    {
        string wrappedJson = "{ \"matrices\": " + json + " }";
        return JsonUtility.FromJson<MatrixData>(wrappedJson);
    }
}