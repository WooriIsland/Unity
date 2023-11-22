using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupSetting : BasePopup
{
    int btnClickIdx;
    public BasePopup stage2;
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void OnClose()
    {
        if(btnClickIdx == 1)
        {
            stage2.prevPopup = this;
            stage2.OpenAction();
        }

        btnClickIdx = 0;

        base.OnClose();
    }

    public void OnClickGallery()
    {
        btnClickIdx = 1;
        CloseAction();
    }

    public void OnClickCamera()
    {
        btnClickIdx = 2;

    }

    public void OnClickOut()
    {
        btnClickIdx = 3;
    }
}
