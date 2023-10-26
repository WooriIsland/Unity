using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;


// ����Ű�� ������
// �ش� �������� �÷��̾ ȸ����Ű�� �ʹ�.

public class RotPlayer : MonoBehaviourPun
{
    private Vector3 dir = Vector3.zero;

    private void Update()
    {
        // �� �÷��̾� �� �ƴϸ� ���� �ʴ´�.
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
