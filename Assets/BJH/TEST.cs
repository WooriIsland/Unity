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
//    //���� ���� ��ȸ
//    public void OnTEST()
//    {

//        LoginInfoTEST aiInfo = new LoginInfoTEST();

//        //���÷� �־��
//        aiInfo.email = "11111";
//        aiInfo.password = "11111";


//        //Json �������� ���� ������ �� -> �̻ڰ� ������ ���� true
//        string aiJsonData = JsonUtility.ToJson(aiInfo, true);
//        print(aiJsonData);

//        //AI �ε� UI
//        HttpManager_LHS.instance.isAichat = false;

//        //AI�� ä���� �Ѵ�!
//        OnGetPost(aiJsonData);
//    }

//    //Ai
//    // ���� ���� �� -> ê�� ������ ����
//    // ������ �Խù� ��ȸ ��û -> HttpManager���� �˷��ַ��� ��
//    public void OnGetPost(string s)
//    {
//        string url = "http://192.168.0.53:8082/api/v1/users/login";

//        //���� -> ������ ��ȸ -> ���� �־��� 
//        HttpRequester requester = new HttpRequester();

//        requester.SetUrl(RequestType.POST, url, false);
//        requester.body = s;
//        requester.isJson = true;
//        requester.isChat = false;

//        requester.onComplete = OnGetPostComplete;
//        requester.onFailed = OnGetPostFailed;

//        print("����Ȯ��");
//        HttpManager_LHS.instance.SendRequest(requester);
//    }

//    private void OnGetPostFailed(DownloadHandler result)
//    {
//        print("�α��� ����");

//    }

//    //���� �Ľ��ϱ�

//    void OnGetPostComplete(DownloadHandler result)
//    {
//        print("�α��� ����");
//    }
//}
