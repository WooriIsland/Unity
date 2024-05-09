using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

#region 참고용
//게시물 정보
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
    *      "name":"김현진",
    *      "id":"rapa_xr",
    *      "email":"lokimve7@naver.com",
    *      "age":20
    * }
    */
}
#endregion


// HTTP 요청을 위한 URL, Request Type, Action함수를 초기화하는 클래스
public class HttpRequester : MonoBehaviour
{
    //요청 타입 (GET, POST, PUT, DELETE)
    public Define.RequestType requestType;
    //URL
    public string url;
    //Post Data 정보를 넣어서 보내주세요
    public string body = "{}";

    //응답이 왔을 때 호출해줄 함수 (Action)
    //Action : 함수를 넣을 수 있는 자료형
    public Action<DownloadHandler> onComplete;
    public Action<DownloadHandler> onFailed;

    // 변지환
    // data type을 설정하여 그에 맞게 HttpManager에서 통신을 진행
    // 프로퍼티로 접근
    private bool _isJson;
    private bool _isChat;
    private bool _isPhoto;
    private bool _isNet;

    public bool IsJson {  get { return _isJson; } }
    public bool IsChat { get { return _isChat; } }

    // 변지환
    // 현숙의 앨범 bool값을 직접 사요하기 위해 IsPhoton만 setter를 제작함
    public bool IsPhoto { get { return _isPhoto;} set { _isPhoto = value; } }
    public bool IsNet { get { return _isNet; } }

    // request type, url 초기화하는 메서드
    public void SetUrl(Define.RequestType type, Define.DataType dataType, string strUrl, bool bUseCommonUrl = true)
    {
        // 요청 타입
        requestType = type;

        // 변지환
        // data type 설정
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

        // 회원가입 / 아이디 / 이메일 / 닉네임 / 로그인 주소가 같음
        if (bUseCommonUrl) url = "http://remembermebackend-env.eba-dcctnmvk.ap-northeast-2.elasticbeanstalk.com";
        url += strUrl;
    }
  
    //성공
    public void OnComplete(DownloadHandler result)
    {
        if (onComplete != null) onComplete(result);
    }

    // 실패
    public void OnFailed(DownloadHandler result)
    {
        if (onFailed != null) onFailed(result);
    }
}


