using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjSetting : MonoBehaviour
{
    public GameObject previewObj;
    public GameObject baseObj;

    // �ٹ� ��ü�� �ݶ��̴� ���� �� Player�� �´��� üũ
    // �÷��̾�� ������
    // Ȯ�� �ٹ� UI ���� -> ȭ����� UI���� �Ǳ���..!
    // �׸��� ������ ������
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            print("�ٹ� ��ư�� Ŭ���ϼ��� UI������");
            PhotoManager.instance.OnPhotoPopup();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("�ٹ� ��ư�� Ŭ���ϼ��� UI������");
            PhotoManager.instance.OnPhotoPopup();

            PhotoManager.instance.OnPhotoDwon();
        }
    }
}
