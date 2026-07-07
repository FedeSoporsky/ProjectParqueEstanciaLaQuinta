using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScenesManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        ScenesManager script = (ScenesManager)target;

        script.sceneSelected = (ScenesManager.Scene)EditorGUILayout.EnumPopup("Scene", script.sceneSelected);

        serializedObject.ApplyModifiedProperties();
    }
}
