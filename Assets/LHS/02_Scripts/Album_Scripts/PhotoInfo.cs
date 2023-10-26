using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PhotoInfo : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timeText;
    [SerializeField]
    private TextMeshProUGUI infoText;

    Texture2D picture;

    //앨범 text 셋팅
    public void SetTextInfo(string time, string info, Texture2D photo)
    {
        timeText.text = time;
        infoText.text = info;

        SetImage(photo);
    }

    //앨범 이미지 셋팅
    void SetImage(Texture2D photo)
    {
        PhotoAreaScript Area = GetComponent<PhotoAreaScript>();

        //※ 저장한 이미지로 변경해야함
        //picture = Resources.Load<Texture2D>("ETC/");
        picture = photo;

        // 사용자 이미지로 대체해서 넣는다 이건 내 PC에서만 하면 됨!
        if (picture != null) Area.UserImage.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));
    }
}
