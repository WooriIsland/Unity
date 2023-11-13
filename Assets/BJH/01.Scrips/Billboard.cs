using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviourPun
{
    public Transform trCam;

    void Update()
    {
        transform.forward = trCam.forward;
    }
}
