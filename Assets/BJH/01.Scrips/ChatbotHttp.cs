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
        // �ӽ�
        // 5�� ������ ê������ get��û�� ������.
        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            CreateHttpInfo();
            HttpManager02.GetInstance().SendRequest(httpInfo);
            HttpInfo info = new HttpInfo();
            //info.Set(HttpMethod.GET, "/conversation", d);

            // info�� ������ ��û�� ������
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
