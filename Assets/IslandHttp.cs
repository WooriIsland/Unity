//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Networking;

//public class IslandHttp : MonoBehaviour
//{
//    // -------------------- �� ���� ��ȸ --------------------
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


//    // -------------------- �� ��ü ��ȸ --------------------
//    // �ӽ� : ����Ʈ �������� ������ json
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

//    // -------------------- �� ���� ��¥ ��ȸ --------------------
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


//    // -------------------- �� ���(����) --------------------
//    [System.Serializable]
//    public class RequestCreateIsland
//    {
//        public long islandUniqueNumber; // ���� �ο�
//        public string islandName; // ����� ����
//        public string island_introduce; // �� ����
//        public string islandIndex; // �� ����
//        public bool secret; // ����/����� ����
//    }




//    // �� ���� ��û
//    public void createIsland(string islandName, string islandIntroduce, string islandIdx, bool secret)
//    {
//        RequestCreateIsland requestIslandCreate = new RequestCreateIsland();

//        requestIslandCreate.islandUniqueNumber = 123456; // �ӽ�
//        InfoManager.Instance.FamilyCode = 123456; // info manager�� ����
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
//        print("�� ���� ����!");

//        // �� ������ �Ϸ�ƴٴ� UI�� ȣ��
        
//    }


//    private void FaileCreateIsland()
//    {
//        print("�� ���� ����!");
//    }


//    // ��ü �� ��ȸ
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
//        print("��ü �� ��ȸ�� �Ϸ��߽��ϴ�.");
//    }

//    public void FaileIslandListAll()
//    {
//        print("��ü �� ��ȸ�� �����߽��ϴ�.");

//    }

//}