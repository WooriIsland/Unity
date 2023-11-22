using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

//조회
[System.Serializable]
public struct AiPhotoInfo
{
    public string island_unique_number;
    public string user_id;
}

//검색조회
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

    //Form data 변수
    private SuccessDelegate OnSuccess;
    private SuccessDelegate OnFaceSuccess;
    private ErrorDelegate OnError;
    private ErrorDelegate OnFaceError;

    [Header("사진추가부모위치")]
    [SerializeField]
    private Transform photoContent;
    [SerializeField]
    private Transform photoFrameContent;

    [Header("사진Itim")]
    [SerializeField]
    private PhotoInfo photoItim;
    [SerializeField]
    private PhotoInfo frameItim;

    [Header("섬꾸미기Photo")]
    public GameObject photoFrameUi;
    [Header("포토북 수정 UI")]
    public PhotoEditMode editMode;
    [Header("안면데이터 UI")]
    [SerializeField]
    private GameObject photoFaceUI;

    [Header("검색키워드")]
    [SerializeField]
    private TMP_InputField summaryText;
    [SerializeField]
    private TMP_InputField summaryFrameText;

    [Header("결과값 UI")]
    public GameObject[] faceUI;
    public GameObject[] editUI;
    public GameObject[] deleteUI;

    [Header("사진없을때 UI")]
    public GameObject noPicture;

    [Header("통신 로딩 UI")]
    public GameObject loding;

    private List<PhotoInfo> photoList;

    //앨범 조회 OR 섬꾸미기앨범 조건
    private bool isBookCheck;
    private PhotoInfo photo;

    //수정 삭제시 선택오브젝트를 알기 위한 변수
    GameObject photoObj;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        //Form data 안면등록/사진등록 성공 or 실패값
        OnSuccess += OnPostComplete;
        OnFaceSuccess += OnFacePostComplete;
        OnError += OnFailed;
        OnFaceError += OnFaceFailed;
    }

    public void Update()
    {

    }

    #region 안면등록 1개 (form-data)
    public void OnFaceUpload(byte[] Array)
    {
        byte[] readFile = Array;
        Debug.Log(readFile.Length);

        WWWForm form = new WWWForm();

        form.AddField("island_unique_number", "11111"); //※유저 고유 가족키
        form.AddField("user_id", "2"); //※유저 고유 번호
        form.AddField("user_nickname", "혜리"); //※유저 고유 닉네임
        //이미지
        form.AddBinaryData("face_image", readFile, "F0011_IND_D_13_0_01.jpg"); //이미지 여러개 가능?

        HttpManager_LHS.instance.SendPhoto(form, OnFaceSuccess, OnFaceFailed, true);
    }

    void OnFacePostComplete(DownloadHandler result)
    {
        /*for (int i = 0; i < 3; i++)
        {
            faceUI[i].SetActive(false);
        }*/

        //성공 UI
        faceUI[2].GetComponent<PopupGallery>().CloseAction(faceUI[3].GetComponent<BasePopup>());
        
        print("안면데이터등록");

        JObject data = JObject.Parse(result.text);
        print(data);
    }


    private void OnFaceFailed(DownloadHandler handler)
    {
        /*for (int i = 0; i < 3; i++)
        {
            faceUI[i].SetActive(false);
        }*/

        //실패 UI
        faceUI[2].GetComponent<PopupGallery>().CloseAction(faceUI[4].GetComponent<BasePopup>());

        print("ai 안면 등록 실패");
    }

    //삭제 가능
    public void OnFaceFailedReset()
    {
        /*for (int i = 0; i < 2; i++)
        {
            faceUI[i].SetActive(true);
        }*/
    }
    #endregion

    #region 사진등록 여러개 (form-data)
    public void OnPhotoCreate(List<byte[]> listByteArrays)
    {
        //byte 바꾸기 
        List<byte[]> readFile = listByteArrays;

        Debug.Log("사진 갯수" + readFile.Count);

        WWWForm form = new WWWForm();

        form.AddField("user_id", "2"); //※유저아이디 변경

        for (int i = 0; i < readFile.Count; i++)
        {
            //이미지 넣기
            form.AddBinaryData("photo_image", readFile[i], "F0011_GM_F_D_71-46-13_04_travel.jpg");
        }

        //키 값 만들기 -> ※필요한가 ?
        string deb = "";

        foreach (var item in form.headers)
        {
            deb += item.Key + " : " + item.Value + "\n";
        }

        Debug.Log(deb);

        HttpManager_LHS.instance.SendPhoto(form, OnSuccess, OnFailed, false);
    }

    void OnPostComplete(DownloadHandler result)
    {
        print("ai 사진 등록 성공");
        print(result.text);

        //성공하면 이미지 추가
        JObject data = JObject.Parse(result.text);

        JArray jsonArray = data["data"].ToObject<JArray>();

        print("파일 갯수 : " + jsonArray.Count);

        for (int i = 0; i < jsonArray.Count; i++)
        {
            JObject json = jsonArray[i].ToObject<JObject>();
            string photo_datetime = json["photo_datetime"].ToObject<string>();
            string summary = json["summary"].ToObject<string>();
            string id = json["photo_id"].ToObject<string>(); //※ 오류 나는지 확인
            string image = json["photo_image"].ToObject<string>();
            string location = json["photo_location"].ToObject<string>();

            //바이너리 이미지파일로 변환
            //byte[] byteData = Convert.FromBase64String(iamgeData); //Encoding.UTF8.GetBytes(iamgeData);
            //File.WriteAllBytes(Application.dataPath + "/" + i + ".jpg", byteData);

            Texture2D texture = new Texture2D(0, 0);
            //texture.LoadImage(byteData);

            //사진URL을 전달해서 해당 프리팹에서 URL이 보여질 수 있도록 하기 S3통신
            OnAddPhoto(photo_datetime, summary, location, texture, id, image);
        }
    }

    private void OnFailed(DownloadHandler handler)
    {
        print("ai 사진 등록 실패");
    }
    #endregion

    #region 사진조회 True or 섬꾸미기사진조회 False
    public void OnPhotoInquiry(bool isBook)
    {
        noPicture.SetActive(false);
        isBookCheck = isBook;
        print(isBookCheck);

        //사진 초기화
        OnDestroyPhoto(isBookCheck);

        AiPhotoInfo aiInfo = new AiPhotoInfo();

        aiInfo.island_unique_number = "11111"; //※가족섬고유번호 변경
        aiInfo.user_id = "2"; //※유저아이디 변경

        //Json 형식으로 값이 들어가지게 됨 -> 이쁘게 나오기 위해 true
        string aiJsonData = JsonUtility.ToJson(aiInfo, true);
        print(aiJsonData);

        //로딩 UI
        HttpManager_LHS.instance.isPhoto = true;

        OnGetPost(aiJsonData);
    }

    public void OnGetPost(string s)
    {
        //앨범조회 OR 섬꾸미기 앨범조회 API 다름
        string url;
        string urlBase = "http://221.163.19.218:5137/album_inquiry_integ/inquiry";
        string urlDeco = "http://221.163.19.218:5137/album_decoration_inquiry_integ/decoration_inquiry";

        if (isBookCheck)
        {
            url = urlBase;
        }

        else
        {
            url = urlDeco;
        }

        HttpRequester_LHS requester = new HttpRequester_LHS();
        requester.SetUrl(RequestType.POST, url, false);
        requester.body = s;
        requester.isJson = true;
        requester.isChat = false;

        requester.onComplete = OnGetPostComplete;
        requester.onFailed = OnGetPostFailed;

        HttpManager_LHS.instance.SendRequest(requester);
    }

    void OnGetPostComplete(DownloadHandler result)
    {
        print("사진 조회 성공");

        //직접 파싱하기 using Newtonsoft.Json.Linq; 필요
        JObject data = JObject.Parse(result.text);

        bool available = data["available"].ToObject<bool>();
        print("유저안면등록확인" + available);

        JArray jsonArray = data["data"].ToObject<JArray>();
        print("파일 갯수 : " + jsonArray.Count);

        if(jsonArray.Count == 0)
        {
            noPicture.SetActive(true);
            print("사진이 없습니다");
        }

        for (int i = 0; i < jsonArray.Count; i++)
        {
            JObject json = jsonArray[i].ToObject<JObject>();
            string photo_datetime = json["photo_datetime"].ToObject<string>();
            string summary = json["summary"].ToObject<string>();
            string id = json["photo_id"].ToObject<string>();
            string image = json["photo_image"].ToObject<string>();
            string location = json["photo_location"].ToObject<string>();

            #region 배열파싱
            /*JArray character = json["character"].ToObject<JArray>();

            List<string> list = new List<string>();
            for(int j = 0; j < character.Count; j++)
            {
                 list.Add(character[j].ToObject<string>());
            }*/

            //if (i == 0)
            #endregion

            Texture2D texture = new Texture2D(0, 0);

            OnAddPhoto(photo_datetime, summary, location, texture, id, image);
        }

        if(isBookCheck && !available)
        {
            print("안면등록이필요");

            faceUI[0].GetComponent<BasePopup>().OpenAction();
            faceUI[5].GetComponent<BaseAlpha>().OpenAlpha();
        }
    }

    void OnGetPostFailed(DownloadHandler result)
    {
        noPicture.SetActive(true);
        print("사진 조회 실패");
    }
    #endregion

    #region 키워드조회 True or 섬꾸미기키워드조회 False (유저 입력한 값 들어가야함)
    public void OnSearchInquiry(bool isBook)
    {
        noPicture.SetActive(false);

        isBookCheck = isBook;
        print(isBookCheck);

        OnDestroyPhoto(isBookCheck);

        AiSearchPhotoInfo aiInfo = new AiSearchPhotoInfo();

        aiInfo.island_unique_number = "11111"; //※가족섬고유번호 변경

        if (isBookCheck)
        {
            aiInfo.search_keyword = summaryText.text;
        }

        else
        {
            aiInfo.search_keyword = summaryFrameText.text;
        }

        string aiJsonData = JsonUtility.ToJson(aiInfo, true);
        print(aiJsonData);

        HttpManager_LHS.instance.isAichat = false;

        OnSearchGetPost(aiJsonData);

        //초기화
        summaryText.text = null;
        summaryFrameText.text = null;
    }

    public void OnSearchGetPost(string s)
    {
        string url = "http://221.163.19.218:5137/album_search_integ/search";

        HttpRequester_LHS requester = new HttpRequester_LHS();

        requester.SetUrl(RequestType.POST, url, false);
        requester.body = s;
        requester.isJson = true;
        requester.isChat = false;

        requester.onComplete = OnSearchGetPostComplete;
        requester.onFailed = OnSearchGetPostFailed;

        HttpManager_LHS.instance.SendRequest(requester);
    }

    void OnSearchGetPostComplete(DownloadHandler result)
    {
        print("Ai 사진 키워드조회 성공");

        JObject data = JObject.Parse(result.text);
        JArray jsonArray = data["data"].ToObject<JArray>();
        print("파일 갯수 : " + jsonArray.Count);

        if (jsonArray.Count == 0)
        {
            noPicture.SetActive(true);
            print("사진이 없습니다");
        }

        for (int i = 0; i < jsonArray.Count; i++)
        {
            JObject json = jsonArray[i].ToObject<JObject>();
            string photo_datetime = json["photo_datetime"].ToObject<string>();
            string summary = json["summary"].ToObject<string>();
            string id = json["photo_id"].ToObject<string>();
            string image = json["photo_image"].ToObject<string>();
            string location = json["photo_location"].ToObject<string>();

            Texture2D texture = new Texture2D(0, 0);

            OnAddPhoto(photo_datetime, summary, location, texture, id, image);
        }
    }

    void OnSearchGetPostFailed(DownloadHandler result)
    {
        noPicture.SetActive(true);
        print("Ai 사진 검색조회 실패");
    }
    #endregion

    #region 사진수정
    //사진 수정 -> 각자의 프리팹에서 진행 (But 정보값을전달해야함)
    //선택한 Obj의 값들을 EditMode에 전달
    //사진 
    public void PhotoEditMode(GameObject obj, string id, string time, string summary, string location)
    {
        //editMode.gameObject.SetActive(true);
        editUI[0].GetComponent<BasePopup>().OpenAction();
        editUI[3].GetComponent<BaseAlpha>().OpenAlpha();

        photoObj = obj;
        editMode.time.text = "날짜:" + " " + time;
        editMode.summary.text = summary;
        editMode.location.text = location;
    }

    //수정된 내용을 다시 선택한 Obj에 저장
    public void PhotoEditSave()
    {
        //다시 전달해주기 (통신할 수 있게)
        editUI[0].GetComponent<BasePopup>().CloseAction();
        photoObj.GetComponent<PhotoInfo>().OnChangeEnd(editMode.summary.text);
        print("수정 내용" + editMode.summary.text);
    }

    public void PhotoEditSuccess()
    {
        //editUI[0].SetActive(false);
        editUI[1].GetComponent<BasePopup>().OpenAction();
    }

    public void PhotoEditFail()
    {
        //editUI[0].SetActive(false);
        editUI[2].GetComponent<BasePopup>().OpenAction();
    }
    #endregion

    #region 사진삭제 UI

    //2.삭제 오브젝트 저장
    public void PhotoDeleteMode(GameObject obj)
    {
        photoObj = obj;

        deleteUI[4].GetComponent<BaseAlpha>().OpenAlpha();
        deleteUI[0].GetComponent<BasePopup>().OpenAction();
        //deleteUI[3].SetActive(true);
    }

    //2.삭제 오브젝트의 삭제통신 실행
    public void PhotoDeleteSave()
    {
        deleteUI[0].GetComponent<BasePopup>().CloseAction();
        photoObj.GetComponent<PhotoInfo>().OnDeletePhoto();
    }

    public void PhotoDeleteSuccess()
    {
        //deleteUI[0].GetComponent<BasePopup>().CloseAction(deleteUI[1].GetComponent<BasePopup>());
        deleteUI[1].GetComponent<BasePopup>().OpenAction();
    }

    public void PhotoDeleteFail()
    {
        //deleteUI[0].GetComponent<BasePopup>().CloseAction(deleteUI[2].GetComponent<BasePopup>());
        deleteUI[2].GetComponent<BasePopup>().OpenAction();
    }
    #endregion

    //사진 생성
    public void OnAddPhoto(string time, string summary, string location, Texture2D texture, string id, string url)
    {
        //초기화
        photoList = new List<PhotoInfo>();

        if (isBookCheck)
        {
            photo = Instantiate(photoItim, photoContent);
        }

        else
        {
            photo = Instantiate(frameItim, photoFrameContent);
        }

        //하이어라키 창에 들어가는 순서를 바꾸는 것
        photo.transform.SetSiblingIndex(0);

        //각자 자신의 Obj 셋팅
        photo.SetTextInfo(time, summary, location, texture, id, url);

        //사진 리스트
        photoList.Add(photo);
    }

    //사진 초기화
    public void OnDestroyPhoto(bool isBook)
    {
        //photoList.Clear();

        if (isBook)
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
            print("섬꾸미기앨범 모두 삭제");
            PhotoInfo[] photoFrameObj = photoFrameContent.GetComponentsInChildren<PhotoInfo>();
            print(photoFrameObj.Length);

            foreach (var obj in photoFrameObj)
            {
                Destroy(obj.gameObject);
            }
        }
    }

    #region 섬꾸미기 사진 등록
    PhotoInfo framePhotoInfo;

    //선택한 Obj
    public void FrameObject(GameObject obj)
    {
        print("실행1" + obj);
        framePhotoInfo = obj.GetComponentInChildren<PhotoInfo>();
    }

    public void FrameSetting(string time, string summary, string location, string id, string url)
    {
        //선택한 오브젝트가 null이 아니라면
        if (framePhotoInfo != null)
        {
            print("실행3 - 다시 셋팅 해야 함");

            Texture2D texture = new Texture2D(0, 0);
            framePhotoInfo.SetTextInfo(time, summary, location, texture, id, url);

            //초기화
            //framePhotoInfo = null;
        }
    }
    #endregion
}