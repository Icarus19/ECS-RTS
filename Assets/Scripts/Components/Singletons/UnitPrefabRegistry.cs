using Units;
using Unity.Entities;

namespace Components
{
    public struct UnitPrefabRegistry : IBufferElementData
    {
        public UnitType Id;
        public ResourceCost Cost;
        public Entity Prefab;
    }
}