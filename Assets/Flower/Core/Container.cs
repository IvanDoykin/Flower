using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Flower
{
    [ExecuteAlways]
    public class Container : MonoBehaviour
    {
        [SerializeField]
        internal Flow DefaultFlow = Flow.Default;
        [SerializeField]
        internal Entity DefaultEntity = null;

        public List<Flow> Flows = new List<Flow>();
        public List<Entity> Entities = new List<Entity>();

        internal static Action<Container> HasCreated;
        internal static Action<Container> HasDestroyed;

        internal int Id;

        [ExecuteAlways]
        private void Awake()
        {
            Debug.Log("Container has created.");
            HasCreated?.Invoke(this);
        }

        [ExecuteAlways]
        private void OnDestroy()
        {
            Debug.Log("Container has destroyed.");
            HasDestroyed?.Invoke(this);
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
                Flows[i].InputEvent.ContainerId = Id;
                Flows[i].OutputMethod.ContainerId = Id;

                Flows[i].OutputMethod.FlowIndex = i;
                Flows[i].InputEvent.FlowIndex = i;
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