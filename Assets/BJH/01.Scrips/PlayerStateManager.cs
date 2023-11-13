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

    string[] playerNames;
    public Dictionary<string, GameObject> dicPlayerState = new Dictionary<string, GameObject>();

    private void Awake()
    {
        instance = this;
        PlayerUiSettingAtFirst();
    }

    private void Start()
    {
    }

    void PlayerUiSettingAtFirst()
    {
        // 저장된 가족 수
        //int familySize = 4;

        // 입장한 플레이어 정보
        //string[] playerNames = PhotonNetwork.PlayerList.Select(player => player.NickName).ToArray();

        // 임시
        // member img 이름을 담은 string[]
        playerNames = new string[3];
        playerNames[0] = "dongsik";
        playerNames[1] = "dongdong";
        playerNames[2] = "sook";


        for (int i = 0; i < playerNames.Length; i++)
        {


            // 프리팹 생성
            GameObject go = Instantiate(playerStatePrefab, playerStateBox.transform);

            // 프리팹이 알고있는 image 게임오브젝트의 image 컴포넌트를 가져옴
            Image image = go.GetComponent<PlayerState>().playerImg.GetComponent<Image>();

            // resoures에서 사진을 가져옴
            Texture2D picture = Resources.Load<Texture2D>("Member/" + playerNames[i]);
            go.name = playerNames[i];

            // resources에서 가져온 사진을 image에 적용하기
            image.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));

            // dicPlayerState 셋팅
            dicPlayerState[playerNames[i]] = go.GetComponent<PlayerState>().offline;


            //if (playerNames[i] == "dongsik" )
            //{
            //    go.GetComponent<PlayerState>().offline.SetActive(false);
            //}
        }
    }

    void PlayerUiSettingAtUpdate(Player newPlayer)
    {
        string name = newPlayer.NickName;

        playerStateBox.transform.Find(name);

        print(playerStateBox.transform.Find(name)); // none
        //print(playerStateBox.transform.Find(name).name); // null

        playerStateBox.transform.Find("dongdong").gameObject.GetComponent<PlayerState>().offline.SetActive(false);

        print(playerStateBox.transform.Find("dongdong"));


        print("aaaaaaaaa");
    }

    // 방에 새로운 플레이어가 입장하면 호출되는 함수
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // 새로운 플레이어가 방에 입장했을 때 실행되는 코드
        Debug.Log(newPlayer.NickName + "이(가) 방에 입장했습니다!");

        //PlayerUiSettingAtUpdate(newPlayer);
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
           print("접속한 플레이어 정보 : " + PhotonNetwork.PlayerList[0].NickName);

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
