using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Photon.Pun, Photon.Realtime ����
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

// ����ȯ
// ���� ������ ���� �κ�, �� ������ ���� Ŭ����
public class ConnectionManager : MonoBehaviourPunCallbacks
{
    public string nickName;
    public string familyCode;
    public string characterName;

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


    //public void LoadGameScene()
    //{
    //    SceneManager.LoadScene(4);
    //}



    //public void ConnectRequest()
    //{
    //    //���� ���� ��û
    //    PhotonNetwork.ConnectUsingSettings();
    //    print(nameof(ConnectRequest));
    //}




    ////Lobby�� �����ϴ� �޼���
    ////Lobby�� ���� �ڵ�
    //void JoinLobby()
    //{
    //    PhotonNetwork.NickName = InfoManager.Instance.NickName;

    //    //Ư�� lobby ���� ����
    //    TypedLobby typedLobby = new TypedLobby("WooriIsland", LobbyType.Default);

    //    //request to join lobby
    //    PhotonNetwork.JoinLobby(typedLobby);

    //}




    //public void CreateRoom()
    //{
    //    // �� �ɼ� ����
    //    RoomOptions roomOption = new RoomOptions();

    //    // ����, ����� ����
    //    roomOption.IsVisible = true;

    //    // �� ����
    //    PhotonNetwork.CreateRoom(InfoManager.Instance.IslandName, roomOption);
    //}

    //public override void OnCreatedRoom()
    //{
    //    base.OnCreatedRoom();
    //    print($"������ ���� : {InfoManager.Instance.IslandName}");
    //}

    //public override void OnCreateRoomFailed(short returnCode, string message)
    //{
    //    base.OnCreateRoomFailed(returnCode, message);
    //    print($"������ ���� ���� : {message}");
    //}

    //public void JoinRoom(string code)
    //{
    //    PhotonNetwork.JoinRoom(code);
    //}

    ////�� ���� �Ϸ� �޼���
    //public override void OnJoinedRoom()
    //{
    //    base.OnJoinedRoom();
    //    print(nameof(OnJoinedRoom));
    //}

}
