using System;
using Unity.VisualScripting;
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
public struct MatrixData
{
    [SerializeField] public Matrix4x4Data[] matrices;

    public void VisualizeOnScene(GameObject visualizationObject, Transform parent)
    {
        foreach (var mat in matrices)
        {
            var matrix = mat.ToMatrix4x4();
            var newObject = GenerateObjectFromMatrix(matrix, visualizationObject, parent);
        }   
    }

    private GameObject GenerateObjectFromMatrix(Matrix4x4 matrix, 
        GameObject visualizationObject, Transform parent)
    {
        var position = matrix.GetColumn(3);
        var rotation = Quaternion.LookRotation(matrix.GetColumn(2), matrix.GetColumn(1));
        var scale = new Vector3(matrix.GetColumn(0).magnitude,
            matrix.GetColumn(1).magnitude, matrix.GetColumn(2).magnitude);
        //newTransform.SetPositionAndRotation(position, rotation);
        //newTransform.localScale = scale;

        var newObject = GameObject.Instantiate(visualizationObject);
        newObject.transform.SetPositionAndRotation(position, rotation);
        newObject.transform.localScale = scale;
        newObject.transform.parent = parent;

        return newObject;
    }
}