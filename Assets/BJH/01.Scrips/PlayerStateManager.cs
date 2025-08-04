using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using System.Text;
using TMPro;

public class PlayerStateManager : MonoBehaviourPunCallbacks
{
    public static PlayerStateManager instance;

    PhotonView pv;

    public GameObject playerStateBox;
    public GameObject playerStatePrefab;


    List<string> playerNames; // 발표용 : 가족섬에 포함된 가족 정보를 미리 저장

    public Dictionary<string, GameObject> dicPlayerState = new Dictionary<string, GameObject>(); // 닉네임 : state 프리팹

    public bool plzUpdate = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        // 나에게 저장된 섬과 방문하고자하는 섬이 같다면? == 섬 주인
        if (InfoManager.Instance.visit == "정이 & 혜리")
        {

            PlayerUiSettingAtFirst();
        }

        //PlayerUiSettingAtFirst();
    }

    private void Update()
    {
        if (plzUpdate == true) 
        {
            PlayerUiSettingUpdate();
            plzUpdate = false;
        }
        
    }

    // PlayerStateUI 초기 설정
    void PlayerUiSettingAtFirst()
    {
        playerNames = new List<string>();

        int id = 2; // 2번 섬 == 정이 & 혜리
        List<string> members = InfoManager.Instance.dicIslandMembers[id]; // 2번 섬에 존재하는 사람들의 List
        foreach (string member in members)
        {
            playerNames.Add(member);
        }
        playerNames.Add("까망이"); // 정이, 혜리, 심사위원 1, 2, 3, 까망이

        for (int i = 0; i < playerNames.Count; i++)
        {
            GameObject go = Instantiate(playerStatePrefab, playerStateBox.transform);

            // 프리팹이 알고있는 image 게임오브젝트의 image 컴포넌트를 가져옴
            Image image = go.GetComponent<PlayerState>().playerImg.GetComponent<Image>();

            // resoures에서 사진을 가져옴
            Texture2D picture = Resources.Load<Texture2D>("Member/" + InfoManager.Instance.dicMemberCharacter[playerNames[i]]);
            go.name = playerNames[i];

            // resources에서 가져온 사진을 image에 적용하기
            image.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));

            // dicPlayerState 셋팅
            // 플레이어 이름 : 접속 정보의 offline 오브젝트
            dicPlayerState[playerNames[i]] = go.GetComponent<PlayerState>().offline;


            // 닉네임 설정
            go.GetComponent<PlayerState>().name.text = go.name;

            // 위치
            go.GetComponent<PlayerState>().location.text = "위치 X"; // 현숙이 코드 확인


            // 까망이는 항상 Online
            if(playerNames[i] == "까망이")
            {
                go.GetComponent<PlayerState>().offline.SetActive(false);
                go.GetComponent<PlayerState>().location.text = "우리섬"; // 현숙이 코드 확인
            }

            // 버튼
            Button button = go.GetComponent<PlayerState>().button;
            button.onClick.AddListener(() => OnClick_PlayerStateBtn(go));

        }
    }


    void PlayerUiSettingUpdate()
    {
        for (int i = 0; i < playerStateBox.transform.childCount; i++) // playerStateBox를 가져와서
        {
            GameObject go = playerStateBox.transform.GetChild(i).gameObject;
            if(go.name == PhotonNetwork.NickName)
            {
                print($"go.name : {go.name}, photonnetwork.nickname : {PhotonNetwork.NickName}");
                continue;
            }
            Image image = go.GetComponent<PlayerState>().playerImg.GetComponent<Image>();

            // resoures에서 사진을 가져옴
            Texture2D picture = Resources.Load<Texture2D>("Member/" + InfoManager.Instance.dicMemberCharacter[go.name]);
            print(go.name + " 비교 " + InfoManager.Instance.dicMemberCharacter[go.name]);
            // resources에서 가져온 사진을 image에 적용하기
            image.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));

            // dicPlayerState 셋팅
            // 플레이어 이름 : 접속 정보의 offline 오브젝트
            dicPlayerState[playerNames[i]] = go.GetComponent<PlayerState>().offline;
        }
    }



    // 플레이어가 접속하면 해당하는 플레이어 이름을 dic에서 찾고 offline을 꺼서 무지개를 노출시킨다.
    // 플레이어 입장시 PunRPC[]에서 state manager를 불러서 해당 함수 실행

    // 접속중인 플레이어의 캐릭터 정보 저장
    public List<string> OnlinePlayers = new List<string>();

    //나중에 변경해야함
    public void JoinedPlayerStateUpdate(string name)
    {
        if (dicPlayerState[name] != null)
        {
            dicPlayerState[name].SetActive(false);
        }
    }

    public void LeavePlayerStateUpdate(string name)
    {
        // 만약 해당 섬의 주인이라면?
        // PlyaerUISettingAtFirst()를 해서 업데이트를 해줘라!
        //if (InfoManager.Instance.visit == "정이 & 혜리")
        //{
        //    List<string> members = InfoManager.Instance.dicIslandMembers[2];
        //    foreach (string member in members)
        //    {
        //        if (InfoManager.Instance.NickName == member)
        //        {
        //            dicPlayerState[name].SetActive(true);
        //            break;
        //        }
        //    }
        //}

    }

    // 접속한 플레이어 UI를 나타내주는 메서드
    void OnLineUI(string name)
    {
        GameObject go = GameObject.Find(name).gameObject;

        go.GetComponent<PlayerState>().offline.SetActive(false);
    }

    // 방 구성원 UI를 생성해주는 메서드
    public void Member()
    {

            // 프리팹 생성
            GameObject go = Instantiate(playerStatePrefab, playerStateBox.transform);

            // 프리팹이 알고있는 image 게임오브젝트의 image 컴포넌트를 가져옴
            Image image = go.GetComponent<PlayerState>().playerImg.GetComponent<Image>();

            // resoures에서 사진을 가져옴
            Texture2D picture = Resources.Load<Texture2D>("Member/dongsik");

            // resources에서 가져온 사진을 image에 적용하기
            image.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));
    }


    public void ChangeOffLine(string nickName, bool isOffLine)
    {
        if (dicPlayerState.ContainsKey(nickName))
        {
            GameObject go = dicPlayerState[nickName];

            go.SetActive(isOffLine);
        }
    }





    // 플레이어 이름, 위치 정보 Box 껐다 켜기
    public void OnClick_PlayerStateBtn(GameObject go)
    {
        GameObject locationBox = go.GetComponent<PlayerState>().locationBox;
        print(locationBox.name);
        TMP_Text nickName = go.GetComponent<PlayerState>().name;
        TMP_Text location = go.GetComponent <PlayerState>().location;

        //locationBox.SetActive(!locationBox.activeSelf);

    }
}
