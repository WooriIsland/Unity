using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Photon.Pun, Photon.Realtime 선언
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

// 변지환
// 포톤 서버를 통한 로비, 방 진입을 위한 클래스
public class ConnectionManager : MonoBehaviourPunCallbacks
{
    public string nickName;
    public string familyCode;
    public string characterName;

    // 포톤 방 입장 요청
    public void OnClickConnect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // 마스터 진입 확인
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print(nameof(OnConnectedToMaster));

        PhotonNetwork.NickName = Managers.Info.NickName;

        TypedLobby typedLobby = new TypedLobby("Woori Island", LobbyType.Default);
        PhotonNetwork.JoinLobby(typedLobby);
    }


    // 로비에 진입 후 가족섬 생성 씬으로 이동
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print(nameof(OnJoinedLobby));

        // 씬 이동
        SceneManager.LoadScene(2);

        ////방 생성 or 방 진입
        //RoomOptions roomOptions = new RoomOptions();

        //familyCode = InfoManager.Instance.FamilyCode;
        //print($"InfoManager에게서 가져온 FamilyCode는 {familyCode} 입니다.");
        //PhotonNetwork.JoinOrCreateRoom(familyCode, roomOptions, TypedLobby.Default);
    }


    //public void LoadGameScene()
    //{
    //    SceneManager.LoadScene(4);
    //}



    //public void ConnectRequest()
    //{
    //    //서버 접속 요청
    //    PhotonNetwork.ConnectUsingSettings();
    //    print(nameof(ConnectRequest));
    //}




    ////Lobby에 접속하는 메서드
    ////Lobby는 가족 코드
    //void JoinLobby()
    //{
    //    PhotonNetwork.NickName = InfoManager.Instance.NickName;

    //    //특정 lobby 정보 셋팅
    //    TypedLobby typedLobby = new TypedLobby("WooriIsland", LobbyType.Default);

    //    //request to join lobby
    //    PhotonNetwork.JoinLobby(typedLobby);

    //}




    //public void CreateRoom()
    //{
    //    // 방 옵션 설정
    //    RoomOptions roomOption = new RoomOptions();

    //    // 공개, 비공개 여부
    //    roomOption.IsVisible = true;

    //    // 방 생성
    //    PhotonNetwork.CreateRoom(InfoManager.Instance.IslandName, roomOption);
    //}

    //public override void OnCreatedRoom()
    //{
    //    base.OnCreatedRoom();
    //    print($"가족섬 생성 : {InfoManager.Instance.IslandName}");
    //}

    //public override void OnCreateRoomFailed(short returnCode, string message)
    //{
    //    base.OnCreateRoomFailed(returnCode, message);
    //    print($"가족섬 생성 실패 : {message}");
    //}

    //public void JoinRoom(string code)
    //{
    //    PhotonNetwork.JoinRoom(code);
    //}

    ////방 진입 완료 메서드
    //public override void OnJoinedRoom()
    //{
    //    base.OnJoinedRoom();
    //    print(nameof(OnJoinedRoom));
    //}

}
