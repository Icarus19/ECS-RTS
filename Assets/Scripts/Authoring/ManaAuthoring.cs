using Unity.Entities;
using UnityEngine;

[RequireComponent(typeof(RegenableAuthoring))]
public class ManaAuthoring : MonoBehaviour
{
    public float mana;
    public float regen;
    class Baker : Baker<ManaAuthoring>
    {
        public override void Bake(ManaAuthoring authoring)
        {
            Entity entity = GetEntity(authoring, TransformUsageFlags.None);
            AddComponent(entity, new Mana
            {
                Current = authoring.mana,
                Max = authoring.mana,
                Regen = authoring.regen
            });
        }
    }
}