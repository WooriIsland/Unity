using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

[Serializable]
public class GPSObjectInfo
{
    //��������Ű
    public string familyKey;
    //GPS �ǹ� �̸�
    public string gpsName;
    //GPS ��ġ ������Ʈ ��ȣ
    public int gpsNum;
    //����
    public float latitude;
    //�浵
    public float longitude;
    //��ġ
    public Vector3 pos;
    //ȸ��
    public Quaternion rot;
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

    //----- GPS ����ġ üũ ------//
    //���ϴ� ��ġ ���� �ݰ�


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
            yield return new WaitForSeconds(resendTime);

            li = Input.location.lastData;
            latitude = li.latitude;
            longitude = li.longitude;


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
            familyKey = "��������123",//ó������
            latitude = latitudeinfo, //�޾ƿ� ��
            longitude = longitudeinfo, //�޾ƿ� ��
            gpsName = gpsNameinfo, //���� �г��� + ����� �Է�
            gpsNum = gpsNuminfo, //����� ����
            //pos = gpsObject.transform.position, //����� ����
            //rot = gpsObject.transform.rotation, //����� ����
        };

        //���� ���� (�����)
        string filePath = Path.Combine(Application.persistentDataPath, "data.txt");
        string json = JsonUtility.ToJson(gpsObjectinfo, true);
        json_text.text = "���Ͼ���" + json;

        File.WriteAllText(filePath, json);
    }

    public void OnPlaceLode()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "data.txt");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            GPSObjectInfo gpsObjectInfo = JsonUtility.FromJson<GPSObjectInfo>(json);
            json_text.text = "�����б�" + json;
        }

        else
        {
            json_text.text = "�о� �� ������ �����ϴ�.";
        }
    }

    public void DetectPlace()
    {

    }
}
