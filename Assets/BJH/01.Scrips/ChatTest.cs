using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//AI 통신 내용
//Json에 담길 내용 "키" : "값"
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

    //가족 사진 조회
    public void OnChatRequery()
    {
        AiChat aiInfo = new AiChat();

        //예시로 넣어놈
        aiInfo.island_id = "family_1";
        aiInfo.user_id = "jihwan";
        aiInfo.content = "지환이 배고파";
        aiInfo.datetime = "2023-10-23 14:40:25.779082";
    


        //Json 형식으로 값이 들어가지게 됨 -> 이쁘게 나오기 위해 true
        string aiJsonData = JsonUtility.ToJson(aiInfo, true);
        print(aiJsonData);

        //AI 로딩 UI
        HttpManager_LHS.instance.isAichat = false;

        //AI와 채팅을 한다!
        OnGetPost(aiJsonData);
    }

    //Ai
    // 엔터 쳤을 때 -> 챗봇 보내는 내용
    // 서버에 게시물 조회 요청 -> HttpManager한테 알려주려고 함
    public void OnGetPost(string s)
    {
        string url = "http://172.17.113.213:5011/api/chatbot/conversation";

        //생성 -> 데이터 조회 -> 값을 넣어줌 
        HttpRequester_LHS requester = new HttpRequester_LHS();

        requester.SetUrl(RequestType.POST, url, false);
        requester.body = s; // json data
        requester.isJson = true;
        requester.isChat = false; // 이거 뭐지

        requester.onComplete = OnGetPostComplete;
        requester.onFailed = OnGetPostFailed;

        HttpManager_LHS.instance.SendRequest(requester);
    }


    void OnGetPostComplete(DownloadHandler result)
    {
        print("Chat 성공");

        //HttpAiPhotoData aiPhotoData = new HttpAiPhotoData();
        //aiPhotoData = JsonUtility.FromJson<HttpAiPhotoData>(result.text);

        print(result.text);
        //downloadHandler에 받아온 Json형식 데이터 가공하기
        //2.FromJson으로 형식 바꿔주기
        //ChatData chatData = JsonUtility.FromJson<ChatData>(result.text);

        //-----------------챗봇 넣기--------------

        //if (aiPhotoData.results.body.response.Trim() == "") return;
    }

    void OnGetPostFailed()
    {
        print("Chat 실패");
    }
}
