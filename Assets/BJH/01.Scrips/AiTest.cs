using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AiTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickGet()
    {
        HttpInfo info = new HttpInfo();
        info.Set(HttpMethod.GET, "/conversation", OnReceiveGet);

        // info의 정보로 요청을 보내기
        HttpManager02.GetInstance().SendRequest(info);
    }

    void OnReceiveGet(DownloadHandler download)
    {

    }

    public void PostTest()
    {
        HttpInfo info = new HttpInfo();
        info.Set(HttpMethod.POST, "/임시", (DownloadHandler DownloadHandler) => { 
                // post 데이터를 전송하면 서버로부터 응답이 옴
        });

        ChatInfo chatInfo = new ChatInfo();

        chatInfo.island_id = "ji09hwan123";
        chatInfo.user_id = "jihwan98";
        chatInfo.content = "지환이 잠와";
        chatInfo.datetiem = "날짜가 들어오는데 데이터 타입이 뭐지";


        info.body = JsonUtility.ToJson(chatInfo);

        HttpManager02.GetInstance().SendRequest(info);

    }
}
