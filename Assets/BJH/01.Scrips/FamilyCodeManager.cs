using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

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

public class FamilyCodeManager : MonoBehaviour
{
    public GameObject createIsland, islandSelect, islandCustom, islandCode;

    string selectedIsland, islandName, islandIntroduce;
    bool secret;
    string familyCode;

    private void Start()
    {
        createIsland.SetActive(false);    
    }

    public void OnClickJoinBtn(string code)
    {
        //int num = UnityEngine.Random.Range(0, 100);
        ////현숙 임시 구현(조건문으로 가야함)
        //nickName = "정이" + num.ToString();
        familyCode = code;

        

        InfoManager.Instance.FamilyCode = code;


        ////지환 구현 
        ///*nickName = inputNickName.text;
        //byte[] a = System.Text.Encoding.UTF8.GetBytes(nickName);
        //familyCode = inputFamilyCode.text;*/

        //PlayerPrefs.SetString("NickName", nickName);

        SceneManager.LoadScene(3);
    }

    public void OnClick_CreateIsland()
    {
        createIsland.SetActive(true);
    }

    public void Onclick_SelectedIsland(string s)
    {
        selectedIsland = s;
    }

    public void OnClick_CustomIsland()
    {
        islandSelect.SetActive(false);
        islandCustom.SetActive(true);
    }

    public void Onclick_GetFamilyCode()
    {
        islandCustom.SetActive(false);
        islandCode.SetActive(true);

        CreateFamilyCode();
    }



    public void OnClick_CloseCreateIsland()
    {
        islandCode.SetActive(false);
        createIsland.SetActive(false);
    }

    private void CreateFamilyCode()
    {
        int minValue = 1;
        int maxValue = 100;
        string familyCode = "FamilyCode" + UnityEngine.Random.Range(minValue, maxValue);
        islandCode.GetComponent<CreateIslandInfo>().code.text = familyCode;
        InfoManager.Instance.FamilyCode = familyCode;
    }
}
