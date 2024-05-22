using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

//삭제
[System.Serializable]
public struct AiDeletePhotoInfo
{
    public string photo_id;
    public string island_unique_number;
}

//수정
[System.Serializable]
public struct AiUpdatePhotoInfo
{
    public string photo_id;
    public string island_unique_number;
    public string new_summary;
}

public class PhotoInfo : MonoBehaviourPun
{
    //시간
    [SerializeField]
    private TextMeshProUGUI timeText;
    //summary
    [SerializeField]
    private TextMeshProUGUI infoText;
    //주소
    [SerializeField]
    private TextMeshProUGUI locationText;
    //EditObj
    [SerializeField]
    private GameObject obj;
    [SerializeField]
    private UnityEngine.UI.Image downloadImage;

    //Background
    [SerializeField]
    private BaseAlpha background;

    [SerializeField]
    private BasePopup[] editPopUp;

    // 사진 등록 여부
    public bool isPhotoCheck;

    //해당 사진 정보
    private string photo_id;
    private string photo_url;

    //private PhotonView PV;

    //이미지
    Texture2D picture;

    private void Start()
    {
        //PV = photonView;
    }

    private void Update()
    {
        //사진이 등록되어 있다 없다 체크
        if (photo_id == null)
        {
            isPhotoCheck = false;
        }

        else
        {
            isPhotoCheck = true;
        }
    }

    //앨범 셋팅
    public void SetTextInfo(string time, string info, string location, Texture2D photo, string id, string url, bool isChristmasMap)
    {
        Debug.Log("자기자신셋팅" + time + info + location + id + url);
        timeText.text = time;
        infoText.text = info;
        locationText.text = location;

        photo_id = id;
        photo_url = url;

        if(!isChristmasMap)
        {
            //이미지 다운로드
            OnClickDownloadImage();
        }

        else
        {
            SetImage(photo);
        }
    }

