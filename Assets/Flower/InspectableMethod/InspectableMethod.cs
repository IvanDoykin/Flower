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
        public MethodInfo Info
        {
            get
            {
                return _info;
            }
            private set 
            {
                Debug.Log($"set {value.Name}");
                _info = value; 
            }
        }
        private MethodInfo _info;

        public void OnBeforeSerialize()
        {
                _methodName = "Receive";
            Debug.Log($"set methodName");
        }

        public void OnAfterDeserialize()
        {
            if (!string.IsNullOrEmpty(_methodName) && _methodName != "<empty>")
            {
                Debug.Log("set Info");
                Info = typeof(Receiver).GetMethod("Receive");
            }
        }
    }
}
