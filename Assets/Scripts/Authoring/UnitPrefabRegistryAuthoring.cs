using System.Collections.Generic;
using Components;
using Units;
using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    [System.Serializable]
    public struct PrefabRegistry
    {
        public UnitType Id;
        public ResourceCost Cost;
        public GameObject Prefab;
    }

    public class UnitPrefabRegistryAuthoring : MonoBehaviour
    {
        public List<PrefabRegistry> registry;

        class Baker : Baker<UnitPrefabRegistryAuthoring>
        {
            public override void Bake(UnitPrefabRegistryAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                var buffer = AddBuffer<UnitPrefabRegistry>(entity);
                foreach (var prefab in authoring.registry)
                {
                    buffer.Add(new UnitPrefabRegistry
                    {
                        Id = prefab.Id,
                        Cost = prefab.Cost,
                        Prefab = GetEntity(prefab.Prefab, TransformUsageFlags.Dynamic)
                    });
                }
            }
        }
    }
}