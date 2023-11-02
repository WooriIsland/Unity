using System;
using System.Collections;
using System.Collections.Generic;
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
    public TextMeshProUGUI latitude_text;
    public TextMeshProUGUI longitude_text;
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

    //���� -> ���� ���� �ؾ���
    public GameObject gpsObject;

    string GPSName;
    public TMP_InputField inputGPSName;
    public Button btnGPSSave;

    public void Start()
    {
        // �̸��Է� ĭ�� ����ɶ� ȣ��Ǵ� �Լ� ���
        inputGPSName.onValueChanged.AddListener(OnGPSNameValueChanged);
        // �̸��Է�ĭ�� ����ɶ� ȣ��Ǵ� �Լ���� (�Է°� ����)
        btnGPSSave.onClick.AddListener(() => OnSaveInfo());
    }

    private void OnGPSNameValueChanged(string s)
    {
        //�Է°��� 0���� Ŭ��
        btnGPSSave.interactable = s.Length > 0;
    }

    private void OnSaveInfo()
    {
        GPSName = inputGPSName.text;
        print(GPSName);

        //�Էµ� ���� �ִٸ�
        if(GPSName != null)
        {
            //��ġ ������Ʈ �ֱ�
            DetectPlace();
        }
    }

    public void OnGPS()
    {
        //GPS �����ð��� �ֱ� ������ �ڷ�ƾ ���
        StartCoroutine(GPS_On());
        print("GPS ���");
    }

    public IEnumerator GPS_On()
    {
        //GPS ����㰡 �� ��ġ ���� Ȯ��
        //using UnityEngine.Android; �ʿ�
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
            latitude_text.text = "GPS off";
            longitude_text.text = "GPS off";
            DetectPlace();
            gpsOffUI.SetActive(true);
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
            latitude_text.text = "��ġ ���� ���� ����";
            longitude_text.text = "��ġ ���� ���� ����";
        }

        //���� ��� �ð��� �Ѿ���� ������ �����ٸ� �ð� �ʰ������� ���
        if (waitTime >= maxWaitTime)
        {
            latitude_text.text = "���� ��� �ð� �ʰ�";
            longitude_text.text = "���� ��� �ð� �ʰ�";
        }

        //���ŵ� GPS�����͸� ȭ�鿡 ���
        LocationInfo li = Input.location.lastData;
        latitude = li.latitude;
        longitude = li.longitude;
        latitude_text.text = "���� :" + latitude.ToString();
        longitude_text.text = "�浵 :" + longitude.ToString();

        gpsOnUI.SetActive(true);
        
        //��ġ ���� ���� ���� üũ
        receiveGPS = true;

        //��ġ ������ ���� ���� ���� resendTime ������� ��ġ ������ �����ϰ� ���
        while (receiveGPS)
        {
            yield return new WaitForSeconds(resendTime);

            li = Input.location.lastData;
            latitude = li.latitude;
            longitude = li.longitude;
            latitude_text.text = "���� :" + latitude.ToString();
            longitude_text.text = "�浵 :" + longitude.ToString();
        }
    }

    //����ڰ� ���ϴ� ����, �浵 �ֺ��� �ִ��� Ȯ��
    private void DetectPlace()
    {
        //���ϴ� ��ġ ����ŭ �ݺ�
        //���� ��ġ�� ���ϴ� ��ġ�� �Ÿ��� ���� ���� ���� ������ isInPlace = false, �ݺ��� continue
        //���� �� ���ٸ� isInPlace = true, ��ġ �̸��� place�� ����
        GPSObjectInfo gpsObjectinfo = new GPSObjectInfo()
        {
            familyKey = "abc123",//ó������
            latitude = this.latitude, //�޾ƿ� ��
            longitude = this.longitude, //�޾ƿ� ��
            gpsName = GPSName, //���� �г��� + ����� �Է�
            gpsNum = 1, //����� ����
            pos = gpsObject.transform.position, //����� ����
            rot = gpsObject.transform.rotation, //����� ����
        };

        //���� ����
        string json = JsonUtility.ToJson(gpsObjectinfo, true);
        json_text.text = json;
    }
}
