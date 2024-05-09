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
    //가족고유키
    public int island_id;
    //GPS 건물 이름
    public string building_name;
    //GPS 설치 오브젝트 번호
    public int building_index;
    //위도
    public float building_latitude;
    //경도
    public float building_longitude;
    //위치
    public Vector3 building_pos;
    //회전
    public Quaternion building_rot;
}

//받는 값들
[Serializable]
public class GPSObjectList<T>
{
    public List<T> gpsObjectList;
}

public class GPSManager : MonoBehaviour
{
    public static GPSManager instance;

    [Header("최대응답대기시간")]
    public float maxWaitTime = 10.0f;
    [Header("위치정보갱신시간")]
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

    [Header("꾸미기모드")]
    public PlacementSystem placementSystem;

    [Header("섬꾸미기애니메이션")]
    public MainUISlide planeUI;

    //목표지점
    double TargetLatitude, TargetLongitude;

    //현재경과된대기시간
    float waitTime = 0;
    //GPS 켜져있는지 확인
    bool receiveGPS = false;

    // 현재 위도 경도 
    float latitude;
    float longitude;
    //저장 할 위도 경도
    float latitudeinfo;
    float longitudeinfo;
    //GPS 건물 이름
    string gpsNameinfo;
    //GPS 설치 오브젝트 번호
    int gpsNuminfo;

    // 현재 GPS 정보 반올림한 변수
    double MyLatitude, MyLongtitude;

    //저장된 위치
    string TargetName;
    //현재 내 위치
    public string CurrentName;

    //서버 통신 전 확인용 파일 위치
    string filePath;

    //unityCoor를 담을 변수 -> 사용은 안해도됨 (기술만알고있기)
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
        // 이름입력칸이 변경될때 호출되는 함수등록 (입력값 저장)
        btnGps.onClick.AddListener(() => OnGpsSave());
        btnGpsName.onClick.AddListener(() => OnGpsName());
        btnGpsObject.onClick.AddListener(() => OnGpsObject());

        //파일 읽어와서 목표지점 셋팅
        OnPlaceLode();

