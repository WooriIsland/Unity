using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

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

//사진
public class PhotoInfo : MonoBehaviour
{
    //수정해야 할 부분
    //시간
    [SerializeField]
    private TextMeshProUGUI timeText;
    //summary
    [SerializeField]
    private TextMeshProUGUI infoText;
    [SerializeField]
    private TMP_InputField summaryText;

    //이미지
    Texture2D picture;

    //저장 될 부분
    [SerializeField]
    private string photo_id;
    private string photo_url;

    public Image downloadImage;

    public GameObject obj;

    public void Start()
    {

    }

    //앨범 text 셋팅
    public void SetTextInfo(string time, string info, Texture2D photo, string id, string url)
    {
        timeText.text = time;
        infoText.text = info;

        //수정했을 때 고칠 수 있음
        //summaryText.text = info;

        photo_id = id;
        photo_url = url;

        OnClickDownloadImage();
        //SetImage(photo);
    }

    //앨범 이미지 셋팅
    void SetImage(Texture2D photo)
    {
        PhotoAreaScript Area = GetComponent<PhotoAreaScript>();

        //※ 저장한 이미지로 변경해야함
        //picture = Resources.Load<Texture2D>("ETC/");
        picture = photo;

        // 사용자 이미지로 대체해서 넣는다 이건 내 PC에서만 하면 됨!
        if (picture != null) Area.UserImage.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));
    }

    //이미지 통신
    //수정
    //삭제
    public void OnDeletePhotoStart()
    {
        PhotoManager.instance.PhotoDeleteMode(this.gameObject);
    }

    public void OnDeletePhoto()
    {
        AiDeletePhotoInfo aiInfo = new AiDeletePhotoInfo();

        //예시로 넣어놈
        aiInfo.photo_id = photo_id;
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
        print("사진 삭제");
        string url = "http://221.163.19.218:5137/album_delete_integ/delete";

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
        PhotoManager.instance.PhotoDeleteSuccess();
        print("Ai 삭제 성공");

        //나 삭제
        Destroy(gameObject);
    }

    //통신성공 시 생성됨 -> 정렬알고리즘 사용해서 해야함
    void OnGetPostFailed()
    {
        PhotoManager.instance.PhotoDeleteFail();

        print("Ai 사진 삭제 실패");
    }

    public void EditMode(GameObject btn)
    {
        if(btn.activeSelf == true)
        {
            btn.SetActive(false);
        }

        else
        {
            btn.SetActive(true);
        }
    }

    //수정하기
    //수정하기 버튼을 누르면 인풋필드에 내용이 적용되고 
    //이후 수정을 한 후에 오케이를 누르면 그대로 적용됨
    public void OnChangeStart()
    {
        print("사진 수정 가능");
        PhotoManager.instance.PhotoEditMode(this.gameObject, photo_id, timeText.text, infoText.text);

        //summaryText.gameObject.SetActive(true);
        
        //수정이 가능해짐
        //summaryText.text = infoText.text;
    }

    //수정 끝
    public void OnChangeEnd(string summary)
    {
        print("사진 수정 끝");
        //summaryText.gameObject.SetActive(false);
        obj.SetActive(false);

        infoText.text = summary;
        OnUpdatePhoto(infoText.text);
    }

    public void OnUpdatePhoto(string s)
    {
        AiUpdatePhotoInfo aiInfo = new AiUpdatePhotoInfo();

        //예시로 넣어놈
        aiInfo.photo_id = photo_id;
        aiInfo.island_unique_number = "11111";
        aiInfo.new_summary = s;

        //Json 형식으로 값이 들어가지게 됨 -> 이쁘게 나오기 위해 true
        string aiJsonData = JsonUtility.ToJson(aiInfo, true);
        print(aiJsonData);

        //AI 로딩 UI
        HttpManager_LHS.instance.isAichat = false;

        //AI와 채팅을 한다!
        OnUpdateGetPost(aiJsonData);
    }

    //Ai
    // 엔터 쳤을 때 -> 챗봇 보내는 내용
    // 서버에 게시물 조회 요청 -> HttpManager한테 알려주려고 함
    public void OnUpdateGetPost(string s)
    {
        string url = "http://221.163.19.218:5137/album_update_integ/update"; 

        //생성 -> 데이터 조회 -> 값을 넣어줌 
        HttpRequester_LHS requester = new HttpRequester_LHS();

        requester.SetUrl(RequestType.PUT, url, false);
        requester.body = s;
        requester.isJson = true;
        requester.isChat = false;

        requester.onComplete = OnUpdatePostComplete;
        requester.onFailed = OnUpdatePostFailed;

        HttpManager_LHS.instance.SendRequest(requester);
    }

    //직접 파싱하기
    void OnUpdatePostComplete(DownloadHandler result)
    {
        print("Ai 수정 성공");
        PhotoManager.instance.PhotoEditSuccess();
    }

    //통신성공 시 생성됨 -> 정렬알고리즘 사용해서 해야함
    void OnUpdatePostFailed()
    {
        print("Ai 수정 실패");
        PhotoManager.instance.PhotoEditFail();
    }

    //URL S3통신
    public void OnClickDownloadImage()
    {
        string url = photo_url;
        //string url = "https://jmbucket731.s3.ap-northeast-2.amazonaws.com/Family_Album_Bucket_Folder/KakaoTalk_20231030_122623506.jpg";

        //생성 -> 데이터 조회 -> 값을 넣어줌 
        HttpRequester_LHS requester = new HttpRequester_LHS();

        requester.SetUrl(RequestType.TEXTURE, url, false);
        /*requester.body = s;
        requester.isJson = true;
        requester.isChat = false;*/

        requester.onComplete = OnImagePostComplete;
        requester.onFailed = OnImageUpdatePostFailed;

        HttpManager_LHS.instance.SendRequest(requester);
    }

    void OnImagePostComplete(DownloadHandler result)
    {

        print("사진 받아오기 완료");

        Texture2D texture = ((DownloadHandlerTexture)result).texture;

        downloadImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }

    void OnImageUpdatePostFailed()
    {
        print("사진 받아오기 실패");
    }
}
