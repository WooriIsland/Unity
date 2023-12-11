using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//플레이어 카메라에서 Ray를 쏴서 
//앨범레이어의 오브젝트가 있고 0번을 누른다면
//앨범설치용 UI 발생
//가까이 간다면 앨범 켜지고
//멀어지면 꺼지게 하기

//! 오브젝트 자체에서 판별하게 변경
public class RayCastObject : MonoBehaviourPunCallbacks
{
    public Camera cam;
    public float length = 3;

    //FrameMain / Memo 으로
    public LayerMask mask;
    private LayerMask maskMain;

    // 예외 처리할 UI 요소의 레이어 이름
    private string exceptionLayerName = "IgnoreRaycast";

    public bool isCheck;

    private void Start()
    {
        // FrameMain, Memo만 확인할 수 있도록
        maskMain = (1 << LayerMask.NameToLayer("FrameMain")) + (1 << LayerMask.NameToLayer("Memo"));
    }

    bool uiCheck = true;

    private void Update()
    {
        //나 일때만 실행되게 하기
        if (photonView.IsMine)
        {
            print("실행할수있는 기능");

            //포톤 나만 적용될 수 있도록 해야함 //앨범 모드 아닐때만
            //※다른사람이 이 오브젝트를 수정하고 있으면 할 수 없게 (동시접속일때 문제 생길 수 있음)


            Vector3 mousePos = Input.mousePosition;
            mousePos.z = length;

            mousePos = cam.ScreenToWorldPoint(mousePos);
            Debug.DrawRay(transform.position, mousePos - transform.position, Color.yellow);

            //버튼을 눌렀을때 게시판이라면
            //각자의 게시판에서만 실행될 수 있게
            if (Input.GetMouseButtonDown(0))
            {
                /*if (IsPointerOverUIObject())
                {
                    // UI 요소 위에 있는 경우 Raycast를 수행하지 않음
                    return;
                }*/

                //섬꾸미는 모드일때도 실행 x
                //앨범 모드일때도 
                if (PhotoManager.instance.isCustomMode || PhotoManager.instance.isPhotoMode)
                {
                    print(PhotoManager.instance.isPhotoMode);
                    return;
                }

                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, length, maskMain))
                {
                    Debug.Log(hit.transform.name);
                    //hit.transform.GetComponent<Renderer>().material.color = Color.red;

                    ObjSetting obj = hit.transform.GetComponent<ObjSetting>();
                    //앨범에 true인지에 따라 다르게

                    //앨범모드에서는 안되게 해야함 /각 앨범의 true 일때 실행되게
                    if (PhotoManager.instance.isCustomMode == false && obj != null && obj.isinPhoto == true)
                    {
                        print("앨범게시판활성화");
                        //처음만 앨범 게시판 조회
                        if (obj.isPhotoZoom == false && obj.isChristmas == false)
                        {
                            print("앨범설치 1단계_1 : 최초1회 바로 사진전시하기");
                            obj.GetComponentInChildren<FramePhoto>().OnPhotoInquiry();
                        }

                        //이후 사진 zoom 기능
                        else
                        {
                            print("앨범설치 1단계_2 : 게시판오브젝트 전달" + obj.gameObject);

                            //PhotoManager.instance.OnPhotoPopup(obj.gameObject);

                            //게임오브젝트 전달은 직렬화하여 전달해야함
                            /*object[] data = new object[] { obj.gameObject.GetComponentInChildren<PhotonView>().ViewID};
                            sendObj = obj;*/

                            //나의 조건문 내꺼만 실행될 수 있게
                            isCheck = true;

                            PhotoManager.instance.OnPhotoPopupSet(obj.Photo, obj.isChristmas, isCheck);

                            //내꺼만 실행될 수 있게!
                            int objectID = obj.GetComponentInChildren<PhotonView>().ViewID;

                            //내 닉네임 보내기 -> 내 닉네임이랑 팝업창의 주인이랑 비교해서 같을때만 바뀔 수 있게 변경
                            photonView.RPC("PhotoPopup", RpcTarget.All, objectID, InfoManager.Instance.NickName);
                        }
                    }

                    //메모
                    MemoSetting memoSet = hit.transform.GetComponent<MemoSetting>();

                    if (MemoManager.instance.isMemo == false && memoSet != null && memoSet.isinMemo == true)
                    {
                        print("메모기능활성화");
                        MemoManager.instance.OnMemoPanel();
                    }
                }
            }
        }

        else
        {
            print("실행할수없는 기능");
        }
    }

    //할필요없을 거 같은데 ?
    private ObjSetting sendObj;

    [PunRPC]
    void PhotoPopup(int objectID, string nickName)
    {
        /*int viewID = (int)data[0];
        GameObject obj = PhotonView.Find(viewID).gameObject;*/
        //PhotoManager.instance.OnPhotoPopup(sendObj.gameObject);

        GameObject obj = PhotonView.Find(objectID).gameObject;
        print("RPC" + obj.name);

        PhotoManager.instance.OnPhotoPopup(obj, nickName);
    }
    
    // 나중 확인할 예정
    private bool IsPointerOverUIObject()
    {
        // 마우스 위치에 UI 요소가 있는지 여부를 검사
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        // UI 요소 중에서 예외 처리할 레이어에 속한 요소가 있는지 검사
        foreach (var result in results)
        {
            if (result.gameObject.layer == LayerMask.NameToLayer(exceptionLayerName))
            {
                return true; // 예외 처리할 레이어에 속한 UI 요소가 있으면 true 반환
            }
        }

        return results.Count > 0;
    }
}
