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

    // 방 정보
    public GameObject myItem, christmasItem;

    private void Start()
    {
        // 최초 입장시 좋아요를 InfoManager에 저장
        myItem.GetComponent<CreatedRoomInfo>().likeCnt.text = InfoManager.Instance.MyIslandLike;
        myItem.GetComponent<CreatedRoomInfo>().Unlike.SetActive(InfoManager.Instance.isMyIslandLike);

        christmasItem.GetComponent<CreatedRoomInfo>().likeCnt.text = InfoManager.Instance.ChristmasIslandLike;
        christmasItem.GetComponent<CreatedRoomInfo>().Unlike.SetActive(InfoManager.Instance.isChristmasIslandLike);




        //createIsland.SetActive(false);

        //// 좋아요 infoManager에서 불러오기
        //if (InfoManager.Instance.likeCnt.ToString().Length > 0)
        //{
        //    likeCnt.text = InfoManager.Instance.likeCnt.ToString();
        //}

        //like.SetActive(InfoManager.Instance.isLike);
    }

    // 오브젝트 끄기 : X 버튼
    public void CloseObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
    
    // 섬 생성 : 섬 생성버튼 클릭
    public void OnClick_CreateIslandBtn()
    {
        //팝업모션으로 나와야함 현숙추가
        createIsland.SetActive(true);
        frame.GetComponent<PopupPhotoED>().OpenAction();
        back.OpenAlpha();
    }

    // 섬 생성하기 버튼을 클릭하면
    // 지금은 섬을 생성할 수 없다는 팝업을 보여준다.
    public void Onclick_CreateIslandError()
    {
        SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.Alert);
        createIslandError.SetActive(true);
        createIslandErrorFrame.GetComponent<PopupPhotoED>().OpenAction();
        createIslandErrorBG.OpenAlpha();
    }

    // 게임 오브젝트를 끄는 메서드
    public void Onclick_Close(GameObject go)
    {
        go.SetActive(false);
    }


    // 섬 타입 선택
    public void OnClick_SelectIsland(string islandType)
    {
        InfoManager.Instance.IslandType = islandType;
    }

    // 섬 타입 선택 후 섬 커스텀 창으로 이동
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
        //HttpManager_LHS.instance.mainLoding.GetComponent<AlphaGPSSet>().OpenAlpha();

        // 방문 하고싶은 섬을 클릭하면? 해당하는 섬의 섬 이름, 섬 유형 저장
        InfoManager.Instance.visit = go.GetComponent<CreatedRoomInfo>().roomName.text;
        InfoManager.Instance.visitType = go.GetComponent<CreatedRoomInfo>().islandType.name;

        SceneManager.LoadScene(3);
    }


    // 들어갈 수 없는 방을 클릭했을 때
    public void OnClick_VisitRoomError()
    {
        SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.Alert);
        visitIslandError.SetActive(true);

        visitRoomErrorFrame.GetComponent<PopupPhotoED>().OpenAction();
        visitRoomErrorBG.OpenAlpha();

    }








    // 임시 : 서버에서 기능 구현이 완료되면, 연결해야됨
    // 섬 좋아요 기능
    // 고민 : 다시 돌아왔을 때 어디다가 저장하지? infoManager?
    public void ClickLike(GameObject go)
    {
        CreatedRoomInfo roomInfo = go.GetComponent<CreatedRoomInfo>();
        string roomName = roomInfo.roomName.text;
        bool isUnlike = roomInfo.Unlike.activeSelf;

        if(roomName == "정이 & 혜리")
        {
            // 좋아요가 눌려지지 않은 상태면?
            // 좋아요를 누르자
            if(isUnlike == true)
            {
                roomInfo.Unlike.SetActive(false); // 좋아요
                int cnt = int.Parse(roomInfo.likeCnt.text);
                cnt++;
                InfoManager.Instance.MyIslandLike = cnt.ToString();
                InfoManager.Instance.isMyIslandLike = false;
                roomInfo.likeCnt.text = InfoManager.Instance.MyIslandLike;

            }
            else
            {
                // 좋아요 취소
                roomInfo.Unlike.SetActive(true); // 좋아요 취소
                int cnt = int.Parse(roomInfo.likeCnt.text);
                cnt--;
                InfoManager.Instance.MyIslandLike = cnt.ToString();
                InfoManager.Instance.isMyIslandLike = true;
                roomInfo.likeCnt.text = InfoManager.Instance.MyIslandLike;
            }
        }
        
        if(roomName == "크리스마스 섬")
        {
            // 좋아요가 눌려지지 않은 상태면?
            // 좋아요를 누르자
            if (isUnlike == true)
            {
                roomInfo.Unlike.SetActive(false); // 좋아요
                int cnt = int.Parse(roomInfo.likeCnt.text);
                cnt++;
                InfoManager.Instance.ChristmasIslandLike = cnt.ToString();
                InfoManager.Instance.isChristmasIslandLike = false;
                roomInfo.likeCnt.text = InfoManager.Instance.ChristmasIslandLike;
            }
            else
            {
                // 좋아요 취소
                roomInfo.Unlike.SetActive(true); // 좋아요 취소
                int cnt = int.Parse(roomInfo.likeCnt.text);
                cnt--;
                InfoManager.Instance.ChristmasIslandLike = cnt.ToString();
                InfoManager.Instance.isChristmasIslandLike = true;
                roomInfo.likeCnt.text = InfoManager.Instance.ChristmasIslandLike;
            }
        }
        
    }


}
