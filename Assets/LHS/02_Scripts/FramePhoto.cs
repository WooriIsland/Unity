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

        //������Ʈ �ٹ������ UI ǥ��
        PhotoManager.instance.photoFrameUi.SetActive(true);
        //���� ��ȸ -> ������ �ٹ���������� ������������
        PhotoManager.instance.OnPhotoInquiry(false);

        //�� ���� -> get set���� ����
        PhotoManager.instance.FrameObject(this.gameObject);
    }
}
