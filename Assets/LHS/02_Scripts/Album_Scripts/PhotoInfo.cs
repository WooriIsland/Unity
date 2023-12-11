using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

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

public class PhotoInfo : MonoBehaviourPun
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
    private UnityEngine.UI.Image downloadImage;

    //Background
    [SerializeField]
    private BaseAlpha background;

    [SerializeField]
    private BasePopup[] editPopUp;

    // ���� ��� ����
    public bool isPhotoCheck;

    //�ش� ���� ����
    private string photo_id;
    private string photo_url;

    //private PhotonView PV;

    //�̹���
    Texture2D picture;

    private void Start()
    {
        //PV = photonView;
    }

    private void Update()
    {
        //������ ��ϵǾ� �ִ� ���� üũ
        if (photo_id == null)
        {
            isPhotoCheck = false;
        }

        else
        {
            isPhotoCheck = true;
        }
    }

    //�ٹ� ����
    public void SetTextInfo(string time, string info, string location, Texture2D photo, string id, string url, bool isChristmasMap)
    {
        print("�ڱ��ڽż���" + time + info + location + id + url);
        timeText.text = time;
        infoText.text = info;
        locationText.text = location;

        photo_id = id;
        photo_url = url;

        if(!isChristmasMap)
        {
            //�̹��� �ٿ�ε�
            OnClickDownloadImage();
        }

        else
        {
            SetImage(photo);
        }
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

    private bool isCoroutineRunning = false;

    //EditUI ����
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

    #region ���� ����
    //1.���� ���� -> ���� ������Ʈ �Ѱ��ֱ�
    public void OnDeletePhotoStart()
    {
        obj.SetActive(false);
        PhotoManager.instance.PhotoDeleteMode(this.gameObject);
    }

    //3.PhotoMg Ȯ�� -> ���� ��� ����
    public void OnDeletePhoto()
    {
        AiDeletePhotoInfo aiInfo = new AiDeletePhotoInfo();

        aiInfo.photo_id = photo_id;
        aiInfo.island_unique_number = InfoManager.Instance.isIslandUniqueNumber; //�ذ���������ȣ ����

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
        requester.isPhoto = true;

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
        aiInfo.island_unique_number = InfoManager.Instance.isIslandUniqueNumber; //�ذ���������ȣ ����
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
        requester.isPhoto = true;

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

    // ������ ����Ѵ� -> Ray��� ��
    #region ���ٹ̱� ���� ���
    //������ ������Ʈ�� �ٹ� UI�� ������
    public void OnFramePhotoChange()
    {
        Invoke("DeleyChange", 0.4f);

        PhotoManager.instance.isCustomMode = false;
        PlayerManager.Instance.isAni = true;

        print("�ٹ���ġ 4�ܰ�_1 : ������ ������ƮUI ������ ����");
        PhotoManager.instance.FrameSetting(timeText.text, infoText.text, locationText.text, photo_id, photo_url);
    }

    void DeleyChange()
    {
        PhotoManager.instance.photoFrameUi.GetComponent<PopupPhotoDeco>().CloseAction();
    }

    //Ȯ�� �϶�
    public void OnFramePhotoZoom()
    {
        print("�ٹ���ġ 3�ܰ�(Zoom) : ������ ������ƮUI ������ ����");
        //Invoke("DeleyChange", 0.4f);

        PhotoManager.instance.isCustomMode = false;
        PlayerManager.Instance.isAni = true;
        //PhotoManager.instance.photoFrameUi.SetActive(false);

        //photonView.RPC("FrameZoomSet", RpcTarget.Others);

        //���϶��� ����ǰ� �ϱ�
        PhotoManager.instance.FrameZoomSet(timeText.text, infoText.text, locationText.text, photo_id, photo_url);
    }

    //���ʿ���� �� ����

    [PunRPC]
    public void FrameZoomSet()
    {
        print("�ٹ���ġ 3�ܰ�(Zoom)-2 : PunRPC" + photonView);
        PhotoManager.instance.FrameZoomSet(timeText.text, infoText.text, locationText.text, photo_id, photo_url);
    }
    #endregion
}
