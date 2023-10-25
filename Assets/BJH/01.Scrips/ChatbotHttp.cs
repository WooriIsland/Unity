using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ChatbotHttp : MonoBehaviour
{
    public string testResul;
    // Start is called before the first frame update
    void Start()
    {

    }

    DownloadHandler d;

    // Update is called once per frame
    void Update()
    {
        // 임시
        // 5를 누르면 챗봇에게 get요청을 보낸다.
        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            CreateHttpInfo();
            HttpManager02.GetInstance().SendRequest(httpInfo);
            HttpInfo info = new HttpInfo();
            //info.Set(HttpMethod.GET, "/conversation", d);

            // info의 정보로 요청을 보내기
            HttpManager02.GetInstance().SendRequest(info);
        }
    }

    HttpInfo httpInfo;
    public void CreateHttpInfo()
    {
        httpInfo = new HttpInfo();
        httpInfo.httpMethod = HttpMethod.GET;
        httpInfo.url = "hello";
    }
}
