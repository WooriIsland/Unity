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
    public Define.RequestType _requestType;
    //URL
    public string _url;
    //Post Data ������ �־ �����ּ���
    public string _body = "{}";

    //������ ���� �� ȣ������ �Լ� (Action)
    //Action : �Լ��� ���� �� �ִ� �ڷ���
    public Action<DownloadHandler> _onComplete;
    public Action<DownloadHandler> _onFailed;

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

    // ������
    public HttpRequester(Define.RequestType type, Define.DataType dataType, string strUrl, bool bUseCommonUrl = true)
    {
        // ��û Ÿ��
        _requestType = type;

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
        if (bUseCommonUrl) _url = "http://remembermebackend-env.eba-dcctnmvk.ap-northeast-2.elasticbeanstalk.com";
        _url += strUrl;
    }

  
    //����
    public void OnComplete(DownloadHandler result)
    {
        if (_onComplete != null) _onComplete(result);
    }

    // ����
    public void OnFailed(DownloadHandler result)
    {
        if (_onFailed != null) _onFailed(result);
    }
}


