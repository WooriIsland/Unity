using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjSetting : MonoBehaviour
{
    public GameObject previewObj;
    public GameObject baseObj;

    public GameObject uiPopup;

    Outline[] outline;
    // �ٹ� ��ü�� �ݶ��̴� ���� �� Player�� �´��� üũ
    // �÷��̾�� ������
    // Ȯ�� �ٹ� UI ���� -> ȭ����� UI���� �Ǳ���..!
    // �׸��� ������ ������

    public void Start()
    {
        //outline = transform.GetComponentsInChildren<Outline>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            outline = transform.GetComponentsInChildren<Outline>();
            //1�ܰ� : �˾� UI�� ���´�
            //���� : ����1ȸ ������ ����ϼ��� / ���� ������ Ȯ���ϼ���
            print("�ٹ� ��ư�� Ŭ���ϼ��� UI������");
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
            print("�ٹ� ��ư�� Ŭ���ϼ��� UI������");

            for (int i = 0; i < outline.Length; i++)
            {
                outline[i].OutlineWidth = 0;
            }

            PhotoManager.instance.OnPhotoDwon();
        }
    }
}
