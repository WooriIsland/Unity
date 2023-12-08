using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MemoInfo : MonoBehaviour
{
    //시간
    [SerializeField]
    private TextMeshProUGUI timeText;
    //summar
    [SerializeField]
    private TextMeshProUGUI infoText;
    //닉네임
    [SerializeField]
    private TextMeshProUGUI nickNameText;
    //이미지
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

        //내가 가지고 있는 섬
        //character 이미지 갈아끼우면 됨
    }
}
