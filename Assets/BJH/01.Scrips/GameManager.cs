using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Reflection;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPun
{
    [Header("#Player Info")]
    public string chName;

    public Transform spawnPoint;

    public GameObject myPlayer;

    public static GameManager instance;

    [Header("# Select Charactor Canvas GameObject")]
    public GameObject selectCharactorCanvas;
    public GameObject mainUI;
    public GameObject initCamera;
    public GameObject chatCanvas;

    public bool gameState = false;
    string characterName;

    // �÷��̾� ���� ����




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
        // player ����
        GameObject player = PhotonNetwork.Instantiate("Player", spawnPoint.position, Quaternion.identity); // �÷��̾� �����
        PlayerManager pm = player.GetComponent<PlayerManager>(); // �� �÷��̾��� PlayerManager ������

        //characterName = PlayerPrefs.GetString("CharacterName"); // ���� ������ �����ߴ� ĳ���� �̸� ������
        pm.SelectModel(InfoManager.Instance.Character); // �� ĳ���͸� Ȱ��ȭ ������
    }


    // ĳ���Ͱ� ���õǸ�
    // ���õ� ĳ���� ������ �����ϰ�
    // ���� ������ �̵�
    int characterIdx;
    public void SelectCharacter(int idx)
    {
        characterIdx = idx;

        SceneManager.LoadScene(3);

        //SpawnSelectCharacter(idx);

        

        gameState = true;

        if (gameState)
        {
            chatCanvas.SetActive(true);
        }
    }

    // ������ ĳ���͸� Ȱ��ȭ�ǰ� ���ִ� �ڷ�ƾ �Լ�
    // �÷��̾ ������ �� ����Ǹ� ��
    IEnumerator CoFindSeletedCharactor(string name)
    {
        //SpawnSelectCharactor();
        yield return new WaitForSeconds(2f);

        // ���� �÷��̾ ã��
        // �ش� �÷��̾��� ĳ���͸� on
        if (photonView.IsMine)
        {
            GameObject[] charactors = GameObject.FindGameObjectsWithTag("Charactor");
            foreach (GameObject charactor in charactors)
            {
                if (charactor.name == name)
                {
                    charactor.SetActive(true);
                    break;
                }
            }
        }

        // isCharactorSpawn = false;

        
    }



}
