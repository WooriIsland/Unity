using JetBrains.Annotations;
using System;
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
    public string islandUniqueNumber;
    public int no; // ai 통신용
    public string character;
}

[System.Serializable]
public class TokenDto
{
    public string accessToken;
    public string refreshToken;
}


[System.Serializable]
public class RequestSignUp
{
    public string email;
    public string password;
    public string nickName;
}

[System.Serializable]
public class RequestAuthEmailSend
{
    public string email;
}

[System.Serializable]
public class RequestAuthEmailCheck
{
    public string code;
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
        //ConnectionManager03._instance.nickName = "Dongsik";
        //ConnectionManager03._instance.familyCode = "Dongsik_Family";
        //OnBoardingManager._instance.completeLoginBoxEmpty.SetActive(true); // 바로 로그인
        //OnBoardingManager._instance.faileLoginBox.SetActive(true); // 로그인 실패 -> 회원가입 유도

        // 임시
        // 서버랑 테스트 할 때 해당 함수를 사용
        CreateJsonData(email, pw);
    }

    public void CreateJsonData(string email, string password)
    {
        LoginInfo loginInfo = new LoginInfo();

        loginInfo.email = email;
        loginInfo.password = password;

        string createdJsonData = JsonUtility.ToJson(loginInfo, true);

        OnGetRequest(createdJsonData);
    }

    public void OnGetRequest(string s)
    {
        // 요청 url
        string url = "http://121.165.108.236:7070/api/v1/users/login";

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
        if (emailReponse.resultCode == "USER_NOT_FOUND")
        {
            Debug.Log("회원이 존재하지 않습니다.");
            OnBoardingManager.Instance.faileLoginBox.SetActive(true);
        }

        // 회원이 있다면?
        // 로그인하고 로비씬으로 입장
        if(emailReponse.resultCode == "SUCCESS")
        {
            Debug.Log("회원이 존재합니다. 로그인합니다.");

            // 저장
            InfoManager.Instance.NickName = emailReponse.message.nickname;
            InfoManager.Instance.FamilyCode = emailReponse.message.islandUniqueNumber; // islandUniqueNumber == familyCode
            InfoManager.Instance.Character = emailReponse.message.character;
            InfoManager.Instance.accessToken = emailReponse.message.tokenDto.accessToken;
            InfoManager.Instance.refreshToken = emailReponse.message.tokenDto.refreshToken;


            // 스택 오버플로우
            //InfoManager.Instance.AcessToken = emailReponse.message.tokenDto.accessToken;
            //InfoManager.Instance.RefreshToken = emailReponse.message.tokenDto.refreshToken;



            // 로그인 완료 UI 표시
            OnBoardingManager.Instance.completeLoginBoxEmpty.SetActive(true);

            //ConnectionManager03._instance.nickName = emailReponse.message.nickname;
            //ConnectionManager03._instance.familyCode = emailReponse.message.islandUniqueNumber;

            
        }
    }

    void OnGetRequestFailed(DownloadHandler result)
    {
        Debug.Log("통신에 실패했습니다.");

        // 서버에게 받은 데이터를 역직렬화
        EmailReponse emailReponse = new EmailReponse();
        emailReponse = JsonUtility.FromJson<EmailReponse>(result.text);

        if(emailReponse.resultCode == "USER_NOT_FOUND")
        {
            OnBoardingManager.Instance.faileLoginBox.SetActive(true);
        }

        if (emailReponse.resultCode == "INVALID_PASSWORD")
        {
            OnBoardingManager.Instance.faileLoginBox.SetActive(true);
            print("패스워드가 잘못됐지만 아무튼 로그인 다시 하던가 회원가입 다시 하셈");
        }
    }


    // 이메일로 인증 코드 전송
    public void SendAuthEmail(string email)
    {
        RequestAuthEmailSend requestAuthEmail = new RequestAuthEmailSend();

        requestAuthEmail.email = email;
        string jsonData = JsonUtility.ToJson(requestAuthEmail, true);

        string url = "http://121.165.108.236:7070/api/v1/users/send-auth-email";

        HttpRequester_LHS requester = new HttpRequester_LHS();

        requester.SetUrl(RequestType.GET, url, false);

        requester.body = jsonData;
        requester.isJson = true;
        requester.isChat = false;

        requester.onComplete = CompleteSendAuthEmail;
        requester.onFailed = FailSendAuthEmail;

        HttpManager_LHS.instance.SendRequest(requester);
    }

    private void CompleteSendAuthEmail(DownloadHandler request)
    {
        print("인증 이메일을 전송했습니다.");
    }

    private void FailSendAuthEmail(DownloadHandler request)
    {
        print("인증 이메일 전송을 실패했습니다.");
    }

    // 인증 코드
    public void AuthEmailCheck(string code)
    {
        RequestAuthEmailCheck requestAuthEmailCheck = new RequestAuthEmailCheck();

        requestAuthEmailCheck.code = code;
        string jsonData = JsonUtility.ToJson(requestAuthEmailCheck, true);

        string url = "http://121.165.108.236:7070/api/v1/users/check-auth-email";

        HttpRequester_LHS requester = new HttpRequester_LHS();

        requester.SetUrl(RequestType.GET, url, false);

        requester.body = jsonData;
        requester.isJson = true;
        requester.isChat = false;

        requester.onComplete = CompleteAuthEmailCheck;
        requester.onFailed = FailAuthEmailCheck;

        HttpManager_LHS.instance.SendRequest(requester);
    }

    private void CompleteAuthEmailCheck(DownloadHandler request)
    {
        print("이메일 인증 성공");

        // 이메일 인증에 성공했다는 UI
    }

    private void FailAuthEmailCheck(DownloadHandler request)
    {
        print("이메일 인증 실패");

        // 이메일 인증에 실패했다는 UI

    }




    // 회원가입
    public void SignUp(string email, string password, string nickName)
    {
        RequestSignUp requestSignUp = new RequestSignUp();

        requestSignUp.email = email;

        // 버튼 클릭하면
        // 이메일 인증번호 보내기
        

        requestSignUp.password = password;
        requestSignUp.nickName = nickName;

        string jsonData = JsonUtility.ToJson(requestSignUp, true);

        string url = "http://121.165.108.236:7070/api/v1/users/join";

        HttpRequester_LHS requester = new HttpRequester_LHS();

        requester.SetUrl(RequestType.POST, url, false);
        requester.body = jsonData;
        requester.isJson = true;
        requester.isChat = false;

        requester.onComplete = CompleteSignUp;
        requester.onFailed = FaileSignUp;

        HttpManager_LHS.instance.SendRequest(requester);
    }
    private void CompleteSignUp(DownloadHandler result)
    {
        print("회원가입에 성공했습니다!");

        // 회원가입에 성공했습니다 UI
        OnBoardingManager.Instance.CompleteSignUpBox.SetActive(true);
        
    }

    private void FaileSignUp(DownloadHandler result)
    {
        print("회원가입에 실패했습니다.");

        
    }

}
