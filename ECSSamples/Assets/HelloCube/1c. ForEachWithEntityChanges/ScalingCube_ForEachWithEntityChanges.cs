using System;
using Unity.Entities;

namespace Samples.HelloCube_1c
{
    // Serializable attribute is for editor support.
    [Serializable]
    public struct ScalingCube_ForEachWithEntityChanges : IComponentData
    {
        // ScalingCube_ForEachWithEntityChanges는 "태그"구성 요소이며 데이터를 포함하지 않습니다.
        // 태그 구성 요소는 시스템이 처리해야하는 엔티티를 표시하는 데 사용될 수 있습니다.
    }
}

