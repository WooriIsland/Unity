using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public void CreateRoom()
    {
        // �� �ɼ� ����
        RoomOptions roomOption = new RoomOptions();

        // ����, ����� ����
        roomOption.IsVisible = true;

        // �� ����
        PhotonNetwork.CreateRoom(InfoManager.Instance.IslandName, roomOption);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print($"������ ���� : {InfoManager.Instance.IslandName}");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print($"������ ���� ���� : {message}");
    }
}
