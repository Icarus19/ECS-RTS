using Units;
using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    public struct SpawnRequest : IComponentData
    {
        public UnitType Id;
        public float Time;
        public float3 Position;
    }
}