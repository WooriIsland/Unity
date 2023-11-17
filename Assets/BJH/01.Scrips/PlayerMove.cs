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

    // �߷�
    float gravity = -9.8f;
    private Vector3 velocity;

    // ��ġ �̵�
    private Vector3 touchStartPosition;
    private Vector3 touchEndPosition;

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



        trCam.position = player.transform.position + offSet;
        trCam.rotation = Quaternion.Euler(rotationX, 0, 0);

        // �߷� ����
        velocity.y += gravity * Time.deltaTime;

        // ���� �̵�
        cc.Move(velocity * Time.deltaTime);
#endif

        // ��ġ �̵�
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
