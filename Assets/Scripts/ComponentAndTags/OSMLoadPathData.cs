using Unity.Collections;
using Unity.Entities;

namespace Curvit
{
    public struct OSMLoadPathData : IComponentData
    {
        public FixedString4096Bytes LoadPathData;
    }
}