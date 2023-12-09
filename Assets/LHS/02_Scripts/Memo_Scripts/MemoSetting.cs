using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoSetting : MonoBehaviour
{
    public GameObject uiPopup;
    public bool isinMemo = false;

    Outline[] outline;

    private void Start()
    {
        outline = transform.GetComponentsInChildren<Outline>();
    }

    private void Update()
    {
    }

    //����� ���� ���� -> Player�� RayCastObject���� �������
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //�� �ϋ��� ����ǰ� �ϱ� ����
            if (other.gameObject.GetComponentInChildren<PhotonView>().IsMine)
            {
                isinMemo = true;

                // �ƿ�����
                for (int i = 0; i < outline.Length; i++)
                {
                    outline[i].OutlineWidth = 6;
                }

                uiPopup.GetComponent<BasePopup>().OpenAction();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //�� �ϋ��� ����ǰ� �ϱ� ����
            if (other.gameObject.GetComponentInChildren<PhotonView>().IsMine)
            {
                isinMemo = false;

                // ���� 1ȸ
                // ���� ��� UI ��Ȱ��ȭ
                uiPopup.GetComponent<BasePopup>().CloseAction();

                print("�ٹ� ��ư�� Ŭ���ϼ��� UI������");

                for (int i = 0; i < outline.Length; i++)
                {
                    outline[i].OutlineWidth = 0;
                }

                // ���� ���� Ȯ�� ��� ��Ȱ��ȭ
                PhotoManager.instance.OnPhotoDwon();
            }
        }
    }
}
