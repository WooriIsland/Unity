//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using TMPro;
//using UnityEngine;
//using UnityEngine.Android;
//using UnityEngine.Networking;
//using UnityEngine.UI;

//[System.Serializable]
//public class LoginInfoTEST
//{
//    public string email;
//    public string password;
//}

//public class TEST : MonoBehaviour
//{
//    //가족 사진 조회
//    public void OnTEST()
//    {

//        LoginInfoTEST aiInfo = new LoginInfoTEST();

//        //예시로 넣어놈
//        aiInfo.email = "11111";
//        aiInfo.password = "11111";


//        //Json 형식으로 값이 들어가지게 됨 -> 이쁘게 나오기 위해 true
//        string aiJsonData = JsonUtility.ToJson(aiInfo, true);
//        print(aiJsonData);

//        //AI 로딩 UI
//        HttpManager_LHS.instance.isAichat = false;

//        //AI와 채팅을 한다!
//        OnGetPost(aiJsonData);
//    }

//    //Ai
//    // 엔터 쳤을 때 -> 챗봇 보내는 내용
//    // 서버에 게시물 조회 요청 -> HttpManager한테 알려주려고 함
//    public void OnGetPost(string s)
//    {
//        string url = "http://192.168.0.53:8082/api/v1/users/login";

//        //생성 -> 데이터 조회 -> 값을 넣어줌 
//        HttpRequester requester = new HttpRequester();

//        requester.SetUrl(RequestType.POST, url, false);
//        requester.body = s;
//        requester.isJson = true;
//        requester.isChat = false;

//        requester.onComplete = OnGetPostComplete;
//        requester.onFailed = OnGetPostFailed;

//        print("서버확인");
//        HttpManager_LHS.instance.SendRequest(requester);
//    }

//    private void OnGetPostFailed(DownloadHandler result)
//    {
//        print("로그인 실패");

//    }

//    //직접 파싱하기

//    void OnGetPostComplete(DownloadHandler result)
//    {
//        print("로그인 성공");
//    }
//}
