using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class ScaleSpeedSystem_IJobChunk : JobComponentSystem
{
    EntityQuery m_Group;

    protected override void OnCreate()
    {
        // 특정 쿼리를 기반으로 한 ComponentData 세트에 대한 캐시 된 액세스
        m_Group = GetEntityQuery(typeof(NonUniformScale), ComponentType.ReadOnly<ScaleSpeed_IJobChunk>());
    }

    [BurstCompile]
    struct ScaleSpeedJob : IJobChunk
    {
        public            float                                             DeltaTime;
        public            ArchetypeChunkComponentType<NonUniformScale>      ScaleType;
        [ReadOnly] public ArchetypeChunkComponentType<ScaleSpeed_IJobChunk> ScaleSpeedType;

        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            var chunkScales       = chunk.GetNativeArray(ScaleType);
            var chunkScalesSpeeds = chunk.GetNativeArray(ScaleSpeedType);
            for (int i = 0; i < chunk.Count; i++)
            {
                var scale      = chunkScales[i];
                var scaleSpeed = chunkScalesSpeeds[i];

                // ScaleSpeed_IJobChunk가 제공 한 속도로 크기를 증가시킵니다.
                chunkScales[i] = new NonUniformScale
                {
                    Value = new float3(scale.Value.x + scale.Value.x * scaleSpeed.ScalePerSecond * DeltaTime,
                        scale.Value.y + scale.Value.y * scaleSpeed.ScalePerSecond * DeltaTime,
                        scale.Value.z + scale.Value.z * scaleSpeed.ScalePerSecond * DeltaTime)
                };
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var scaleType      = GetArchetypeChunkComponentType<NonUniformScale>();
        var scaleSpeedType = GetArchetypeChunkComponentType<ScaleSpeed_IJobChunk>(true);

        var job = new ScaleSpeedJob()
        {
            ScaleType      = scaleType,
            ScaleSpeedType = scaleSpeedType,
            DeltaTime      = Time.deltaTime
        };

        return job.Schedule(m_Group, inputDependencies);
    }
}