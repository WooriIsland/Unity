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

    public bool canMove = true;
    public bool isMoving = false;

    public Animator[] animator;

    public GameObject PlayerRig;
    CharacterController cc;

    // �߷�
    float gravity = -9.8f;
    private Vector3 velocity;

    // ��ġ �̵�
    Vector3 targetPosition;


    private void Start()
    {
        // �� �÷��̾� �϶��� ī�޶� �Ҵ�.
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

        if ((Input.touchCount > 0) )
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                targetPosition = Camera.main.ScreenToWorldPoint(touch.position);
                targetPosition.z = transform.position.z;
                isMoving = true;
            }
        }

        if(isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);

            // ��ǥ ��ġ�� �����ϸ� �̵� ����
            if (transform.position == targetPosition)
            {
                isMoving = false;
            }
        }

#if PC
        // Ű �Է� �� ���� ����
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
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
        for (int i = 0; i < animator.Length; i++) 
        {
            if(animator[i].gameObject.activeSelf == false)
            {
                continue;
            }
            animator[i].SetFloat("MoveSpeed", speed);
        }

        trCam.position = player.transform.position + new Vector3(0, 1.5f, -3f);
        trCam.rotation = Quaternion.Euler(22f, 0, 0);

        // �߷� ����
        velocity.y += gravity * Time.deltaTime;

        // ���� �̵�
        cc.Move(velocity * Time.deltaTime);
    }
#endif
}
