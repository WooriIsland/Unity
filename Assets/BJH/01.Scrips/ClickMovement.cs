using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMovement : MonoBehaviour
{
    // 클릭한 순간 마우스 위치 가져오기
    // 스크린 좌표상 마우스 위치 -> 월드 좌표 위치로 변경하기 위해서 Camera 가져오기
    public Camera camera;

    public Animator animator;

    public CharacterController cc;

    public float speed; // 5f
    bool isMoving;
    private Vector3 destination;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(1))
        {
            RaycastHit hit;
            if(Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                SetDestination(hit.point);
            }
        }

        Move();
    }

    private void SetDestination(Vector3 dest)
    {
        destination = dest;
        isMoving = true;
        animator.SetBool("IsMoving", true);
    }

    // 캐릭터를 목적지까지 이동시키는 함수
    private void Move()
    {
        if(isMoving)
        {
            var dir = destination - transform.position; // 방향 = 목적지 - 현재 위치
            // nomalized : 클릭한 마우스 위치, 캐릭터 위치가 멀수록 이동 속도가 빨라지는 문제를 해결
            //transform.position += dir.normalized * Time.deltaTime * speed;
            cc.Move(dir.normalized * Time.deltaTime * speed);

        }

        if (Vector3.Distance(transform.position, destination) <= 0.1f)
        {
            isMoving = false;
            animator.SetBool("IsMoving", false);
        }
    }

}
