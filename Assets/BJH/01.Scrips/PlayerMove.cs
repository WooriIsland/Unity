using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using TMPro;
using System;

public class PlayerMove : MonoBehaviourPun
{
    // 조이스틱
    public GameObject joystickCanvas;
    public FixedJoystick joystick;
    [SerializeField] private float moveSpeed;
    private float horizontal, vertical;

    [SerializeField] private float speed;
    [SerializeField] private float walkSpeed;
    
    [SerializeField] private float runSpeed;

    // 카메라 위치 설정
    public Transform player;
    public Transform camera;
    public Vector3 offSet;
    public float rotationX, rotationY, rotationZ;
    public bool canMove = true;
    public bool isMoving = false;

    public Animator[] animator;

    public GameObject PlayerRig;
    public CharacterController cc;

    // 중력
    float gravity = -9.8f;





    // 인사 애니메이션
    public Camera aniCam;
    int aniTemp;





    private void Start()
    {
        // 내 플레이어 일때만 카메라를 켠다.
        if (photonView.IsMine)
        {
            camera.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        // 내 플레이어 가 아니면 걷지 않는다.
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

        // 걸을 수 없는 상태라면 걷지 않는다.
        if (!canMove)
        {
            return;
        }

        // 플레이어 속도 설정
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

        // 중력 적용
        dir.y += gravity * speed * Time.deltaTime;

        // 이동
        cc.Move(dir * speed * Time.deltaTime);



        // 애니메이션 적용
        for (int i = 0; i < animator.Length; i++)
        {
            if (animator[i].gameObject.activeSelf == false)
            {
                continue;
            }
            animator[i].SetFloat("MoveSpeed", speed);
        }

        // 카메라
        //camera.position = player.transform.position + offSet;
        //camera.rotation = Quaternion.Euler(rotationX, rotationY, rotationZ);


        

        // 다른 플레이어를 클릭하면?
        // 인사하기
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = aniCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit))
            {
                print("클릭 인지2");

                if (hit.transform.gameObject.CompareTag("Player"))
                {
                    print("클릭 인지3");

                    // 인사하기
                    for (int i = 0; i < animator.Length; i++)
                    {
                        print("클릭 인지4");

                        if (animator[i].gameObject.activeSelf == false)
                        {
                            continue;
                        }
                        print("트리거 진입 완");
                        animator[i].SetTrigger("Hello");
                        aniTemp = i;

                        Invoke("FalseAnimationTrigger", 3);
                        
                    }
                }
            }



        }
    }



    // 애니메이션 트리거 3초 뒤에 꺼주기
    void FalseAnimationTrigger()
    {
        animator[aniTemp].ResetTrigger("Hello");

    }


    public void IfPc()
    {
        // 키 입력 및 방향 설정
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v).normalized;

        // 움직이는 상태 판별
        isMoving = h != 0f || v != 0f;


        // 움직이지 않으면
        if (!isMoving)
        {
            speed = 0f;
        }
        // 움직이면
        if (isMoving)
        {
            // 걸을 때
            speed = 5f;

            // 뛸 때
            if (Input.GetKey(KeyCode.RightShift))
            {
                speed = 10f;
            }
        }



        // 이동
        cc.Move(dir * speed * Time.deltaTime);

        // 애니메이션 적용(Photon X)
        for (int i = 0; i < animator.Length; i++)
        {
            if (animator[i].gameObject.activeSelf == false)
            {
                continue;
            }
            animator[i].SetFloat("MoveSpeed", speed);
        }


        // 카메라 위치
        //camera.position = player.transform.position + offSet;
        //camera.rotation = Quaternion.Euler(rotationX, rotationY, rotationZ);

        // 중력 적용
        //velocity.y += gravity * Time.deltaTime;

        // 수직 이동
        //cc.Move(velocity * Time.deltaTime);
    }



}
