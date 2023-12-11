using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KkamangBillboard : MonoBehaviour
{
    private Transform cam;

    private void Update()
    {
        if(cam == null)
        {
            GameObject[] goList = GameObject.FindGameObjectsWithTag("Player");
            foreach(GameObject go in goList)
            {
                if(go.GetComponent<PhotonView>().IsMine == true)
                {
                    cam = go.GetComponent<PlayerManager>().camera.transform;
                    break;
                }
            }
        }

        if(cam != null)
        {
            transform.forward = cam.transform.forward;
        }
    }
}
