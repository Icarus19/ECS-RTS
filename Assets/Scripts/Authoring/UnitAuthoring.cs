using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class UnitAuthoring : MonoBehaviour
    {
        public int costGold;
        public int costFood;
        public int health;

        class Baker : Baker<UnitAuthoring>
        {
            public override void Bake(UnitAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent<Prefab>(entity);
            }
        }
    }
}