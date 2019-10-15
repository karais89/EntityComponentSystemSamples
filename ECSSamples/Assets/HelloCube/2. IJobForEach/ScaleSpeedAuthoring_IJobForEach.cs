using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[RequiresEntityConversion]
public class ScaleSpeedAuthoring_IJobForEach : MonoBehaviour, IConvertGameObjectToEntity
{
    public float ScalePerSecond = 1;
    
    // MonoBehaviour 데이터는 엔티티에서 ComponentData로 변환됩니다.
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var data = new ScaleSpeed_IJobForEach {ScalePerSecond = ScalePerSecond};
        
        dstManager.AddComponentData(entity, new NonUniformScale { Value = new float3(1.0f, 1.0f, 1.0f)});
        dstManager.AddComponentData(entity, data);
    }
}
