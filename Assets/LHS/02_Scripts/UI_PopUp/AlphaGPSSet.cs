using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaGPSSet : BaseAlpha 
{
    //[Header("OnClose ����� �۵�")]
    //public GameObject OnUI;

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
        base.OnClose();

        //�������� ����Ǿ���ϴ� ��
        //OnUI.SetActive(true);
    }
}
