using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramePhoto : MonoBehaviour
{
    public GameObject ui;
    public GameObject photo;

    //1�� ���� ������ -> �ٹ̱� ��� Ray�Ǹ� �ȵ�
    public void OnPhotoInquiry()
    {
        ui.SetActive(false);
        photo.SetActive(true);

        PhotoManager.instance.isCustomMode = true;
        //��ȯ�κ�
        PlayerManager.Instance.isAni = false;

        PhotoManager.instance.FrameTutorial++;
        //������Ʈ �ٹ������ UI ǥ��
        PhotoManager.instance.photoFrameUi.GetComponent<BasePopup>().OpenAction();
        PhotoManager.instance.photoFrameAlpha.GetComponent<BaseAlpha>().OpenAlpha();

        /*if(PhotoManager.instance.isFrameTutorial == true)
        {
            PhotoManager.instance.photoTutorial.GetComponent<PopupPhoto>().OpenAction();
        }*/

        //���� ��ȸ -> ������ �ٹ���������� ������������
        PhotoManager.instance.OnPhotoInquiry(false);

        //�� ���� -> ��get set���� ����
        PhotoManager.instance.FrameObject(this.gameObject);
    }
}
