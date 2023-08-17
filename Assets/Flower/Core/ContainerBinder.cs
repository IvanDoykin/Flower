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
        private Dictionary<int, Container> _containers = new Dictionary<int, Container>();
        private int _containerLastId = 0;

        [ExecuteAlways]
        private void OnEnable()
        {
            _containers = new Dictionary<int, Container>();
            _containerLastId = 0;

            Container.HasCreated += AddContainer;
            foreach (var container in FindObjectsOfType<Container>())
            {
                AddContainer(container);
            }
        }

        private void OnDisable()
        {
            Container.HasCreated -= AddContainer;
        }

        internal Container GetContainer(int containerId)
        {
            if (!_containers.ContainsKey(containerId))
            {
                throw new Exception($"Container with id:{containerId} not found.");
            }
            return _containers[containerId];
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
                LinkFlow(flow, container);
            }
        }

        private void LinkFlow(Flow flow, Container container)
        {
            Entity outputEntity = null;
            Entity inputEntity = null;

            foreach (var entity in container.Entities)
            {
                var entityType = entity.GetType();

                Debug.Log($"{entityType} is derived from {flow.OutputClass} = {flow.OutputClass.StoredType.IsAssignableFrom(entityType)}.");
                Debug.Log($"{entityType} is derived from {flow.InputClass.StoredType} = {flow.InputClass.StoredType.IsAssignableFrom(entityType)}.");

                if (flow.OutputClass.StoredType.IsAssignableFrom(entityType))
                {
                    outputEntity = entity;
                }

                else if (flow.InputClass.StoredType.IsAssignableFrom(entityType))
                {
                    inputEntity = entity;
                }
            }

            if (inputEntity != null && outputEntity != null)
            {
                inputEntity.Link(flow.InputEvent.ActionId, (Entity.EntityMessages)Delegate.CreateDelegate(typeof(Entity.EntityMessages), outputEntity, flow.OutputMethod.Info, false));
            }
        }
    }
}