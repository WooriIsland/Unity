using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Networking;
using UnityEngine.UI;

[Serializable]
public class GPSObjectInfo
{
    //��������Ű
    public int island_id;
    //GPS �ǹ� �̸�
    public string building_name;
    //GPS ��ġ ������Ʈ ��ȣ
    public int building_index;
    //����
    public float building_latitude;
    //�浵
    public float building_longitude;
    //��ġ
    public Vector3 building_pos;
    //ȸ��
    public Quaternion building_rot;
}

//�޴� ����
[Serializable]
public class GPSObjectList<T>
{
    public List<T> gpsObjectList;
}

public class GPSManager : MonoBehaviour
{
    //�ؽ�Ʈ UI
    /*public TextMeshProUGUI latitude_text;
    public TextMeshProUGUI longitude_text;*/
    //���� ���� �ؾ���
    public TextMeshProUGUI json_text;

    //���� �浵 
    public float latitude;
    public float longitude;

    //�ִ�������ð�
    public float maxWaitTime = 10.0f;
    //��ġ�������Žð�
    public float resendTime = 1.0f;

    //�������ȴ��ð�
    float waitTime = 0;
    bool receiveGPS = false;

    //UI
    public GameObject gpsOffUI;
    public GameObject gpsOnUI;

    //�����ؾ��� ��
    public TMP_InputField inputGPSName;

    public Button btnGps;
    public Button btnGpsName;
    public Button btnGpsObject;

    public PlacementSystem placementSystem;
    public TextMeshProUGUI gpsName_text;

    //GPS �ǹ� �̸�
    string gpsNameinfo;
    //GPS ��ġ ������Ʈ ��ȣ
    int gpsNuminfo;
    //����
    float latitudeinfo;
    //�浵
    float longitudeinfo;

    //Ui bool
    bool isGps = false;

    public GameObject gpsObject;

    //----- GPS ����ġ üũ ------//
    //unityCoor�� ���� ����
    public Vector3 unityCoor;
    public Vector3 currentLocation;

    public void Start()
    {
        // �̸��Է� ĭ�� ����ɶ� ȣ��Ǵ� �Լ� ���
        //inputGPSName.onValueChanged.AddListener(OnGPSNameValueChanged);

        // �̸��Է�ĭ�� ����ɶ� ȣ��Ǵ� �Լ���� (�Է°� ����)
        btnGps.onClick.AddListener(() => OnGpsSave());
        btnGpsName.onClick.AddListener(() => OnGpsName());
        btnGpsObject.onClick.AddListener(() => OnGpsObject());
    }

    public void Update()
    {
   
    }

    //0.����Ҹ����
    public void OnGPS()
    {
        isGps = true;

        //GPS �����ð��� �ֱ� ������ �ڷ�ƾ ���
        StartCoroutine(GPS_On());
        print("GPS ���");
    }

    public IEnumerator GPS_On()
    {
        //GPS ����㰡 �� ��ġ ���� Ȯ�� (using UnityEngine.Android; �ʿ�)
        //����, GPS ��� �㰡�� ���� ���ߴٸ�, ���� �㰡 �˾��� ����.
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);

