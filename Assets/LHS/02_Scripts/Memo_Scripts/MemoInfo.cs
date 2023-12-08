using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        //���� ������ �ִ� ��
        //character �̹��� ���Ƴ���� ��
        ImageSet(characteraName);
    }

   private void ImageSet(string name)
    {
         Texture2D picture = Resources.Load<Texture2D>("PlayerSelectMemberImg/" + name);

        if (picture != null) UserImage.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));
    }
}
