using Unity.Entities;

namespace Curvit
{
    public struct CurvitPrefabData : IComponentData
    {
        public Entity nodeEntityPrefab;
        public Entity wayEntityPrefab;
        public Entity laneletEntityPrefab;
    }
}