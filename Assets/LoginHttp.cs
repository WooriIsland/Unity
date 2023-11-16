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
    public int no; // ai ��ſ�
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
    // �׽�Ʈ
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
            // �������� ���� �����͸� ������ȭ
            EmailReponse emailReponse = new EmailReponse();
            //emailReponse = JsonUtility.FromJson<EmailReponse>(result.text);
            emailReponse = JsonUtility.FromJson<EmailReponse>(jsonString);
            string test = emailReponse.message.nickname;
            print(test);
        }
    }

    // �̸���, ��й�ȣ�� �Է��ϰ� ���� ��ư�� ������ ��
    // �Է��� ������ JsonData�� �����ϰ�
    // ������ Get��û�� ������ ȸ������ ���θ� Ȯ�ι޴´�.

    public void TryLogin(string email, string pw)
    {
        // �ӽ�
        // ������ ������ �Ϸ�ȴٸ� �Ʒ� �ڵ� ����
        ConnectionManager03._instance.nickName = "Dongsik";
        ConnectionManager03._instance.familyCode = "Dongsik_Family";
        OnBoardingManager._instance.completeLoginBoxEmpty.SetActive(true); // �ٷ� �α���
        //OnBoardingManager._instance.faileLoginBox.SetActive(true); // �α��� ���� -> ȸ������ ����

        // �ӽ�
        // ������ �׽�Ʈ �� �� �ش� �Լ��� ���
        //CreateJsonData(email, pw);
    }

    public void CreateJsonData(string email, string password)
    {
        print("${email}�� json data�� ����ڽ��ϴ�.");
        LoginInfo loginInfo = new LoginInfo();

        loginInfo.email = email;
        loginInfo.password = password;

        string createdJsonData = JsonUtility.ToJson(loginInfo, true);
        print("$ù �α��� ��û���� ������� Json : {createdJsonData}");

        print(loginInfo.email + " Ȯ�� " + loginInfo.password);

        OnGetRequest(createdJsonData);
    }

    public void OnGetRequest(string s)
    {
        // ��û url
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
        if (emailReponse.resultCode == "ERROR")
        {
            Debug.Log("ȸ���� �������� �ʽ��ϴ�. ȸ�������� �����մϴ�.");
            OnBoardingManager._instance.faileLoginBox.SetActive(true);
        }

        // ȸ���� �ִٸ�?
        // �α����ϰ� �κ������ ����
        if(emailReponse.resultCode == "SUCCESS")
        {
            Debug.Log("ȸ���� �����մϴ�. �α����մϴ�.");
            ConnectionManager03._instance.nickName = emailReponse.message.nickname;
            ConnectionManager03._instance.familyCode = emailReponse.message.islanedId;
        }
    }

    void OnGetRequestFailed()
    {
        Debug.Log("��ſ� �����߽��ϴ�.");
    }
}
