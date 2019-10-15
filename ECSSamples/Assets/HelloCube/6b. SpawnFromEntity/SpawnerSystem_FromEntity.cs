using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Samples.HelloCube_6b
{
    // JobComponentSystems는 worker 스레드에서 실행될 수 있습니다.
    // 그러나 엔터티 생성 및 제거는 race condition을 조건을 방지하기 위해 메인 스레드에서만 수행 할 수 있습니다.
    // 시스템은 EntityCommandBuffer를 사용하여 작업 내에서 수행 할 수없는 작업을 연기합니다.
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class SpawnerSystem_FromEntity : JobComponentSystem
    {
        // BeginInitializationEntityCommandBufferSystem은 명령 버퍼를 작성하는 데 사용됩니다.
        // 그 장벽 시스템이 실행될 때.
        // 인스턴스화 명령은 SpawnJob에 기록되지만 실제로 처리되지는 않습니다 (또는 "재생 됨").
        // 해당 EntityCommandBufferSystem이 업데이트 될 때까지 변환 시스템이 기회를 갖도록
        // 새로 생성 된 엔티티를 처음 렌더링하기 전에 SpawnerSystem_FromEntity에서 실행
        // BeginSimulationEntityCommandBufferSystem을 사용하여 해당 명령을 재생합니다. 이것은 한 프레임 지연을 소개합니다
        // 명령을 기록하고 엔티티를 인스턴스화하는 것 사이에서 실제로는 눈에 띄지 않습니다.
        BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;

        protected override void OnCreate()
        {
            // 필드에서 BeginInitializationEntityCommandBufferSystem을 캐시하므로 매 프레임마다 시스템을 작성할 필요가 없습니다.
            m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }

        struct SpawnJob : IJobForEachWithEntity<Spawner_FromEntity, LocalToWorld>
        {
            public EntityCommandBuffer.Concurrent CommandBuffer;

            public void Execute(Entity      entity, int index, [ReadOnly] ref Spawner_FromEntity spawnerFromEntity,
                [ReadOnly] ref LocalToWorld location)
            {
                for (var x = 0; x < spawnerFromEntity.CountX; x++)
                {
                    for (var y = 0; y < spawnerFromEntity.CountY; y++)
                    {
                        var instance = CommandBuffer.Instantiate(index, spawnerFromEntity.Prefab);

                        // 노이즈가있는 그리드에 인스턴스를 배치하십시오
                        var position = math.transform(location.Value,
                            new float3(x * 1.3F, noise.cnoise(new float2(x, y) * 0.21F) * 2, y * 1.3F));
                        CommandBuffer.SetComponent(index, instance, new Translation {Value = position});
                    }
                }

                CommandBuffer.DestroyEntity(index, entity);
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            // 작업은 구조적 변경을 직접 수행하는 대신 EntityCommandBuffer에 명령을 추가하여 작업이 완료된 후 기본 스레드에서 이러한 변경을 수행 할 수 있습니다.
            // 명령 버퍼를 사용하면 작업자 스레드에서 잠재적으로 비용이 많이 드는 계산을 수행하는 동시에 나중에 실제 삽입 및 삭제를 대기시킬 수 있습니다.

            // EntityCommandBuffer에 Instantiate 명령을 추가 할 작업을 예약하십시오.
            var job = new SpawnJob
            {
                CommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent()
            }.Schedule(this, inputDeps);


            // SpawnJob은 장벽 시스템이 실행될 때까지 동기화 지점없이 병렬로 실행됩니다.
            // 장벽 시스템이 실행되면 SpawnJob을 완료 한 다음 명령을 재생합니다 (엔티티 생성 및 배치).
            // 명령을 재생하기 전에 어떤 작업을 완료해야하는지 장벽 시스템에 알려야합니다.
            m_EntityCommandBufferSystem.AddJobHandleForProducer(job);

            return job;
        }
    }
}