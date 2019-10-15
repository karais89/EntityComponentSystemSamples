using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

// 이 시스템은 장면의 모든 엔티티를 ScaleSpeedSystem_IJobForEach 및 NonUniformScale 컴포넌트로 업데이트합니다.

public class ScaleSpeedSystem_IJobForEach : JobComponentSystem
{
    // 버스트를 사용하여 작업을 컴파일하려면 [BurstCompile] 속성을 사용하십시오. 상당한 속도 향상을 볼 수 있으므로 시도하십시오!
    struct ScaleSpeedJob : IJobForEach<NonUniformScale, ScaleSpeed_IJobForEach>
    {
        public float DeltaTime;

        // [ReadOnly] 속성은이 작업이 sclSpeedIJobForEach 쓰지 않을 것이라고 작업 스케줄러에 알려줍니다.
        public void Execute(ref NonUniformScale uniformScale, [ReadOnly] ref ScaleSpeed_IJobForEach sclSpeedIJobForEach)
        {
            // ScaleSpeed_IJobForEach 제공 한 속도로 크기를 증가시킵니다.
            var scale = uniformScale.Value;
            scale.x += sclSpeedIJobForEach.ScalePerSecond * DeltaTime;
            scale.y += sclSpeedIJobForEach.ScalePerSecond * DeltaTime;
            scale.z += sclSpeedIJobForEach.ScalePerSecond * DeltaTime;

            uniformScale.Value = scale;
        }
    }

    // OnUpdate는 메인 스레드에서 실행됩니다.
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new ScaleSpeedJob
        {
            DeltaTime = Time.deltaTime
        };

        return job.Schedule(this, inputDependencies);
    }
}