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
    public Camera roomCam;
    public bool isMine;
    //private bool state = true;

    // 프리팹의 닉네임
    //public TMP_Text nickName;
    public TMP_Text nickName;

    public GameObject playerList;

    //public GameObject[] models;

    private void Start()
    {
        // 내 캐릭터가 아니면
        if(!photonView.IsMine)
        {
            // 카메라 끄기
            camera.enabled = false;
            roomCam.enabled = false;
            isMine = false;            
        }
        // 내 캐릭터면
        else
        {
            // 카메라 켜기
            isMine = true;

            // 닉네임 끄기
            nickName.enabled = false;
        }

        // 닉네임 설정
        nickName.text = photonView.Owner.NickName; // connection manager의 join room에서 설정해줌

        // 접속한것으로 셋팅  
        PlayerStateManager.instance.ChangeOffLine(photonView.Owner.NickName, false);
    }

    private void OnDestroy()
    {
        // 접속한것으로 셋팅  
        PlayerStateManager.instance.ChangeOffLine(photonView.Owner.NickName, true);
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

    public void SelectModel(string characterName)
    {
        // rpc 함수로 캐릭터를 생성
        photonView.RPC(nameof(RpcSelectModel), RpcTarget.AllBuffered, characterName);

        
    }

    [PunRPC]
    void RpcSelectModel(string characterName)
    {
        // Player 프리팹 안에 들어있는 캐릭터 중
        foreach(Transform t in playerList.transform)
        {
            if(t.name == characterName)
            {
                t.gameObject.SetActive(true);
                break;
            }
        }

        ChatManager.Instance.dicAllPlayerProfile[nickName.text] = characterName;



        //models[modelIdx].SetActive(true);
        //nickName = "";
    }
}
