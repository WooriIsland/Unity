using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�˾��� ���۽�ƼUI ���� ���� �� �ְ�
public class PopupStart : BasePopup
{
    public BaseAlpha back;

    protected override void Start()
    {
        Invoke("StartSetting", 0.1f);
    }

    public void StartSetting()
    {
        back.OpenAlpha();
        this.OpenAction();
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

        back.CloseAlpha();
    }
}
