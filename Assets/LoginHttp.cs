using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

[System.Serializable]
public class EmailReponse
{
    public string resultCode;
    public Message message;
}

[System.Serializable]
public class Message
{
    public TokenDto tokenDto;
    public string nickname;
    public string islanedId;
    public int no; // ai 통신용
    public string character;


}

[System.Serializable]
public class TokenDto
{
    public string accessToken;
    public string refreshToken;
}

public class LoginHttp : MonoBehaviour
{
    // 테스트
    string jsonString = @"
        {
            ""resultCode"": ""SUCCESS"",
            ""message"": {
                ""tokenDto"": {
                    ""accessToken"": ""..."",
                    ""refreshToken"": ""...""
                },
                ""nickname"": ""string123"",
                ""islandId"": null,
                ""no"": 1,
                ""character"": null
            }
        }";
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            // 서버에게 받은 데이터를 역직렬화
            EmailReponse emailReponse = new EmailReponse();
            //emailReponse = JsonUtility.FromJson<EmailReponse>(result.text);
            emailReponse = JsonUtility.FromJson<EmailReponse>(jsonString);
            string test = emailReponse.message.nickname;
            print(test);
        }
    }

    // 이메일, 비밀번호를 입력하고 다음 버튼을 눌렀을 때
    // 입력한 정보로 JsonData를 생성하고
    // 서버에 Get요청을 보내서 회원존재 여부를 확인받는다.

    public void TryLogin(string email, string pw)
    {
        // 임시
        // 서버랑 연결이 완료된다면 아래 코드 삭제
        ConnectionManager03._instance.nickName = "Dongsik";
        ConnectionManager03._instance.familyCode = "Dongsik_Family";
        OnBoardingManager._instance.completeLoginBoxEmpty.SetActive(true); // 바로 로그인
        //OnBoardingManager._instance.faileLoginBox.SetActive(true); // 로그인 실패 -> 회원가입 유도

        // 임시
        // 서버랑 테스트 할 때 해당 함수를 사용
        //CreateJsonData(email, pw);
    }

    public void CreateJsonData(string email, string password)
    {
        print("${email}로 json data를 만들겠습니다.");
        LoginInfo loginInfo = new LoginInfo();

        loginInfo.email = email;
        loginInfo.password = password;

        string createdJsonData = JsonUtility.ToJson(loginInfo, true);
        print("$첫 로그인 요청으로 만들어진 Json : {createdJsonData}");

        print(loginInfo.email + " 확인 " + loginInfo.password);

        OnGetRequest(createdJsonData);
    }

    public void OnGetRequest(string s)
    {
        // 요청 url
        string url = "http://192.168.0.53:8082/api/v1/users/login";

        //HttpRequester_LHS requester = new HttpRequester_LHS();

        //requester.SetUrl(RequestType.GET, url, false);
        //requester.body = s;
        //requester.isJson = true;
        //requester.isChat = false;
        //requester.onComplete = OnGetRequestComplete;
        //requester.onFailed = OnGetRequestFailed;

        //HttpManager_LHS.instance.SendRequest(requester);

        HttpRequester_LHS requester = new HttpRequester_LHS();

        requester.SetUrl(RequestType.POST, url, false);
        requester.body = s; // json data
        requester.isJson = true;
        requester.isChat = false; // 이거 뭐지

        requester.onComplete = OnGetRequestComplete;
        requester.onFailed = OnGetRequestFailed;

        HttpManager_LHS.instance.SendRequest(requester);
    }


    void OnGetRequestComplete(DownloadHandler result)
    {
        print("성공");
        // 서버에게 받은 데이터를 역직렬화
        EmailReponse emailReponse = new EmailReponse();
        emailReponse = JsonUtility.FromJson<EmailReponse>(result.text);


        // 만약 회원이 없으면?
        // 회원이 없다면 로그인이 실패했다는 팝업을 띄움
        if (emailReponse.resultCode == "ERROR")
        {
            Debug.Log("회원이 존재하지 않습니다. 회원가입을 시작합니다.");
            OnBoardingManager._instance.faileLoginBox.SetActive(true);
        }

        // 회원이 있다면?
        // 로그인하고 로비씬으로 입장
        if(emailReponse.resultCode == "SUCCESS")
        {
            Debug.Log("회원이 존재합니다. 로그인합니다.");
            ConnectionManager03._instance.nickName = emailReponse.message.nickname;
            ConnectionManager03._instance.familyCode = emailReponse.message.islanedId;
        }
    }

    void OnGetRequestFailed()
    {
        Debug.Log("통신에 실패했습니다.");
    }
}
