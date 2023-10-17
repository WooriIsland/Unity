using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public Transform tr;
    // Start is called before the first frame update
    void Start()
    {
        // create player
        PhotonNetwork.Instantiate("m_11", Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
