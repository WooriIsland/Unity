using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramePhoto : MonoBehaviourPun
{
    public GameObject ui;

    private void Update()
    {
        // ���� ������� �ÿ���
        if (ui != null)
        {
            if (transform.GetComponentInChildren<PhotoInfo>().isPhotoCheck == true)
            {
                ui.SetActive(false);
            }
        }
    }

    //1�� ���� ������ -> �ٹ̱� ��� Ray�Ǹ� �ȵ�
    public void OnPhotoInquiry()
    {
        print("�ٹ���ġ 2�ܰ� : �ٹ�UI �ѱ� ���� �۾� / ���� ������Ʈ PhotoManager�� ������");
        //Ŀ���� ���
        PhotoManager.instance.isCustomMode = true;

        //��ȯ�κ� -> �ȵǴ� �� ����
        PlayerManager.Instance.isAni = false;

        //Ʃ�丮�� �ѹ��� ����ǰ� �ϱ� ����
        PhotoManager.instance.FrameTutorial++;

        //������Ʈ �ٹ������ UI ǥ��
        PhotoManager.instance.photoFrameUi.GetComponent<BasePopup>().OpenAction();
        PhotoManager.instance.photoFrameAlpha.GetComponent<BaseAlpha>().OpenAlpha();

        //���� ��ȸ -> ������ �ٹ���������� ������������
        PhotoManager.instance.OnPhotoInquiry(false);

        print(this.gameObject);

        //PUN
        //�� ���� -> ��get set���� ����
        photonView.RPC("FrameObject", RpcTarget.All);
    }

    [PunRPC]
    void FrameObject()
    {
        PhotoManager.instance.FrameObject(this.gameObject);
    }
}
