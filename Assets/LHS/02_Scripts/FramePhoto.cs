using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramePhoto : MonoBehaviour
{
    public GameObject ui;
    public GameObject photo;

    //1번 액자 누른다 -> 꾸미기 모드 Ray되면 안됨
    public void OnPhotoInquiry()
    {
        ui.SetActive(false);
        photo.SetActive(true);

        PhotoManager.instance.isCustomMode = true;
        //지환부분
        PlayerManager.Instance.isAni = false;

        PhotoManager.instance.FrameTutorial++;
        //오브젝트 앨범열기용 UI 표시
        PhotoManager.instance.photoFrameUi.GetComponent<BasePopup>().OpenAction();
        PhotoManager.instance.photoFrameAlpha.GetComponent<BaseAlpha>().OpenAlpha();

        /*if(PhotoManager.instance.isFrameTutorial == true)
        {
            PhotoManager.instance.photoTutorial.GetComponent<PopupPhoto>().OpenAction();
        }*/

        //사진 조회 -> 프리팹 앨범열기용으로 가져오기위해
        PhotoManager.instance.OnPhotoInquiry(false);

        //나 전달 -> ※get set으로 변경
        PhotoManager.instance.FrameObject(this.gameObject);
    }
}
