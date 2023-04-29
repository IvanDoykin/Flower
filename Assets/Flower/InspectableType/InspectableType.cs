using UnityEngine;

namespace Flower
{
    [System.Serializable]
    public class InspectableType<T> : ISerializationCallbackReceiver
    {
#if UNITY_EDITOR
        // HACK: I wasn't able to find the base type from the SerializedProperty,
        // so I'm smuggling it in via an extra string stored only in-editor.
        [SerializeField] string baseTypeName;
#endif

        [SerializeField] string qualifiedName;
        public System.Type StoredType { get; private set; }

        public InspectableType(System.Type typeToStore)
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
            StoredType = System.Type.GetType(qualifiedName);
        }

        public static implicit operator System.Type(InspectableType<T> t) => t.StoredType;

        // TODO: Validate that t is a subtype of T?
        public static implicit operator InspectableType<T>(System.Type t) => new InspectableType<T>(t);
    }
}