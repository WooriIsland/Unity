using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChatManager02))]
public class ChatEditor : Editor
{
    ChatManager02 chatManager02;
    string text;

    void OnEnable()
    {
        // ChatManager02를 선택했을 때 실행되는 OnEnable() 함수
        // target을 ChatManager02로서 사용한다는 의미
        chatManager02 = target as ChatManager02;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.BeginHorizontal();
        text = EditorGUILayout.TextArea(text);

        if(GUILayout.Button("보내기", GUILayout.Width(60)) && text.Trim() != "")
        {
            chatManager02.Chat(true, text, "나", null);
            text = "";
            GUI.FocusControl(null);
        }

        if(GUILayout.Button("받기", GUILayout.Width(60)) && text.Trim() != "")
        {
            chatManager02.Chat(false, text, "타인", null);
            text = "";
            GUI.FocusControl(null);
        }
        EditorGUILayout.EndHorizontal();
    }
}
