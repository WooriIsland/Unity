using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LoginRequest
{

}

public class LoginHttp : MonoBehaviour
{
    // �̸���, ��й�ȣ�� �Է��ϰ� ���� ��ư�� ������ ��
    // �Է��� ������ JsonData�� �����ϰ�
    // ������ Get��û�� ������ ȸ������ ���θ� Ȯ�ι޴´�.
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
        // ���� ȸ���� ������?
        // ȸ������ ����
        

        // ȸ���� �ִٸ�?
        // �α����ϰ� �κ������ ����
    }

    public void OnGetRequestFailed()
    {

    }
}
