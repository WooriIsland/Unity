using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramePhoto : MonoBehaviour
{
    public GameObject ui;
    public GameObject photo;

    public void OnPhotoInquiry()
    {
        ui.SetActive(false);
        photo.SetActive(true);

        //오브젝트 앨범열기용 UI 표시
        PhotoManager.instance.photoFrameUi.SetActive(true);
        //사진 조회 -> 프리팹 앨범열기용으로 가져오기위해
        PhotoManager.instance.OnPhotoInquiry(false);

        //나 전달 -> get set으로 변경
        PhotoManager.instance.FrameObject(this.gameObject);
    }
}
