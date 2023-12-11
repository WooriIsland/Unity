using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//�÷��̾� ī�޶󿡼� Ray�� ���� 
//�ٹ����̾��� ������Ʈ�� �ְ� 0���� �����ٸ�
//�ٹ���ġ�� UI �߻�
//������ ���ٸ� �ٹ� ������
//�־����� ������ �ϱ�

//! ������Ʈ ��ü���� �Ǻ��ϰ� ����
public class RayCastObject : MonoBehaviourPunCallbacks
{
    public Camera cam;
    public float length = 3;

    //FrameMain / Memo ����
    public LayerMask mask;
    private LayerMask maskMain;

    // ���� ó���� UI ����� ���̾� �̸�
    private string exceptionLayerName = "IgnoreRaycast";

    public bool isCheck;

    private void Start()
    {
        // FrameMain, Memo�� Ȯ���� �� �ֵ���
        maskMain = (1 << LayerMask.NameToLayer("FrameMain")) + (1 << LayerMask.NameToLayer("Memo"));
    }

    bool uiCheck = true;

    private void Update()
    {
        //�� �϶��� ����ǰ� �ϱ�
        if (photonView.IsMine)
        {
            print("�����Ҽ��ִ� ���");

            //���� ���� ����� �� �ֵ��� �ؾ��� //�ٹ� ��� �ƴҶ���
            //�شٸ������ �� ������Ʈ�� �����ϰ� ������ �� �� ���� (���������϶� ���� ���� �� ����)


            Vector3 mousePos = Input.mousePosition;
            mousePos.z = length;

            mousePos = cam.ScreenToWorldPoint(mousePos);
            Debug.DrawRay(transform.position, mousePos - transform.position, Color.yellow);

            //��ư�� �������� �Խ����̶��
            //������ �Խ��ǿ����� ����� �� �ְ�
            if (Input.GetMouseButtonDown(0))
            {
                /*if (IsPointerOverUIObject())
                {
                    // UI ��� ���� �ִ� ��� Raycast�� �������� ����
                    return;
                }*/

                //���ٹ̴� ����϶��� ���� x
                //�ٹ� ����϶��� 
                if (PhotoManager.instance.isCustomMode || PhotoManager.instance.isPhotoMode)
                {
                    print(PhotoManager.instance.isPhotoMode);
                    return;
                }

                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, length, maskMain))
                {
                    Debug.Log(hit.transform.name);
                    //hit.transform.GetComponent<Renderer>().material.color = Color.red;

                    ObjSetting obj = hit.transform.GetComponent<ObjSetting>();
                    //�ٹ��� true������ ���� �ٸ���

                    //�ٹ���忡���� �ȵǰ� �ؾ��� /�� �ٹ��� true �϶� ����ǰ�
                    if (PhotoManager.instance.isCustomMode == false && obj != null && obj.isinPhoto == true)
                    {
                        print("�ٹ��Խ���Ȱ��ȭ");
                        //ó���� �ٹ� �Խ��� ��ȸ
                        if (obj.isPhotoZoom == false && obj.isChristmas == false)
                        {
                            print("�ٹ���ġ 1�ܰ�_1 : ����1ȸ �ٷ� ���������ϱ�");
                            obj.GetComponentInChildren<FramePhoto>().OnPhotoInquiry();
                        }

                        //���� ���� zoom ���
                        else
                        {
                            print("�ٹ���ġ 1�ܰ�_2 : �Խ��ǿ�����Ʈ ����" + obj.gameObject);

                            //PhotoManager.instance.OnPhotoPopup(obj.gameObject);

                            //���ӿ�����Ʈ ������ ����ȭ�Ͽ� �����ؾ���
                            /*object[] data = new object[] { obj.gameObject.GetComponentInChildren<PhotonView>().ViewID};
                            sendObj = obj;*/

                            //���� ���ǹ� ������ ����� �� �ְ�
                            isCheck = true;

                            PhotoManager.instance.OnPhotoPopupSet(obj.Photo, obj.isChristmas, isCheck);

                            //������ ����� �� �ְ�!
                            int objectID = obj.GetComponentInChildren<PhotonView>().ViewID;

                            //�� �г��� ������ -> �� �г����̶� �˾�â�� �����̶� ���ؼ� �������� �ٲ� �� �ְ� ����
                            photonView.RPC("PhotoPopup", RpcTarget.All, objectID, InfoManager.Instance.NickName);
                        }
                    }

                    //�޸�
                    MemoSetting memoSet = hit.transform.GetComponent<MemoSetting>();

                    if (MemoManager.instance.isMemo == false && memoSet != null && memoSet.isinMemo == true)
                    {
                        print("�޸���Ȱ��ȭ");
                        MemoManager.instance.OnMemoPanel();
                    }
                }
            }
        }

        else
        {
            print("�����Ҽ����� ���");
        }
    }

    //���ʿ���� �� ������ ?
    private ObjSetting sendObj;

    [PunRPC]
    void PhotoPopup(int objectID, string nickName)
    {
        /*int viewID = (int)data[0];
        GameObject obj = PhotonView.Find(viewID).gameObject;*/
        //PhotoManager.instance.OnPhotoPopup(sendObj.gameObject);

        GameObject obj = PhotonView.Find(objectID).gameObject;
        print("RPC" + obj.name);

        PhotoManager.instance.OnPhotoPopup(obj, nickName);
    }
    
    // ���� Ȯ���� ����
    private bool IsPointerOverUIObject()
    {
        // ���콺 ��ġ�� UI ��Ұ� �ִ��� ���θ� �˻�
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        // UI ��� �߿��� ���� ó���� ���̾ ���� ��Ұ� �ִ��� �˻�
        foreach (var result in results)
        {
            if (result.gameObject.layer == LayerMask.NameToLayer(exceptionLayerName))
            {
                return true; // ���� ó���� ���̾ ���� UI ��Ұ� ������ true ��ȯ
            }
        }

        return results.Count > 0;
    }
}
