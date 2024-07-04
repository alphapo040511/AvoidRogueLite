using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ActionData))]
public class ActionDataEditor : Editor                  //데이터에 접근한다
{
    public override void OnInspectorGUI()               //기존 인스펙터 GUI 설정을 가져오고 커스텀 할 것을 코딩
    {
        base.OnInspectorGUI();                          //기존 동작을 유지한다
        ActionData actionData = (ActionData)target;

        EditorGUI.BeginChangeCheck();                   //스크립터블 오브젝트에 변화가 있을 때 감지

        var newClip = (AnimationClip)EditorGUILayout.ObjectField("Event Clip", actionData.EventClip, typeof(AnimationClip), false);

        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(actionData, "Changed Animation Clip");
            actionData.EventClip = newClip;             //세터 내에서 mecanimName과 waitTime 설정
            EditorUtility.SetDirty(actionData);
        }
    }
}
