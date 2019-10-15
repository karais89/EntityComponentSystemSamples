using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Samples.HelloCube_1c
{
    public class Spawner_ForEachWithEntityChanges : MonoBehaviour
    {
        public GameObject Prefab;
        public int        CountX = 100;
        public int        CountY = 100;

        void Start()
        {
            // 게임 오브젝트 계층에서 엔티티 프리 팹을 한 번 생성
            var prefab        = GameObjectConversionUtility.ConvertGameObjectHierarchy(Prefab, World.Active);
            var entityManager = World.Active.EntityManager;

            for (int x = 0; x < CountX; x++)
            {
                for (int y = 0; y < CountX; y++)
                {
                    // 이미 변환 된 엔타티 프리팹에서 여러 엔티티를 효율적으로 인스턴스화
                    var instance = entityManager.Instantiate(prefab);

                    // 노이즈가있는 그리드에 인스턴스화 된 엔터티를 배치합니다
                    var position = transform.TransformPoint(new float3(x - CountX / 2, noise.cnoise(new float2(x, y) * 0.21F) * 10, y - CountY / 2));
                    entityManager.SetComponentData(instance, new Translation() {Value = position});
                    entityManager.AddComponentData(instance, new NonUniformScale {Value = new float3(1.0f, 1.0f, 1.0f)});
                    entityManager.AddComponentData(instance, new ScaleUp_ForEachWithEntityChanges());
                    entityManager.AddComponentData(instance, new ScalingCube_ForEachWithEntityChanges());
                }
            }
        }
    }
}