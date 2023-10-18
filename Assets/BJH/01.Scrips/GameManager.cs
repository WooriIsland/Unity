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
        PhotonNetwork.Instantiate("Player", tr.position, Quaternion.identity); // юс╫ц

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
