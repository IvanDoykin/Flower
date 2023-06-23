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
        }

        [ExecuteAlways]
        private void Awake()
        {
            Container.HasCreated += AddContainer;
            foreach (var container in FindObjectsOfType<Container>())
            {
                AddContainer(container);
            }
        }

        private void OnDestroy()
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
            _containerLastId++;

            foreach (var flow in container.Flows)
            {
                LinkFlow(flow);
            }
        }

        private void LinkFlow(Flow flow)
        {
            Entity outputEntity = null;
            Entity inputEntity = null;

            foreach (var container in _containers)
            {
                foreach (var entity in container.Value.Entities)
                {
                    var entityType = entity.GetType();
                    if (entityType == flow.OutputClass)
                    {
                        outputEntity = entity;
                    }

                    else if (entityType == flow.InputClass.StoredType)
                    {
                        inputEntity = entity;
                    }
                }
            }

            if (inputEntity != null && outputEntity != null)
            {
                inputEntity.Link(flow.InputEvent.ActionId, (Entity.EntityMessages)Delegate.CreateDelegate(typeof(Entity.EntityMessages), outputEntity, flow.OutputMethod.Info, false));
            }
        }
    }
}