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

    //�ݾ��������� ���� �ִϸ��̼��� �ٸ��� ������ 
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
