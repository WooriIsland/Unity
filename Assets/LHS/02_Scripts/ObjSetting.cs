using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjSetting : MonoBehaviour
{
    public GameObject previewObj;
    public GameObject baseObj;

    public BasePopup uiPopup;

    public bool isinPhoto = false;

    public bool isPhotoZoom = false;

    public bool isChristmas = false;

    //��ư �ݱ� �ѹ��� �ϱ� ���� ���ǹ�
    private bool isClose = false;

    Outline[] outline;
    // �ٹ� ��ü�� �ݶ��̴� ���� �� Player�� �´��� üũ
    // �÷��̾�� ������
    // Ȯ�� �ٹ� UI ���� -> ȭ����� UI���� �Ǳ���..!
    
  
    private void Start()
    {
        outline = transform.GetComponentsInChildren<Outline>();
    }

    private void Update()
    {
        if(transform.GetComponentInChildren<PhotoInfo>())
        {
            isPhotoZoom = transform.GetComponentInChildren<PhotoInfo>().isPhotoCheck;
        }
    }

    //����� ���� ���� -> Player�� RayCastObject���� �������
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //�� �ϋ��� ����ǰ� �ϱ� ����
            if (other.gameObject.GetComponentInChildren<PhotonView>().IsMine)
            {
                // ���ǹ� ����!
                isinPhoto = true;

                // �ƿ�����
                for (int i = 0; i < outline.Length; i++)
                {
                    outline[i].OutlineWidth = 6;
                }

                print("�ٹ� ��ư�� Ŭ���ϼ��� UI������");
                uiPopup.OpenAction();

                // ���� 1ȸ
                // ���� ��� �ϼ��� UI 
                if (isPhotoZoom == false)
                {
                    uiPopup.GetComponentInChildren<TextMeshProUGUI>().text = "�Խ����� Ŭ���� ������ ����ϼ���";
                }

                else
                {
                    // ���� ���� Ȯ�� ��� Ȱ��ȭ
                    //PhotoManager.instance.OnPhotoPopup();
                    uiPopup.GetComponentInChildren<TextMeshProUGUI>().text = "�Խ����� Ŭ���� ������ Ȯ���ϼ���";
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //�� �ϋ��� ����ǰ� �ϱ� ����
            if (other.gameObject.GetComponentInChildren<PhotonView>().IsMine)
            {
                if (isPhotoZoom == true && isClose == false)
                {
                    uiPopup.GetComponent<BasePopup>().CloseAction();
                    isClose = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //�� �ϋ��� ����ǰ� �ϱ� ����
            if (other.gameObject.GetComponentInChildren<PhotonView>().IsMine)
            {
                isinPhoto = false;

                // ���� 1ȸ
                // ���� ��� UI ��Ȱ��ȭ
                uiPopup.GetComponent<BasePopup>().CloseAction();

                print("�ٹ� ��ư�� Ŭ���ϼ��� UI������");

                for (int i = 0; i < outline.Length; i++)
                {
                    outline[i].OutlineWidth = 0;
                }

                // ���� ���� Ȯ�� ��� ��Ȱ��ȭ
                //PhotoManager.instance.OnPhotoDwon();
            }
        }
    }
}
