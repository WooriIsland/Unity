using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Photon.Pun;

//로그인 성공 시 받는 값
[System.Serializable]
public class HttpGetData
{
    public Results results;
}

[System.Serializable]
public class Results
{
    public string nickname;
    public string username;
    public string token;
}

//캐릭터 커스텀 성공 시 받는 값
[System.Serializable]
public class HttpCharacterData
{
    public characterResults results;
}

[System.Serializable]
public class characterResults
{
    public string nickName;
    public int gender;
    public int hairNum;
    public int jacketNum;
    public int chestNum;
    public int tieNum;
    public int legsNum;
    public int feetNum;
}

//챗봇 성공 시 받는 값
[System.Serializable]
public class HttpChatBotData
{
    public chatBotBody results;
}

[System.Serializable]
public class chatBotBody
{
    public chatBotResults body;
}

[System.Serializable]
public class chatBotResults
{
    public string response;
}

// 보이스 성공 시 받는 값
//챗봇 성공 시 받는 값
[System.Serializable]
public class HttpChatVoiceData
{
    public chatVoiceBody results;
}

[System.Serializable]
public class chatVoiceBody
{
    public chatVoiceResults voice;
}

[System.Serializable]
public class chatVoiceResults
{
    public string body;
}

public enum RequestType
{
    GET,
    POST,
    PUT,
    DELETE,
    TEXTURE,
}

[System.Serializable]
public class ChatData
{
    public string response;
}

public class HttpManager_LHS : MonoBehaviourPun
{
    //싱글톤으로 만드는 이유 = 하나만 존재하기 위해서
    public static HttpManager_LHS instance;

    List<HttpRequester_LHS> requesters = new List<HttpRequester_LHS>();

    public string token = "";

    public string username = "";

    public string nickname = "";

    public bool kks = true;

    //채팅통신이랑 구분되게 해야함 -> 나중에 보고 삭제 해도 됨
    public bool isAichat = true;

    //사진 로딩화면
    public bool isPhoto = false;
    public bool isNet = false;
    //다른 통신들 로딩화면
    public GameObject mainLoding;

    private void Awake()
    {
        mainLoding.SetActive(false);

        //만약에 instance에 값이 없다면(HttpManager가 하나도 생성되지 않았다면)
        if (instance == null)
        {
            //instance에 나 자신을 넣는다.
            instance = this;

            //씬이 바껴도 파괴되지 않게 한다.
            DontDestroyOnLoad(gameObject);
        }

        //만약에 instance에 값이 있다면(이미 만들어진 HttpManager가 존재 한다면)
        else
        {
            print("중복으로 생성한다! 파괴하라!");
            //파괴하자
            Destroy(gameObject);

        }
    }

    Coroutine co;

    //서버에게 요청
    public void SendRequest(HttpRequester_LHS requester)
    {
        print(nameof(SendRequest));
        co = StartCoroutine(SendProcess(requester));
    }


