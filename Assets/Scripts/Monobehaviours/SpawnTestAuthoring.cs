using Unity.Entities;
using UnityEngine;

public class SpawnTestAuthoring : MonoBehaviour
{
    public GameObject prefab;
    class Baker : Baker<SpawnTestAuthoring>
    {
        public override void Bake(SpawnTestAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new SpawnData
            {
                Prefab = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic)
            });
        }
    }
}