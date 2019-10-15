using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Samples.HelloCube_1c
{
    // 이 시스템은 장면의 모든 엔티티를 NonUniformScale 컴포넌트로 업데이트합니다.
    // ScaleUp 컴포넌트가 있는지 여부에 따라 엔티티를 다르게 처리합니다.
    public class ScaleSystem_ForEachWithEntityChanges : ComponentSystem
    {
        protected override void OnUpdate()
        {
            // ScaleUp 컴포넌트가 있으면 시스템은 NonUniformScale 컴포넌트를 업데이트하여 엔터티의 크기를 키웁니다.
            // 엔티티가 미리 정해진 크기에 도달하면이 함수는 ScaleUp 컴포넌트를 제거합니다.
            Entities.WithAllReadOnly<ScalingCube_ForEachWithEntityChanges, ScaleUp_ForEachWithEntityChanges>().ForEach(
                (Entity id, ref NonUniformScale uniformScale) =>
                {
                    var deltaTime = Time.deltaTime;
                    uniformScale = new NonUniformScale()
                    {
                        Value = new float3(uniformScale.Value.x + deltaTime, uniformScale.Value.y + deltaTime, uniformScale.Value.z + deltaTime)
                    };
                    
                    if (uniformScale.Value.x > 5.0f && uniformScale.Value.y > 5.0f && uniformScale.Value.z > 5.0f)
                        EntityManager.RemoveComponent<ScaleUp_ForEachWithEntityChanges>(id);
                }
            );

            // 엔터티에 ScaleUp 컴포넌트가 없지만 NonUniformScale 컴포넌트가 있는 경우
            // 그런 다음 시스템은 엔티티를 기본 크기로 되돌리고 ScaleUp 컴포넌트를 추가합니다.
            Entities.WithAllReadOnly<ScalingCube_ForEachWithEntityChanges>().WithNone<ScaleUp_ForEachWithEntityChanges>().ForEach(
                (Entity id, ref NonUniformScale uniformScale) =>
                {
                    uniformScale.Value = new float3(1.0f, 1.0f, 1.0f);

                    EntityManager.AddComponentData(id, new ScaleUp_ForEachWithEntityChanges());
                }
            );
        }
    }
}
