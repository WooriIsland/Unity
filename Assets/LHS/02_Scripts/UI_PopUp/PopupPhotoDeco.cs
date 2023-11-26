using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupPhotoDeco : BasePopup
{
    public BaseAlpha alpha;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void OnOpen()
    {
        base.OnOpen();
    }

    //�ݾ��������� ���� �ִϸ��̼��� �ٸ��� ������ 
    public override void OnClose()
    {
        base.OnClose();
        print("���Ĳ�������");
        alpha.CloseAlpha();

        PhotoManager.instance.noPictureFrame.SetActive(false);
        PhotoManager.instance.OnDestroyPhoto(false);
    }
}
