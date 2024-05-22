using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;
using static Define;

#region request, response class
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
    public int userId;
    public string nickname;
    public string character;
    public string islandUniqueNumber;
    public int islandID;
    // public int no; // ai 통신용
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
    public string nickname;
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
#endregion

// 변지환
// 로그인 통신을 진행하는 클래스
public class LoginHttp : MonoBehaviour
{
    // 임시
    public OnBoardingInfo onBoardingInfo;



    // 이메일, 비밀번호를 입력하고 다음 버튼을 눌렀을 때
    // 입력한 정보로 JsonData를 생성하고
    // 서버에 Get요청을 보내서 회원존재 여부를 확인받는다.

    #region 로그인 통신
    public void TryLogin(string email, string pw)
    {
        // 서버가 없을 때 사용하는 코드
        //서버랑 연결이 완료된다면 아래 코드 삭제
        //InfoManager.Instance.NickName = email;
        //OnBoardingManager.Instance.completeLoginBoxEmpty.SetActive(true); // 바로 로그인
        //ConnectionManager03._instance.nickName = "Dongsik";
        //ConnectionManager03._instance.familyCode = "Dongsik_Family";
        //OnBoardingManager.Instance.completeLoginBoxEmpty.SetActive(true); // 바로 로그인
        //OnBoardingManager._instance.faileLoginBox.SetActive(true); // 로그인 실패 -> 회원가입 유도

        // 로그인을 위한 데이터 생성
        CreateJsonData(email, pw);
    }

    public void CreateJsonData(string email, string password)
    {
        LoginInfo loginInfo = new LoginInfo();

        loginInfo.email = email;
        loginInfo.password = password;

        // json data 생성
        string createdJsonData = JsonUtility.ToJson(loginInfo, true);

        OnGetRequest(createdJsonData);
    }

    public void OnGetRequest(string s)
    {
        // 요청 url
        string url = "http://3.35.234.195:7070/api/v1/users/login";

        // requester로 통신 준비하기
        HttpRequester requester = new HttpRequester(RequestType.POST, Define.DataType.JSON, url, false);
        requester._body = s; // json data
        requester._onComplete = OnGetRequestComplete;
        requester._onFailed = OnGetRequestFailed;

        // 통신 시작
        HttpManager.Instance.SendRequest(requester);
    }


    // 임시 : 로그인
    // 로그인 서버 닫혀서 임시로 작성하는 코드
    public void OnGetRequest_Test()
    {
        Managers.Info.NickName = onBoardingInfo.name;

        SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.SFX_BtnSearch);

        //현숙 애니메이션
        OnBoardingManager.Instance.signInBox.GetComponent<BasePopup>().CloseAction();
    }


    void OnGetRequestComplete(DownloadHandler result)
    {
        print("성공");
        // 서버에게 받은 데이터를 역직렬화
        EmailReponse emailReponse = new EmailReponse();
        emailReponse = JsonUtility.FromJson<EmailReponse>(result.text);
        print(result.text);


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
            Managers.Info.NickName = emailReponse.message.nickname;
            Managers.Info.FamilyCode = emailReponse.message.islandUniqueNumber; // islandUniqueNumber == familyCode
            Managers.Info.Character = emailReponse.message.character;
            Managers.Info.accessToken = emailReponse.message.tokenDto.accessToken;
            Managers.Info.refreshToken = emailReponse.message.tokenDto.refreshToken;
            Managers.Info.userId = emailReponse.message.userId;
            Managers.Info.isIslandUniqueNumber = emailReponse.message.islandUniqueNumber;
            Managers.Info.islandId = emailReponse.message.islandID;


            // 스택 오버플로우
            //InfoManager.Instance.AcessToken = emailReponse.message.tokenDto.accessToken;
            //InfoManager.Instance.RefreshToken = emailReponse.message.tokenDto.refreshToken;



            // 로그인 완료 UI 표시
            //OnBoardingManager.Instance.completeLoginBoxEmpty.SetActive(true);

            SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.SFX_BtnSearch);

            //현숙 애니메이션
            OnBoardingManager.Instance.signInBox.GetComponent<BasePopup>().CloseAction();
            

            //ConnectionManager03._instance.nickName = emailReponse.message.nickname;
            //ConnectionManager03._instance.familyCode = emailReponse.message.islandUniqueNumber

        }
    }

    // {"resultCode":"ERROR","message": {"errorCode":"USER_NOT_FOUNDED","message":null}}