            while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                yield return null;
            }
        }

        //����, GPS ��ġ�� ���� ���� ������ ��ġ ������ ������ �� ���ٰ� ǥ���Ѵ�.
        //��ġ ���� ���� �Ӽ��̳� �Լ� -> Input.location
        if (!Input.location.isEnabledByUser)
        {
            //latitude_text.text = "GPS off";
            //longitude_text.text = "GPS off";
            print("GPS off");
            gpsOffUI.SetActive(true);
            PlaceJsonSave();
            //gps ��� �˾��߰��ϱ�
            yield break;
        }

        //��ġ �����͸� ��û -> ���Ŵ��
        Input.location.Start();

        //GPS ���Ż��°� �ʱ� ���¿��� ���� �ð� ���� ���
        while (Input.location.status == LocationServiceStatus.Initializing && waitTime < maxWaitTime)
        {
            yield return new WaitForSeconds(1.0f);
            waitTime++;
        }

        //���� ���� �� ������ ���еƴٴ� ���� ���
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            /*latitude_text.text = "��ġ ���� ���� ����";
            longitude_text.text = "��ġ ���� ���� ����";*/
        }

        //���� ��� �ð��� �Ѿ���� ������ �����ٸ� �ð� �ʰ������� ���
        if (waitTime >= maxWaitTime)
        {
            /*latitude_text.text = "���� ��� �ð� �ʰ�";
            longitude_text.text = "���� ��� �ð� �ʰ�";*/
        }

        //���ŵ� GPS�����͸� ȭ�鿡 ���
        LocationInfo li = Input.location.lastData;
        latitude = li.latitude;
        longitude = li.longitude;

        /*latitude_text.text = "���� :" + latitude.ToString();
        longitude_text.text = "�浵 :" + longitude.ToString();*/

        if(isGps == true)
        {
            //gps ��� UI �߱�
            gpsOnUI.SetActive(true);
        }

        else
        {
            gpsOnUI.SetActive(false);
        }


        //��ġ ���� ���� ���� üũ
        receiveGPS = true;

        //��ġ ������ ���� ���� ���� resendTime ������� ��ġ ������ �����ϰ� ���
        while (receiveGPS)
        {

            li = Input.location.lastData;
            latitude = li.latitude;
            longitude = li.longitude;

            //GPSEncoder�� GPSToUCS ���
            unityCoor = GPSEncoder.GPSToUCS(latitude, longitude);
            //json_text.text = unityCoor.ToString();

            if (isGps == true)
            {
                //gps ��� UI �߱�
                gpsOnUI.SetActive(true);
            }

            else
            {
                gpsOnUI.SetActive(false);
            }


            /*latitude_text.text = "���� :" + latitude.ToString();
            longitude_text.text = "�浵 :" + longitude.ToString();*/

            yield return new WaitForSeconds(resendTime);
        }
    }

    //1.���� �浵 ����
    public void OnGpsSave()
    {
        latitudeinfo = this.latitude;
        longitudeinfo = this.longitude;
    }

    //2.�̸� ����
    /*private void OnGPSNameValueChanged(string s)
    {
        //�Է°��� 0���� Ŭ��
        btnGps.interactable = s.Length > 0;
    }*/


    public void OnGpsName()
    {
        gpsNameinfo = inputGPSName.text;
        gpsName_text.text = gpsNameinfo;
    }

    //3.������Ʈ �ε��� ����
    public void OnGpsObjectIndex(int num)
    {
        gpsNuminfo = num;
    }

    public void OnGpsObject()
    {
        isGps = false;
        //�ٹ̱� ��� Ȱ��ȭ
        placementSystem.StartPlacement(gpsNuminfo);
        PlaceJsonSave();
    }

    //����ڰ� ���ϴ� ����, �浵 �ֺ��� �ִ��� Ȯ��
    private void PlaceJsonSave()
    {
        //���ϴ� ��ġ ����ŭ �ݺ�
        //���� ��ġ�� ���ϴ� ��ġ�� �Ÿ��� ���� ���� ���� ������ isInPlace = false, �ݺ��� continue
        //���� �� ���ٸ� isInPlace = true, ��ġ �̸��� place�� ����
        GPSObjectInfo gpsObjectinfo = new GPSObjectInfo()
        {
            island_id = 1,//ó������
            building_latitude = latitudeinfo, //�޾ƿ� ��
            building_longitude = longitudeinfo, //�޾ƿ� ��
            building_name = gpsNameinfo, //���� �г��� + ����� �Է�
            building_index = gpsNuminfo, //����� ����
            building_pos = gpsObject.transform.position, //����� ����
            building_rot = gpsObject.transform.rotation, //����� ����
        };

        //���� ���� (�����)
        string filePath = Path.Combine(Application.persistentDataPath, "data.txt");

        string json = JsonUtility.ToJson(gpsObjectinfo, true);
        //json_text.text = "���Ͼ���" + json + "GPS" + unityCoor;

        // ��� ������
        Debug.Log(json);
        //File.WriteAllText(filePath, json);

        //AI �ε� UI
        HttpManager_LHS.instance.isAichat = false;

        //AI�� ä���� �Ѵ�!
        OnGetPost(json);
    }

    //Ai
    // ���� ���� �� -> ê�� ������ ����
    // ������ �Խù� ��ȸ ��û -> HttpManager���� �˷��ַ��� ��
    public void OnGetPost(string s)
    {
        print("������Ʈ ���� ����غ���");
        string url = "http://192.168.0.53:8082/api/building-location-info";

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
        print("������Ʈ �������� ����");
        JObject data = JObject.Parse(result.text);

        /*JObject data = JObject.Parse(result.text);

        JArray jsonArray = data["data"].ToObject<JArray>();

        print("���� ���� : " + jsonArray.Count);

        for (int i = 0; i < jsonArray.Count; i++)
        {
            JObject json = jsonArray[i].ToObject<JObject>();
            //string iamgeData = json["binary_image"].ToObject<string>();
            string photo_datetime = json["photo_datetime"].ToObject<string>();
            string summary = json["summary"].ToObject<string>();
            string id = json["photo_id"].ToObject<string>();
            string image = json["photo_image"].ToObject<string>();

            #region �迭
            *//*JArray character = json["character"].ToObject<JArray>();

            List<string> list = new List<string>();
            for(int j = 0; j < character.Count; j++)
            {
                 list.Add(character[j].ToObject<string>());
            }*//*

            //if (i == 0)
            #endregion
        }*/
    }


    void OnGetPostFailed()
    {
        print("������Ʈ �������� ����");
    }

    public void OnPlaceLode()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "data.txt");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            GPSObjectInfo gpsObjectInfo = JsonUtility.FromJson<GPSObjectInfo>(json);
            //json_text.text = "�����б�" + json;
        }

        else
        {
            //json_text.text = "�о� �� ������ �����ϴ�.";
        }
    }

    public void DetectPlace()
    {
        //if(Vector3.Magnitude(currentLocation - GPSEncoder.GPSToUCS())
    }
}
