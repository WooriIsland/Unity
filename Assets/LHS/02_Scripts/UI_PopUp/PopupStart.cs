using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�˾��� ���۽�ƼUI ���� ���� �� �ְ�
public class PopupStart : BasePopup
{
    public BaseAlpha back;

    private void Awake()
    {
        back.OpenAlpha();
        this.OpenAction();
    }

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

        back.CloseAlpha();
    }
}
