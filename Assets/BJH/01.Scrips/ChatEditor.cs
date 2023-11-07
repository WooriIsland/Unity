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
//        // ChatManager02를 선택했을 때 실행되는 OnEnable() 함수
//        // target을 ChatManager02로서 사용한다는 의미
//        photonChatManager = target as PhotonChatManager;
//    }

//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();

//        EditorGUILayout.BeginHorizontal();
//        text = EditorGUILayout.TextArea(text);

//        if(GUILayout.Button("보내기", GUILayout.Width(60)) && text.Trim() != "")
//        {
//            // 내가 보냈는지, 채팅 내용, 보낸 사람의 이름, 사진
//            photonChatManager.CreateChat(true, text, "나");
            
//            // 채팅 보내주고 text 초기화   
//            text = "";
//            GUI.FocusControl(null);
//        }

//        if(GUILayout.Button("받기", GUILayout.Width(60)) && text.Trim() != "")
//        {
//            photonChatManager.CreateChat(false, text, "타인");
//            text = "";
//            GUI.FocusControl(null);
//        }
//        EditorGUILayout.EndHorizontal();
//    }
//}
