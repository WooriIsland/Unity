using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramePhoto : MonoBehaviourPun
{
    public GameObject ui;

    private void Update()
    {
        // 사진 등록했을 시에만
        if (ui != null)
        {
            if (transform.GetComponentInChildren<PhotoInfo>().isPhotoCheck == true)
            {
                ui.SetActive(false);
            }
        }
    }

    //1번 액자 누른다 -> 꾸미기 모드 Ray되면 안됨
    public void OnPhotoInquiry()
    {
        print("앨범설치 2단계 : 앨범UI 켜기 위한 작업 / 나의 오브젝트 PhotoManager에 보내기");
        //커스텀 모드
        PhotoManager.instance.isCustomMode = true;

        //지환부분 -> 안되는 거 같음
        PlayerManager.Instance.isAni = false;

        //튜토리얼 한번만 실행되게 하기 위해
        PhotoManager.instance.FrameTutorial++;

        //오브젝트 앨범열기용 UI 표시
        PhotoManager.instance.photoFrameUi.GetComponent<BasePopup>().OpenAction();
        PhotoManager.instance.photoFrameAlpha.GetComponent<BaseAlpha>().OpenAlpha();

        //사진 조회 -> 프리팹 앨범열기용으로 가져오기위해
        PhotoManager.instance.OnPhotoInquiry(false);

        print(this.gameObject);

        //PUN
        //나 전달 -> ※get set으로 변경
        photonView.RPC("FrameObject", RpcTarget.All);
    }

    [PunRPC]
    void FrameObject()
    {
        PhotoManager.instance.FrameObject(this.gameObject);
    }
}
