using Newtonsoft.Json.Linq;
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
    public string island_unique_number;
}

[System.Serializable]
public struct AiSearchPhotoInfo
{
    public string island_unique_number;
    public string search_keyword;
}

public delegate void SuccessDelegate(DownloadHandler handle);

//이미지 파일 Json 형식으로 변환 -> 이미지를 바이트 배열로 읽은 다음 Base64문자열로 인코딩하고 Json 객체의 일부로 만듬
//Base64 문자열은 이미지 데이터를 텍스트 형식으로 안전하게 전송할 수 있게 해줌

//이미지 자체로도 보낼 수 있음 -> 폼 데이터를 사용 (바이너리 데이터를 포함시켜 전송할 수 있음)
public class PhotoManager : MonoBehaviour
{
    public static PhotoManager instance;

    public SuccessDelegate OnSuccess;
    public SuccessDelegate OnFaceSuccess;

    [SerializeField]
    private Transform photoContent;

    [SerializeField]
    private PhotoInfo photoItim;

    public List<PhotoInfo> photoList = new List<PhotoInfo>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        OnSuccess += OnPostComplete;
        OnFaceSuccess += OnFacePostComplete;
    }

    void Update()
    {

    }

    #region AI 통신
    //등록하고 조회 되야 되는 거 아닌가?

    //가족 사진 등록
    public void OnPhotoCreate(List<byte[]> listByteArrays)
    {
        //byte 바꾸기 
        //byte[] readFile = File.ReadAllBytes("C:/Users/HP/Desktop/Test/voice/voice.wav");
        //byte[] readFile = File.ReadAllBytes(Application.streamingAssetsPath + "/" + "3" + ".jpg");
        List<byte[]> readFile = listByteArrays;

        Debug.Log(readFile.Count);

        //UnityWebRequest[] files = new UnityWebRequest[2];
        WWWForm form = new WWWForm();

        //files[0] = UnityWebRequest.Get("file:///C:/Users/HP/Desktop/Test/voice/voice.wav");

        /*form.AddField("family_id", PhotonNetwork.NickName);
        form.AddField("opponentNickname", "회은");
        form.AddBinaryData("voice", readFile, "voice.wav");*/

        form.AddField("user_id", "2");
        for(int i = 0; i < readFile.Count; i++)
        {
            //이미지
            form.AddBinaryData("photo_image", readFile[i], "F0011_GM_F_D_71-46-13_04_travel.jpg");
        }

        string deb = "";
        foreach (var item in form.headers)
        {
            deb += item.Key + " : " + item.Value + "\n";
        }
        Debug.Log(deb);

        HttpManager_LHS.instance.SendVoice(form, OnSuccess, false);
    }

    // 성공했을 때
    void OnPostComplete(DownloadHandler result)
    {
        print("ai 사진 등록 성공");
        print(result.text);

        /*HttpChatVoiceData photoData = new HttpChatVoiceData();
        photoData = JsonUtility.FromJson<HttpChatVoiceData>(result.text);*/

        //성공하면 그 뒤에 이미지 추가
        JObject data = JObject.Parse(result.text);

        JArray jsonArray = data["data"].ToObject<JArray>();

        print("파일 갯수 : " + jsonArray.Count);

        for (int i = 0; i < jsonArray.Count; i++)
        {
            JObject json = jsonArray[i].ToObject<JObject>();
            //string iamgeData = json["binary_image"].ToObject<string>();
            string photo_datetime = json["photo_datetime"].ToObject<string>();
            string summary = json["summary"].ToObject<string>();
            //string id = json["photo_id"].ToObject<string>();
            string image = json["photo_image"].ToObject<string>();

            //바이너리 이미지파일로 변환
            // byte[] byteData = Convert.FromBase64String(iamgeData); //Encoding.UTF8.GetBytes(iamgeData);
            //File.WriteAllBytes(Application.dataPath + "/" + i + ".jpg", byteData);

            Texture2D texture = new Texture2D(0, 0);
            //texture.LoadImage(byteData);

            OnAddPhoto(photo_datetime, summary, texture, null, image);

            //사진URL을 전달해서 해당 프리팹에서 URL이 보여질 수 있도록 하기 S3통신
        }
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
        aiInfo.island_unique_number = "11111";

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

        for (int i = 0; i < jsonArray.Count; i++)
        {
            JObject json = jsonArray[i].ToObject<JObject>();
            //string iamgeData = json["binary_image"].ToObject<string>();
            string photo_datetime = json["photo_datetime"].ToObject<string>();
            string summary = json["summary"].ToObject<string>();
            string id = json["photo_id"].ToObject<string>();
            string image = json["photo_image"].ToObject<string>();

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
            //byte[] byteData = Convert.FromBase64String(iamgeData); //Encoding.UTF8.GetBytes(iamgeData);
            //File.WriteAllBytes(Application.dataPath + "/" + i + ".jpg", byteData);

            Texture2D texture = new Texture2D(0, 0);
            //texture.LoadImage(byteData);

            OnAddPhoto(photo_datetime, summary, texture, id, image);
        }

        //HttpData<HttpAiPhotoData> aiData = JsonUtility.FromJson<HttpData<HttpAiPhotoData>>(jsonData);
        //HttpAiPhotoData aiPhotoData = aiData.data;

        //aiPhotoData = JsonUtility.FromJson<HttpAiPhotoData>(result.text);

        //downloadHandler에 받아온 Json형식 데이터 가공하기
        //2.FromJson으로 형식 바꿔주기
        //ChatData chatData = JsonUtility.FromJson<ChatData>(result.text);
    }

    //통신성공 시 생성됨 -> 정렬알고리즘 사용해서 해야함
    public void OnAddPhoto(string time, string summary, Texture2D texture, string id, string url)
    {
        PhotoInfo photo = Instantiate(photoItim, photoContent);

        photo.SetTextInfo(time, summary, texture, id, url);
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

        foreach (var obj in photoObj)
        {
            Destroy(obj);
        }
    }

    // 안면 데이터 등록 (form-data)
    public void OnFaceUpload(byte[] Array)
    {
        //byte 바꾸기 
        byte[] readFile = Array;

        Debug.Log(readFile.Length);

        WWWForm form = new WWWForm();

        form.AddField("island_unique_number", "11111"); //유저 고유 가족키
        form.AddField("user_id", "3"); //유저 고유 번호
        form.AddField("user_nickname", "정민이"); //유저 고유 닉네임
        //이미지
        form.AddBinaryData("face_image", readFile, "F0011_IND_D_13_0_01.jpg"); //이미지 여러개 가능?

        HttpManager_LHS.instance.SendVoice(form, OnFaceSuccess, true);
    }

    //직접 파싱하기
    void OnFacePostComplete(DownloadHandler result)
    {
        print("안면데이터등록");

        JObject data = JObject.Parse(result.text);
        print(data);
    }

    //검색 조회 (유저가 입력한 값이 들어가야 함)
    public void OnSearchInquiry(string s)
    {
        AiSearchPhotoInfo aiInfo = new AiSearchPhotoInfo();

        //예시로 넣어놈
        aiInfo.island_unique_number = "11111";
        aiInfo.search_keyword = s;

        //Json 형식으로 값이 들어가지게 됨 -> 이쁘게 나오기 위해 true
        string aiJsonData = JsonUtility.ToJson(aiInfo, true);
        print(aiJsonData);

        //AI 로딩 UI
        HttpManager_LHS.instance.isAichat = false;

        //AI와 채팅을 한다!
        OnSearchGetPost(aiJsonData);
    }

    public void OnSearchGetPost(string s)
    {
        string url = "http://221.163.19.218:5137/album_search_integ/search";

        //생성 -> 데이터 조회 -> 값을 넣어줌 
        HttpRequester_LHS requester = new HttpRequester_LHS();

        requester.SetUrl(RequestType.POST, url, false);
        requester.body = s;
        requester.isJson = true;
        requester.isChat = false;

        requester.onComplete = OnSearchGetPostComplete;
        requester.onFailed = OnSearchGetPostFailed;

        HttpManager_LHS.instance.SendRequest(requester);
    }

    //직접 파싱하기
    void OnSearchGetPostComplete(DownloadHandler result)
    {
        print("Ai 사진 검색조회 성공");

        JObject data = JObject.Parse(result.text);

        JArray jsonArray = data["data"].ToObject<JArray>();

        print("파일 갯수 : " + jsonArray.Count);

        for (int i = 0; i < jsonArray.Count; i++)
        {
            JObject json = jsonArray[i].ToObject<JObject>();
            //string iamgeData = json["binary_image"].ToObject<string>();
            string photo_datetime = json["photo_datetime"].ToObject<string>();
            string summary = json["summary"].ToObject<string>();
            string id = json["photo_id"].ToObject<string>();
            string image = json["photo_image"].ToObject<string>();


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
            //byte[] byteData = Convert.FromBase64String(iamgeData); //Encoding.UTF8.GetBytes(iamgeData);
            //File.WriteAllBytes(Application.dataPath + "/" + i + ".jpg", byteData);

            Texture2D texture = new Texture2D(0, 0);
            //texture.LoadImage(byteData);

            //이전사진 다 삭제해야함
            OnAddPhoto(photo_datetime, summary, texture,id, image);

        }
    }

    void OnSearchGetPostFailed()
    {
        print("Ai 사진 검색조회 실패");
    }

}
