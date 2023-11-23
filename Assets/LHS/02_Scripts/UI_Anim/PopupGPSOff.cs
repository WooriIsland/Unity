using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupGPSOff : BasePopup
{
    [Header("OnClose 실행시 작동")]
    public GameObject goCustomSet;

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

        if(goCustomSet != null)
        {
            //꺼졌을 때 실행되어야 하는 액션
            goCustomSet.SetActive(true);
        }
    }
}
