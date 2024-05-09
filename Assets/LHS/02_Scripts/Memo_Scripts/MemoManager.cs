using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MemoManager : MonoBehaviourPunCallbacks
{
    public static MemoManager instance;

    [Header("�޸�UI")]
    [SerializeField]
    private PopupPhotoED memoUI;
    [SerializeField]
    private BaseAlpha backUI;
    [SerializeField]
    private Button btnOut;

    [Header("�޸�������")]
    [SerializeField]
    private MemoInfo memoOne;
    [SerializeField]
    private MemoInfo memoTwo;

    [Header("�޸���UI")]
    [SerializeField]
    private PopupPhotoED memoAddPanel;
    [SerializeField]
    private Button btnBack;
    [SerializeField]
    private Button btnSave;

    [Header("�޸��Ͻ�")]
    [SerializeField]
    private TMP_InputField infoText;
    [SerializeField]
    private TextMeshProUGUI timeText;

    [Header("�޸��� ��ġ")]
    [SerializeField]
    private Transform memoContent;

    //������������ ������ϱ� ���� ���ǹ�
    public bool isMemo = false;

    //������ �޸�
    private MemoInfo memo;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        btnBack.onClick.AddListener(() => OnBack());
        btnSave.onClick.AddListener(() => OnSave());
        btnOut.onClick.AddListener(() => OnMemoOut());
    }

    private void Update()
    {
        //���� ��¥��
        timeText.text = DateTime.Now.ToString("yyyy.MM.dd");
    }

    GameObject memoMain;
    int check;

    //�޸� ����
    private void OnSave()
    {
        //�������ִ� ��� �޸��� ������ ����
        //¦�� or Ȧ���� ���� ������ �ٸ��ɷ� ��������
        MemoInfo[] MemoAll = memoContent.GetComponentsInChildren<MemoInfo>();

        int num = MemoAll.Length;
      
        print("�޸� ����" +num);

        print(timeText.text + infoText.text + Managers.Info.NickName + Managers.Info.Character);

        //¦��Ȧ���Ǻ�
        if (num % 2 == 0)
        {
            check = 1;
        }

        else
        {
            check = 2;
        }

        photonView.RPC("AddMemo", RpcTarget.AllBuffered, timeText.text, infoText.text, Managers.Info.NickName, Managers.Info.Character, Managers.Info.isIslandUniqueNumber, check);

        //¦��
        /*if (num % 2 == 0)
        {
            //������ ���� -> ���溯��
            //���� ��ܾ� �� (���� ���� ĳ���� ����, �г���, ���糯¥, ����)
            //memo = Instantiate(memoOne, memoContent);

            // pun ���� �߻�?
            *//*GameObject memoMain = PhotonNetwork.Instantiate(memoOne.name, Vector3.zero, Quaternion.identity);
            memo = memoMain.GetComponent<MemoInfo>();
            memo.transform.SetParent(memoContent);
            memo.transform.localScale = new Vector3(1, 1, 1);*//*

            photonView.RPC("AddMemo", RpcTarget.All);
        }

        //Ȧ��
        else
        {
           // memo =  Instantiate(memoTwo, memoContent);

            GameObject memoMain = PhotonNetwork.Instantiate(memoTwo.name, Vector3.zero, Quaternion.identity);
            memo = memoMain.GetComponent<MemoInfo>();
            memo.transform.SetParent(memoContent);
            memo.transform.localScale = new Vector3(1, 1, 1);
        }*/

        print(timeText.text + infoText.text + Managers.Info.NickName + Managers.Info.Character);
        
        OnBack();
    }

    private void OnBack()
    {
        memoAddPanel.CloseAction();
        //�ؽ�Ʈ �� ����������
        infoText.text = null;
    }

    private void OnMemoOut()
    {
        isMemo = false;
        memoUI.CloseAction();
    }

    //�޸��г� ����
    public void OnMemoPanel()
    {
        isMemo = true; 
        memoUI.OpenAction();
        backUI.OpenAlpha();
    }

    // ���� �濡 ���� �� -> �ѹ��� ���� �� �� �ְ� ����
    [PunRPC]
    void AddMemo(string time, string info, string nick, string character, string islandUniqueNumber, int num)
    {

        if (num == 1)
        {
            memo = Instantiate(memoOne, memoContent);
        }

        else
        {
            memo = Instantiate(memoTwo, memoContent);
        }

        //�ֽż�
        memo.transform.SetSiblingIndex(0);
        //��鸮�� ȿ��
        memo.GetComponentInChildren<PhotoClick>().ClickAction();

        print(timeText.text + infoText.text + Managers.Info.NickName + Managers.Info.Character);

        //������ ������ ���õǴµ�!
        //�����Ǵ� �޸� ���� �ؾ���
        memo.SetTextInfo(time, info, nick, character, islandUniqueNumber);
    }
}