    //2가지 정보를 요청 해달라고 파라미터 값으로 던져줘야함
    IEnumerator SendProcess(HttpRequester_LHS requester)
    {

        //처음 셋팅할 때 아무것도 없다 -> 요청종류에 따라서 다르게
        UnityWebRequest request = null;

        //requestType 에 따라 request를 다르게 셋팅해야 한다.
        switch (requester.requestType)
        {
            case RequestType.GET:

                //loding.SetActive(true);
                //back.SetActive(true);

                request = UnityWebRequest.Get(requester.url);

                //byte[] jsonToGet = new UTF8Encoding().GetBytes(requester.body);
                //request.uploadHandler = new UploadHandlerRaw(jsonToGet);
                request.SetRequestHeader("Content-Type", "application/json");
                if (requester.isChat)
                {
                    //채팅시에만 넣어서 보낼 수 있게 해야함
                    //로그인이랑 회원가입(중복확인) 제외한 모든 곳에 헤더에 토큰이 들어가야 함!
                    //Authorization : Bearer {token} 
                    request.SetRequestHeader("Authorization", "Bearer" + token);
                    print("보내짐");
                }
                break;
            case RequestType.POST:

                if(requester.isPhoto == true)
                {
                    PhotoManager.instance.loding.GetComponent<AlphaGPSSet>().OpenAlpha();
                    //SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.SFX_LodingCat);
                }

                else if(requester.isNet == true)
                {
                    mainLoding.GetComponent<AlphaGPSSet>().OpenAlpha();
                }

                print("body : " + requester.body); // body값 josn으로 출력
                request = UnityWebRequest.Post(requester.url, requester.body);

                // body데이터를 바이트로 변환
                byte[] jsonToSend = new UTF8Encoding().GetBytes(requester.body);

                request.uploadHandler.Dispose();
                request.uploadHandler = new UploadHandlerRaw(jsonToSend);

                if (requester.isJson)
                {
                    request.SetRequestHeader("Content-Type", "application/json"); // 헤더
                }

                if (requester.isChat)
                {
                    //채팅시에만 넣어서 보낼 수 있게 해야함
                    //로그인이랑 회원가입(중복확인) 제외한 모든 곳에 헤더에 토큰이 들어가야 함!
                    //Authorization : Bearer {token} 
                    request.SetRequestHeader("Authorization", "Bearer" + token);
                    print("보내짐");
                }
                break;

            case RequestType.PUT:

                if (requester.isPhoto == true)
                {
                    PhotoManager.instance.loding.GetComponent<AlphaGPSSet>().OpenAlpha();
                    //SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.SFX_LodingCat);
                    //시작시 사운드
                }

                else if (requester.isNet == true)
                {
                    mainLoding.GetComponent<AlphaGPSSet>().OpenAlpha();
                }

                request = UnityWebRequest.Put(requester.url, requester.body);
                byte[] jsonToPut = new UTF8Encoding().GetBytes(requester.body);

                request.uploadHandler.Dispose();
                request.uploadHandler = new UploadHandlerRaw(jsonToPut);

                request.SetRequestHeader("Content-Type", "application/json");

                if (requester.isChat)
                {
                    //채팅시에만 넣어서 보낼 수 있게 해야함
                    //로그인이랑 회원가입(중복확인) 제외한 모든 곳에 헤더에 토큰이 들어가야 함!
                    //Authorization : Bearer {token} 
                    request.SetRequestHeader("Authorization", "Bearer" + token);
                    print("보내짐");
                }
                break;
            case RequestType.DELETE:
                request = UnityWebRequest.Delete(requester.url);
                break;
            //TEDTURE
            case RequestType.TEXTURE:
                request = UnityWebRequestTexture.GetTexture(requester.url);
                break;
        }
        
        print("서버 기다리는 중(얌전)");

        //서버에 요청을 보내고 응답이 올때까지 기다린다.
        yield return request.SendWebRequest();

        //응답이 성공했다면
        if (request.result == UnityWebRequest.Result.Success)
        {
            //downloadHandler -> 서버에서 받은 내용들이 담겨있는 곳
            print("NET COMPLETE : " + request.downloadHandler.text);
            //3.완료되었다고 실행
            requester.OnComplete(request.downloadHandler);

            if (requester.isPhoto == true)
            {
                StartCoroutine(Loding());
            }

            else if (requester.isNet == true)
            {
                StartCoroutine(MainLoding());
            }
               
        }

        //그렇지 않다면(실패)
        else
        {
            print("NET ERROR : " + request.error);
            print("NET ERROR : " + request.downloadHandler.text);
            requester.OnFailed(request.downloadHandler);

            if (requester.isPhoto == true)
            {
                StartCoroutine(Loding());
            }

            else if (requester.isNet == true)
            {
                StartCoroutine(MainLoding());
            }
        }
        request.Dispose();
    }

