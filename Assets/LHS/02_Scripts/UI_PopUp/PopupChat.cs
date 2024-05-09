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

    //닫았을때마다 각자 애니메이션이 다르기 때문에 
    public override void OnClose()
    {
        base.OnClose();

        Managers.Chat.OnClickChatBtn();
    }
}
