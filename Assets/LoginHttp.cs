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
    // �̸���, ��й�ȣ�� �Է��ϰ� ���� ��ư�� ������ ��
    // �Է��� ������ JsonData�� �����ϰ�
    // ������ Get��û�� ������ ȸ������ ���θ� Ȯ�ι޴´�.

    public void TryLogin(string email, string pw)
    {
        // �ӽ�
        // ������ ������ �Ϸ�ȴٸ� �Ʒ� �ڵ� ����
        ConnectionManager03._instance.nickName = email;
        ConnectionManager03._instance.familyCode = pw;
        //OnBoardingManager._instance.completeLoginBoxEmpty.SetActive(true);
        OnBoardingManager._instance.faileLoginBox.SetActive(true);

        // �ӽ�
        // ������ �׽�Ʈ �� �� �ش� �Լ��� ���
        // CreateJsonData(email, pw);
    }

    public void CreateJsonData(string email, string password)
    {
        print("${email}�� json data�� ����ڽ��ϴ�.");
        EmailRequest emailRequest = new EmailRequest();

        emailRequest.email = email;
        emailRequest.password = password;

        string createdJsonData = JsonUtility.ToJson(emailRequest, true);
        print("$ù �α��� ��û���� ������� Json : {createdJsonData}");

        OnGetRequest(createdJsonData);
    }

    public void OnGetRequest(string s)
    {
        // ��û url
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
        // �������� ���� �����͸� ������ȭ
        EmailReponse emailReponse = new EmailReponse();
        emailReponse = JsonUtility.FromJson<EmailReponse>(result.text);

        // ���� ȸ���� ������?
        // ȸ���� ���ٸ� �α����� �����ߴٴ� �˾��� ���
        if(emailReponse.resultCode == "Fail")
        {
            Debug.Log("ȸ���� �������� �ʽ��ϴ�. ȸ�������� �����մϴ�.");
            OnBoardingManager._instance.faileLoginBox.SetActive(true);
        }

        // ȸ���� �ִٸ�?
        // �α����ϰ� �κ������ ����
        if(emailReponse.resultCode == "success")
        {
            Debug.Log("ȸ���� �����մϴ�. �α����մϴ�.");
            ConnectionManager03._instance.nickName = emailReponse.message.nickname;
            ConnectionManager03._instance.familyCode = emailReponse.message.islanedId;
        }
    }

    public void OnGetRequestFailed()
    {
        Debug.Log("��ſ� �����߽��ϴ�.");
    }
}
