using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerKeyboard : MonoBehaviour
{
    public float speed;

    CharacterController cc;

    public float jumpPower;
    float gravity = -9.81f;
    float yVelocity = 0f;
    bool isJimping;
    

    private void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir.Normalize();

        // �÷��̾ ���� ������
        // yVelocity�� 0���� �ʱ�ȭ
        if(cc.isGrounded)
        {
            yVelocity = 0f;
            isJimping = false;
        }


        // jump
        // �����̽��ٸ� ������ �������� �ƴϸ�? ���� ����
        if (Input.GetKeyDown(KeyCode.Space) && isJimping==false)
        {
            yVelocity = jumpPower;
            isJimping = true;
        }

        yVelocity += gravity * Time.deltaTime;

        dir.y = yVelocity;

        //transform.position += dir * speed * Time.deltaTime;
        cc.Move(dir * speed * Time.deltaTime);
    }
}
