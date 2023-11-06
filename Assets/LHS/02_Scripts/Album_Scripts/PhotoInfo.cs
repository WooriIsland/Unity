using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

[System.Serializable]
public struct AiDeletePhotoInfo
{
    public string photo_id;
    public string island_unique_number;
}

public class PhotoInfo : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timeText;
    [SerializeField]
    private TextMeshProUGUI infoText;

    Texture2D picture;

    [SerializeField]
    private string photo_id;


    public void Start()
    {
    }

    //�ٹ� text ����
    public void SetTextInfo(string time, string info, Texture2D photo, string id)
    {
        timeText.text = time;
        infoText.text = info;

        photo_id = id;

        SetImage(photo);
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
        print("Ai ���� ����");

        JObject data = JObject.Parse(result.text);

        /*JArray jsonArray = data["data"].ToObject<JArray>();

        print("���� ���� : " + jsonArray.Count);

        for (int i = 0; i < jsonArray.Count; i++)
        {
            JObject json = jsonArray[i].ToObject<JObject>();
            //string iamgeData = json["binary_image"].ToObject<string>();
            string photo_datetime = json["photo_datetime"].ToObject<string>();
            string summary = json["summary"].ToObject<string>();
            string id = json["photo_id"].ToObject<string>();
        }*/
    }

    //��ż��� �� ������ -> ���ľ˰��� ����ؼ� �ؾ���

    void OnGetPostFailed()
    {
        print("Ai ���� ���� ����");
    }

}
