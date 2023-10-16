using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Photon.Pun, Photon.Realtime 선언
using Photon.Pun;
using Photon.Realtime;



// MonoBehaviourPunCallbacks을 상속
public class ConnectionManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        // Photon 환경설정을 기반으로 접속 시도하는 메서드
        PhotonNetwork.ConnectUsingSettings();
    }

    // Master에 연결됐는지 확인하는 메서드
    // MonoBehaviourPunCallbacks에 선언된 메서드 overried
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster(); // base == 부모꺼 먼저 실행 후 내꺼 실행(재정의)
        print(nameof(OnConnectedToMaster));

        // master scene에 접속했으면
        // 로비에 진입
        JoinLobby();
    }

    // Lobby에 접속하는 메서드
    void JoinLobby()
    {
        // 닉네임 설정
        // 현재는 임의로 설정
        PhotonNetwork.NickName = "동식Park";

        // 기본 로비에 입장
        PhotonNetwork.JoinLobby();
    }

    // 로비 진입 완료 메서드
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print(nameof(OnJoinedLobby));

        // 방 생성 or 방 진입
        RoomOptions roomOptions = new RoomOptions();
        PhotonNetwork.JoinOrCreateRoom("WooriIsland", roomOptions, TypedLobby.Default);

    }

    // 방 생성 완료 메서드
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print(nameof(OnCreatedRoom));
    }

    // 방 생성 실패 메서드
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print(nameof(OnCreateRoomFailed));
    }

    // 방 진입 완료 메서드
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print(nameof(OnJoinedRoom));

        // Game Scene으로 이동
        PhotonNetwork.LoadLevel(1); // build setting 기준 1번 씬으로 이동
    }
}