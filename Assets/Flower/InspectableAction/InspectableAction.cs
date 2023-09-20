using System;
using System.Reflection;
using UnityEngine;

namespace Flower
{
    [Serializable]
    public class InspectableAction : ISerializationCallbackReceiver
    {
        [SerializeField] private string _type;
        [SerializeField] private int _flowIndex;
        [SerializeField] private int _containerId = -1;

        [SerializeField] internal int ActionId = -1;

        public static InspectableAction Default
        {
            get
            {
                return new InspectableAction();
            }
        }

        private InspectableAction()
        {
        }

        internal int FlowIndex
        {
            get { return _flowIndex; }
            set { _flowIndex = value; }
        }

        internal int ContainerId
        {
            get { return _containerId; }
            set { _containerId = value; }
        }

        public bool Validate()
        {
            return ActionId != -1;
        }

        public void OnAfterDeserialize()
        {
        }

        public void OnBeforeSerialize()
        {
        }
    }
}