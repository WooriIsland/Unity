using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using TMPro;
using System;

public class PlayerMove : MonoBehaviourPun
{
    // ���̽�ƽ
    public GameObject joystickCanvas;
    public FixedJoystick joystick;
    [SerializeField] private float moveSpeed;
    private float horizontal, vertical;

    [SerializeField] private float speed;
    [SerializeField] private float walkSpeed;
    
    [SerializeField] private float runSpeed;

    // ī�޶� ��ġ ����
    public Transform player;
    public Transform camera;
    public Vector3 offSet;
    public float rotationX, rotationY, rotationZ;
    public bool canMove = true;
    public bool isMoving = false;

    public Animator[] animator;

    public GameObject PlayerRig;
    public CharacterController cc;

    // �߷�
    float gravity = -9.8f;





    // �λ� �ִϸ��̼�
    public Camera aniCam;
    int aniTemp;





    private void Start()
    {
        // �� �÷��̾� �϶��� ī�޶� �Ҵ�.
        if (photonView.IsMine)
        {
            camera.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        // �� �÷��̾� �� �ƴϸ� ���� �ʴ´�.
        if (!photonView.IsMine)
        {
            return;
        }

        if (joystickCanvas == null)
        {
            joystickCanvas = GameObject.Find("Joystick_Canvas");
            joystick = joystickCanvas.transform.GetChild(0).GetComponent<FixedJoystick>();
            print(joystick.gameObject);

        }

        // ���� �� ���� ���¶�� ���� �ʴ´�.
        if (!canMove)
        {
            return;
        }

        // �÷��̾� �ӵ� ����
        if (!isMoving)
        {
            speed = 0f;
        }

        if (isMoving)
        {

            speed = 5f;


            if (Input.GetKey(KeyCode.RightShift))
            {
                speed = 10f;
            }
        }

        horizontal = joystick.Horizontal;
        vertical = joystick.Vertical;

        Vector3 dir = new Vector3(horizontal, 0, vertical).normalized;

        isMoving = horizontal != 0f || vertical != 0f;

        // �߷� ����
        dir.y += gravity * speed * Time.deltaTime;

        // �̵�
        cc.Move(dir * speed * Time.deltaTime);



        // �ִϸ��̼� ����
        for (int i = 0; i < animator.Length; i++)
        {
            if (animator[i].gameObject.activeSelf == false)
            {
                continue;
            }
            animator[i].SetFloat("MoveSpeed", speed);
        }

        // ī�޶�
        //camera.position = player.transform.position + offSet;
        //camera.rotation = Quaternion.Euler(rotationX, rotationY, rotationZ);


        

        // �ٸ� �÷��̾ Ŭ���ϸ�?
        // �λ��ϱ�
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = aniCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit))
            {
                print("Ŭ�� ����2");

                if (hit.transform.gameObject.CompareTag("Player"))
                {
                    print("Ŭ�� ����3");

                    // �λ��ϱ�
                    for (int i = 0; i < animator.Length; i++)
                    {
                        print("Ŭ�� ����4");

                        if (animator[i].gameObject.activeSelf == false)
                        {
                            continue;
                        }
                        print("Ʈ���� ���� ��");
                        animator[i].SetTrigger("Hello");
                        aniTemp = i;

                        Invoke("FalseAnimationTrigger", 3);
                        
                    }
                }
            }



        }
    }



    // �ִϸ��̼� Ʈ���� 3�� �ڿ� ���ֱ�
    void FalseAnimationTrigger()
    {
        animator[aniTemp].ResetTrigger("Hello");

    }


    public void IfPc()
    {
        // Ű �Է� �� ���� ����
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v).normalized;

        // �����̴� ���� �Ǻ�
        isMoving = h != 0f || v != 0f;


        // �������� ������
        if (!isMoving)
        {
            speed = 0f;
        }
        // �����̸�
        if (isMoving)
        {
            // ���� ��
            speed = 5f;

            // �� ��
            if (Input.GetKey(KeyCode.RightShift))
            {
                speed = 10f;
            }
        }



        // �̵�
        cc.Move(dir * speed * Time.deltaTime);

        // �ִϸ��̼� ����(Photon X)
        for (int i = 0; i < animator.Length; i++)
        {
            if (animator[i].gameObject.activeSelf == false)
            {
                continue;
            }
            animator[i].SetFloat("MoveSpeed", speed);
        }


        // ī�޶� ��ġ
        //camera.position = player.transform.position + offSet;
        //camera.rotation = Quaternion.Euler(rotationX, rotationY, rotationZ);

        // �߷� ����
        //velocity.y += gravity * Time.deltaTime;

        // ���� �̵�
        //cc.Move(velocity * Time.deltaTime);
    }



}
