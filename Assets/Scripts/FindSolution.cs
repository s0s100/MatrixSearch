using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FindSolution : MonoBehaviour
{
    [Inject] private readonly JSONReaderService jsonReaderService;
    [Inject] private readonly JSONMatrixFillService matrixFillService;
    [Inject] private readonly ConfigProvider configProvider;

    private MatrixData _modelData;
    private MatrixData _spaceData;

    private void Start()
    {
        ReadAndVisualizeData();
        StartCoroutine(UpdateEverySecond(returnValue =>
        {
            GenerateData(returnValue);
        }));
    }

    private void ReadAndVisualizeData()
    {
        var modelText = jsonReaderService.ReadJSON(configProvider.ModelsPath);
        var spaceText = jsonReaderService.ReadJSON(configProvider.SpacePath);

        _modelData = matrixFillService.GetMatricesFromJSON(modelText);
        _spaceData = matrixFillService.GetMatricesFromJSON(spaceText);

        _modelData.VisualizeOnSceneAsModel(configProvider);
        _spaceData.VisualizeOnSceneAsSpace(configProvider);
    }

    private IEnumerator UpdateEverySecond(System.Action<MatrixData> callback = null)
    {
        List<Matrix4x4Data> resultMatrices = new();
        var firstModelMatrix = _modelData.matrices[0].ToMatrix4x4();
        var firstModelMatrixI = firstModelMatrix.inverse;

        for (int i = 0; i < _spaceData.matrices.Length; i++)
        {
            var curSpaceMatrix = _spaceData.matrices[i].ToMatrix4x4();
            var transformMatrix = curSpaceMatrix * firstModelMatrixI;
            UpdateTransform(configProvider.ModelParentTransform,
                transformMatrix);

            for (int j = 0; j < _modelData.matrices.Length; j++)
            {
                var curModelMatrix = _modelData.matrices[j].ToMatrix4x4();
                var multiplicationMatrix = transformMatrix * curModelMatrix;
                var isEqual = _spaceData.ContainsRequiredMatrix(multiplicationMatrix,
                    configProvider.DecimalLimit);
                if (!isEqual)
                    break;

                if (j == _modelData.matrices.Length - 1)
                {
                    Debug.Log("Solution found!");
                    resultMatrices.Add(new Matrix4x4Data(multiplicationMatrix));
                }
            }
            yield return new WaitForSeconds(0);
        }

        MatrixData result = new(resultMatrices);
        callback.Invoke(result);
    }

    private void UpdateTransform(Transform targetTransform, Matrix4x4 matrix)
    {
        Vector3 position = matrix.GetColumn(3);
        Quaternion rotation = Quaternion.LookRotation(
            new Vector3(matrix.m02, matrix.m12, matrix.m22),
            new Vector3(matrix.m01, matrix.m11, matrix.m21)
        );
        targetTransform.SetPositionAndRotation(position, rotation);
    }

    private void GenerateData(MatrixData matrixData)
    {
        matrixFillService.GetJSONFromMatrices(matrixData);
    }
}