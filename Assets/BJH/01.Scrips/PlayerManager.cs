using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviourPun
{
    // 모든 플에이어
    //public GameObject[] players;

    // 나의 카메라
    public Camera camera;

    //private bool state = true;

    // 프리팹의 닉네임
    //public TMP_Text nickName;
    public TextMeshProUGUI nickName;


    public GameObject[] models;

    private void Start()
    {
        //players = GameObject.FindGameObjectsWithTag("Player");

        //만약에 내가 만든 캐릭터가 아니라면 카메라를 꺼주자
        if(!photonView.IsMine)
        {
            camera.enabled = false;
        }

        // 닉네임 설정
        nickName.text = photonView.Owner.NickName;

    }

    private void Update()
    {
        // Test
    }

    // 플레이어 카메라와 플레이어 상태를 껐다 켜는 함수
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
