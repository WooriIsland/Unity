using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjSetting : MonoBehaviour
{
    public GameObject previewObj;
    public GameObject baseObj;

    // 앨범 자체에 콜라이더 적용 후 Player가 맞는지 체크
    // 플레이어와 닿으면
    // 확대 앨범 UI 나옴 -> 화면상의 UI여야 되구낭..!
    // 그리고 밑으로 내려감
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            print("앨범 버튼을 클릭하세요 UI켜지자");
            PhotoManager.instance.OnPhotoPopup();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("앨범 버튼을 클릭하세요 UI켜지자");
            PhotoManager.instance.OnPhotoPopup();

            PhotoManager.instance.OnPhotoDwon();
        }
    }
}
