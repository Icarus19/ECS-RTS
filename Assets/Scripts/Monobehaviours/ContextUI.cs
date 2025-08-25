using Components;
using Units;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Monobehaviours
{
    public class ContextUI : MonoBehaviour
    {
        public static ContextUI instance;
        
        public Transform buttonContainer;
        public Button buttonPrefab;

        EntityManager em;
        Entity selected;

        void Awake()
        {
            if(instance == null)
                instance = this;
            if(instance != this)
                Destroy(this.gameObject);
        }
        public void Init(EntityManager entityManager, Entity entity)
        {
            em = entityManager;
            selected = entity;
            
            foreach(Transform child in buttonContainer)
                Destroy(child.gameObject); //Create objectpool for better handling

            //Check for selectedType before fetching info
            var buffer = em.GetBuffer<PurchaseList>(selected);
            for (int i = 0; i < buffer.Length; i++)
            {
                var unitId = buffer[i].Id;
                var button = Instantiate(buttonPrefab, buttonContainer);
                button.GetComponentInChildren<Text>().text = unitId.ToString();
                
                button.onClick.AddListener(() => OnPurchase(unitId));
            }
        }

        void OnPurchase(UnitType unitId)
        {
            var request = em.CreateEntity();
            em.AddComponentData(request, new SpawnRequest
            {
                Id = unitId,
                Position = new float3(0, 0, 0)
            });
        }
    }
}