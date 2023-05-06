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

        private void Start()
        {
            AddContainerFromObject(_testContainer);
        }

        public void AddContainerFromObject(GameObject subscribingGameobject)
        {
            _containers.Clear();

            if (subscribingGameobject.TryGetComponent(out Container container))
            {
                container.Initialize();
                _containers.Add(container);

                foreach (var flow in container.Flows)
                {
                    TestFlow(flow);
                }
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
                Debug.Log(flow.InputEvent.EventId);
                Debug.Log(flow.InputClass.ToString());
                Debug.Log(flow.OutputClass.ToString());
                Debug.Log(flow.OutputMethod.Info?.Name);
                inputEntity.Link(flow.InputEvent.EventId, (Entity.EntityMessages)Delegate.CreateDelegate(typeof(Entity.EntityMessages), outputEntity, flow.OutputMethod.Info, false));
            }
        }
    }
}