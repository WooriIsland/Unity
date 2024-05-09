using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

#region �����
//�Խù� ����
[Serializable]
public class PostData
{
    public int userId;
    public int id;
    public string title;
    public string body;
}

[Serializable]
public class PostDataArray
{
    public List<PostData> data;
}

public class UserData
{
    public string name;
    public string id;
    public string email;
    public int age;

    /*
    * {
    *      "name":"������",
    *      "id":"rapa_xr",
    *      "email":"lokimve7@naver.com",
    *      "age":20
    * }
    */
}
#endregion


// HTTP ��û�� ���� URL, Request Type, Action�Լ��� �ʱ�ȭ�ϴ� Ŭ����
public class HttpRequester : MonoBehaviour
{
    //��û Ÿ�� (GET, POST, PUT, DELETE)
    public Define.RequestType requestType;
    //URL
    public string url;
    //Post Data ������ �־ �����ּ���
    public string body = "{}";

    //������ ���� �� ȣ������ �Լ� (Action)
    //Action : �Լ��� ���� �� �ִ� �ڷ���
    public Action<DownloadHandler> onComplete;
    public Action<DownloadHandler> onFailed;

    // ����ȯ
    // data type�� �����Ͽ� �׿� �°� HttpManager���� ����� ����
    // ������Ƽ�� ����
    private bool _isJson;
    private bool _isChat;
    private bool _isPhoto;
    private bool _isNet;

    public bool IsJson {  get { return _isJson; } }
    public bool IsChat { get { return _isChat; } }

    // ����ȯ
    // ������ �ٹ� bool���� ���� ����ϱ� ���� IsPhoton�� setter�� ������
    public bool IsPhoto { get { return _isPhoto;} set { _isPhoto = value; } }
    public bool IsNet { get { return _isNet; } }

    // request type, url �ʱ�ȭ�ϴ� �޼���
    public void SetUrl(Define.RequestType type, Define.DataType dataType, string strUrl, bool bUseCommonUrl = true)
    {
        // ��û Ÿ��
        requestType = type;

        // ����ȯ
        // data type ����
        switch(dataType)
        {
            case Define.DataType.JSON:
                _isJson = true;
                break;
            case Define.DataType.CHAT:
                _isChat = true;
                break;
            case Define.DataType.PHOTO:
                _isPhoto = true;
                break;
            case Define.DataType.NET:
                _isNet = true;
            break;
            case Define.DataType.NONE:
            break;
        }

        // ȸ������ / ���̵� / �̸��� / �г��� / �α��� �ּҰ� ����
        if (bUseCommonUrl) url = "http://remembermebackend-env.eba-dcctnmvk.ap-northeast-2.elasticbeanstalk.com";
        url += strUrl;
    }
  
    //����
    public void OnComplete(DownloadHandler result)
    {
        if (onComplete != null) onComplete(result);
    }

    // ����
    public void OnFailed(DownloadHandler result)
    {
        if (onFailed != null) onFailed(result);
    }
}


