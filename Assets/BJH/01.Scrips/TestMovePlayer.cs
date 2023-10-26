using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovePlayer : MonoBehaviour
{
    // character controller
    public CharacterController cc;

    // �̵� �ӵ�
    float speed = 5f;

    // ������
    float jump = 5f;

    // �߷�
    float gravity = -9.8f;

    // y�ӷ�
    float yVelocity = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v).normalized;

        if(cc.isGrounded == true)
        {
            yVelocity = 0f;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            yVelocity = jump;
        }

        // yVelocity�� �߷¸�ŭ ����
        yVelocity += gravity * Time.deltaTime;

        // yvelocity���� dir�� y���� ����
        dir.y = yVelocity;

        cc.Move(dir * speed * Time.deltaTime);
    }
}
