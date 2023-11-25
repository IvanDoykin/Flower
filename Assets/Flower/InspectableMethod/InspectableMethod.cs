using System;
using System.Reflection;
using UnityEngine;

namespace Flower
{
    [Serializable]
    public class InspectableMethod : ISerializationCallbackReceiver
    {
        [SerializeField] private string _methodName;
        [SerializeField] private string _type;
        [SerializeField] private int _flowIndex;
        [SerializeField] private int _containerId = -1;

        [SerializeField] public bool IsEditable = true;

        public static InspectableMethod Default
        {
            get
            {
                return new InspectableMethod();
            }
        }

        private InspectableMethod()
        {
        }

        public InspectableMethod(MethodInfo info)
        {
            Info = info;
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

        internal MethodInfo Info { get; private set; }

        public void OnBeforeSerialize()
        {
            if (Info != null)
            {
                _methodName = Info.Name;
            }
        }

        public void OnAfterDeserialize()
        {
            if (Validate())
            {
                try
                {
                    Info = Type.GetType(_type).GetMethod(_methodName);
                }
                catch (Exception)
                {
                    _methodName = "<empty>";
                }
            }
        }

        public bool Validate()
        {
            return !string.IsNullOrEmpty(_methodName) && _methodName != "<empty>";
        }
    }
}
