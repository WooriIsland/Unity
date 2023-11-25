using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinRoom : MonoBehaviour
{
    public void Onclick_JoinRoom()
    {
        LobbyManager.Instance.CreateOrJoinRoom();
    }
}
