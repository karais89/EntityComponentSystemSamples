using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[RequiresEntityConversion]
public class ScaleSpeedAuthoring_IJobChunk : MonoBehaviour, IConvertGameObjectToEntity
{
    public float ScalePerSecond = 1;
    
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var data = new ScaleSpeed_IJobChunk() {ScalePerSecond = ScalePerSecond};
        dstManager.AddComponentData(entity, new NonUniformScale { Value = new float3(1.0f, 1.0f, 1.0f)});
        dstManager.AddComponentData(entity, data);
    }
}
