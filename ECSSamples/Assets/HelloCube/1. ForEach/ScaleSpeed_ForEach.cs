using System;
using Unity.Entities;

// Serializable attribute is for editor support.
// ReSharper disable once InconsistentNaming
[Serializable]
public struct ScaleSpeed_ForEach : IComponentData
{
    public float ScalePerSecond;
}