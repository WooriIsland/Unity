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
    // �ν��Ͻ�
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
        // PUN �ݹ� ���
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDestroy()
    {
        // PUN �ݹ� ����
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // ���ο� �÷��̾ room�� �����ϸ� �÷��̾� ����Ʈ�� ������
        Photon.Realtime.Player[] playerList = PhotonNetwork.PlayerList;

        // �÷��̾� ����Ʈ ���
        foreach (var player in playerList)
        {
            Debug.Log("Nickname: " + player.NickName);
        }
    }



    public void OnClickConnect()
    {
        //���� ���� ��û
        PhotonNetwork.ConnectUsingSettings();
        print(nameof(OnClickConnect));
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
        string nickName = PlayerPrefs.GetString("NickName");
        PhotonNetwork.NickName = nickName;

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
        string familyCode = PlayerPrefs.GetString("FamilyCode");
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
        PhotonNetwork.LoadLevel(3); // build setting ���� 3�� ������ �̵�
    }

}
