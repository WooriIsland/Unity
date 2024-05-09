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

    // ����� �� ���� �ҷ��ͼ� create or join
    public void CreateOrJoinRoom()
    {
        print("���� ��û");
        if(Managers.Info.visit == null || Managers.Info.visit == "")
        {
            print("��������");
            RoomOptions option = new RoomOptions();
            //option.IsOpen = InfoManager.Instance.Secret;
            PhotonNetwork.JoinOrCreateRoom(Managers.Info.IslandName, option, TypedLobby.Default);
        }
        else
        {
            print("��������");
            RoomOptions option = new RoomOptions();
            //option.IsOpen = InfoManager.Instance.Secret;
            PhotonNetwork.JoinOrCreateRoom(Managers.Info.visit, option, TypedLobby.Default);
        }
        
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print($"������ ���� : {Managers.Info.IslandName}");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print($"������ ���� ���� : {message}");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("�� ���� �Ϸ�");

        //InfoManager.Instance.visit = null;

        string type = Managers.Info.visitType;

        if(type == "Island01") // ���� ��
        {
            // ���� ������ �̵�
            PhotonNetwork.LoadLevel(4);
        }

        else // �Ϲ� ��
        {
            // ���� ������ �̵�
            PhotonNetwork.LoadLevel(5);
        }

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print($"�� ���� ���� : {message}");
    }



    // �� ������
    public void LeaveRoom()
    {
        //  ���� ���� ���ֱ�
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
//            islandCustom.GetComponent<CreateIslandManager>().islandName.text = "���� & ����";
//        }
//#endif
//    }

