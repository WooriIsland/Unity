using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateIslandManager : MonoBehaviour
{
    public GameObject createIsland, islandSelect, islandCustom, islandCode;
    string code;

    private void Start()
    {
        createIsland.SetActive(false);

        // ���ƿ� infoManager���� �ҷ�����
        if(InfoManager.Instance.likeCnt.ToString().Length > 0)
        {
            likeCnt.text = InfoManager.Instance.likeCnt.ToString();
        }
        
        like.SetActive(InfoManager.Instance.isLike);
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
        //HttpManager_LHS.instance.mainLoding.GetComponent<AlphaGPSSet>().OpenAlpha();
        
        // �湮 �ϰ���� ���� Ŭ���ϸ�? �ش��ϴ� ���� �� �̸�, �� ���� ����
        InfoManager.Instance.visit = go.GetComponent<CreatedRoomInfo>().islandName.text;
        InfoManager.Instance.visitType = go.GetComponent<CreatedRoomInfo>().islandType.name;

        SceneManager.LoadScene(3);
    }

    // �ӽ� : �������� ��� ������ �Ϸ�Ǹ�, �����ؾߵ�
    // �� ���ƿ� ���
    // ��� : �ٽ� ���ƿ��� �� ���ٰ� ��������? infoManager?
    public GameObject like, unLike;
    public TMP_Text likeCnt;
    int cnt;
    bool isLike = false;
    public void ClickLike()
    {
        cnt = InfoManager.Instance.likeCnt;

        // ���ƿ䰡 �������� �� ���ƿ並 ������
        // ���ƿ� Ȱ��ȭ, �����ƿ� ��Ȱ��ȭ
        // ���ƿ� text +1
        if (!like.activeSelf)
        {
            print("������?");
            like.SetActive(true);
            unLike.SetActive(false);
            cnt++;
            likeCnt.text = cnt.ToString();

            InfoManager.Instance.likeCnt = cnt;
            InfoManager.Instance.isLike = true;
        }
        // ���ƿ� ����� �������� �� ���ƿ並 ������
        // ���ƿ� ��Ȱ, �����ƿ� Ȱ��
        // ���ƿ� �ؽ�Ʈ -1
        else
        {
            like.SetActive(false);
            unLike.SetActive(true);
            cnt--;
            likeCnt.text = cnt.ToString();
            InfoManager.Instance.likeCnt = cnt;
            InfoManager.Instance.isLike = false;
        }
    }


}