        //섬 들어오자마자 GPS 실행 할 수 있게
        StartCoroutine(GPS_On());
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
            Debug.Log("GPS off");

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
            Debug.Log("위치 정보 수신 실패");
        }

        //응답 대기 시간을 넘어가도록 수신이 없었다면 시간 초과됐음을 출력
        if (waitTime >= maxWaitTime)
        {
            Debug.Log("응답 대기 시간 초과");
        }

        //수신된 GPS데이터를 화면에 출력
        LocationInfo li = Input.location.lastData;
        latitude = li.latitude;
        longitude = li.longitude;

        Debug.Log("GPS활성화 : " + "[latitude 위도] " + latitude + "[longitude 경도]" + longitude);

        //위치 정보 수신 시작 체크
        receiveGPS = true;

        //위치 데이터 수신 시작 이후 resendTime 경과마다 위치 정보를 갱신하고 출력
        while (receiveGPS)
        {
            li = Input.location.lastData;
            latitude = li.latitude;
            longitude = li.longitude;

            //GPSEncoder의 GPSToUCS 사용
            unityCoor = GPSEncoder.GPSToUCS(latitude, longitude);

            Debug.Log("GPS활성화 반복 : " + "[latitude 위도] " + latitude + "[longitude 경도]" + longitude);

            //거리 비교 들어가기
            getUpdateedGPSstring();

            yield return new WaitForSeconds(resendTime);
        }
    }

    //0.새장소만들기
    public void OnGPS()
    {
        if(receiveGPS)
        {
            planeUI.CloseAction();
            gpsOnUI.GetComponent<BasePopup>().OpenAction();
            Debug.Log("gps 활성화");
        }

        else
        {
            planeUI.CloseAction();
            gpsOffUI.GetComponent<BasePopup>().OpenAction();
            Debug.Log("gps 비활성화");
        }
    }


    //1.위도 경도 저장
    public void OnGpsSave()
    {
        latitudeinfo = this.latitude;
        longitudeinfo = this.longitude;
        Debug.Log("GPS 등록 : [위도]" + latitudeinfo + "[경도]" + longitudeinfo);
    }

    //2. 이름 저장
    public void OnGpsName()
    {
        gpsNameinfo = inputGPSName.text;
        gpsName_text.text = gpsNameinfo;
        Debug.Log(gpsNameinfo + "이름저장");
        inputGPSName.text = null;
    }

    //3.오브젝트 인덱스 저장
    public void OnGpsObjectIndex(int num)
    {
        gpsNuminfo = num;
    }

    //4. 꾸미기모드
    public void OnGpsObject()
    {
        Debug.Log("오브젝트 저장");
        gpsName_text.text = null;
        //꾸미기 기능 활성화
        placementSystem.StartPlacement(gpsNuminfo);
        PlaceJsonSave();

        //성공 후 다시 파일 읽기 -> 서버통신으로 변경
        OnPlaceLode();
    }

    private void PlaceJsonSave()
    {
        //원하는 위치 수만큼 반복
        //현재 위치와 원하는 위치의 거리가 범위 내에 들어가지 않으면 isInPlace = false, 반복문 continue
        //범위 내 들어간다면 isInPlace = true, 위치 이름을 place에 저장
        GPSObjectInfo gpsObjectinfo = new GPSObjectInfo()
        {
            island_id = 1, //※가족고유섬
            building_latitude = latitudeinfo, //※위도
            building_longitude = longitudeinfo, //※경도
            building_name = gpsNameinfo, //※사용자 입력
            building_index = gpsNuminfo, //※사용자 선택
            //building_pos = gpsObject.transform.position, //※사용자 선택
            //building_rot = gpsObject.transform.rotation, //※사용자 선택
        };

        string json = JsonUtility.ToJson(gpsObjectinfo, true);

        // 통신 보내기
        Debug.Log(json);
        
        //파일 쓰기 (모바일)
        string filePath = Path.Combine(Application.persistentDataPath, "GPSdata.txt");
        File.WriteAllText(filePath, json);

        //AI 로딩 UI
        HttpManager_LHS.instance.isAichat = false;

        OnGetPost(json);
    }

    public void OnGetPost(string s)
    {
        Debug.Log("오브젝트 서버 통신해보자");
        string url = "http://192.168.0.53:8082/api/building-location-info";

        //생성 -> 데이터 조회 -> 값을 넣어줌 
        HttpRequester requester = new HttpRequester();

        requester.SetUrl(Define.RequestType.POST, Define.DataType.JSON, url, false);

        requester.body = s;
        requester.onComplete = OnGetPostComplete;
        requester.onFailed = OnGetPostFailed;

        HttpManager_LHS.instance.SendRequest(requester);
    }

    //직접 파싱하기
    void OnGetPostComplete(DownloadHandler result)
    {
        JObject data = JObject.Parse(result.text);
        Debug.Log("GPS 정보 저장 성공" + data);
    }

    void OnGetPostFailed(DownloadHandler result)
    {
        Debug.Log("GPS 오브젝트 정보저장 실패");
    }

    public void OnPlaceLode()
    {
        filePath = Path.Combine(Application.persistentDataPath, "GPSdata.txt");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            GPSObjectInfo gpsObjectInfo = JsonUtility.FromJson<GPSObjectInfo>(json);
            Debug.Log("위치 읽기" + json);

            TargetLatitude = gpsObjectInfo.building_latitude;
            TargetLongitude = gpsObjectInfo.building_longitude;
            TargetName = gpsObjectInfo.building_name;
            Debug.Log("목표지점 재설정" + TargetLatitude + "/" + TargetLongitude);
        }

        else
        {
            Debug.Log("읽어 올 파일이 없습니다.");
        }
    }

    public void OnPlaceDelete()
    {
        if(File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("파일 삭제 삭제");

            //초기화
            TargetLatitude = 0;
            TargetLongitude = 0;
            TargetName = null;
        }

        else
        {
            Debug.Log("삭제 할 파일 없음");
        }
    }

    #region 두 거리 사이 체크
    public void getUpdateedGPSstring()
    {
        //현재 장치의 GPS정보에서 해당 값을 소수점 여섯 자리까지 반올림
        //경도와 위도를 일반적으로 소수점 여섯자리까지 정확하게 사용하는 것이 일반적
        MyLatitude = Math.Round(latitude, 6);
        MyLongtitude = Math.Round(longitude, 6);

        double DistanceToMeter;
        string storeRange;

        //두 점간의 거리
        //DistUnit.meter 거리의 단위를 미터로 지정하는 열거형 상수
        //두 지점 간의 거리를 표시할 때 사용되며 meter or kilometer 선택 가능
        //열거형으로 사용하는 이유 -> 사용자가 거리를 어떤 단위로 표시하길 원하는지 선택할 수 있도록
        DistanceToMeter = distance(MyLatitude, MyLongtitude, TargetLatitude, TargetLongitude, DistUnit.meter);
        
        // 건물의 높낮이 등 환경적인 요소로 인해 오차가 발생 할 수 있음.
        if (DistanceToMeter < 50)
        {
            storeRange = "근처매장 O";

            if(TargetName != null)
            {
                storeRange += TargetName;
                CurrentName = TargetName;
            }
        }
        else
        {
            storeRange = "근처매장 X";
            CurrentName = "위치 없음";
        }

        print(storeRange + " / 목표와의 거리" + DistanceToMeter);
    }

    // 두 지점간의 거리 계산
    // 지점1 위도, 경도 , 지점2 위도, 경도, 거리 표출단위
    // 두 지점간의 표면상의 최단 거리를 찾기 위한 공식
    static double distance(double lat1, double lon1, double lat2, double lon2, DistUnit unit)
    {
        //경도의 차이 계산
        double theta = lon1 - lon2;
        //하버사인 공식 (두 위경도 좌표 사이의 거리를 구할 때 사용)
        double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));

        //결과를 라디안으로 변환
        dist = Math.Acos(dist);
        //결과를 디그리 단위로 변환
        dist = rad2deg(dist);
        //거리를 해리 마일로 변환
        dist = dist * 60 * 1.1515;

        //거리를 원하는 단위로 변환(킬로미터 또는 미터)
        if (unit == DistUnit.kilometer)
        {
            dist = dist * 1.609344; //1해리 마일 = 1.609344 킬로미터
        }

        else if (unit == DistUnit.meter)
        {
            dist = dist * 1609.344; //1해리 마일 = 1609.344 미터
        }

        return (dist);
    }

    // This function converts decimal degrees to radians
    // 도를 라디안으로 변환하는 함수
    static double deg2rad(double deg)
    {
        return (deg * Math.PI / 180.0);
    }

    // This function converts radians to decimal degrees
    // 라디안을 도로 변환하는 함수
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

