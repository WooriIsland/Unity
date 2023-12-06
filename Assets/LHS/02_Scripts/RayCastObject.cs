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
public class RayCastObject : MonoBehaviourPun
{
    public Camera cam;
    public float length = 3;
    public LayerMask mask;

    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = length;

        mousePos = cam.ScreenToWorldPoint(mousePos);
        Debug.DrawRay(transform.position, mousePos - transform.position, Color.yellow);

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, length, mask))
            {
                Debug.Log(hit.transform.name);
                //hit.transform.GetComponent<Renderer>().material.color = Color.red;

                Button btn = hit.transform.GetComponentInChildren<Button>();

                //�ٹ���忡���� �ȵǰ� �ؾ���
                if(PhotoManager.instance.isCustomMode == false)
                {
                    //����϶���
                    btn.onClick.Invoke();
                }
            }
        }
    }
}
