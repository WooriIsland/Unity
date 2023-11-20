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
        if (instance == null)
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

    private void Update()
    {
        
    }

    private void OnDestroy()
    {
        // PUN 콜백 해제
        PhotonNetwork.RemoveCallbackTarget(this);
    }



    //public override void OnPlayerEnteredRoom(PlayerTest newPlayer)
    //{
    //    // 새로운 플레이어가 방에 입장했을 때 실행되는 코드
    //    Debug.Log(newPlayer.NickName + "이(가) 방에 입장했습니다!");
    //}

    

    public string[] GetJoinedPlayerLIst()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            joinedPlayers[i] = PhotonNetwork.PlayerList[i].NickName;
        }
        
        return joinedPlayers;
    }
}
