using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public Transform spawnPoint;

    public GameObject myPlayer;
    // Start is called before the first frame update
    void Awake()
    {
        PhotonNetwork.SerializationRate = 60;

        // create player
        PhotonNetwork.Instantiate("Player", spawnPoint.position, Quaternion.identity).transform.SetParent(myPlayer.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
