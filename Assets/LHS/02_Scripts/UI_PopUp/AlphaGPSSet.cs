using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaGPSSet : BaseAlpha 
{
    //[Header("OnClose 실행시 작동")]
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

        //꺼졌을때 샐행되어야하는 것
        //OnUI.SetActive(true);
    }
}
