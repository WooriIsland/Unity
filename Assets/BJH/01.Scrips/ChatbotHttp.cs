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
        // 임시
        // 5를 누르면 챗봇에게 get요청을 보낸다.
        HttpManager02.GetInstance().SendRequest();
    }
}
