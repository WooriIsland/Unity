using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// ����Ű�� ������
// �ش� �������� �÷��̾ ȸ����Ű�� �ʹ�.

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
