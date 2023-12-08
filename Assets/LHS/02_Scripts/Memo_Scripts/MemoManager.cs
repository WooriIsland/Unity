using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MemoManager : MonoBehaviour
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

    //�޸� ����
    private void OnSave()
    {
        //�������ִ� ��� �޸��� ������ ����
        //¦�� or Ȧ���� ���� ������ �ٸ��ɷ� ��������
        MemoInfo[] MemoAll = memoContent.GetComponentsInChildren<MemoInfo>();

        int num = MemoAll.Length;
        print("�޸� ����" +num);
        
        //¦��
        if(num%2 == 0)
        {
            //������ ���� -> ���溯��
            //���� ��ܾ� �� (���� ���� ĳ���� ����, �г���, ���糯¥, ����)
            memo = Instantiate(memoOne, memoContent);

            
        }

        //Ȧ��
        else
        {
            memo =  Instantiate(memoTwo, memoContent);
        }

        //�ֽż�
        memo.transform.SetSiblingIndex(0);
        //��鸮�� ȿ��
        memo.GetComponentInChildren<PhotoClick>().ClickAction();
        print(timeText.text + infoText.text + InfoManager.Instance.NickName + InfoManager.Instance.Character);
        //�����Ǵ� �޸� ���� �ؾ���
        memo.SetTextInfo(timeText.text, infoText.text, InfoManager.Instance.NickName, InfoManager.Instance.Character);
        
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
}
