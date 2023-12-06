using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjSetting : MonoBehaviour
{
    public GameObject previewObj;
    public GameObject baseObj;

    public GameObject uiPopup;

    Outline[] outline;
    // 앨범 자체에 콜라이더 적용 후 Player가 맞는지 체크
    // 플레이어와 닿으면
    // 확대 앨범 UI 나옴 -> 화면상의 UI여야 되구낭..!
    // 그리고 밑으로 내려감

    public void Start()
    {
        //outline = transform.GetComponentsInChildren<Outline>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            outline = transform.GetComponentsInChildren<Outline>();
            //1단계 : 팝업 UI가 나온다
            //조건 : 최초1회 사진을 등록하세요 / 이후 사진을 확대하세요
            print("앨범 버튼을 클릭하세요 UI켜지자");
            for(int i = 0; i <outline.Length; i++)
            {
                outline[i].OutlineWidth = 10;
            }

            //PhotoManager.instance.OnPhotoPopup();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("앨범 버튼을 클릭하세요 UI켜지자");

            for (int i = 0; i < outline.Length; i++)
            {
                outline[i].OutlineWidth = 0;
            }

            PhotoManager.instance.OnPhotoDwon();
        }
    }
}
