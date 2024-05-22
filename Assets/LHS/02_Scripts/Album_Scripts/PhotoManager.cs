using Newtonsoft.Json.Linq;
using Photon.Pun;
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
public class PhotoManager : MonoBehaviourPunCallbacks
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
    private GameObject frameItim;

    [Header("섬꾸미기Photo")]
    public GameObject photoFrameUi;
    public BaseAlpha photoFrameAlpha;
    public GameObject photoStart1;
    public GameObject photoStart2;

    //전시 튜토리얼 한번만 보여주기 위해
    public int FrameTutorial = 0;

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
    public GameObject noPictureFrame;

    [Header("통신 로딩 UI")]
    public GameObject loding;

    [Header("사진상호작용 UI")]
    public GameObject photoPopup;
    public Transform photoPopupContent;
    public MainUISlide mainUiSlide;

    private List<PhotoInfo> photoList;

    //앨범 조회 OR 섬꾸미기앨범 조건
    private bool isBookCheck;
    private PhotoInfo photo;

    //수정 삭제시 선택오브젝트를 알기 위한 변수
    GameObject photoObj;

    public bool isCustomMode;

    //크리스마스를 위한
    private bool isChristmasMap;
    Texture2D christmasMapPhoto;

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

        //포토팝업을 자기가 생성해서 자기꺼만 실행되게 만들면 됨
        //인스턴스화 할때 해당 객체의 PhotonVeiw를 쓰고 싶으면 PhotonNetwork.Instantiate로 진행해야함.
        /*GameObject go = PhotonNetwork.Instantiate(photoPopup.name, Vector3.zero, Quaternion.identity);
        go.transform.SetParent(photoPopupContent);
        go.transform.localScale = new Vector3(1, 1, 1);
        //RectTransform = > 0으로 셋팅
        RectTransform rectTransform = go.GetComponent<RectTransform>();
        rectTransform.sizeDelta = Vector2.zero;*/

        GameObject go = PhotonNetwork.Instantiate(photoPopup.name, Vector3.zero, Quaternion.identity);
        go.transform.SetParent(photoPopupContent);
        go.transform.SetSiblingIndex(1);
        go.transform.localScale = Vector3.one;
        //RectTransform = > 0으로 셋팅
        RectTransform rectTransform = go.GetComponent<RectTransform>();

        // Anchors 설정 (부모 컨테이너에 맞추기 위해 필요한 경우)
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;

        // Offset 설정 (부모 컨테이너에 맞추기 위해 필요한 경우)
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        // 크기 설정
        rectTransform.sizeDelta = Vector2.zero;

        //그리고 비활성화
        go.SetActive(false);

        photoPopup = go;
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            print("확인용111111111111111" + framePhotoInfo.name);
        }
    }

    #region 안면등록 1개 (form-data)
    public void OnFaceUpload(byte[] Array)
    {
        byte[] readFile = Array;
        Debug.Log(readFile.Length);

        WWWForm form = new WWWForm();

        form.AddField("island_unique_number", Managers.Info.isIslandUniqueNumber); //※유저 고유 가족키
        form.AddField("user_id", Managers.Info.userId.ToString()); //※유저 고유 번호
        form.AddField("user_nickname", Managers.Info.NickName); //※유저 고유 닉네임
        //이미지
        form.AddBinaryData("face_image", readFile, "F0011_IND_D_13_0_01.jpg"); //이미지 여러개 가능?

        HttpManager.Instance.SendPhoto(form, OnFaceSuccess, OnFaceFailed, true);
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

        form.AddField("user_id", Managers.Info.userId.ToString()); //※유저아이디 변경

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

        HttpManager.Instance.SendPhoto(form, OnSuccess, OnFailed, false);
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
        noPictureFrame.SetActive(false);

        isBookCheck = isBook;
        print(isBookCheck);

        //사진 초기화
        OnDestroyPhoto(isBookCheck);

        AiPhotoInfo aiInfo = new AiPhotoInfo();

        aiInfo.island_unique_number = Managers.Info.isIslandUniqueNumber; //※가족섬고유번호 변경
        aiInfo.user_id = Managers.Info.userId.ToString(); //※유저아이디 변경

        //Json 형식으로 값이 들어가지게 됨 -> 이쁘게 나오기 위해 true
        string aiJsonData = JsonUtility.ToJson(aiInfo, true);
        print(aiJsonData);

        //로딩 UI
        HttpManager.Instance.isPhoto = true;

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
            Debug.Log("앨범기본");
        }

        else
        {
            url = urlDeco;
            Debug.Log("앨범데코");
        }

        HttpRequester requester = new HttpRequester(Define.RequestType.POST, Define.DataType.JSON, url, false);
        requester._body = s;
        requester.IsPhoto = true; // 변지환 : 현숙 코드만 예외적으로 IsPhoto 프로퍼티에 접근하여 _isPhoto값을 변경하도록 함
        requester._onComplete = OnGetPostComplete;
        requester._onFailed = OnGetPostFailed;

        HttpManager.Instance.SendRequest(requester);
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
            noPictureFrame.SetActive(true);
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

        if(!isBookCheck && FrameTutorial == 1)
        {
            print("사진 등록");
            photoStart1.GetComponent<BasePopup>().OpenAction();
            photoStart2.GetComponent<BaseAlpha>().OpenAlpha();
        }
    }

    void OnGetPostFailed(DownloadHandler result)
    {
        noPicture.SetActive(true);
        noPictureFrame.SetActive(true);
        print("사진 조회 실패");
    }
    #endregion

    #region 키워드조회 True or 섬꾸미기키워드조회 False (유저 입력한 값 들어가야함)
    public void OnSearchInquiry(bool isBook)
    {
        noPicture.SetActive(false);
        noPictureFrame.SetActive(false);


        isBookCheck = isBook;
        print(isBookCheck);

        OnDestroyPhoto(isBookCheck);

        AiSearchPhotoInfo aiInfo = new AiSearchPhotoInfo();

        aiInfo.island_unique_number = Managers.Info.isIslandUniqueNumber; //※가족섬고유번호 변경

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

        HttpManager.Instance.isAichat = false;

        OnSearchGetPost(aiJsonData);

        //초기화
        summaryText.text = null;
        summaryFrameText.text = null;
    }

    public void OnSearchGetPost(string s)
    {
        string url = "http://221.163.19.218:5137/album_search_integ/search";

        HttpRequester requester = new HttpRequester(Define.RequestType.POST, Define.DataType.JSON, url, false);
        requester._body = s;
        requester.IsPhoto = true; // 변지환 : 현숙 코드만 예외적으로 IsPhoto 프로퍼티에 접근하여 _isPhoto값을 변경하도록 함
        requester._onComplete = OnSearchGetPostComplete;
        requester._onFailed = OnSearchGetPostFailed;

        HttpManager.Instance.SendRequest(requester);
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
            noPictureFrame.SetActive(true);
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
        noPictureFrame.SetActive(true);
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
        editMode.time.text = time;
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
            print("앨범");
            photo = Instantiate(photoItim, photoContent);
        }

        else
        {
            print("데코");
            //photo = Instantiate(frameItim, photoFrameContent);

            //인스턴스화 할때 해당 객체의 PhotonVeiw를 쓰고 싶으면 PhotonNetwork.Instantiate로 진행해야함.
            GameObject go = PhotonNetwork.Instantiate(frameItim.name, Vector3.zero, Quaternion.identity);
            go.transform.SetParent(photoFrameContent);
            go.transform.localScale = new Vector3(1,1,1);
            photo = go.GetComponent<PhotoInfo>();
        }

        //하이어라키 창에 들어가는 순서를 바꾸는 것
        photo.transform.SetSiblingIndex(0);

        //각자 자신의 Obj 셋팅
        photo.SetTextInfo(time, summary, location, texture, id, url, isChristmasMap);

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
    string saveTime;
    string saveSummary;
    string saveLocation;
    string saveID;
    string saveURL;

    //선택한 Obj
    public void FrameObject(GameObject obj)
    {
        print("앨범설치 3단계 : 나의 오브젝트 저장해두기" + obj);
        framePhotoInfo = obj.GetComponentInChildren<PhotoInfo>();
    }
    
    public bool isZoom = false;

    public void FrameSetting(string time, string summary, string location, string id, string url)
    {
        print("앨범설치 5단계 : PhotoManagr에 있는 오브젝트의 정보값을 변경");

        print(time + summary+  location + id + url);

        //선택한 오브젝트가 null이 아니라면 && framePhotoPopup == null
        //줌모드가 아닐 때
        if (framePhotoInfo != null && isZoom == false)
        {
            print("앨범설치 5단계_1 :" + framePhotoInfo + "의 정보 다시 셋팅");

            //상대
            photonView.RPC("FramePhoto", RpcTarget.All, time, summary, location, id, url);

            //나
            //Texture2D texture = new Texture2D(0, 0);
            //framePhotoInfo.SetTextInfo(time, summary, location, texture, id, url);

            //초기화
            //framePhotoInfo = null;
        }

        // 줌 팝업의 단계를 하나 더 거쳐야함
        else if(framePhotoPopup != null && isZoom == true)
        {
            print("앨범설치 5단계_2 :" + framePhotoPopup + "의 정보 다시 셋팅");

            photonView.RPC("FramePhotoZoom", RpcTarget.All, time, summary, location, id, url);

            //Texture2D texture = new Texture2D(0, 0);
            //framePhotoPopup.SetTextInfo(time, summary, location, texture, id, url);

            isZoom = false;
        }
    }

    [PunRPC]
    void FramePhoto(string time, string summary, string location, string id, string url)
    {
        Texture2D texture = new Texture2D(0, 0);
        framePhotoInfo.SetTextInfo(time, summary, location, texture, id, url, isChristmasMap);
    }

    [PunRPC]
    void FramePhotoZoom(string time, string summary, string location, string id, string url)
    {
        Texture2D texture = new Texture2D(0, 0);
        framePhotoPopup.SetTextInfo(time, summary, location, texture, id, url, isChristmasMap);
    }

    public void OnZoomCheck()
    {
        isZoom = true;
    }

    #endregion

    #region 섬꾸미기 사진 Popup 상호작용

    PhotoInfo framePhotoPopup;
    GameObject framePhotoPopupObj;

    List<GameObject> framePhotoPopupList;

    public void OnPhotoPopup(GameObject obj, string nickName)
    {
        print("앨범설치 2단계(Zoom) : 나의 오브젝트 저장해두기" + obj);

        //팝업창 뜨기 셋팅
        /*photoPopup.GetComponent<BasePopup>().OpenAction();
        photoPopup.GetComponentInChildren<PhotoClick>().ClickAction();

        //설치 오브젝트 꺼주기
        mainUiSlide.CloseAction();*/

        //※ 켜지면서 해당 선택한 오브젝트의 정보를 받아서 넣어준다. => framePhotoInfo 얘도 가능할 거 같은데

        /* if(isMineCheck)
        {
            print("나 일때만 셋팅");
            framePhotoPopup = obj.GetComponentInChildren<PhotoInfo>();
            // 방장만 isMine일텐데
            framePhotoPopup.OnFramePhotoZoom();
        }*/
        //*************** 구조변경 필요*********************//
        if(framePhotoPopupObj != null)
        {
            if (framePhotoPopupObj.GetComponentInChildren<PhotonView>().ViewID == obj.GetComponentInChildren<PhotonView>().ViewID)
            {
                print("같은 오브젝트 고친느 중");
                framePhotoPopupObj = obj;
                framePhotoPopup = framePhotoPopupObj.GetComponentInChildren<PhotoInfo>();
            }

            else
            {
                print("다른 오브젝트 고치는 중");
                framePhotoPopupList.Add(obj);

                framePhotoPopupObj = obj;
                framePhotoPopup = framePhotoPopupObj.GetComponentInChildren<PhotoInfo>();
            }

            // 일단 넣기
            // 고치는 코드를 배열 중에 같은 애로 바뀔 수 있게 해야함

        }

        else
        {
            framePhotoPopupObj = obj;
            framePhotoPopup = framePhotoPopupObj.GetComponentInChildren<PhotoInfo>();
        }

        if (nickName == photoPopup.GetComponent<PhotonView>().Owner.NickName)
        {
            print("나 일때만 셋팅");
            // 방장만 isMine일텐데
            //framePhotoPopup = obj.GetComponentInChildren<PhotoInfo>();
            framePhotoPopup.OnFramePhotoZoom();
        }

        //isZoom = true;
    }

    public bool isMineCheck;
    public void OnPhotoPopupSet(Texture2D photo, bool isChristmas, bool isCheck)
    {
        //팝업창 뜨기 셋팅
        photoPopup.GetComponent<BasePopup>().OpenAction();
        photoPopup.GetComponentInChildren<PhotoClick>().ClickAction();

        //설치 오브젝트 꺼주기
        mainUiSlide.CloseAction();

        //크리스마스 적용
        if(isChristmas)
        {
            isChristmasMap = isChristmas;
            christmasMapPhoto = photo;
        }

        isMineCheck = isCheck;

        isZoom = true;
    }

    public void FrameZoomSet(string time, string summary, string location, string id, string url)
    {
        print(time + summary + location + id + url);

        print("앨범설치 4단계(Zoom) : PhotoManagr에 있는 오브젝트의 정보값을 변경");

        if (framePhotoPopup != null)
        {
            print("앨범설치 4단계(Zoom) :" + photoPopup + "의 정보 다시 셋팅");

            //셋팅 내껄로!
            // ※ 얘는 내꺼만 해도 되지 않나 ?

            if(isMineCheck)
            {
                FrameZoom(time, summary, location, id, url);
            }
            //내꺼의 정보만 ...!
            //photonView.RPC("FrameZoom", RpcTarget.All, time, summary, location, id, url);
        }
    }

    [PunRPC]
    void FrameZoom(string time, string summary, string location, string id, string url)
    {
        print("줌셋팅내용" + time + summary + location + id + url);
        Texture2D texture;

        if (isChristmasMap)
        {
           texture = christmasMapPhoto;
        }
        else
        {
            texture = new Texture2D(0, 0);
        }

        //크리스마스 섬이면 ~!
        //아니면 저렇게 통신하기
        //얘는 나만!!1
        photoPopup.GetComponentInChildren<PhotoInfo>().SetTextInfo(time, summary, location, texture, id, url, isChristmasMap);
    }

    //껐을때 실행될 수 있게 해야함.
    public void OnPhotoDwon()
    {
        photoPopup.GetComponent<BasePopup>().CloseAction();
        mainUiSlide.OpenAction();

        //PoptoPopup에 있는 정보를 framePhotoPhotoPopup에 넣어야 함
        //photoPopup.GetComponentInChildren<PhotoInfo>().OnFramePhotoChange();
    }
    #endregion

    // 조회일때 Ray안되게 하기 위해
    public bool isPhotoMode = false;

    public void OnPhotoing()
    {
        isPhotoMode = true;
        print("앨범모드실행");
    }

    public void OffPhotoing()
    {
        isPhotoMode = false;
    }
}