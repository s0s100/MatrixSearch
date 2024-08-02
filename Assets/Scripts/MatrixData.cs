using System;
using System.Collections.Generic;
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

    public Matrix4x4Data(Matrix4x4 matrix)
    {
        m00 = matrix.m00;
        m01 = matrix.m01;
        m02 = matrix.m02;
        m03 = matrix.m03;
        m10 = matrix.m10;
        m11 = matrix.m11;
        m12 = matrix.m12;
        m13 = matrix.m13;
        m20 = matrix.m20;
        m21 = matrix.m21;
        m22 = matrix.m22;
        m23 = matrix.m23;
        m30 = matrix.m30;
        m31 = matrix.m31;
        m32 = matrix.m32;
        m33 = matrix.m33;
    }

    public static Matrix4x4 RoundMatrix(Matrix4x4 matrix, int decimalPlaces)
    {
        Matrix4x4 roundedMatrix = new();
        roundedMatrix.m00 = RoundToDecimalPlaces(matrix.m00, decimalPlaces);
        roundedMatrix.m01 = RoundToDecimalPlaces(matrix.m01, decimalPlaces);
        roundedMatrix.m02 = RoundToDecimalPlaces(matrix.m02, decimalPlaces);
        roundedMatrix.m03 = RoundToDecimalPlaces(matrix.m03, decimalPlaces);
        roundedMatrix.m10 = RoundToDecimalPlaces(matrix.m10, decimalPlaces);
        roundedMatrix.m11 = RoundToDecimalPlaces(matrix.m11, decimalPlaces);
        roundedMatrix.m12 = RoundToDecimalPlaces(matrix.m12, decimalPlaces);
        roundedMatrix.m13 = RoundToDecimalPlaces(matrix.m13, decimalPlaces);
        roundedMatrix.m20 = RoundToDecimalPlaces(matrix.m20, decimalPlaces);
        roundedMatrix.m21 = RoundToDecimalPlaces(matrix.m21, decimalPlaces);
        roundedMatrix.m22 = RoundToDecimalPlaces(matrix.m22, decimalPlaces);
        roundedMatrix.m23 = RoundToDecimalPlaces(matrix.m23, decimalPlaces);
        roundedMatrix.m30 = RoundToDecimalPlaces(matrix.m30, decimalPlaces);
        roundedMatrix.m31 = RoundToDecimalPlaces(matrix.m31, decimalPlaces);
        roundedMatrix.m32 = RoundToDecimalPlaces(matrix.m32, decimalPlaces);
        roundedMatrix.m33 = RoundToDecimalPlaces(matrix.m33, decimalPlaces);

        return roundedMatrix;
    }

    private static float RoundToDecimalPlaces(float value, int decimalPlaces)
    {
        float scale = Mathf.Pow(10, decimalPlaces);
        return Mathf.Round(value * scale) / scale;
    }
}

[Serializable]
public class MatrixData
{
    [SerializeField] public Matrix4x4Data[] matrices;

    public MatrixData(List<Matrix4x4Data> matrixList) 
    {
        matrices = new Matrix4x4Data[matrixList.Count];
        for (int i = 0; i < matrices.Length; i++) 
        {
            matrices[i] = matrixList[i];
        }
    }

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

    public bool ContainsRequiredMatrix(Matrix4x4 compareMatrix, int decimalLimit)
    {
        compareMatrix = Matrix4x4Data.RoundMatrix(compareMatrix, decimalLimit);
        foreach (var mat in matrices)
        {
            var matrix = mat.ToMatrix4x4();
            matrix = Matrix4x4Data.RoundMatrix(matrix, decimalLimit);
            if (matrix.Equals(compareMatrix))
                return true;
        }

        return false;
    }
}