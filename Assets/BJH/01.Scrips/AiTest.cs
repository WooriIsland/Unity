using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AiTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickGet()
    {
        HttpInfo info = new HttpInfo();
        info.Set(HttpMethod.GET, "/conversation", OnReceiveGet);

        // info�� ������ ��û�� ������
        HttpManager02.GetInstance().SendRequest(info);
    }

    void OnReceiveGet(DownloadHandler download)
    {

    }

    public void PostTest()
    {
        HttpInfo info = new HttpInfo();
        info.Set(HttpMethod.POST, "/�ӽ�", (DownloadHandler DownloadHandler) => { 
                // post �����͸� �����ϸ� �����κ��� ������ ��
        });

        ChatInfo chatInfo = new ChatInfo();

        chatInfo.island_id = "ji09hwan123";
        chatInfo.user_id = "jihwan98";
        chatInfo.content = "��ȯ�� ���";
        chatInfo.datetiem = "��¥�� �����µ� ������ Ÿ���� ����";


        info.body = JsonUtility.ToJson(chatInfo);

        HttpManager02.GetInstance().SendRequest(info);

    }
}