void OnGetRequestFailed(DownloadHandler result)
    {
        Debug.Log("로그인 할 수 없습니다.");

        // JObject로 클래스, 구조체 없이 키, 값 가져오기
        JObject data = JObject.Parse(result.text);
        JObject json = data.ToObject<JObject>();
        JObject message = json["message"].ToObject<JObject>();
        string errorCode = message["errorCode"].ToObject<string>();

        if(errorCode == "USER_NOT_FOUNDED" || errorCode == "INVALID_PASSWORD")
        {
            print("로그인 실패 UI를 켭니다.");
            OnBoardingManager.Instance.faileLoginBox.SetActive(true);
        }
    }
    #endregion


    #region 이메일 인증

    // 이메일로 인증 코드 전송
    public void SendAuthEmail(string email)
    {
        RequestAuthEmailSend requestAuthEmail = new RequestAuthEmailSend();

        requestAuthEmail.email = email;
        print(email);
        string jsonData = JsonUtility.ToJson(requestAuthEmail, true);

        string url = "http://3.35.234.195:7070/api/v1/users/send-auth-email";
        //string url = "http://192.168.0.104:8080/users/send-auth-email";

        HttpRequester requester = new HttpRequester(Define.RequestType.POST, Define.DataType.JSON, url, false);
        requester._body = jsonData;
        requester._onComplete = CompleteSendAuthEmail;
        requester._onFailed = FailSendAuthEmail;

        HttpManager.Instance.SendRequest(requester);
    }

    private void CompleteSendAuthEmail(DownloadHandler request)
    {
        print("인증 이메일을 전송했습니다.");
    }

    private void FailSendAuthEmail(DownloadHandler request)
    {
        //JObject data = JObject.Parse(request.text);
        //JObject json = data.ToObject<JObject>();
        

    }

    // 인증 코드
    public void AuthEmailCheck(string code)
    {
        RequestAuthEmailCheck requestAuthEmailCheck = new RequestAuthEmailCheck();

        requestAuthEmailCheck.code = code;
        string jsonData = JsonUtility.ToJson(requestAuthEmailCheck, true);

        string url = "http://3.35.234.195:7070/api/v1/users/check-auth-email";
        //string url = "http://192.168.0.104:8080/users/check-auth-email";


        HttpRequester requester = new HttpRequester(RequestType.POST, DataType.JSON, url, false);
        requester._body = jsonData;
        requester._onFailed = FailAuthEmailCheck;

        HttpManager.Instance.SendRequest(requester);
    }

    private void CompleteAuthEmailCheck(DownloadHandler request)
    {
        Debug.Log("이메일 인증 성공");
        OnBoardingManager.Instance.authEmailBoxEmpty.SetActive(false);
    }

    private void FailAuthEmailCheck(DownloadHandler request)
    {
        Debug.Log("이메일 인증 실패");

    }
    #endregion


    #region 회원가입
    // 회원가입
    public void SignUp(string email, string password, string nickName)
    {
        RequestSignUp requestSignUp = new RequestSignUp();

        requestSignUp.email = email;

        requestSignUp.password = password;
        requestSignUp.nickname = nickName;

        print(requestSignUp.ToString());

        string jsonData = JsonUtility.ToJson(requestSignUp, true);

        string url = "http://3.35.234.195:7070/api/v1/users/join";

        HttpRequester requester = new HttpRequester(RequestType.POST, DataType.JSON, url, false);
        requester._body = jsonData;
        requester._onComplete = CompleteSignUp;
        requester._onFailed = FaileSignUp;

        HttpManager.Instance.SendRequest(requester);
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
    #endregion
}
