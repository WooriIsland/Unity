using Photon.Pun;
using UnityEngine;

//�÷��̾� ī�޶󿡼� Ray�� ���� 
//�ٹ����̾��� ������Ʈ�� �ְ� 0���� �����ٸ�
//�ٹ���ġ�� UI �߻�
//������ ���ٸ� �ٹ� ������
//�־����� ������ �ϱ�

//! ������Ʈ ��ü���� �Ǻ��ϰ� ����
public class RayCastObject : MonoBehaviourPun
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
        //���� ���� ����� �� �ֵ��� �ؾ���

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
                        obj.GetComponentInChildren<FramePhoto>().OnPhotoInquiry();
                    }

                    //���� ���� zoom ���
                    else
                    {
                        PhotoManager.instance.OnPhotoPopup(obj.gameObject);
                    }
                }

                //�޸�
                MemoSetting memoSet = hit.transform.GetComponent<MemoSetting>();

                if(MemoManager.instance.isMemo == false && memoSet != null && memoSet.isinMemo == true)
                {
                    print("�޸���Ȱ��ȭ");
                    MemoManager.instance.OnMemoPanel();
                }
            }
        }
    }
}
