using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupChat : BasePopup
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

        Managers.Chat.OnClickChatBtn();
    }

    //�ݾ��������� ���� �ִϸ��̼��� �ٸ��� ������ 
    public override void OnClose()
    {
        base.OnClose();

        Managers.Chat.OnClickChatBtn();
    }
}
