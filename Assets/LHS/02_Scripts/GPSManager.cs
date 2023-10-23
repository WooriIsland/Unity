using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
using TMPro;

public class GPSManager : MonoBehaviour
{
    //텍스트 UI
    public TextMeshProUGUI latitude_text;
    public TextMeshProUGUI longitude_text;

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

    void Start()
    {
        //GPS 지연시간이 있기 때문에 코루틴 사용
        StartCoroutine(GPS_On());
    }

    public IEnumerator GPS_On()
    {
        //GPS 사용허가 및 장치 꺼짐 확인
        //using UnityEngine.Android; 필요
        //만일, GPS 사용 허가를 받지 못했다면, 권한 허가 팝업을 띄운다.
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);

            while(!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                yield return null;
            }
        }

        //만일, GPS 장치가 켜져 있지 않으면 위치 정보를 수신할 수 없다고 표시한다.
        //위치 정보 관련 속성이나 함수 -> Input.location
        if(!Input.location.isEnabledByUser)
        {
            latitude_text.text = "GPS Off";
            longitude_text.text = "GPS off";
            yield break;
        }

        //위치 데이터를 요청 -> 수신대기
        Input.location.Start();

        //GPS 수신상태가 초기 상태에서 일정 시간 동안 대기
        while(Input.location.status == LocationServiceStatus.Initializing && waitTime < maxWaitTime)
        {
            yield return new WaitForSeconds(1.0f);
            waitTime++;
        }

        //수신 실패 시 수신이 실패됐다는 것을 출력
        if(Input.location.status == LocationServiceStatus.Failed)
        {
            latitude_text.text = "위치 정보 수신 실패";
            longitude_text.text = "위치 정보 수실 실패";
        }

        //응답 대기 시간을 넘어가도록 수신이 없었다면 시간 초과됐음을 출력
        if(waitTime >= maxWaitTime)
        {
            latitude_text.text = "응답 대기 시간 초과";
            longitude_text.text = "응답 대기 시간 초과";
        }

        //수신된 GPS데이터를 화면에 출력
        LocationInfo li = Input.location.lastData;
        latitude = li.latitude;
        longitude = li.longitude;
        latitude_text.text = "위도 :" + latitude.ToString();
        longitude_text.text = "경도 :" + longitude.ToString();

        //위치 정보 수신 시작 체크
        receiveGPS = true;

        //위치 데이터 수신 시작 이후 resendTime 경과마다 위치 정보를 갱신하고 출력
        while(receiveGPS)
        {
            yield return new WaitForSeconds(resendTime);

            li = Input.location.lastData;
            latitude = li.latitude;
            longitude = li.longitude;
            latitude_text.text = "위도 :" + latitude.ToString();
            longitude_text.text = "경도 :" + longitude.ToString();
        }
    }

    void Update()
    {
        
    }
}
