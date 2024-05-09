using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private static LobbyManager instance;

    public static LobbyManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

    }

    // 저장된 방 정보 불러와서 create or join
    public void CreateOrJoinRoom()
    {
        print("입장 요청");
        if(Managers.Info.visit == null || Managers.Info.visit == "")
        {
            print("정보없음");
            RoomOptions option = new RoomOptions();
            //option.IsOpen = InfoManager.Instance.Secret;
            PhotonNetwork.JoinOrCreateRoom(Managers.Info.IslandName, option, TypedLobby.Default);
        }
        else
        {
            print("정보있음");
            RoomOptions option = new RoomOptions();
            //option.IsOpen = InfoManager.Instance.Secret;
            PhotonNetwork.JoinOrCreateRoom(Managers.Info.visit, option, TypedLobby.Default);
        }
        
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print($"가족섬 생성 : {Managers.Info.IslandName}");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print($"가족섬 생성 실패 : {message}");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("방 입장 완료");

        //InfoManager.Instance.visit = null;

        string type = Managers.Info.visitType;

        if(type == "Island01") // 얼음 섬
        {
            // 게임 씬으로 이동
            PhotonNetwork.LoadLevel(4);
        }

        else // 일반 섬
        {
            // 게임 씬으로 이동
            PhotonNetwork.LoadLevel(5);
        }

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print($"방 입장 실패 : {message}");
    }



    // 방 나가기
    public void LeaveRoom()
    {
        //  접속 상태 없애기
        PlayerManager.Instance.CallRpcLeftPlayer();

        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        PhotonNetwork.LoadLevel(2);
    }
}

//#if UNITY_EDITOR
//        if (Input.GetKeyDown(KeyCode.F2))
//        {
//            islandCustom.GetComponent<CreateIslandManager>().islandName.text = "정이 & 혜리";
//        }
//#endif
//    }

