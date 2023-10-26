using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovePlayer : MonoBehaviour
{
    // character controller
    public CharacterController cc;

    // 이동 속도
    float speed = 5f;

    // 점프값
    float jump = 5f;

    // 중력
    float gravity = -9.8f;

    // y속력
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

        // yVelocity를 중력만큼 감소
        yVelocity += gravity * Time.deltaTime;

        // yvelocity값을 dir의 y값에 셋팅
        dir.y = yVelocity;

        cc.Move(dir * speed * Time.deltaTime);
    }
}
