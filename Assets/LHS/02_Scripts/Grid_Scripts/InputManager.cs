using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//마우스의 위치 감지
public class InputManager : MonoBehaviour
{
    //카메라 - 스크린 좌표
    [SerializeField]
    private Camera sceneCamera;

    //마지막 위치
    private Vector3 lastPosition;

    //배치 레이어 마스크 (그리드로 사용하려는 평면만 감지)
    [SerializeField]
    private LayerMask placementLayermask;

    //시스템 라이브러리 사용 , 클릭하면 생성 종료 시 하나 더 추가하여 Escape 클릭 배치모드 종료
    public event Action OnClicked, OnExit;

    public PlayerManager playerManager_BJH;

    //--- 지환 코드 나중 변경해야함
    // 모든 플에이어
    public GameObject[] players;

    private bool state = true;

    // 나의 카메라
    public Camera camera;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //클릭이 참이라면 실행
            OnClicked?.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnExit?.Invoke();
        }
    }

    //UI와 상호작용할 때 클릭을 방지
    //using UnityEngine.EventSystems; 필요
    //포인터가 UI개체 위에 있으면 true 또는 false를 반환
    public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject();

    //마우스 클릭한 곳의 벡터
    public Vector3 GetSelectedMapPosition()
    {
        //마우스 위치
        Vector3 mousePos = Input.mousePosition;
        //카메라에서 렌더링되지 않은 개체를 선택할 수 없음
        //nearClipPlane 해당 카메라의 위치부터 렌더링을 시작할 최소 위치까지의 거리 (카메라 컴포넌트에서 near 값 리턴)
        mousePos.z = sceneCamera.nearClipPlane;

        //선택한 위치 감지
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100, placementLayermask))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
    }

    public void CamChangeOn()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        //카메라 버튼을 누르면 꺼지고
        OnOff();

        //켜짐 카메라 켜지고 
        sceneCamera.gameObject.SetActive(true);
    }
    
    //다시 닫기 누르면 켜지게 하기
    public void CamChagneOff()
    {
        OnOff();

        sceneCamera.gameObject.SetActive(false);
    }


    // 플레이어 카메라와 플레이어 상태를 껐다 켜는 함수
    public void OnOff()
    {

        state = !state;

        foreach (GameObject go in players)
        {
            go.SetActive(state);
        }

        //camera.gameObject.SetActive(state);
    }
}
