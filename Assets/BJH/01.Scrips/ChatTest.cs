using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//AI ��� ����
//Json�� ��� ���� "Ű" : "��"
[System.Serializable]
public class AiChat
{
    public string island_id;
    public string user_id;
    public string content;
    public string datetime;
}

public class ChatTest : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //���� ���� ��ȸ
    public void OnChatRequery()
    {
        AiChat aiInfo = new AiChat();

        //���÷� �־��
        aiInfo.island_id = "family_1";
        aiInfo.user_id = "jihwan";
        aiInfo.content = "��ȯ�� �����";
        aiInfo.datetime = "2023-10-23 14:40:25.779082";
    


        //Json �������� ���� ������ �� -> �̻ڰ� ������ ���� true
        string aiJsonData = JsonUtility.ToJson(aiInfo, true);
        print(aiJsonData);

        //AI �ε� UI
        HttpManager_LHS.instance.isAichat = false;

        //AI�� ä���� �Ѵ�!
        OnGetPost(aiJsonData);
    }

    //Ai
    // ���� ���� �� -> ê�� ������ ����
    // ������ �Խù� ��ȸ ��û -> HttpManager���� �˷��ַ��� ��
    public void OnGetPost(string s)
    {
        string url = "http://172.17.113.213:5011/api/chatbot/conversation";

        //���� -> ������ ��ȸ -> ���� �־��� 
        HttpRequester_LHS requester = new HttpRequester_LHS();

        requester.SetUrl(RequestType.POST, url, false);
        requester.body = s; // json data
        requester.isJson = true;
        requester.isChat = false; // �̰� ����

        requester.onComplete = OnGetPostComplete;
        requester.onFailed = OnGetPostFailed;

        HttpManager_LHS.instance.SendRequest(requester);
    }


    void OnGetPostComplete(DownloadHandler result)
    {
        print("Chat ����");

        //HttpAiPhotoData aiPhotoData = new HttpAiPhotoData();
        //aiPhotoData = JsonUtility.FromJson<HttpAiPhotoData>(result.text);

        print(result.text);
        //downloadHandler�� �޾ƿ� Json���� ������ �����ϱ�
        //2.FromJson���� ���� �ٲ��ֱ�
        //ChatData chatData = JsonUtility.FromJson<ChatData>(result.text);

        //-----------------ê�� �ֱ�--------------

        //if (aiPhotoData.results.body.response.Trim() == "") return;
    }

    void OnGetPostFailed()
    {
        print("Chat ����");
    }
}
