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
        if (instance = null)
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

    private void OnDestroy()
    {
        // PUN �ݹ� ����
        PhotonNetwork.RemoveCallbackTarget(this);
    }



    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // ���ο� �÷��̾ room�� �����ϸ� �÷��̾� ����Ʈ�� ������
        Photon.Realtime.Player[] playerList = PhotonNetwork.PlayerList;
        joinedPlayers = new string[playerList.Length];

        // �÷��̾� ����Ʈ ��� �� �迭�� ����
        foreach (var player in playerList)
        {
            Debug.Log("Nickname: " + player.NickName);

            for (int i = 0; i < playerList.Length; i++)
            {
                joinedPlayers[i] = player.NickName;
            }

        }
    }
}
