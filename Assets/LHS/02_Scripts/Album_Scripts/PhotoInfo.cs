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

    //�ٹ� text ����
    public void SetTextInfo(string time, string info, Texture2D photo)
    {
        timeText.text = time;
        infoText.text = info;

        SetImage(photo);
    }

    //�ٹ� �̹��� ����
    void SetImage(Texture2D photo)
    {
        PhotoAreaScript Area = GetComponent<PhotoAreaScript>();

        //�� ������ �̹����� �����ؾ���
        //picture = Resources.Load<Texture2D>("ETC/");
        picture = photo;

        // ����� �̹����� ��ü�ؼ� �ִ´� �̰� �� PC������ �ϸ� ��!
        if (picture != null) Area.UserImage.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));
    }
}
