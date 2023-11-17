using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using TMPro;
using System;

public class PlayerMove : MonoBehaviourPun
{
    // ���̽�ƽ
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private float moveSpeed;
    private float horizontal, vertical;

    [SerializeField] private float speed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private Transform player;
    [SerializeField] private Transform trCam;

    public Vector3 offSet;
    public float rotationX, rotationY, rotationZ;

    public bool canMove = true;
    public bool isMoving = false;

    public Animator[] animator;

    public GameObject PlayerRig;
    public CharacterController cc;

    private GameObject go;

    // �߷�
    float gravity = -9.8f;
    private Vector3 velocity;

    private void Start()
    {


        // �� �÷��̾� �϶��� ī�޶� �Ҵ�.
        if (photonView.IsMine)
        {
            trCam.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (go == null)
        {
            go = GameObject.FindGameObjectWithTag("Joystick");
            print(go.name);
            joystick = go.GetComponent<FixedJoystick>();
        }

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

        // �̵�
        cc.Move(dir * speed * Time.deltaTime);

        // �߷� ����
        velocity.y += gravity * Time.deltaTime;

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
        trCam.position = player.transform.position + offSet;
        trCam.rotation = Quaternion.Euler(rotationX, rotationY, rotationZ);
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
        trCam.position = player.transform.position + offSet;
        trCam.rotation = Quaternion.Euler(rotationX, rotationY, rotationZ);

        // �߷� ����
        velocity.y += gravity * Time.deltaTime;

        // ���� �̵�
        //cc.Move(velocity * Time.deltaTime);
    }
}
