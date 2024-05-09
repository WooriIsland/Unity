using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 버튼 UI에 연결하여 조작하는 클래스 
public class ButtonController : MonoBehaviour
{
    public void OnClick_JoinLobby()
    {
        Managers.Connection.OnClickConnect();
    }
}
