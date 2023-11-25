using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Flower
{
    [CustomPropertyDrawer(typeof(Flow), true)]
    public class FlowDrawer : PropertyDrawer
    {
        private const float _subLabelSpacing = 4;
        private const float _bottomSpacing = 2;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            DrawMultiplePropertyFields(position, new GUIContent[4] { new GUIContent(), new GUIContent(), new GUIContent(), new GUIContent() },
                new SerializedProperty[4] { property.FindPropertyRelative("InputClass"), property.FindPropertyRelative("InputEvent") ,
                property.FindPropertyRelative("OutputClass"), property.FindPropertyRelative("OutputMethod")});
        }

        private static void DrawMultiplePropertyFields(Rect pos, GUIContent[] subLabels, SerializedProperty[] props)
        {
            // backup gui settings
            var indent = EditorGUI.indentLevel;
            var labelWidth = EditorGUIUtility.labelWidth;

            // draw properties
            var propsCount = props.Length;
            var width = (pos.width - (propsCount - 1) * _subLabelSpacing) / propsCount;
            var contentPos = new Rect(pos.x, pos.y, width, pos.height);
            EditorGUI.indentLevel = 0;
            for (var i = 0; i < propsCount; i++)
            {
                EditorGUIUtility.labelWidth = EditorStyles.label.CalcSize(subLabels[i]).x;
                EditorGUI.PropertyField(contentPos, props[i], subLabels[i]);
                contentPos.x += width + _subLabelSpacing;
            }

            // restore gui settings
            EditorGUIUtility.labelWidth = labelWidth;
            EditorGUI.indentLevel = indent;
        }
    }
}