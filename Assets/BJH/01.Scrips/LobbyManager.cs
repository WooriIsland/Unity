using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

// 섬 등록
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

    // 방 생성
    public void CreateRoom()
    {
        // 방 옵션 설정
        RoomOptions roomOption = new RoomOptions();

        // 공개, 비공개 여부
        roomOption.IsVisible = true; // 임시

        // 방 생성
        islandName = islandCustom.GetComponent<CreateIslandInfo>().islandName.text;
        InfoManager.Instance.IslandName = islandName; // 방 이름 info에 저장
        InfoManager.Instance.IslandIntroduce = islandCustom.GetComponent<CreateIslandInfo>().introduce.text; // 방 설명 info에 저장
        TypedLobby typedLobby = new TypedLobby("Woori Island", LobbyType.Default);
        PhotonNetwork.CreateRoom(islandName, roomOption, typedLobby);
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


    // 방 입장
    public void JoinRoom(GameObject go)
    {
        string islandName = go.GetComponent<CreatedRoomInfo>().islandName.text;
        PhotonNetwork.JoinRoom(islandName);
        print(islandName);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("방 입장 완료");

        // 게임 씬으로 이동
        SceneManager.LoadScene(3);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print($"방 입장 실패 : {message}");
    }







    // 오브젝트 끄기
    public void Onclick_CloseBtn(GameObject go)
    {
        go.SetActive(false);
    }

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





    // 섬 생성 버튼 클릭
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

    // 임시
    private void CreateFamilyCode()
    {
        // 방 생성 구현되면 주석 풀기
        //int minValue = 1;
        //int maxValue = 100;
        //string familyCode = "FamilyCode" + UnityEngine.Random.Range(minValue, maxValue);

        string familyCode = "familycode123";
        islandCode.GetComponent<CreateIslandInfo>().code.text = familyCode;
        InfoManager.Instance.IslandCode = familyCode;
    }


    //public void Onclick_GetFamilyCode()
    //{
    //    // Custom에서 모든 정보 입력 후 다음 버튼을 누르면
    //    // Custom했던 정보를 받고
    //    // Custom에서 Code로 이동x
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
    //    // 방 생성 구현되면 주석 풀기
    //    //int minValue = 1;
    //    //int maxValue = 100;
    //    //string familyCode = "FamilyCode" + UnityEngine.Random.Range(minValue, maxValue);

    //    string familyCode = "familycode123";
    //    islandCode.GetComponent<CreateIslandInfo>().code.text = familyCode;
    //    InfoManager.Instance.IslandCode = familyCode;
    //}
}
