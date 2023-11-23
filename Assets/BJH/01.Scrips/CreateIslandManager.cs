using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateIslandManager : MonoBehaviour
{
    public GameObject createIsland, islandSelect, islandCustom, islandCode;
    string code;

    private void Start()
    {
        createIsland.SetActive(false);
    }

    // 오브젝트 끄기 : X 버튼
    public void CloseObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
    
    // 섬 생성 : 섬 생성버튼 클릭
    public void OnClick_CreateIslandBtn()
    {
        createIsland.SetActive(true);
    }


    // 섬 타입 선택
    public void OnClick_SelectIsland(string islandType)
    {
        InfoManager.Instance.IslandType = islandType;
    }

    // 섬 타입 선택 후 섬 커스텀 창으로 이동
    public void Onclick_GoIslandCustom()
    {
        islandSelect.SetActive(false);
        islandCustom.SetActive(true);
    }


    // 섬 이름, 섬 소개, 공개/비공개 저장
    public void CustomIsland()
    {
        var info = islandCustom.GetComponent<CreateIslandInfo>();
        InfoManager.Instance.IslandName = info.islandName.text;
        InfoManager.Instance.IslandIntroduce = info.introduce.text;
        InfoManager.Instance.Secret = !info.secret;
        print(info.secret);

        islandCustom.SetActive(false);
        islandCode.SetActive(true);

        GetFamilyCode();
    }


    // 가족섬 코드 제공
    public void GetFamilyCode()
    {
        // 임시 : 임의로 가족코드 생성 및 노출
        code = "Woori1339";
        islandCode.GetComponent<CreateIslandInfo>().code.text = code;


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



    // 가족섬 코드 저장 후 캐릭터 선택 씬으로 이동
    public void GoCharacterScene()
    {
        InfoManager.Instance.FamilyCode = code;

        // 캐릭터 선택 창으로 이동
        SceneManager.LoadScene(3);
    }


    // 생성된 섬을 클릭하면, 저장된 가족섬 코드를 임시로 저장해두기
    public void WantVisitThisIsland(GameObject go)
    {
        InfoManager.Instance.visit = go.GetComponent<CreatedRoomInfo>().islandName.text;
        SceneManager.LoadScene(3);
    }


}
