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
        // ChatManager02�� �������� �� ����Ǵ� OnEnable() �Լ�
        // target�� ChatManager02�μ� ����Ѵٴ� �ǹ�
        chatManager02 = target as ChatManager02;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.BeginHorizontal();
        text = EditorGUILayout.TextArea(text);

        if(GUILayout.Button("������", GUILayout.Width(60)) && text.Trim() != "")
        {
            chatManager02.Chat(true, text, "��", null);
            text = "";
            GUI.FocusControl(null);
        }

        if(GUILayout.Button("�ޱ�", GUILayout.Width(60)) && text.Trim() != "")
        {
            chatManager02.Chat(false, text, "Ÿ��", null);
            text = "";
            GUI.FocusControl(null);
        }
        EditorGUILayout.EndHorizontal();
    }
}
