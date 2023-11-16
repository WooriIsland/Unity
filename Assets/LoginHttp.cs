using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class  EmailRequest
{
    public string email;
    public string password;
}
public class EmailReponse
{
    public string resultCode;
    public Message message;
}

public class Message
{
    public int no;
    public string email;
    public string password;
    public string role;
    public string name;
    public string nickname;
    public string character;
    public string islanedId;
}

public class LoginHttp : MonoBehaviour
{
    // 이메일, 비밀번호를 입력하고 다음 버튼을 눌렀을 때
    // 입력한 정보로 JsonData를 생성하고
    // 서버에 Get요청을 보내서 회원존재 여부를 확인받는다.

    public void TryLogin(string email, string pw)
    {
        // 임시
        // 서버랑 연결이 완료된다면 아래 코드 삭제
        ConnectionManager03._instance.nickName = email;
        ConnectionManager03._instance.familyCode = pw;
        //OnBoardingManager._instance.completeLoginBoxEmpty.SetActive(true);
        OnBoardingManager._instance.faileLoginBox.SetActive(true);

        // 임시
        // 서버랑 테스트 할 때 해당 함수를 사용
        // CreateJsonData(email, pw);
    }

    public void CreateJsonData(string email, string password)
    {
        print("${email}로 json data를 만들겠습니다.");
        EmailRequest emailRequest = new EmailRequest();

        emailRequest.email = email;
        emailRequest.password = password;

        string createdJsonData = JsonUtility.ToJson(emailRequest, true);
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
        // 서버에게 받은 데이터를 역직렬화
        EmailReponse emailReponse = new EmailReponse();
        emailReponse = JsonUtility.FromJson<EmailReponse>(result.text);

        // 만약 회원이 없으면?
        // 회원이 없다면 로그인이 실패했다는 팝업을 띄움
        if(emailReponse.resultCode == "Fail")
        {
            Debug.Log("회원이 존재하지 않습니다. 회원가입을 시작합니다.");
            OnBoardingManager._instance.faileLoginBox.SetActive(true);
        }

        // 회원이 있다면?
        // 로그인하고 로비씬으로 입장
        if(emailReponse.resultCode == "success")
        {
            Debug.Log("회원이 존재합니다. 로그인합니다.");
            ConnectionManager03._instance.nickName = emailReponse.message.nickname;
            ConnectionManager03._instance.familyCode = emailReponse.message.islanedId;
        }
    }

    public void OnGetRequestFailed()
    {
        Debug.Log("통신에 실패했습니다.");
    }
}
