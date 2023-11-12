using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviourPun
{
    // ��� �ÿ��̾�
    //public GameObject[] players;

    // ���� ī�޶�
    public Camera camera;

    //private bool state = true;

    // �������� �г���
    //public TMP_Text nickName;
    public TextMeshProUGUI nickName;

    public GameObject playerList;


    //public GameObject[] models;

    private void Start()
    {
        // �� ĳ���Ͱ� �ƴϸ� ī�޶� ����
        if(!photonView.IsMine)
        {
            camera.enabled = false;
        }

        // �г��� ����
        nickName.text = photonView.Owner.NickName; // connection manager�� join room���� ��������

    }

    private void Update()
    {
        // Test
    }

    // �÷��̾� ī�޶�� �÷��̾� ���¸� ���� �Ѵ� �Լ�
/*    public void OnOff()
    {
        state = !state;

        foreach (GameObject go in players)
        {
            go.SetActive(state);
        }

        camera.gameObject.SetActive(state);
    }*/

    public void SelectModel(string characterName)
    {
        // rpc �Լ��� ĳ���͸� ����
        photonView.RPC(nameof(RpcSelectModel), RpcTarget.AllBuffered, characterName);
    }

    [PunRPC]
    void RpcSelectModel(string characterName)
    {
        // Player ������ �ȿ� ����ִ� ĳ���� ��
        foreach(Transform t in playerList.transform)
        {
            if(t.name == characterName)
            {
                t.gameObject.SetActive(true);
            }
        }

        //models[modelIdx].SetActive(true);
        //nickName = "";
    }
}
