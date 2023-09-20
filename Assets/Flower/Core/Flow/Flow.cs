using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Flower
{
    [Serializable]
    public class Flow
    {
        public static Flow Default
        {
            get
            {
                return new Flow();
            }
        }

        private Flow()
        {
            InputClass = InspectableType<IEntityInterface>.Default;
            InputEvent = InspectableAction.Default;
            InputEvent.ActionId = 0;
            InputEvent.ContainerId = 0;
            InputEvent.FlowIndex = 0;
            OutputClass = InspectableType<IEntityInterface>.Default;
        }

        public InspectableType<IEntityInterface> InputClass;
        public InspectableAction InputEvent;
        public InspectableType<IEntityInterface> OutputClass;
        public InspectableMethod OutputMethod;

        internal bool Validate()
        {
            return InputClass != null && InputEvent != null && OutputClass != null && OutputMethod != null
                && InputClass.Validate() && InputEvent.Validate() && OutputClass.Validate() && OutputMethod.Validate();
        }
    }
}