using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

namespace Flower
{
    public class ContainerBinder : MonoBehaviour
    {
        [SerializeField] private GameObject _testContainer;
        private List<Container> _containers = new List<Container>();

        private void Awake()
        {
            Container.HasCreated += AddContainer;   
        }

        private void OnDestroy()
        {
            Container.HasCreated -= AddContainer;
        }

        private void AddContainer(Container container)
        {
            container.Initialize();
            _containers.Add(container);

            foreach (var flow in container.Flows)
            {
                TestFlow(flow);
            }
        }

        private void TestFlow(Flow flow)
        {
            Entity outputEntity = null;
            Entity inputEntity = null;

            foreach (var container in _containers)
            {
                foreach (var entity in container.Entities)
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