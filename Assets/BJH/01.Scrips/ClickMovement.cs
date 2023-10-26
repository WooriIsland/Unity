using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMovement : MonoBehaviour
{
    // Ŭ���� ���� ���콺 ��ġ ��������
    // ��ũ�� ��ǥ�� ���콺 ��ġ -> ���� ��ǥ ��ġ�� �����ϱ� ���ؼ� Camera ��������
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

    // ĳ���͸� ���������� �̵���Ű�� �Լ�
    private void Move()
    {
        if(isMoving)
        {
            var dir = destination - transform.position; // ���� = ������ - ���� ��ġ
            // nomalized : Ŭ���� ���콺 ��ġ, ĳ���� ��ġ�� �ּ��� �̵� �ӵ��� �������� ������ �ذ�
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
