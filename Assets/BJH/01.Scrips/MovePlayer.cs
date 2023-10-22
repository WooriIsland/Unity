using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class MovePlayer : MonoBehaviourPun
{
    public float speed;
    public Transform trCam;

    public bool canMove = true;
    public bool isMoving = false;


    private void Start()
    {
        if(photonView.IsMine)
        {
            trCam.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if(!photonView.IsMine)
        {
            return;
        }

        if (!canMove)
        {
            return;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");



        Vector3 dir = new Vector3(h, 0, v).normalized;

        transform.position += dir * speed * Time.deltaTime;

        trCam.transform.position = transform.position + new Vector3(0, 2.5f, -2.5f);


        if (Input.GetKeyDown(KeyCode.A)) {
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
