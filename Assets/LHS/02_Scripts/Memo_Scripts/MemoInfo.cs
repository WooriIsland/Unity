using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MemoInfo : MonoBehaviour
{
    //�ð�
    [SerializeField]
    private TextMeshProUGUI timeText;
    //summar
    [SerializeField]
    private TextMeshProUGUI infoText;
    //�г���
    [SerializeField]
    private TextMeshProUGUI nickNameText;
    //�̹���
    [SerializeField]
    private UnityEngine.UI.Image downloadImage;

    //EditObj
    [SerializeField]
    //private GameObject obj;

    void Start()
    {
        
    }
    void Update()
    {
        
    }

    public void SetTextInfo(string time, string info, string nickName, string character)
    {
        timeText.text = time;
        infoText.text = info;
        nickNameText.text = nickName;

        //���� ������ �ִ� ��
        //character �̹��� ���Ƴ���� ��
    }
}
