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

    // ������Ʈ ���� : X ��ư
    public void CloseObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
    
    // �� ���� : �� ������ư Ŭ��
    public void OnClick_CreateIslandBtn()
    {
        createIsland.SetActive(true);
    }


    // �� Ÿ�� ����
    public void OnClick_SelectIsland(string islandType)
    {
        InfoManager.Instance.IslandType = islandType;
    }

    // �� Ÿ�� ���� �� �� Ŀ���� â���� �̵�
    public void Onclick_GoIslandCustom()
    {
        islandSelect.SetActive(false);
        islandCustom.SetActive(true);
    }


    // �� �̸�, �� �Ұ�, ����/����� ����
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


    // ������ �ڵ� ����
    public void GetFamilyCode()
    {
        // �ӽ� : ���Ƿ� �����ڵ� ���� �� ����
        code = "Woori1339";
        islandCode.GetComponent<CreateIslandInfo>().code.text = code;


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



    // ������ �ڵ� ���� �� ĳ���� ���� ������ �̵�
    public void GoCharacterScene()
    {
        InfoManager.Instance.FamilyCode = code;

        // ĳ���� ���� â���� �̵�
        SceneManager.LoadScene(3);
    }


    // ������ ���� Ŭ���ϸ�, ����� ������ �ڵ带 �ӽ÷� �����صα�
    public void WantVisitThisIsland(GameObject go)
    {
        InfoManager.Instance.visit = go.GetComponent<CreatedRoomInfo>().islandName.text;
        SceneManager.LoadScene(3);
    }


}
