using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��ư UI�� �����Ͽ� �����ϴ� Ŭ���� 
public class ButtonController : MonoBehaviour
{
    public void OnClick_JoinLobby()
    {
        Managers.Connection.OnClickConnect();
    }
}
