using Unity.Entities;

namespace Components.Singletons
{
    public struct VFXPrefabRegistry : IBufferElementData
    {
        public Entity Prefab;
    }
}