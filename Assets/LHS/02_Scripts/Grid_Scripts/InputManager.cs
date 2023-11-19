using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

//���콺�� ��ġ ����
public class InputManager : MonoBehaviour
{
    //ī�޶� - ��ũ�� ��ǥ
    [SerializeField]
    private Camera sceneCamera;
    [SerializeField]
    private Camera resetCamra;

    //������ ��ġ
    private Vector3 lastPosition;

    //��ġ ���̾� ����ũ (�׸���� ����Ϸ��� ��鸸 ����)
    [SerializeField]
    private LayerMask placementLayermask;

    //�ý��� ���̺귯�� ��� , Ŭ���ϸ� ���� ���� �� �ϳ� �� �߰��Ͽ� Escape Ŭ�� ��ġ��� ����
    public event Action OnClicked, OnExit;

    //--- ��ȯ �ڵ� ���� �����ؾ���
    // ��� �ÿ��̾�
    public GameObject[] players;

    private bool state = true;

    //���� �����ؾ���
    public PlacementSystem placementSystem;

    bool bbb = true;
    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
#if UNITY_EDITOR
            if (EventSystem.current.IsPointerOverGameObject() == true)
#else
            if(EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) == true)
#endif
            {
                //print("ui ���� �ö�����");
                bbb = true;
            }
            else
            {
                //print("ui ����");
                bbb = false;
            }
        }
        // -> ���� ���� ���� �����ؾ���
        if (Input.GetMouseButtonUp(0))
        {
            if (bbb == false)
            {
                //Ŭ���� ���̶�� ����
                OnClicked?.Invoke();

            }
            bbb = true;

        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnExit?.Invoke();
        }
    }

    //UI�� ��ȣ�ۿ��� �� Ŭ���� ����
    //using UnityEngine.EventSystems; �ʿ�
    //�����Ͱ� UI��ü ���� ������ true �Ǵ� false�� ��ȯ
    public bool IsPointerOverUI()
    {
#if ANDORID
        return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
#else
        return EventSystem.current.IsPointerOverGameObject();
#endif
    }

    //���콺 Ŭ���� ���� ����
    public Vector3 GetSelectedMapPosition()
    {
        //���콺 ��ġ
        Vector3 mousePos = Input.mousePosition;
        //ī�޶󿡼� ���������� ���� ��ü�� ������ �� ����
        //nearClipPlane �ش� ī�޶��� ��ġ���� �������� ������ �ּ� ��ġ������ �Ÿ� (ī�޶� ������Ʈ���� near �� ����)
        mousePos.z = sceneCamera.nearClipPlane;

        //������ ��ġ ����
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100, placementLayermask))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
    }

    // ī�޶� ���� ����
    public void CamChangeOn()
    {
        if (sceneCamera == null || resetCamra == null || playerObjs == null)
        {
            OnCamSetting();
        }

        print("ī�޶� ��������");
        sceneCamera.gameObject.SetActive(true);
        resetCamra.gameObject.SetActive(false);

        //���� �ִϸ��̼� �ִ� ���ӿ�����Ʈ�� ������
        foreach (GameObject player in playerObjs)
        {
            player.SetActive(false);
        }

        foreach (GameObject offgo in offPlayer)
        {
            print("���� �ƴ� ĳ���͵��� �� ������");
            offgo.SetActive(false);
        }
    }

    //�ٽ� �ݱ� ������ ������ �ϱ�
    public void CamChagneOff()
    {
        sceneCamera.gameObject.SetActive(false);
        resetCamra.gameObject.SetActive(true);

        //������
        placementSystem.StopPlacement();
        
        foreach (GameObject player in playerObjs)
        {
            player.SetActive(true);
        }

        foreach (GameObject offgo in offPlayer)
        {
            print("���� �ƴ� ĳ���͵��� �� ������");
            offgo.SetActive(true);
        }
    }

    //���� ĳ���� ������
    List<GameObject> offPlayer;

    //ĳ���� ��� ����
    List<GameObject> playerObjs;

    // �ְ� �� ���ӿ�����Ʈ ī�޶�
    public void OnCamSetting()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        offPlayer = new List<GameObject>();

        foreach (GameObject go in players)
        {
            PlayerManager playerMange = go.GetComponent<PlayerManager>();

            if(playerMange.isMine == true)
            {
                sceneCamera = playerMange.roomCam;
                resetCamra = playerMange.camera;

                //Animator anim = playerMange.gameObject.GetComponentInChildren<Animator>();
                //print(anim.name);
                
                Animator[] animList = playerMange.gameObject.GetComponentsInChildren<Animator>();

                foreach(Animator anim in animList)
                {
                    playerObjs.Add(anim.gameObject);
                }
            }

            else
            {
                offPlayer.Add(go);
            }
        }
    }
}
