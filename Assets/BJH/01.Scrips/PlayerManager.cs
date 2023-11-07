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


    public GameObject[] models;

    private void Start()
    {
        //players = GameObject.FindGameObjectsWithTag("Player");

        //���࿡ ���� ���� ĳ���Ͱ� �ƴ϶�� ī�޶� ������
        if(!photonView.IsMine)
        {
            camera.enabled = false;
        }

        // �г��� ����
        nickName.text = photonView.Owner.NickName;

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

    public void SelectModel(int modelIdx)
    {
        photonView.RPC(nameof(RpcSelectModel), RpcTarget.AllBuffered, modelIdx);
    }

    [PunRPC]
    void RpcSelectModel(int modelIdx)
    {
        models[modelIdx].SetActive(true);
        //nickName = "";
    }
}
