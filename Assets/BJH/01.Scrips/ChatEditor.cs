//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//[CustomEditor(typeof(PhotonChatManager))]
//public class ChatEditor : Editor
//{
//    PhotonChatManager photonChatManager;
//    string text;

//    void OnEnable()
//    {
//        // ChatManager02�� �������� �� ����Ǵ� OnEnable() �Լ�
//        // target�� ChatManager02�μ� ����Ѵٴ� �ǹ�
//        photonChatManager = target as PhotonChatManager;
//    }

//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();

//        EditorGUILayout.BeginHorizontal();
//        text = EditorGUILayout.TextArea(text);

//        if(GUILayout.Button("������", GUILayout.Width(60)) && text.Trim() != "")
//        {
//            // ���� ���´���, ä�� ����, ���� ����� �̸�, ����
//            photonChatManager.CreateChat(true, text, "��");
            
//            // ä�� �����ְ� text �ʱ�ȭ   
//            text = "";
//            GUI.FocusControl(null);
//        }

//        if(GUILayout.Button("�ޱ�", GUILayout.Width(60)) && text.Trim() != "")
//        {
//            photonChatManager.CreateChat(false, text, "Ÿ��");
//            text = "";
//            GUI.FocusControl(null);
//        }
//        EditorGUILayout.EndHorizontal();
//    }
//}
