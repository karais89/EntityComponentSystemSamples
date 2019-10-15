using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

// ReSharper disable once InconsistentNaming
[RequiresEntityConversion]
public class RotationSpeedAuthoring_ForEach : MonoBehaviour, IConvertGameObjectToEntity
{
    public float DegreesPerSecond;

    // The MonoBehaviour data is converted to ComponentData on the entity.
    // We are specifically transforming from a good editor representation of the data (Represented in degrees)
    // To a good runtime representation (Represented in radians)
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var data = new RotationSpeed_ForEach { RadiansPerSecond = math.radians(DegreesPerSecond) };
        var scale = new NonUniformScale {Value = new float3(1, 1, 1)};
        
        dstManager.AddComponentData(entity, data);
        dstManager.AddComponentData(entity, scale);
    }
}
