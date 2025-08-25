using Components;
using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class ClickMarkerAuthoring : MonoBehaviour
    {
        class Baker : Baker<ClickMarkerAuthoring>
        {
            public override void Bake(ClickMarkerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<ClickMarker>(entity);
            }
        }
    }
}