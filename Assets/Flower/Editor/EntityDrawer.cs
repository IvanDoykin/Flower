using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace Flower
{
#if UNITY_EDITOR
    [CustomEditor(typeof(Entity), true)]
    public class EntityDrawer : Editor
    {
        public override void OnInspectorGUI()
        {
            ShowInfo();
            FilterFields();
            DeleteOtherComponents();
            BlockAddComponentButton();
        }

        private void ShowInfo()
        {
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = (int)(EditorStyles.label.fontSize * 1.5f),
                wordWrap = true,
            };

            GUILayout.BeginVertical("box");
            GUILayout.Label("Entity is an indivisible unit in code design.", labelStyle);
            GUILayout.Label("You should know that using other components with Entity is prohibited.", labelStyle);
            GUILayout.EndVertical();

            EditorGUILayout.Space(10f);
        }

        private void DeleteOtherComponents()
        {
            Entity entity = (Entity)target;
            GameObject gameObject = entity.gameObject;

            MonoBehaviour[] components = gameObject.GetComponents<MonoBehaviour>();
            foreach (var component in components)
            {
                if (component != entity)
                {
                    Debug.LogWarning("Other components are prohibited.");
                    DestroyImmediate(component);
                }
            }
        }

        private void FilterFields()
        {
            serializedObject.Update();

            SerializedProperty property = serializedObject.GetIterator();
            bool isFirst = true;
            while (property.NextVisible(isFirst))
            {
                if (property.name == "m_Script")
                {
                    continue;
                }

                isFirst = false;

                bool isInvalidType = false;

                if (property.propertyType == SerializedPropertyType.ObjectReference)
                {
                    System.Type fieldType = GetFieldType(property);

                    if (fieldType != null && (fieldType == typeof(Entity) || fieldType.IsSubclassOf(typeof(Entity))))
                    {
                        isInvalidType = true;
                    }
                }

                if (!isInvalidType)
                {
                    EditorGUILayout.PropertyField(property, true);
                }
                else
                {
                    EditorGUILayout.HelpBox($"Field {property.name} has type that inherited from Entity. That relation is not allowed.", MessageType.Error);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void BlockAddComponentButton()
        {
            EditorGUILayout.Space(float.MaxValue);
        }

        private System.Type GetFieldType(SerializedProperty property)
        {
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            FieldInfo fieldInfo = property.serializedObject.targetObject.GetType().GetField(property.propertyPath, bindingFlags);
            return fieldInfo?.FieldType;
        }
    }
#endif
}