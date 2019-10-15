using Unity.Entities;

namespace Samples.HelloCube_5b
{
    public struct Spawner_FromEntity : IComponentData
    {
        public int    CountX;
        public int    CountY;
        public Entity Prefab;
    }
}