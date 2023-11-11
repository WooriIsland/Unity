using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerStateManager : MonoBehaviourPunCallbacks
{
    PhotonView pv;

    public Transform playerStateBox;
    public GameObject playerStatePrefab;

    string[] playerNames;

    private void Start()
    {
        PlayerUiSettingAtFirst();
    }

    void PlayerUiSettingAtFirst()
    {
        // ����� ���� ��
        //int familySize = 4;

        // ������ �÷��̾� ����
        //string[] playerNames = PhotonNetwork.PlayerList.Select(player => player.NickName).ToArray();

        // �ӽ�
        // member img �̸��� ���� string[]
        playerNames = new string[3];
        playerNames[0] = "dongsik";
        playerNames[1] = "dongdong";
        playerNames[2] = "sook";


        for (int i = 0; i < playerNames.Length; i++)
        {
            // ������ ����
            GameObject go = Instantiate(playerStatePrefab, playerStateBox);

            // �������� �˰��ִ� image ���ӿ�����Ʈ�� image ������Ʈ�� ������
            Image image = go.GetComponent<PlayerState>().playerImg.GetComponent<Image>();

            // resoures���� ������ ������
            Texture2D picture = Resources.Load<Texture2D>("Member/" + playerNames[i]);
            go.name = playerNames[i];

            // resources���� ������ ������ image�� �����ϱ�
            image.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));

            if (playerNames[i] == "dongsik" )
            {
                go.GetComponent<PlayerState>().offline.SetActive(false);
            }
        }
    }

    // �濡 ���ο� �÷��̾ �����ϸ� ȣ��Ǵ� �Լ�
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // ���ο� �÷��̾ �濡 �������� �� ����Ǵ� �ڵ�
        Debug.Log(newPlayer.NickName + "��(��) �濡 �����߽��ϴ�!");

        // ����
        // newPlayer.NIckName�� debug�� �����µ�, �̰� �����ϸ� null�� �߻�
        // ������Ÿ�Ե� string��.. �� �ȵǴ°���?

        pv.RPC(nameof(OnLineUI), RpcTarget.All, newPlayer.NickName);
    }

    // ������ �÷��̾� UI�� ��Ÿ���ִ� �޼���
    [PunRPC]
    void OnLineUI(string name)
    {
        GameObject go = GameObject.Find(name).gameObject;

        go.GetComponent<PlayerState>().offline.SetActive(false);
    }

    // �� ������ UI�� �������ִ� �޼���
    public void Member()
    {
           print("������ �÷��̾� ���� : " + PhotonNetwork.PlayerList[0].NickName);

            // ������ ����
            GameObject go = Instantiate(playerStatePrefab, playerStateBox);

            // �������� �˰��ִ� image ���ӿ�����Ʈ�� image ������Ʈ�� ������
            Image image = go.GetComponent<PlayerState>().playerImg.GetComponent<Image>();

            // resoures���� ������ ������
            Texture2D picture = Resources.Load<Texture2D>("Member/dongsik");

            // resources���� ������ ������ image�� �����ϱ�
            image.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));
    }

}
