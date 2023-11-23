using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

// �� ���
//{
//    "createdAt": "2023-11-21T04:36:06.582Z",
//  "lastModifiedAt": "2023-11-21T04:36:06.582Z",
//  "islandId": 0,
//  "islandUniqueNumber": "string",
//  "islandName": "string",
//  "island_introduce": "string",
//  "daysSinceCreation": 0,
//  "secret": true
//}

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public GameObject createIsland, islandSelect, islandCustom, islandCode;
    string islandName;

    // �� ����
    public void CreateRoom()
    {
        // �� �ɼ� ����
        RoomOptions roomOption = new RoomOptions();

        // ����, ����� ����
        roomOption.IsVisible = true; // �ӽ�

        // �� ����
        islandName = islandCustom.GetComponent<CreateIslandInfo>().islandName.text;
        InfoManager.Instance.IslandName = islandName; // �� �̸� info�� ����
        InfoManager.Instance.IslandIntroduce = islandCustom.GetComponent<CreateIslandInfo>().introduce.text; // �� ���� info�� ����
        TypedLobby typedLobby = new TypedLobby("Woori Island", LobbyType.Default);
        PhotonNetwork.CreateRoom(islandName, roomOption, typedLobby);
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


    // �� ����
    public void JoinRoom(GameObject go)
    {
        string islandName = go.GetComponent<CreatedRoomInfo>().islandName.text;
        PhotonNetwork.JoinRoom(islandName);
        print(islandName);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("�� ���� �Ϸ�");

        // ���� ������ �̵�
        SceneManager.LoadScene(3);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print($"�� ���� ���� : {message}");
    }







    // ������Ʈ ����
    public void Onclick_CloseBtn(GameObject go)
    {
        go.SetActive(false);
    }

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





    // �� ���� ��ư Ŭ��
    bool state = false;
    public void OnClick_CreateIsland(GameObject go)
    {
        state = !go.activeSelf;
        go.SetActive(state);
        islandSelect.SetActive(state);
    }

    public void Onclick_SelectIslandType(string s)
    {
        InfoManager.Instance.IslandType = s;
    }

    public void OnClick_Open_IslandCustomPage()
    {
        islandSelect.SetActive(false);
        islandCustom.SetActive(true);
    }

    public void OnClick_Open_GetIslandCode()
    {
        islandCustom.SetActive(false);
        islandCode.SetActive(true);
        CreateFamilyCode();
    }

    public void OnClick_Close_IslandCode(GameObject go)
    {
        islandCode.SetActive(false);
        go.SetActive(false);


        CreateRoom();

    }

    // �ӽ�
    private void CreateFamilyCode()
    {
        // �� ���� �����Ǹ� �ּ� Ǯ��
        //int minValue = 1;
        //int maxValue = 100;
        //string familyCode = "FamilyCode" + UnityEngine.Random.Range(minValue, maxValue);

        string familyCode = "familycode123";
        islandCode.GetComponent<CreateIslandInfo>().code.text = familyCode;
        InfoManager.Instance.IslandCode = familyCode;
    }


    //public void Onclick_GetFamilyCode()
    //{
    //    // Custom���� ��� ���� �Է� �� ���� ��ư�� ������
    //    // Custom�ߴ� ������ �ް�
    //    // Custom���� Code�� �̵�x
    //    var islandInfo = islandCustom.GetComponent<CreateIslandInfo>();

    //    InfoManager.Instance.IslandName = islandInfo.islandName.text;
    //    InfoManager.Instance.IslandIntroduce = islandInfo.introduce.text;

    //    islandCustom.SetActive(false);
    //    islandCode.SetActive(true);

    //    CreateFamilyCode();
    //}



    //public void OnClick_CloseCreateIsland()
    //{
    //    islandCode.SetActive(false);
    //    createIsland.SetActive(false);

    //    //ConnectionManager03.Instance.CreateRoom();
    //}

    //private void CreateFamilyCode()
    //{
    //    // �� ���� �����Ǹ� �ּ� Ǯ��
    //    //int minValue = 1;
    //    //int maxValue = 100;
    //    //string familyCode = "FamilyCode" + UnityEngine.Random.Range(minValue, maxValue);

    //    string familyCode = "familycode123";
    //    islandCode.GetComponent<CreateIslandInfo>().code.text = familyCode;
    //    InfoManager.Instance.IslandCode = familyCode;
    //}
}
