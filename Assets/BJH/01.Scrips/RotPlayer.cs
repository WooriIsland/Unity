using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;


// ����Ű�� ������
// �ش� �������� �÷��̾ ȸ����Ű�� �ʹ�.

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

        // �� �÷��̾� �� �ƴϸ� ���� �ʴ´�.
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
