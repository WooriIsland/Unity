using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatbotHttp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // �ӽ�
        // 5�� ������ ê������ get��û�� ������.
        HttpManager02.GetInstance().SendRequest();
    }
}
