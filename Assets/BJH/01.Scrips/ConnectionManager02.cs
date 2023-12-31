//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//// Photon.Pun, Photon.Realtime 선언
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
//        // btn 비활성
//        connectBtn.interactable = false;

//        // inputNickName의 내용이 변경될 때 호출되는 함수 등록
//        inputFamilyCode.onValueChanged.AddListener((string s) => { 
//            connectBtn.interactable = s.Length > 0;
//        });

//        // inputNickName에서 enter하면 호출되는 함수 등록
//        inputFamilyCode.onSubmit.AddListener((string s) =>
//        {
//            if (connectBtn.interactable == true) // UI 활성화
//            {
//                OnClickConnect();
//            }
//        });
//    }

//    public void OnClickConnect()
//    {
//        // 서버 접속 요청
//        PhotonNetwork.ConnectUsingSettings();
//        print(nameof(OnClickConnect));
//    }


//    // Master 연결됐는지 확인하는 메서드
//    public override void OnConnectedToMaster()
//    {
//        base.OnConnectedToMaster();
//        print(nameof(OnConnectedToMaster));

//        // master scene에 접속했으면
//        // 로비에 진입
//        JoinLobby();
//    }

//    // Lobby에 접속하는 메서드
//    // Lobby는 가족 코드
//    void JoinLobby()
//    {
//        // setting nickname
//        PhotonNetwork.NickName = inputNickName.text;

//        // 특정 lobby 정보 셋팅
//        TypedLobby typedLobby = new TypedLobby("WooriIsland", LobbyType.Default);

//        // request to join lobby
//        PhotonNetwork.JoinLobby(typedLobby);

//    }

//    // 로비 진입 완료 메서드
//    public override void OnJoinedLobby()
//    {
//        base.OnJoinedLobby();
//        print(nameof(OnJoinedLobby));

//        // 방 생성 or 방 진입
//        RoomOptions roomOptions = new RoomOptions();
//        PhotonNetwork.JoinOrCreateRoom(inputFamilyCode.text, roomOptions, TypedLobby.Default);
//    }

//    // 방 생성 완료 메서드
//    public override void OnCreatedRoom()
//    {
//        base.OnCreatedRoom();
//        print(nameof(OnCreatedRoom));
//    }

//    // 방 생성 실패 메서드
//    public override void OnCreateRoomFailed(short returnCode, string message)
//    {
//        base.OnCreateRoomFailed(returnCode, message);
//        print(nameof(OnCreateRoomFailed));
//    }

//    // 방 진입 완료 메서드
//    public override void OnJoinedRoom()
//    {
//        base.OnJoinedRoom();
//        print(nameof(OnJoinedRoom));

//        // Game Scene으로 이동
//        PhotonNetwork.LoadLevel(3); // build setting 기준 1번 씬으로 이동
//    }

//}
