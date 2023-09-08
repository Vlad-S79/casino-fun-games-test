using System;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace Core.Ui.Common.Editor
{
    public class RaycastElementEditor
    {
        [CanEditMultipleObjects, CustomEditor(typeof(RaycastElement), false)]
        public class RaycstElementEditor : GraphicEditor
        {
            public override void OnInspectorGUI ()
            {
                serializedObject.Update();
        
                EditorGUILayout.PropertyField( m_Script, Array.Empty<GUILayoutOption>() );
                RaycastControlsGUI();
        
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}