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

    // ������ ���� ����Ʈ
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
        // player ����
        GameObject player = PhotonNetwork.Instantiate("Player", spawnPoint.position, Quaternion.identity); // �÷��̾� �����
        PlayerManager pm = player.GetComponent<PlayerManager>(); // �� �÷��̾��� PlayerManager ������

        //characterName = PlayerPrefs.GetString("CharacterName"); // ���� ������ �����ߴ� ĳ���� �̸� ������
        pm.SelectModel(InfoManager.Instance.Character); // �� ĳ���͸� Ȱ��ȭ ������

        // ���� �����Ű��
        SoundManager_LHS.instance.PlayBGM(SoundManager_LHS.EBgm.BGM_XMAS);

        // ���� ����� ���ƿ� ���� �����ͼ� ������ ����
        if(likeBtnInfo.roomName == "���� & ����")
        {
            print("�������� ���ƿ� �� ��");
            likeCnt.text = InfoManager.Instance.MyIslandLike;
            unLike.SetActive(InfoManager.Instance.isMyIslandLike);
            
        }
        else
        {
            likeCnt.text = InfoManager.Instance.ChristmasIslandLike;
            unLike.SetActive(InfoManager.Instance.isMyIslandLike);

        }

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

    public void OnClick_LeaveRoom()
    {
        // �÷��̾� ���¸� offline���� ����
        //PlayerStateManager.instance.

        LobbyManager.Instance.LeaveRoom();
    }


    // �� ���ƿ� ���
    // �ӽ� : �������� ��� ������ �Ϸ�Ǹ�, �����ؾߵ�
    public void ClickLike(GameObject go)
    {
        LikeBtnInfo likeBtnInfo = go.GetComponent<LikeBtnInfo>();
        string roomName = likeBtnInfo.roomName;
        bool isUnlike = likeBtnInfo.unLike.activeSelf;

        if (roomName == "���� & ����")
        {
            // ���ƿ䰡 �������� ���� ���¸�?
            // ���ƿ並 ������
            if (isUnlike == true)
            {
                likeBtnInfo.unLike.SetActive(false); // ���ƿ�
                int cnt = int.Parse(likeBtnInfo.likeCnt.text);
                cnt++;
                InfoManager.Instance.MyIslandLike = cnt.ToString();
                InfoManager.Instance.isMyIslandLike = false;
                likeBtnInfo.likeCnt.text = InfoManager.Instance.MyIslandLike;
            }
            else
            {
                // ���ƿ� ���
                likeBtnInfo.unLike.SetActive(true); // ���ƿ� ���
                int cnt = int.Parse(likeBtnInfo.likeCnt.text);
                cnt--;
                InfoManager.Instance.MyIslandLike = cnt.ToString();
                InfoManager.Instance.isMyIslandLike = true;

                likeBtnInfo.likeCnt.text = InfoManager.Instance.MyIslandLike;
            }
        }

        if (roomName == "ũ�������� ��")
        {
            // ���ƿ䰡 �������� ���� ���¸�?
            // ���ƿ並 ������
            if (isUnlike == true)
            {
                likeBtnInfo.unLike.SetActive(false); // ���ƿ�
                int cnt = int.Parse(likeBtnInfo.likeCnt.text);
                cnt++;
                InfoManager.Instance.ChristmasIslandLike = cnt.ToString();
                InfoManager.Instance.isChristmasIslandLike = false;

                likeBtnInfo.likeCnt.text = InfoManager.Instance.ChristmasIslandLike;
            }
            else
            {
                // ���ƿ� ���
                likeBtnInfo.unLike.SetActive(true); // ���ƿ� ���
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
        // ��ư�� Ŭ���ϸ�
        // PlayerTag�� ���� ���� ������Ʈ�� ã�ƿ´�.
        GameObject[] go = GameObject.FindGameObjectsWithTag("Player");

        // Ismine�� �͵��� �����ͼ�
        foreach (GameObject go2 in go)
        {
            if (go2.GetComponent<PhotonView>().IsMine == true)
            {
                // �� �÷��̾��� PlayerManager�� ã�Ƽ� StartHello �޼��带 �����Ŵ
                go2.GetComponent<PlayerManager>().StartHello();
            }
        }
    }


    // Punch
    public void Onclick_Punch()
    {
        // ��ư�� Ŭ���ϸ�
        // PlayerTag�� ���� ���� ������Ʈ�� ã�ƿ´�.
        GameObject[] go = GameObject.FindGameObjectsWithTag("Player");


        // Ismine�� �͵��� �����ͼ�
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
        // ��ư�� Ŭ���ϸ�
        // PlayerTag�� ���� ���� ������Ʈ�� ã�ƿ´�.
        GameObject[] go = GameObject.FindGameObjectsWithTag("Player");


        // Ismine�� �͵��� �����ͼ�
        foreach (GameObject go2 in go)
        {
            if (go2.GetComponent<PhotonView>().IsMine == true)
            {
                go2.GetComponent<PlayerManager>().StartDance();
            }
        }
    }
}
