using System;
using System.Reflection;
using UnityEngine;

namespace Flower
{
    [System.Serializable]
    public class InspectableMethod : ISerializationCallbackReceiver
    {
        [SerializeField] private string _methodName;
        [SerializeField] private string _type;
        [SerializeField] private int _flowIndex;

        internal int FlowIndex
        {
            get { return _flowIndex; }
            set { _flowIndex = value; }
        }
        internal MethodInfo Info { get; private set; }

        public void OnAfterDeserialize()
        {
            if (Info != null)
            {
                _methodName = Info.Name;
            }
        }

        public void OnBeforeSerialize()
        {
            if (!string.IsNullOrEmpty(_methodName) && _methodName != "<empty>")
            {
                Info = Type.GetType(_type).GetMethod(_methodName);
            }
        }
    }
}
