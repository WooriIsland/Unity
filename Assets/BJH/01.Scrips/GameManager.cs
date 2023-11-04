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


    // 캐릭터가 선택되면
    // 선택된 캐릭터 정보를 저장하고
    // 다음 씬으로 이동
    int characterIdx;
    public void SelectCharacter(int idx)
    {
        print(idx + "번 캐릭터를 선택했습니다.");
        characterIdx = idx;

        SceneManager.LoadScene(3);

        SpawnSelectCharacter(idx);

        

        gameState = true;

        if (gameState)
        {
            chatCanvas.SetActive(true);
        }
    }

    // 선택한 캐릭터만 활성화되게 해주는 코루틴 함수
    // 플레이어가 생성된 후 실행되면 됨
    IEnumerator CoFindSeletedCharactor(string name)
    {
        //SpawnSelectCharactor();
        yield return new WaitForSeconds(2f);

        // 나의 플레이어를 찾고
        // 해당 플레이어의 캐릭터만 on
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

    private void SpawnSelectCharacter(int idx)
    {
        // player 생성
        GameObject player = PhotonNetwork.Instantiate("Player", spawnPoint.position, Quaternion.identity);

        // character 선택
        PlayerManager pm = player.GetComponent<PlayerManager>();
        pm.SelectModel(idx);

        // nickName 변경
        
        
        print("캐릭터 생성 완료");
    }

    // Update is called once per frame
    void Update()
    {
        
    }




}
