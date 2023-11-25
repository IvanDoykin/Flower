using System;
using UnityEngine;

namespace Flower
{
    [Serializable]
    public class InspectableType<T> : ISerializationCallbackReceiver
    {
        [SerializeField] string baseTypeName;
        [SerializeField] string qualifiedName;
        [SerializeField] public bool IsEditable = true;

        internal Type StoredType { get; private set; }

        public static InspectableType<T> Default
        {
            get
            {
                return new InspectableType<T>(typeof(Entity));
            }
        }

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
            if (Validate())
            {
                StoredType = Type.GetType(qualifiedName);
            }
            else
            {
                StoredType = null;
            }
        }

        public bool Validate()
        {
            return !string.IsNullOrEmpty(qualifiedName) && qualifiedName != "null";
        }

        public static implicit operator Type(InspectableType<T> t) => t.StoredType;

        // TODO: Validate that t is a subtype of T?
        public static implicit operator InspectableType<T>(Type t) => new InspectableType<T>(t);
    }
}