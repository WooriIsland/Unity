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

        // 플레이어가 땅에 닿으면
        // yVelocity를 0으로 초기화
        if(cc.isGrounded)
        {
            yVelocity = 0f;
            isJimping = false;
        }


        // jump
        // 스페이스바를 누르고 점프중이 아니면? 점프 가능
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
