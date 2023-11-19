using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;


// ����Ű�� ������
// �ش� �������� �÷��̾ ȸ����Ű�� �ʹ�.

public class RotPlayer : MonoBehaviourPun
{
    // ���̽�ƽ
    public GameObject joystickCanvas;
    public FixedJoystick joystick;

    private Vector3 rot = Vector3.zero;

    private void Start()
    {

    }

    private void Update()
    {
        // �� �÷��̾� �� �ƴϸ� ���� �ʴ´�.
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
