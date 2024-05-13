using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Photon.Pun, Photon.Realtime ����
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

// ����ȯ
// ���� ������ ���� �κ�, �� ������ ���� Ŭ����
public class ConnectionManager : MonoBehaviourPunCallbacks
{
    //public string nickName;
    //public string familyCode;
    //public string characterName;

    // ���� �� ���� ��û
    public void OnClickConnect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // ������ ���� Ȯ��
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print(nameof(OnConnectedToMaster));

        PhotonNetwork.NickName = Managers.Info.NickName;

        TypedLobby typedLobby = new TypedLobby("Woori Island", LobbyType.Default);
        PhotonNetwork.JoinLobby(typedLobby);
    }


    // �κ� ���� �� ������ ���� ������ �̵�
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print(nameof(OnJoinedLobby));

        // �� �̵�
        SceneManager.LoadScene(2);

        ////�� ���� or �� ����
        //RoomOptions roomOptions = new RoomOptions();

        //familyCode = InfoManager.Instance.FamilyCode;
        //print($"InfoManager���Լ� ������ FamilyCode�� {familyCode} �Դϴ�.");
        //PhotonNetwork.JoinOrCreateRoom(familyCode, roomOptions, TypedLobby.Default);
    }
}
