using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    public struct Movement : IComponentData
    {
        public float Value;
        public float3 Target;
    }
}