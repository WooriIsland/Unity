using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;


// 방향키를 누르면
// 해당 방향으로 플레이어를 회전시키고 싶다.

public class RotPlayer : MonoBehaviourPun
{
    // 조이스틱
    public GameObject joystickCanvas;
    public FixedJoystick joystick;

    private Vector3 rot = Vector3.zero;

    private void Start()
    {

    }

    private void Update()
    {
        // 내 플레이어 가 아니면 걷지 않는다.
        if (!photonView.IsMine)
        {
            return;
        }

        if (joystickCanvas == null)
        {
            joystickCanvas = GameObject.Find("Joystick_Canvas");
            joystick = joystickCanvas.transform.GetChild(0).GetComponent<FixedJoystick>();
            print(joystick.gameObject);
        }

        rot.x = joystick.Horizontal; //Input.GetAxis("Horizontal");
        rot.z = joystick.Vertical; //Input.GetAxis("Vertical");

        transform.forward = rot;

        if (rot != Vector3.zero)
        {
        transform.forward = rot;

        }
    }


}
