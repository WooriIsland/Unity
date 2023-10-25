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
        print("GameStart�Լ� ����");
        StartCoroutine(CoFindSeletedCharactor(name));

        selectCharactorCanvas.SetActive(false);
        initCamera.SetActive(false);
        gameState = true;

        if(gameState)
        {
            chatCanvas.SetActive(true);
        }

    }

    // ������ ĳ���͸� Ȱ��ȭ�ǰ� ���ִ� �ڷ�ƾ �Լ�
    // �÷��̾ ������ �� ����Ǹ� ��
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
            print("foreach ����");
            if (foundObject.name == name)
            {
                print("ã�� ĳ���� �̸� : " + name + " ã�� ĳ���� �̸� : " + foundObject.name);
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
        print("ĳ���� ���� �Ϸ�");
        return isCharactorSpawn = true;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
