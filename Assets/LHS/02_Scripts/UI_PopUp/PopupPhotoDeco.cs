using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupPhotoDeco : BasePopup
{
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

        PhotoManager.instance.noPictureFrame.SetActive(false);
        PhotoManager.instance.OnDestroyPhoto(false);
    }
}
