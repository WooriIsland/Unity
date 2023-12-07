using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�÷��̾� ī�޶󿡼� Ray�� ���� 
//�ٹ����̾��� ������Ʈ�� �ְ� 0���� �����ٸ�
//�ٹ���ġ�� UI �߻�
//������ ���ٸ� �ٹ� ������
//�־����� ������ �ϱ�

//! ������Ʈ ��ü���� �Ǻ��ϰ� ����
public class RayCastObject : MonoBehaviourPun
{
    public Camera cam;
    public float length = 3;

    //FrameMain����
    public LayerMask mask;

    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = length;

        mousePos = cam.ScreenToWorldPoint(mousePos);
        Debug.DrawRay(transform.position, mousePos - transform.position, Color.yellow);

        //��ư�� �������� �Խ����̶��
        //������ �Խ��ǿ����� ����� �� �ְ�
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, length, mask))
            {
                Debug.Log(hit.transform.name);
                //hit.transform.GetComponent<Renderer>().material.color = Color.red;

                ObjSetting obj = hit.transform.GetComponent<ObjSetting>();

                print(obj.name);
                
                //�ٹ���忡���� �ȵǰ� �ؾ���
                if (PhotoManager.instance.isCustomMode == false)
                {
                    //�� �ٹ��� true �϶� ����ǰ�
                    if(obj.isPhotoBtn == true)
                    {
                        //ó���� ��ȸ����
                        if(obj.isPhotoZoom == false)
                        {
                            obj.GetComponentInChildren<FramePhoto>().OnPhotoInquiry();
                        }

                        else
                        {
                            PhotoManager.instance.OnPhotoPopup(obj.gameObject);
                        }
                    }
                }
            }
        }
    }
}
