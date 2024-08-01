using System;
using UnityEngine;

[Serializable]
public struct Matrix4x4Data
{
    public float m00, m01, m02, m03;
    public float m10, m11, m12, m13;
    public float m20, m21, m22, m23;
    public float m30, m31, m32, m33;

    public Matrix4x4 ToMatrix4x4()
    {
        return new Matrix4x4(
            new Vector4(m00, m10, m20, m30),
            new Vector4(m01, m11, m21, m31),
            new Vector4(m02, m12, m22, m32),
            new Vector4(m03, m13, m23, m33)
        );
    }
}

[Serializable]
public class MatrixData
{

    [SerializeField] public Matrix4x4Data[] matrices;

    // Implement intefaces to inherit for model and space matrices
    public void VisualizeOnSceneAsModel(ConfigProvider configProvider)
    {
        foreach (var mat in matrices)
        {
            var matrix = mat.ToMatrix4x4();
            var newObject = GenerateObjectFromMatrix(matrix, configProvider);
            newObject.transform.parent = configProvider.ModelParentTransform;
            newObject.GetComponent<MeshRenderer>().material = configProvider.SelectedMaterial;
        }   
    }

    public void VisualizeOnSceneAsSpace(ConfigProvider configProvider)
    {
        foreach (var mat in matrices)
        {
            var matrix = mat.ToMatrix4x4();
            var newObject = GenerateObjectFromMatrix(matrix, configProvider);
            newObject.transform.parent = configProvider.SpaceParentTransform;
            newObject.GetComponent<MeshRenderer>().material = configProvider.DefaultMaterial;
        }
    }

    private GameObject GenerateObjectFromMatrix(Matrix4x4 matrix, 
        ConfigProvider configProvider)
    {
        var position = matrix.GetColumn(3);
        var rotation = Quaternion.LookRotation(matrix.GetColumn(2), 
            matrix.GetColumn(1));
        var scale = new Vector3(matrix.GetColumn(0).magnitude,
            matrix.GetColumn(1).magnitude, matrix.GetColumn(2).magnitude);

        var newObject = GameObject.Instantiate(configProvider.VisualObject);
        newObject.transform.SetPositionAndRotation(position, rotation);
        newObject.transform.localScale = scale;

        return newObject;
    }
}