using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjSetting : MonoBehaviour
{
    public GameObject previewObj;
    public GameObject baseObj;

    public GameObject uiPopup;
    public  bool isPhotoBtn = false;

    Outline[] outline;
    // �ٹ� ��ü�� �ݶ��̴� ���� �� Player�� �´��� üũ
    // �÷��̾�� ������
    // Ȯ�� �ٹ� UI ���� -> ȭ����� UI���� �Ǳ���..!

    private void Start()
    {
        outline = transform.GetComponentsInChildren<Outline>();
    }

    //����� ���� ���� -> Player�� RayCastObject���� �������
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            // ���� 1ȸ
            // ���� ��� �ϼ��� UI 
            // �ƿ�����
            uiPopup.GetComponent<BasePopup>().OpenAction();

            print("�ٹ� ��ư�� Ŭ���ϼ��� UI������");
            for(int i = 0; i <outline.Length; i++)
            {
                outline[i].OutlineWidth = 6;
            }

            // ���ǹ� ����!
            isPhotoBtn = true;

            // ���� ���� Ȯ�� ��� Ȱ��ȭ
            //PhotoManager.instance.OnPhotoPopup();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // ���� 1ȸ
            // ���� ��� UI ��Ȱ��ȭ
            uiPopup.GetComponent<BasePopup>().CloseAction();

            print("�ٹ� ��ư�� Ŭ���ϼ��� UI������");

            for (int i = 0; i < outline.Length; i++)
            {
                outline[i].OutlineWidth = 0;
            }

            isPhotoBtn = false;
            // ���� ���� Ȯ�� ��� ��Ȱ��ȭ
            //PhotoManager.instance.OnPhotoDwon();
        }
    }
}
