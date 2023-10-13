using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Curvit
{
    public struct CurvitEntityData : IComponentData
    {
        public int Id;
        public float3 Position;
    }
}