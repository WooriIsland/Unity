using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LoginRequest
{

}

public class LoginHttp : MonoBehaviour
{
    // 이메일, 비밀번호를 입력하고 다음 버튼을 눌렀을 때
    // 입력한 정보로 JsonData를 생성하고
    // 서버에 Get요청을 보내서 회원존재 여부를 확인받는다.
    public OnBoardingManager loginInput;
    

    public void OnclickLoginBtn()
    {
        string id = loginInput.id.text;

        CreateJsonData(id);
    }

    public void CreateJsonData(string id)
    {
        LoginInfo loginInfo = new LoginInfo();

        loginInfo.id = id;

        string createdJsonData = JsonUtility.ToJson(loginInfo, true);
        print("$첫 로그인 요청으로 만들어진 Json : {createdJsonData}");

        OnGetRequest(createdJsonData);
    }

    public void OnGetRequest(string s)
    {
        // 요청 url
        string url = "http://192.169.0.53:8080/api/v1/users";  

        HttpRequester_LHS request = new HttpRequester_LHS();

        request.SetUrl(RequestType.GET, url, false);
        request.body = s;
        request.isJson = true;
        request.isChat = false;
        request.onComplete = OnGetRequestComplete;
        request.onFailed = OnGetRequestFailed;

        HttpManager_LHS.instance.SendRequest(request);
    }


    public void OnGetRequestComplete(DownloadHandler result)
    {
        // 만약 회원이 없으면?
        // 회원가입 시작
        

        // 회원이 있다면?
        // 로그인하고 로비씬으로 입장
    }

    public void OnGetRequestFailed()
    {

    }
}
