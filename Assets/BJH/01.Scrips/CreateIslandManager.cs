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

        // 좋아요 infoManager에서 불러오기
        if(InfoManager.Instance.likeCnt.ToString().Length > 0)
        {
            likeCnt.text = InfoManager.Instance.likeCnt.ToString();
        }
        
        like.SetActive(InfoManager.Instance.isLike);
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
        //HttpManager_LHS.instance.mainLoding.GetComponent<AlphaGPSSet>().OpenAlpha();
        
        // 방문 하고싶은 섬을 클릭하면? 해당하는 섬의 섬 이름, 섬 유형 저장
        InfoManager.Instance.visit = go.GetComponent<CreatedRoomInfo>().islandName.text;
        InfoManager.Instance.visitType = go.GetComponent<CreatedRoomInfo>().islandType.name;

        SceneManager.LoadScene(3);
    }

    // 임시 : 서버에서 기능 구현이 완료되면, 연결해야됨
    // 섬 좋아요 기능
    // 고민 : 다시 돌아왔을 때 어디다가 저장하지? infoManager?
    public GameObject like, unLike;
    public TMP_Text likeCnt;
    int cnt;
    bool isLike = false;
    public void ClickLike()
    {
        cnt = InfoManager.Instance.likeCnt;

        // 좋아요가 꺼져있을 때 좋아요를 누르면
        // 좋아요 활성화, 안좋아요 비활성화
        // 좋아요 text +1
        if (!like.activeSelf)
        {
            print("들어오나?");
            like.SetActive(true);
            unLike.SetActive(false);
            cnt++;
            likeCnt.text = cnt.ToString();

            InfoManager.Instance.likeCnt = cnt;
            InfoManager.Instance.isLike = true;
        }
        // 좋아요 기능이 켜져있을 때 좋아요를 누르면
        // 좋아요 비활, 안좋아요 활성
        // 좋아요 텍스트 -1
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
