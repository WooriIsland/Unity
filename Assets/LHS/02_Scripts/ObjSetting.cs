using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjSetting : MonoBehaviour
{
    public GameObject previewObj;
    public GameObject baseObj;

    public BasePopup uiPopup;

    public bool isinPhoto = false;

    public bool isPhotoZoom = false;

    public bool isChristmas = false;

    //버튼 닫기 한번만 하기 위한 조건문
    private bool isClose = false;

    Outline[] outline;
    // 앨범 자체에 콜라이더 적용 후 Player가 맞는지 체크
    // 플레이어와 닿으면
    // 확대 앨범 UI 나옴 -> 화면상의 UI여야 되구낭..!
    
  
    private void Start()
    {
        outline = transform.GetComponentsInChildren<Outline>();
    }

    private void Update()
    {
        if(transform.GetComponentInChildren<PhotoInfo>())
        {
            isPhotoZoom = transform.GetComponentInChildren<PhotoInfo>().isPhotoCheck;
        }
    }

    //등록을 위한 셋팅 -> Player의 RayCastObject에서 사진등록
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //나 일떄만 실행되게 하기 위해
            if (other.gameObject.GetComponentInChildren<PhotonView>().IsMine)
            {
                // 조건문 실행!
                isinPhoto = true;

                // 아웃라인
                for (int i = 0; i < outline.Length; i++)
                {
                    outline[i].OutlineWidth = 6;
                }

                print("앨범 버튼을 클릭하세요 UI켜지자");
                uiPopup.OpenAction();

                // 최초 1회
                // 사진 등록 하세요 UI 
                if (isPhotoZoom == false)
                {
                    uiPopup.GetComponentInChildren<TextMeshProUGUI>().text = "게시판을 클릭해 사진을 등록하세요";
                }

                else
                {
                    // 이후 사진 확대 기능 활성화
                    //PhotoManager.instance.OnPhotoPopup();
                    uiPopup.GetComponentInChildren<TextMeshProUGUI>().text = "게시판을 클릭해 사진을 확대하세요";
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //나 일떄만 실행되게 하기 위해
            if (other.gameObject.GetComponentInChildren<PhotonView>().IsMine)
            {
                if (isPhotoZoom == true && isClose == false)
                {
                    uiPopup.GetComponent<BasePopup>().CloseAction();
                    isClose = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //나 일떄만 실행되게 하기 위해
            if (other.gameObject.GetComponentInChildren<PhotonView>().IsMine)
            {
                isinPhoto = false;

                // 최초 1회
                // 사진 등록 UI 비활성화
                uiPopup.GetComponent<BasePopup>().CloseAction();

                print("앨범 버튼을 클릭하세요 UI켜지자");

                for (int i = 0; i < outline.Length; i++)
                {
                    outline[i].OutlineWidth = 0;
                }

                // 이후 사진 확대 기능 비활성화
                //PhotoManager.instance.OnPhotoDwon();
            }
        }
    }
}
