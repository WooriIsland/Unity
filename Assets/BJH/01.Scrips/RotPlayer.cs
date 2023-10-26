using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;


// 방향키를 누르면
// 해당 방향으로 플레이어를 회전시키고 싶다.

public class RotPlayer : MonoBehaviourPun
{
    private Vector3 dir = Vector3.zero;

    private void Update()
    {
        // 내 플레이어 가 아니면 걷지 않는다.
        if (!photonView.IsMine)
        {
            return;
        }

        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");

        if(dir != Vector3.zero)
        {
            transform.forward = dir;
        }
    }


}
