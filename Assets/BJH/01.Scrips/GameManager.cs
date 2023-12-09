using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Reflection;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO.IsolatedStorage;
using UnityEngine.UIElements;

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

    public LikeBtnInfo likeBtnInfo;
    public TMP_Text likeCnt;
    public GameObject unLike;

    // 접속한 유저 리스트
    public List<int> userList = new List<int>();


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        PhotonNetwork.SerializationRate = 60;

        // create player
        //PhotonNetwork.Instantiate("Player", spawnPoint.position, Quaternion.identity);
    }

    private void Start()
    {
        // player 생성
        GameObject player = PhotonNetwork.Instantiate("Player", spawnPoint.position, Quaternion.identity); // 플레이어 만들어
        PlayerManager pm = player.GetComponent<PlayerManager>(); // 그 플레이어의 PlayerManager 가져와

        //characterName = PlayerPrefs.GetString("CharacterName"); // 이전 씬에서 선택했던 캐릭터 이름 가져와
        pm.SelectModel(InfoManager.Instance.Character); // 그 캐릭터만 활성화 시켜줘

        // 음악 실행시키기
        SoundManager_LHS.instance.PlayBGM(SoundManager_LHS.EBgm.BGM_XMAS);

        // 최초 입장시 좋아요 정보 가져와서 저장후 노출
        if(likeBtnInfo.roomName == "정이 & 혜리")
        {
            print("정이혜리 좋아요 수 들어감");
            likeCnt.text = InfoManager.Instance.MyIslandLike;
            unLike.SetActive(InfoManager.Instance.isMyIslandLike);
            
        }
        else
        {
            likeCnt.text = InfoManager.Instance.ChristmasIslandLike;
            unLike.SetActive(InfoManager.Instance.isMyIslandLike);

        }

    }


    // 캐릭터가 선택되면
    // 선택된 캐릭터 정보를 저장하고
    // 다음 씬으로 이동
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

    public void OnClick_LeaveRoom()
    {
        // 플레이어 상태를 offline으로 변경
        //PlayerStateManager.instance.

        LobbyManager.Instance.LeaveRoom();
    }


    // 섬 좋아요 기능
    // 임시 : 서버에서 기능 구현이 완료되면, 연결해야됨
    public void ClickLike(GameObject go)
    {
        LikeBtnInfo likeBtnInfo = go.GetComponent<LikeBtnInfo>();
        string roomName = likeBtnInfo.roomName;
        bool isUnlike = likeBtnInfo.unLike.activeSelf;

        if (roomName == "정이 & 혜리")
        {
            // 좋아요가 눌려지지 않은 상태면?
            // 좋아요를 누르자
            if (isUnlike == true)
            {
                likeBtnInfo.unLike.SetActive(false); // 좋아요
                int cnt = int.Parse(likeBtnInfo.likeCnt.text);
                cnt++;
                InfoManager.Instance.MyIslandLike = cnt.ToString();
                InfoManager.Instance.isMyIslandLike = false;
                likeBtnInfo.likeCnt.text = InfoManager.Instance.MyIslandLike;
            }
            else
            {
                // 좋아요 취소
                likeBtnInfo.unLike.SetActive(true); // 좋아요 취소
                int cnt = int.Parse(likeBtnInfo.likeCnt.text);
                cnt--;
                InfoManager.Instance.MyIslandLike = cnt.ToString();
                InfoManager.Instance.isMyIslandLike = true;

                likeBtnInfo.likeCnt.text = InfoManager.Instance.MyIslandLike;
            }
        }

        if (roomName == "크리스마스 섬")
        {
            // 좋아요가 눌려지지 않은 상태면?
            // 좋아요를 누르자
            if (isUnlike == true)
            {
                likeBtnInfo.unLike.SetActive(false); // 좋아요
                int cnt = int.Parse(likeBtnInfo.likeCnt.text);
                cnt++;
                InfoManager.Instance.ChristmasIslandLike = cnt.ToString();
                InfoManager.Instance.isChristmasIslandLike = false;

                likeBtnInfo.likeCnt.text = InfoManager.Instance.ChristmasIslandLike;
            }
            else
            {
                // 좋아요 취소
                likeBtnInfo.unLike.SetActive(true); // 좋아요 취소
                int cnt = int.Parse(likeBtnInfo.likeCnt.text);
                cnt--;
                InfoManager.Instance.ChristmasIslandLike = cnt.ToString();
                InfoManager.Instance.isChristmasIslandLike = true;

                likeBtnInfo.likeCnt.text = InfoManager.Instance.ChristmasIslandLike;
            }
        }


    }

    // Hello
    public void Onclick_Hello()
    {
        // 버튼을 클릭하면
        // PlayerTag를 가진 게임 오브젝트를 찾아온다.
        GameObject[] go = GameObject.FindGameObjectsWithTag("Player");

        // Ismine인 것들을 가져와서
        foreach (GameObject go2 in go)
        {
            if (go2.GetComponent<PhotonView>().IsMine == true)
            {
                // 내 플레이어의 PlayerManager를 찾아서 StartHello 메서드를 실행시킴
                go2.GetComponent<PlayerManager>().StartHello();
            }
        }
    }


    // Punch
    public void Onclick_Punch()
    {
        // 버튼을 클릭하면
        // PlayerTag를 가진 게임 오브젝트를 찾아온다.
        GameObject[] go = GameObject.FindGameObjectsWithTag("Player");


        // Ismine인 것들을 가져와서
        foreach (GameObject go2 in go)
        {
            if (go2.GetComponent<PhotonView>().IsMine == true)
            {
                go2.GetComponent<PlayerManager>().StartPunch();
            }
        }
    }

    // Dance
    public void OnClick_Dance()
    {
        // 버튼을 클릭하면
        // PlayerTag를 가진 게임 오브젝트를 찾아온다.
        GameObject[] go = GameObject.FindGameObjectsWithTag("Player");


        // Ismine인 것들을 가져와서
        foreach (GameObject go2 in go)
        {
            if (go2.GetComponent<PhotonView>().IsMine == true)
            {
                go2.GetComponent<PlayerManager>().StartDance();
            }
        }
    }
}
