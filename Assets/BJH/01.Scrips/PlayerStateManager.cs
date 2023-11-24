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

public class PlayerStateManager : MonoBehaviourPunCallbacks
{
    public static PlayerStateManager instance;

    PhotonView pv;

    public GameObject playerStateBox;
    public GameObject playerStatePrefab;


    string[] playerNames; // 발표용 : 가족섬에 포함된 가족 정보를 미리 저장

    public Dictionary<string, GameObject> dicPlayerState = new Dictionary<string, GameObject>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        PlayerUiSettingAtFirst();
    }

    void PlayerUiSettingAtFirst()
    {
        // 발표용 : 서버에서 DB 저장 구현이 완료되면 request를 플레이어, response를 캐릭터 정보로 받아와야함
        // member img 이름을 담은 string[]
        playerNames = new string[3];
        playerNames[0] = "m_10";
        playerNames[1] = "f_3";
        playerNames[2] = "까망이";

        for (int i = 0; i < playerNames.Length; i++)
        {
            GameObject go = Instantiate(playerStatePrefab, playerStateBox.transform);

            // 프리팹이 알고있는 image 게임오브젝트의 image 컴포넌트를 가져옴
            Image image = go.GetComponent<PlayerState>().playerImg.GetComponent<Image>();

            // resoures에서 사진을 가져옴
            Texture2D picture = Resources.Load<Texture2D>("Member/" + playerNames[i]);
            go.name = playerNames[i];

            // resources에서 가져온 사진을 image에 적용하기
            image.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));

            // dicPlayerState 셋팅
            // 플레이어 이름 : 접속 정보의 offline 오브젝트
            dicPlayerState[playerNames[i]] = go.GetComponent<PlayerState>().offline;

            // 까망이는 항상 Online
            if(playerNames[i] == "까망이")
            {
                go.GetComponent<PlayerState>().offline.SetActive(false);
            }


            //if (playerNames[i] == "dongsik" )
            //{
            //    go.GetComponent<PlayerState>().offline.SetActive(false);
            //}
        }
    }

    // 플레이어가 접속하면 해당하는 플레이어 이름을 dic에서 찾고 offline을 꺼서 무지개를 노출시킨다.
    // 플레이어 입장시 PunRPC[]에서 state manager를 불러서 해당 함수 실행

    // 접속중인 플레이어의 캐릭터 정보 저장
    public List<string> OnlinePlayers = new List<string>();

    public void JoinedPlayerStateUpdate(string name)
    {
        dicPlayerState[name].SetActive(false);
    }

    public void LeavePlayerStateUpdate(string name)
    {
        dicPlayerState[name].SetActive(true);
    }

    //void PlayerUiSettingAtUpdate(PlayerTest newPlayer)
    //{
    //    string name = newPlayer.NickName;

    //    playerStateBox.transform.Find(name);

    //    print(playerStateBox.transform.Find(name)); // none
    //    //print(playerStateBox.transform.Find(name).name); // null

    //    playerStateBox.transform.Find("dongdong").gameObject.GetComponent<PlayerState>().offline.SetActive(false);

    //    print(playerStateBox.transform.Find("dongdong"));


    //    print("aaaaaaaaa");
    //}

    // 방에 새로운 플레이어가 입장하면 호출되는 함수
    //public override void OnPlayerEnteredRoom(PlayerTest newPlayer)
    //{
    //    // 새로운 플레이어가 방에 입장했을 때 실행되는 코드
    //    Debug.Log(newPlayer.NickName + "이(가) 방에 입장했습니다!");

    //    //PlayerUiSettingAtUpdate(newPlayer);
    //}

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
}
