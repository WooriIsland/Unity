using System;
using System.Collections;
using System.Collections.Generic;
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

    private void Update()
    {
        // -> ���� ���� ���� �����ؾ���
        if (Input.GetMouseButtonUp(0))
        {
            //Ŭ���� ���̶�� ����
            OnClicked?.Invoke();
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
        => EventSystem.current.IsPointerOverGameObject();

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

    public void CamChangeOn()
    {
        if (sceneCamera == null || resetCamra == null)
        {
            OnCamSetting();
        }

        print("ī�޶� ��������");
        sceneCamera.gameObject.SetActive(true);
        resetCamra.gameObject.SetActive(false);
        
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
    }

    List<GameObject> offPlayer;

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
            }

            else
            {
                offPlayer.Add(go);
            }
        }
    }
}
