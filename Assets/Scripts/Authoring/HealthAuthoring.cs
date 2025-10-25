using Unity.Entities;
using UnityEngine;

[RequireComponent(typeof(RegenableAuthoring))]
public class HealthAuthoring : MonoBehaviour
{
    public float health;
    public float regen;
    public float shield = 0;
    class Baker : Baker<HealthAuthoring>
    {
        public override void Bake(HealthAuthoring authoring)
        {
            Entity entity = GetEntity(authoring, TransformUsageFlags.None);
            AddComponent(entity, new Shield
            {
                Value = authoring.shield
            });
            AddComponent(entity, new Health
            {
                Current = authoring.health,
                Max = authoring.health,
                Regen = authoring.regen
            });
        }
    }
}