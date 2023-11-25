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
  
    // ����� �� ���� �ҷ��ͼ� create or join
    public void CreateOrJoinRoom()
    {
        if(InfoManager.Instance.visit == null || InfoManager.Instance.visit == "")
        {
            print("��������");
            RoomOptions option = new RoomOptions();
            //option.IsOpen = InfoManager.Instance.Secret;
            PhotonNetwork.JoinOrCreateRoom(InfoManager.Instance.IslandName, option, TypedLobby.Default);
        }
        else
        {
            print("��������");
            RoomOptions option = new RoomOptions();
            //option.IsOpen = InfoManager.Instance.Secret;
            PhotonNetwork.JoinOrCreateRoom(InfoManager.Instance.visit, option, TypedLobby.Default);
        }
        
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

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("�� ���� �Ϸ�");

        InfoManager.Instance.visit = null;

        string type = InfoManager.Instance.visitType;
        if(type == "Island01") // ���� ��
        {
            // ���� ������ �̵�
            PhotonNetwork.LoadLevel(4);
        }

        else // �Ϲ� ��
        {
            // ���� ������ �̵�
            PhotonNetwork.LoadLevel(5);
        }

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print($"�� ���� ���� : {message}");
    }


    // �� ����
    //public void JoinRoom(GameObject go)
    //{
    //    string islandName = go.GetComponent<CreatedRoomInfo>().islandName.text;

    //    // �� ����
    //    PhotonNetwork.JoinRoom(islandName);
    //    print(islandName);
    //}



    //public void OnClickJoinBtn(string code)
    //{
    //    //int num = UnityEngine.Random.Range(0, 100);
    //    ////���� �ӽ� ����(���ǹ����� ������)
    //    //nickName = "����" + num.ToString();

    //    ////��ȯ ���� 
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
//            islandCustom.GetComponent<CreateIslandManager>().islandName.text = "���� & ����";
//        }
//#endif
//    }

