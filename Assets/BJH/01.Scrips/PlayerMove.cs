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

    public Animator[] animator = new Animator[2]; // �ӽ� : �迭 ũ�� ��

    // cc
    //public CharacterController cc;

    public GameObject PlayerRig;
    CharacterController cc;

    // �߷� ����
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
        // �� �÷��̾� �� �ƴϸ� ���� �ʴ´�.
        if (!photonView.IsMine)
        {
            return;
        }

        // ���� �� ���� ���¶�� ���� �ʴ´�.
        if (!canMove)
        {
            return;
        }

        // Ű �Է� �� ���� ����
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(h, 0, v).normalized;

        isMoving = h != 0f || v != 0f;

        // �������� ������
        if(!isMoving)
        {
            speed = 0f;
        }
        // �����̸�
        if(isMoving)
        {
            // ���� ��
            speed = 5f;

            // �� ��
            if(Input.GetKey(KeyCode.RightShift))
            {
                speed = 10f;
            }
        }




        // �̵�
        //transform.position += dir * speed * Time.deltaTime;
        cc.Move(dir* speed *Time.deltaTime);

        // �ִϸ��̼� ����(Photon X)
        //photonView.RPC("PlayAnimation", RpcTarget.All, dir);
        animator[0].SetFloat("MoveSpeed", speed, 0.1f, Time.deltaTime);
        animator[1].SetFloat("MoveSpeed", speed, 0.1f, Time.deltaTime);

        trCam.position = player.transform.position + new Vector3(0, 1.5f, -3f);
        trCam.rotation = Quaternion.Euler(22f, 0, 0);

        // �߷� ����
        velocity.y += gravity * Time.deltaTime;

        // ���� �̵�
        cc.Move(velocity * Time.deltaTime);

        // GetTouch() : ����� ��ġ ȭ�鿡 ������ �հ����� ����
        // Touch ����ü ��ȯ
        //Debug.Log(Input.GetTouch(0).position);

        //Coroutine co;
        //GameObject obj;
        //bool isCo = false;
        //if(Input.touchCount > 0)
        //{
        //    Touch touch = Input.GetTouch(0);

        //    if(touch.phase == TouchPhase.Began)
        //    {
        //        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        //        RaycastHit hit;

        //        if(Physics.Raycast(ray, out hit))
        //        {
        //            if(hit.transform.tag == "Player")
        //            {
        //                obj = hit.transform.gameObject;
        //            } 
        //            else if(hit.transform.tag == "Floor")
        //            {
        //                if(isCo)
        //                {
        //                    isCo = false;
        //                    StopCoroutine(co);
        //                }
        //                co = StartCoroutine(CoMove(hit.point));
        //            }
        //        }
        //    }
        //}

        


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


    // Ŭ���� �÷��̾� �̵�
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
