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
    //가족고유키
    public string familyKey;
    //GPS 건물 이름
    public string gpsName;
    //GPS 설치 오브젝트 번호
    public int gpsNum;
    //위도
    public float latitude;
    //경도
    public float longitude;
    //위치
    public Vector3 pos;
    //회전
    public Quaternion rot;
}

//받는 값들
[Serializable]
public class GPSObjectList<T>
{
    public List<T> gpsObjectList;
}

public class GPSManager : MonoBehaviour
{
    //텍스트 UI
    /*public TextMeshProUGUI latitude_text;
    public TextMeshProUGUI longitude_text;*/
    //추후 삭제 해야함
    public TextMeshProUGUI json_text;

    //위도 경도 
    public float latitude;
    public float longitude;

    //최대응답대기시간
    public float maxWaitTime = 10.0f;
    //위치정보갱신시간
    public float resendTime = 1.0f;

    //현재경과된대기시간
    float waitTime = 0;
    bool receiveGPS = false;

    //UI
    public GameObject gpsOffUI;
    public GameObject gpsOnUI;

    //저장해야할 값
    public TMP_InputField inputGPSName;

    public Button btnGps;
    public Button btnGpsName;
    public Button btnGpsObject;

    public PlacementSystem placementSystem;
    public TextMeshProUGUI gpsName_text;

    //GPS 건물 이름
    string gpsNameinfo;
    //GPS 설치 오브젝트 번호
    int gpsNuminfo;
    //위도
    float latitudeinfo;
    //경도
    float longitudeinfo;

    //Ui bool
    bool isGps = false;

    //----- GPS 현위치 체크 ------//
    //원하는 위치 감지 반경


    public void Start()
    {
        // 이름입력 칸이 변경될때 호출되는 함수 등록
        //inputGPSName.onValueChanged.AddListener(OnGPSNameValueChanged);

        // 이름입력칸이 변경될때 호출되는 함수등록 (입력값 저장)
        btnGps.onClick.AddListener(() => OnGpsSave());
        btnGpsName.onClick.AddListener(() => OnGpsName());
        btnGpsObject.onClick.AddListener(() => OnGpsObject());
    }

    public void Update()
    {
   
    }

    //0.새장소만들기
    public void OnGPS()
    {
        isGps = true;

        //GPS 지연시간이 있기 때문에 코루틴 사용
        StartCoroutine(GPS_On());
        print("GPS 등록");
    }

    public IEnumerator GPS_On()
    {
        //GPS 사용허가 및 장치 꺼짐 확인 (using UnityEngine.Android; 필요)
        //만일, GPS 사용 허가를 받지 못했다면, 권한 허가 팝업을 띄운다.
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);

            while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                yield return null;
            }
        }

        //만일, GPS 장치가 켜져 있지 않으면 위치 정보를 수신할 수 없다고 표시한다.
        //위치 정보 관련 속성이나 함수 -> Input.location
        if (!Input.location.isEnabledByUser)
        {
            //latitude_text.text = "GPS off";
            //longitude_text.text = "GPS off";
            print("GPS off");
            gpsOffUI.SetActive(true);
            PlaceJsonSave();
            //gps 허용 팝업뜨게하기
            yield break;
        }

        //위치 데이터를 요청 -> 수신대기
        Input.location.Start();

        //GPS 수신상태가 초기 상태에서 일정 시간 동안 대기
        while (Input.location.status == LocationServiceStatus.Initializing && waitTime < maxWaitTime)
        {
            yield return new WaitForSeconds(1.0f);
            waitTime++;
        }

        //수신 실패 시 수신이 실패됐다는 것을 출력
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            /*latitude_text.text = "위치 정보 수신 실패";
            longitude_text.text = "위치 정보 수실 실패";*/
        }

        //응답 대기 시간을 넘어가도록 수신이 없었다면 시간 초과됐음을 출력
        if (waitTime >= maxWaitTime)
        {
            /*latitude_text.text = "응답 대기 시간 초과";
            longitude_text.text = "응답 대기 시간 초과";*/
        }

        //수신된 GPS데이터를 화면에 출력
        LocationInfo li = Input.location.lastData;
        latitude = li.latitude;
        longitude = li.longitude;

        /*latitude_text.text = "위도 :" + latitude.ToString();
        longitude_text.text = "경도 :" + longitude.ToString();*/

        if(isGps == true)
        {
            //gps 등록 UI 뜨기
            gpsOnUI.SetActive(true);
        }

        else
        {
            gpsOnUI.SetActive(false);
        }


        //위치 정보 수신 시작 체크
        receiveGPS = true;

        //위치 데이터 수신 시작 이후 resendTime 경과마다 위치 정보를 갱신하고 출력
        while (receiveGPS)
        {
            yield return new WaitForSeconds(resendTime);

            li = Input.location.lastData;
            latitude = li.latitude;
            longitude = li.longitude;


            if (isGps == true)
            {
                //gps 등록 UI 뜨기
                gpsOnUI.SetActive(true);
            }

            else
            {
                gpsOnUI.SetActive(false);
            }

            
            /*latitude_text.text = "위도 :" + latitude.ToString();
            longitude_text.text = "경도 :" + longitude.ToString();*/
        }
    }

    //1.위도 경도 저장
    public void OnGpsSave()
    {
        latitudeinfo = this.latitude;
        longitudeinfo = this.longitude;
    }

    //2.이름 저장
    /*private void OnGPSNameValueChanged(string s)
    {
        //입력값이 0보다 클때
        btnGps.interactable = s.Length > 0;
    }*/

    public void OnGpsName()
    {
        gpsNameinfo = inputGPSName.text;
        gpsName_text.text = gpsNameinfo;
    }

    //3.오브젝트 인덱스 저장
    public void OnGpsObjectIndex(int num)
    {
        gpsNuminfo = num;
    }

    public void OnGpsObject()
    {
        isGps = false;
        //꾸미기 기능 활성화
        placementSystem.StartPlacement(gpsNuminfo);
        PlaceJsonSave();
    }

    //사용자가 원하는 위도, 경도 주변에 있는지 확인
    private void PlaceJsonSave()
    {
        //원하는 위치 수만큼 반복
        //현재 위치와 원하는 위치의 거리가 범위 내에 들어가지 않으면 isInPlace = false, 반복문 continue
        //범위 내 들어간다면 isInPlace = true, 위치 이름을 place에 저장
        GPSObjectInfo gpsObjectinfo = new GPSObjectInfo()
        {
            familyKey = "현숙가족123",//처음부터
            latitude = latitudeinfo, //받아온 값
            longitude = longitudeinfo, //받아온 값
            gpsName = gpsNameinfo, //추후 닉네임 + 사용자 입력
            gpsNum = gpsNuminfo, //사용자 선택
            //pos = gpsObject.transform.position, //사용자 선택
            //rot = gpsObject.transform.rotation, //사용자 선택
        };

        //파일 쓰기 (모바일)
        string filePath = Path.Combine(Application.persistentDataPath, "data.txt");
        string json = JsonUtility.ToJson(gpsObjectinfo, true);
        json_text.text = "파일쓰기" + json;

        File.WriteAllText(filePath, json);
    }

    public void OnPlaceLode()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "data.txt");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            GPSObjectInfo gpsObjectInfo = JsonUtility.FromJson<GPSObjectInfo>(json);
            json_text.text = "파일읽기" + json;
        }

        else
        {
            json_text.text = "읽어 올 파일이 없습니다.";
        }
    }

    public void DetectPlace()
    {

    }
}
