using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    private Image UserImage;

    [Header("파도타기기능용")]
    //이미지
    [SerializeField]
    private Image UserImageCopy;
    [SerializeField]
    private TextMeshProUGUI nickNameTextCopy;
    [SerializeField]
    private TextMeshProUGUI nickNameTextCopy2;

    //EditObj
    [SerializeField]
    private BaseAlpha background;
    [SerializeField]
    private Button btnSurf;
    [SerializeField]
    private Button btnSurfYes;
    [SerializeField]
    private Button btnSurfReset;

    private string characteraName;
    private string islandUniqueNumber;

    void Start()
    {
        btnSurf.onClick.AddListener(() => OnSurf());
        btnSurfReset.onClick.AddListener(() => OnSurf());
    }


    void Update()
    {
        
    }

    //파도타기 실행하기 위함
    private void OnSurf()
    {
        // 내 닉네임과 같을떄만 실행 될 수 있게
        // -> 삭제 기능
        // -> 같은 섬일때는 실행 되지 않음
        if(InfoManager.Instance.NickName == nickNameText.text)
        {
            return;
        }

        //배경이 꺼져있다면 다시 켜야함
        if (background.gameObject.activeSelf == true)
        {
            print("파도타기 가능불가");

            background.CloseAlpha();
            btnSurfYes.GetComponent<BasePopup>().CloseAction();

            Invoke("Delay", 0.4f);
        }
        
        else
        {
            print("파도타기 가능해야함");
            btnSurfReset.gameObject.SetActive(true);
            background.gameObject.SetActive(true);

            //내용 셋팅
            ImageSet(characteraName, UserImageCopy);
            nickNameTextCopy2.text = nickNameText.text;
            nickNameTextCopy.text = nickNameText.text + "의 섬에도";

            //흔들리는 애니메이션 실행
            this.GetComponent<PhotoClick>().ClickAction();
            //배경 생성
            background.OpenAlpha();
            //버튼 애니메이션 실행
            btnSurfYes.GetComponent<BasePopup>().OpenAction();
        }
    }

    void Delay()
    {
        btnSurfReset.gameObject.SetActive(false);
    }

    public void SetTextInfo(string time, string info, string nickName, string character, string islandUniqueNum)
    {
        timeText.text = time;
        infoText.text = info;
        nickNameText.text = nickName;
        characteraName = character;
        islandUniqueNumber = islandUniqueNum;

        //내가 가지고 있는 섬
        //character 이미지 갈아끼우면 됨
        ImageSet(characteraName, UserImage);
    }

   private void ImageSet(string name, Image userImage)
    {
         Texture2D picture = Resources.Load<Texture2D>("PlayerSelectMemberImg/" + name);

        if (picture != null) userImage.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));
    }
}
