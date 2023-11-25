using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
  
    // 저장된 방 정보 불러와서 create or join
    public void CreateOrJoinRoom()
    {
        if(InfoManager.Instance.visit == null || InfoManager.Instance.visit == "")
        {
            print("정보없음");
            RoomOptions option = new RoomOptions();
            //option.IsOpen = InfoManager.Instance.Secret;
            PhotonNetwork.JoinOrCreateRoom(InfoManager.Instance.IslandName, option, TypedLobby.Default);
        }
        else
        {
            print("정보있음");
            RoomOptions option = new RoomOptions();
            //option.IsOpen = InfoManager.Instance.Secret;
            PhotonNetwork.JoinOrCreateRoom(InfoManager.Instance.visit, option, TypedLobby.Default);
        }
        
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print($"가족섬 생성 : {InfoManager.Instance.IslandName}");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print($"가족섬 생성 실패 : {message}");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("방 입장 완료");

        InfoManager.Instance.visit = null;

        string type = InfoManager.Instance.visitType;
        if(type == "Island01") // 얼음 섬
        {
            // 게임 씬으로 이동
            PhotonNetwork.LoadLevel(4);
        }

        else // 일반 섬
        {
            // 게임 씬으로 이동
            PhotonNetwork.LoadLevel(5);
        }

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print($"방 입장 실패 : {message}");
    }


    // 방 입장
    //public void JoinRoom(GameObject go)
    //{
    //    string islandName = go.GetComponent<CreatedRoomInfo>().islandName.text;

    //    // 방 입장
    //    PhotonNetwork.JoinRoom(islandName);
    //    print(islandName);
    //}



    //public void OnClickJoinBtn(string code)
    //{
    //    //int num = UnityEngine.Random.Range(0, 100);
    //    ////현숙 임시 구현(조건문으로 가야함)
    //    //nickName = "정이" + num.ToString();

    //    ////지환 구현 
    //    ///*nickName = inputNickName.text;
    //    //byte[] a = System.Text.Encoding.UTF8.GetBytes(nickName);
    //    //familyCode = inputFamilyCode.text;*/

    //    //PlayerPrefs.SetString("NickName", nickName);

    //    //SceneManager.LoadScene(3);

    //    //ConnectionManager03.Instance.JoinRoom(code);

    //}




}








//    private void Update()
//    {
//#if UNITY_EDITOR
//        if (Input.GetKeyDown(KeyCode.F2))
//        {
//            islandCustom.GetComponent<CreateIslandManager>().islandName.text = "정이 & 혜리";
//        }
//#endif
//    }

