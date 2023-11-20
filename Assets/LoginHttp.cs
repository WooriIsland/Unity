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
    public int no; // ai ��ſ�
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
    // �̸���, ��й�ȣ�� �Է��ϰ� ���� ��ư�� ������ ��
    // �Է��� ������ JsonData�� �����ϰ�
    // ������ Get��û�� ������ ȸ������ ���θ� Ȯ�ι޴´�.

    public void TryLogin(string email, string pw)
    {
        // �ӽ�
        // ������ ������ �Ϸ�ȴٸ� �Ʒ� �ڵ� ����
        //ConnectionManager03._instance.nickName = "Dongsik";
        //ConnectionManager03._instance.familyCode = "Dongsik_Family";
        //OnBoardingManager._instance.completeLoginBoxEmpty.SetActive(true); // �ٷ� �α���
        //OnBoardingManager._instance.faileLoginBox.SetActive(true); // �α��� ���� -> ȸ������ ����

        // �ӽ�
        // ������ �׽�Ʈ �� �� �ش� �Լ��� ���
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
        // ��û url
        string url = "http://121.165.108.236:7070/api/v1/users/login";

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

    void OnGetRequestFailed(DownloadHandler result)
    {
        Debug.Log("��ſ� �����߽��ϴ�.");

        // �������� ���� �����͸� ������ȭ
        EmailReponse emailReponse = new EmailReponse();
        emailReponse = JsonUtility.FromJson<EmailReponse>(result.text);

        if(emailReponse.resultCode == "USER_NOT_FOUND")
        {
            OnBoardingManager.Instance.faileLoginBox.SetActive(true);
        }

        if (emailReponse.resultCode == "INVALID_PASSWORD")
        {
            OnBoardingManager.Instance.faileLoginBox.SetActive(true);
            print("�н����尡 �߸������� �ƹ�ư �α��� �ٽ� �ϴ��� ȸ������ �ٽ� �ϼ�");
        }
    }


    // �̸��Ϸ� ���� �ڵ� ����
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
        print("���� �̸����� �����߽��ϴ�.");
    }

    private void FailSendAuthEmail(DownloadHandler request)
    {
        print("���� �̸��� ������ �����߽��ϴ�.");
    }

    // ���� �ڵ�
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
        print("�̸��� ���� ����");

        // �̸��� ������ �����ߴٴ� UI
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

        // ��ư Ŭ���ϸ�
        // �̸��� ������ȣ ������
        

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
        print("ȸ�����Կ� �����߽��ϴ�!");

        // ȸ�����Կ� �����߽��ϴ� UI
        OnBoardingManager.Instance.CompleteSignUpBox.SetActive(true);
        
    }

    private void FaileSignUp(DownloadHandler result)
    {
        print("ȸ�����Կ� �����߽��ϴ�.");

        
    }

}
