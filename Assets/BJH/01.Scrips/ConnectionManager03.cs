using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Photon.Pun, Photon.Realtime 선언
using Photon.Pun;
using Photon.Realtime;
using TMPro;


public class ConnectionManager03 : MonoBehaviourPunCallbacks
{
    // 인스턴스
    public static ConnectionManager03 instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
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

        // 플레이어 리스트 출력
        foreach (var player in playerList)
        {
            Debug.Log("Nickname: " + player.NickName);
        }
    }



    public void OnClickConnect()
    {
        //서버 접속 요청
        PhotonNetwork.ConnectUsingSettings();
        print(nameof(OnClickConnect));
    }


    //Master 연결됐는지 확인하는 메서드
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print(nameof(OnConnectedToMaster));

        //master scene에 접속했으면
        //로비에 진입
        JoinLobby();
    }

    //Lobby에 접속하는 메서드
    //Lobby는 가족 코드
    void JoinLobby()
    {
        string nickName = PlayerPrefs.GetString("NickName");
        PhotonNetwork.NickName = nickName;

        //특정 lobby 정보 셋팅
        TypedLobby typedLobby = new TypedLobby("WooriIsland", LobbyType.Default);

        //request to join lobby
        PhotonNetwork.JoinLobby(typedLobby);

    }

    //로비 진입 완료 메서드
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print(nameof(OnJoinedLobby));

        //방 생성 or 방 진입
        RoomOptions roomOptions = new RoomOptions();
        string familyCode = PlayerPrefs.GetString("FamilyCode");
        PhotonNetwork.JoinOrCreateRoom(familyCode, roomOptions, TypedLobby.Default);
    }

    //방 생성 완료 메서드
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print(nameof(OnCreatedRoom));
    }

    //방 생성 실패 메서드
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print(nameof(OnCreateRoomFailed));
    }

    //방 진입 완료 메서드
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print(nameof(OnJoinedRoom));

        //Game Scene으로 이동
        PhotonNetwork.LoadLevel(3); // build setting 기준 3번 씬으로 이동
    }

}
