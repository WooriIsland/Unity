using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

using System.Collections;
using System.Collections.Generic;

public class ConnectedPlayerInfo : MonoBehaviourPunCallbacks
{
    // 입장한 플레이어 정보를 담는 리스트
    public string[] joinedPlayers;

    private static ConnectedPlayerInfo instance;

    private void Awake()
    {
        if (instance = null)
        {
            instance = this;
        }
    }

    public static ConnectedPlayerInfo Instance
    {
        get
        {
            return instance;
        }
    }


    private void Start()
    {
        // PUN 콜백 등록
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDestroy()
    {
        // PUN 콜백 해제
        PhotonNetwork.RemoveCallbackTarget(this);
    }



    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // 새로운 플레이어가 room에 입장하면 플레이어 리스트를 가져옴
        Photon.Realtime.Player[] playerList = PhotonNetwork.PlayerList;
        joinedPlayers = new string[playerList.Length];

        // 플레이어 리스트 출력 및 배열에 저장
        foreach (var player in playerList)
        {
            Debug.Log("Nickname: " + player.NickName);

            for (int i = 0; i < playerList.Length; i++)
            {
                joinedPlayers[i] = player.NickName;
            }

        }
    }
}
