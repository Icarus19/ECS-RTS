using Unity.Entities;
using UnityEngine;

namespace Systems.DamageEventSystems
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct DamageEventManagerBootstrap : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            //DamageEventManagerSingleton.Instance();
        }

        public void OnDestroy(ref SystemState state)
        {
            DamageEventManagerSingleton.Instance.Dispose();
        }
    }
}