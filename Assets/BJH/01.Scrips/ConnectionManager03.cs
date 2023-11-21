using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Photon.Pun, Photon.Realtime 선언
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class ConnectionManager03 : MonoBehaviourPunCallbacks
{
    public string nickName;
    public string familyCode;
    public string characterName;

    private static ConnectionManager03 instnace;

    public static ConnectionManager03 Instance
    {
        get
        {
            return instnace;
        }
    }

    private void Awake()
    {
        if(instnace == null)
        {
            instnace = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void ConnectRequest()
    {
        //서버 접속 요청
        PhotonNetwork.ConnectUsingSettings();
        print(nameof(ConnectRequest));
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
        PhotonNetwork.NickName = InfoManager.Instance.NickName;

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

        SceneManager.LoadScene(2);

        ////방 생성 or 방 진입
        //RoomOptions roomOptions = new RoomOptions();

        //familyCode = InfoManager.Instance.FamilyCode;
        //print($"InfoManager에게서 가져온 FamilyCode는 {familyCode} 입니다.");
        //PhotonNetwork.JoinOrCreateRoom(familyCode, roomOptions, TypedLobby.Default);
    }


    public void CreateRoom()
    {
        // 방 옵션 설정
        RoomOptions roomOption = new RoomOptions();

        // 공개, 비공개 여부
        roomOption.IsVisible = true;

        // 방 생성
        PhotonNetwork.CreateRoom(InfoManager.Instance.IslandName, roomOption);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print($"가족섬 생성 : {InfoManager.Instance.IslandName}");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print($"가족섬 생성 실패 : {message}");
    }

    public void JoinRoom(string code)
    {
        PhotonNetwork.JoinRoom(code);
    }

    //방 진입 완료 메서드
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print(nameof(OnJoinedRoom));
    }

}
