using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public void CreateRoom()
    {
        // 规 可记 汲沥
        RoomOptions roomOption = new RoomOptions();

        // 傍俺, 厚傍俺 咯何
        roomOption.IsVisible = true;

        // 规 积己
        PhotonNetwork.CreateRoom(InfoManager.Instance.IslandName, roomOption);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print($"啊练级 积己 : {InfoManager.Instance.IslandName}");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print($"啊练级 积己 角菩 : {message}");
    }
}
