using Unity.Entities;

namespace Components
{
    public struct Health : IComponentData
    {
        public float Value;
        public int Max;
    }
}