using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Flower
{
    [CustomPropertyDrawer(typeof(InspectableMethod), true)]
    public class InspectableMethodDrawer : PropertyDrawer
    {
        private Dictionary<string, MethodInfo[]> _methods = new Dictionary<string, MethodInfo[]>();
        private Dictionary<string, GUIContent[]> _optionLabels = new Dictionary<string, GUIContent[]>();
        private Dictionary<string, int> _selectedIndices = new Dictionary<string, int>();
        private Dictionary<string, string> _oldTypes = new Dictionary<string, string>();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int containerId = property.FindPropertyRelative("_containerId").intValue;
            if (containerId == -1)
            {
                return;
            }

            Container container = ContainerBinder.Instance.GetContainer(containerId);
            int flowIndex = property.FindPropertyRelative("_flowIndex").intValue;

            if (flowIndex >= container.Flows.Count)
            {
                return;
            }

            string newType = "";
            if (flowIndex == -1)
            {
                newType = container.DefaultFlow.OutputClass?.ToString();
            }
            else
            {
                newType = container.Flows[flowIndex].OutputClass?.ToString();
            }

            string propertyPath = property.propertyPath;
            var typeProperty = property.FindPropertyRelative("_type");
            typeProperty.stringValue = newType;

            var storedProperty = property.FindPropertyRelative("_methodName");
            string methodName = storedProperty.stringValue;

            if (!_oldTypes.ContainsKey(propertyPath) || _oldTypes[propertyPath] != newType)
            {
                Initialize(property, storedProperty, propertyPath);
            }

            _oldTypes[propertyPath] = newType;

            EditorGUI.BeginChangeCheck();

            var propLabel = EditorGUI.BeginProperty(position, label, property);
            if (string.IsNullOrEmpty(typeProperty.stringValue))
            {
                EditorGUI.EndProperty();
                return;
            }
            _selectedIndices[propertyPath] = EditorGUI.Popup(position, propLabel, _selectedIndices[propertyPath], _optionLabels[propertyPath]);

            if (EditorGUI.EndChangeCheck())
            {
                storedProperty.stringValue = _selectedIndices[propertyPath] < _methods[propertyPath].Length ? _methods[propertyPath][_selectedIndices[propertyPath]].Name : "<empty>";
            }
            EditorGUI.EndProperty();
        }

        private void Initialize(SerializedProperty property, SerializedProperty stored, string propertyPath)
        {
            var typeProperty = property.FindPropertyRelative("_type");
            var type = Type.GetType(typeProperty.stringValue);

            if (type == null)
            {
                _optionLabels[propertyPath] = new GUIContent[0];
                _methods[propertyPath] = new MethodInfo[0];

                return;
            }

            _methods[propertyPath] = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

            if (_methods[propertyPath].Length == 0)
            {
                _optionLabels[propertyPath] = new[] { new GUIContent($"No methods from {type.Name} found.") };
                return;
            }

            _optionLabels[propertyPath] = new GUIContent[_methods[propertyPath].Length + 1];
            for (int i = 0; i < _methods[propertyPath].Length; i++)
            {
                _optionLabels[propertyPath][i] = new GUIContent(_methods[propertyPath][i].Name);
            }
            _optionLabels[propertyPath][_methods[propertyPath].Length] = new GUIContent("<empty>");

            UpdateIndex(stored, propertyPath);
        }

        private void UpdateIndex(SerializedProperty stored, string propertyPath)
        {
            string methodName = stored.stringValue;

            for (int i = 0; i < _methods[propertyPath].Length; i++)
            {
                if (_methods[propertyPath][i].Name == methodName)
                {
                    _selectedIndices[propertyPath] = i;
                    return;
                }
            }

            _selectedIndices[propertyPath] = _methods[propertyPath].Length;
            stored.stringValue = "<empty>";
        }
    }
}