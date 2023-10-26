﻿using Newtonsoft.Json.Linq;
using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

//AI 통신 내용
//Json에 담길 내용 "키" : "값"
[System.Serializable]
public struct AiPhotoInfo
{
    public string family_id;
}

public delegate void SuccessDelegate(DownloadHandler handle);

//이미지 파일 Json 형식으로 변환 -> 이미지를 바이트 배열로 읽은 다음 Base64문자열로 인코딩하고 Json 객체의 일부로 만듬
//Base64 문자열은 이미지 데이터를 텍스트 형식으로 안전하게 전송할 수 있게 해줌

//이미지 자체로도 보낼 수 있음 -> 폼 데이터를 사용 (바이너리 데이터를 포함시켜 전송할 수 있음)
public class PhotoManager : MonoBehaviour
{
    public static PhotoManager instance;

    public SuccessDelegate OnSuccess;

    [SerializeField]
    private Transform photoContent;

    [SerializeField]
    private PhotoInfo photoItim;

    public List<PhotoInfo> photoList = new List<PhotoInfo>();

    // 통신 시
    

    //예시용
    public Texture2D image;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        OnSuccess += OnPostComplete;
    }

    void Update()
    {

    }

    #region AI 통신
    //등록하고 조회 되야 되는 거 아닌가?

    //가족 사진 등록
    public void OnPhotoCreate(byte[] Array)
    {
        //byte 바꾸기 
        //byte[] readFile = File.ReadAllBytes("C:/Users/HP/Desktop/Test/voice/voice.wav");
        //byte[] readFile = File.ReadAllBytes(Application.streamingAssetsPath + "/" + "3" + ".jpg");
        byte[] readFile = Array;

        Debug.Log(readFile.Length);

        //UnityWebRequest[] files = new UnityWebRequest[2];
        WWWForm form = new WWWForm();

        //files[0] = UnityWebRequest.Get("file:///C:/Users/HP/Desktop/Test/voice/voice.wav");

        /*form.AddField("family_id", PhotonNetwork.NickName);
        form.AddField("opponentNickname", "회은");
        form.AddBinaryData("voice", readFile, "voice.wav");*/

        form.AddField("family_id", "family_1");
        //이미지
        form.AddBinaryData("image", readFile, "F0011_GM_F_D_71-46-13_04_travel.jpg");

        string deb = "";
        foreach (var item in form.headers)
        {
            deb += item.Key + " : " + item.Value + "\n";
        }
        Debug.Log(deb);

        HttpManager_LHS.instance.SendVoice(form, OnSuccess);
    }

    // 성공했을 때
    void OnPostComplete(DownloadHandler result)
    {
        print("ai 사진 등록 성공");
        print(result.text);
        HttpChatVoiceData photoData = new HttpChatVoiceData();
        photoData = JsonUtility.FromJson<HttpChatVoiceData>(result.text);

        //성공하면 그 뒤에 이미지 추가
        JObject data = JObject.Parse(result.text);

        JArray jsonArray = data["data"].ToObject<JArray>();

        print("파일 갯수 : " + jsonArray.Count);

        for (int i = 0; i < jsonArray.Count; i++)
        {
            JObject json = jsonArray[i].ToObject<JObject>();
            string iamgeData = json["binary_image"].ToObject<string>();
            string date_time = json["date_time"].ToObject<string>();
            string character = json["summary"].ToObject<string>();

            //바이너리 이미지파일로 변환
            byte[] byteData = Convert.FromBase64String(iamgeData); //Encoding.UTF8.GetBytes(iamgeData);
            //File.WriteAllBytes(Application.dataPath + "/" + i + ".jpg", byteData);

            Texture2D texture = new Texture2D(0, 0);
            texture.LoadImage(byteData);

            OnAddPhoto(date_time, character, texture);
        }
        #region 성공시 구현
        //※byte[] data = Convert.FromBase64String(chatVoiceData.results.voice.body);

        //byte[] downData = result.data;

        //SavWav_LHS.Save("C:/Users/HP/Desktop/Test/voice/return_voice", clipData);
        //File.WriteAllBytes("C:/Users/HP/Desktop/Test/voice/return_voice.wav", result.data);

        //※File.WriteAllBytes(Application.streamingAssetsPath + "/" + PhotonNetwork.NickName + "_return.wav", data);
        //File.WriteAllBytes(Application.streamingAssetsPath + "/" + PhotonNetwork.NickName + "_return.wav", chatVoiceData.results.voice.body);

        //AudioClip downClip = AudioClip.Create("result", 0, 1, 22050, false);

        //StartCoroutine(GetWav2AudioClip("C:/Users/HP/Desktop/Test/voice/return_voice.wav"));
        //※StartCoroutine(GetWav2AudioClip(Application.streamingAssetsPath + "/" + PhotonNetwork.NickName + "_return.wav"));
        #endregion
    }

    //성공시
    /*   {
    "description": "Complete Register Family data",
    "images_count": 1,
    "message": "저장 완료!"
    }*/

    //응답받고 실행 시킬
    /*IEnumerator GetWav2AudioClip(string path)
    {
        Uri voiceURI = new Uri(path);
        UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(voiceURI, AudioType.WAV);

        yield return www.SendWebRequest();

        AudioClip clipData = ((DownloadHandlerAudioClip)www.downloadHandler).audioClip;

        AudioSource audio = GetComponent<AudioSource>();
        if (audio)
        {
            audio.clip = clipData;
            audio.Play();
        }
    }*/

    void OnPostFailed()
    {
        print("ai 사진 등록 실패");
    }

    //가족 사진 조회
    public void OnPhotoInquiry()
    {
        AiPhotoInfo aiInfo = new AiPhotoInfo();

        //예시로 넣어놈
        aiInfo.family_id = "family_1";

        //Json 형식으로 값이 들어가지게 됨 -> 이쁘게 나오기 위해 true
        string aiJsonData = JsonUtility.ToJson(aiInfo, true);
        print(aiJsonData);

        //AI 로딩 UI
        HttpManager_LHS.instance.isAichat = false;

        //AI와 채팅을 한다!
        OnGetPost(aiJsonData);
    }

    //Ai
    // 엔터 쳤을 때 -> 챗봇 보내는 내용
    // 서버에 게시물 조회 요청 -> HttpManager한테 알려주려고 함
    public void OnGetPost(string s)
    {
        string url = "http://221.163.19.218:5137/album_inquiry_integ/inquiry";

        //생성 -> 데이터 조회 -> 값을 넣어줌 
        HttpRequester_LHS requester = new HttpRequester_LHS();

        requester.SetUrl(RequestType.POST, url, false);
        requester.body = s;
        requester.isJson = true;
        requester.isChat = false;

        requester.onComplete = OnGetPostComplete;
        requester.onFailed = OnGetPostFailed;

        HttpManager_LHS.instance.SendRequest(requester);
    }

    //직접 파싱하기
    void OnGetPostComplete(DownloadHandler result)
    {
        print("Ai 사진 조회 성공");

        JObject data = JObject.Parse(result.text);

        JArray jsonArray = data["data"].ToObject<JArray>();

        print("파일 갯수 : " + jsonArray.Count);

        for(int i = 0; i < jsonArray.Count; i++)
        {
            JObject json = jsonArray[i].ToObject<JObject>();
            string iamgeData = json["binary_image"].ToObject<string>();
            string date_time = json["date_time"].ToObject<string>();
            string character = json["summary"].ToObject<string>();

            #region 배열
            /*JArray character = json["character"].ToObject<JArray>();

            List<string> list = new List<string>();
            for(int j = 0; j < character.Count; j++)
            {
                 list.Add(character[j].ToObject<string>());
            }*/

            //if (i == 0)
            #endregion

            //바이너리 이미지파일로 변환
            byte[] byteData = Convert.FromBase64String(iamgeData); //Encoding.UTF8.GetBytes(iamgeData);
            //File.WriteAllBytes(Application.dataPath + "/" + i + ".jpg", byteData);

            Texture2D texture = new Texture2D(0, 0);
            texture.LoadImage(byteData);

            OnAddPhoto(date_time, character, texture);
        }

        //HttpData<HttpAiPhotoData> aiData = JsonUtility.FromJson<HttpData<HttpAiPhotoData>>(jsonData);
        //HttpAiPhotoData aiPhotoData = aiData.data;

        //aiPhotoData = JsonUtility.FromJson<HttpAiPhotoData>(result.text);

        //downloadHandler에 받아온 Json형식 데이터 가공하기
        //2.FromJson으로 형식 바꿔주기
        //ChatData chatData = JsonUtility.FromJson<ChatData>(result.text);
    }

    //통신성공 시 생성됨 -> 정렬알고리즘 사용해서 해야함
    public void OnAddPhoto(string time, string summary, Texture2D texture)
    {
        PhotoInfo photo = Instantiate(photoItim, photoContent);

        photo.SetTextInfo(time, summary, texture);
        photoList.Add(photo);
    }

    void OnGetPostFailed()
    {
        print("Ai 사진 조회 실패");
    }
    #endregion

    public void OnDestroyPhoto()
    {
        //photoList.Clear();

        PhotoInfo[] photoObj = photoContent.GetComponentsInChildren<PhotoInfo>();

        print(photoObj.Length);

        foreach(var obj in photoObj)
        {
            Destroy(obj);
        }
    }
}