using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//플레이어 카메라에서 Ray를 쏴서 
//앨범레이어의 오브젝트가 있고 0번을 누른다면
//앨범설치용 UI 발생
//가까이 간다면 앨범 켜지고
//멀어지면 꺼지게 하기

//! 오브젝트 자체에서 판별하게 변경
public class RayCastObject : MonoBehaviourPun
{
    public Camera cam;
    public float length = 3;

    //FrameMain으로
    public LayerMask mask;

    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = length;

        mousePos = cam.ScreenToWorldPoint(mousePos);
        Debug.DrawRay(transform.position, mousePos - transform.position, Color.yellow);

        //버튼을 눌렀을때 게시판이라면
        //각자의 게시판에서만 실행될 수 있게
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
                
                //앨범모드에서는 안되게 해야함
                if (PhotoManager.instance.isCustomMode == false)
                {
                    //각 앨범의 true 일때 실행되게
                    if(obj.isPhotoBtn == true)
                    {
                        //처음만 조회가능
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
