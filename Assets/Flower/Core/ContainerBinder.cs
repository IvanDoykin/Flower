using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Flower
{
    public class ContainerBinder
    {
        public static ContainerBinder Instance => _instance;
        private static ContainerBinder _instance = new ContainerBinder();

        private Dictionary<int, Container> _containers = new Dictionary<int, Container>();
        private int _containerLastId = 0;

        public ContainerBinder()
        {
            if (_instance != null)
            {
                Reinitialize();
            }
            else
            {
                Initialize();
            }
        }

        [ContextMenu("Reinitialize")]
        private void Reinitialize()
        {
            _instance?.Dispose();
            _instance = new ContainerBinder();
            _instance.Initialize();
        }

        private void Initialize()
        {
            Debug.Log("Binder has initialized.");
            _containers = new Dictionary<int, Container>();
            _containerLastId = 0;

            Container.HasCreated += AddContainer;
            Container.HasDestroyed += RemoveContainer;

            foreach (var container in GameObject.FindObjectsOfType<Container>())
            {
                Debug.Log("Bind container.");
                AddContainer(container);
            }
        }

        private void Dispose()
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

            List<Flow> removedFlows = new List<Flow>();
            foreach (var flow in container.Flows)
            {
                if (flow != null)
                {
                    if (ValidateFlow(flow, container))
                    {
                        if (CheckFlowOnDifference(flow, container))
                        {
                            LinkFlow(flow, container);
                        }
                    }
                    else
                    {
                        removedFlows.Add(flow);
                    }
                }
            }
            foreach (var flow in removedFlows)
            {
                container.Flows.Remove(flow);
            }

            Debug.LogError("Containers: " + _containers.Count);
        }

        public bool ValidateFlow(Flow checkFlow, Container container)
        {
            return checkFlow.Validate();
        }

        public bool CheckFlowOnDifference(Flow checkingFlow, Container container)
        {
            foreach (var flow in container.Flows)
            {
                if (flow == checkingFlow)
                {
                    continue;
                }

                bool isSimilarInputClasses = flow.InputClass.StoredType.IsAssignableFrom(checkingFlow.InputClass.StoredType) ||
                    checkingFlow.InputClass.StoredType.IsAssignableFrom(flow.InputClass.StoredType);
                bool isSimilarOutputClasses = flow.OutputClass.StoredType.IsAssignableFrom(checkingFlow.OutputClass.StoredType) ||
                    checkingFlow.OutputClass.StoredType.IsAssignableFrom(flow.OutputClass.StoredType);
                bool isEqualEvents = flow.InputEvent.ActionId == checkingFlow.InputEvent.ActionId;
                bool isEqualMethods = flow.OutputMethod.Info.Name == checkingFlow.OutputMethod.Info.Name;

                if (isSimilarInputClasses && isSimilarOutputClasses && isEqualEvents && isEqualMethods)
                {
                    return false;
                }
            }

            return true;
        }

        public void LinkFlow(Flow flow, Container container)
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