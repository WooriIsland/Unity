using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// 방향키를 누르면
// 해당 방향으로 플레이어를 회전시키고 싶다.

public class RotPlayer : MonoBehaviour
{
    private Vector3 dir = Vector3.zero;

    private void Update()
    {
        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");

        if(dir != Vector3.zero)
        {
            transform.forward = dir;
        }
    }


}
