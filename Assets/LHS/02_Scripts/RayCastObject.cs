using Photon.Pun;
using UnityEngine;

//플레이어 카메라에서 Ray를 쏴서 
//앨범레이어의 오브젝트가 있고 0번을 누른다면
//앨범설치용 UI 발생
//가까이 간다면 앨범 켜지고
//멀어지면 꺼지게 하기

//! 오브젝트 자체에서 판별하게 변경
public class RayCastObject : MonoBehaviourPun
{
    public Camera cam;
    public float length = 3;

    //FrameMain / Memo 으로
    public LayerMask mask;
    private LayerMask maskMain;

    private void Start()
    {
        // FrameMain, Memo만 확인할 수 있도록
        maskMain = (1 << LayerMask.NameToLayer("FrameMain")) + (1 << LayerMask.NameToLayer("Memo"));
    }

    private void Update()
    {
        //포톤 나만 적용될 수 있도록 해야함

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = length;

        mousePos = cam.ScreenToWorldPoint(mousePos);
        Debug.DrawRay(transform.position, mousePos - transform.position, Color.yellow);

        //버튼을 눌렀을때 게시판이라면
        //각자의 게시판에서만 실행될 수 있게
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, length, maskMain))
            {
                Debug.Log(hit.transform.name);
                //hit.transform.GetComponent<Renderer>().material.color = Color.red;

                ObjSetting obj = hit.transform.GetComponent<ObjSetting>();

                //앨범모드에서는 안되게 해야함 /각 앨범의 true 일때 실행되게
                if (PhotoManager.instance.isCustomMode == false && obj != null && obj.isinPhoto == true)
                {
                    print("앨범게시판활성화");
                    //처음만 앨범 게시판 조회
                    if (obj.isPhotoZoom == false)
                    {
                        obj.GetComponentInChildren<FramePhoto>().OnPhotoInquiry();
                    }

                    //이후 사진 zoom 기능
                    else
                    {
                        PhotoManager.instance.OnPhotoPopup(obj.gameObject);
                    }
                }

                //메모
                MemoSetting memoSet = hit.transform.GetComponent<MemoSetting>();

                if(MemoManager.instance.isMemo == false && memoSet != null && memoSet.isinMemo == true)
                {
                    print("메모기능활성화");
                    MemoManager.instance.OnMemoPanel();
                }
            }
        }
    }
}
