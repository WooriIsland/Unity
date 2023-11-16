using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

//AI 통신 내용
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
public delegate void ErrorDelegate(DownloadHandler handler);

//이미지 파일 Json 형식으로 변환 -> 이미지를 바이트 배열로 읽은 다음 Base64문자열로 인코딩하고 Json 객체의 일부로 만듬
//Base64 문자열은 이미지 데이터를 텍스트 형식으로 안전하게 전송할 수 있게 해줌

//이미지 자체로도 보낼 수 있음 -> 폼 데이터를 사용 (바이너리 데이터를 포함시켜 전송할 수 있음)
public class PhotoManager : MonoBehaviour
{
    public static PhotoManager instance;

    public SuccessDelegate OnSuccess;
    public SuccessDelegate OnFaceSuccess;
    public ErrorDelegate OnError;
    public ErrorDelegate OnFaceError;

    [SerializeField]
    private Transform photoContent;

    [SerializeField]
    private Transform photoFrameContent;



    [SerializeField]
    private PhotoInfo photoItim;
    [SerializeField]
    private PhotoInfo frameItim;

    public GameObject photoFrameUi;

    public List<PhotoInfo> photoList;
    //public List<PhotoInfo> photoFrameList;

    [SerializeField]
    private TMP_InputField summaryText;
    [SerializeField]
    private TMP_InputField summaryFrameText;

    [Header("결과값 UI")]
    public GameObject[] faceUI;

    public GameObject[] editUI;

    public GameObject[] deleteUI;

    [Header("포토북 수정 UI")]
    public PhotoEditMode editMode;

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
        OnError += OnFailed;
        OnFaceError += OnFaceFailed;
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

        form.AddField("user_id", "1");
        for (int i = 0; i < readFile.Count; i++)
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

        HttpManager_LHS.instance.SendVoice(form, OnSuccess, OnFailed, false);
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
            string location = json["photo_location"].ToObject<string>();

            //바이너리 이미지파일로 변환
            // byte[] byteData = Convert.FromBase64String(iamgeData); //Encoding.UTF8.GetBytes(iamgeData);
            //File.WriteAllBytes(Application.dataPath + "/" + i + ".jpg", byteData);

            Texture2D texture = new Texture2D(0, 0);
            //texture.LoadImage(byteData);

            OnAddPhoto(photo_datetime, summary, location, texture, null, image);

