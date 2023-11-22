using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using OpenCover.Framework.Model;

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

public class PhotoInfo : MonoBehaviour
{
    //�ð�
    [SerializeField]
    private TextMeshProUGUI timeText;
    //summary
    [SerializeField]
    private TextMeshProUGUI infoText;
    //�ּ�
    [SerializeField]
    private TextMeshProUGUI locationText;
    //EditObj
    [SerializeField]
    private GameObject obj;
    [SerializeField]
    private Image downloadImage;

    //�ش� ���� ����
    private string photo_id;
    private string photo_url;

    //�̹���
    Texture2D picture;

    //�ٹ� ����
    public void SetTextInfo(string time, string info, string location, Texture2D photo, string id, string url)
    {
        timeText.text = time;
        infoText.text = info;
        locationText.text = location;

        photo_id = id;
        photo_url = url;

        //�̹��� �ٿ�ε�
        OnClickDownloadImage();
    }

    //�ٹ� �̹��� ����  ��Ȯ���ʿ�
    void SetImage(Texture2D photo)
    {
        PhotoAreaScript Area = GetComponent<PhotoAreaScript>();

        //�� ������ �̹����� �����ؾ���
        //picture = Resources.Load<Texture2D>("ETC/");
        picture = photo;

        // ����� �̹����� ��ü�ؼ� �ִ´� �̰� �� PC������ �ϸ� ��!
        if (picture != null) Area.UserImage.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));
    }

    //EditUI ����
    public void EditMode(GameObject btn)
    {
        if (btn.activeSelf == true)
        {
            btn.SetActive(false);
        }

        else
        {
            btn.SetActive(true);
        }
    }

    #region ���� ����
    //1.���� ���� -> ���� ������Ʈ �Ѱ��ֱ�
    public void OnDeletePhotoStart()
    {
        PhotoManager.instance.PhotoDeleteMode(this.gameObject);
    }

    //3.PhotoMg Ȯ�� -> ���� ��� ����
    public void OnDeletePhoto()
    {
        AiDeletePhotoInfo aiInfo = new AiDeletePhotoInfo();

        aiInfo.photo_id = photo_id;
        aiInfo.island_unique_number = "11111"; //�ذ���������ȣ ����

        string aiJsonData = JsonUtility.ToJson(aiInfo, true);
        print(aiJsonData);

        HttpManager_LHS.instance.isAichat = false;

        //AI�� ä���� �Ѵ�!
        OnGetPost(aiJsonData);
    }

    public void OnGetPost(string s)
    {
        print("���� ����");
        string url = "http://221.163.19.218:5137/album_delete_integ/delete";

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
        print("Ai ���� ����");
        PhotoManager.instance.PhotoDeleteSuccess();

        Destroy(gameObject);
    }

    //��ż��� �� ������ -> ���ľ˰��� ����ؼ� �ؾ���
    void OnGetPostFailed(DownloadHandler result)
    {
        print("Ai ���� ���� ����");
        PhotoManager.instance.PhotoDeleteFail();
    }
    #endregion

    #region ���� ����
    //�����ϱ� ��ư�� ������ ��ǲ�ʵ忡 ������ ����ǰ� 
    //���� ������ �� �Ŀ� �����̸� ������ �״�� �����
    public void OnChangeStart()
    {
        print("���� ���� ����");
        PhotoManager.instance.PhotoEditMode(this.gameObject, photo_id, timeText.text, infoText.text, locationText.text);
    }

    string summarySet;

    //���� ��
    public void OnChangeEnd(string summary)
    {
        print("���� ���� ��");
        obj.SetActive(false);

        summarySet = summary;
        //infoText.text = summary;

        //�������
        OnUpdatePhoto(summarySet);
    }

    public void OnUpdatePhoto(string s)
    {
        AiUpdatePhotoInfo aiInfo = new AiUpdatePhotoInfo();

        aiInfo.photo_id = photo_id;
        aiInfo.island_unique_number = "11111"; //�ذ���������ȣ ����
        aiInfo.new_summary = s;

        string aiJsonData = JsonUtility.ToJson(aiInfo, true);
        print(aiJsonData);

        HttpManager_LHS.instance.isAichat = false;

        OnUpdateGetPost(aiJsonData);
    }

    public void OnUpdateGetPost(string s)
    {
        string url = "http://221.163.19.218:5137/album_update_integ/update";

        HttpRequester_LHS requester = new HttpRequester_LHS();

        requester.SetUrl(RequestType.PUT, url, false);
        requester.body = s;
        requester.isJson = true;
        requester.isChat = false;

        requester.onComplete = OnUpdatePostComplete;
        requester.onFailed = OnUpdatePostFailed;

        HttpManager_LHS.instance.SendRequest(requester);
    }

    void OnUpdatePostComplete(DownloadHandler result)
    {
        print("Ai ���� ����");

        infoText.text = summarySet;
        summarySet = null;

        PhotoManager.instance.PhotoEditSuccess();
    }

    void OnUpdatePostFailed(DownloadHandler result)
    {
        print("Ai ���� ����");
        PhotoManager.instance.PhotoEditFail();
    }
    #endregion

    #region URL S3���
    public void OnClickDownloadImage()
    {
        string url = photo_url;

        HttpRequester_LHS requester = new HttpRequester_LHS();

        requester.SetUrl(RequestType.TEXTURE, url, false);

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

    void OnImageUpdatePostFailed(DownloadHandler result)
    {
        print("���� �޾ƿ��� ����");
    }
    #endregion

    #region ���ٹ̱� ���� ���
    //������ ������Ʈ�� �ٹ� UI�� ������
    public void OnFramePhotoChange()
    {
        PhotoManager.instance.photoFrameUi.SetActive(false);

        print("����2 �� ���� ������");
        PhotoManager.instance.FrameSetting(timeText.text, infoText.text, locationText.text, photo_id, photo_url);
    }
    #endregion
}
