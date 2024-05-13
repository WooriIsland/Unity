using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateIslandManager : MonoBehaviour
{
    public GameObject createIsland,frame, islandSelect, islandCustom, islandCode, visitIslandError, createIslandErrorFrame, visitRoomErrorFrame;
    public BaseAlpha back, createIslandErrorBG, visitRoomErrorBG;
    string code;

    public GameObject createIslandError;

    // �� ����
    public GameObject myItem, christmasItem;

    private void Start()
    {
        // ���� ����� ���ƿ並 InfoManager�� ����
        myItem.GetComponent<CreatedRoomInfo>().likeCnt.text = Managers.Info.MyIslandLike;
        myItem.GetComponent<CreatedRoomInfo>().Unlike.SetActive(Managers.Info.isMyIslandLike);

        christmasItem.GetComponent<CreatedRoomInfo>().likeCnt.text = Managers.Info.ChristmasIslandLike;
        christmasItem.GetComponent<CreatedRoomInfo>().Unlike.SetActive(Managers.Info.isChristmasIslandLike);




        //createIsland.SetActive(false);

        //// ���ƿ� infoManager���� �ҷ�����
        //if (InfoManager.Instance.likeCnt.ToString().Length > 0)
        //{
        //    likeCnt.text = InfoManager.Instance.likeCnt.ToString();
        //}

        //like.SetActive(InfoManager.Instance.isLike);
    }

    // ������Ʈ ���� : X ��ư
    public void CloseObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
    
    // �� ���� : �� ������ư Ŭ��
    public void OnClick_CreateIslandBtn()
    {
        //�˾�������� ���;��� �����߰�
        createIsland.SetActive(true);
        frame.GetComponent<PopupPhotoED>().OpenAction();
        back.OpenAlpha();
    }

    // �� �����ϱ� ��ư�� Ŭ���ϸ�
    // ������ ���� ������ �� ���ٴ� �˾��� �����ش�.
    public void Onclick_CreateIslandError()
    {
        SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.Alert);
        createIslandError.SetActive(true);
        createIslandErrorFrame.GetComponent<PopupPhotoED>().OpenAction();
        createIslandErrorBG.OpenAlpha();
    }

    // ���� ������Ʈ�� ���� �޼���
    public void Onclick_Close(GameObject go)
    {
        go.SetActive(false);
    }


    // �� Ÿ�� ����
    public void OnClick_SelectIsland(string islandType)
    {
        Managers.Info.IslandType = islandType;
    }

    // �� Ÿ�� ���� �� �� Ŀ���� â���� �̵�
    public void Onclick_GoIslandCustom()
    {
        createIslandError.SetActive(true);
        SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.Alert);

        //islandSelect.SetActive(false);
        //islandCustom.SetActive(true);
    }

    public void OnClick_CreateIslandErrorBackBtn()
    {
        createIslandError.SetActive(false);
        createIsland.SetActive(false);
    }


    // �� �̸�, �� �Ұ�, ����/����� ����
    public void CustomIsland()
    {
        var info = islandCustom.GetComponent<CreateIslandInfo>();
        Managers.Info.IslandName = info.islandName.text;
        Managers.Info.IslandIntroduce = info.introduce.text;
        Managers.Info.Secret = !info.secret;
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
        Managers.Info.FamilyCode = code;

        // ĳ���� ���� â���� �̵�
        SceneManager.LoadScene(3);
    }


    // ������ ���� Ŭ���ϸ�, ����� ������ �ڵ带 �ӽ÷� �����صα�
    public void WantVisitThisIsland(GameObject go)
    {
        //HttpManager_LHS.instance.mainLoding.GetComponent<AlphaGPSSet>().OpenAlpha();

        // �湮 �ϰ���� ���� Ŭ���ϸ�? �ش��ϴ� ���� �� �̸�, �� ����
        // �ӽ� : �� ID ����
        Managers.Info.visit = go.GetComponent<CreatedRoomInfo>().roomName.text;
        Debug.Log(Managers.Info.visit + "    " + go.GetComponent<CreatedRoomInfo>().roomName.text);

        // create room info ���ӿ�����Ʈ ��ġ �ľ� ������
        // switch case�� ��ü
        // ���� �ڵ� : Managers.Info.visitType = go.GetComponent<CreatedRoomInfo>().islandType.name;
        string name = go.GetComponent<CreatedRoomInfo>().islandType.name;
        switch(name)
        {
            // 01 ũ��������
            case "Island01":
                Managers.Info.visitType = Define.IslandType.CHRISTMAS;
                break;
            case "Island02":
                Managers.Info.visitType = Define.IslandType.BASIC;
                break;
        }
        Managers.Info.islandId = go.GetComponent<CreatedRoomInfo>().islandId; // �ӽ� : �� ID ����

        SceneManager.LoadScene(3);
    }


    // �� �� ���� ���� Ŭ������ ��
    public void OnClick_VisitRoomError()
    {
        //SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.Alert);
        visitIslandError.SetActive(true);

        visitRoomErrorFrame.GetComponent<PopupPhotoED>().OpenAction();
        visitRoomErrorBG.OpenAlpha();

    }

    // Alert
    public void OnClick_LikeBtnError()
    {
        SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.Alert);
    }








    // �ӽ� : �������� ��� ������ �Ϸ�Ǹ�, �����ؾߵ�
    // �� ���ƿ� ���
    // ��� : �ٽ� ���ƿ��� �� ���ٰ� ��������? infoManager?
    public void ClickLike(GameObject go)
    {
        CreatedRoomInfo roomInfo = go.GetComponent<CreatedRoomInfo>();
        string roomName = roomInfo.roomName.text;
        bool isUnlike = roomInfo.Unlike.activeSelf;

        if(roomName == "���� & ����")
        {
            // ���ƿ䰡 �������� ���� ���¸�?
            // ���ƿ並 ������
            if(isUnlike == true)
            {
                roomInfo.Unlike.SetActive(false); // ���ƿ�
                int cnt = int.Parse(roomInfo.likeCnt.text);
                cnt++;
                Managers.Info.MyIslandLike = cnt.ToString();
                Managers.Info.isMyIslandLike = false;
                roomInfo.likeCnt.text = Managers.Info.MyIslandLike;

            }
            else
            {
                // ���ƿ� ���
                roomInfo.Unlike.SetActive(true); // ���ƿ� ���
                int cnt = int.Parse(roomInfo.likeCnt.text);
                cnt--;
                Managers.Info.MyIslandLike = cnt.ToString();
                Managers.Info.isMyIslandLike = true;
                roomInfo.likeCnt.text = Managers.Info.MyIslandLike;
            }
        }
        
        if(roomName == "ũ�������� ��")
        {
            // ���ƿ䰡 �������� ���� ���¸�?
            // ���ƿ並 ������
            if (isUnlike == true)
            {
                roomInfo.Unlike.SetActive(false); // ���ƿ�
                int cnt = int.Parse(roomInfo.likeCnt.text);
                cnt++;
                Managers.Info.ChristmasIslandLike = cnt.ToString();
                Managers.Info.isChristmasIslandLike = false;
                roomInfo.likeCnt.text = Managers.Info.ChristmasIslandLike;
            }
            else
            {
                // ���ƿ� ���
                roomInfo.Unlike.SetActive(true); // ���ƿ� ���
                int cnt = int.Parse(roomInfo.likeCnt.text);
                cnt--;
                Managers.Info.ChristmasIslandLike = cnt.ToString();
                Managers.Info.isChristmasIslandLike = true;
                roomInfo.likeCnt.text = Managers.Info.ChristmasIslandLike;
            }
        }
        
    }


}
