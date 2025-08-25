using Components;
using Units;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Systems
{
    partial struct TavernSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Tavern>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var resources = SystemAPI.GetSingletonRW<PlayerResources>();
            //resources.ValueRW.Gold += 10;
            //resources.ValueRW.Food += 1;
            //Debug.Log($"Gold: {resources.ValueRO.Gold}, Food: {resources.ValueRO.Food}");
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        
        }
    }
}
