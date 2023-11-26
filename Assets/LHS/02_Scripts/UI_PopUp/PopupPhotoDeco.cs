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

    //닫았을때마다 각자 애니메이션이 다르기 때문에 
    public override void OnClose()
    {
        base.OnClose();
        print("알파꺼져야함");
        alpha.CloseAlpha();

        PhotoManager.instance.noPictureFrame.SetActive(false);
        PhotoManager.instance.OnDestroyPhoto(false);
    }
}
