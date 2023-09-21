using System;
using System.Reflection;
using System.Runtime.InteropServices;
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
            OutputClass = InspectableType<IEntityInterface>.Default;
            OutputMethod = InspectableMethod.Default;
        }

        public Flow(InspectableType<IEntityInterface> inputClass, InspectableAction inputEvent, InspectableType<IEntityInterface> outputClass, InspectableMethod outputMethod)
        {
            InputClass = new InspectableType<IEntityInterface>(inputClass.StoredType);

            InputEvent = InspectableAction.Default;
            InputEvent.FlowIndex = inputEvent.FlowIndex;
            InputEvent.ActionId = inputEvent.ActionId;

            OutputClass = new InspectableType<IEntityInterface>(outputClass.StoredType);

            OutputMethod = InspectableMethod.Default;
            OutputMethod.FlowIndex = outputMethod.FlowIndex;
            OutputMethod.ContainerId = outputMethod.ContainerId;
            OutputMethod = new InspectableMethod(outputMethod.Info);
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