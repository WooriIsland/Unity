using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Reflection;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("#Player Info")]
    public int charactorId;
    public List<GameObject> charactors = new List<GameObject>();

    public Transform spawnPoint;

    public GameObject myPlayer;

    public static GameManager instance;

    [Header("# Select Charactor Canvas GameObject")]
    public GameObject selectCharactorCanvas;
    public GameObject initCamera;
    public GameObject chatCanvas;

    public bool gameState = false;


    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        PhotonNetwork.SerializationRate = 60;

        // create player
        //PhotonNetwork.Instantiate(charactors[charactorId].name, spawnPoint.position, Quaternion.identity).transform.SetParent(myPlayer.transform);
    }

    private void Start()
    {
        chatCanvas.SetActive(false);

    }

    public void GameStart(int id)
    {
        charactorId = id;
        SpawnSelectCharactor();
        selectCharactorCanvas.SetActive(false);
        initCamera.SetActive(false);
        gameState = true;

        if(gameState)
        {
            chatCanvas.SetActive(true);
        }

}



private void SpawnSelectCharactor()
    {
        PhotonNetwork.Instantiate(charactors[charactorId].name, spawnPoint.position, Quaternion.identity).transform.SetParent(myPlayer.transform);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
