using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;

public class ConnectedPlayerInfo : MonoBehaviourPunCallbacks
{
    // ������ �÷��̾� ������ ��� ����Ʈ
    public string[] joinedPlayers;

    private static ConnectedPlayerInfo instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public static ConnectedPlayerInfo Instance
    {
        get
        {
            return instance;
        }
    }


    private void Start()
    {
        // PUN �ݹ� ���
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void Update()
    {
        foreach(var player in PhotonNetwork.PlayerList)
        {
            print("������ �÷��̾� ���� : " + player.NickName);

        }
    }

    private void OnDestroy()
    {
        // PUN �ݹ� ����
        PhotonNetwork.RemoveCallbackTarget(this);
    }



    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // ���ο� �÷��̾ �濡 �������� �� ����Ǵ� �ڵ�
        Debug.Log(newPlayer.NickName + "��(��) �濡 �����߽��ϴ�!");
    }

    

    public string[] GetJoinedPlayerLIst()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            joinedPlayers[i] = PhotonNetwork.PlayerList[i].NickName;
        }
        
        return joinedPlayers;
    }
}
