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
    [Header("�ִ�������ð�")]
    public float maxWaitTime = 10.0f;
    [Header("��ġ�������Žð�")]
    public float resendTime = 1.0f;

    [Header("GPS ON/OFF")]
    public GameObject gpsOffUI;
    public GameObject gpsOnUI;

    [Header("GPS Name")]
    public TMP_InputField inputGPSName;
    public TextMeshProUGUI gpsName_text;

    [Header("GPS Btn")]
    public Button btnGps;
    public Button btnGpsName;
    public Button btnGpsObject;

    [Header("�ٹ̱���")]
    public PlacementSystem placementSystem;

    //������
    [Header("��ǥ����")]
    public double TargetLatitude, TargetLongitude;

    //�������ȴ��ð�
    float waitTime = 0;
    //GPS �����ִ��� Ȯ��
    bool receiveGPS = false;

    // ���� ���� �浵 
    float latitude;
    float longitude;
    //���� �� ���� �浵
    float latitudeinfo;
    float longitudeinfo;
    //GPS �ǹ� �̸�
    string gpsNameinfo;
    //GPS ��ġ ������Ʈ ��ȣ
    int gpsNuminfo;

    //Ui bool
    bool isGps = false;

    // ���� GPS ���� �ݿø��� ����
    double MyLatitude, MyLongtitude;

    //unityCoor�� ���� ���� -> ����� ���ص��� (������˰��ֱ�)
    Vector3 unityCoor;

    public void Start()
    {
        // �̸��Է�ĭ�� ����ɶ� ȣ��Ǵ� �Լ���� (�Է°� ����)
        btnGps.onClick.AddListener(() => OnGpsSave());
        btnGpsName.onClick.AddListener(() => OnGpsName());
        btnGpsObject.onClick.AddListener(() => OnGpsObject());
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
        print("����üũ���̴�GPS" + "-latitude" + latitude + "-longitude" + longitude);
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

            print("����üũ���̴�GPS" + "-latitude" + latitude + "-longitude" + longitude);

            /*latitude_text.text = "���� :" + latitude.ToString();
            longitude_text.text = "�浵 :" + longitude.ToString();*/

            getUpdateedGPSstring();

            yield return new WaitForSeconds(resendTime);
        }
    }

    //1.���� �浵 ����
    public void OnGpsSave()
    {
        latitudeinfo = this.latitude;
        longitudeinfo = this.longitude;
    }

    //2. �̸� ����
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

    //4. �ٹ̱���
    public void OnGpsObject()
    {
        isGps = false;

        //�ٹ̱� ��� Ȱ��ȭ
        placementSystem.StartPlacement(gpsNuminfo);
        PlaceJsonSave();
    }

    private void PlaceJsonSave()
    {
        //���ϴ� ��ġ ����ŭ �ݺ�
        //���� ��ġ�� ���ϴ� ��ġ�� �Ÿ��� ���� ���� ���� ������ isInPlace = false, �ݺ��� continue
        //���� �� ���ٸ� isInPlace = true, ��ġ �̸��� place�� ����
        GPSObjectInfo gpsObjectinfo = new GPSObjectInfo()
        {
            island_id = 1, //�ذ���������
            building_latitude = latitudeinfo, //������
            building_longitude = longitudeinfo, //�ذ浵
            building_name = gpsNameinfo, //�ػ���� �Է�
            building_index = gpsNuminfo, //�ػ���� ����
            //building_pos = gpsObject.transform.position, //�ػ���� ����
            //building_rot = gpsObject.transform.rotation, //�ػ���� ����
        };

        string json = JsonUtility.ToJson(gpsObjectinfo, true);

        // ��� ������
        Debug.Log(json);
        
        //���� ���� (�����)
        string filePath = Path.Combine(Application.persistentDataPath, "data.txt");
        //File.WriteAllText(filePath, json);

        //AI �ε� UI
        HttpManager_LHS.instance.isAichat = false;

        //AI�� ä���� �Ѵ�!
        OnGetPost(json);
    }

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
        JObject data = JObject.Parse(result.text);
        print("GPS ���� ���� ����" + data);
    }

    void OnGetPostFailed()
    {
        print("GPS ������Ʈ �������� ����");
    }

    public void OnPlaceLode()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "data.txt");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            GPSObjectInfo gpsObjectInfo = JsonUtility.FromJson<GPSObjectInfo>(json);
            print("��ġ �б�" + json);
        }

        else
        {
            print("�о� �� ������ �����ϴ�.");
        }
    }

    #region �� �Ÿ� ���� üũ
    public void getUpdateedGPSstring()
    {
        //���� ��ġ�� GPS�������� �ش� ���� �Ҽ��� ���� �ڸ����� �ݿø�
        //�浵�� ������ �Ϲ������� �Ҽ��� �����ڸ����� ��Ȯ�ϰ� ����ϴ� ���� �Ϲ���
        MyLatitude = Math.Round(latitude, 6);
        MyLongtitude = Math.Round(longitude, 6);

        double DistanceToMeter;
        string storeRange;

        //�� ������ �Ÿ�
        //DistUnit.meter �Ÿ��� ������ ���ͷ� �����ϴ� ������ ���
        //�� ���� ���� �Ÿ��� ǥ���� �� ���Ǹ� meter or kilometer ���� ����
        //���������� ����ϴ� ���� -> ����ڰ� �Ÿ��� � ������ ǥ���ϱ� ���ϴ��� ������ �� �ֵ���
        DistanceToMeter = distance(MyLatitude, MyLongtitude, TargetLatitude, TargetLongitude, DistUnit.meter);
        
        // �ǹ��� ������ �� ȯ������ ��ҷ� ���� ������ �߻� �� �� ����.
        if (DistanceToMeter < 50)
        {
            storeRange = "��ó���� O";

        }
        else
        {
            storeRange = "��ó���� X";
        }

        print(storeRange + " / ��ǥ���� �Ÿ�" + DistanceToMeter);
    }

    // �� �������� �Ÿ� ���
    // ����1 ����, �浵 , ����2 ����, �浵, �Ÿ� ǥ�����
    // �� �������� ǥ����� �ִ� �Ÿ��� ã�� ���� ����
    static double distance(double lat1, double lon1, double lat2, double lon2, DistUnit unit)
    {
        //�浵�� ���� ���
        double theta = lon1 - lon2;
        //�Ϲ����� ���� (�� ���浵 ��ǥ ������ �Ÿ��� ���� �� ���)
        double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));

        //����� �������� ��ȯ
        dist = Math.Acos(dist);
        //����� ��׸� ������ ��ȯ
        dist = rad2deg(dist);
        //�Ÿ��� �ظ� ���Ϸ� ��ȯ
        dist = dist * 60 * 1.1515;

        //�Ÿ��� ���ϴ� ������ ��ȯ(ų�ι��� �Ǵ� ����)
        if (unit == DistUnit.kilometer)
        {
            dist = dist * 1.609344; //1�ظ� ���� = 1.609344 ų�ι���
        }

        else if (unit == DistUnit.meter)
        {
            dist = dist * 1609.344; //1�ظ� ���� = 1609.344 ����
        }

        return (dist);
    }

    // This function converts decimal degrees to radians
    // ���� �������� ��ȯ�ϴ� �Լ�
    static double deg2rad(double deg)
    {
        return (deg * Math.PI / 180.0);
    }

    // This function converts radians to decimal degrees
    // ������ ���� ��ȯ�ϴ� �Լ�
    static double rad2deg(double rad)
    {
        return (rad * 180 / Math.PI);
    }
    #endregion
}

enum DistUnit
{
    kilometer,
    meter
}

