using Unity.Entities;
using UnityEngine;

public class RegenableAuthoring : MonoBehaviour
{
    class Baker : Baker<RegenableAuthoring>
    {
        public override void Bake(RegenableAuthoring authoring)
        {
            Entity entity = GetEntity(authoring, TransformUsageFlags.None);
            AddComponent(entity, new Regenable());
        }
    }
}