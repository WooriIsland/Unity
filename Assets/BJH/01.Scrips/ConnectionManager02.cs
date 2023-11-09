//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//// Photon.Pun, Photon.Realtime ����
//using Photon.Pun;
//using Photon.Realtime;
//using TMPro;


//public class ConnectionManager03 : MonoBehaviourPunCallbacks
//{
//    private static ConnectionManager03 instance;

//    void Awake()
//    {
//        if (instance == null) 
//        { 
//            instance = this;
//            DontDestroyOnLoad(this.gameObject);
//        }
//    }

//    // nick name
//    public TMP_Text inputNickName;

//    // family code
//    public InputField inputFamilyCode;

//    // connect btn
//    public Button connectBtn;

//    // Start is called before the first frame update
//    void Start()
//    {
//        // btn ��Ȱ��
//        connectBtn.interactable = false;

//        // inputNickName�� ������ ����� �� ȣ��Ǵ� �Լ� ���
//        inputFamilyCode.onValueChanged.AddListener((string s) => { 
//            connectBtn.interactable = s.Length > 0;
//        });

//        // inputNickName���� enter�ϸ� ȣ��Ǵ� �Լ� ���
//        inputFamilyCode.onSubmit.AddListener((string s) =>
//        {
//            if (connectBtn.interactable == true) // UI Ȱ��ȭ
//            {
//                OnClickConnect();
//            }
//        });
//    }

//    public void OnClickConnect()
//    {
//        // ���� ���� ��û
//        PhotonNetwork.ConnectUsingSettings();
//        print(nameof(OnClickConnect));
//    }


//    // Master ����ƴ��� Ȯ���ϴ� �޼���
//    public override void OnConnectedToMaster()
//    {
//        base.OnConnectedToMaster();
//        print(nameof(OnConnectedToMaster));

//        // master scene�� ����������
//        // �κ� ����
//        JoinLobby();
//    }

//    // Lobby�� �����ϴ� �޼���
//    // Lobby�� ���� �ڵ�
//    void JoinLobby()
//    {
//        // setting nickname
//        PhotonNetwork.NickName = inputNickName.text;

//        // Ư�� lobby ���� ����
//        TypedLobby typedLobby = new TypedLobby("WooriIsland", LobbyType.Default);

//        // request to join lobby
//        PhotonNetwork.JoinLobby(typedLobby);

//    }

//    // �κ� ���� �Ϸ� �޼���
//    public override void OnJoinedLobby()
//    {
//        base.OnJoinedLobby();
//        print(nameof(OnJoinedLobby));

//        // �� ���� or �� ����
//        RoomOptions roomOptions = new RoomOptions();
//        PhotonNetwork.JoinOrCreateRoom(inputFamilyCode.text, roomOptions, TypedLobby.Default);
//    }

//    // �� ���� �Ϸ� �޼���
//    public override void OnCreatedRoom()
//    {
//        base.OnCreatedRoom();
//        print(nameof(OnCreatedRoom));
//    }

//    // �� ���� ���� �޼���
//    public override void OnCreateRoomFailed(short returnCode, string message)
//    {
//        base.OnCreateRoomFailed(returnCode, message);
//        print(nameof(OnCreateRoomFailed));
//    }

//    // �� ���� �Ϸ� �޼���
//    public override void OnJoinedRoom()
//    {
//        base.OnJoinedRoom();
//        print(nameof(OnJoinedRoom));

//        // Game Scene���� �̵�
//        PhotonNetwork.LoadLevel(3); // build setting ���� 1�� ������ �̵�
//    }

//}
