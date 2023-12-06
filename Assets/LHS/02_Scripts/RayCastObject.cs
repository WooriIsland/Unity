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

        //일정 거리안에 있을 시 오브젝트의 레이아웃 변하고 있고 
        //버튼을 눌렀을 시 버튼 실행
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, length, mask))
        {
            Debug.Log(hit.transform.name);

            //hit.transform.GetComponent<Outline>().OutlineWidth = 6;

            if (Input.GetMouseButtonDown(0))
            {
                //닿은 물체의 버튼을 가져옴
                Button btn = hit.transform.GetComponentInChildren<Button>();

                //앨범모드에서는 안되게 해야함
                if (PhotoManager.instance.isCustomMode == false)
                {
                    //모드일때만
                    btn.onClick.Invoke();
                }
            }
        }

        //버튼을 눌렀을때 프레임 버튼이 있다면
        /*if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, length, mask))
            {
                Debug.Log(hit.transform.name);
                //hit.transform.GetComponent<Renderer>().material.color = Color.red;

                //닿은 물체의 버튼을 가져옴
                Button btn = hit.transform.GetComponentInChildren<Button>();

                //앨범모드에서는 안되게 해야함
                if(PhotoManager.instance.isCustomMode == false)
                {
                    //모드일때만
                    btn.onClick.Invoke();
                }
            }
        }*/
    }
}
