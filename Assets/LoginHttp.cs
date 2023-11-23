using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
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
    public int userId;
    public string nickname;
    public string character;
    public string islandUniqueNumber;
    // public int no; // ai ��ſ�
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

public class LoginHttp : MonoBehaviour
{
    // �̸���, ��й�ȣ�� �Է��ϰ� ���� ��ư�� ������ ��
    // �Է��� ������ JsonData�� �����ϰ�
    // ������ Get��û�� ������ ȸ������ ���θ� Ȯ�ι޴´�.

    public void TryLogin(string email, string pw)
    {
        // �ӽ�
        //������ ������ �Ϸ�ȴٸ� �Ʒ� �ڵ� ����
        InfoManager.Instance.NickName = email;
        OnBoardingManager.Instance.completeLoginBoxEmpty.SetActive(true); // �ٷ� �α���



        //ConnectionManager03._instance.nickName = "Dongsik";
        //ConnectionManager03._instance.familyCode = "Dongsik_Family";
        //OnBoardingManager.Instance.completeLoginBoxEmpty.SetActive(true); // �ٷ� �α���
        //OnBoardingManager._instance.faileLoginBox.SetActive(true); // �α��� ���� -> ȸ������ ����

        // �ӽ�
        // ������ �׽�Ʈ �� �� �ش� �Լ��� ���
        //CreateJsonData(email, pw);
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
        // ��û url
        string url = "http://121.165.108.236:7071/api/v1/users/login";

        HttpRequester_LHS requester = new HttpRequester_LHS();

        requester.SetUrl(RequestType.POST, url, false);
        requester.body = s; // json data
        requester.isJson = true;
        requester.isChat = false; // �̰� ����

        requester.onComplete = OnGetRequestComplete;
        requester.onFailed = OnGetRequestFailed;

        HttpManager_LHS.instance.SendRequest(requester);
    }


    void OnGetRequestComplete(DownloadHandler result)
    {
        print("����");
        // �������� ���� �����͸� ������ȭ
        EmailReponse emailReponse = new EmailReponse();
        emailReponse = JsonUtility.FromJson<EmailReponse>(result.text);
        print(result.text);


        // ���� ȸ���� ������?
        // ȸ���� ���ٸ� �α����� �����ߴٴ� �˾��� ���
        if (emailReponse.resultCode == "USER_NOT_FOUND")
        {
            Debug.Log("ȸ���� �������� �ʽ��ϴ�.");
            OnBoardingManager.Instance.faileLoginBox.SetActive(true);
        }

        // ȸ���� �ִٸ�?
        // �α����ϰ� �κ������ ����
        if(emailReponse.resultCode == "SUCCESS")
        {
            Debug.Log("ȸ���� �����մϴ�. �α����մϴ�.");

            // ����
            InfoManager.Instance.NickName = emailReponse.message.nickname;
            InfoManager.Instance.FamilyCode = emailReponse.message.islandUniqueNumber; // islandUniqueNumber == familyCode
            InfoManager.Instance.Character = emailReponse.message.character;
            InfoManager.Instance.accessToken = emailReponse.message.tokenDto.accessToken;
            InfoManager.Instance.refreshToken = emailReponse.message.tokenDto.refreshToken;


            // ���� �����÷ο�
            //InfoManager.Instance.AcessToken = emailReponse.message.tokenDto.accessToken;
            //InfoManager.Instance.RefreshToken = emailReponse.message.tokenDto.refreshToken;



            // �α��� �Ϸ� UI ǥ��
            OnBoardingManager.Instance.completeLoginBoxEmpty.SetActive(true);

            //ConnectionManager03._instance.nickName = emailReponse.message.nickname;
            //ConnectionManager03._instance.familyCode = emailReponse.message.islandUniqueNumber;

            
        }
    }

