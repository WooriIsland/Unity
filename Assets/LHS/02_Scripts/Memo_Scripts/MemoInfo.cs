using System;
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

    [Header("�ĵ�Ÿ���ɿ�")]
    //�̹���
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

    //�ĵ�Ÿ�� �����ϱ� ����
    private void OnSurf()
    {
        // �� �г��Ӱ� �������� ���� �� �� �ְ�
        // -> ���� ���
        // -> ���� ���϶��� ���� ���� ����
        if(InfoManager.Instance.NickName == nickNameText.text)
        {
            return;
        }

        //����� �����ִٸ� �ٽ� �Ѿ���
        if (background.gameObject.activeSelf == true)
        {
            print("�ĵ�Ÿ�� ���ɺҰ�");

            background.CloseAlpha();
            btnSurfYes.GetComponent<BasePopup>().CloseAction();

            Invoke("Delay", 0.4f);
        }
        
        else
        {
            print("�ĵ�Ÿ�� �����ؾ���");
            btnSurfReset.gameObject.SetActive(true);
            background.gameObject.SetActive(true);

            //���� ����
            ImageSet(characteraName, UserImageCopy);
            nickNameTextCopy2.text = nickNameText.text;
            nickNameTextCopy.text = nickNameText.text + "�� ������";

            //��鸮�� �ִϸ��̼� ����
            this.GetComponent<PhotoClick>().ClickAction();
            //��� ����
            background.OpenAlpha();
            //��ư �ִϸ��̼� ����
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

        //���� ������ �ִ� ��
        //character �̹��� ���Ƴ���� ��
        ImageSet(characteraName, UserImage);
    }

   private void ImageSet(string name, Image userImage)
    {
         Texture2D picture = Resources.Load<Texture2D>("PlayerSelectMemberImg/" + name);

        if (picture != null) userImage.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));
    }
}
