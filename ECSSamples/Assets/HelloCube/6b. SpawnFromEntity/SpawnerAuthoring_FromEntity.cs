using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Samples.HelloCube_6b
{
    [RequiresEntityConversion]
    public class SpawnerAuthoring_FromEntity : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
    {
        public GameObject Prefab;
        public int        CountX;
        public int        CountY;

        // 변환 시스템이 사전에 미리 알 수 있도록 참조 된 프리 팹을 선언해야합니다.
        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.Add(Prefab);
        }

        // 편집기 데이터 표현을 엔티티 최적 런타임 표현으로 변환 할 수 있습니다
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var spawnerData = new Spawner_FromEntity
            {
                // 참조된 프리 팹은 DeclareReferencedPrefabs로 인해 변환됩니다.
                // 여기서는 게임 오브젝트를 해당 프리 팹에 대한 엔티티 참조에 매핑합니다.
                Prefab = conversionSystem.GetPrimaryEntity(Prefab),
                CountX = CountX,
                CountY = CountY
            };
            dstManager.AddComponentData(entity, spawnerData);
        }
    }
}