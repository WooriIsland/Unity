using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using static Define;
using System.Text;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        // ���� �˻�
        object lobby = Managers.Lobby;
        if(lobby == null )
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ����� �� ���� �ҷ��ͼ� create or join
    public void CreateOrJoinRoom()
    {
        RoomOptions option = new RoomOptions();
        if(Managers.Info.visit == null || Managers.Info.visit == "")
        {
            //option.IsOpen = InfoManager.Instance.Secret;
            PhotonNetwork.JoinOrCreateRoom(Managers.Info.IslandName, option, TypedLobby.Default);
        }
        else
        {
            //option.IsOpen = InfoManager.Instance.Secret;
            PhotonNetwork.JoinOrCreateRoom(Managers.Info.visit, option, TypedLobby.Default);
        }
        
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log($"������ ���� : {Managers.Info.IslandName}");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log($"������ ���� ���� : {message}");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        IslandType type = Managers.Info.visitType;

        switch(type)
        {
            case IslandType.BASIC:
                PhotonNetwork.LoadLevel(5);
                break;
            case IslandType.CHRISTMAS:
                PhotonNetwork.LoadLevel(4);
                break;
            default:
                Debug.Log($"{type}�� ������ �����մϴ�.");
                break;
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
