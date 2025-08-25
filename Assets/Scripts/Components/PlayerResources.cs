using Unity.Entities;

namespace Components
{
    public struct PlayerResources : IComponentData
    {
        public int Gold;
        public int Food;
    }
}