    //앨범 이미지 셋팅  ※확인필요
    void SetImage(Texture2D photo)
    {
        PhotoAreaScript Area = GetComponent<PhotoAreaScript>();

        //※ 저장한 이미지로 변경해야함
        //picture = Resources.Load<Texture2D>("ETC/");
        picture = photo;

        // 사용자 이미지로 대체해서 넣는다 이건 내 PC에서만 하면 됨!
        if (picture != null) Area.UserImage.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));
    }

    private bool isCoroutineRunning = false;

    //EditUI 설정
    public void EditMode(GameObject btn)
    {
        if (btn.activeSelf == true)
        {
            background.CloseAlpha();

            for (int i = 0; i < editPopUp.Length; i++)
            {
                editPopUp[i].CloseAction();
            }

            btn.SetActive(false);
        }

        else
        {
            btn.SetActive(true);

            background.OpenAlpha();

            if (!isCoroutineRunning)
            {
                StartCoroutine(OpenBtnAction());
            }
        }
    }

    IEnumerator OpenBtnAction()
    {
        isCoroutineRunning = true;

        editPopUp[0].OpenAction();

        yield return new WaitForSeconds(0.05f);

        editPopUp[1].OpenAction();

        isCoroutineRunning = false;
    }

    #region 사진 삭제
    //1.사진 삭제 -> 나의 오브젝트 넘겨주기
    public void OnDeletePhotoStart()
    {
        obj.SetActive(false);
        PhotoManager.instance.PhotoDeleteMode(this.gameObject);
    }

    //3.PhotoMg 확인 -> 삭제 통신 실행
    public void OnDeletePhoto()
    {
        AiDeletePhotoInfo aiInfo = new AiDeletePhotoInfo();

        aiInfo.photo_id = photo_id;
        aiInfo.island_unique_number = Managers.Info.isIslandUniqueNumber; //※가족고유번호 변경

        string aiJsonData = JsonUtility.ToJson(aiInfo, true);
        Debug.Log(aiJsonData);

        HttpManager.Instance.isAichat = false;

        //AI와 채팅을 한다!
        OnGetPost(aiJsonData);
    }

    public void OnGetPost(string s)
    {
        Debug.Log("사진 삭제");
        string url = "http://221.163.19.218:5137/album_delete_integ/delete";

        HttpRequester requester = new HttpRequester(Define.RequestType.POST, Define.DataType.PHOTO, url, false);
        requester._body = s;
        requester._onComplete = OnGetPostComplete;
        requester._onFailed = OnGetPostFailed;

        HttpManager.Instance.SendRequest(requester);
    }

    //직접 파싱하기
    void OnGetPostComplete(DownloadHandler result)
    {
        Debug.Log("Ai 삭제 성공");
        PhotoManager.instance.PhotoDeleteSuccess();

        Destroy(gameObject);
    }

    //통신성공 시 생성됨 -> 정렬알고리즘 사용해서 해야함
    void OnGetPostFailed(DownloadHandler result)
    {
        Debug.Log("Ai 사진 삭제 실패");
        PhotoManager.instance.PhotoDeleteFail();
    }
    #endregion

    #region 사진 수정
    //수정하기 버튼을 누르면 인풋필드에 내용이 적용되고 
    //이후 수정을 한 후에 오케이를 누르면 그대로 적용됨
    public void OnChangeStart()
    {
        Debug.Log("사진 수정 가능");
        PhotoManager.instance.PhotoEditMode(this.gameObject, photo_id, timeText.text, infoText.text, locationText.text);
    }

    string summarySet;

    //수정 끝
    public void OnChangeEnd(string summary)
    {
        Debug.Log("사진 수정 끝");
        obj.SetActive(false);

        summarySet = summary;
        //infoText.text = summary;

        //수정통신
        OnUpdatePhoto(summarySet);
    }

    public void OnUpdatePhoto(string s)
    {
        AiUpdatePhotoInfo aiInfo = new AiUpdatePhotoInfo();

        aiInfo.photo_id = photo_id;
        aiInfo.island_unique_number = Managers.Info.isIslandUniqueNumber; //※가족고유번호 변경
        aiInfo.new_summary = s;

        string aiJsonData = JsonUtility.ToJson(aiInfo, true);
        Debug.Log(aiJsonData);

        HttpManager.Instance.isAichat = false;

        OnUpdateGetPost(aiJsonData);
    }

    public void OnUpdateGetPost(string s)
    {
        string url = "http://221.163.19.218:5137/album_update_integ/update";

        HttpRequester requester = new HttpRequester(Define.RequestType.PUT, Define.DataType.PHOTO, url, false);
        requester._body = s;
        requester._onComplete = OnUpdatePostComplete;
        requester._onFailed = OnUpdatePostFailed;

        HttpManager.Instance.SendRequest(requester);
    }

    void OnUpdatePostComplete(DownloadHandler result)
    {
        Debug.Log("Ai 수정 성공");

        infoText.text = summarySet;
        summarySet = null;

        PhotoManager.instance.PhotoEditSuccess();
    }

    void OnUpdatePostFailed(DownloadHandler result)
    {
        Debug.Log("Ai 수정 실패");
        PhotoManager.instance.PhotoEditFail();
    }
    #endregion

    #region URL S3통신
    public void OnClickDownloadImage()
    {
        string url = photo_url;

        HttpRequester requester = new HttpRequester(Define.RequestType.TEXTURE, Define.DataType.NONE, url, false);
        requester._onComplete = OnImagePostComplete;
        requester._onFailed = OnImageUpdatePostFailed;

        HttpManager.Instance.SendRequest(requester);
    }

    void OnImagePostComplete(DownloadHandler result)
    {
        Debug.Log("사진 받아오기 완료");

        Texture2D texture = ((DownloadHandlerTexture)result).texture;

        downloadImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }

    void OnImageUpdatePostFailed(DownloadHandler result)
    {
        Debug.Log("사진 받아오기 실패");
    }
    #endregion

    // 사진을 등록한다 -> Ray모드 됨
    #region 섬꾸미기 사진 등록
    //선택한 오브젝트를 앨범 UI에 보내기
    public void OnFramePhotoChange()
    {
        Invoke("DeleyChange", 0.4f);

        PhotoManager.instance.isCustomMode = false;
        PlayerManager.Instance.isAni = true;

        Debug.Log("앨범설치 4단계_1 : 변경할 오브젝트UI 프리팹 선택");
        PhotoManager.instance.FrameSetting(timeText.text, infoText.text, locationText.text, photo_id, photo_url);
    }

    void DeleyChange()
    {
        PhotoManager.instance.photoFrameUi.GetComponent<PopupPhotoDeco>().CloseAction();
    }

    //확대 일때
    public void OnFramePhotoZoom()
    {
        Debug.Log("앨범설치 3단계(Zoom) : 변경할 오브젝트UI 프리팹 전달");
        //Invoke("DeleyChange", 0.4f);

        PhotoManager.instance.isCustomMode = false;
        PlayerManager.Instance.isAni = true;
        //PhotoManager.instance.photoFrameUi.SetActive(false);

        //photonView.RPC("FrameZoomSet", RpcTarget.Others);

        //나일때만 실행되게 하기
        PhotoManager.instance.FrameZoomSet(timeText.text, infoText.text, locationText.text, photo_id, photo_url);
    }

    //할필요없을 거 같음

    [PunRPC]
    public void FrameZoomSet()
    {
        Debug.Log("앨범설치 3단계(Zoom)-2 : PunRPC" + photonView);
        PhotoManager.instance.FrameZoomSet(timeText.text, infoText.text, locationText.text, photo_id, photo_url);
    }
    #endregion
}