    #region AI 이미지 통신
    public void SendPhoto(WWWForm photoData, SuccessDelegate dele, ErrorDelegate  error,bool isFace)
    {
        print("제발 들어가게 해주세요");
        StartCoroutine(SendMedia(photoData, dele, error, isFace));
    }

    IEnumerator SendMedia(WWWForm photoData, SuccessDelegate dele, ErrorDelegate error, bool isFace)
    {

        string url = "http://221.163.19.218:5137/";

        if (isFace)
        {
            print("얼굴 URL");
            url += "face_registration_integ/upload_data";
        }

        else
        {
            print("앨범 URL");
            url += "album_registration_integ/images_analysis";
        }

        UnityWebRequest www = UnityWebRequest.Post(url, photoData);

        //www.SetRequestHeader("Content-Type", "multipart/form-data");
        //※ 헤더 필요 없나?
        //www.SetRequestHeader("Authorization", "Bearer" + token);

        PhotoManager.instance.loding.GetComponent<AlphaGPSSet>().OpenAlpha();
        //SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.SFX_LodingCat);

        yield return www.SendWebRequest();

        //응답이 성공했다면
        if (www.result == UnityWebRequest.Result.Success)
        {
            //downloadHandler -> 서버에서 받은 내용들이 담겨있는 곳
            print("NET COMPLETE : " + www.downloadHandler.text);
            dele(www.downloadHandler);

            StartCoroutine(Loding());
        }

        else
        {
            print("NET ERROR : " + www.error);
            print("NET ERROR : " + www.downloadHandler.text);
            error(www.downloadHandler);

            StartCoroutine(Loding());
        }

        www.Dispose();
    }

    public void SendVoiceChat(WWWForm voiceData, SuccessDelegate dele)
    {
        StartCoroutine(SendMediaChat(voiceData, dele));
    }

    IEnumerator SendMediaChat(WWWForm voiceData, SuccessDelegate dele)
    {
        //string url = "https://f5ef-119-194-163-123.jp.ngrok.io/voice_chat_bot_inference";

        string url = "http://remembermebackend-env.eba-dcctnmvk.ap-northeast-2.elasticbeanstalk.com/voice/postvoice/";
        url += username;

        UnityWebRequest www = UnityWebRequest.Post(url, voiceData);

        //www.SetRequestHeader("Content-Type", "multipart/form-data");
        www.SetRequestHeader("Authorization", "Bearer" + token);

        PhotoManager.instance.loding.GetComponent<AlphaGPSSet>().OpenAlpha();

        yield return www.SendWebRequest();

        //응답이 성공했다면
        if (www.result == UnityWebRequest.Result.Success)
        {
            //downloadHandler -> 서버에서 받은 내용들이 담겨있는 곳
            print("NET COMPLETE : " + www.downloadHandler.text);

            StartCoroutine(Loding());

            dele(www.downloadHandler);
        }
        else
        {
            StartCoroutine(Loding());

            print("NET ERROR : " + www.error);
            print("NET ERROR : " + www.downloadHandler.text);
        }

        www.Dispose();
    }
    #endregion

    //로딩중
    IEnumerator Loding()
    {
        PhotoManager.instance.loding.GetComponent<AlphaGPSSet>().CloseAlpha();
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator MainLoding()
    {
        mainLoding.GetComponent<AlphaGPSSet>().CloseAlpha();
        yield return new WaitForSeconds(0.1f);
    }

    #region 리스트를 사용해서 한번에 처리
    public void AddRequester(HttpRequester_LHS requester)
    {
        requesters.Add(requester);
    }

    public void SendRequest()
    {
        StartCoroutine(Send());
    }

    IEnumerator Send()
    {
        while (requesters.Count > 0)
        {
            yield return SendProcess(requesters[0]);

            requesters.RemoveAt(0);
        }
        yield return null;
    }
    #endregion
}

