using Photon.Pun;
using UnityEngine;

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

    private void Start()
    {
        // FrameMain, Memo�� Ȯ���� �� �ֵ���
        maskMain = (1 << LayerMask.NameToLayer("FrameMain")) + (1 << LayerMask.NameToLayer("Memo"));
    }

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
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, length, maskMain))
                {
                    Debug.Log(hit.transform.name);
                    //hit.transform.GetComponent<Renderer>().material.color = Color.red;

                    ObjSetting obj = hit.transform.GetComponent<ObjSetting>();

                    //�ٹ���忡���� �ȵǰ� �ؾ��� /�� �ٹ��� true �϶� ����ǰ�
                    if (PhotoManager.instance.isCustomMode == false && obj != null && obj.isinPhoto == true)
                    {
                        print("�ٹ��Խ���Ȱ��ȭ");
                        //ó���� �ٹ� �Խ��� ��ȸ
                        if (obj.isPhotoZoom == false)
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
                            PhotoManager.instance.OnPhotoPopupSet();

                            int objectID = obj.GetComponentInChildren<PhotonView>().ViewID;
                            photonView.RPC("PhotoPopup", RpcTarget.All, objectID);
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
    void PhotoPopup(int objectID)
    {
        /*int viewID = (int)data[0];
        GameObject obj = PhotonView.Find(viewID).gameObject;*/

        //PhotoManager.instance.OnPhotoPopup(sendObj.gameObject);
        GameObject obj = PhotonView.Find(objectID).gameObject;
        print("RPC" + obj.name);
        PhotoManager.instance.OnPhotoPopup(obj);
    }
}
