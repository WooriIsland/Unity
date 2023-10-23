using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerJoystick : MonoBehaviour
{
    // 이동 속도
    public float speed;

    // Update is called once per frame
    void Update()
    {
        //// awds 받기
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");

        //Vector3 dir = new Vector3(h, 0, v);
        //dir.Normalize();

        //transform.position += dir * speed * Time.deltaTime;
    }

    // joystick move
    public void Move(Vector2 inputDirection)
    {
        Vector2 moveInput = inputDirection;
        bool isMove = moveInput.magnitude != 0; 
        
        if(isMove)
        {
            Vector3 dir = new Vector3(inputDirection.x, 0, inputDirection.y);
            dir.Normalize();

            transform.position += dir * speed * Time.deltaTime;
        }
    }
}
