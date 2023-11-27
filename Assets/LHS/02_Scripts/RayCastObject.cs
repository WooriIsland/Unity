using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayCastObject : MonoBehaviourPun
{
    public Camera cam;
    public float length = 3;
    public LayerMask mask;

    private void Start()
    {
        //cam = Camera.main;
    }

    private void Update()
    {
        /*if (photonView.IsMine)
        {
            
        }*/

        //포톤 is 내 카메라만!
        /*cam = GameObject.FindWithTag("Player").GetComponentInChildren<Camera>();
        print(cam.gameObject.name);*/

        Vector3 mousePos = Input.mousePosition;

        mousePos.z = length;
        mousePos = cam.ScreenToWorldPoint(mousePos);
        Debug.DrawRay(transform.position, mousePos - transform.position, Color.yellow);

        if (Input.GetMouseButtonDown(0))
        {
            //앨범에서는 안되게
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, length, mask))
            {
                Debug.Log(hit.transform.name);
                //hit.transform.GetComponent<Renderer>().material.color = Color.red;

                Button btn = hit.transform.GetComponentInChildren<Button>();


                if(PhotoManager.instance.isCustomMode == false)
                {
                    //모드일때만
                    btn.onClick.Invoke();
                }
            }
        }
    }
}
