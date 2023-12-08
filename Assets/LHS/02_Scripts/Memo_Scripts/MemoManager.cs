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

    [Header("메모UI")]
    [SerializeField]
    private PopupPhotoED memoUI;
    [SerializeField]
    private BaseAlpha backUI;
    [SerializeField]
    private Button btnOut;

    [Header("메모프리팹")]
    [SerializeField]
    private MemoInfo memoOne;
    [SerializeField]
    private MemoInfo memoTwo;

    [Header("메모등록UI")]
    [SerializeField]
    private PopupPhotoED memoAddPanel;
    [SerializeField]
    private Button btnBack;
    [SerializeField]
    private Button btnSave;

    [Header("메모등록시")]
    [SerializeField]
    private TMP_InputField infoText;
    [SerializeField]
    private TextMeshProUGUI timeText;

    [Header("메모등록 위치")]
    [SerializeField]
    private Transform memoContent;

    //열려있을때는 실행안하기 위한 조건문
    public bool isMemo = false;

    //생성된 메모
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
        //오늘 날짜로
        timeText.text = DateTime.Now.ToString("yyyy.MM.dd");
    }

    //메모 생성
    private void OnSave()
    {
        //가지고있는 모든 메모의 갯수를 세고
        //짝수 or 홀수에 따라 프리팹 다른걸로 가져오기
        MemoInfo[] MemoAll = memoContent.GetComponentsInChildren<MemoInfo>();

        int num = MemoAll.Length;
        print("메모 갯수" +num);
        
        //짝수
        if(num%2 == 0)
        {
            //프리팹 생성 -> 포톤변경
            //내용 담겨야 함 (현재 나의 캐릭터 정보, 닉네임, 현재날짜, 내용)
            memo = Instantiate(memoOne, memoContent);

            
        }

        //홀수
        else
        {
            memo =  Instantiate(memoTwo, memoContent);
        }

        //최신순
        memo.transform.SetSiblingIndex(0);
        //흔들리는 효과
        memo.GetComponentInChildren<PhotoClick>().ClickAction();
        print(timeText.text + infoText.text + InfoManager.Instance.NickName + InfoManager.Instance.Character);
        //생성되는 메모 셋팅 해야함
        memo.SetTextInfo(timeText.text, infoText.text, InfoManager.Instance.NickName, InfoManager.Instance.Character);
        
        OnBack();
    }

    private void OnBack()
    {
        memoAddPanel.CloseAction();
        //텍스트 다 지워져야함
        infoText.text = null;
    }

    private void OnMemoOut()
    {
        isMemo = false;
        memoUI.CloseAction();
    }

    //메모패널 열기
    public void OnMemoPanel()
    {
        isMemo = true; 
        memoUI.OpenAction();
        backUI.OpenAlpha();
    }
}
