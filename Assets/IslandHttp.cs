//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Networking;

//public class IslandHttp : MonoBehaviour
//{
//    // -------------------- 섬 단일 조회 --------------------
//    [System.Serializable]
//    public class RequestIsland
//    {
//        public string islandId;
//    }

//    [System.Serializable]
//    public class ResponseIsland
//    {
//        public string createdAt;
//        public string lastModifiedAt;
//        public long islandId;
//        public string islandUniqueNumber;
//        public string islandName;
//        public string island_introduce;
//        public string islandIndex;
//        public int daysSinceCreation;
//        public bool secret;
//    }


//    // -------------------- 섬 전체 조회 --------------------
//    // 임시 : 리스트 형식으로 감싸진 json
//    [System.Serializable]
//    public class ResponseIslandAll
//    {
//        public string createdAt;
//        public string lastModifiedAt;
//        public long islandId;
//        public string islandUniqueNumber;
//        public string islandName;
//        public string island_introduce;
//        public string islandIndex;
//        public int daysSinceCreation;
//        public bool secret;
//    }

//    // -------------------- 섬 생성 날짜 조회 --------------------
//    [System.Serializable]
//    public class RequestIslandCreatDay
//    {
//        public long islandId;
//    }

//    [System.Serializable]
//    public class ResponseIslandCreatDay
//    {
//        public int days;
//    }


//    // -------------------- 섬 등록(생성) --------------------
//    [System.Serializable]
//    public class RequestCreateIsland
//    {
//        public long islandUniqueNumber; // 랜덤 부여
//        public string islandName; // 사용자 지정
//        public string island_introduce; // 섬 설명
//        public string islandIndex; // 섬 유형
//        public bool secret; // 공개/비공개 여부
//    }




//    // 섬 생성 요청
//    public void createIsland(string islandName, string islandIntroduce, string islandIdx, bool secret)
//    {
//        RequestCreateIsland requestIslandCreate = new RequestCreateIsland();

//        requestIslandCreate.islandUniqueNumber = 123456; // 임시
//        InfoManager.Instance.FamilyCode = 123456; // info manager에 저장
//        requestIslandCreate.islandName = islandName;
//        requestIslandCreate.island_introduce = islandIntroduce;
//        requestIslandCreate.islandIndex = islandIdx;
//        requestIslandCreate.secret = secret;

//        string jsonData = JsonUtility.ToJson(requestIslandCreate, true);

//        string url = "http://121.165.108.236:7070/api/islands";

//        HttpRequester_LHS requester = new HttpRequester_LHS();

//        requester.SetUrl(RequestType.POST, url, false);
//        requester.body = jsonData;
//        requester.isJson = true;
//        requester.isChat = false;

//        requester.onComplete = CompleteCreateIsland;
//        requester.onFailed = FaileCreateIsland;

//        HttpManager_LHS.instance.SendRequest(requester);
//    }

//    private void CompleteCreateIsland(DownloadHandler result)
//    {
//        print("섬 생성 성공!");

//        // 섬 생성이 완료됐다는 UI를 호출
        
//    }


//    private void FaileCreateIsland()
//    {
//        print("섬 생성 실패!");
//    }


//    // 전체 섬 조회
//    public void RequestIslandListAll()
//    {
//        string url = "http://121.165.108.236:7070/api/islands";

//        HttpRequester_LHS requester = new HttpRequester_LHS();

//        requester.SetUrl(RequestType.GET, url, false);
//        requester.onComplete = CompleteIslandListAll;
//        requester.onFailed = FaileIslandListAll;

//        HttpManager_LHS.instance.SendRequest(requester);
//    }

//    public void CompleteIslandListAll(DownloadHandler result)
//    {
//        print("전체 섬 조회를 완료했습니다.");
//    }

//    public void FaileIslandListAll()
//    {
//        print("전체 섬 조회가 실패했습니다.");

//    }

//}