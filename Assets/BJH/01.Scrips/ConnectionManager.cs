using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Photon.Pun, Photon.Realtime ����
using Photon.Pun;
using Photon.Realtime;



// MonoBehaviourPunCallbacks�� ���
public class ConnectionManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        // Photon ȯ�漳���� ������� ���� �õ��ϴ� �޼���
        PhotonNetwork.ConnectUsingSettings();
    }

    // Master�� ����ƴ��� Ȯ���ϴ� �޼���
    // MonoBehaviourPunCallbacks�� ����� �޼��� overried
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster(); // base == �θ� ���� ���� �� ���� ����(������)
        print(nameof(OnConnectedToMaster));

        // master scene�� ����������
        // �κ� ����
        JoinLobby();
    }

    // Lobby�� �����ϴ� �޼���
    void JoinLobby()
    {
        // �г��� ����
        // ����� ���Ƿ� ����
        PhotonNetwork.NickName = "����Park";

        // �⺻ �κ� ����
        PhotonNetwork.JoinLobby();
    }

    // �κ� ���� �Ϸ� �޼���
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print(nameof(OnJoinedLobby));

        // �� ���� or �� ����
        RoomOptions roomOptions = new RoomOptions();
        PhotonNetwork.JoinOrCreateRoom("WooriIsland", roomOptions, TypedLobby.Default);

    }

    // �� ���� �Ϸ� �޼���
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print(nameof(OnCreatedRoom));
    }

    // �� ���� ���� �޼���
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print(nameof(OnCreateRoomFailed));
    }

    // �� ���� �Ϸ� �޼���
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print(nameof(OnJoinedRoom));

        // Game Scene���� �̵�
        PhotonNetwork.LoadLevel(1); // build setting ���� 1�� ������ �̵�
    }
}