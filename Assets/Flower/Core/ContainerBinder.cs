using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

namespace Flower
{
    [ExecuteAlways]
    public class ContainerBinder : MonoBehaviour
    {
        [SerializeField] private List<Container> _containersDebug = new List<Container>();
        private Dictionary<int, Container> _containers = new Dictionary<int, Container>();
        private int _containerLastId = 0;

        [ExecuteAlways]
        private void Start()
        {
            Debug.Log("Binder has initialized.");
            _containers = new Dictionary<int, Container>();
            _containerLastId = 0;

            Container.HasCreated += AddContainer;
            Container.HasDestroyed += RemoveContainer;

            foreach (var container in FindObjectsOfType<Container>())
            {
                Debug.Log("Bind container.");
                AddContainer(container);
            }
        }

        [ExecuteAlways]
        private void Update()
        {
            _containersDebug.Clear();
            foreach (var container in _containers.Values)
            {
                _containersDebug.Add(container);
            }
        }

        [ExecuteAlways]
        private void OnDestroy()
        {
            Container.HasCreated -= AddContainer;
            Container.HasDestroyed -= RemoveContainer;
        }

        internal Container GetContainer(int containerId)
        {
            if (!_containers.ContainsKey(containerId))
            {
                throw new Exception($"Container with id:{containerId} not found.");
            }
            return _containers[containerId];
        }

        private void RemoveContainer(Container container)
        {
            _containers.Remove(container.Id);
            Debug.Log($"Container with id:{container.Id} has removed.");
        }

        private void AddContainer(Container container)
        {
            container.Initialize();

            container.Id = _containerLastId;
            _containers.Add(_containerLastId, container);
            Debug.Log($"Container with id:{container.Id} has created.");

            _containerLastId++;

            foreach (var flow in container.Flows)
            {
                if (flow != null && flow.Validate())
                {
                    LinkFlow(flow, container);
                }
            }
        }

        private bool ValidateFlow(Flow checkFlow, Container container)
        {
            foreach (var flow in container.Flows)
            {
                if (checkFlow.InputClass.GetType() == flow.InputClass.GetType() && checkFlow.InputEvent == flow.InputEvent)
                {
                    container.Flows.Remove(flow);
                    return false;
                }
            }

            return true;
        }

        private void LinkFlow(Flow flow, Container container)
        {
            if (!ValidateFlow(flow, container))
            {
                throw new Exception("False validate");
            }
            List<Entity> outputEntities = new List<Entity>();
            List<Entity> inputEntities = new List<Entity>();

            foreach (var entity in container.Entities)
            {
                var entityType = entity.GetType();

                Debug.Log($"{entityType} is derived from {flow.OutputClass} = {flow.OutputClass.StoredType.IsAssignableFrom(entityType)}.");
                Debug.Log($"{entityType} is derived from {flow.InputClass.StoredType} = {flow.InputClass.StoredType.IsAssignableFrom(entityType)}.");

                if (flow.OutputClass.StoredType.IsAssignableFrom(entityType))
                {
                    outputEntities.Add(entity);
                }

                else if (flow.InputClass.StoredType.IsAssignableFrom(entityType))
                {
                    inputEntities.Add(entity);
                }
            }

            if (outputEntities.Count > 0 && inputEntities.Count > 0)
            {
                for (int i = 0; i < outputEntities.Count; i++)
                {
                    for (int j = 0; j < inputEntities.Count; j++)
                    {
                        inputEntities[j].Link(flow.InputEvent.ActionId, (Entity.EntityMessages)Delegate.CreateDelegate(typeof(Entity.EntityMessages), outputEntities[i], flow.OutputMethod.Info, false), outputEntities[i].GetHashCode());
                    }
                }
            }
        }

        public void UnlinkFlow(Flow flow, Container container)
        {
            List<Entity> outputEntities = new List<Entity>();
            List<Entity> inputEntities = new List<Entity>();

            foreach (var entity in container.Entities)
            {
                var entityType = entity.GetType();

                Debug.Log($"{entityType} is derived from {flow.OutputClass} = {flow.OutputClass.StoredType.IsAssignableFrom(entityType)}.");
                Debug.Log($"{entityType} is derived from {flow.InputClass.StoredType} = {flow.InputClass.StoredType.IsAssignableFrom(entityType)}.");

                if (flow.OutputClass.StoredType.IsAssignableFrom(entityType))
                {
                    outputEntities.Add(entity);
                }

                else if (flow.InputClass.StoredType.IsAssignableFrom(entityType))
                {
                    inputEntities.Add(entity);
                }
            }

            if (outputEntities.Count > 0 && inputEntities.Count > 0)
            {
                for (int i = 0; i < outputEntities.Count; i++)
                {
                    for (int j = 0; j < inputEntities.Count; j++)
                    {
                        container.Flows.Remove(flow);
                        inputEntities[j].Unlink(flow.InputEvent.ActionId, flow.OutputMethod.Info, outputEntities[i].GetHashCode());
                    }
                }
            }
        }
    }
}