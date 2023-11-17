using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;


// 방향키를 누르면
// 해당 방향으로 플레이어를 회전시키고 싶다.

public class RotPlayer : MonoBehaviourPun
{
    [SerializeField] FixedJoystick joystick;

    private Vector3 rot = Vector3.zero;

    private GameObject go;

    private void Start()
    {

    }

    private void Update()
    {
        if(go == null)
        {
            go = GameObject.FindGameObjectWithTag("Joystick");

            joystick = go.GetComponent<FixedJoystick>();
        }

        // 내 플레이어 가 아니면 걷지 않는다.
        if (!photonView.IsMine)
        {
            return;
        }

        rot.x = joystick.Horizontal; //Input.GetAxis("Horizontal");
        rot.z = joystick.Vertical; //Input.GetAxis("Vertical");

        print(joystick.Horizontal);

        transform.forward = rot;

        if (rot != Vector3.zero)
        {
        transform.forward = rot;

        }
    }


}
