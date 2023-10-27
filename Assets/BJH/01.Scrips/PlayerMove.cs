using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;


public class PlayerMove : MonoBehaviourPun
{
    [SerializeField]
    float speed;
    public float walkSpeed;
    public float runSpeed;
    public Transform player;
    public Transform trCam;

    public bool canMove = true;
    public bool isMoving = false;

    public Animator[] animator = new Animator[2]; // 임시 : 배열 크기 값

    // cc
    //public CharacterController cc;

    public GameObject PlayerRig;
    CharacterController cc;

    // 중력 적용
    float gravity = -9.8f;
    private Vector3 velocity;


    private void Start()
    {
        if (photonView.IsMine)
        {
            trCam.gameObject.SetActive(true);
        }

        cc = PlayerRig.GetComponent<CharacterController>();
    }

    private void Update()
    {
        speed = walkSpeed;
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

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v).normalized;

        if(Input.GetKey(KeyCode.RightShift)) 
        {
            speed = runSpeed;
            print("쉬프트 누름 : " + speed);

        } else if(Input.GetKeyUp(KeyCode.RightShift))
        {
            speed = walkSpeed;
            print("쉬프트 X : " + speed);
        }

        // 이동
        //transform.position += dir * speed * Time.deltaTime;
        cc.Move(dir* speed *Time.deltaTime);

        //photonView.RPC("PlayAnimation", RpcTarget.All, dir);

        animator[0].SetFloat("MoveSpeed", dir.magnitude, 0.1f, Time.deltaTime);
        animator[1].SetFloat("MoveSpeed", dir.magnitude, 0.1f, Time.deltaTime);

        trCam.position = player.transform.position + new Vector3(0, 1.5f, -3f);
        trCam.rotation = Quaternion.Euler(22f, 0, 0);

        // 중력 적용
        velocity.y += gravity * Time.deltaTime;

        // 수직 이동
        cc.Move(velocity * Time.deltaTime);

        // ???
        if (Input.GetKeyDown(KeyCode.A))
        {
            isMoving = true;
        }
    }

    // player animation
    //[PunRPC]
    //public void PlayAnimation(Vector3 dir)
    //{
    //    animator[0].SetFloat("MoveSpeed", dir.magnitude, 0.1f, Time.deltaTime);
    //    animator[1].SetFloat("MoveSpeed", dir.magnitude, 0.1f, Time.deltaTime);
    //}

    //NavMeshAgent agent;
    //public Transform trCam;
    //public bool canMove = true;



    // Start is called before the first frame update
    //void Start()
    //{
    //agent = GetComponent<NavMeshAgent>();

    //if(photonView.IsMine)
    //{
    //    trCam.gameObject.SetActive(true);
    //} else
    //{
    //    trCam.gameObject.SetActive(false);

    //}
    //}


    // 클릭시 플레이어 이동
    //void Update()
    //{
    //    if(!photonView.IsMine)
    //    {
    //        return;
    //    }

    //    if (!canMove)
    //    {
    //        return;
    //    }

    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        print(Input.mousePosition);
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    //        RaycastHit hit;

    //        if (Physics.Raycast(ray, out hit))
    //        {
    //            agent.SetDestination(hit.point);
    //        }
    //    }
    //    trCam.position = transform.position + new Vector3(0, 2.35f, -3.451f);
    //}
}
