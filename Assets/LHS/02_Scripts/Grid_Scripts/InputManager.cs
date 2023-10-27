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

    //������ ��ġ
    private Vector3 lastPosition;

    //��ġ ���̾� ����ũ (�׸���� ����Ϸ��� ��鸸 ����)
    [SerializeField]
    private LayerMask placementLayermask;

    //�ý��� ���̺귯�� ��� , Ŭ���ϸ� ���� ���� �� �ϳ� �� �߰��Ͽ� Escape Ŭ�� ��ġ��� ����
    public event Action OnClicked, OnExit;

    public PlayerManager playerManager_BJH;

    //--- ��ȯ �ڵ� ���� �����ؾ���
    // ��� �ÿ��̾�
    public GameObject[] players;

    private bool state = true;

    // ���� ī�޶�
    public Camera camera;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
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
        players = GameObject.FindGameObjectsWithTag("Player");

        //ī�޶� ��ư�� ������ ������
        OnOff();

        //���� ī�޶� ������ 
        sceneCamera.gameObject.SetActive(true);
    }
    
    //�ٽ� �ݱ� ������ ������ �ϱ�
    public void CamChagneOff()
    {
        OnOff();

        sceneCamera.gameObject.SetActive(false);
    }


    // �÷��̾� ī�޶�� �÷��̾� ���¸� ���� �Ѵ� �Լ�
    public void OnOff()
    {

        state = !state;

        foreach (GameObject go in players)
        {
            go.SetActive(state);
        }

        //camera.gameObject.SetActive(state);
    }
}
