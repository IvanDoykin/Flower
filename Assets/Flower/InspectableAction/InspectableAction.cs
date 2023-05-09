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

        [SerializeField] internal int ActionId = -1;

        internal int FlowIndex
        {
            get { return _flowIndex; }
            set { _flowIndex = value; }
        }

        public void OnAfterDeserialize()
        {
        }

        public void OnBeforeSerialize()
        {
        }
    }
}