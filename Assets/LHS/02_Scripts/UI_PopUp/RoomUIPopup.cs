using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomUIPopup : BasePopup
{
    public BaseAlpha back;
    public bool isUI;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    //닫았을때마다 각자 애니메이션이 다르기 때문에 
    public override void OnClose()
    {
        base.OnClose();

        if(isUI == true)
        {
            back.CloseAlpha();
            GPSManager.instance.planeUI.OpenAction();
        }
    }
}
