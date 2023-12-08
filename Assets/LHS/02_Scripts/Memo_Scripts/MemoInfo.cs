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

    //EditObj
    [SerializeField]
    //private GameObject obj;

    private string characteraName;

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
        characteraName = character;
        //내가 가지고 있는 섬
        //character 이미지 갈아끼우면 됨
        ImageSet(characteraName);
    }

   private void ImageSet(string name)
    {
         Texture2D picture = Resources.Load<Texture2D>("PlayerSelectMemberImg/" + name);

        if (picture != null) UserImage.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));
    }
}
