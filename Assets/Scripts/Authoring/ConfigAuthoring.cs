using Unity.Entities;
using UnityEngine;

public class ConfigAuthoring : MonoBehaviour
{
    class Baker : Baker<ConfigAuthoring>
    {
        public override void Bake(ConfigAuthoring authoring)
        {
            Entity entity = GetEntity(authoring, TransformUsageFlags.None);

            
        }
    }
}