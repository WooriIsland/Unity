using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;


public class MovePlayer : MonoBehaviourPun
{
    public float speed;
    public Transform trCam;

    public bool canMove = true;
    public bool isMoving = false;

    public Animator[] animator = new Animator[2]; // 임시 : 배열 크기 값

    // cc
    public CharacterController cc;

    // jump
    public float jumpPower; // 5


    private void Start()
    {
        if (photonView.IsMine)
        {
            trCam.gameObject.SetActive(true);
        }

         
    }

    private void Update()
    {
        // 내 플레이어 가 아니면 걷지 않는다.
        if(!photonView.IsMine)
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

        // 이동
        //transform.position += dir * speed * Time.deltaTime;
        cc.Move(dir* speed *Time.deltaTime);


        animator[0].SetFloat("MoveSpeed", dir.magnitude, 0.1f, Time.deltaTime);
        
        trCam.position = transform.position + new Vector3(0, 1.5f, -3f);


        // ???
        if (Input.GetKeyDown(KeyCode.A))
        {
            isMoving = true;
        }
    }




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
