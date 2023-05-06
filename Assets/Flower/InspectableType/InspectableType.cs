using System;
using UnityEngine;

namespace Flower
{
    [Serializable]
    public class InspectableType<T> : ISerializationCallbackReceiver
    {
#if UNITY_EDITOR
        [SerializeField] string baseTypeName;
#endif

        [SerializeField] string qualifiedName;

        internal Type StoredType { get; private set; }

        public InspectableType(Type typeToStore)
        {
            StoredType = typeToStore;
        }

        public override string ToString()
        {
            if (StoredType == null) return string.Empty;
            return StoredType.Name;
        }

        public void OnBeforeSerialize()
        {
            qualifiedName = StoredType?.AssemblyQualifiedName;

#if UNITY_EDITOR
            baseTypeName = typeof(T).AssemblyQualifiedName;
#endif
        }

        public void OnAfterDeserialize()
        {
            if (string.IsNullOrEmpty(qualifiedName) || qualifiedName == "null")
            {
                StoredType = null;
                return;
            }

            StoredType = Type.GetType(qualifiedName);
        }

        public static implicit operator Type(InspectableType<T> t) => t.StoredType;

        // TODO: Validate that t is a subtype of T?
        public static implicit operator InspectableType<T>(Type t) => new InspectableType<T>(t);
    }
}