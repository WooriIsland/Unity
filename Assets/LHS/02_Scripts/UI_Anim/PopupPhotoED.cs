using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupPhotoED : BasePopup
{
    public BaseAlpha back;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    //�ݾ��������� ���� �ִϸ��̼��� �ٸ��� ������ 
    public override void OnClose()
    {
        base.OnClose();

        back.CloseAlpha();
    }
}
