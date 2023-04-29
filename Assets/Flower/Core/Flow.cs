using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Flower
{
    [Serializable]
    public class Flow
    {
        public InspectableType<Entity> InputClass;
        public InspectableType<Entity> OutputClass;
        public InspectableMethod OutputMethod;
    }
}