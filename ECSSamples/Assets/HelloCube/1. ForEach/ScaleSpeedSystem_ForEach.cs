using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

// 이 시스템은 장면의 모든 요소를 ScaleSpeedSystem_ForEach 및 Rotation 구성 요소로 모두 업데이트합니다.

// ReSharper disable once InconsistentNaming
public class ScaleSpeedSystem_ForEach : ComponentSystem
{
    protected override void OnUpdate()
    {
        // Entities.ForEach는 기본 스레드에서 각 ComponentData 세트를 처리합니다. 권장하지 않습니다
        // 최상의 성능을위한 방법. 그러나 여기서는 더 명확한 분리를 보여주기 위해 여기에서 시작합니다.
        // ComponentSystem 업데이트 (logic)와 ComponentData (데이터) 간.
        // 개별 ComponentData에는 업데이트 로직이 없습니다.
        Entities.ForEach((ref ScaleSpeed_ForEach scaleSpeed, ref NonUniformScale uniformScale) =>
        {
            var deltaTime = Time.deltaTime;

            var scale = uniformScale.Value;
            scale.x += scaleSpeed.ScalePerSecond * deltaTime;
            scale.y += scaleSpeed.ScalePerSecond * deltaTime;
            scale.z += scaleSpeed.ScalePerSecond * deltaTime;
            uniformScale.Value = scale;
        });
    }
}