            //사진URL을 전달해서 해당 프리팹에서 URL이 보여질 수 있도록 하기 S3통신
        }
    }

    private void OnFailed(DownloadHandler handler)
    {
        print("ai 사진 등록 실패");
    }


    /*void OnPostFailed()
    {
        print("ai 사진 등록 실패");
    }*/

    //책 조회인지 앨범 넣는조회인지 확인
    bool isBookCheck;
    PhotoInfo photo;
    //PhotoInfo photoFrame;

    //가족 사진 조회
    public void OnPhotoInquiry(bool isBook)
    {
        isBookCheck = isBook;
        print(isBookCheck);

        OnDestroyPhoto(isBookCheck);

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
            string location = json["photo_location"].ToObject<string>();

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

            OnAddPhoto(photo_datetime, summary, location, texture, id, image);
        }
    }

    //통신성공 시 생성됨 -> 정렬알고리즘 사용해서 해야함
    public void OnAddPhoto(string time, string summary, string location, Texture2D texture, string id, string url)
    {

        //초기화
        photoList = new List<PhotoInfo>();

        if (isBookCheck)
        {
            photo = Instantiate(photoItim, photoContent);
            print("ㅍ");
        }

        else
        {
            photo = Instantiate(frameItim, photoFrameContent);
        }

        //들어가는 순서를 바꾸는 것
        photo.transform.SetSiblingIndex(0);

        photo.SetTextInfo(time, summary, location, texture, id, url);

        photoList.Add(photo);
    }

    void OnGetPostFailed()
    {
        print("Ai 사진 조회 실패");
    }
    #endregion

    public void OnDestroyPhoto(bool isBook)
    {
        //photoList.Clear();
        if(isBook)
        {
            print("앨범 모두 삭제");
            PhotoInfo[] photoObj = photoContent.GetComponentsInChildren<PhotoInfo>();

            print(photoObj.Length);

            foreach (var obj in photoObj)
            {
                Destroy(obj.gameObject);
            }

        }

        else
        {
            print("앨범오브젝트 모두 삭제");
            PhotoInfo[] photoFrameObj = photoFrameContent.GetComponentsInChildren<PhotoInfo>();
            print(photoFrameObj.Length);

            foreach (var obj in photoFrameObj)
            {
                Destroy(obj.gameObject);
            }

        }

        /*print("삭제해야함");
        PhotoInfo[] photoInfoComponents = photoContent.GetComponentsInChildren<PhotoInfo>();

        foreach (PhotoInfo photoInfo in photoInfoComponents)
        {
            Destroy(photoInfo.gameObject);
        }*/

    }

    // 안면 데이터 등록 (form-data)
    public void OnFaceUpload(byte[] Array)
    {
        //byte 바꾸기 
        byte[] readFile = Array;

        Debug.Log(readFile.Length);

        WWWForm form = new WWWForm();

        form.AddField("island_unique_number", "11111"); //유저 고유 가족키
        form.AddField("user_id", "1"); //유저 고유 번호
        form.AddField("user_nickname", "정이"); //유저 고유 닉네임
        //이미지
        form.AddBinaryData("face_image", readFile, "F0011_IND_D_13_0_01.jpg"); //이미지 여러개 가능?

        HttpManager_LHS.instance.SendVoice(form, OnFaceSuccess, OnFaceFailed, true);
    }

    //직접 파싱하기
    void OnFacePostComplete(DownloadHandler result)
    {
        for (int i = 0; i < 3; i++)
        {
            faceUI[i].SetActive(false);
        }

        faceUI[3].SetActive(true);
        print("안면데이터등록");

        JObject data = JObject.Parse(result.text);
        print(data);
    }


    private void OnFaceFailed(DownloadHandler handler)
    {
        for (int i = 0; i < 3; i++)
        {
            faceUI[i].SetActive(false);
        }

        faceUI[4].SetActive(true);
        print("ai 안면 등록 실패");
    }

    public void OnFaceFailedReset()
    {
        for (int i = 0; i < 2; i++)
        {
            faceUI[i].SetActive(true);
        }
    }


    //검색 조회 (유저가 입력한 값이 들어가야 함)

    public void OnSearchInquiry(bool isBook)
    {
        isBookCheck = isBook;

        print(isBookCheck);

        OnDestroyPhoto(isBookCheck);

        AiSearchPhotoInfo aiInfo = new AiSearchPhotoInfo();

        //예시로 넣어놈
        aiInfo.island_unique_number = "11111";
        if(isBookCheck)
        {
            aiInfo.search_keyword = summaryText.text;
        }
        else
        {
            aiInfo.search_keyword = summaryFrameText.text;
        }

        //Json 형식으로 값이 들어가지게 됨 -> 이쁘게 나오기 위해 true
        string aiJsonData = JsonUtility.ToJson(aiInfo, true);
        print(aiJsonData);

        //AI 로딩 UI
        HttpManager_LHS.instance.isAichat = false;

        OnSearchGetPost(aiJsonData);
        summaryText.text = null;
        summaryFrameText.text = null;
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

        string available = data["available"].ToObject<string>();
        print("안면등록확인" + available);

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
            string location = json["photo_location"].ToObject<string>();


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
            OnAddPhoto(photo_datetime, summary, location, texture, id, image);

        }
    }

    void OnSearchGetPostFailed()
    {
        print("Ai 사진 검색조회 실패");
    }

    GameObject photoObj;

    public void PhotoEditMode(GameObject obj, string id, string time, string summary, string location)
    {
        editMode.gameObject.SetActive(true);
        photoObj = obj;
        editMode.time.text = "날짜:" + " " + time;
        editMode.summary.text = summary;
        editMode.location.text = location;
    }

    public void PhotoEditSave()
    {
        //다시 전달해주기 (통신할 수 있게)
        photoObj.GetComponent<PhotoInfo>().OnChangeEnd(editMode.summary.text);

        print(editMode.summary.text);

        //editMode.gameObject.SetActive(false);
    }

    public void PhotoEditSuccess()
    {
        editUI[0].SetActive(false);
        editUI[1].SetActive(true);
    }

    public void PhotoEditFail()
    {
        editUI[0].SetActive(false);
        editUI[2].SetActive(true);
    }

    public void PhotoDeleteMode(GameObject obj)
    {
        photoObj = obj;
        deleteUI[3].SetActive(true);
    }

    public void PhotoDeleteSave()
    {
        photoObj.GetComponent<PhotoInfo>().OnDeletePhoto();
    }

    public void PhotoDeleteSuccess()
    {
        deleteUI[0].SetActive(false);
        deleteUI[1].SetActive(true);
    }

    public void PhotoDeleteFail()
    {
        deleteUI[0].SetActive(false);
        deleteUI[2].SetActive(true);
    }

    PhotoInfo framePhotoInfo;
    public void FrameObject(GameObject obj)
    {
        print("1번" + obj);
        framePhotoInfo = obj.GetComponentInChildren<PhotoInfo>();
    }

    public void FrameSetting(string time, string summary, string location, string id, string url)
    {
        //선택한 오브젝트가 null이 아니라면
        if (framePhotoInfo != null)
        {
            print("3번 다시 셋팅 해야 함");

            Texture2D texture = new Texture2D(0, 0);
            framePhotoInfo.SetTextInfo(time, summary, location, texture, id, url);
            //초기화
            //framePhotoInfo = null;
        }
    }
}
