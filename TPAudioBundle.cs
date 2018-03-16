/**
*   Authored by Tomasz Piowczyk
*   @ 2018 MIT LICENSE
*   Repository: https://github.com/Prastiwar/TPAudioPool 
*/
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
        bool isValid;

        public void OnEnable()
        {
            list = new UnityEditorInternal.ReorderableList(serializedObject, serializedObject.FindProperty("AudioObjects"), true, true, true, true)
            {
                drawElementCallback = DrawElement,
                onAddCallback = OnAdd,
                drawHeaderCallback = (Rect rect) => { UnityEditor.EditorGUI.LabelField(rect, "Audio Objects in this bundle"); }
            };
        }

        void OnAdd(UnityEditorInternal.ReorderableList reList)
        {
            var index = reList.serializedProperty.arraySize;
            reList.serializedProperty.arraySize++;
            reList.index = index;
            var element = reList.serializedProperty.GetArrayElementAtIndex(index);
            element.FindPropertyRelative("Name").stringValue = "";
            element.FindPropertyRelative("Clip").objectReferenceValue = null;
        }

        void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            float halfWidth = rect.width / 2;
            int length = list.serializedProperty.arraySize;

            UnityEditor.EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, halfWidth, UnityEditor.EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("Name"), GUIContent.none);

            UnityEditor.EditorGUI.PropertyField(
                new Rect(rect.x + halfWidth, rect.y, halfWidth, UnityEditor.EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("Clip"), GUIContent.none);

            for (int i = 0; i < length; i++)
            {
                if (i == index)
                {
                    continue;
                }

                var otherElement = list.serializedProperty.GetArrayElementAtIndex(i);
                if (otherElement.FindPropertyRelative("Name").stringValue == element.FindPropertyRelative("Name").stringValue)
                {
                    isValid = false;
                    UnityEditor.EditorGUI.DrawRect(new Rect(rect.x - 16, rect.y, 15, UnityEditor.EditorGUIUtility.singleLineHeight), Color.red);
                    return;
                }
            }

            isValid = true;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfRequiredOrScript();

            list.DoLayoutList();

            if (!isValid)
            {
                if (list.serializedProperty.arraySize < 1)
                {
                    isValid = true;
                }
                UnityEditor.EditorGUILayout.HelpBox("You have non unique names in bundle!", UnityEditor.MessageType.Error);
            }

            serializedObject.ApplyModifiedProperties();
        }
        
    }
}
