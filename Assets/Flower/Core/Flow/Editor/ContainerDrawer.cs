using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Flower
{
    [CustomEditor(typeof(Container))]
    public class ContainerDrawer : Editor
    {
        private const string _defaultFlow = "DefaultFlow";

        private const string _flowFieldName = "Flow";
        private const string _flowListName = "Flows";

        private const string _entityFieldName = "Entity";
        private const string _entityListName = "Entities";

        private SerializedProperty _flowListProperty;
        private SerializedProperty _entityListProperty;

        private Entity _entity;

        private void OnEnable()
        {
            _flowListProperty = serializedObject.FindProperty(_flowListName);
            _entityListProperty = serializedObject.FindProperty(_entityListName);
        }

        public override void OnInspectorGUI()
        {
            ShowInfo();
            DeleteOtherComponents();

            UpdateFlowField(_flowListProperty, _flowFieldName);
            UpdateFlowField(_entityListProperty, _entityFieldName);

            BlockAddComponentButton();
        }

        private void DeleteOtherComponents()
        {
            Container container = (Container)target;
            GameObject gameObject = container.gameObject;

            MonoBehaviour[] components = gameObject.GetComponents<MonoBehaviour>();
            foreach (var component in components)
            {
                if (component != container)
                {
                    Debug.LogWarning("Other components are prohibited.");
                    DestroyImmediate(component);
                }
            }
        }

        private void BlockAddComponentButton()
        {
            EditorGUILayout.Space(float.MaxValue);
        }

        private void ShowInfo()
        {
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = (int)(EditorStyles.label.fontSize * 1.5f),
                wordWrap = true,
            };

            GUILayout.BeginVertical("box");
            GUILayout.Label("Container has entities and flows, that connected from each other.", labelStyle);
            GUILayout.Label("You should know that other components with Container are prohibited.", labelStyle);
            GUILayout.EndVertical();

            EditorGUILayout.Space(10f);
        }

        private void UpdateFlowField(SerializedProperty property, string field)
        {
            serializedObject.Update();

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                stretchHeight = true,
                fontSize = 18,
                fontStyle = FontStyle.Bold,
            };

            EditorGUILayout.LabelField(field, labelStyle, GUILayout.Height(25f));

            for (int i = 0; i < property.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(property.GetArrayElementAtIndex(i), GUIContent.none);
                if (GUILayout.Button("Remove", GUILayout.Width(60)))
                {
                    Container container = target as Container;
                    ContainerBinder.Instance.UnlinkFlow(container.Flows[i], container);

                    property.DeleteArrayElementAtIndex(i);
                }
                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Add new element"))
            {
                Container container = target as Container;
                if (field == _flowFieldName)
                {
                    Flow newFlow = new Flow(container.DefaultFlow.InputClass,
                        container.DefaultFlow.InputEvent,
                        container.DefaultFlow.OutputClass,
                        container.DefaultFlow.OutputMethod
                    );

                    Debug.Log(ContainerBinder.Instance.ValidateFlow(container.DefaultFlow, container));
                    Debug.Log(ContainerBinder.Instance.CheckFlowOnDifference(container.DefaultFlow, container));
                    if (ContainerBinder.Instance.ValidateFlow(container.DefaultFlow, container) && ContainerBinder.Instance.CheckFlowOnDifference(container.DefaultFlow, container))
                    {
                        container.Flows.Add(newFlow);
                        ContainerBinder.Instance.LinkFlow(newFlow, container);
                    }
                }
                else if (field == _entityFieldName)
                {
                    container.AddEntity(_entity);
                }
                EditorUtility.SetDirty(target);
            }

            if (field == _flowFieldName)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty(_defaultFlow), GUIContent.none);
            }
            else if (field == _entityFieldName)
            {
                _entity = (Entity)EditorGUILayout.ObjectField("New entity", _entity, typeof(Entity), true);
            }

            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space(10f);
        }
    }
}