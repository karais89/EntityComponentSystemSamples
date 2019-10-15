using System;
using Unity.Entities;

[Serializable]
public struct ScaleSpeed_IJobForEach : IComponentData
{
    public float ScalePerSecond;
}
