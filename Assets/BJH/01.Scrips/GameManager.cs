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
    public string chName;

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
        //PhotonNetwork.Instantiate("Player", spawnPoint.position, Quaternion.identity);
    }

    private void Start()
    {
        //chatCanvas.SetActive(false);

    }

    public void GameStart(string name)
    {
        print("GameStart함수 시작");
        StartCoroutine(CoFindSeletedCharactor(name));

        selectCharactorCanvas.SetActive(false);
        initCamera.SetActive(false);
        gameState = true;

        if(gameState)
        {
            chatCanvas.SetActive(true);
        }

    }

    // 선택한 캐릭터만 활성화되게 해주는 코루틴 함수
    // 플레이어가 생성된 후 실행되면 됨
    IEnumerator CoFindSeletedCharactor(string name)
    {
        SpawnSelectCharactor();
        yield return SpawnSelectCharactor();

        GameObject[] foundObjects = GameObject.FindGameObjectsWithTag("Charactor");
        for (int i = 0; i < foundObjects.Length; i++)
        {
            print("foundObject" + i + " : " + foundObjects[i]);
        }

        foreach (GameObject foundObject in foundObjects)
        {
            print("foreach 실행");
            if (foundObject.name == name)
            {
                print("찾을 캐릭터 이름 : " + name + " 찾은 캐릭터 이름 : " + foundObject.name);
                foundObject.SetActive(true);
            }
        }
        // isCharactorSpawn = false;
    }

    private bool isCharactorSpawn;
    private bool SpawnSelectCharactor()
    {
        // PhotonNetwork.Instantiate(charactors[charactorId].name, spawnPoint.position, Quaternion.identity).transform.SetParent(myPlayer.transform);
        PhotonNetwork.Instantiate("Player", spawnPoint.position, Quaternion.identity);
        print("캐릭터 생성 완료");
        return isCharactorSpawn = true;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
