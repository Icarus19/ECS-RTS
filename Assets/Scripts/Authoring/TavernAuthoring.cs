using System.Collections.Generic;
using Components;
using Units;
using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class TavernAuthoring : MonoBehaviour
    {
        public int radius;
        public int health;
        public int food;
        public int gold;

        public List<UnitType> unitList;
        class Baker : Baker<TavernAuthoring>
        {
            public override void Bake(TavernAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
            
                AddComponent(entity, new Tavern());
                AddComponent(entity, new Selected());
                AddComponent(entity, new Health
                {
                    Value = authoring.health,
                    Max = authoring.health
                });
                AddComponent(entity, new PlayerResources
                {
                    Food = authoring.food,
                    Gold = authoring.gold
                });
                AddComponent(entity, new UnitRadius
                {
                    Value = authoring.radius
                });
                var bufferPurchase = AddBuffer<PurchaseList>(entity);
                foreach (var type in authoring.unitList)
                {
                    bufferPurchase.Add(new PurchaseList { Id = type });
                }
            }
        }
    }
}