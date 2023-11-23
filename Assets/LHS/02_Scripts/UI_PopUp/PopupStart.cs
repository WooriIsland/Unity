using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//팝업과 오퍼시티UI 같이 나올 수 있게
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

    //닫았을때마다 각자 애니메이션이 다르기 때문에 
    public override void OnClose()
    {
        base.OnClose();

        back.CloseAlpha();
    }
}
