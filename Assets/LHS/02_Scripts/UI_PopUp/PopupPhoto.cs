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

    //닫았을때마다 각자 애니메이션이 다르기 때문에 
    public override void OnClose()
    {
        base.OnClose();

        PhotoManager.instance.noPicture.SetActive(false);
        PhotoManager.instance.OnDestroyPhoto(true);
    }
}
