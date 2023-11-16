using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public enum HttpMethod
{
    GET,
    POST,
    PUT,
    DELETE
}

// ������� ���� ������ ���� class
public class HttpInfo
{
    public HttpMethod httpMethod;
    public string url;
    public Action<DownloadHandler> onReceive;
    public string body;

    public void Set(HttpMethod h, string u, Action<DownloadHandler> d)
    {
        httpMethod = h;
        url = "http://172.17.113.213:5011/docs/api/chatbot/" + u;
        onReceive = d;
    }
}

// POST
[Serializable]
public class ChatInfo
{
    public string island_id;
    public string user_id;
    public string content;
    public string datetiem; // �̰� �ڷ����� ����
}

[Serializable]
public class LoginInfo
{
    public string id;
    public string pw;
}



public class HttpManager02 : MonoBehaviour
{
    private static HttpManager02 instance;

    public static HttpManager02 GetInstance()
    {
        if(instance == null)
        {
            GameObject go = new GameObject("HttpManager");
            go.AddComponent<HttpManager02>();
        }

        return instance;
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(instance);
        }
    }

    // Rest API���� ��û(GET, DELETE, PUT, POST)
    public void SendRequest(HttpInfo httpInfo)
    {
        StartCoroutine(CoSendRequest(httpInfo));
    }

    IEnumerator CoSendRequest(HttpInfo httpInfo)
    {
        // �ε��� ����(��Ÿ)

        UnityWebRequest request = null;

        switch (httpInfo.httpMethod)
        {
            case HttpMethod.GET:
                request = UnityWebRequest.Get(httpInfo.url);
                break;

            case HttpMethod.POST:
                request = UnityWebRequest.Post(httpInfo.url, httpInfo.body);

                // bite �迭�� ����
                byte[] byteBody = Encoding.UTF8.GetBytes(httpInfo.body);
                request.uploadHandler = new UploadHandlerRaw(byteBody);

                // header ����
                // key - value
                // ��� ������ �߰����ּ����� api server�� �˷���
                request.SetRequestHeader("Content-Type", "application/json");
                //request.SetRequestHeader("Content-Type", "application/json"); ����� �������� �Ʒ��� �߰�

                break;
            case HttpMethod.PUT:
                request = UnityWebRequest.Put(httpInfo.url, httpInfo.body);

                break;
            case HttpMethod.DELETE:
                request = UnityWebRequest.Get(httpInfo.url);

                break;

                
        }

        yield return request.SendWebRequest();

        if(request.result == UnityWebRequest.Result.Success)
        {
            print("network ok : " + request.downloadHandler.text);

            if(httpInfo.onReceive != null) // onReceive�� �޼��尡 ������
            {
                httpInfo.onReceive(request.downloadHandler);
            }

        }
        else
        {
            print("network error : " + request.error);
        }

        // �ε��� ��(��Ÿ)
    }
}
