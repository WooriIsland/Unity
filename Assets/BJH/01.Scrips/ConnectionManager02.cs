using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Photon.Pun, Photon.Realtime ����
using Photon.Pun;
using Photon.Realtime;

// connect scene : �α���
// �α����� �Ǿ��ִٸ�? �����ڵ� �Է�
// �����ڵ尡 �ԷµǾ��ִٸ�? �ٷ� ����
// lobby ���ʿ�

public class ConnectionManager02 : MonoBehaviourPunCallbacks
{
    private static ConnectionManager02 instance;

    void Awake()
    {
        if (instance == null) 
        { 
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // nick name
    public InputField inputNickName;

    // family code
    public InputField inputFamilyCode;

    // connect btn
    public Button connectBtn;

    // Start is called before the first frame update
    void Start()
    {
        // btn ��Ȱ��
        connectBtn.interactable = false;

        // inputNickName�� ������ ����� �� ȣ��Ǵ� �Լ� ���
        inputFamilyCode.onValueChanged.AddListener((string s) => { 
            connectBtn.interactable = s.Length > 0;
        });

        // inputNickName���� enter�ϸ� ȣ��Ǵ� �Լ� ���
        inputFamilyCode.onSubmit.AddListener((string s) =>
        {
            if (connectBtn.interactable == true) // UI Ȱ��ȭ
            {
                OnClickConnect();
            }
        });
    }

    public void OnClickConnect()
    {
        // ���� ���� ��û
        PhotonNetwork.ConnectUsingSettings();
        print(nameof(OnClickConnect));
    }


    // Master ����ƴ��� Ȯ���ϴ� �޼���
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print(nameof(OnConnectedToMaster));

        // master scene�� ����������
        // �κ� ����
        JoinLobby();
    }

    // Lobby�� �����ϴ� �޼���
    void JoinLobby()
    {
        // setting nickname
        PhotonNetwork.NickName = inputNickName.text; // �ӽ�

        // Ư�� lobby ���� ����
        TypedLobby typedLobby = new TypedLobby("Family Lobby", LobbyType.Default);

        // request to join lobby
        PhotonNetwork.JoinLobby(typedLobby);

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