    // {"resultCode":"ERROR","message": {"errorCode":"USER_NOT_FOUNDED","message":null}}
void OnGetRequestFailed(DownloadHandler result)
    {
        Debug.Log("�α��� �� �� �����ϴ�.");

        // JObject�� Ŭ����, ����ü ���� Ű, �� ��������
        JObject data = JObject.Parse(result.text);
        JObject json = data.ToObject<JObject>();
        JObject message = json["message"].ToObject<JObject>();
        string errorCode = message["errorCode"].ToObject<string>();

        if(errorCode == "USER_NOT_FOUNDED" || errorCode == "INVALID_PASSWORD")
        {
            print("�α��� ���� UI�� �մϴ�.");
            OnBoardingManager.Instance.faileLoginBox.SetActive(true);
        }
    }


    // �̸��Ϸ� ���� �ڵ� ����
    public void SendAuthEmail(string email)
    {
        RequestAuthEmailSend requestAuthEmail = new RequestAuthEmailSend();

        requestAuthEmail.email = email;
        print(email);
        string jsonData = JsonUtility.ToJson(requestAuthEmail, true);

        string url = "http://121.165.108.236:7071/api/v1/users/send-auth-email";
        //string url = "http://192.168.0.104:8080/users/send-auth-email";

        HttpRequester_LHS requester = new HttpRequester_LHS();

        requester.SetUrl(RequestType.POST, url, false);

        requester.body = jsonData;
        requester.isJson = true;
        requester.isChat = false;

        requester.onComplete = CompleteSendAuthEmail;
        requester.onFailed = FailSendAuthEmail;

        HttpManager_LHS.instance.SendRequest(requester);
    }

    private void CompleteSendAuthEmail(DownloadHandler request)
    {
        print("���� �̸����� �����߽��ϴ�.");
    }

    private void FailSendAuthEmail(DownloadHandler request)
    {
        //JObject data = JObject.Parse(request.text);
        //JObject json = data.ToObject<JObject>();
        

    }

    // ���� �ڵ�
    public void AuthEmailCheck(string code)
    {
        RequestAuthEmailCheck requestAuthEmailCheck = new RequestAuthEmailCheck();

        requestAuthEmailCheck.code = code;
        string jsonData = JsonUtility.ToJson(requestAuthEmailCheck, true);

        string url = "http://121.165.108.236:7071/api/v1/users/check-auth-email";
        //string url = "http://192.168.0.104:8080/users/check-auth-email";


        HttpRequester_LHS requester = new HttpRequester_LHS();

        requester.SetUrl(RequestType.POST, url, false);

        requester.body = jsonData;
        requester.isJson = true;
        requester.isChat = false;

        requester.onComplete = CompleteAuthEmailCheck;
        requester.onFailed = FailAuthEmailCheck;

        HttpManager_LHS.instance.SendRequest(requester);
    }

    private void CompleteAuthEmailCheck(DownloadHandler request)
    {
        print("�̸��� ���� ����");

        OnBoardingManager.Instance.authEmailBoxEmpty.SetActive(false);
    }

    private void FailAuthEmailCheck(DownloadHandler request)
    {
        print("�̸��� ���� ����");

        // �̸��� ������ �����ߴٴ� UI

    }




    // ȸ������
    public void SignUp(string email, string password, string nickName)
    {
        RequestSignUp requestSignUp = new RequestSignUp();

        requestSignUp.email = email;

        requestSignUp.password = password;
        requestSignUp.nickname = nickName;

        print(requestSignUp.ToString());

        string jsonData = JsonUtility.ToJson(requestSignUp, true);

        string url = "http://121.165.108.236:7071/api/v1/users/join";

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
        print("ȸ�����Կ� �����߽��ϴ�!");

        // ȸ�����Կ� �����߽��ϴ� UI
        OnBoardingManager.Instance.CompleteSignUpBox.SetActive(true);
        
    }

    private void FaileSignUp(DownloadHandler result)
    {
        print("ȸ�����Կ� �����߽��ϴ�.");
    }
}
