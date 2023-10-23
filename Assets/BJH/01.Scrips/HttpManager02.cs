using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HttpManager02 : MonoBehaviour
{
    private static HttpManager02 instance;

    public enum RequestType
    {
        GET,
        POST,
        PUT,
        DELET
    }

    public class HttpInto
    {
        
    }

    public static HttpManager02 Get()
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    string url = "192.168.0.23:8080/api/chatbot/conversation";

    // Rest API���� ��û(GET, DELETE, PUT, POST)
    public void SendRequest()
    {
        StartCoroutine(CoSendRequest());
    }

    IEnumerator CoSendRequest()
    {
        // �ε��� ����(��Ÿ)

        UnityWebRequest request = null;

        request = UnityWebRequest.Get(url);


        yield return request.SendWebRequest();

        if(request.result == UnityWebRequest.Result.Success)
        {
            print("network ok : " + request.downloadHandler.text);

        }
        else
        {
            print("network error : " + request.error);
        }

        // �ε��� ��(��Ÿ)
    }
}
