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
    public static GPSManager instance;

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

    [Header("���ٹ̱�ִϸ��̼�")]
    public MainUISlide planeUI;

    //��ǥ����
    double TargetLatitude, TargetLongitude;

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

    // ���� GPS ���� �ݿø��� ����
    double MyLatitude, MyLongtitude;

    //����� ��ġ
    string TargetName;
    //���� �� ��ġ
    public string CurrentName;

    //���� ��� �� Ȯ�ο� ���� ��ġ
    string filePath;

    //unityCoor�� ���� ���� -> ����� ���ص��� (������˰��ֱ�)
    Vector3 unityCoor;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void Start()
    {
        // �̸��Է�ĭ�� ����ɶ� ȣ��Ǵ� �Լ���� (�Է°� ����)
        btnGps.onClick.AddListener(() => OnGpsSave());
        btnGpsName.onClick.AddListener(() => OnGpsName());
        btnGpsObject.onClick.AddListener(() => OnGpsObject());

        //���� �о�ͼ� ��ǥ���� ����
        OnPlaceLode();

        //�� �����ڸ��� GPS ���� �� �� �ְ�
        StartCoroutine(GPS_On());
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
            Debug.Log("GPS off");

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
            Debug.Log("��ġ ���� ���� ����");
        }

        //���� ��� �ð��� �Ѿ���� ������ �����ٸ� �ð� �ʰ������� ���
        if (waitTime >= maxWaitTime)
        {
            Debug.Log("���� ��� �ð� �ʰ�");
        }

        //���ŵ� GPS�����͸� ȭ�鿡 ���
        LocationInfo li = Input.location.lastData;
        latitude = li.latitude;
        longitude = li.longitude;

        Debug.Log("GPSȰ��ȭ : " + "[latitude ����] " + latitude + "[longitude �浵]" + longitude);

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

            Debug.Log("GPSȰ��ȭ �ݺ� : " + "[latitude ����] " + latitude + "[longitude �浵]" + longitude);

            //�Ÿ� �� ����
            getUpdateedGPSstring();

            yield return new WaitForSeconds(resendTime);
        }
    }

    //0.����Ҹ����
    public void OnGPS()
    {
        if(receiveGPS)
        {
            planeUI.CloseAction();
            gpsOnUI.GetComponent<BasePopup>().OpenAction();
            Debug.Log("gps Ȱ��ȭ");
        }

        else
        {
            planeUI.CloseAction();
            gpsOffUI.GetComponent<BasePopup>().OpenAction();
            Debug.Log("gps ��Ȱ��ȭ");
        }
    }


    //1.���� �浵 ����
    public void OnGpsSave()
    {
        latitudeinfo = this.latitude;
        longitudeinfo = this.longitude;
        Debug.Log("GPS ��� : [����]" + latitudeinfo + "[�浵]" + longitudeinfo);
    }

    //2. �̸� ����
    public void OnGpsName()
    {
        gpsNameinfo = inputGPSName.text;
        gpsName_text.text = gpsNameinfo;
        Debug.Log(gpsNameinfo + "�̸�����");
        inputGPSName.text = null;
    }

    //3.������Ʈ �ε��� ����
    public void OnGpsObjectIndex(int num)
    {
        gpsNuminfo = num;
    }

    //4. �ٹ̱���
    public void OnGpsObject()
    {
        Debug.Log("������Ʈ ����");
        gpsName_text.text = null;
        //�ٹ̱� ��� Ȱ��ȭ
        placementSystem.StartPlacement(gpsNuminfo);
        PlaceJsonSave();

        //���� �� �ٽ� ���� �б� -> ����������� ����
        OnPlaceLode();
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
        string filePath = Path.Combine(Application.persistentDataPath, "GPSdata.txt");
        File.WriteAllText(filePath, json);

        //AI �ε� UI
        HttpManager_LHS.instance.isAichat = false;

        OnGetPost(json);
    }

    public void OnGetPost(string s)
    {
        Debug.Log("������Ʈ ���� ����غ���");
        string url = "http://192.168.0.53:8082/api/building-location-info";

        //���� -> ������ ��ȸ -> ���� �־��� 
        HttpRequester requester = new HttpRequester();

        requester.SetUrl(Define.RequestType.POST, Define.DataType.JSON, url, false);

        requester.body = s;
        requester.onComplete = OnGetPostComplete;
        requester.onFailed = OnGetPostFailed;

        HttpManager_LHS.instance.SendRequest(requester);
    }

    //���� �Ľ��ϱ�
    void OnGetPostComplete(DownloadHandler result)
    {
        JObject data = JObject.Parse(result.text);
        Debug.Log("GPS ���� ���� ����" + data);
    }

    void OnGetPostFailed(DownloadHandler result)
    {
        Debug.Log("GPS ������Ʈ �������� ����");
    }

    public void OnPlaceLode()
    {
        filePath = Path.Combine(Application.persistentDataPath, "GPSdata.txt");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            GPSObjectInfo gpsObjectInfo = JsonUtility.FromJson<GPSObjectInfo>(json);
            Debug.Log("��ġ �б�" + json);

            TargetLatitude = gpsObjectInfo.building_latitude;
            TargetLongitude = gpsObjectInfo.building_longitude;
            TargetName = gpsObjectInfo.building_name;
            Debug.Log("��ǥ���� �缳��" + TargetLatitude + "/" + TargetLongitude);
        }

        else
        {
            Debug.Log("�о� �� ������ �����ϴ�.");
        }
    }

    public void OnPlaceDelete()
    {
        if(File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("���� ���� ����");

            //�ʱ�ȭ
            TargetLatitude = 0;
            TargetLongitude = 0;
            TargetName = null;
        }

        else
        {
            Debug.Log("���� �� ���� ����");
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

            if(TargetName != null)
            {
                storeRange += TargetName;
                CurrentName = TargetName;
            }
        }
        else
        {
            storeRange = "��ó���� X";
            CurrentName = "��ġ ����";
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

