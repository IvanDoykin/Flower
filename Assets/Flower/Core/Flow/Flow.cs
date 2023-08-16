using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Flower
{
    [Serializable]
    public class Flow
    {
        public InspectableType<IEntityInterface> InputClass;
        public InspectableAction InputEvent;
        public InspectableType<IEntityInterface> OutputClass;
        public InspectableMethod OutputMethod;
    }
}