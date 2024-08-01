using System;
using UnityEngine;

[Serializable]
public struct Matrix4x4Data
{
    public float m00, m01, m02, m03;
    public float m10, m11, m12, m13;
    public float m20, m21, m22, m23;
    public float m30, m31, m32, m33;
}

[Serializable]
public struct MatrixData
{
    [SerializeField] public Matrix4x4Data[] matrices;
}