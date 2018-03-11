using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TP.Utilities
{
    [CreateAssetMenu(menuName = "TPAudioPool/Audio Bundle", fileName = "AudioBundle")]
    public class TPAudioBundle : ScriptableObject
    {
        public TPAudioObject[] AudioObjects;
    }

    [UnityEditor.CustomEditor(typeof(TPAudioBundle))]
    class TPAudioBundleEditor : UnityEditor.Editor
    {
        UnityEditorInternal.ReorderableList list;
        UnityEditor.SerializedProperty array;

        public void OnEnable()
        {
            array = serializedObject.FindProperty("AudioObjects");
            list = new UnityEditorInternal.ReorderableList(serializedObject, array, true, true, true, true)
            {
                drawElementCallback = DrawElement
            };
        }

        void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            float halfWidth = rect.width / 2;

            UnityEditor.EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, halfWidth, UnityEditor.EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("Name"), GUIContent.none);

            UnityEditor.EditorGUI.PropertyField(
                new Rect(rect.x + halfWidth, rect.y, halfWidth, UnityEditor.EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("Clip"), GUIContent.none);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfRequiredOrScript();

            list.DoLayoutList();

            //base.DrawDefaultInspector();
            //if (GUI.changed)
            //{
            //    foreach (UnityEditor.SerializedProperty item in array)
            //    {
            //        bundle.AudioObjects[item.FindPropertyRelative("Name").stringValue] =
            //            (item.FindPropertyRelative("Clip").objectReferenceValue as object as AudioClip);
            //    }
            //}

            serializedObject.ApplyModifiedProperties();
        }
        
    }
}
