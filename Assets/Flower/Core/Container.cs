using System;
using System.Collections.Generic;
using UnityEngine;

namespace Flower
{
    [ExecuteInEditMode]
    public class Container : MonoBehaviour
    {
        [SerializeField] public List<Flow> Flows = new List<Flow>();
        [SerializeField] public List<Entity> Entities = new List<Entity>();

        public void Initialize()
        {
            Entities.Clear();

            var entities = GetComponentsInChildren<Entity>();
            foreach (var entity in entities)
            {
                Entities.Add(entity);
                entity.Initialize();
            }
        }

        [ExecuteInEditMode]

        private void Update()
        {
            for (int i = 0; i < Flows.Count; i++)
            {
                Flows[i].OutputMethod.FlowIndex = i;
            }
        }

        public void AddEntity<T>() where T : Entity
        {
            Entities.Add(gameObject.AddComponent<T>());
        }

        public void AddEntity(Entity entity)
        {
            if (entity == null)
            {
                throw new ArgumentException("Try to add null entity.");
            }

            Entities.Add(entity);
        }
    }
}