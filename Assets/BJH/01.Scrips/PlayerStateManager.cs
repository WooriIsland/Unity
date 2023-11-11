using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerStateManager : MonoBehaviourPunCallbacks
{
    PhotonView pv;

    public Transform playerStateBox;
    public GameObject playerStatePrefab;

    string[] playerNames;

    private void Start()
    {
        PlayerUiSettingAtFirst();
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
            GameObject go = Instantiate(playerStatePrefab, playerStateBox);

            // 프리팹이 알고있는 image 게임오브젝트의 image 컴포넌트를 가져옴
            Image image = go.GetComponent<PlayerState>().playerImg.GetComponent<Image>();

            // resoures에서 사진을 가져옴
            Texture2D picture = Resources.Load<Texture2D>("Member/" + playerNames[i]);
            go.name = playerNames[i];

            // resources에서 가져온 사진을 image에 적용하기
            image.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));

            if (playerNames[i] == "dongsik" )
            {
                go.GetComponent<PlayerState>().offline.SetActive(false);
            }
        }
    }

    // 방에 새로운 플레이어가 입장하면 호출되는 함수
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // 새로운 플레이어가 방에 입장했을 때 실행되는 코드
        Debug.Log(newPlayer.NickName + "이(가) 방에 입장했습니다!");

        // 질문
        // newPlayer.NIckName울 debug는 찍히는데, 이걸 대입하면 null이 발생
        // 데이터타입도 string임.. 왜 안되는거지?

        pv.RPC(nameof(OnLineUI), RpcTarget.All, newPlayer.NickName);
    }

    // 접속한 플레이어 UI를 나타내주는 메서드
    [PunRPC]
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
            GameObject go = Instantiate(playerStatePrefab, playerStateBox);

            // 프리팹이 알고있는 image 게임오브젝트의 image 컴포넌트를 가져옴
            Image image = go.GetComponent<PlayerState>().playerImg.GetComponent<Image>();

            // resoures에서 사진을 가져옴
            Texture2D picture = Resources.Load<Texture2D>("Member/dongsik");

            // resources에서 가져온 사진을 image에 적용하기
            image.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));
    }

}
