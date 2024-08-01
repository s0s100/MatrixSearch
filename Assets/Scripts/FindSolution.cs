using UnityEngine;
using Zenject;

public class FindSolution : MonoBehaviour
{
    [Inject] private readonly JSONReaderService jsonReaderService;
    [Inject] private readonly JSONMatrixFillService matrixFillService;
    [Inject] private readonly ConfigProvider configProvider;

    private MatrixData modelData;
    private MatrixData spaceData;

    private void Start()
    {
        ReadJSONData();
        var result = Solve();
        GenerateJSONData(result);
    }

    private void ReadJSONData()
    {
        var modelText = jsonReaderService.ReadJSON(configProvider.ModelsPath);
        var spaceText = jsonReaderService.ReadJSON(configProvider.SpacePath);

        modelData = matrixFillService.GetMatricesFromJSON(modelText);
        spaceData = matrixFillService.GetMatricesFromJSON(spaceText);

        modelData.VisualizeOnSceneAsModel(configProvider);
        spaceData.VisualizeOnSceneAsSpace(configProvider);
    }

    // Отталкиваясь от первого элемента массива буду искать
    private MatrixData Solve()
    {
        var firstModelMatrix = modelData.matrices[0].ToMatrix4x4();
        var firstModelMatrixI = firstModelMatrix.inverse;
        //for (int i = 0; i < spaceData.matrices.Length; i++)
        //{
        //    var curSpaceMatrix = spaceData.matrices[i].ToMatrix4x4();
        //    var transformationMatrix = curSpaceMatrix * firstModelMatrixI;
        //    var modelMatrix = configProvider.ModelParentTransform.localToWorldMatrix;
        //    var resultMatrix = modelMatrix * transformationMatrix;
        //    UpdateTransform(configProvider.ModelParentTransform,
        //        resultMatrix);
        //}

        var curSpaceMatrix = spaceData.matrices[0].ToMatrix4x4();
        var transformationMatrix = curSpaceMatrix * firstModelMatrixI;
        var modelMatrix = configProvider.ModelParentTransform.localToWorldMatrix;
        var resultMatrix = modelMatrix * transformationMatrix;
        UpdateTransform(configProvider.ModelParentTransform,
            resultMatrix);

        return modelData;
    }

    private void UpdateTransform(Transform targetTransform, Matrix4x4 matrix)
    {
        Vector3 position = matrix.GetColumn(3);

        Vector3 scale = new Vector3(
            new Vector4(matrix.m00, matrix.m10, matrix.m20, matrix.m30).magnitude,
            new Vector4(matrix.m01, matrix.m11, matrix.m21, matrix.m31).magnitude,
            new Vector4(matrix.m02, matrix.m12, matrix.m22, matrix.m32).magnitude
        );

        Quaternion rotation = Quaternion.LookRotation(
            new Vector3(matrix.m02, matrix.m12, matrix.m22),
            new Vector3(matrix.m01, matrix.m11, matrix.m21)
        );

        targetTransform.SetPositionAndRotation(position, rotation);
        targetTransform.localScale = scale;
    }

    private void GenerateJSONData(MatrixData matrixData)
    {
        matrixFillService.GetJSONFromMatrices(matrixData);
    }
}