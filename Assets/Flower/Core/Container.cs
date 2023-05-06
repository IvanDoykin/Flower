using System;
using System.Collections.Generic;
using UnityEngine;

namespace Flower
{
    [ExecuteInEditMode]
    public class Container : MonoBehaviour
    {
        internal static Action<Container> HasCreated;

        public List<Flow> Flows = new List<Flow>();
        public List<Entity> Entities = new List<Entity>();

        private void Awake()
        {
            HasCreated?.Invoke(this);
        }

        internal void Initialize()
        {
            Entities.Clear();

            var entities = GetComponentsInChildren<Entity>();
            foreach (var entity in entities)
            {
                AddEntity(entity);
            }
        }

#if UNITY_EDITOR
        [ExecuteInEditMode]
        private void Update()
        {
            for (int i = 0; i < Flows.Count; i++)
            {
                Flows[i].OutputMethod.FlowIndex = i;
            }
        }
#endif

        public void AddEntity<T>() where T : Entity
        {
            var entity = gameObject.AddComponent<T>();
            entity.Initialize();

            Entities.Add(entity);
        }

        public void AddEntity(Entity entity)
        {
            if (entity == null)
            {
                throw new ArgumentException("Try to add null entity.");
            }

            Entities.Add(entity);
            entity.Initialize();
        }
    }
}