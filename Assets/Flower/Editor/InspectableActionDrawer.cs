using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Flower
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(InspectableAction), true)]
    public class InspectableActionDrawer : PropertyDrawer
    {
        private EventInfo[] _events;
        private GUIContent[] _optionLabels;
        private int _selectedIndex;
        private string _oldType = "null";

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
                newType = container.DefaultFlow.InputClass?.ToString();
            }
            else
            {
                newType = container.Flows[flowIndex].InputClass?.ToString();
            }

            var typeProperty = property.FindPropertyRelative("_type");
            typeProperty.stringValue = newType;

            var storedProperty = property.FindPropertyRelative("ActionId");
            var actionIndex = storedProperty.intValue;

            if (_optionLabels == null || newType == "null" || _oldType == "null" || actionIndex == -1 || newType != _oldType)
            {
                Initialize(property, storedProperty);
            }
            else if (_selectedIndex == _events.Length)
            {
                if (actionIndex != -1)
                {
                    Initialize(property, storedProperty);
                }
            }
            else
            {
                if (actionIndex != _selectedIndex)
                {
                    Initialize(property, storedProperty);
                }
            }

            _oldType = newType;

            EditorGUI.BeginChangeCheck();

            var propLabel = EditorGUI.BeginProperty(position, label, property);
            if (string.IsNullOrEmpty(typeProperty.stringValue))
            {
                EditorGUI.EndProperty();
                return;
            }
            _selectedIndex = EditorGUI.Popup(position, propLabel, _selectedIndex, _optionLabels);

            if (EditorGUI.EndChangeCheck() && property.FindPropertyRelative("IsEditable").boolValue)
            {
                storedProperty.intValue = _selectedIndex < _events.Length ? _selectedIndex : -1;
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
                _events = new EventInfo[0];

                return;
            }

            _events = type.GetEvents();

            if (_events.Length == 0)
            {
                _optionLabels = new[] { new GUIContent($"No events from {type.Name} found.") };
                stored.intValue = -1;
                return;
            }

            _optionLabels = new GUIContent[_events.Length + 1];
            for (int i = 0; i < _events.Length; i++)
            {
                _optionLabels[i] = new GUIContent(_events[i].Name);
            }
            _optionLabels[_events.Length] = new GUIContent("<empty>");

            UpdateIndex(stored);
        }

        private void UpdateIndex(SerializedProperty stored)
        {
            var eventIndex = stored.intValue;

            for (int i = 0; i < _events.Length; i++)
            {
                if (i == eventIndex)
                {
                    _selectedIndex = i;
                    return;
                }
            }

            _selectedIndex = _events.Length;
            stored.intValue = -1;
        }

    }
#endif
}