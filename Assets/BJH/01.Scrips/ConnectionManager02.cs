using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

// connect scene : 로그인
// 로그인이 되어있다면? 가족코드 입력
// 가족코드가 입력되어있다면? 바로 접속
// lobby 불필요

public class ConnectionManager02 : MonoBehaviourPunCallbacks
{
    // nick name
    public InputField inputNickName;

    // family code
    public InputField inputFamilyCode;

    // connect btn
    public Button connectBtn;

    // Start is called before the first frame update
    void Start()
    {
        // inputNickName의 내용이 변경될 때 호출되는 함수 등록
        inputFamilyCode.onValueChanged.AddListener((string s) => { 
            connectBtn.interactable = s.Length > 0;
        });

        // inputNickName에서 enter하면 호출되는 함수 등록
        inputFamilyCode.onSubmit.AddListener((string s) =>
        {
            if (connectBtn.interactable == true)
            {
                OnClickConnect();
            }
        });
        
        // btn 비활성
        connectBtn.interactable = false;
    }

    public void OnClickConnect()
    {
        // 서버 접속 요청
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        // setting nickname
        PhotonNetwork.NickName = inputNickName.text;

        // 특정 lobby 정보 셋팅
        TypedLobby typedLobby = new TypedLobby("Family Lobby", LobbyType.Default);

        // request to join lobby
        PhotonNetwork.JoinLobby(typedLobby);
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        // lobby scene으로 이동

    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
