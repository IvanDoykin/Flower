using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Flower
{
    [CustomPropertyDrawer(typeof(InspectableMethod), true)]
    public class InspectableMethodDrawer : PropertyDrawer
    {
        private MethodInfo[] _methods;
        private GUIContent[] _optionLabels;
        private int _selectedIndex;
        private string _oldType = "null";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Debug.Log("GUI");

            string newType = GameObject.FindObjectOfType<Container>().Flows[property.FindPropertyRelative("_flowIndex").intValue].OutputClass?.ToString();

            var typeProperty = property.FindPropertyRelative("_type");
            typeProperty.stringValue = newType;

            var storedProperty = property.FindPropertyRelative("_methodName");
            string methodName = storedProperty.stringValue;

            if (_optionLabels == null || newType == "null" || _oldType == "null" || methodName == "<empty>" || newType != _oldType)
            {
                Debug.Log("check");
                Initialize(property, storedProperty);
                storedProperty.stringValue = "<empty>";
            }
            else if (_selectedIndex == _methods.Length)
            {
                if (methodName != "<empty>")
                {
                    Initialize(property, storedProperty);
                }
            }
            else
            {
                if (methodName != _methods[_selectedIndex].Name) 
                {
                    Initialize(property, storedProperty);
                }
            }

            _oldType = newType;

            EditorGUI.BeginChangeCheck();

            var propLabel = EditorGUI.BeginProperty(position, label, property);
            _selectedIndex = EditorGUI.Popup(position, propLabel, _selectedIndex, _optionLabels);

            if (EditorGUI.EndChangeCheck())
            {
                Debug.Log("end");
                storedProperty.stringValue = _selectedIndex < _methods.Length ? _methods[_selectedIndex].Name : "<empty>";
            }
            EditorGUI.EndProperty();
        }

        private void Initialize(SerializedProperty property, SerializedProperty stored)
        {
            var typeProperty = property.FindPropertyRelative("_type");
            var type = Type.GetType(typeProperty.stringValue);

            if (type == null)
            {
                _optionLabels = new GUIContent[0];
                _methods = new MethodInfo[0];

                return;
            }

            _methods = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

            if (_methods.Length == 0)
            {
                _optionLabels = new[] { new GUIContent($"No methods from {type.Name} found.") };
                return;
            }

            _optionLabels = new GUIContent[_methods.Length + 1];
            for (int i = 0; i < _methods.Length; i++)
            {
                _optionLabels[i] = new GUIContent(_methods[i].Name);
            }
            _optionLabels[_methods.Length] = new GUIContent("<empty>");

            UpdateIndex(stored);
        }

        private void UpdateIndex(SerializedProperty stored)
        {
            string methodName = stored.stringValue;

            for (int i = 0; i < _methods.Length; i++)
            {
                if (_methods[i].Name == methodName)
                {
                    _selectedIndex = i;
                    return;
                }
            }

            _selectedIndex = _methods.Length;
            stored.stringValue = "<empty>";
        }
    }
}