using System.Resources;
using Components;
using Components.Singletons;
using Monobehaviours;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
    public partial struct SelectionSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            
        }
        public void OnUpdate(ref SystemState state)
        {
            if (!Input.GetMouseButton(0))
                return;
            
            foreach (var selectedEntity in SystemAPI.QueryBuilder().WithAll<Selected>().Build()
                         .ToEntityArray(Allocator.Temp))
            {
                state.EntityManager.SetComponentEnabled<Selected>(selectedEntity, false);
            }
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!new Plane(Vector3.up, Vector3.zero).Raycast(ray, out float enter))
                return;
            
            Vector3 worldPos = ray.GetPoint(enter);
            float2 clickPos = new float2(worldPos.x, worldPos.z);
            
            Entity closestEntity = Entity.Null;
            float closestDistSq = float.MaxValue;

            foreach (var (transform, radius, entity) in
                     SystemAPI.Query<RefRO<LocalTransform>, RefRO<UnitRadius>>()
                         .WithAll<Selected>()
                         .WithEntityAccess())
            {
                float2 entityPos = new float2(transform.ValueRO.Position.x, transform.ValueRO.Position.z);
                float distSq = math.distancesq(entityPos, clickPos);

                if (distSq <= radius.ValueRO.Value * radius.ValueRO.Value && distSq < closestDistSq)
                {
                    closestDistSq = distSq;
                    closestEntity = entity;
                }
            }

            if (closestEntity != Entity.Null)
            {
                state.EntityManager.SetComponentEnabled<Selected>(closestEntity, true);
            
                var ui = ContextUI.instance;
                ui.Init(state.EntityManager, closestEntity);
            }

            foreach (var (transform, entity) in
                     SystemAPI.Query<RefRW<LocalTransform>>().WithAll<ClickMarker>().WithEntityAccess())
            {
                transform.ValueRW.Position = worldPos;
            }

            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                foreach(var (radius, transform)
                        in SystemAPI.Query<RefRO<UnitRadius>, RefRO<LocalTransform>>())
                {
                    Debug.Log($"Position: {transform.ValueRO.Position}, Radius: {radius.ValueRO.Value}");
                }
            }
        }
    }
}