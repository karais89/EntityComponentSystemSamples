using System;
using Unity.Entities;

[Serializable]
public struct ScaleSpeed_IJobChunk : IComponentData
{
    public float ScalePerSecond;
}
