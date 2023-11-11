using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

//����
[System.Serializable]
public struct AiDeletePhotoInfo
{
    public string photo_id;
    public string island_unique_number;
}

//����
[System.Serializable]
public struct AiUpdatePhotoInfo
{
    public string photo_id;
    public string island_unique_number;
    public string new_summary;
}

//����
public class PhotoInfo : MonoBehaviour
{
    //�����ؾ� �� �κ�
    //�ð�
    [SerializeField]
    private TextMeshProUGUI timeText;
    //summary
    [SerializeField]
    private TextMeshProUGUI infoText;
    [SerializeField]
    private TMP_InputField summaryText;

    //�̹���
    Texture2D picture;

    //���� �� �κ�
    [SerializeField]
    private string photo_id;
    private string photo_url;

    public Image downloadImage;

    public GameObject obj;

    public void Start()
    {

    }

    //�ٹ� text ����
    public void SetTextInfo(string time, string info, Texture2D photo, string id, string url)
    {
        timeText.text = time;
        infoText.text = info;

        //�������� �� ��ĥ �� ����
        //summaryText.text = info;

        photo_id = id;
        photo_url = url;

        OnClickDownloadImage();
        //SetImage(photo);
    }

    //�ٹ� �̹��� ����
    void SetImage(Texture2D photo)
    {
        PhotoAreaScript Area = GetComponent<PhotoAreaScript>();

        //�� ������ �̹����� �����ؾ���
        //picture = Resources.Load<Texture2D>("ETC/");
        picture = photo;

        // ����� �̹����� ��ü�ؼ� �ִ´� �̰� �� PC������ �ϸ� ��!
        if (picture != null) Area.UserImage.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));
    }

    //�̹��� ���
    //����
    //����
    public void OnDeletePhotoStart()
    {
        PhotoManager.instance.PhotoDeleteMode(this.gameObject);
    }

    public void OnDeletePhoto()
    {
        AiDeletePhotoInfo aiInfo = new AiDeletePhotoInfo();

        //���÷� �־��
        aiInfo.photo_id = photo_id;
        aiInfo.island_unique_number = "11111";

        //Json �������� ���� ������ �� -> �̻ڰ� ������ ���� true
        string aiJsonData = JsonUtility.ToJson(aiInfo, true);
        print(aiJsonData);

        //AI �ε� UI
        HttpManager_LHS.instance.isAichat = false;

        //AI�� ä���� �Ѵ�!
        OnGetPost(aiJsonData);
    }

    //Ai
    // ���� ���� �� -> ê�� ������ ����
    // ������ �Խù� ��ȸ ��û -> HttpManager���� �˷��ַ��� ��
    public void OnGetPost(string s)
    {
        print("���� ����");
        string url = "http://221.163.19.218:5137/album_delete_integ/delete";

        //���� -> ������ ��ȸ -> ���� �־��� 
        HttpRequester_LHS requester = new HttpRequester_LHS();

        requester.SetUrl(RequestType.POST, url, false);
        requester.body = s;
        requester.isJson = true;
        requester.isChat = false;

        requester.onComplete = OnGetPostComplete;
        requester.onFailed = OnGetPostFailed;

        HttpManager_LHS.instance.SendRequest(requester);
    }

    //���� �Ľ��ϱ�
    void OnGetPostComplete(DownloadHandler result)
    {
        PhotoManager.instance.PhotoDeleteSuccess();
        print("Ai ���� ����");

        //�� ����
        Destroy(gameObject);
    }

    //��ż��� �� ������ -> ���ľ˰��� ����ؼ� �ؾ���
    void OnGetPostFailed()
    {
        PhotoManager.instance.PhotoDeleteFail();

        print("Ai ���� ���� ����");
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

    //�����ϱ�
    //�����ϱ� ��ư�� ������ ��ǲ�ʵ忡 ������ ����ǰ� 
    //���� ������ �� �Ŀ� �����̸� ������ �״�� �����
    public void OnChangeStart()
    {
        print("���� ���� ����");
        PhotoManager.instance.PhotoEditMode(this.gameObject, photo_id, timeText.text, infoText.text);

        //summaryText.gameObject.SetActive(true);
        
        //������ ��������
        //summaryText.text = infoText.text;
    }

    //���� ��
    public void OnChangeEnd(string summary)
    {
        print("���� ���� ��");
        //summaryText.gameObject.SetActive(false);
        obj.SetActive(false);

        infoText.text = summary;
        OnUpdatePhoto(infoText.text);
    }

    public void OnUpdatePhoto(string s)
    {
        AiUpdatePhotoInfo aiInfo = new AiUpdatePhotoInfo();

        //���÷� �־��
        aiInfo.photo_id = photo_id;
        aiInfo.island_unique_number = "11111";
        aiInfo.new_summary = s;

        //Json �������� ���� ������ �� -> �̻ڰ� ������ ���� true
        string aiJsonData = JsonUtility.ToJson(aiInfo, true);
        print(aiJsonData);

        //AI �ε� UI
        HttpManager_LHS.instance.isAichat = false;

        //AI�� ä���� �Ѵ�!
        OnUpdateGetPost(aiJsonData);
    }

    //Ai
    // ���� ���� �� -> ê�� ������ ����
    // ������ �Խù� ��ȸ ��û -> HttpManager���� �˷��ַ��� ��
    public void OnUpdateGetPost(string s)
    {
        string url = "http://221.163.19.218:5137/album_update_integ/update"; 

        //���� -> ������ ��ȸ -> ���� �־��� 
        HttpRequester_LHS requester = new HttpRequester_LHS();

        requester.SetUrl(RequestType.PUT, url, false);
        requester.body = s;
        requester.isJson = true;
        requester.isChat = false;

        requester.onComplete = OnUpdatePostComplete;
        requester.onFailed = OnUpdatePostFailed;

        HttpManager_LHS.instance.SendRequest(requester);
    }

    //���� �Ľ��ϱ�
    void OnUpdatePostComplete(DownloadHandler result)
    {
        print("Ai ���� ����");
        PhotoManager.instance.PhotoEditSuccess();
    }

    //��ż��� �� ������ -> ���ľ˰��� ����ؼ� �ؾ���
    void OnUpdatePostFailed()
    {
        print("Ai ���� ����");
        PhotoManager.instance.PhotoEditFail();
    }

    //URL S3���
    public void OnClickDownloadImage()
    {
        string url = photo_url;
        //string url = "https://jmbucket731.s3.ap-northeast-2.amazonaws.com/Family_Album_Bucket_Folder/KakaoTalk_20231030_122623506.jpg";

        //���� -> ������ ��ȸ -> ���� �־��� 
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

        print("���� �޾ƿ��� �Ϸ�");

        Texture2D texture = ((DownloadHandlerTexture)result).texture;

        downloadImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }

    void OnImageUpdatePostFailed()
    {
        print("���� �޾ƿ��� ����");
    }
}
