using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupPhoto : BasePopup
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

        PhotoManager.instance.OnPhotoInquiry(true);
    }

    //�ݾ��������� ���� �ִϸ��̼��� �ٸ��� ������ 
    public override void OnClose()
    {
        base.OnClose();

        PhotoManager.instance.noPicture.SetActive(false);
        PhotoManager.instance.OnDestroyPhoto(true);
    }
}
