using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using TMPro;


public class PlayerMove : MonoBehaviourPun
{
    [SerializeField]
    float speed;
    public float walkSpeed;
    public float runSpeed;
    public Transform player;
    public Transform trCam;

    public Vector3 offSet;
    public float rotationX;

    public bool canMove = true;
    public bool isMoving = false;

    public Animator[] animator;

    public GameObject PlayerRig;
    CharacterController cc;

    // 중력
    float gravity = -9.8f;
    private Vector3 velocity;

    // 터치 이동
    private Vector3 touchStartPosition;
    private Vector3 touchEndPosition;

    private void Start()
    {
        // 내 플레이어 일때만 카메라를 켠다.
        if (photonView.IsMine)
        {
            trCam.gameObject.SetActive(true);
        }

        cc = PlayerRig.GetComponent<CharacterController>();
    }

    private void Update()
    {
        // 내 플레이어 가 아니면 걷지 않는다.
        if (!photonView.IsMine)
        {
            return;
        }

        // 걸을 수 없는 상태라면 걷지 않는다.
        if (!canMove)
        {
            return;
        }

#if PC
        // 키 입력 및 방향 설정
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(h, 0, v).normalized;

        isMoving = h != 0f || v != 0f;

        // 움직이지 않으면
        if(!isMoving)
        {
            speed = 0f;
        }
        // 움직이면
        if(isMoving)
        {
            // 걸을 때
            speed = 5f;

            // 뛸 때
            if(Input.GetKey(KeyCode.RightShift))
            {
                speed = 10f;
            }
        }

        // 이동
        //transform.position += dir * speed * Time.deltaTime;
        cc.Move(dir* speed *Time.deltaTime);

        // 애니메이션 적용(Photon X)
        for (int i = 0; i < animator.Length; i++) 
        {
            if(animator[i].gameObject.activeSelf == false)
            {
                continue;
            }
            animator[i].SetFloat("MoveSpeed", speed);
        }



        trCam.position = player.transform.position + offSet;
        trCam.rotation = Quaternion.Euler(rotationX, 0, 0);

        // 중력 적용
        velocity.y += gravity * Time.deltaTime;

        // 수직 이동
        cc.Move(velocity * Time.deltaTime);
#endif

        // 터치 이동
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                touchStartPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                touchEndPosition = touch.position;
            }
        }
    }

    

}
