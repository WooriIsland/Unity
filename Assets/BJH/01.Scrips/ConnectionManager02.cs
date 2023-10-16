using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

// connect scene : �α���
// �α����� �Ǿ��ִٸ�? �����ڵ� �Է�
// �����ڵ尡 �ԷµǾ��ִٸ�? �ٷ� ����
// lobby ���ʿ�

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
        // inputNickName�� ������ ����� �� ȣ��Ǵ� �Լ� ���
        inputFamilyCode.onValueChanged.AddListener((string s) => { 
            connectBtn.interactable = s.Length > 0;
        });

        // inputNickName���� enter�ϸ� ȣ��Ǵ� �Լ� ���
        inputFamilyCode.onSubmit.AddListener((string s) =>
        {
            if (connectBtn.interactable == true)
            {
                OnClickConnect();
            }
        });
        
        // btn ��Ȱ��
        connectBtn.interactable = false;
    }

    public void OnClickConnect()
    {
        // ���� ���� ��û
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        // setting nickname
        PhotonNetwork.NickName = inputNickName.text;

        // Ư�� lobby ���� ����
        TypedLobby typedLobby = new TypedLobby("Family Lobby", LobbyType.Default);

        // request to join lobby
        PhotonNetwork.JoinLobby(typedLobby);
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        // lobby scene���� �̵�

    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
