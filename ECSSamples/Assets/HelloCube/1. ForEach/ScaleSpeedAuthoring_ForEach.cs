using Unity.Entities;
using UnityEngine;

// ReSharper disable once InconsistentNaming
[RequiresEntityConversion]
public class ScaleSpeedAuthoring_ForEach : MonoBehaviour, IConvertGameObjectToEntity
{
    public float ScalePerSecond;

    // 모노비헤비어 데이터는 엔티티의 컴포넌트 데이터로 변형된다.
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var data = new ScaleSpeed_ForEach { ScalePerSecond = ScalePerSecond };
        dstManager.AddComponentData(entity, data);
    }
}