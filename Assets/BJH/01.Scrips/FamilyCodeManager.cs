using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

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

public class FamilyCodeManager : MonoBehaviour
{
    public GameObject createIsland, islandSelect, islandCustom, islandCode;

    private void Start()
    {
        createIsland.SetActive(false);    
    }

    public void OnClickJoinBtn(string code)
    {
        //int num = UnityEngine.Random.Range(0, 100);
        ////���� �ӽ� ����(���ǹ����� ������)
        //nickName = "����" + num.ToString();

        ////��ȯ ���� 
        ///*nickName = inputNickName.text;
        //byte[] a = System.Text.Encoding.UTF8.GetBytes(nickName);
        //familyCode = inputFamilyCode.text;*/

        //PlayerPrefs.SetString("NickName", nickName);

        //SceneManager.LoadScene(3);

        ConnectionManager03.Instance.JoinRoom(code);

    }

    public void OnClick_CreateIsland()
    {
        createIsland.SetActive(true);
    }

    public void Onclick_SelectedIsland(string s)
    {
        InfoManager.Instance.IslandType = s;
    }

    public void OnClick_CustomIsland()
    {
        islandSelect.SetActive(false);
        islandCustom.SetActive(true);
    }

    public void Onclick_GetFamilyCode()
    {
        // Custom���� ��� ���� �Է� �� ���� ��ư�� ������
        // Custom�ߴ� ������ �ް�
        // Custom���� Code�� �̵�x
        var islandInfo = islandCustom.GetComponent<CreateIslandInfo>();

        InfoManager.Instance.IslandName = islandInfo.islandName.text;
        InfoManager.Instance.IslandIntroduce = islandInfo.introduce.text;

        islandCustom.SetActive(false);
        islandCode.SetActive(true);

        CreateFamilyCode();
    }



    public void OnClick_CloseCreateIsland()
    {
        islandCode.SetActive(false);
        createIsland.SetActive(false);

        ConnectionManager03.Instance.CreateRoom();
    }

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
}
