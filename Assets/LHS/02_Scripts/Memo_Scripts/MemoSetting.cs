using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoSetting : MonoBehaviour
{
    public GameObject uiPopup;

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
    }

    //등록을 위한 셋팅 -> Player의 RayCastObject에서 사진등록
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 아웃라인
            for (int i = 0; i < outline.Length; i++)
            {
                outline[i].OutlineWidth = 6;
            }

            uiPopup.GetComponent<BasePopup>().OpenAction();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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
