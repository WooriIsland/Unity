using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoSetting : MonoBehaviour
{
    public GameObject uiPopup;
    public bool isinMemo = false;

    Outline[] outline;

    private void Start()
    {
        outline = transform.GetComponentsInChildren<Outline>();
    }

    private void Update()
    {
    }

    //등록을 위한 셋팅 -> Player의 RayCastObject에서 사진등록
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //나 일떄만 실행되게 하기 위해
            if (other.gameObject.GetComponentInChildren<PhotonView>().IsMine)
            {
                isinMemo = true;

                // 아웃라인
                for (int i = 0; i < outline.Length; i++)
                {
                    outline[i].OutlineWidth = 6;
                }

                uiPopup.GetComponent<BasePopup>().OpenAction();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //나 일떄만 실행되게 하기 위해
            if (other.gameObject.GetComponentInChildren<PhotonView>().IsMine)
            {
                isinMemo = false;

                // 최초 1회
                // 사진 등록 UI 비활성화
                uiPopup.GetComponent<BasePopup>().CloseAction();

                print("앨범 버튼을 클릭하세요 UI켜지자");

                for (int i = 0; i < outline.Length; i++)
                {
                    outline[i].OutlineWidth = 0;
                }

                // 이후 사진 확대 기능 비활성화
                PhotoManager.instance.OnPhotoDwon();
            }
        }
    }
}
