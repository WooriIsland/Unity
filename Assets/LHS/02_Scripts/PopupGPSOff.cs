using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupGPSOff : BasePopUp
{
    public GameObject goCustomSet;
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

        goCustomSet.SetActive(true);

    }




}
