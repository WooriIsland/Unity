using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Photon.Pun, Photon.Realtime ����
using Photon.Pun;
using Photon.Realtime;
using TMPro;


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
    }

    public void ConnectRequest()
    {
        //���� ���� ��û
        PhotonNetwork.ConnectUsingSettings();
        print(nameof(ConnectRequest));
    }


    //Master ����ƴ��� Ȯ���ϴ� �޼���
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print(nameof(OnConnectedToMaster));

        //master scene�� ����������
        //�κ� ����
        JoinLobby();
    }

    //Lobby�� �����ϴ� �޼���
    //Lobby�� ���� �ڵ�
    void JoinLobby()
    {
        PhotonNetwork.NickName = InfoManager.Instance.NickName;

        //Ư�� lobby ���� ����
        TypedLobby typedLobby = new TypedLobby("WooriIsland", LobbyType.Default);

        //request to join lobby
        PhotonNetwork.JoinLobby(typedLobby);

    }

    //�κ� ���� �Ϸ� �޼���
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print(nameof(OnJoinedLobby));

        //�� ���� or �� ����
        RoomOptions roomOptions = new RoomOptions();

        familyCode = InfoManager.Instance.FamilyCode;
        print($"InfoManager���Լ� ������ FamilyCode�� {familyCode} �Դϴ�.");
        PhotonNetwork.JoinOrCreateRoom(familyCode, roomOptions, TypedLobby.Default);
    }

    //�� ���� �Ϸ� �޼���
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print(nameof(OnCreatedRoom));
    }

    //�� ���� ���� �޼���
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print(nameof(OnCreateRoomFailed));
    }

    //�� ���� �Ϸ� �޼���
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print(nameof(OnJoinedRoom));

        //Game Scene���� �̵�
        PhotonNetwork.LoadLevel(4); // build setting ���� 3�� ������ �̵�
    }

}
